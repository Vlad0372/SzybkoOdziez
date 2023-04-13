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
        public ObservableCollection<Order> Orders { get; }
        public Command LoadOrdersCommand { get; }

        public OrderHistoryViewModel()
        {

            Orders = new ObservableCollection<Order>();
            LoadOrdersCommand = new Command(async () => await LoadOrderHistoryListAsync());
        }

        public OrderHistoryViewModel(ObservableCollection<Order> orders)
        {
            Orders = orders;
        }

        public async void OnOrderHistoryOpen()
        {
            await LoadOrderHistoryListAsync();
        }

        async Task LoadOrderHistoryListAsync()
        {
            Orders.Clear();
            
            for (int i = 0; i < 10; i++)
            {
                var order = new Order
                {
                    Id = i,
                    Name = "order_title_" + i,
                    Products = new List<Product>()               
                };

                Orders.Add(order);
            }
            //Orders.Clear();
            //var app = (App)Application.Current;
            //var wishlistDataStore = app.wishlistDataStore;
            //var wishlistIEnumerable = await wishlistDataStore.GetItemsAsync();
            //foreach (var order in wishlistIEnumerable)
            //{
            //    Orders.Add(order);
            //}
        }
    }
}
