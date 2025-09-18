using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Parking.Mobile.DependencyService.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Linq;
using ZXing.Mobile;

[assembly: Dependency(typeof(Parking.Mobile.Droid.DependencyService.CamScanner))]
namespace Parking.Mobile.Droid.DependencyService
{
    public class CamScanner : ICamScanner
    {
        public event CamScannerHandler OnScannerReader;

        public CameraResolution SelectLowestResolutionMatchingDisplayAspectRatio(List<CameraResolution> availableResolutions)
        {
            CameraResolution result = null;
            double aspectTolerance = 0.1;

            var displayOrientationHeight = DeviceDisplay.MainDisplayInfo.Orientation == DisplayOrientation.Portrait ? DeviceDisplay.MainDisplayInfo.Height : DeviceDisplay.MainDisplayInfo.Width;
            var displayOrientationWidth = DeviceDisplay.MainDisplayInfo.Orientation == DisplayOrientation.Portrait ? DeviceDisplay.MainDisplayInfo.Width : DeviceDisplay.MainDisplayInfo.Height;
            var targetRatio = displayOrientationHeight / displayOrientationWidth;
            var targetHeight = displayOrientationHeight;
            var minDiff = double.MaxValue;

            foreach (var r in availableResolutions.Where(r => Math.Abs(((double)r.Width / r.Height) - targetRatio) < aspectTolerance))
            {
                if (Math.Abs(r.Height - targetHeight) < minDiff)
                    minDiff = Math.Abs(r.Height - targetHeight);
                result = r;
            }

            return result;
        }

        public void ScanAsync()
        {
            var optionsCustom = new MobileBarcodeScanningOptions()
            {
                TryHarder = true,
                //AutoRotate = true,
                //DelayBetweenContinuousScans = 2000,
                //TryInverted = true,
                //CameraResolutionSelector = (availableResolutions) => SelectLowestResolutionMatchingDisplayAspectRatio(availableResolutions),
                UseNativeScanning = true,

                PossibleFormats = new List<ZXing.BarcodeFormat>
                {
                    ZXing.BarcodeFormat.QR_CODE,
                    ZXing.BarcodeFormat.ITF,
                    ZXing.BarcodeFormat.EAN_13
                }
            };

            var scanner = new MobileBarcodeScanner()
            {
                TopText = "Aproxime a câmera do ticket/credencial",
                BottomText = "Toque na tela para focar"
            };

            scanner.AutoFocus();
            scanner.Torch(true);

            if (!scanner.IsTorchOn)
            {
                scanner.ToggleTorch();
            }

            Task.Run(async () =>
            {
                var scanResults = await scanner.Scan(optionsCustom);

                if (scanner.IsTorchOn)
                {
                    scanner.ToggleTorch();
                }

                if (scanResults != null)
                {
                    if (OnScannerReader != null)
                    {
                        OnScannerReader(scanResults.Text);
                    }
                }
                else
                {
                    if (OnScannerReader != null)
                    {
                        OnScannerReader(null);
                    }
                }
            });
        }

        public void ClearDelegates()
        {
            if (this.OnScannerReader != null)
            {
                var dels = (CamScannerHandler)this.OnScannerReader;

                foreach (CamScannerHandler del in dels.GetInvocationList())
                    this.OnScannerReader -= del;
            }
        }
    }
}

