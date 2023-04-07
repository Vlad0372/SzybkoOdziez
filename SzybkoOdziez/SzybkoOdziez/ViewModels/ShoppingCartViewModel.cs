using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using SzybkoOdziez.Models;
using SzybkoOdziez.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SzybkoOdziez.ViewModels
{
    public class ShoppingCartViewModel : BaseViewModel
    {
        public ObservableCollection<Product> Products { get; set; }
        public ShoppingCartViewModel()
        {
            Title = "Koszyk";

            Products = new ObservableCollection<Product>();
            //Products.Add(new Product() { Id = 1, Name = "Bluza", Description = "desc1", Price = 250, ImageUrl = "bluza.jpg" });
            //Products.Add(new Product() { Id = 2, Name = "Czapka", Description = "desc2", Price = 15, ImageUrl = "czapka.jpg" });
            //Products.Add(new Product() { Id = 3, Name = "Buty", Description = "desc3", Price = 100, ImageUrl = "buty.jpg" });
            //Products.Add(new Product() { Id = 4, Name = "Dresy", Description = "desc4", Price = 200, ImageUrl = "dresy.jpg" });
            //Products.Add(new Product() { Id = 5, Name = "Kurtka", Description = "desc5", Price = 500, ImageUrl = "kurtka.jpg" });
            var app = (App)Application.Current;
            foreach (var product in Products)
            {
                app.shoppingCartDataStore.AddItemAsync(product);
            }

        }

        public async void OnShoppingCartOpen()
        {
            await LoadShoppingCartAsync();
        }

        async Task LoadShoppingCartAsync()
        {
            Products.Clear();
            var app = (App)Application.Current;
            var shoppingCartDataStore = app.shoppingCartDataStore;
            var shoppingCartIEnumerable = await shoppingCartDataStore.GetItemsAsync();
            foreach (var product in shoppingCartIEnumerable)
            {
                Products.Add(product);
            }
        }
    }
}