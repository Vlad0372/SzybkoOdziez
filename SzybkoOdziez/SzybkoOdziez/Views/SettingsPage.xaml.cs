using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Android.Media.TV;
using Acr.UserDialogs;

namespace SzybkoOdziez.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        INotificationManager notificationManager;
        int notificationNumber = 0;
        public SettingsPage()
        {
            InitializeComponent();
            SwitchTheme();

            notificationManager = DependencyService.Get<INotificationManager>();
            notificationManager.NotificationReceived += (sender, eventArgs) =>
            {
                var evtData = (NotificationEventArgs)eventArgs;
                ShowNotification(evtData.Title, evtData.Message);
            };
        }

        public void EnableNightMode_Clicked(object sender, EventArgs e)
        {
            SwitchTheme();

            if (Application.Current.UserAppTheme == OSAppTheme.Light)
            {
                UserDialogs.Instance.Toast("Tryb nocny włączony pomyślnie!", TimeSpan.FromSeconds(2));
            }
            else
            {
                UserDialogs.Instance.Toast("Tryb nocny wyłączony pomyślnie!", TimeSpan.FromSeconds(2));
            }
        }

        private void SwitchTheme()
        {
            if (Application.Current.UserAppTheme == OSAppTheme.Light)
            {
                Application.Current.UserAppTheme = OSAppTheme.Dark;

                nightModeLabel.Text = "Tryb nocny (włączono)";
                nightModeBtn.Text = "Wyłącz";
            }
            else
            {
                Application.Current.UserAppTheme = OSAppTheme.Light;

                nightModeLabel.Text = "Tryb nocny (wyłączono)";
                nightModeBtn.Text = "Włącz";
            }
        }

        void SendNotification_Clicked(object sender, EventArgs e)
        {
            notificationNumber++;
            string title = $"Local Notification #{notificationNumber}";
            string message = $"You have now received {notificationNumber} notifications!";
            notificationManager.SendNotification(title, message);
        }

        void SendScheduledNotification_Clicked(object sender, EventArgs e)
        {
            notificationNumber++;
            string title = $"Local Notification #{notificationNumber}";
            string message = $"You have now received {notificationNumber} notifications!";
            notificationManager.SendNotification(title, message, DateTime.Now.AddSeconds(10));
        }

        void ShowNotification(string title, string message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var msg = new Label()
                {
                    Text = $"Notification Received:\nTitle: {title}\nMessage: {message}"
                };
                stackLayout.Children.Add(msg);
            });
        }
    }
}