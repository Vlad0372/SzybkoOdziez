﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SzybkoOdziez.Models;
using SzybkoOdziez.ViewModels;
using SzybkoOdziez.Services;
using Oracle.ManagedDataAccess.Client;

namespace SzybkoOdziez.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShoppingCartPage : ContentPage
    {
        private ShoppingCartViewModel _viewModel;
        public ShoppingCartPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ShoppingCartViewModel();
            
         
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.InitializeShoppingCartFromDB(99);
            _viewModel.OnShoppingCartOpen();

        }

      
        private async void ClearShoppingCartDataStoreList(object sender, EventArgs e)
        {
            var app = (App)Application.Current;
            var shoppingCartDataStore = app.shoppingCartDataStore;

            if (shoppingCartDataStore.Count() == 0)
            {
                await DisplayAlert("Pusta lista", "W koszyku nie znajduje się zadnego przedmiotu", "Anuluj");
            }
            else
            {
                if (await DisplayAlert("Zatwierdź", "Czy na pewno chcesz usunąć wszystkie przedmioty z listy?", "Tak", "Nie"))
                {
                    await shoppingCartDataStore.ClearAll();

                    _viewModel.OnShoppingCartOpen();

                    await DisplayAlert("Lista wyczyszczona", "Lista została wyczyszczona pomyślnie!", "OK");
                }
            }


        }
        private async void ShoppingCartTrashcan_Tapped(object sender, EventArgs e)
        {
            //TappedEventArgs tappedEventArgs = (TappedEventArgs)e;

            //Product product = ((ShoppingCartViewModel)BindingContext).Products.
            //    FirstOrDefault(prod => prod.Id == (int)tappedEventArgs.Parameter);

            //((ShoppingCartViewModel)BindingContext).Products.Remove(product);

            var tappedImage = (Image)sender;
            var tappedProduct = (Product)tappedImage.BindingContext;
            var tappedProduct1 = (Order)tappedImage.BindingContext;
            var app = (App)Application.Current;
            var shoppingCartDataStore = app.shoppingCartDataStore;

            if (shoppingCartDataStore.CheckInDataStore(tappedProduct))
            {
                await DisplayAlert("Error!", "Item not found in data store when trying to remove it from shoppingCartDataStore!", "Ok");
            }
            else
            {
                await shoppingCartDataStore.DeleteItemAsync(tappedProduct);
                _viewModel.OnShoppingCartOpen();
            }
            RemoveItemFromUserObserved(99, tappedProduct.Id);

        }


        private void RemoveItemFromUserObserved(int user_id, int item_id)
        {
            string ConnectionString = "Data Source=(DESCRIPTION=" +
           "(ADDRESS=(PROTOCOL=TCP)(HOST=217.173.198.135)(PORT=1521))" +
           "(CONNECT_DATA=(SERVICE_NAME=tpdb)));" + "" +
           "User Id=s100824;Password=Sddb2023;";

            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                var query = "DELETE shopping_cart WHERE (user_user_id = :user_user_id AND item_item_id = :item_item_id)";

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
        private void Kliknienie_zamowienia(object sender, EventArgs e)
        {
            Navigation.PushAsync(new OrderConfirmationPage());
            Navigation.RemovePage(this);
        }

    }
}
