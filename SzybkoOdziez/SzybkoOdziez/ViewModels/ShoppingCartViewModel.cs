using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using SzybkoOdziez.Models;
using SzybkoOdziez.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Data.SqlClient;
using Xamarin.Forms.Xaml;
using Acr.UserDialogs;
namespace SzybkoOdziez.ViewModels
{
    public class ShoppingCartViewModel : BaseViewModel
    {
        public ObservableCollection<Product> Products { get; }
        public Command LoadProductsCommand { get; }
        public ShoppingCartViewModel()
        {
            Title = "Koszyk";

            Products = new ObservableCollection<Product>();
            LoadProductsCommand = new Command(async () => await LoadShoppingCartAsync());
            //Products.Add(new Product() { Id = 1, Name = "Bluza", Description = "desc1", Price = 250, ImageUrl = "bluza.jpg" });
            //Products.Add(new Product() { Id = 2, Name = "Czapka", Description = "desc2", Price = 15, ImageUrl = "czapka.jpg" });
            //Products.Add(new Product() { Id = 3, Name = "Buty", Description = "desc3", Price = 100, ImageUrl = "buty.jpg" });
            //Products.Add(new Product() { Id = 4, Name = "Dresy", Description = "desc4", Price = 200, ImageUrl = "dresy.jpg" });
            //Products.Add(new Product() { Id = 5, Name = "Kurtka", Description = "desc5", Price = 500, ImageUrl = "kurtka.jpg" });

            //var app = (App)Application.Current;
            //foreach (var product in Products)
            //{
            //    app.shoppingCartDataStore.AddItemAsync(product);
            //}

        }

        public async void OnShoppingCartOpen()
        {
            await LoadShoppingCartAsync();
        }

        async Task LoadShoppingCartAsync()
        {
            Products.Clear();
            var app = (App)Application.Current;
            var shoppingCartDataStore = app.shoppingCartDataStore;
            var shoppingCartIEnumerable = await shoppingCartDataStore.GetItemsAsync();
            foreach (var product in shoppingCartIEnumerable)
            {
                Products.Add(product);
            }
        }
        public void InitializeShoppingCartFromDB(int user_id)
        {
            Products.Clear();
            var app = (App)Application.Current;
            user_id = app.userId;
            string ConnectionString = "Data Source=(DESCRIPTION=" +
            "(ADDRESS=(PROTOCOL=TCP)(HOST=217.173.198.135)(PORT=1521))" +
            "(CONNECT_DATA=(SERVICE_NAME=tpdb)));" + "" +
            "User Id=s100824;Password=Sddb2023;";

            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                var querySelectIDs = "SELECT * FROM shopping_cart WHERE(user_user_id = :user_id)";
                List<int> item_ids = new List<int>();
                List<string> item_sizes = new List<string>();
                //Dictionary<int, string> item_id_size = new Dictionary<int, string>();
                List<Tuple<int, string>> item_id_size = new List<Tuple<int, string>>();
                using (OracleCommand cmdInsert = new OracleCommand(querySelectIDs, conn))
                {
                    OracleCommand command = new OracleCommand(querySelectIDs, conn);
                    command.Parameters.Add(new OracleParameter("user_id", user_id));
                    int item_id = 0;
                    string item_size = "";
                    try
                    {
                        conn.Open();
                        OracleDataReader reader = null;
                        reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                item_id = reader.GetInt32(1);
                                item_ids.Add(reader.GetInt32(1));
                                if (reader.IsDBNull(3))
                                {
                                    item_size = "-";
                                    item_sizes.Add("-");
                                }
                                else
                                {
                                    item_size = reader.GetString(3);
                                    item_sizes.Add(reader.GetString(3));
                                }
                                //item_id_size.Add(item_id, item_size);
                                item_id_size.Add(new Tuple<int, string>(item_id, item_size));
                            }
                        }
                        else
                        {
                            return;
                        }
                        conn.Close();

                    }
                    catch (OracleException ex)
                    {
                        // wystąpił błąd Oracle - wyświetl komunikat o błędzie
                        Console.WriteLine(ex.ToString());
                        conn.Close();
                    }
                }
                var querySelectItems = "SELECT item_id, name, price, img_source FROM item WHERE (item_id = :item_id)";
                using (OracleCommand cmdInsert = new OracleCommand(querySelectItems, conn))
                {
                    foreach (Tuple<int, string> tuple in item_id_size)
                    {
                        OracleCommand command = new OracleCommand(querySelectItems, conn);
                        command.Parameters.Add(new OracleParameter("item_id", tuple.Item1));
                        try
                        {
                            conn.Open();
                            OracleDataReader reader = null;
                            reader = command.ExecuteReader();
                            if (reader.HasRows)
                            {
                                reader.Read();
                                Product sqlproduct = new Product
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Price = reader.GetDecimal(2),
                                    ImageUrl = reader.GetString(3),
                                    Size = tuple.Item2,
                                };
                                Products.Add(sqlproduct);
                            }
                            conn.Close();
                        }
                        catch (OracleException ex)
                        {
                            //blad oracle 

                            Console.WriteLine(ex.ToString());
                            conn.Close();
                        }
                    }
                }



                var shoppingCartDataStore = app.shoppingCartDataStore;
                shoppingCartDataStore.ClearAll();
                foreach(Product product in Products)
                {
                    shoppingCartDataStore.AddItemAsync(product);
                }


            }


            //OracleCommand oraCommand = new OracleCommand("SELECT fullname FROM user_profile WHERE domain_user_name = :userName");
            //oraCommand.BindByName = true;
            //oraCommand.Parameters.Add(new OracleParameter("@userName", domainUser));

            //OracleDataReader oraReader = null;
            //oraReader = oraCommand.ExecuteReader();

            //if (oraReader.HasRows)
            //{
            //    while (oraReader.Read())
            //    {
            //        fullName = oraReader.GetString(0);
            //    }
            //}
            //else
            //{
            //    return "No Rows Found";
            //}
        }

        
    }
}