using Java.Nio.Channels;
using Java.Util;
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
using static Java.Text.Normalizer;

namespace SzybkoOdziez.ViewModels
{
    public class WatchlistViewModel : BaseViewModel
    {
        //private Product _selectedProduct;

        public ObservableCollection<Product> Products { get; }
        public ObservableCollection<Product> FilteredProducts { get; }
        public Command LoadProductsCommand { get; }
        //public Command<Product> RemoveProductCommand { get; }

        public WatchlistViewModel()
        {

            Products = new ObservableCollection<Product>();
            FilteredProducts = new ObservableCollection<Product>();
            LoadProductsCommand = new Command(async () => await LoadWishlistAsync());
            //RemoveProductCommand = new Command(async () => await RemoveProductWishlistAsync(_selectedProduct));
        }

        public WatchlistViewModel(ObservableCollection<Product> products)
        {
            Products = products;
        }
        public async void FilterProducts(string category)
        {
            FilteredProducts.Clear();
            if (category == "Wszystkie")
            {
                foreach(var product in Products)
                {
                    FilteredProducts.Add(product);
                }
            }
            else
            {
                foreach (var product in Products)
                {
                    if (product.Category == category)
                    {
                        FilteredProducts.Add(product);
                    }
                }
            }
        }
        public async void OnWishlistOpen()
        {
            await LoadWishlistAsync();
        }

        async Task LoadWishlistAsync()
        {
            Products.Clear();
            var app = (App)Application.Current;
            var wishlistDataStore = app.wishlistDataStore;
            var wishlistIEnumerable = await wishlistDataStore.GetItemsAsync();
            foreach (var product in wishlistIEnumerable)
            {
                Products.Add(product);
            }
        }

        public void InitializeWishlistFromDB(int user_id)
        {
            Products.Clear();
            string ConnectionString = "Data Source=(DESCRIPTION=" +
            "(ADDRESS=(PROTOCOL=TCP)(HOST=217.173.198.135)(PORT=1521))" +
            "(CONNECT_DATA=(SERVICE_NAME=tpdb)));" + "" +
            "User Id=s100824;Password=Sddb2023;";

            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                var querySelectIDs = "SELECT * FROM observed WHERE(user_user_id = :user_id)";
                List<int> item_ids = new List<int>();
                using (OracleCommand cmdInsert = new OracleCommand(querySelectIDs, conn))
                {
                    OracleCommand command = new OracleCommand(querySelectIDs, conn);
                    command.Parameters.Add(new OracleParameter("user_id", user_id));

                    try
                    {
                        conn.Open();
                        OracleDataReader reader = null;
                        reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                item_ids.Add(reader.GetInt32(1));
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
                var querySelectItems = "SELECT item_id, name, description, price, img_source, category FROM item WHERE (item_id = :item_id)";
                using (OracleCommand cmdInsert = new OracleCommand(querySelectItems, conn))
                {
                    foreach (var item_id in item_ids)
                    {
                        OracleCommand command = new OracleCommand(querySelectItems, conn);
                        command.Parameters.Add(new OracleParameter("item_id", item_id));
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
                                    Description = reader.GetString(2),
                                    Price = reader.GetInt32(3),
                                    ImageUrl = reader.GetString(4),
                                    Category = reader.GetString(5),
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


                var app = (App)Application.Current;
                var wishlistDataStore = app.wishlistDataStore;
                foreach (var item in Products)
                {
                    if (wishlistDataStore.CheckInDataStore(item))
                    {
                        wishlistDataStore.AddItemAsync(item);
                    }
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

        //public Product SelectedProduct
        //{
        //    get => _selectedProduct;
        //    set
        //    {
        //        SetProperty(ref _selectedProduct, value);
        //        OnItemSelected(value);
        //    }
        //}

        //async Task RemoveProductWishlistAsync(Product product)
        //{
        //    var app = (App)Application.Current;
        //    var wishlistDataStore = app.wishlistDataStore;
        //    await wishlistDataStore.DeleteItemAsync(product.Id);
        //}


    }
}