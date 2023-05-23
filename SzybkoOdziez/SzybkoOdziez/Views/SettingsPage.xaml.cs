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
        public SettingsPage()
        {
            InitializeComponent();
            SwitchTheme();
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
    }
}