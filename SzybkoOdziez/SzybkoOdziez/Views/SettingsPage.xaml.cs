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

namespace SzybkoOdziez.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            // OGARNĄĆ ŻEBY INNY UŻYTKOWNIK ZOBACZYŁ SWOJE DANE!!!!!
            string Namee = "";
            string LastNamee = "";
            string Maill = "";
            string Nicknamee = "";
            string Passwordd = "";
            int userId = GetLoggedInUserID("ada", "ada");



            string ConnectionString = "Data Source=(DESCRIPTION=" +
               "(ADDRESS=(PROTOCOL=TCP)(HOST=217.173.198.135)(PORT=1521))" +
               "(CONNECT_DATA=(SERVICE_NAME=tpdb)));" + "" +
               "User Id=s100824;Password=Sddb2023;";
            OracleConnection connection = new OracleConnection(ConnectionString);
            connection.Open();

            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                conn.Open();

                var query = "SELECT name, last_name, mail, nickname from \"user\" where user_id = :id";


                var param = new OracleParameter(":id", OracleDbType.Int32);
                //param.Value = userId;
                param.Value = 99;
                using (var cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(param);

                    using (var reader = cmd.ExecuteReader())
                    {


                        if (reader.Read())
                        {


                            Namee = reader.GetString(0);
                            LastNamee = reader.GetString(1);
                            Maill = reader.GetString(2);
                            Nicknamee = reader.GetString(3);


                            // przypisanie imienia i nazwiska użytkownika do kontrolki Label


                        }
                        reader.Close();
                        
                    }
                }
            }



        }

        public int GetLoggedInUserID(string nicknamee, string passwordd)
        {
            string ConnectionString = "Data Source=(DESCRIPTION=" +
               "(ADDRESS=(PROTOCOL=TCP)(HOST=217.173.198.135)(PORT=1521))" +
               "(CONNECT_DATA=(SERVICE_NAME=tpdb)));" + "" +
               "User Id=s100824;Password=Sddb2023;";
            int userId = -1;
            using (OracleConnection connection = new OracleConnection(ConnectionString))
            {
                connection.Open();
                var query = "SELECT user_id from \"user\" where nickname = :nickname AND password = :password";
                OracleCommand command = new OracleCommand(query, connection);
                command.Parameters.Add("nickname", nicknamee);
                command.Parameters.Add("password", passwordd);
                OracleDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    userId = reader.GetInt32(0);
                }

                reader.Close();

                return userId;
            }
        }







    }
}