using System;
using Android.Graphics;
using Parking.Mobile.DependencyService.Interfaces;
using System.Collections.Generic;
using System.IO;
using Android.Util;
using Java.IO;
using SkiaSharp;
using Xamarin.Forms;
using ZXing;
using ZXing.Common;
using ZXing.Mobile;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

[assembly: Dependency(typeof(Parking.Mobile.Droid.DependencyService.FilePath))]
namespace Parking.Mobile.Droid.DependencyService
{
    public class FilePath : IFilePath
    {
        public string GetPath()
        {
            return Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath;
            //return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).AbsolutePath;
        }

        public MemoryStream GetBrandTicket()
        {
            Bitmap brandTicket = BitmapFactory.DecodeResource(Forms.Context.Resources, Resource.Drawable.BrandIco);

            MemoryStream stream = new MemoryStream();

            brandTicket.Compress(Bitmap.CompressFormat.Png, 50, stream);

            return stream;
        }

        public MemoryStream GenerateQRCode(string text, int widthImg, int heghtImg)
        {
            var options = new EncodingOptions
            {
                Margin = 0
            };

            options.Hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.L);

            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = options
            };

            var bitmap = writer.Write(text);

            using (Bitmap resizedImage = Bitmap.CreateScaledBitmap(bitmap, widthImg, heghtImg, false))
            {
                MemoryStream memoryStream = new MemoryStream();
                resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, memoryStream);
                memoryStream.Position = 0;
                return memoryStream;
            }

            //return GenerateQrCodeWithCircles(text, widthImg, heghtImg);
        }

        public MemoryStream BitmapToMemoryStreamWithoutCompression(Bitmap bitmap)
        {
            // Calcula o tamanho necessário para armazenar os bytes brutos do bitmap
            int size = bitmap.RowBytes * bitmap.Height;

            // Cria um array de bytes para armazenar os dados brutos do bitmap
            byte[] byteArray = new byte[size];

            // Copia os pixels do bitmap para o array de bytes
            bitmap.CopyPixelsToBuffer(Java.Nio.ByteBuffer.Wrap(byteArray));

            // Converte o array de bytes para um MemoryStream
            MemoryStream stream = new MemoryStream(byteArray);

            // Retorna o MemoryStream contendo os dados do bitmap
            return stream;
        }

        public MemoryStream GenerateQrCodeWithCircles(string text, int width, int height)
        {
            // Configuração para gerar o QR Code
            var options = new QrCodeEncodingOptions
            {
                Width = width,
                Height = height,
                Margin = 0  // Ajuste a margem para evitar cortes
            };

            var writer = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.QR_CODE,
                Options = options
            };

            // Gerar QR Code
            var pixelData = writer.Write(text);

            // Criar um bitmap com SkiaSharp
            var info = new SKImageInfo(width, height);
            var bitmap = new SKBitmap(info);

            using (var canvas = new SKCanvas(bitmap))
            {
                canvas.Clear(SKColors.White);

                // Calcular o tamanho dos módulos no QR Code
                float moduleWidth = (float)width / pixelData.Width;
                float moduleHeight = (float)height / pixelData.Height;
                float circleRadius = Math.Min(moduleWidth, moduleHeight) / 2; // Raio do círculo

                // Desenhar cada módulo "preto" do QR Code como um círculo
                for (int y = 0; y < pixelData.Height; y++)
                {
                    for (int x = 0; x < pixelData.Width; x++)
                    {
                        // Verifica se o pixel é "preto" no QR Code (0 = preto)
                        if (pixelData.Pixels[y * pixelData.Width + x] == 0)
                        {
                            // Calcular a posição central do círculo
                            float cx = x * moduleWidth + moduleWidth / 2;
                            float cy = y * moduleHeight + moduleHeight / 2;

                            // Criar a tinta preta para desenhar o círculo
                            var paint = new SKPaint
                            {
                                Color = SKColors.Black,
                                IsAntialias = true // Suavização das bordas
                            };

                            // Desenhar o círculo
                            canvas.DrawCircle(cx, cy, circleRadius, paint);
                        }
                    }
                }
            }

            // Converter o bitmap para SKImage e depois para MemoryStream
            using (var image = SKImage.FromBitmap(bitmap))
            {
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                {
                    var stream = new MemoryStream();
                    data.SaveTo(stream);
                    stream.Seek(0, SeekOrigin.Begin); // Reposicionar o ponteiro no início do stream
                    return stream;
                }
            }
        }
        /*public MemoryStream GenerateQRCode(string text, int width, int height)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
                int moduleCount = qrCodeData.ModuleMatrix.Count;
                int moduleSize = Math.Min(width / moduleCount, height / moduleCount);

                using (QRCode qrCode = new QRCode(qrCodeData))
                {
                    using (Bitmap qrCodeImage = qrCode.GetGraphic(moduleSize))
                    {
                        // Redimensiona a imagem para as dimensões exatas desejadas
                        using (Bitmap resizedImage = Bitmap.CreateScaledBitmap(qrCodeImage, width, height, false))
                        {
                            MemoryStream memoryStream = new MemoryStream();
                            resizedImage.Compress(Bitmap.CompressFormat.Png, 100, memoryStream);
                            memoryStream.Position = 0; // Resetando a posição do stream para o início
                            return memoryStream;
                        }
                    }
                }
            }
        }*/

        public MemoryStream GenerateBarcode(string code, int widthImg, int heghtImg)
        {
            BitMatrix bitmapMatrix = null;
            if (code.Length == 13)
            {
                bitmapMatrix = new MultiFormatWriter().encode(code, BarcodeFormat.EAN_13, widthImg, heghtImg);
            }
            else
            {
                bitmapMatrix = new MultiFormatWriter().encode(code, BarcodeFormat.ITF, widthImg, heghtImg);
            }

            var width = bitmapMatrix.Width;
            var height = bitmapMatrix.Height;
            int[] pixelsImage = new int[width * height];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (bitmapMatrix[j, i])
                        pixelsImage[i * width + j] = (int)Convert.ToInt64(0xff000000);
                    else
                        pixelsImage[i * width + j] = (int)Convert.ToInt64(0xffffffff);

                }
            }

            Bitmap bitmap = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
            bitmap.SetPixels(pixelsImage, 0, width, 0, 0, width, height);

            /*var sdpath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            var path = System.IO.Path.Combine(sdpath, "barcodeimg.png");
            var stream = new FileStream(path, FileMode.Create);
            bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
            stream.Close();*/
            MemoryStream memoryStream = new MemoryStream();

            bitmap.Compress(Bitmap.CompressFormat.Png, 100, memoryStream);

            return memoryStream;
        }

        public long GetFileSize(string filepath)
        {
            var fileInfo = new FileInfo(filepath);

            return fileInfo.Length;
        }
    }
}

