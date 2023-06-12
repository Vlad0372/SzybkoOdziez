using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SzybkoOdziez.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserProfilePageChangeData : ContentPage
    {
        int user_id;
        public UserProfilePageChangeData()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var app = (App)Application.Current;
            user_id = app.userId;
        }
        
        private bool VerifyOldPassword(int user_id, string oldPassword)
        {

            string ConnectionString = "Data Source=(DESCRIPTION=" +
           "(ADDRESS=(PROTOCOL=TCP)(HOST=217.173.198.135)(PORT=1521))" +
           "(CONNECT_DATA=(SERVICE_NAME=tpdb)));" + "" +
           "User Id=s100824;Password=Sddb2023;";
            OracleConnection connection1 = new OracleConnection(ConnectionString);
            connection1.Open();

            using (OracleConnection connection = new OracleConnection(ConnectionString))
            {
                connection.Open();

                // Pobierz aktualne hasło użytkownika z bazy danych
                string selectQuery = "SELECT password FROM \"user\" WHERE user_id = :user_id";

                using (OracleCommand command = new OracleCommand(selectQuery, connection))
                {
                    command.Parameters.Add("user_id", user_id);

                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        
                        if (reader.Read())
                        {
                            string storedPassword = reader.GetString(0);

                            // Porównaj wprowadzone stare hasło z przechowywanym hasłem
                            if (storedPassword == oldPassword)
                            {
                                return true; // Stare hasło jest poprawne
                            }
                        }
                    }
                }
            }

            return false; // Stare hasło jest nieprawidłowe lub wystąpił błąd
        }
        //
        private async void button_change_data_user_2_Clicked(object sender, EventArgs e)
        {




            string old_passworddd = old_password.Text;
            int user_id = this.user_id;
            if (VerifyOldPassword(user_id, old_passworddd))
            {
                string ConnectionString = "Data Source=(DESCRIPTION=" +
               "(ADDRESS=(PROTOCOL=TCP)(HOST=217.173.198.135)(PORT=1521))" +
               "(CONNECT_DATA=(SERVICE_NAME=tpdb)));" + "" +
               "User Id=s100824;Password=Sddb2023;";
                OracleConnection connection = new OracleConnection(ConnectionString);
                connection.Open();










                //
                using (OracleConnection conn = new OracleConnection(ConnectionString))
                {




                    // var query = "INSERT INTO \"user\" (user_id, name, last_name, mail, nickname, password) VALUES (:user_id, :name, :last_name, :mail, :nickname, :password)";

                    var query = "UPDATE \"user\" SET mail = :mail, nickname = :nickname, password = :password WHERE user_id = :user_id";

                    using (OracleCommand cmdInsert = new OracleCommand(query, conn))
                    {

                        int userrr = this.user_id;

                        string maillll = mail.Text;
                        string nicknameee = nickname.Text;
                        string passworddd = password.Text;


                        OracleCommand command = new OracleCommand(query, connection);

                        command.Parameters.Add(new OracleParameter("mail", maillll));
                        command.Parameters.Add(new OracleParameter("nickname", nicknameee));
                        command.Parameters.Add(new OracleParameter("password", passworddd));
                        command.Parameters.Add(new OracleParameter("user_id", userrr));
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
            else 
            {
                DisplayAlert("UPS", "Obecne hasło zostało źle napisane!!!!", "Spróbuj ponownie!");
            }
            
        }
    }
}