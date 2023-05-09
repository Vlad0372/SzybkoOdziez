using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using SzybkoOdziez.Models;
using SzybkoOdziez.Services;
using SzybkoOdziez.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SzybkoOdziez.Views;
using System.Collections.Generic;
using System.Linq;
using Oracle.ManagedDataAccess.Client;
using Java.Util;

namespace SzybkoOdziez.Views
{
    public partial class OrderHistoryPage : ContentPage
    {
        private OrderHistoryViewModel _viewModel; 
        public ObservableCollection<Order> Orders { get; set; }

        public OrderHistoryPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new OrderHistoryViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnOrderHistoryOpen();

            Orders = _viewModel.Orders;
            //Orders = GetOrdersFromDB();
        }
        private ObservableCollection<Order> GetOrdersFromDB()
        {
            var orders = new ObservableCollection<Order>();
            var productsList = DependencyService.Get<IImgArrayGetterService>().GetProductListFromDBStreamAsync();

            string connStr = "Data Source=(DESCRIPTION=" +
           "(ADDRESS=(PROTOCOL=TCP)(HOST=217.173.198.135)(PORT=1521))" +
           "(CONNECT_DATA=(SERVICE_NAME=tpdb)));" + "" +
           "User Id=s100824;Password=Sddb2023;";

            using (OracleConnection conn = new OracleConnection(connStr))
            {
                conn.Open();

                OracleCommand command = new OracleCommand();

                command.Connection = conn;
                command.CommandText = "select distinct order_order_id from item_order";

                OracleDataReader data = command.ExecuteReader();

                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        string currentsOrderId = data["order_order_id"].ToString();                     

                        OracleCommand subCommand = new OracleCommand();

                        subCommand.Connection = conn;
                        subCommand.CommandText = "select item_item_id from item_order where order_order_id = '" + currentsOrderId + "'";
                        
                        OracleDataReader subData = subCommand.ExecuteReader();

                        var currentProducts = new ObservableCollection<Product>();

                        if (subData.HasRows)
                        {
                            while (subData.Read())
                            {
                                var currentProdInfo = productsList.Find(i => i.Id == Convert.ToInt32(subData["item_item_id"].ToString()));

                                var currentProd = new Product()
                                {
                                    Id = currentProdInfo.Id,
                                    Name = currentProdInfo.Name,
                                    Description = currentProdInfo.Description,
                                    ImageUrl = currentProdInfo.Url,
                                    Price = Convert.ToDecimal(currentProdInfo.Price),
                                    TotalPrice = 0,
                                    Comments = new List<Comment>(),
                                };

                                currentProducts.Add(currentProd);
                            }
                        }

                        var currentOrder = new Order()
                        {
                            Id = Convert.ToInt32(data["order_order_id"]),
                            Number = "1234",
                            CreatedDate = DateTime.Now.Date.ToString(),
                            TotalPrice = 0,
                            Products = currentProducts,
                        };

                        orders.Add(currentOrder);
                    }
                }
            }

            return orders;
        }

        private async void DokonajZwrotu_Tapped(object sender, EventArgs e)
        {
            var tappedLabel = (Label)sender;
            var tappedOrder = (Order)tappedLabel.BindingContext;
            //tappedOrder.orderState = 1;
            var app = (App)Application.Current;
            var orderHistoryDataStore = await app.orderHistoryDataStore.GetItemsAsync();

            if (orderHistoryDataStore.Contains(tappedOrder)){
                await app.orderHistoryDataStore.DeleteItemAsync(tappedOrder);
                _viewModel.OnOrderHistoryOpen();
            }

            await DisplayAlert("Zwrot", "Dokonano zwrotu tego zamówienia", "Ok");

        }
    }
}