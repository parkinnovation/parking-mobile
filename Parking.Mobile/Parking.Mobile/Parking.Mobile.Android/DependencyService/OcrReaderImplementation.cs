using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Xamarin.Forms;
using Parking.Mobile.Droid;
using Parking.Mobile.DependencyService.Interfaces;

[assembly: Dependency(typeof(OcrReaderImplementation))]
namespace Parking.Mobile.Droid
{
    public class OcrReaderImplementation : Java.Lang.Object, IOcrReader
    {
        public Task<string> ReadPlateAsync(byte[] imageBytes)
        {
            // Cria o Intent para abrir a Activity de câmera
            var activityClass = Java.Lang.Class.FromType(typeof(CameraOcrActivity));
            var intent = new Intent(Android.App.Application.Context, activityClass);
            intent.SetFlags(ActivityFlags.NewTask);

            // Inicia a Activity
            Android.App.Application.Context.StartActivity(intent);

            // Aguarda o resultado (a placa detectada)
            return CameraOcrActivity.WaitForResult();
        }
    }
}
