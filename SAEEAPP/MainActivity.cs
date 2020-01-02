using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Widget;
using System;

namespace SAEEAPP
{
    [Activity(Label = "@string/app_name", MainLauncher = true, Theme = "@style/AppTheme")]
    public class MainActivity : AppCompatActivity
    {
        private EditText etCuenta;
        private EditText etContrasenia;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            Button btnIngresar = FindViewById<Button>(Resource.Id.btAceptar);
            btnIngresar.Click += OnClick_Ingresar;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void OnClick_Ingresar(object sender, EventArgs e)
        {
            etCuenta = FindViewById<EditText>(Resource.Id.txtCuenta);
            etContrasenia = FindViewById<EditText>(Resource.Id.etContraseniaLogin);
            string cuenta = etCuenta.Text.ToString();
            string contrasenia = etContrasenia.Text.ToString();
            Intent siguiente = new Intent(this, typeof(MenuActivity));
            StartActivity(siguiente);
        }
    }
}