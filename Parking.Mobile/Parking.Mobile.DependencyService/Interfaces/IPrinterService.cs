
using System;
using Parking.Mobile.DependencyService.Model;

namespace Parking.Mobile.DependencyService.Interfaces
{
    public interface IPrinterService
    {
        void PrintText(string text);

        void PrintTicketEntry(PrintTicketInfoModel info);

        void PrintPaymentReceipt(PrintTicketInfoModel info);

        void PrintCashier(PrintTicketInfoModel info);

        void PrintChangeSector(PrintTicketInfoModel info);
    }
}

