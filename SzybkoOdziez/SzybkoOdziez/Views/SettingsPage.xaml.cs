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
            try
            {
                string title = "powiadomienie z poziomu ustawień";
                string message = "treść powiadomienia";
                notificationManager.SendNotification(title, message);
            }
            catch (Exception ex)
            {
                DisplayAlert("Powiadomienie", "Wygląda na to, że nie zezwoliłeś aplikacji na wysyłania wiadomości, "+
                    "zmień to w ustawieniach urządzenia i spróbuj ponownie, jak problem się powtórzy, zgłoś o problemie poprzez" +
                    " formularz zgłoszeniowy", "OK");
            }
        }

        void SendScheduledNotification_Clicked(object sender, EventArgs e)
        {
            try
            {
                string title = "powiadomienie z poziomu ustawień";
                string message = "treść powiadomienia";
                notificationManager.SendNotification(title, message, DateTime.Now.AddSeconds(5));
            }
            catch (Exception ex)
            {
                DisplayAlert("Powiadomienie", "Wygląda na to, że nie zezwoliłeś aplikacji na wysyłania wiadomości, " +
                    "zmień to w ustawieniach urządzenia i spróbuj ponownie, jak problem się powtórzy, zgłoś o problemie poprzez" +
                    " formularz zgłoszeniowy", "OK");
            }          
        }

        void ShowNotification(string title, string message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {              
                Shell.Current.GoToAsync($"//MainPage//NotificationPage?title={title}&message={message}");
            });
        }
    }
}