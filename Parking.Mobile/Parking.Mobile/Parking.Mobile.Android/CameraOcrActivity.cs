using Android.App;
using Android.OS;
using Android.Views;
using Android.Graphics;
using Android.Hardware;
using Android.Widget;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Parking.Mobile.Droid.DependencyService;

// 🔹 Resolve conflito entre Android.Graphics.Camera e Android.Hardware.Camera
using Camera = Android.Hardware.Camera;

namespace Parking.Mobile.Droid
{
    [Activity(Label = "Camera OCR", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class CameraOcrActivity : Activity, ISurfaceHolderCallback
    {
        private Camera _camera;
        private SurfaceView _surfaceView;
        private ISurfaceHolder _holder;
        private OverlayView _overlayView;
        private Button _captureButton;

        private readonly PlateRecognizer _plateRecognizer = new PlateRecognizer("9a07c1e8b98705af8c20d20a59cb7b36f961a396");
        private static TaskCompletionSource<string> _tcs;

        public static Task<string> WaitForResult()
        {
            _tcs = new TaskCompletionSource<string>();
            return _tcs.Task;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window.AddFlags(WindowManagerFlags.KeepScreenOn);

            var rootLayout = new FrameLayout(this);

            // 🔹 Surface da câmera
            _surfaceView = new SurfaceView(this);
            _holder = _surfaceView.Holder;
            _holder.AddCallback(this);
            rootLayout.AddView(_surfaceView);

            // 🔹 Overlay de mira
            _overlayView = new OverlayView(this);
            rootLayout.AddView(_overlayView);

            // 🔹 Botão de captura
            _captureButton = new Button(this)
            {
                Text = "📸 Tirar Foto",
                TextSize = 18f
            };
            _captureButton.SetPadding(40, 20, 40, 20);
            _captureButton.SetBackgroundColor(Color.ParseColor("#007AFF"));
            _captureButton.SetTextColor(Color.White);
            _captureButton.Click += async (s, e) => await CaptureAndRecognize();

            var buttonParams = new FrameLayout.LayoutParams(
                ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.WrapContent,
                GravityFlags.Bottom | GravityFlags.CenterHorizontal);
            buttonParams.BottomMargin = 100;

            rootLayout.AddView(_captureButton, buttonParams);

            SetContentView(rootLayout);

            RequestRuntimePermissions();
        }

        private void RequestRuntimePermissions()
        {
            var permissions = new string[]
            {
                Android.Manifest.Permission.Camera,
                Android.Manifest.Permission.ReadExternalStorage,
                Android.Manifest.Permission.WriteExternalStorage
            };

            var toRequest = permissions
                .Where(p => CheckSelfPermission(p) != Android.Content.PM.Permission.Granted)
                .ToArray();

            if (toRequest.Length > 0)
                RequestPermissions(toRequest, 1001);
        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            try
            {
                _camera = Camera.Open();
                _camera.SetDisplayOrientation(90); // Preview em modo retrato
                _camera.SetPreviewDisplay(holder);
                _camera.StartPreview();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Erro ao abrir câmera: " + ex.Message, ToastLength.Long).Show();
                Finish();
            }
        }

        public void SurfaceChanged(ISurfaceHolder holder, Android.Graphics.Format format, int width, int height)
        {
            if (_camera == null) return;

            try
            {
                _camera.StopPreview();
                var parameters = _camera.GetParameters();
                var size = parameters.SupportedPreviewSizes
                    .OrderByDescending(s => s.Width * s.Height)
                    .FirstOrDefault();

                if (size != null)
                    parameters.SetPreviewSize(size.Width, size.Height);

                parameters.FocusMode = Camera.Parameters.FocusModeContinuousPicture;
                _camera.SetParameters(parameters);

                _camera.SetDisplayOrientation(90);
                _camera.SetPreviewDisplay(holder);
                _camera.StartPreview();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Erro ao configurar câmera: " + ex.Message, ToastLength.Long).Show();
            }
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            try
            {
                _camera?.StopPreview();
                _camera?.Release();
                _camera = null;
            }
            catch { }
        }

        private int GetCameraDisplayOrientation()
        {
            var info = new Camera.CameraInfo();
            Camera.GetCameraInfo(0, info); // 0 = câmera traseira

            SurfaceOrientation rotation = WindowManager.DefaultDisplay.Rotation;
            int degrees = rotation switch
            {
                SurfaceOrientation.Rotation0 => 0,
                SurfaceOrientation.Rotation90 => 90,
                SurfaceOrientation.Rotation180 => 180,
                SurfaceOrientation.Rotation270 => 270,
                _ => 0
            };

            int result;
            if (info.Facing == CameraFacing.Front)
                result = (info.Orientation + degrees) % 360;
            else
                result = (info.Orientation - degrees + 360) % 360;

            return result;
        }


        private async Task CaptureAndRecognize()
        {
            if (_camera == null)
            {
                Toast.MakeText(this, "Câmera não disponível.", ToastLength.Short).Show();
                return;
            }

            _captureButton.Enabled = false;
            _captureButton.Text = "⏳ Processando...";

            try
            {
                _camera.TakePicture(null, null, new PictureCallback(async (data) =>
                {
                    try
                    {
                        var bmp = BitmapFactory.DecodeByteArray(data, 0, data.Length);

                        // 🔹 Corrige a rotação da imagem conforme o sensor
                        var matrix = new Matrix();
                        matrix.PostRotate(GetCameraDisplayOrientation());

                        var rotated = Bitmap.CreateBitmap(bmp, 0, 0, bmp.Width, bmp.Height, matrix, true);
                        bmp.Recycle();

                        // 🔹 Recorta a região central (mira)
                        var cropRect = _overlayView.GetCropRect(rotated.Width, rotated.Height);
                        var cropped = Bitmap.CreateBitmap(rotated, cropRect.Left, cropRect.Top, cropRect.Width(), cropRect.Height());
                        rotated.Recycle();

                        _overlayView.SetStatusColor(Color.Aqua); // processando...

                        var plate = await _plateRecognizer.ReadPlateAsync(cropped);

                        if (!string.IsNullOrEmpty(plate))
                        {
                            _overlayView.SetStatusColor(Color.Lime);
                            RunOnUiThread(() =>
                            {
                                Toast.MakeText(this, $"✅ Placa detectada: {plate}", ToastLength.Long).Show();
                            });

                            _tcs?.TrySetResult(plate);
                            await Task.Delay(600);
                            Finish();
                        }
                        else
                        {
                            _overlayView.SetStatusColor(Color.Red);
                            RunOnUiThread(() =>
                            {
                                Toast.MakeText(this, "❌ Nenhuma placa detectada.", ToastLength.Short).Show();
                                _captureButton.Enabled = true;
                                _captureButton.Text = "📸 Tirar Foto";
                            });

                            // 🔹 Reativa o preview da câmera
                            RunOnUiThread(() =>
                            {
                                try
                                {
                                    _camera.StopPreview();
                                    _camera.StartPreview();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Erro ao reiniciar preview: {ex.Message}");
                                }
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        RunOnUiThread(() =>
                        {
                            Toast.MakeText(this, $"Erro: {ex.Message}", ToastLength.Long).Show();
                            _captureButton.Enabled = true;
                            _captureButton.Text = "📸 Tirar Foto";
                        });

                        // 🔹 Garante que preview volta mesmo em caso de erro
                        RunOnUiThread(() =>
                        {
                            try
                            {
                                _camera.StopPreview();
                                _camera.StartPreview();
                            }
                            catch { }
                        });
                    }
                }));
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, $"Erro ao capturar: {ex.Message}", ToastLength.Long).Show();
                _captureButton.Enabled = true;
                _captureButton.Text = "📸 Tirar Foto";

                try
                {
                    _camera.StartPreview();
                }
                catch { }
            }
        }


        protected override void OnPause()
        {
            base.OnPause();
            try
            {
                _camera?.StopPreview();
            }
            catch { }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            try
            {
                _camera?.Release();
                _camera = null;
            }
            catch { }
        }
    }

    // 🔹 Callback auxiliar para capturar foto (permite await)
    public class PictureCallback : Java.Lang.Object, Camera.IPictureCallback
    {
        private readonly Func<byte[], Task> _onPictureTaken;

        public PictureCallback(Func<byte[], Task> onPictureTaken)
        {
            _onPictureTaken = onPictureTaken;
        }

        public async void OnPictureTaken(byte[] data, Camera camera)
        {
            await _onPictureTaken?.Invoke(data);
        }
    }

    // 🔲 Overlay visual (mira e cor de status)
    public class OverlayView : Android.Views.View
    {
        private readonly Paint _paint;
        private readonly Rect _rect;
        private Color _statusColor = Color.Aqua;

        public OverlayView(Android.Content.Context context) : base(context)
        {
            _paint = new Paint
            {
                StrokeWidth = 6,
                AntiAlias = true,
                StrokeCap = Paint.Cap.Round,
                StrokeJoin = Paint.Join.Round
            };
            _paint.SetStyle(Paint.Style.Stroke);
            _rect = new Rect();
        }

        public void SetStatusColor(Color color)
        {
            _statusColor = color;
            PostInvalidate();
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            int width = Width;
            int height = Height;
            int rectWidth = (int)(width * 0.6);
            int rectHeight = (int)(height * 0.25);

            int left = (width - rectWidth) / 2;
            int top = (height - rectHeight) / 2;
            int right = left + rectWidth;
            int bottom = top + rectHeight;

            _rect.Set(left, top, right, bottom);

            _paint.Color = _statusColor;
            canvas.DrawRect(_rect, _paint);
        }

        public Rect GetCropRect(int imageWidth, int imageHeight)
        {
            int rectWidth = (int)(imageWidth * 0.6);
            int rectHeight = (int)(imageHeight * 0.25);

            int left = (imageWidth - rectWidth) / 2;
            int top = (imageHeight - rectHeight) / 2;

            return new Rect(left, top, left + rectWidth, top + rectHeight);
        }
    }
}
