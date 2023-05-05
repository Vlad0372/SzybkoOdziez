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
using Oracle.ManagedDataAccess.Client;
using System.Data.SqlClient;
using static Android.Provider.ContactsContract.CommonDataKinds;

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
            //string ConnectionString = "Data Source=(DESCRIPTION=" +
            //"(ADDRESS=(PROTOCOL=TCP)(HOST=217.173.198.135)(PORT=1521))" +
            //"(CONNECT_DATA=(SERVICE_NAME=tpdb)));" + "" +
            //"User Id=s100824;Password=Sddb2023;";
            //OracleConnection connection = new OracleConnection(ConnectionString);
            //connection.Open();

            
            if (string.IsNullOrEmpty(name.Text)|| string.IsNullOrEmpty(last_name.Text) || string.IsNullOrEmpty(mail.Text) || string.IsNullOrEmpty(nickname.Text) || string.IsNullOrEmpty(password.Text))
            {

                
                DisplayAlert("UPS...!", "Musisz podać wszystkie dane", "Spróbuj ponownie");

            }
            else
            {
                //var query = "INSERT INTO user (name, last_name, mail, nickname, password) VALUES (@name, @last_name, @mail, @nickname, @password)";

                //OracleCommand command = new OracleCommand(query, connection);

                //command.Parameters.Add("@name", name);
                //command.Parameters.Add("@last_name", last_name);
                //command.Parameters.Add("@mail", mail);
                //command.Parameters.Add("@nickname", nickname);
                //command.Parameters.Add("@password", password);

                //command.ExecuteNonQuery();
                //connection.Close();
                await Shell.Current.GoToAsync("//MainPage");
                Navigation.RemovePage(this);
            }

           
        }
    }
}