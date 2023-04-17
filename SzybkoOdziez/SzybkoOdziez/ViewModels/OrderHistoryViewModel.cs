using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using SzybkoOdziez.Models;
using SzybkoOdziez.Views;
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
            LoadOrdersCommand = new Command(async () => await LoadOrderHistoryAsync());            
        }

        public OrderHistoryViewModel(ObservableCollection<Order> orders)
        {
            Orders = orders;
        }

        public async void OnOrderHistoryOpen()
        {
            //await LoadOrderHistoryAsync();
            await LoadOrderHistoryAsync_TEST();
        }

        async Task LoadOrderHistoryAsync()
        {
            Orders.Clear();

            var app = (App)Application.Current;
            var orderHistoryDataStore = await app.orderHistoryDataStore.GetItemsAsync();

            foreach (var order in orderHistoryDataStore)
            {
                Orders.Add(order);
            }
        }

        async Task LoadOrderHistoryAsync_TEST()
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
        }
    }
}
