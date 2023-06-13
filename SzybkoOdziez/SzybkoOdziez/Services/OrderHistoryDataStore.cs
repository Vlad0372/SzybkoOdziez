using Java.Nio.Channels;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SzybkoOdziez.Models;
using Xamarin.Forms;
using static Android.Resource;

namespace SzybkoOdziez.Services
{
    public class OrderHistoryDataStore : IDataStore<Order, int>
    {
        private List<Order> _orders = new List<Order>();

        public OrderHistoryDataStore()
        {
            _orders = new List<Order>();
        }

        public async Task<bool> AddItemAsync(Order item)
        {
            _orders.Add(item);
            return await Task.FromResult(true);
        }

        public async Task<bool> AddItemListAsync(List<Order> orderList)
        {
            foreach (var order in orderList)
            {
                _orders.Add(order);
            }
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Order item)
        {
            var oldItem = _orders.Where((Order arg) => arg.Id == item.Id).FirstOrDefault();
            _orders.Remove(oldItem);
            _orders.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var oldItem = _orders.Where((Order arg) => arg.Id == id).FirstOrDefault();
            _orders.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(Order order)
        {
            _orders.Remove(order);
            return await Task.FromResult(true);
        }

        public async Task<Order> GetItemAsync(int id)
        {
            return await Task.FromResult(_orders.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Order>> GetItemsAsync(bool forceRefresh = false)
        {
            _orders = GetOrdersFromDB();
         
            return await Task.FromResult(_orders);
        }

        public bool CheckInDataStore(Order order)
        {
            bool notFoundInDataStore = true;
            foreach (var item in _orders)
            {
                //trzeba zmienic na porownanie product == product, ale id sie nie zgadza poki co
                if (item.Number == order.Number)
                {
                    notFoundInDataStore = false;
                }
            }
            return notFoundInDataStore;
        }
        public int Count()
        {
            if (_orders != null)
            {
                return _orders.Count;
            }
            return 0;
        }
        public Order GetLastItem()
        {
            if (_orders != null)
            {
                return _orders.LastOrDefault();
            }
            return new Order();
        }
        public async Task ClearAll()
        {
            // Removes all products from the product list
            if (_orders != null)
            {
                for (int i = _orders.Count - 1; i >= 0; i--)
                {
                    await DeleteItemAsync(_orders[i]);
                }
            }
        }
        private List<Order> GetOrdersFromDB()
        {
            var app = (App)Application.Current;
            string currentUserId = app.userId.ToString();

            var orders = new List<Order>();
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
                //command.CommandText = "select distinct order_order_id from item_order";
                command.CommandText = " SELECT distinct order_order_id FROM item_order INNER JOIN \"order\" " +
                    "ON item_order.order_order_id = \"order\".order_id " +
                    "INNER JOIN \"user\" ON \"order\".user_user_id = \"user\".user_id " +
                    "where \"user\".user_id = " + currentUserId;

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
                                    ImageUrl = currentProdInfo.ImageUrl,
                                    Price = currentProdInfo.Price,
                                    TotalPrice = 0,
                                    Comments = new List<Comment>(),
                                };

                                currentProducts.Add(currentProd);
                            }
                        }


                        var currentOrder = new Order()
                        {
                            Id = Convert.ToInt32(data["order_order_id"]),
                            Number = data["order_order_id"].ToString(),
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
    }
}

