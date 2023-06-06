using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Acr.UserDialogs;
using Plugin.FirebasePushNotification;
using System.Collections.Generic;

namespace SzybkoOdziez.Droid
{
    [Activity(Label = "SzybkoOdziez", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private bool IsNotification = false;
        private IDictionary<string, object> NotificationData;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            //base.OnCreate(savedInstanceState);

            //Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            //global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            //UserDialogs.Init(this);
            //if (!IsNotification)
            //    LoadApplication(new App());
            //FirebasePushNotificationManager.ProcessIntent(this, Intent);

            //CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            //{
            //    System.Diagnostics.Debug.WriteLine("NOTIFICATION Opened", p.Data);
            //    NotificationData = p.Data;
            //    IsNotification = true;
            //    LoadApplication(new App(IsNotification, NotificationData));
            //};

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            UserDialogs.Init(this);

            FirebasePushNotificationManager.ProcessIntent(this, Intent);

            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

    }
}