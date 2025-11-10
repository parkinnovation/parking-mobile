using Android.App;
using Android.Graphics;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Tesseract.Droid;

namespace Parking.Mobile.Droid.DependencyService
{
    public class OcrReader
    {
        private readonly TesseractApi _tesseract;
        private bool _isInitialized = false;
        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

        public OcrReader()
        {
            _tesseract = new TesseractApi(Application.Context, AssetsDeployment.OncePerInitialization);
        }

        public async Task<string> ReadPlateAsync(Bitmap bitmap)
        {
            await _lock.WaitAsync();
            try
            {
                if (!_isInitialized)
                {
                    Console.WriteLine("Inicializando Tesseract...");
                    await EnsureTrainedDataExists();

                    var dataPath = Application.Context.FilesDir.AbsolutePath;
                    Console.WriteLine($"Caminho Tesseract: {dataPath}");

                    bool success = await _tesseract.Init(dataPath, "eng");
                    if (!success)
                    {
                        Console.WriteLine("❌ Falha ao inicializar Tesseract.");
                        return null;
                    }

                    ConfigureTesseractForPlates();
                    _isInitialized = true;
                    Console.WriteLine("✅ Tesseract inicializado.");
                }

                // 🔥 TESTAR MÚLTIPLAS ABORDAGENS DE PRÉ-PROCESSAMENTO
                var results = new System.Collections.Generic.List<string>();

                // Abordagem 1: Imagem original
                var result1 = await ProcessWithTesseract(bitmap, "Original");
                if (!string.IsNullOrEmpty(result1)) results.Add(result1);

                // Abordagem 2: Escala de cinza + alto contraste
                var processed2 = PreprocessApproach1(bitmap);
                var result2 = await ProcessWithTesseract(processed2, "Approach1");
                if (!string.IsNullOrEmpty(result2)) results.Add(result2);
                processed2?.Recycle();

                // Abordagem 3: Threshold agressivo
                var processed3 = PreprocessApproach2(bitmap);
                var result3 = await ProcessWithTesseract(processed3, "Approach2");
                if (!string.IsNullOrEmpty(result3)) results.Add(result3);
                processed3?.Recycle();

                // Abordagem 4: Inversão de cores (placas escuras)
                var processed4 = PreprocessApproach3(bitmap);
                var result4 = await ProcessWithTesseract(processed4, "Approach3");
                if (!string.IsNullOrEmpty(result4)) results.Add(result4);
                processed4?.Recycle();

                // Retornar a placa mais comum (consenso)
                if (results.Count > 0)
                {
                    var mostCommon = results.GroupBy(x => x)
                                           .OrderByDescending(g => g.Count())
                                           .First().Key;

                    Console.WriteLine($"✅ PLACA FINAL (consenso): {mostCommon}");
                    Console.WriteLine($"   Detectada em {results.Count(x => x == mostCommon)}/{results.Count} abordagens");
                    return mostCommon;
                }

                Console.WriteLine("⚠️ Nenhuma placa detectada em nenhuma abordagem.");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro no OCR: {ex.Message}\n{ex.StackTrace}");
                return null;
            }
            finally
            {
                _lock.Release();
            }
        }

        private async Task<string> ProcessWithTesseract(Bitmap bitmap, string approach)
        {
            try
            {
                using var stream = new MemoryStream();
                bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
                var bytes = stream.ToArray();

                await _tesseract.SetImage(bytes);
                var text = _tesseract.Text?.Trim()?.ToUpper() ?? string.Empty;

                Console.WriteLine($"\n--- {approach} ---");
                Console.WriteLine($"OCR Raw: {text.Replace("\n", " | ")}");

                var plate = ExtractPlateFromText(text);
                if (!string.IsNullOrEmpty(plate))
                {
                    Console.WriteLine($"✅ Placa detectada: {plate}");
                }
                else
                {
                    Console.WriteLine($"❌ Nenhuma placa válida");
                }

                return plate;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro em {approach}: {ex.Message}");
                return null;
            }
        }

        private void ConfigureTesseractForPlates()
        {
            try
            {
                // Configurações otimizadas
                _tesseract.SetVariable("tessedit_char_whitelist", "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789");
                _tesseract.SetVariable("tessedit_pageseg_mode", "6"); // Uniform block of text
                _tesseract.SetVariable("tessedit_ocr_engine_mode", "1"); // LSTM

                Console.WriteLine("✅ Tesseract configurado");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Erro ao configurar: {ex.Message}");
            }
        }

        // ==================== ABORDAGENS DE PRÉ-PROCESSAMENTO ====================

        /// <summary>
        /// Abordagem 1: Escala de cinza + Alto contraste + Threshold médio
        /// </summary>
        private Bitmap PreprocessApproach1(Bitmap original)
        {
            try
            {
                var resized = ResizeBitmap(original, 1000);
                var grayscale = ToGrayscale(resized);
                var contrast = AdjustBrightnessContrast(grayscale, 0, 50); // +50 contraste
                var threshold = ApplyThreshold(contrast, 127);

                if (resized != original) resized.Recycle();
                grayscale.Recycle();
                contrast.Recycle();

                return threshold;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro Approach1: {ex.Message}");
                return original;
            }
        }

        /// <summary>
        /// Abordagem 2: Alto contraste + Threshold agressivo
        /// </summary>
        private Bitmap PreprocessApproach2(Bitmap original)
        {
            try
            {
                var resized = ResizeBitmap(original, 1000);
                var grayscale = ToGrayscale(resized);
                var contrast = AdjustBrightnessContrast(grayscale, 30, 80); // +30 brilho, +80 contraste
                var threshold = ApplyThreshold(contrast, 150); // Threshold alto

                if (resized != original) resized.Recycle();
                grayscale.Recycle();
                contrast.Recycle();

                return threshold;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro Approach2: {ex.Message}");
                return original;
            }
        }

        /// <summary>
        /// Abordagem 3: Para placas escuras - Inversão de cores
        /// </summary>
        private Bitmap PreprocessApproach3(Bitmap original)
        {
            try
            {
                var resized = ResizeBitmap(original, 1000);
                var grayscale = ToGrayscale(resized);
                var inverted = InvertColors(grayscale);
                var contrast = AdjustBrightnessContrast(inverted, 0, 50);
                var threshold = ApplyThreshold(contrast, 127);

                if (resized != original) resized.Recycle();
                grayscale.Recycle();
                inverted.Recycle();
                contrast.Recycle();

                return threshold;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro Approach3: {ex.Message}");
                return original;
            }
        }

        // ==================== FUNÇÕES DE PROCESSAMENTO ====================

        private Bitmap ResizeBitmap(Bitmap original, int maxSize)
        {
            if (original.Width <= maxSize && original.Height <= maxSize)
                return original;

            float scale = Math.Min((float)maxSize / original.Width, (float)maxSize / original.Height);
            int newWidth = (int)(original.Width * scale);
            int newHeight = (int)(original.Height * scale);

            return Bitmap.CreateScaledBitmap(original, newWidth, newHeight, true);
        }

        private Bitmap ToGrayscale(Bitmap original)
        {
            Bitmap grayscale = Bitmap.CreateBitmap(original.Width, original.Height, Bitmap.Config.Argb8888);
            Canvas canvas = new Canvas(grayscale);
            Paint paint = new Paint();

            ColorMatrix colorMatrix = new ColorMatrix();
            colorMatrix.SetSaturation(0);

            paint.SetColorFilter(new ColorMatrixColorFilter(colorMatrix));
            canvas.DrawBitmap(original, 0, 0, paint);

            return grayscale;
        }

        private Bitmap AdjustBrightnessContrast(Bitmap original, int brightness, int contrast)
        {
            Bitmap adjusted = Bitmap.CreateBitmap(original.Width, original.Height, Bitmap.Config.Argb8888);
            Canvas canvas = new Canvas(adjusted);
            Paint paint = new Paint();

            float contrastFactor = (259f * (contrast + 255f)) / (255f * (259f - contrast));
            float brightnessFactor = brightness;

            ColorMatrix cm = new ColorMatrix(new float[]
            {
                contrastFactor, 0, 0, 0, brightnessFactor,
                0, contrastFactor, 0, 0, brightnessFactor,
                0, 0, contrastFactor, 0, brightnessFactor,
                0, 0, 0, 1, 0
            });

            paint.SetColorFilter(new ColorMatrixColorFilter(cm));
            canvas.DrawBitmap(original, 0, 0, paint);

            return adjusted;
        }

        private Bitmap ApplyThreshold(Bitmap original, int thresholdValue)
        {
            int width = original.Width;
            int height = original.Height;
            int[] pixels = new int[width * height];

            original.GetPixels(pixels, 0, width, 0, 0, width, height);

            for (int i = 0; i < pixels.Length; i++)
            {
                int pixel = pixels[i];
                int gray = Color.GetRedComponent(pixel);
                pixels[i] = gray > thresholdValue ? Color.White : Color.Black;
            }

            Bitmap result = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
            result.SetPixels(pixels, 0, width, 0, 0, width, height);

            return result;
        }

        private Bitmap InvertColors(Bitmap original)
        {
            Bitmap inverted = Bitmap.CreateBitmap(original.Width, original.Height, Bitmap.Config.Argb8888);
            Canvas canvas = new Canvas(inverted);
            Paint paint = new Paint();

            ColorMatrix cm = new ColorMatrix(new float[]
            {
                -1, 0, 0, 0, 255,
                0, -1, 0, 0, 255,
                0, 0, -1, 0, 255,
                0, 0, 0, 1, 0
            });

            paint.SetColorFilter(new ColorMatrixColorFilter(cm));
            canvas.DrawBitmap(original, 0, 0, paint);

            return inverted;
        }

        // ==================== EXTRAÇÃO DE PLACA ====================

        private string ExtractPlateFromText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;

            // Limpar texto
            text = text.ToUpper()
                      .Replace(" ", "")
                      .Replace("-", "")
                      .Replace(".", "")
                      .Replace(",", "")
                      .Replace("|", "1")
                      .Replace("!", "1");

            Console.WriteLine($"Texto limpo: {text}");

            // Padrões de placa
            var patterns = new[]
            {
                @"[A-Z]{3}\d[A-Z]\d{2}",  // Mercosul: ABC1D23
                @"[A-Z]{3}\d{4}"           // Antiga: ABC1234
            };

            // Tentar encontrar padrão direto
            foreach (var pattern in patterns)
            {
                var match = Regex.Match(text, pattern);
                if (match.Success)
                {
                    var plate = match.Value;
                    Console.WriteLine($"Match direto: {plate}");
                    return FormatPlate(plate);
                }
            }

            // Tentar linha por linha
            var lines = text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var cleanLine = Regex.Replace(line, @"[^A-Z0-9]", "");
                Console.WriteLine($"Linha limpa: {cleanLine}");

                if (cleanLine.Length >= 7 && cleanLine.Length <= 10)
                {
                    foreach (var pattern in patterns)
                    {
                        var match = Regex.Match(cleanLine, pattern);
                        if (match.Success)
                        {
                            var plate = match.Value;
                            Console.WriteLine($"Match em linha: {plate}");
                            return FormatPlate(plate);
                        }
                    }
                }
            }

            // Busca em texto contínuo
            var continuous = Regex.Replace(text, @"[^A-Z0-9]", "");
            Console.WriteLine($"Texto contínuo: {continuous}");

            if (continuous.Length >= 7)
            {
                for (int i = 0; i <= continuous.Length - 7; i++)
                {
                    string segment = continuous.Substring(i, Math.Min(7, continuous.Length - i));

                    foreach (var pattern in patterns)
                    {
                        if (Regex.IsMatch(segment, pattern))
                        {
                            Console.WriteLine($"Match em segmento: {segment}");
                            return FormatPlate(segment);
                        }
                    }
                }
            }

            Console.WriteLine("❌ Nenhum padrão de placa encontrado");
            return null;
        }

        private string FormatPlate(string plate)
        {
            if (string.IsNullOrEmpty(plate) || plate.Length != 7)
                return plate;

            // Mercosul: ABC1D23 -> ABC-1D23
            if (Regex.IsMatch(plate, @"^[A-Z]{3}\d[A-Z]\d{2}$"))
            {
                return $"{plate.Substring(0, 3)}-{plate.Substring(3)}";
            }

            // Antiga: ABC1234 -> ABC-1234
            return $"{plate.Substring(0, 3)}-{plate.Substring(3)}";
        }

        private async Task EnsureTrainedDataExists()
        {
            try
            {
                var tessdataDir = System.IO.Path.Combine(Application.Context.FilesDir.AbsolutePath, "tessdata");
                var traineddataFile = System.IO.Path.Combine(tessdataDir, "eng.traineddata");

                if (!Directory.Exists(tessdataDir))
                {
                    Directory.CreateDirectory(tessdataDir);
                    Console.WriteLine($"✅ Pasta criada: {tessdataDir}");
                }

                if (File.Exists(traineddataFile))
                {
                    var fileSize = new FileInfo(traineddataFile).Length;
                    Console.WriteLine($"✅ eng.traineddata existe ({fileSize / 1024 / 1024}MB)");

                    if (fileSize > 1000000)
                        return;
                }

                Console.WriteLine("📥 Copiando eng.traineddata...");
                using var asset = Application.Context.Assets.Open("tessdata/eng.traineddata");
                using var file = File.Create(traineddataFile);
                await asset.CopyToAsync(file);

                Console.WriteLine("✅ eng.traineddata copiado com sucesso.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao copiar traineddata: {ex.Message}");
                throw;
            }
        }
    }
}