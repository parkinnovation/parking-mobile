using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;

namespace Parking.Mobile.Droid
{
	[Activity(Label = "NetPark", Icon = "@drawable/Icon", MainLauncher = true, Theme = "@style/SplashTheme", NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        protected override async void OnResume()
        {
            base.OnResume();

            await Task.Delay(3000);

            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}

