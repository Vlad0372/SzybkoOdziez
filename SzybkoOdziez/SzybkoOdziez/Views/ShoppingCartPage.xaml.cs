using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SzybkoOdziez.Models;
using SzybkoOdziez.ViewModels;
using SzybkoOdziez.Services;

namespace SzybkoOdziez.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShoppingCartPage : ContentPage
    {
        ShoppingCartViewModel _viewModel;
        public ShoppingCartPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ShoppingCartViewModel();
            
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnShoppingCartOpen();
            
        }

        private void Kliknienie_zamowienia(object sender, EventArgs e)
        {
            Navigation.PushAsync(new OrderConfirmationPage());
            Navigation.RemovePage(this);
        }
        private async void ShoppingCartTrashcan_Tapped(object sender, EventArgs e)
        {
            //TappedEventArgs tappedEventArgs = (TappedEventArgs)e;

            //Product product = ((ShoppingCartViewModel)BindingContext).Products.
            //    FirstOrDefault(prod => prod.Id == (int)tappedEventArgs.Parameter);

            //((ShoppingCartViewModel)BindingContext).Products.Remove(product);

            var tappedImage = (Image)sender;
            var tappedProduct = (Product)tappedImage.BindingContext;

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
        }
        private async void ClearShoppingCartDataStoreList(object sender, EventArgs e)
        {
            var app = (App)Application.Current;
            var shoppingCartDataStore = app.shoppingCartDataStore;

            if (shoppingCartDataStore.Count() == 0)
            {
                await DisplayAlert("Pusta lista", "W liscie obserwowanych przedmiotów nie znajduje się zadnego przedmiotu", "Anuluj");
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
        

    }
}
