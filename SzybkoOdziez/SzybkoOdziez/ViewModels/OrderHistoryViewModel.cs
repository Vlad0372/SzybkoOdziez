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
            //await LoadOrderHistoryAsync_TEST();
            await LoadOrderHistoryAsync();
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
                var bruh = new ObservableCollection<Product>();

                if(i % 2 == 0)
                {
                    bruh.Add(new Product { Id = i, ImageUrl="@drawable/m_acc_item_1.jpg"});
                }
                else
                {
                    bruh.Add(new Product { Id = i, ImageUrl = "@drawable/m_acc_item_3.jpg" });
                    bruh.Add(new Product { Id = i*2, ImageUrl = "@drawable/m_acc_item_2.jpg" });
            

                }
                var order = new Order
                {
                    Id = i,
                    Number = "order_number_" + i,
                    CreatedDate = DateTime.Now.ToString("dd.MM.yyy"),
                    Products = bruh//new List<Product>()
                };

                Orders.Add(order);
            }
        }
    }
}
