using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Parking.Mobile.DependencyService.Interfaces;

namespace Parking.Mobile.Common
{
    public enum LogType
    {
        Info,
        Error,
        Warning
    }

    public static class Util
    {
        public static string ConvertDecimalToHex(decimal value)
        {
            return Convert.ToInt64(value * (decimal)100.00).ToString("X");
        }

        public static decimal ConvertHexToDecimal(string value)
        {
            return Convert.ToDecimal(Convert.ToInt64(value, 16)) / (decimal)100.00;
        }

        public static string ReverseString(string str)
        {
            char[] charArray = str.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static Int64 EncodeDatetimeToUnix(DateTime date)
        {
            return Convert.ToInt64((date - new DateTime(2020, 1, 1)).TotalSeconds);
        }

        public static DateTime DecodeUnixToDatetime(Int64 seconds)
        {
            return new DateTime(2020, 1, 1).AddSeconds(seconds);
        }

        public static string CompactGuidToBase64(Guid guid)
        {
            string base64Guid = Convert.ToBase64String(guid.ToByteArray());

            return base64Guid;
        }

        public static Guid DecodeBase64ToGuid(string base64Guid)
        {
            byte[] guidBytes = Convert.FromBase64String(base64Guid);

            return new Guid(guidBytes);
        }

        public static decimal RoundToNearestTwoDecimalPlaces(decimal d)
        {
            return Math.Round(d, 2, MidpointRounding.AwayFromZero);
        }

        public static T Clone<T>(T obj)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj));
        }

        public static bool ValidateRenavan(string renavan)
        {
            if (string.IsNullOrEmpty(renavan.Trim())) return false;

            int[] d = new int[11];
            string sequencia = "3298765432";
            string SoNumero = Regex.Replace(renavan, "[^0-9]", string.Empty);

            if (string.IsNullOrEmpty(SoNumero)) return false;

            if (new string(SoNumero[0], SoNumero.Length) == SoNumero) return false;
            SoNumero = Convert.ToInt64(SoNumero).ToString("00000000000");

            int v = 0;

            for (int i = 0; i < 11; i++)
                d[i] = Convert.ToInt32(SoNumero.Substring(i, 1));

            for (int i = 0; i < 10; i++)
                v += d[i] * Convert.ToInt32(sequencia.Substring(i, 1));

            v = (v * 10) % 11; v = (v != 10) ? v : 0;

            return (v == d[10]);
        }

        public static bool ValidateCNPJ(string cnpj)
        {
            var numeros = cnpj.Replace(".", "").Replace(@"/", "").Replace("-", "").Substring(0, 12);

            if (!ulong.TryParse(numeros, out ulong _))
                return false;

            var digito = GetCpfCnpjDigit(numeros, true);
            numeros += digito.ToString();

            digito = GetCpfCnpjDigit(numeros, true);
            numeros += digito.ToString();

            return numeros.Equals(cnpj.Replace(".", "").Replace(@"/", "").Replace("-", ""));
        }

        public static bool ValidateCPF(string cpf)
        {
            var numeros = cpf.Replace(".", "").Replace("-", "").Substring(0, 9);

            if (!ulong.TryParse(numeros, out ulong _))
                return false;

            var digito = GetCpfCnpjDigit(numeros, false);
            numeros += digito.ToString();

            digito = GetCpfCnpjDigit(numeros, false);
            numeros += digito.ToString();

            return numeros.Equals(cpf.Replace(".", "").Replace("-", ""));
        }

        public static List<string> ConvertStringToList(string str, int size)
        {
            List<string> lstRet = new List<string>();

            if (!String.IsNullOrEmpty(str))
            {
                string strAux = str;

                while (strAux.Length > 0)
                {
                    int sizeAux = size;

                    if (strAux.Length < size)
                        sizeAux = strAux.Length;

                    lstRet.Add(strAux.Substring(0, sizeAux));

                    if (sizeAux >= size)
                    {
                        strAux = strAux.Substring(sizeAux);
                    }
                    else
                    {
                        strAux = "";
                    }
                }
            }

            return lstRet;
        }

        public static string EncodeToBase64(string texto)
        {
            try
            {
                byte[] textoAsBytes = Encoding.UTF8.GetBytes(texto);
                string resultado = System.Convert.ToBase64String(textoAsBytes);
                return resultado;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string EncodeToBase64(string texto, string enconding)
        {
            try
            {
                return Convert.ToBase64String(Encoding.GetEncoding(enconding).GetBytes(texto));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string DecodeBase64ToString(string texto)
        {
            try
            {
                byte[] dadosAsBytes = System.Convert.FromBase64String(texto);
                string resultado = System.Text.ASCIIEncoding.ASCII.GetString(dadosAsBytes);
                return resultado;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string DecodeBase64ToString(string texto, string enconding)
        {
            try
            {
                return Encoding.GetEncoding(enconding).GetString(Convert.FromBase64String(texto));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string ConvertEnconding(string str)
        {
            return Regex.Replace(str, "[^0-9a-zA-Z]+?", "");
        }

        private static int GetCpfCnpjDigit(string numbers, bool isCnpj)
        {
            var multiplicador = 2;
            var somador = 0;
            var arr = numbers.ToCharArray();
            Array.Reverse(arr);

            foreach (var item in arr)
            {
                somador += multiplicador * Int32.Parse(item.ToString());

                if (isCnpj)
                {
                    if (multiplicador == 9)
                        multiplicador = 1;
                }

                multiplicador++;
            }

            var resto = somador % 11;
            var digito = resto < 2 ? 0 : 11 - resto;

            return digito;
        }

        public static string GenerateVerifierCodeEAN13(string ean13)
        {
            var arrEan = ean13.ToArray();
            var totalPar = 0;
            var totalImpar = 0;

            for (var i = 1; i < (arrEan.Count() + 1); i++)
            {
                if (i % 2 == 0)
                    totalPar += Int32.Parse(arrEan[i - 1].ToString());
                else
                    totalImpar += Int32.Parse(arrEan[i - 1].ToString());
            }

            totalPar *= 3;
            totalImpar += totalPar;

            var digito = 10 - (totalImpar % 10);

            return ean13 + (digito == 10 ? "0" : digito.ToString());
        }

        public static DateTime GetDateWhithoutSeconds(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);
        }

        public static void WriteLog(string message, LogType logType, Type type = null, string method = null)
        {
            string str = String.Format("{0}: ({1}-{2}) {3}",
                                                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                                            type != null ? type.ToString() : "#",
                                                            !String.IsNullOrEmpty(method) ? method : "#",
                                                            message);
            switch (logType)
            {
                case LogType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;

                    var logger = Xamarin.Forms.DependencyService.Get<ILoggerAnalytics>();

                    logger.LogEvent(Parking.Mobile.DependencyService.Enum.LogEventType.error, "Error", str);
                    break;
                case LogType.Info:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogType.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
            }

            Console.WriteLine(str);

            Console.ResetColor();
        }

        public static string CompressString(string text)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(text);

            using (var memoryStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
                {
                    gzipStream.Write(byteArray, 0, byteArray.Length);
                }

                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }

        public static string DecompressString(string compressedText)
        {
            byte[] compressedData = Convert.FromBase64String(compressedText);

            using (var memoryStream = new MemoryStream(compressedData))
            {
                using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    using (var resultStream = new MemoryStream())
                    {
                        gzipStream.CopyTo(resultStream);
                        byte[] decompressedData = resultStream.ToArray();
                        return Encoding.UTF8.GetString(decompressedData);
                    }
                }
            }
        }

        public static string ValidateStringNull(string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return "";
            }
            else
            {
                return str;
            }
        }
    }
}

