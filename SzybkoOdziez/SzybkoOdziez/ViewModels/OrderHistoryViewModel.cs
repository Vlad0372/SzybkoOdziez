using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using SzybkoOdziez.Models;
using Xamarin.Forms;

namespace SzybkoOdziez.ViewModels
{
    class OrderHistoryViewModel
    {
        public ObservableCollection<Product> Products { get; }
        public Command LoadProductsCommand { get; }

        public OrderHistoryViewModel()
        {

            Products = new ObservableCollection<Product>();
            LoadProductsCommand = new Command(async () => await LoadWishlistAsync());
            //RemoveProductCommand = new Command(async () => await RemoveProductWishlistAsync(_selectedProduct));
        }

        public OrderHistoryViewModel(ObservableCollection<Product> products)
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
    }
}
