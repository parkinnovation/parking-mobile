using System;
using Android.Bluetooth;
using Android.Widget;
using Parking.Mobile.DependencyService.Interfaces;
using Android.Content;
using System.IO;
using System.Text;
using Xamarin.Forms;
using Parking.Mobile.Droid.DependencyService;
using System.Linq;
using Parking.Mobile.DependencyService.Model;
using static System.Net.Mime.MediaTypeNames;
using Parking.Mobile.Common;

[assembly: Dependency(typeof(PrinterService))]
namespace Parking.Mobile.Droid.DependencyService
{
    public class PrinterService : IPrinterService
    {
        private BluetoothSocket socket;

        // Comandos ESC/POS básicos
        private readonly byte[] init = new byte[] { 0x1B, 0x40 };              // Reset
        private readonly byte[] alignCenter = new byte[] { 0x1B, 0x61, 0x01 }; // Centralizar
        private readonly byte[] alignLeft = new byte[] { 0x1B, 0x61, 0x00 };   // Alinhar à esquerda
        private readonly byte[] boldOn = new byte[] { 0x1B, 0x45, 0x01 };      // Negrito ON
        private readonly byte[] boldOff = new byte[] { 0x1B, 0x45, 0x00 };     // Negrito OFF
        private readonly byte[] cut = new byte[] { 0x1D, 0x56, 0x00 };         // Corte (se suportado)
        private readonly byte[] fontCondensed = new byte[] { 0x1B, 0x21, 0x01 }; // Fonte pequena/condensada
        private readonly byte[] fontNormal = new byte[] { 0x1B, 0x21, 0x00 };    // Fonte normal
        private readonly byte[] fontDouble = new byte[] { 0x1B, 0x21, 0x30 };    // Dobra largura e altura
        private readonly byte[] selectCodePage = new byte[] { 0x1B, 0x52, 16 }; // Europa Ocidental

        //private Encoding encoding = Encoding.utf;

        public void PrintText(string text)
        {
            try
            {
                var printerDevice = FindPrinter();
                if (printerDevice == null)
                {
                    //Toast.MakeText(Android.App.Application.Context, "Impressora não encontrada.", ToastLength.Long).Show();
                    return;
                }

                var uuid = Java.Util.UUID.FromString("00001101-0000-1000-8000-00805f9b34fb");
                socket = printerDevice.CreateRfcommSocketToServiceRecord(uuid);
                socket.Connect();

                var output = socket.OutputStream;

                // Inicializa impressora
                output.Write(init, 0, init.Length);

                output.Write(fontNormal, 0, fontNormal.Length);

                output.Write(alignCenter, 0, alignCenter.Length);
                output.Write(boldOn, 0, boldOn.Length);

                var data = Encoding.UTF8.GetBytes($"{text}\n\n");
                output.Write(data, 0, data.Length);

                output.Write(boldOff, 0, boldOff.Length);
                output.Write(fontNormal, 0, fontNormal.Length);

                // Exemplo de impressão de código de barras (Interleaved 2 of 5)
                PrintBarcode(output, "1234567890");

                // Exemplo de impressão de QR Code
                PrintQrCode(output, "https://meusite.com");

                // Corta o papel (se suportado)
                output.Write(cut, 0, cut.Length);
                output.Flush();

                socket.Close();
                //Toast.MakeText(Android.App.Application.Context, "Impressão enviada!", ToastLength.Short).Show();
            }
            catch (Exception ex)
            {
                //Toast.MakeText(Android.App.Application.Context, $"Erro: {ex.Message}", ToastLength.Long).Show();
            }
        }

        private void PrintBarcode(Stream output, string code)
        {
            // Garante apenas dígitos (ITF é numérico)
            var numeric = new string(code?.Where(char.IsDigit).ToArray() ?? new char[0]);

            if (string.IsNullOrEmpty(numeric))
            {
                // nada para imprimir
                return;
            }

            // Se comprimento ímpar, prefixa '0' (ITF precisa de pares)
            if ((numeric.Length % 2) != 0)
                numeric = "0" + numeric;

            // ===== Define parâmetros visuais do código de barras =====
            // Altura do código de barras (GS h n) - 100 dots (ajuste entre 1 e 255)
            output.Write(new byte[] { 0x1D, 0x68, 100 }, 0, 3);

            // Largura do módulo do código de barras (GS w n) - n entre 2 e 6 (ajuste conforme necessário)
            // Alguns firmwares usam GS w para barcodes, outros usam GS x; 0x1D 0x77 é comum.
            // Se sua impressora não suportar, remova esta linha.
            try
            {
                output.Write(new byte[] { 0x1D, 0x77, 3 }, 0, 3); // largura = 3
            }
            catch { /* se não suportar, ignore */ }

            // Posição do HRI (números) (GS H n): 0 = none, 1 = above, 2 = below, 3 = both
            output.Write(new byte[] { 0x1D, 0x48, 0x02 }, 0, 0); // abaixo

            // Fonte do HRI (GS f n): 0 = font A, 1 = font B
            output.Write(new byte[] { 0x1D, 0x66, 0x00 }, 0, 3); // font A

            // Centraliza
            output.Write(alignCenter, 0, alignCenter.Length);

            // ===== Envia o comando Interleaved 2 of 5 =====
            // GS k m d... NUL   where m = 0x05 (I2of5)
            byte m = 0x05;
            byte[] barcodeData = Encoding.ASCII.GetBytes(numeric);

            // Monta o buffer: [GS][k][m][dados][NUL]
            using (var ms = new MemoryStream())
            {
                ms.WriteByte(0x1D); // GS
                ms.WriteByte(0x6B); // k
                ms.WriteByte(m);    // tipo = I25 (0x05)
                ms.Write(barcodeData, 0, barcodeData.Length);
                ms.WriteByte(0x00); // terminador NUL
                var cmd = ms.ToArray();
                output.Write(cmd, 0, cmd.Length);
            }

            // Quebra de linha após o código de barras
            output.Write(Encoding.UTF8.GetBytes("\n\n"), 0, 2);
        }

        private void PrintQrCode(Stream output, string content)
        {
            byte model = 49; // Modelo 2
            byte size = 5;   // Tamanho do módulo (1-16)
            byte errorLevel = 48; // Nível de correção de erro (48=L, 49=M, 50=Q, 51=H)

            byte[] storeQRData = Encoding.UTF8.GetBytes(content);
            int len = storeQRData.Length + 3;

            byte pL = (byte)(len % 256);
            byte pH = (byte)(len / 256);

            // Seleciona o modelo
            output.Write(new byte[] { 0x1D, 0x28, 0x6B, 0x04, 0x00, 0x31, 0x41, model, 0x00 }, 0, 9);
            // Define tamanho
            output.Write(new byte[] { 0x1D, 0x28, 0x6B, 0x03, 0x00, 0x31, 0x43, size }, 0, 8);
            // Define nível de correção
            output.Write(new byte[] { 0x1D, 0x28, 0x6B, 0x03, 0x00, 0x31, 0x45, errorLevel }, 0, 8);
            // Armazena dados
            output.Write(new byte[] { 0x1D, 0x28, 0x6B, pL, pH, 0x31, 0x50, 0x30 }, 0, 8);
            output.Write(storeQRData, 0, storeQRData.Length);
            // Imprime QR Code
            output.Write(new byte[] { 0x1D, 0x28, 0x6B, 0x03, 0x00, 0x31, 0x51, 0x30 }, 0, 8);

            output.Write(Encoding.ASCII.GetBytes("\n\n"), 0, 2);
        }

        private BluetoothDevice FindPrinter()
        {
            var adapter = BluetoothAdapter.DefaultAdapter;
            if (adapter == null || !adapter.IsEnabled)
            {
                //Toast.MakeText(Android.App.Application.Context, "Bluetooth desativado.", ToastLength.Long).Show();
                return null;
            }

            foreach (var device in adapter.BondedDevices)
            {
                if (device.Name.Contains("Printer") || device.Name.Contains("GS"))
                    return device;
            }

            return null;
        }

        public void PrintTicketEntry(PrintTicketInfoModel info)
        {
            try
            {
                var printerDevice = FindPrinter();

                if (printerDevice == null)
                {
                    //Toast.MakeText(Android.App.Application.Context, "Impressora não encontrada.", ToastLength.Long).Show();
                    return;
                }

                var uuid = Java.Util.UUID.FromString("00001101-0000-1000-8000-00805f9b34fb");
                socket = printerDevice.CreateRfcommSocketToServiceRecord(uuid);
                socket.Connect();

                var output = socket.OutputStream;

                output.Write(init, 0, init.Length);

                output.Write(fontNormal, 0, fontNormal.Length);

                output.Write(alignCenter, 0, alignCenter.Length);
                output.Write(fontCondensed, 0, fontCondensed.Length);

                byte[] data;

                if (!String.IsNullOrEmpty(AppContextGeneral.deviceInfo.TicketHeader1))
                {
                    data = Encoding.UTF8.GetBytes($"{AppContextGeneral.deviceInfo.TicketHeader1}\n");
                    output.Write(data, 0, data.Length);
                }

                if (!String.IsNullOrEmpty(AppContextGeneral.deviceInfo.TicketHeader2))
                {
                    data = Encoding.UTF8.GetBytes($"{AppContextGeneral.deviceInfo.TicketHeader2}\n");
                    output.Write(data, 0, data.Length);
                }

                if (!String.IsNullOrEmpty(AppContextGeneral.deviceInfo.TicketHeader3))
                {
                    data = Encoding.UTF8.GetBytes($"{AppContextGeneral.deviceInfo.TicketHeader3}\n");
                    output.Write(data, 0, data.Length);
                }

                data = Encoding.UTF8.GetBytes($"\n");
                output.Write(data, 0, data.Length);

                output.Write(fontDouble, 0, fontDouble.Length);

                if (!String.IsNullOrEmpty(info.Plate))
                {
                    data = Encoding.UTF8.GetBytes($"{info.Plate}\n");
                    output.Write(data, 0, data.Length);
                }

                output.Write(fontCondensed, 0, fontCondensed.Length);

                output.Write(alignLeft, 0, alignLeft.Length);

                data = Encoding.UTF8.GetBytes($"Ticket   : {info.TicketNumber}\n");
                output.Write(data, 0, data.Length);
                data = Encoding.UTF8.GetBytes($"Entrada  : {info.DateEntry.ToString("dd/MM/yyyy HH:mm")}\n");
                output.Write(data, 0, data.Length);
                data = Encoding.UTF8.GetBytes($"Terminal : {AppContextGeneral.deviceInfo.Description}\n");
                output.Write(data, 0, data.Length);

                if (!String.IsNullOrEmpty(info.CredentialName))
                {
                    data = Encoding.UTF8.GetBytes($"Cred/Mens: {info.CredentialName}\n");
                    output.Write(data, 0, data.Length);
                }

                if (!String.IsNullOrEmpty(info.VehicleModel))
                {
                    data = Encoding.UTF8.GetBytes($"Veiculo  : {info.VehicleModel}\n");
                    output.Write(data, 0, data.Length);
                }

                if (!String.IsNullOrEmpty(info.VehicleColor))
                {
                    data = Encoding.UTF8.GetBytes($"Cor      : {info.VehicleColor}\n");
                    output.Write(data, 0, data.Length);
                }

                if (!String.IsNullOrEmpty(info.Prism))
                {
                    data = Encoding.UTF8.GetBytes($"Prisma   : {info.Prism}\n");
                    output.Write(data, 0, data.Length);
                }

                data = Encoding.UTF8.GetBytes($"\n");
                output.Write(data, 0, data.Length);

                output.Write(alignCenter, 0, alignCenter.Length);

                PrintBarcode(output, info.TicketNumber);

                output.Write(fontCondensed, 0, fontCondensed.Length);

                if (!String.IsNullOrEmpty(AppContextGeneral.deviceInfo.TicketFooter1))
                {
                    data = Encoding.UTF8.GetBytes($"{AppContextGeneral.deviceInfo.TicketFooter1}\n");
                    output.Write(data, 0, data.Length);
                }

                if (!String.IsNullOrEmpty(AppContextGeneral.deviceInfo.TicketFooter2))
                {
                    data = Encoding.UTF8.GetBytes($"{AppContextGeneral.deviceInfo.TicketFooter2}\n");
                    output.Write(data, 0, data.Length);
                }

                if (!String.IsNullOrEmpty(AppContextGeneral.deviceInfo.TicketFooter3))
                {
                    data = Encoding.UTF8.GetBytes($"{AppContextGeneral.deviceInfo.TicketFooter3}\n");
                    output.Write(data, 0, data.Length);
                }

                data = Encoding.UTF8.GetBytes($"LokiD  v{AppContextGeneral.Version}\n\n\n\n");
                output.Write(data, 0, data.Length);

                output.Write(cut, 0, cut.Length);
                output.Flush();

                socket.Close();
                //Toast.MakeText(Android.App.Application.Context, "Impressão enviada!", ToastLength.Short).Show();
            }
            catch (Exception ex)
            {
                //Toast.MakeText(Android.App.Application.Context, $"Erro: {ex.Message}", ToastLength.Long).Show();
            }
        }

        public void PrintPaymentReceipt(PrintTicketInfoModel info)
        {
            try
            {
                var printerDevice = FindPrinter();

                if (printerDevice == null)
                {
                    //Toast.MakeText(Android.App.Application.Context, "Impressora não encontrada.", ToastLength.Long).Show();
                    return;
                }

                var uuid = Java.Util.UUID.FromString("00001101-0000-1000-8000-00805f9b34fb");
                socket = printerDevice.CreateRfcommSocketToServiceRecord(uuid);
                socket.Connect();

                var output = socket.OutputStream;

                output.Write(init, 0, init.Length);

              
                output.Write(selectCodePage, 0, selectCodePage.Length);

                output.Write(fontNormal, 0, fontNormal.Length);

                output.Write(alignCenter, 0, alignCenter.Length);
                output.Write(fontCondensed, 0, fontCondensed.Length);

                byte[] data;

                if (!String.IsNullOrEmpty(AppContextGeneral.deviceInfo.TicketHeader1))
                {
                    data = Encoding.UTF8.GetBytes($"{AppContextGeneral.deviceInfo.TicketHeader1}\n");
                    output.Write(data, 0, data.Length);
                }

                if (!String.IsNullOrEmpty(AppContextGeneral.deviceInfo.TicketHeader2))
                {
                    data = Encoding.UTF8.GetBytes($"{AppContextGeneral.deviceInfo.TicketHeader2}\n");
                    output.Write(data, 0, data.Length);
                }

                if (!String.IsNullOrEmpty(AppContextGeneral.deviceInfo.TicketHeader3))
                {
                    data = Encoding.UTF8.GetBytes($"{AppContextGeneral.deviceInfo.TicketHeader3}\n");
                    output.Write(data, 0, data.Length);
                }

                data = Encoding.UTF8.GetBytes($"\n");
                output.Write(data, 0, data.Length);

                output.Write(fontDouble, 0, fontDouble.Length);

                if (!String.IsNullOrEmpty(info.Plate))
                {
                    data = Encoding.UTF8.GetBytes($"{info.Plate}\n");
                    output.Write(data, 0, data.Length);
                }

                output.Write(fontCondensed, 0, fontCondensed.Length);

                output.Write(alignLeft, 0, alignLeft.Length);

                data = Encoding.UTF8.GetBytes($"Ticket      : {info.TicketNumber}\n");
                output.Write(data, 0, data.Length);
                data = Encoding.UTF8.GetBytes($"Entrada     : {info.DateEntry.ToString("dd/MM/yyyy HH:mm")}\n");
                output.Write(data, 0, data.Length);
                data = Encoding.UTF8.GetBytes($"Terminal    : {AppContextGeneral.deviceInfo.Description}\n");
                output.Write(data, 0, data.Length);

                if (!String.IsNullOrEmpty(info.CredentialName))
                {
                    data = Encoding.UTF8.GetBytes($"Cred/Mens   : {info.CredentialName}\n");
                    output.Write(data, 0, data.Length);
                }

                if (!String.IsNullOrEmpty(info.VehicleModel))
                {
                    data = Encoding.UTF8.GetBytes($"Veiculo     : {info.VehicleModel}\n");
                    output.Write(data, 0, data.Length);
                }

                if (!String.IsNullOrEmpty(info.VehicleColor))
                {
                    data = Encoding.UTF8.GetBytes($"Cor         : {info.VehicleColor}\n");
                    output.Write(data, 0, data.Length);
                }

                if (!String.IsNullOrEmpty(info.Prism))
                {
                data = Encoding.UTF8.GetBytes($"Prisma      : {info.Prism}\n");
                    output.Write(data, 0, data.Length);
                }

                data = Encoding.UTF8.GetBytes($"Pagamento   : {info.DatePayment.ToString("dd/MM/yyyy HH:mm")}\n");
                output.Write(data, 0, data.Length);

                data = Encoding.UTF8.GetBytes($"Limite Saida: {info.DateLimitExit.ToString("dd/MM/yyyy HH:mm")}\n");
                output.Write(data, 0, data.Length);

                data = Encoding.UTF8.GetBytes($"Permanencia : {info.Stay}\n\n");
                output.Write(data, 0, data.Length);

                data = Encoding.UTF8.GetBytes($"Valor Total : R$ {info.Amount.ToString("F2")}\n\n");
                output.Write(data, 0, data.Length);

                data = Encoding.UTF8.GetBytes("Formas de Pagamento\n");
                output.Write(data, 0, data.Length);

                foreach(var payment in info.Payments.Where(x=>x.Amount!=0))
                {
                    data = Encoding.UTF8.GetBytes($"{payment.PaymentMethod}: R{(char)0x24} {payment.Amount.ToString("F2")}\n");
                    output.Write(data, 0, data.Length);
                }

                data = Encoding.UTF8.GetBytes($"\n");
                output.Write(data, 0, data.Length);

                output.Write(alignCenter, 0, alignCenter.Length);

                output.Write(fontCondensed, 0, fontCondensed.Length);

                if (!String.IsNullOrEmpty(AppContextGeneral.deviceInfo.ReceiptFooter1))
                {
                    data = Encoding.UTF8.GetBytes($"{AppContextGeneral.deviceInfo.ReceiptFooter1}\n");
                    output.Write(data, 0, data.Length);
                }

                if (!String.IsNullOrEmpty(AppContextGeneral.deviceInfo.ReceiptFooter2))
                {
                    data = Encoding.UTF8.GetBytes($"{AppContextGeneral.deviceInfo.ReceiptFooter2}\n");
                    output.Write(data, 0, data.Length);
                }

                if (!String.IsNullOrEmpty(AppContextGeneral.deviceInfo.ReceiptFooter3))
                {
                    data = Encoding.UTF8.GetBytes($"{AppContextGeneral.deviceInfo.ReceiptFooter3}\n");
                    output.Write(data, 0, data.Length);
                }

                data = Encoding.UTF8.GetBytes($"LokiD  v{AppContextGeneral.Version}\n\n\n\n");
                output.Write(data, 0, data.Length);

                output.Write(cut, 0, cut.Length);
                output.Flush();

                socket.Close();
                //Toast.MakeText(Android.App.Application.Context, "Impressão enviada!", ToastLength.Short).Show();
            }
            catch (Exception ex)
            {
                //Toast.MakeText(Android.App.Application.Context, $"Erro: {ex.Message}", ToastLength.Long).Show();
            }
        }

        public void PrintCashier(PrintTicketInfoModel info)
        {
            try
            {
                var printerDevice = FindPrinter();

                if (printerDevice == null)
                {
                    return;
                }

                var uuid = Java.Util.UUID.FromString("00001101-0000-1000-8000-00805f9b34fb");
                socket = printerDevice.CreateRfcommSocketToServiceRecord(uuid);
                socket.Connect();

                var output = socket.OutputStream;

                output.Write(init, 0, init.Length);

                output.Write(fontNormal, 0, fontNormal.Length);

                output.Write(alignCenter, 0, alignCenter.Length);
                output.Write(fontCondensed, 0, fontCondensed.Length);

                byte[] data;

                if (!String.IsNullOrEmpty(AppContextGeneral.deviceInfo.TicketHeader1))
                {
                    data = Encoding.UTF8.GetBytes($"{AppContextGeneral.deviceInfo.TicketHeader1}\n");
                    output.Write(data, 0, data.Length);
                }

                if (!String.IsNullOrEmpty(AppContextGeneral.deviceInfo.TicketHeader2))
                {
                    data = Encoding.UTF8.GetBytes($"{AppContextGeneral.deviceInfo.TicketHeader2}\n");
                    output.Write(data, 0, data.Length);
                }

                if (!String.IsNullOrEmpty(AppContextGeneral.deviceInfo.TicketHeader3))
                {
                    data = Encoding.UTF8.GetBytes($"{AppContextGeneral.deviceInfo.TicketHeader3}\n");
                    output.Write(data, 0, data.Length);
                }

                data = Encoding.UTF8.GetBytes($"\n");
                output.Write(data, 0, data.Length);

                output.Write(fontCondensed, 0, fontCondensed.Length);

                data = Encoding.UTF8.GetBytes($"Relatorio Parcial\n\n");
                output.Write(data, 0, data.Length);

                output.Write(alignLeft, 0, alignLeft.Length);

                data = Encoding.UTF8.GetBytes($"Operador   : {info.UserName.ToUpper()}\n");
                output.Write(data, 0, data.Length);

                data = Encoding.UTF8.GetBytes($"Terminal   : {AppContextGeneral.deviceInfo.Description}\n");
                output.Write(data, 0, data.Length);

                data = Encoding.UTF8.GetBytes($"Parcial No : {info.CashierNumber.ToString()}\n");
                output.Write(data, 0, data.Length);

                data = Encoding.UTF8.GetBytes($"Abertura   : {info.CashierOpen.ToString("dd/MM/yyyy HH:mm")}\n");
                output.Write(data, 0, data.Length);

                data = Encoding.UTF8.GetBytes($"Fechamento : {info.CashierClose.ToString("dd/MM/yyyy HH:mm")}\n");
                output.Write(data, 0, data.Length);

                data = Encoding.UTF8.GetBytes($"Fundo      : RS {info.CashierAmount.ToString("F2")}\n\n");
                output.Write(data, 0, data.Length);

                output.Write(fontCondensed, 0, fontCondensed.Length);

                output.Write(alignCenter, 0, alignCenter.Length);

                data = Encoding.UTF8.GetBytes($"LokiD  v{AppContextGeneral.Version}\n\n\n\n\n");
                output.Write(data, 0, data.Length);

                output.Write(cut, 0, cut.Length);
                output.Flush();

                socket.Close();
                //Toast.MakeText(Android.App.Application.Context, "Impressão enviada!", ToastLength.Short).Show();
            }
            catch (Exception ex)
            {
                //Toast.MakeText(Android.App.Application.Context, $"Erro: {ex.Message}", ToastLength.Long).Show();
            }
        }
    }
}
