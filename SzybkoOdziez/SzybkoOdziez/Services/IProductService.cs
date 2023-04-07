using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using SzybkoOdziez.Models;

namespace SzybkoOdziez.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task AddProductAsync(Product product);
        Task RemoveProductAsync(Product product);
    }


    public class ProductService : IProductService
    {
        private readonly ProductDataStore _dataStore;

        public ProductService(ProductDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _dataStore.GetItemsAsync();
        }

        public async Task AddProductAsync(Product product)
        {
            await _dataStore.AddItemAsync(product);
        }

        public async Task RemoveProductAsync(Product product)
        {
            //zmienic z ImageUrl na Id, ale dopiero kiedy produktom bedzie przypisywane porzadne Id
            await _dataStore.DeleteItemAsync(product.Id);
        }
    }


}
