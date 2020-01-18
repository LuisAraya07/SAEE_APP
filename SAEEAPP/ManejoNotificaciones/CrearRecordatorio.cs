using Android.App;
using Android.Content;
using Android.Media;
using Android.Support.V4.App;
using Android.Util;
using Android.Widget;
using Newtonsoft.Json;

namespace SAEEAPP.ManejoNotificaciones
{
    [BroadcastReceiver(Enabled = true)]
    public class CrearRecordatorio : BroadcastReceiver
    {
        public const string URGENT_CHANNEL = "com.xamarin.myapp.urgent";
        public const int NOTIFY_ID = 1100;
        public override void OnReceive(Context context, Intent intent)
        {
            
            var message = intent.GetStringExtra("Message");
            var title = intent.GetStringExtra("Title");
            var id = intent.GetIntExtra("Id",0);
            var importance = NotificationImportance.High;
            NotificationChannel chan = new NotificationChannel(URGENT_CHANNEL, "Urgente", importance);
            chan.EnableVibration(true);
            chan.LockscreenVisibility = NotificationVisibility.Public;
            var resultIntent = new Intent(context, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(context, id, resultIntent, PendingIntentFlags.UpdateCurrent);


            //Intent newIntent = new Intent(context, typeof(ReminderContent));
            //    newIntent.PutExtra("reminder", JsonConvert.SerializeObject(reminder));

            //    Android.Support.V4.App.TaskStackBuilder stackBuilder = Android.Support.V4.App.TaskStackBuilder.Create(context);
            //    stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(ReminderContent)));
            //    stackBuilder.AddNextIntent(newIntent);

                //PendingIntent resultPendingIntent = stackBuilder.GetPendingIntent(0, (int)PendingIntentFlags.UpdateCurrent);

                var builder = new NotificationCompat.Builder(context)
                    .SetSmallIcon(Resource.Mipmap.ic_launcher)
                    .SetContentTitle(title)
                    .SetContentText(message)
                    .SetContentIntent(pendingIntent)
                    .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                    .SetAutoCancel(true)
                    .SetChannelId(URGENT_CHANNEL);
            NotificationManager notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
            notificationManager.CreateNotificationChannel(chan);
            notificationManager.Notify(NOTIFY_ID, builder.Build());
         
        }
    }
}
