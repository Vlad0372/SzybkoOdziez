using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SzybkoOdziez.Models;
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
        
    }
}

