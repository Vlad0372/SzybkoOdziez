using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using SzybkoOdziez.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SzybkoOdziez
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public ObservableCollection<string> Items { get; set; }

        public Login()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Rejestracja());
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (txtUZYTKOWNIK.Text == "admin" && txtHASLO.Text == "admin")
            {
                Navigation.PushAsync(new Strona_glowna());
            }
            else
            {
                DisplayAlert("UPS...!", "Podałeś złe hasło albo nazwę uztkownika", "Spróbuj ponownie");
            }
        }

            async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
