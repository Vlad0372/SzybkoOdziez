using Android.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using SzybkoOdziez.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Android.Graphics.ColorSpace;
using static Java.Util.Jar.Attributes;

namespace SzybkoOdziez.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage()
        {
            InitializeComponent();
           
        }

        private async void create_account_Button_Clicked(object sender, EventArgs e)
        {



            if (string.IsNullOrEmpty(area_Name.Text)|| string.IsNullOrEmpty(area_Surname.Text) || string.IsNullOrEmpty(area_Name_user.Text) || string.IsNullOrEmpty(area_PASSWORD.Text) || string.IsNullOrEmpty(area_E_MAIL.Text))
            {

                
                DisplayAlert("UPS...!", "Musisz podać wszystkie dane", "Spróbuj ponownie");

            }
            else
            {
                await Shell.Current.GoToAsync("//MainPage");
                Navigation.RemovePage(this);
            }

           
        }
    }
}