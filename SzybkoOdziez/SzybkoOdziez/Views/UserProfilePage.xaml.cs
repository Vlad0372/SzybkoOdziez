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
    public partial class UserProfilePage : ContentPage
    {
        public UserProfilePage()
        {
            InitializeComponent();



            // OGARNĄĆ ŻEBY INNY UŻYTKOWNIK ZOBACZYŁ SWOJE DANE!!!!!
            // int userId = GetLoggedInUserId();


            int userId = 1;

            string ConnectionString = "Data Source=(DESCRIPTION=" +
               "(ADDRESS=(PROTOCOL=TCP)(HOST=217.173.198.135)(PORT=1521))" +
               "(CONNECT_DATA=(SERVICE_NAME=tpdb)));" + "" +
               "User Id=s100824;Password=Sddb2023;";
            OracleConnection connection = new OracleConnection(ConnectionString);
            connection.Open();
            
            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                conn.Open();

                var query = "SELECT name, last_name, mail, nickname, password from \"user\" where user_id = :id";

                
                var param = new OracleParameter(":id", OracleDbType.Int32);
                param.Value = userId;
                using (var cmd = new OracleCommand(query, conn))
    {
                    cmd.Parameters.Add(param);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                           

                            string Namee = reader.GetString(0);
                            string LastNamee = reader.GetString(1);
                            string Maill = reader.GetString(2);
                            string Nicknamee = reader.GetString(3);
                            string Passwordd = reader.GetString(4);

                            // przypisanie imienia i nazwiska użytkownika do kontrolki Label
                            labelName.Text = Namee;
                            labelLastName.Text = LastNamee;
                            labelMail.Text = Maill;
                            labelNickname.Text = Nicknamee;
                            labelPassword.Text = Passwordd;
                        }
                    }
                }
            }
        }

    

        
            

        

        
    }
}