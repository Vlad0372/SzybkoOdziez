using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SzybkoOdziez.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Android.Provider.ContactsContract.CommonDataKinds;
using static Java.Util.Jar.Attributes;

namespace SzybkoOdziez.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            string connStr = "Data Source=(DESCRIPTION=" +
           "(ADDRESS=(PROTOCOL=TCP)(HOST=217.173.198.135)(PORT=1521))" +
           "(CONNECT_DATA=(SERVICE_NAME=tpdb)));" + "" +
           "User Id=s100824;Password=Sddb2023;";

            using (OracleConnection conn = new OracleConnection(connStr))
            {
                conn.Open();

                OracleCommand command = new OracleCommand();

                command.Connection = conn;
                command.CommandText = "select user_id from \"user\" where nickname = '" + userLogin.Text + "' and password = '" + userPass.Text + "'";
                command.CommandType = System.Data.CommandType.Text;
               
                object isUserExists = command.ExecuteScalar();

                if (isUserExists != null)
                {
                    var app = (App)Application.Current;
                    app.guestMode = false;
                    app.userId = Convert.ToInt32(isUserExists);
                    await Shell.Current.GoToAsync("//MainPage");
                    Navigation.RemovePage(this);
                }
                else
                {
                    DisplayAlert("UPS...!", "Podałeś złe hasło albo nazwę użytkownika", "Spróbuj ponownie");
                }
                conn.Close();
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegistrationPage());
            Navigation.RemovePage(this);
        }

        private void ContinueAsGuestTapped(object sender, EventArgs e)
        {
            var app = (App)Application.Current;
            app.guestMode = true;
            Navigation.PushAsync(new MainPage());
            Navigation.RemovePage(this);
        }
    }
}