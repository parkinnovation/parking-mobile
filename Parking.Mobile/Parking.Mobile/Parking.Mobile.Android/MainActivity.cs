using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using ZXing.Mobile;
using Acr.UserDialogs;
using Parking.Mobile.Common;

namespace Parking.Mobile.Droid
{
    [Activity(Label = "NetPark", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static MainActivity Instance { get; private set; }

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
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
