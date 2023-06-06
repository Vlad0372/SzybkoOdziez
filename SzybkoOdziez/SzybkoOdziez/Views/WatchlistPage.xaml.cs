using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SzybkoOdziez.Models;
using SzybkoOdziez.Services;
using SzybkoOdziez.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Acr.UserDialogs;
using Oracle.ManagedDataAccess.Client;
using Org.BouncyCastle.Bcpg;
using Java.Lang;
using Java.IO;

namespace SzybkoOdziez.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WatchlistPage : ContentPage
    {
        private WishlistDataStore _wishlistDataStore;
        private WatchlistViewModel _viewModel;
        private string ItemCategory = "Wszystkie";

        public WatchlistPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ViewModels.WatchlistViewModel();
            
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.InitializeWishlistFromDB(99);
            _viewModel.OnWishlistOpen();
            _viewModel.FilterProducts(ItemCategory);
        }

        private async void FilterButtonClick(object sender, EventArgs e)
        {
            string tempItemCategory = ItemCategory;
            ItemCategory = await DisplayActionSheet("Filtruj", "Anuluj", null, "Wszystkie", "Bielizna", "Bluza", "Buty", "Czapka", "Koszula", "Kurtka", "Marynarka", "Paski", "Rękawice", "Spodnie", "Spódnice", "Szalik", "T-shirt");
            if(ItemCategory == "Anuluj")
            {
                ItemCategory = tempItemCategory;
            }
            else
            {
                _viewModel.FilterProducts(ItemCategory);
            }
           
        }

        private void OnWishlistProductDescriptionTapped(object sender, EventArgs e)
        {
            var tappedImage = (StackLayout)sender;
            var tappedProduct = (Product)tappedImage.BindingContext;
            ChangePageToItemDescription(tappedProduct);
        }

        private void OnWishlistProductImageTapped(object sender, EventArgs e)
        {
            var tappedElement = (Grid)sender;
            var tappedProduct = (Product)tappedElement.BindingContext;
            Navigation.PushAsync(new ItemCarouselPage(tappedProduct));
        }


        private void ChangePageToItemDescription(Product product)
        {
            Navigation.PushAsync(new ItemDescriptionPage(product));
        }

        private async void OnWishlistProductTrashcanTapped(object sender, EventArgs e)
        {
            var tappedImage = (Image)sender;
            var tappedProduct = (Product)tappedImage.BindingContext;
            //RemoveItemFromWatchlist(tappedProduct);





            var app = (App)Application.Current;
            var wishlistDataStore = app.wishlistDataStore;

            if (wishlistDataStore.CheckInDataStore(tappedProduct))
            {
                await DisplayAlert("Error!", "Item not found in data store, when trying to remove it from wishlistDataStore!", "Ok");
            }
            else
            {
                await wishlistDataStore.DeleteItemAsync(tappedProduct);
                _viewModel.OnWishlistOpen();
                _viewModel.FilteredProducts.Remove(tappedProduct);
            }



            //TappedEventArgs tappedEventArgs = (TappedEventArgs)e;
            //Product product = ((WatchlistViewModel)BindingContext).Products
            //    .FirstOrDefault(prod => prod.Id == (int)tappedEventArgs.Parameter);
            //((WatchlistViewModel)BindingContext).Products.Remove(product);

            RemoveItemFromUserObserved(app.userId, tappedProduct.Id);

        }

        private void RemoveItemFromUserObserved(int user_id, int item_id)
        {
            string ConnectionString = "Data Source=(DESCRIPTION=" +
           "(ADDRESS=(PROTOCOL=TCP)(HOST=217.173.198.135)(PORT=1521))" +
           "(CONNECT_DATA=(SERVICE_NAME=tpdb)));" + "" +
           "User Id=s100824;Password=Sddb2023;";

            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                var query = "DELETE observed WHERE (user_user_id = :user_user_id AND item_item_id = :item_item_id)";

                using (OracleCommand cmdInsert = new OracleCommand(query, conn))
                {
                    OracleCommand command = new OracleCommand(query, conn);
                    command.Parameters.Add(new OracleParameter("user_user_id", user_id));
                    command.Parameters.Add(new OracleParameter("item_item_id", item_id));

                    try
                    {
                        conn.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        conn.Close();

                    }
                    catch (OracleException ex)
                    {
                        // wystąpił błąd Oracle - wyświetl komunikat o błędzie
                        DisplayAlert("UPS", "Cos poszlo nie tak", "Spróbuj ponownie", ex.Message);

                    }
                }
            }
        }

        //private async void OnWishlistProductShoppingCartTapped(object sender, EventArgs e)
        //{
        //    var tappedImage = (Image)sender;
        //    var tappedProduct = (Product)tappedImage.BindingContext;

        //    var app = (App)Application.Current;
        //    var shoppingCartDataStore = app.shoppingCartDataStore;

        //    if (shoppingCartDataStore.CheckInDataStore(tappedProduct))
        //    {
        //        await shoppingCartDataStore.AddItemAsync(tappedProduct);
        //        InsertIntoShoppingCartDB(tappedProduct);

        //        UserDialogs.Instance.Toast("Przedmiot został dodany do koszyka pomyślnie!", TimeSpan.FromSeconds(2));
        //    }
        //    else
        //    {
        //        if(await DisplayAlert("W koszyku juz znajduje sie taki przedmiot", "Chcesz dodac duplikat tego przedmiotu do koszyka?", "Tak", "Nie"))
        //        {
        //            await shoppingCartDataStore.AddItemAsync(tappedProduct);
        //            InsertIntoShoppingCartDB(tappedProduct);

        //            UserDialogs.Instance.Toast("Przedmiot został dodany do koszyka pomyślnie!", TimeSpan.FromSeconds(2));
        //        }
        //    }
        //}


        private async void OnWishlistProductShoppingCartTapped(object sender, EventArgs e)
        {
            //TODO: przedmiot zostaje dodany do koszyka, nawet kiedy użytkownik kliknie anuluj przy wyborze rozmiaru
            var tappedImage = (Image)sender;
            var tappedProduct = (Product)tappedImage.BindingContext;

            var app = (App)Application.Current;
            var shoppingCartDataStore = app.shoppingCartDataStore;

            string selectedSize = await DisplayActionSheet("Wybierz rozmiar", "Anuluj", null,
                                        getClothingSizeArray(tappedProduct).ToArray());
            if (selectedSize == null || selectedSize == "Anuluj") { selectedSize = ""; }

            if (shoppingCartDataStore.CheckInDataStore(tappedProduct))
            {
                await shoppingCartDataStore.AddItemAsync(tappedProduct);
                InsertIntoShoppingCartDB(tappedProduct, selectedSize);

                UserDialogs.Instance.Toast("Przedmiot został dodany do koszyka pomyślnie!", TimeSpan.FromSeconds(2));
            }
            else
            {
                if (await DisplayAlert("W koszyku juz znajduje sie taki przedmiot", "Chcesz dodac duplikat tego przedmiotu do koszyka?", "Tak", "Nie"))
                {
                    await shoppingCartDataStore.AddItemAsync(tappedProduct);
                    InsertIntoShoppingCartDB(tappedProduct, selectedSize);

                    UserDialogs.Instance.Toast("Przedmiot został dodany do koszyka pomyślnie!", TimeSpan.FromSeconds(2));
                }
            }
        }

        private void InsertIntoShoppingCartDB(Product product)
        {
            int productId = product.Id;
            var app = (App)Application.Current;
            string ConnectionString = app.connectionString;
            string queryString = "INSERT INTO shopping_cart (user_user_id, item_item_id)" +
                "VALUES (:user_id, :item_id)";
            int userId = app.userId;
            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                using (OracleCommand cmd = new OracleCommand(queryString, conn))
                {
                    cmd.Parameters.Add("user_id", userId);
                    cmd.Parameters.Add("item_id", productId);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                    catch(OracleException ex)
                    {
                        System.Console.WriteLine(ex);
                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                }
            }
        }

        private void InsertIntoShoppingCartDB(Product product, string size)
        {
            int productId = product.Id;
            var app = (App)Application.Current;
            string ConnectionString = app.connectionString;
            string queryString = "INSERT INTO shopping_cart (user_user_id, item_item_id, temp_item_size)" +
                "VALUES (:user_id, :item_id, :item_size)";
            int userId = app.userId;
            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                using (OracleCommand cmd = new OracleCommand(queryString, conn))
                {
                    cmd.Parameters.Add("user_id", userId);
                    cmd.Parameters.Add("item_id", productId);
                    cmd.Parameters.Add("item_size", size);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                    catch (OracleException ex)
                    {
                        System.Console.WriteLine(ex);
                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                }
            }
        }

        public List<string> getClothingSizeArray(Product product)
        {
            string selectedSize = "";
            Dictionary<string, List<string>> categorySizes = new Dictionary<string, List<string>>();
            categorySizes.Add("Paski", new List<string> { "S", "L" });
            categorySizes.Add("Rękawice", new List<string> { "S", "L" });
            categorySizes.Add("Buty", new List<string> { "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45" });
            categorySizes.Add("Spódnice", new List<string> { "XS", "S", "M", "L", "XL", "XXL" });
            categorySizes.Add("Kurtka", new List<string> { "XS", "S", "M", "L", "XL", "XXL" });
            categorySizes.Add("Suknie", new List<string> { "XS", "S", "M", "L", "XL", "XXL" });
            categorySizes.Add("Czapka", new List<string> { "S", "L" });
            categorySizes.Add("Marynarka", new List<string> { "XS", "S", "M", "L", "XL", "XXL" });
            categorySizes.Add("Szalik", new List<string> { "ONE" });
            categorySizes.Add("Bluza", new List<string> { "XS", "S", "M", "L", "XL", "XXL" });
            categorySizes.Add("Bielizna", new List<string> { "XS", "S", "M", "L", "XL", "XXL" });
            categorySizes.Add("T-shirt", new List<string> { "XS", "S", "M", "L", "XL", "XXL" });
            categorySizes.Add("Koszula", new List<string> { "XS", "S", "M", "L", "XL", "XXL" });
            categorySizes.Add("Spodnie", new List<string> { "32", "34", "36", "38", "40", "42", "44" });

            if(categorySizes.TryGetValue(product.Category, out List<string> sizes))
            {
                return sizes;

            }
            else
            {
                DisplayAlert("Error!", "Nie można zlokalizować rozmiarów dla tego przedmiotu!", "Ok");
            }

            return new List<string>();
        }

        private async void ClearWhishListDataStoreList(object sender, EventArgs e)
        {
            var app = (App)Application.Current;
            var wishlistDataStore = app.wishlistDataStore;
            
            if(wishlistDataStore.Count() == 0)
            {
                await DisplayAlert("Pusta lista", "W liscie obserwowanych przedmiotów nie znajduje się zadnego przedmiotu", "Anuluj");
            }
            else
            {
                if (await DisplayAlert("Zatwierdź", "Czy na pewno chcesz usunąć wszystkie przedmioty z listy?", "Tak", "Nie"))
                {
                    await wishlistDataStore.ClearAll();
                    DeleteAllFromObservedForUserDB(app.userId);

                    _viewModel.OnWishlistOpen();
                    _viewModel.FilterProducts(ItemCategory);
                    await DisplayAlert("Lista wyczyszczona", "Lista została wyczyszczona pomyślnie!", "OK");
                } 
            }          
        }

        private void DeleteAllFromObservedForUserDB(int userId)
        {
            var app = (App)Application.Current;
            string ConnectionString = app.connectionString;
            string queryString = "DELETE observed WHERE (user_user_id = :user_user_id)";
            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                using (OracleCommand cmd = new OracleCommand(queryString, conn))
                {
                    cmd.Parameters.Add("user_user_id", userId);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                    catch (OracleException ex)
                    {
                        System.Console.WriteLine(ex.ToString());
                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                }
            }
        }

    }


}