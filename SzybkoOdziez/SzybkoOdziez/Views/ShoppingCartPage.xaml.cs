using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SzybkoOdziez.Models;
using SzybkoOdziez.ViewModels;

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
            Navigation.PushAsync(new Zatwierdzenie_zamowienie());
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
                ((ShoppingCartViewModel)BindingContext).OnShoppingCartOpen();
            }
        }
    }
}
