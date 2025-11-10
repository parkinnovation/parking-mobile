using System;
using System.Threading.Tasks;

namespace Parking.Mobile.DependencyService.Interfaces
{
    public interface IOcrReader
    {
        Task<string> ReadPlateAsync(byte[] imageBytes);
    }
}

