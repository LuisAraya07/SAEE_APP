using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Database;
using Android.OS;
using Android.Provider;
using Java.Util;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using CursorLoader = Android.Support.V4.Content.CursorLoader;

namespace Xamarin.core.Services
{
    public class CalendarioServices
    {
        private int defaultCalendarId;
        private static Activity _context;

        private string[] calendarProjection = {
            CalendarContract.Calendars.InterfaceConsts.Id,
            CalendarContract.Calendars.InterfaceConsts.CalendarDisplayName,
            CalendarContract.Calendars.InterfaceConsts.AccountName
        };
        private string[] eventProjection =
        {
            CalendarContract.Events.InterfaceConsts.Id
        };
        public CalendarioServices(Activity context)
        {
            _context = context;
        }

        public Task<bool> CheckIfAlarmAlreadyExists(string id)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            var alarmUri = ContentUris.AppendId(CalendarContract.Events.ContentUri.BuildUpon(), long.Parse(id));
            var cursor = Application.Context.ContentResolver.Query(alarmUri.Build(), eventProjection, null, null,null);
            if (cursor.Count == 0)
                tcs.SetResult(false);
            else
                tcs.SetResult(true);
            return tcs.Task;
        }

        [Obsolete]
        public Task<string> CreateAlarmAsync(string title,string description,DateTime timeInit,DateTime timeEnd,int alarmMinutes)
        {
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
            GetSystemCalendar();
            if(defaultCalendarId == 0)
            {
                tcs.SetResult(string.Empty);
                return tcs.Task;
            }
            ContentValues eventValues = new ContentValues();
            eventValues.Put(CalendarContract.Events.InterfaceConsts.CalendarId, defaultCalendarId);
            eventValues.Put(CalendarContract.Events.InterfaceConsts.Title, title);
            eventValues.Put(CalendarContract.Events.InterfaceConsts.Description, description);
            eventValues.Put(CalendarContract.Events.InterfaceConsts.Dtstart, GetDateTimeMS(timeInit));
            eventValues.Put(CalendarContract.Events.InterfaceConsts.Dtend, GetDateTimeMS(timeEnd));
            eventValues.Put(CalendarContract.Events.InterfaceConsts.EventTimezone, System.TimeZone.CurrentTimeZone.StandardName);
            eventValues.Put(CalendarContract.Events.InterfaceConsts.EventEndTimezone, System.TimeZone.CurrentTimeZone.StandardName);
            var uri = Application.Context.ContentResolver.Insert(CalendarContract.Events.ContentUri, eventValues);
            if (!long.TryParse(uri.LastPathSegment, out long eventID))
                tcs.SetResult(string.Empty);
            else
            {
                ContentValues reminderValues = new ContentValues();
                reminderValues.Put(CalendarContract.Reminders.InterfaceConsts.Minutes, alarmMinutes);
                reminderValues.Put(CalendarContract.Reminders.InterfaceConsts.EventId, eventID);
                reminderValues.Put(CalendarContract.Reminders.InterfaceConsts.Method, (int)RemindersMethod.Alarm);
                uri = Application.Context.ContentResolver.Insert(CalendarContract.Reminders.ContentUri, reminderValues);
                tcs.SetResult(eventID.ToString());

            }
            return tcs.Task;

        }

        [Obsolete]
        long GetDateTimeMS(DateTime date)
        {
            Calendar calendar = Calendar.GetInstance(Java.Util.TimeZone.GetTimeZone(System.TimeZone.CurrentTimeZone.StandardName));
            calendar.Set(CalendarField.DayOfMonth, date.Day);
            calendar.Set(CalendarField.Month, date.Month - 1);
            calendar.Set(CalendarField.Year, date.Year);
            calendar.Set(CalendarField.HourOfDay, date.Hour);
            calendar.Set(CalendarField.Minute, date.Minute);
            return calendar.TimeInMillis;
        }
        private void GetSystemCalendar()
        {
            var calendarUri = CalendarContract.Calendars.ContentUri;
            var loader = new CursorLoader(Application.Context, calendarUri, calendarProjection, null, null, null);
            var cursor = (ICursor)loader.LoadInBackground();
            cursor.MoveToLast();
            defaultCalendarId = cursor.GetInt(cursor.GetColumnIndex(calendarProjection[0]));
        }

        public Task<bool> DeleteAlarmAsync(string id)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            var alarmUri = ContentUris.AppendId(CalendarContract.Events.ContentUri.BuildUpon(), long.Parse(id));
            var cursor = Application.Context.ContentResolver.Delete(alarmUri.Build(),null, null);
            if (cursor == 0)
                tcs.SetResult(false);
            else
                tcs.SetResult(true);
            return tcs.Task;
        }

        /*       private static async void RequestAppPermissions()
               {
                   if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
                       return;
                   await GetPermissionsAsync();
                   ActivityCompat.RequestPermissions(_context, new string[]
                       {
                           Manifest.Permission.WriteCalendar,
                           Manifest.Permission.ReadCalendar

                       }, 1000);
               }
               async Task GetPermissionsAsync()
               {
                   if (HasPermissions())
                       return;
                   else
                   {
                       AlertDialog.Builder alert = new AlertDialog.Builder(_context);
                       alert.SetTitle("Permisos Requerios");
                       alert.SetMessage("Esta aplicación necesita permisos para continuar");
                       alert.SetPositiveButton("Solicitar Permisos", (senderAlert, args) => {
                           RequestPermissions(PermissionsGroupLocation, RequestedLocationId);
                       });
                       alert.SetNegativeButton("Cancelar", (senderAlert, args) => {
                           Toast.MakeText(_context, "Cancelado", ToastLength.Short).Show();
                       });
                       Dialog dialog = alert.Create();
                       dialog.Show();
                       return;
                   }
               }
               private static bool HasPermissions()
               {
                   return (ContextCompat.CheckSelfPermission(Application.Context, Manifest.Permission.WriteCalendar) == Permission.Granted &&
                       ContextCompat.CheckSelfPermission(Application.Context, Manifest.Permission.ReadCalendar) == Permission.Granted);
               }*/
    }
}