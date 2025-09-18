using System;
namespace Parking.Mobile.DependencyService.Interfaces
{
    public delegate void CamScannerHandler(string barCode);

    public interface ICamScanner
    {
        event CamScannerHandler OnScannerReader;

        void ScanAsync();

        void ClearDelegates();
    }
}

