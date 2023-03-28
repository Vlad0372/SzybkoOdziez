using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SzybkoOdziez.Models;
using SzybkoOdziez.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SzybkoOdziez.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WatchlistPage : ContentPage
    {
        private ObservableCollection<ProductInfo> _products;

        public WatchlistPage()
        {
            InitializeComponent();
            //pulling list of liked products from app properties
            ObservableCollection<ProductInfo> likedProductsList = (ObservableCollection<ProductInfo>)Application.Current.Properties["likedProductsList"];
            _products = likedProductsList;
            BindingContext = new ViewModels.WatchlistViewModel(_products);
        }


        private void OnWishlistProductStackLayoutTapped(object sender, EventArgs e)
        {
            ChangePageToItemDescription();
        }


        private void ChangePageToItemDescription()
        {
            Navigation.PushAsync(new ItemDescriptionPage());
        }

        private void OnWishlistProductTrashcanTapped(object sender, EventArgs e)
        {
            var tappedImage = (Image)sender;
            var tappedProduct = (ProductInfo)tappedImage.BindingContext;
            RemoveItemFromWatchlist(tappedProduct);


            TappedEventArgs tappedEventArgs = (TappedEventArgs)e;
            ProductInfo product = ((WatchlistViewModel)BindingContext).Products
                .FirstOrDefault(prod => prod.Id == (int)tappedEventArgs.Parameter);
            ((WatchlistViewModel)BindingContext).Products.Remove(product);

            //TappedEventArgs tappedEventArgs = (TappedEventArgs)e;

            //ProductInfo produkt_informacje = ((WatchlistViewModel)BindingContext).Products.
            //    FirstOrDefault(prod => prod.Id == (int)tappedEventArgs.Parameter);


        }

        private async void RemoveItemFromWatchlist(ProductInfo tappedProduct)
        {


            //((WatchlistViewModel)BindingContext).Products.Remove(tappedProduct);

            if (_products.Contains(tappedProduct))
            {
                _products.Remove(tappedProduct);
                Application.Current.Properties.Remove("likedProductsList");
                Application.Current.Properties.Add("likedProductsList", _products);
                await Application.Current.SavePropertiesAsync();

                //odswiezenie viewmodelu, zeby zaktualizowac liste obserwowanych przedmiotow


            }
        }


    }


}