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

namespace SzybkoOdziez.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WatchlistPage : ContentPage
    {
        private WishlistDataStore _wishlistDataStore;
        private WatchlistViewModel _viewModel;

        public WatchlistPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ViewModels.WatchlistViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnWishlistOpen();
        }

        private void OnWishlistProductStackLayoutTapped(object sender, EventArgs e)
        {
            var tappedImage = (Grid)sender;
            var tappedProduct = (Product)tappedImage.BindingContext;
            ChangePageToItemDescription(tappedProduct);
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
            }

            

            //TappedEventArgs tappedEventArgs = (TappedEventArgs)e;
            //Product product = ((WatchlistViewModel)BindingContext).Products
            //    .FirstOrDefault(prod => prod.Id == (int)tappedEventArgs.Parameter);
            //((WatchlistViewModel)BindingContext).Products.Remove(product);





        }

        //private async void RemoveItemFromWatchlist(Product tappedProduct)
        //{


        //    ((WatchlistViewModel)BindingContext).Products.Remove(tappedProduct);

        //    if (_products.Contains(tappedProduct))
        //    {
        //        _products.Remove(tappedProduct);
        //        Application.Current.Properties.Remove("likedProductsList");
        //        Application.Current.Properties.Add("likedProductsList", _products);
        //        await Application.Current.SavePropertiesAsync();

        //        //odswiezenie viewmodelu, zeby zaktualizowac liste obserwowanych przedmiotow


        //        }
        //}

        private async void OnWishlistProductShoppingCartTapped(object sender, EventArgs e)
        {
            var tappedImage = (Image)sender;
            var tappedProduct = (Product)tappedImage.BindingContext;

            var app = (App)Application.Current;
            var shoppingCartDataStore = app.shoppingCartDataStore;

            if (shoppingCartDataStore.CheckInDataStore(tappedProduct))
            {
                await shoppingCartDataStore.AddItemAsync(tappedProduct);

                UserDialogs.Instance.Toast("Przedmiot został dodany do koszyka pomyślnie!", TimeSpan.FromSeconds(2));
            }
            else
            {
                if(await DisplayAlert("W koszyku juz znajduje sie taki przedmiot", "Chcesz dodac duplikat tego przedmiotu do koszyka?", "Tak", "Nie"))
                {
                    await shoppingCartDataStore.AddItemAsync(tappedProduct);

                    UserDialogs.Instance.Toast("Przedmiot został dodany do koszyka pomyślnie!", TimeSpan.FromSeconds(2));
                }
            }
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

                    _viewModel.OnWishlistOpen();

                    await DisplayAlert("Lista wyczyszczona", "Lista została wyczyszczona pomyślnie!", "OK");
                } 
            }          
        }



    }


}