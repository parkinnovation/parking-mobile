using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Xamarin.Forms;
using Acr.UserDialogs;
using Parking.Mobile.ApplicationCore;
using Parking.Mobile.View;
using Parking.Mobile.Common;
using System.Threading.Tasks;
using Parking.Mobile.Interface.Message.Request;
using Parking.Mobile.Data.Model;
using Parking.Mobile.Interface.Message.Response;
using Parking.Mobile.DependencyService.Interfaces;
using Parking.Mobile.DependencyService.Model;
using Xamarin.CommunityToolkit.Extensions;

namespace Parking.Mobile.ViewModel
{
    public class PaymentViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int idPriceTableIndex = -1;

        public int IDPriceTableIndex
        {
            get => idPriceTableIndex;
            set
            {
                idPriceTableIndex = value;

                if (idPriceTableIndex >= 0)
                {
                    LoadDiscount();
                    UpdateCalculatedPrice();
                }

                OnPropertyChanged(nameof(IDPriceTableIndex));

                IsPaymentMode = true;
            }
        }

        private int idDiscountIndex = -1;

        public int IDDiscountIndex
        {
            get => idDiscountIndex;
            set
            {
                idDiscountIndex = value;

                if(idDiscountIndex>=0)
                {
                    UpdateCalculatedPrice();
                }

                OnPropertyChanged(nameof(IDDiscountIndex));
            }
        }

        private int idPaymentMethodIndex = -1;

        public int IDPaymentMethodIndex
        {
            get => idPaymentMethodIndex;
            set
            {
                idPaymentMethodIndex = value;

                OnPropertyChanged(nameof(IDPaymentMethodIndex));
            }
        }

        private string CodeRead;

        private string plate;
        public string Plate { get => plate; set { plate = value; OnPropertyChanged(nameof(Plate)); } }

        private string ticket;
        public string Ticket { get => ticket; set { ticket = value; OnPropertyChanged(nameof(Ticket)); } }

        private bool isSearchMode = true;
        public bool IsSearchMode { get => isSearchMode; set { isSearchMode = value; OnPropertyChanged(nameof(IsSearchMode)); } }

        private bool isTicketMode;
        public bool IsTicketMode { get => isTicketMode; set { isTicketMode = value; OnPropertyChanged(nameof(IsTicketMode)); } }

        private bool isPaymentMode;
        public bool IsPaymentMode { get => isPaymentMode; set { isPaymentMode = value; OnPropertyChanged(nameof(IsPaymentMode)); } }

        private TicketInfoModel ticketInfo;
        public TicketInfoModel TicketInfo { get => ticketInfo; set { ticketInfo = value; OnPropertyChanged(nameof(TicketInfo)); } }

        private List<PriceTableInfo> priceTables;
        public List<PriceTableInfo> PriceTables { get => priceTables; set { priceTables = value; OnPropertyChanged(nameof(PriceTables)); } }

        private List<DiscountInfo> discounts;
        public List<DiscountInfo> Discounts { get => discounts; set { discounts = value; OnPropertyChanged(nameof(Discounts)); } }

        private List<PaymentMethodInfo> paymentMethods;
        public List<PaymentMethodInfo> PaymentMethods { get => paymentMethods; set { paymentMethods = value; OnPropertyChanged(nameof(PaymentMethods)); } }

        private string selectedPaymentMethod;
        public string SelectedPaymentMethod { get => selectedPaymentMethod; set { selectedPaymentMethod = value; OnPropertyChanged(nameof(SelectedPaymentMethod)); } }

        private string selectedPriceTable;
        public string SelectedPriceTable
        {
            get => selectedPriceTable;
            set
            {
                selectedPriceTable = value;
                OnPropertyChanged(nameof(SelectedPriceTable));
                if (!string.IsNullOrEmpty(selectedPriceTable))
                {
                    UpdateCalculatedPrice();
                    IsPaymentMode = true;
                }
            }
        }

        private string selectedDiscount;
        public string SelectedDiscount
        {
            get => selectedDiscount;
            set
            {
                selectedDiscount = value;
                OnPropertyChanged(nameof(SelectedDiscount));
                /*if (!string.IsNullOrEmpty(selectedDiscount))
                {
                    UpdateCalculatedPrice();
                    IsPaymentMode = true;
                }*/
            }
        }

        private decimal calculatedPrice;
        public decimal CalculatedPrice { get => calculatedPrice; set { calculatedPrice = value; OnPropertyChanged(nameof(CalculatedPrice)); } }

        
        public Command<string> ActionPage { get; }
        private readonly INavigation Navigation;

        public PaymentViewModel(INavigation navigation)
        {
            Navigation = navigation;

            AppContextGeneral.scannerDep.ClearDelegates();
            AppContextGeneral.scannerDep.OnScannerReader += ScannerDep_OnScannerReader;

            if (AppContextGeneral.cashierInfo == null)
            {
                var ret = Navigation.ShowPopupAsync<object>(new OpenCashierPage());
            }

            ActionPage = new Command<string>(ActionButton);
        }

        private void ScannerDep_OnScannerReader(string barCode)
        {
            this.CodeRead = barCode;

            if (!String.IsNullOrEmpty(barCode))
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    GetTicketInfo();
                });
            }
        }

        private void ActionButton(string parameter)
        {
            switch (parameter)
            {
                case "Search":
                    SearchTicket();
                    break;

                case "Scanner":
                    this.CodeRead = null;

                    AppContextGeneral.scannerDep.ScanAsync();

                    break;

                case "Confirm":
                    ConfirmPayment();
                    break;

                case "Cancel":
                    ResetScreen();
                    break;
            }
        }

        private void LoadPriceTable()
        {
            UserDialogs.Instance.ShowLoading("Processando...");

            Task.Run(() =>
            {
                try
                {
                    AppPricing appPricing = new AppPricing();

                    var response = appPricing.GetListPriceTable(new GetListPriceTableRequest()
                    {
                        IdDevice = AppContextGeneral.deviceInfo.IDDevice,
                        IdParkingLot = this.TicketInfo.IDParkingLot,
                        ParkingCode = AppContextGeneral.parkingInfo.ParkingCode,
                        Code = this.TicketInfo.Ticket
                    });

                    if (response.Success)
                    {
                        if (response.Data != null && response.Data.PriceTables != null && response.Data.PriceTables.Count > 0)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                UserDialogs.Instance.HideLoading();

                                this.PriceTables = response.Data.PriceTables;

                                if (this.PriceTables.Count > 1)
                                {
                                    int indexDefault = this.PriceTables.FindIndex(x => x.Default);

                                    if (indexDefault >= 0)
                                    {
                                        this.IDPriceTableIndex = indexDefault;
                                    }
                                }
                                else
                                {
                                    this.IDPriceTableIndex = 0;
                                }
                            });
                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                UserDialogs.Instance.HideLoading();

                                Application.Current.MainPage.DisplayAlert("Erro", "Não existe tabela de preço configurada", "Ok");
                            });
                        }
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            UserDialogs.Instance.HideLoading();

                            Application.Current.MainPage.DisplayAlert("Erro", response.Message, "Ok");
                        });
                    }
                }catch(Exception ex)
                {

                }
            });
        }

        private void LoadDiscount()
        { 
            UserDialogs.Instance.ShowLoading("Processando...");

            Task.Run(() =>
            {
                try
                {
                    AppPricing appPricing = new AppPricing();

                    var response = appPricing.GetListDiscount(AppContextGeneral.parkingInfo.ParkingCode, PriceTables[IDPriceTableIndex].IdPriceTable);

                    if (response.Success)
                    {
                        if (response.Data != null && response.Data.Discounts != null && response.Data.Discounts.Count > 0)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                UserDialogs.Instance.HideLoading();

                                this.Discounts = response.Data.Discounts;
                            });
                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                UserDialogs.Instance.HideLoading();
                            });
                        }
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            UserDialogs.Instance.HideLoading();

                            Application.Current.MainPage.DisplayAlert("Erro", response.Message, "Ok");
                        });
                    }
                }
                catch (Exception ex)
                {

                }
            });
        }

        private void LoadListPaymentMethod()
        {
            UserDialogs.Instance.ShowLoading("Processando...");

            Task.Run(() =>
            {
                AppPayment appPayment = new AppPayment();

                var response = appPayment.GetListPaymentMethod(new GetListPaymentMethodRequest()
                {
                    Plate = this.TicketInfo.Plate,
                    ParkingCode = AppContextGeneral.parkingInfo.ParkingCode
                });

                if (response.Success)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        UserDialogs.Instance.HideLoading();

                        this.PaymentMethods = response.Data.Methods;
                    });
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        UserDialogs.Instance.HideLoading();

                        Application.Current.MainPage.DisplayAlert("Erro", response.Message, "Ok");
                    });
                }
            });
        }

        private void GetTicketInfo()
        {
            UserDialogs.Instance.ShowLoading("Processando...");

            Task.Run(async () =>
            {
                AppParkingLot appParkingLot = new AppParkingLot();

                string accessCode = "";

                if (!String.IsNullOrEmpty(Plate))
                {
                    accessCode = this.Plate;
                }

                if (!String.IsNullOrEmpty(ticket))
                {
                    accessCode = this.Ticket;
                }

                if (!String.IsNullOrEmpty(CodeRead))
                {
                    accessCode = this.CodeRead;
                }

                var response = appParkingLot.GetTicketInfo(new GetTicketInfoRequest()
                {
                    AccessCode = accessCode,
                    IDDevice = AppContextGeneral.deviceInfo.IDDevice,
                    IDUser = AppContextGeneral.userInfo.IdUser,
                    ParkingCode = AppContextGeneral.parkingInfo.ParkingCode
                });

                if (response.Success)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        UserDialogs.Instance.HideLoading();

                        TicketInfo = new TicketInfoModel()
                        {
                            Credential = response.Data.Credential,
                            CredentialName = response.Data.CredentialName,
                            DateEntry = response.Data.DateEntry,
                            IDParkingLot = response.Data.IDParkingLot,
                            Plate = response.Data.Plate,
                            Prism = response.Data.Prism,
                            Stay = response.Data.Stay,
                            Ticket = response.Data.Ticket,
                            VehicleColor = response.Data.VehicleColor,
                            VehicleModel = response.Data.VehicleModel
                        };

                        if (response.Data.Payments != null)
                        {
                            TicketInfo.PaymentsOriginal = (from p in response.Data.Payments
                                                           select new TicketPaymentItemInfo()
                                                           {
                                                               Amount = p.Amount,
                                                               Description = p.Description,
                                                               IDDiscount = p.IDDiscount,
                                                               IDParkingSeal = p.IDParkingSeal,
                                                               IDPayment = p.IDPayment,
                                                               IDPaymentMethod = p.IDPaymentMethod
                                                           }).ToList();

                            TicketInfo.Payments = Util.Clone<List<TicketPaymentItemInfo>>(ticketInfo.PaymentsOriginal);
                        }

                        IsSearchMode = false;
                        IsTicketMode = true;
                        IsPaymentMode = false;

                        LoadPriceTable();

                        LoadListPaymentMethod();
                    });
                }
                else
                {
                    string codeAux = this.CodeRead;

                    this.CodeRead = null;

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        UserDialogs.Instance.HideLoading();

                        Application.Current.MainPage.DisplayAlert("Erro", response.Message + " - " + codeAux, "Ok");
                    });
                }
            });
        }

        private bool GetTicketPrice()
        {
            
            AppPricing appPricing = new AppPricing();

            var response = appPricing.GetTicketPrice(new GetTicketPriceRequest()
            {
                AccessCode = this.TicketInfo.Ticket,
                IdParkingLot = this.TicketInfo.IDParkingLot,
                IdPriceTable = PriceTables[IDPriceTableIndex].IdPriceTable,
                IdDevice = AppContextGeneral.deviceInfo.IDDevice,
                ParkingCode = AppContextGeneral.parkingInfo.ParkingCode,
                DatePriceScheduller = this.TicketInfo.DatePriceScheduller,
                DateBillingLimit = this.TicketInfo.DateBillingLimit,
                IdDiscount = IDDiscountIndex>=0 ? Discounts[IDDiscountIndex].IdDiscount : -1
            });

            if (response.Success)
            {
                TicketInfoModel model = this.TicketInfo;

                model.DateLimitExit = response.Data.DateLimitExit;
                model.Price = response.Data.Price;
                model.DiscountPercent = response.Data.DiscountPercent;

                this.TicketInfo = model;

                return true;
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    UserDialogs.Instance.HideLoading();

                    Application.Current.MainPage.DisplayAlert("Aviso", response.Message, "Ok");
                });

                return false;
            }
            
        }

        private void SearchTicket()
        {
            if (string.IsNullOrEmpty(Plate) && string.IsNullOrEmpty(Ticket))
            {
                Application.Current.MainPage.DisplayAlert("Aviso", "Informe a placa ou o ticket.", "OK");
                return;
            }

            UserDialogs.Instance.ShowLoading("Buscando ticket...");
            Thread.Sleep(500); // Simula busca

            GetTicketInfo();

            UserDialogs.Instance.HideLoading();
        }

        private void ConfirmPayment()
        {
            if (IDPaymentMethodIndex<0)
            {
                Application.Current.MainPage.DisplayAlert("Aviso", "Selecione a forma de pagamento.", "OK");
                return;
            }

            ProcessPayment();


            Application.Current.MainPage.DisplayAlert("Sucesso",
                $"Pagamento confirmado!\n\nTicket: {TicketInfo.Ticket}\nPreço: R$ {CalculatedPrice:F2}\nForma: {PaymentMethods[IDPaymentMethodIndex].Description}",
                "OK");

            ResetScreen();
        }

        private void ProcessPayment()
        {
            try
            {
                AppPayment appPayment = new AppPayment();

                DateTime? dateExit = null;

                if (this.TicketInfo.DatePriceScheduller.HasValue && AppContextGeneral.deviceInfo.PaymentInEntry)
                {
                    dateExit = this.TicketInfo.DatePriceScheduller.Value;
                }

                if (TicketInfo.Payments == null)
                {
                    TicketInfo.Payments = new List<TicketPaymentItemInfo>();

                    TicketInfo.Payments.Add(new TicketPaymentItemInfo()
                    {
                        Amount = TicketInfo.Price,
                        IDPaymentMethod = PaymentMethods[IDPaymentMethodIndex].IDPaymentMethod,
                        Description = PaymentMethods[IDPaymentMethodIndex].Description
                    });

                    if (IDDiscountIndex >= 0)
                    {
                        TicketInfo.Payments.Add(new TicketPaymentItemInfo()
                        {
                            Amount = TicketInfo.DiscountValue,
                            IDDiscount = Discounts[IDDiscountIndex].IdDiscount,
                            Description = Discounts[IDDiscountIndex].Description
                        });
                    }
                }

                if (TicketInfo.PriceHistory == null)
                {
                    TicketInfo.PriceHistory = new List<TicketHistoryPriceInfo>();
                }

                
                var response = appPayment.ProcessPayment(new ProcessPaymentRequest()
                {
                    CNPJ = this.TicketInfo.Cnpj,
                    CPF = this.TicketInfo.Cpf,
                    DatePayment = this.TicketInfo.DatePriceScheduller.HasValue ? this.TicketInfo.DatePriceScheduller.Value : DateTime.Now,
                    DateBillingLimit = this.TicketInfo.DateBillingLimit,
                    IDCashTransaction = AppContextGeneral.cashierInfo.CashTransactionId,
                    IDDevice = AppContextGeneral.deviceInfo.IDDevice,
                    IDPriceTable = this.PriceTables[IDPriceTableIndex].IdPriceTable,
                    IDUser = AppContextGeneral.userInfo.IdUser,
                    ParkingCode = AppContextGeneral.parkingInfo.ParkingCode,
                    TicketNumber = this.TicketInfo.Ticket,
                    DateExit = dateExit,
                    Payments = (from l in this.TicketInfo.Payments
                                select new PaymentItemInfo()
                                {
                                    Amount = l.Amount,
                                    AuthorizationCode = l.AuthorizationCode,
                                    Brand = l.Brand,
                                    CardNumberTruncated = l.CardNumberTruncated,
                                    IDDiscount = l.IDDiscount,
                                    IDParkingSeal = l.IDParkingSeal,
                                    IDPayment = l.IDPayment,
                                    IDPaymentMethod = l.IDPaymentMethod,
                                    Nsu = l.Nsu,
                                    NsuHost = l.NsuHost,
                                    PriceTableValue = l.PriceTableValue,
                                    SealCode = l.SealCode,
                                    SealCodeRead = l.SealCodeRead,
                                    SealNumber = l.SealNumber,
                                    SealTypeAccess = l.SealTypeAccess,
                                    SealValue = l.SealValue,
                                    IDBrand = l.IDBrand
                                }).ToList()
                });

                if (response.Success)
                {
                    var print = Xamarin.Forms.DependencyService.Get<IPrinterService>();

                    print.PrintPaymentReceipt(new DependencyService.Model.PrintTicketInfoModel()
                    {
                        DateEntry = TicketInfo.DateEntry,
                        Plate = TicketInfo.Plate,
                        Prism = TicketInfo.Prism,
                        TicketNumber = TicketInfo.Ticket,
                        VehicleColor = TicketInfo.VehicleColor,
                        VehicleModel = TicketInfo.VehicleModel,
                        DatePayment = response.Data.DatePayment.Value,
                        DateLimitExit = response.Data.DateLimitExit.Value,
                        Payments = (from l in TicketInfo.Payments select
                            new PrintPaymentInfoModel()
                            {
                                PaymentMethod = l.Description,
                                Amount = l.Amount.Value
                            }
                        ).ToList(),
                        Amount = (from l in TicketInfo.Payments select l.Amount.Value).Sum()
                    });

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        UserDialogs.Instance.HideLoading();
                    });
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        UserDialogs.Instance.HideLoading();

                        Application.Current.MainPage.DisplayAlert("Erro", response.Message, "Ok");
                    });
                }
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    UserDialogs.Instance.HideLoading();

                    Application.Current.MainPage.DisplayAlert("Erro ex", ex.StackTrace, "Ok");
                });
            }
        }

        private void UpdateCalculatedPrice()
        {
            GetTicketPrice();
        }

        private void ResetScreen()
        {
            Application.Current.MainPage = new MenuPage();
        }

        private void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    
}
