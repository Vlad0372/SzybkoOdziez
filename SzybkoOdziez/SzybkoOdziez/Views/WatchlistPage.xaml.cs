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

namespace SzybkoOdziez.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WatchlistPage : ContentPage
    {
        private ObservableCollection<ProductInfo> _products;
        private WishlistDataStore _wishlistDataStore;
        private WatchlistViewModel _watchlistViewModel;

        public WatchlistPage()
        {
            InitializeComponent();
            //pulling list of liked products from app properties



            ObservableCollection<ProductInfo> likedProductsList = (ObservableCollection<ProductInfo>)Application.Current.Properties["likedProductsList"];
            _products = likedProductsList;
            BindingContext = _watchlistViewModel = new ViewModels.WatchlistViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _watchlistViewModel.OnWishlistOpen();
        }

        private void OnWishlistProductStackLayoutTapped(object sender, EventArgs e)
        {
            ChangePageToItemDescription();
        }


        private void ChangePageToItemDescription()
        {
            Navigation.PushAsync(new ItemDescriptionPage());
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
                ((WatchlistViewModel)BindingContext).OnWishlistOpen();
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
            }
            else
            {
                if(await DisplayAlert("W koszyku juz znajduje sie taki przedmiot", "Chcesz dodac duplikat tego przedmiotu do koszyka?", "Tak", "Nie"))
                {
                    await shoppingCartDataStore.AddItemAsync(tappedProduct);
                }
            }
        }



    }


}