using System;
using Android.Content;
using Android.Net;
using Android.Widget;

namespace Xamarin.core
{
    public class VerificarConexion
    {
        private readonly Context context;
        public VerificarConexion(Context context)
        {
            this.context = context;
        }

        public Boolean IsOnline()
        {
            ConnectivityManager cm = (ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
            NetworkInfo netInfo = cm.ActiveNetworkInfo;
            if (netInfo != null && netInfo.IsConnectedOrConnecting)
            {
                
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}