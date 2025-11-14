using Android.App;
using Android.Content;
using Android.OS;

namespace Parking.Mobile.Droid
{
    [Service(Exported = false)]
    public class KeepAliveService : Service
    {
        public override void OnCreate()
        {
            base.OnCreate();
            CreateNotificationChannel();
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            // OBRIGATÓRIO: chamar StartForeground aqui dentro
            var notification = new Notification.Builder(this, "keepalive_channel")
                .SetContentTitle("LockiD em execução")
                .SetContentText("Mantendo o app ativo")
                .SetSmallIcon(Resource.Mipmap.icon)
                .Build();

            StartForeground(1, notification);

            return StartCommandResult.Sticky;
        }

        public override IBinder OnBind(Intent intent) => null;

        private void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel(
                    "keepalive_channel",
                    "LockiD KeepAlive",
                    NotificationImportance.Low
                );

                var manager = (NotificationManager)GetSystemService(NotificationService);
                manager.CreateNotificationChannel(channel);
            }
        }
    }
}
