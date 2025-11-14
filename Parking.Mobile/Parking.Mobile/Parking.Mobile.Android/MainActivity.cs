using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Provider;
using ZXing.Mobile;
using Acr.UserDialogs;
using Parking.Mobile.Common;

namespace Parking.Mobile.Droid
{
    [Activity(
        Label = "LockiD",
        Icon = "@mipmap/icon",
        Theme = "@style/MainTheme",
        MainLauncher = false,
        ConfigurationChanges =
            ConfigChanges.ScreenSize |
            ConfigChanges.Orientation |
            ConfigChanges.UiMode |
            ConfigChanges.ScreenLayout |
            ConfigChanges.SmallestScreenSize |
            ConfigChanges.Keyboard |
            ConfigChanges.KeyboardHidden |
            ConfigChanges.Navigation |
            ConfigChanges.Density
    )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static MainActivity Instance { get; private set; }

        // GARANTE que o serviço só é iniciado UMA VEZ
        public static bool KeepAliveStarted = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Instance = this;

            MobileBarcodeScanner.Initialize(this.Application);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            UserDialogs.Init(this);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            AppContextGeneral.Version = typeof(MainActivity).Assembly.GetName().Version.ToString();

            LoadApplication(new App());
        }

        protected override void OnStart()
        {
            base.OnStart();

            // EVITA MÚLTIPLA INICIALIZAÇÃO (que trava SignalR)
            if (!KeepAliveStarted)
            {
                KeepAliveStarted = true;

                // Opcional: desativar otimização de bateria
                RequestIgnoreBatteryOptimizations();

                //StartKeepAliveService();
            }
        }

        // Permite ignorar otimização de bateria (Doze Mode)
        private void RequestIgnoreBatteryOptimizations()
        {
            try
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
                {
                    var pm = (PowerManager)GetSystemService(PowerService);

                    if (!pm.IsIgnoringBatteryOptimizations(PackageName))
                    {
                        var intent = new Intent(Settings.ActionRequestIgnoreBatteryOptimizations);
                        intent.SetData(Android.Net.Uri.Parse("package:" + PackageName));
                        StartActivity(intent);
                    }
                }
            }
            catch (Exception) { }
        }

        // Inicia o Foreground Service para manter app vivo
        private void StartKeepAliveService()
        {
            try
            {
                var intent = new Intent(this, typeof(KeepAliveService));

                if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                    StartForegroundService(intent);
                else
                    StartService(intent);
            }
            catch (Exception) { }
        }

        public override void OnRequestPermissionsResult(
            int requestCode,
            string[] permissions,
            [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
