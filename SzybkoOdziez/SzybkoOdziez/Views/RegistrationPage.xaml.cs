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
using Java.Nio.FileNio.Attributes;
using System.Data;

namespace SzybkoOdziez.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage()
        {
            InitializeComponent();
           
        }
        private int PobierzOstatnieID(OracleConnection connection)
        {
            string sqlQuery = "SELECT MAX(user_id) FROM \"user\"";

            using (OracleCommand command = new OracleCommand(sqlQuery, connection))
            {
                object result = command.ExecuteScalar();

                if (result != DBNull.Value && result != null)
                {
                    return Convert.ToInt32(result);
                }
                else
                {
                    // Jeśli nie ma żadnych rekordów, zwróć wartość początkową (np. 1)
                    return 0;
                }
            }
        }

        private async void create_account_Button_Clicked(object sender, EventArgs e)
        {

            string ConnectionString = "Data Source=(DESCRIPTION=" +
           "(ADDRESS=(PROTOCOL=TCP)(HOST=217.173.198.135)(PORT=1521))" +
           "(CONNECT_DATA=(SERVICE_NAME=tpdb)));" + "" +
           "User Id=s100824;Password=Sddb2023;";
            OracleConnection connection = new OracleConnection(ConnectionString);
            connection.Open();



            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {




                var query = "INSERT INTO \"user\" (user_id, name, last_name, mail, nickname, password) VALUES (:user_id, :name, :last_name, :mail, :nickname, :password)";

                using (OracleCommand cmdInsert = new OracleCommand(query, conn))
                {
                    int ostatnieID = PobierzOstatnieID(connection);
                    int noweID = ostatnieID + 1;
                    //int userrr = int.Parse(user_id.Text);
                    string nameee = name.Text;
                    string last_nameee = last_name.Text;
                    string maillll = mail.Text;
                    string nicknameee = nickname.Text;
                    string passworddd = password.Text;


                    OracleCommand command = new OracleCommand(query, connection);
                    command.Parameters.Add(new OracleParameter("@user_id", noweID));
                    command.Parameters.Add(new OracleParameter("name", nameee));
                    command.Parameters.Add(new OracleParameter("last_name", last_nameee));
                    command.Parameters.Add(new OracleParameter("mail", maillll));
                    command.Parameters.Add(new OracleParameter("nickname", nicknameee));
                    command.Parameters.Add(new OracleParameter("password", passworddd));

                    //NIC SIĘ NIE DZIEJE xD
                    //if (user_id.Text == "" && name.Text == "" && last_name.Text == "" && mail.Text == "" && nickname.Text == "" && password.Text == "" )
                    //{
                    //     DisplayAlert("UPS", "Proszę wpisać wszystkie wymagane dane", "Spróbuj ponownie!");

                    //}



                    try
                    {
                        conn.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        conn.Close();

                        if (rowsAffected > 0)
                        {

                            await Shell.Current.GoToAsync("//MainPage");
                            Navigation.RemovePage(this);
                        }
                        else
                        {
                            //NIC SIĘ NIE DZIEJE xD
                            // nic nie zostało dodane do bazy - wyświetl komunikat o błędzie
                            DisplayAlert("UPS", "Nie udało się zarejestrować", "Spróbuj ponownie!");
                        }
                    }
                    catch (OracleException ex)
                    {
                        // wystąpił błąd Oracle - wyświetl komunikat o błędzie
                        DisplayAlert("UPS", "Nie udało się zarejestrować, użytkownik zajął twoją ulubioną liczbnę!", "Spróbuj ponownie", ex.Message);

                    }
                }
            }
        }    
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PrivacyPolicyPage());
        }

    }
}