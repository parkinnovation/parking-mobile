using System;
using System.IO;

namespace Parking.Mobile.DependencyService.Interfaces
{
    public interface IFilePath
    {
        string GetPath();
        MemoryStream GetBrandTicket();
        MemoryStream GenerateBarcode(string code, int widthImg, int heghtImg);
        MemoryStream GenerateQRCode(string text, int widthImg, int heghtImg);
        long GetFileSize(string filepath);
    }
}

