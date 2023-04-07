using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using SzybkoOdziez.Models;
using SzybkoOdziez.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SzybkoOdziez.ViewModels
{
    public class WatchlistViewModel : BaseViewModel
    {
        //private Product _selectedProduct;

        public ObservableCollection<Product> Products { get; }
        public Command LoadProductsCommand { get; }
        //public Command<Product> RemoveProductCommand { get; }

        public WatchlistViewModel()
        {

            Products = new ObservableCollection<Product>();
            LoadProductsCommand = new Command(async () => await LoadWishlistAsync());
            //RemoveProductCommand = new Command(async () => await RemoveProductWishlistAsync(_selectedProduct));
        }

        public WatchlistViewModel(ObservableCollection<Product> products)
        {
            Products = products;
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