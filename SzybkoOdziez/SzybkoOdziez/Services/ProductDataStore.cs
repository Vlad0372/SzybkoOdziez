using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SzybkoOdziez.Models;
using static Android.Resource;

namespace SzybkoOdziez.Services
{
    //przetwarzanie danych na konkretne przedmioty, ktore beda przechowywane w aplikacji,
    //chyba bedzie wazne kiedy bedziemy pobierali dane skads indziej niz z plikow lokalnych

    //TODO zmienic te listy na prywatne
    public class ProductDataStore : IDataStore<Product, int>
    {
        // A list of products that serves as an in-memory data store
        private List<Product> _products = new List<Product>();

        public ProductDataStore()
        {
            // Initialize the list of products in the constructor
            _products = new List<Product>();
        }

        // Adds a new product to the in-memory data store
        public async Task<bool> AddItemAsync(Product item)
        {
            _products.Add(item);
            return await Task.FromResult(true);
        }
        
        // Adds a list of products to the memory data store 
        // juz nie wiem co sie dzieje
        public async Task<bool> AddItemListAsync(List<Product> productList)
        {
            foreach (var product in productList)
            {
                _products.Add(product);
            }
            return await Task.FromResult(true);
        }

        // Updates an existing product in the in-memory data store
        public async Task<bool> UpdateItemAsync(Product item)
        {
            var oldItem = _products.Where((Product arg) => arg.Id == item.Id).FirstOrDefault();
            _products.Remove(oldItem);
            _products.Add(item);

            return await Task.FromResult(true);
        }

        // Deletes a product with the specified ID from the in-memory data store
        public async Task<bool> DeleteItemAsync(int id)
        {
            var oldItem = _products.Where((Product arg) => arg.Id == id).FirstOrDefault();
            _products.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(Product product)
        {
            _products.Remove(product);
            return await Task.FromResult(true);
        }

        // Returns a product with the specified ID from the in-memory data store
        public async Task<Product> GetItemAsync(int id)
        {
            return await Task.FromResult(_products.FirstOrDefault(s => s.Id == id));
        }

        // Returns all products from the in-memory data store
        public async Task<IEnumerable<Product>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(_products);
        }

        //Check if this product is already in the data store
        //Return false if no occurence is found
        public bool CheckInDataStore(Product product)
        {
            bool notFoundInDataStore = true;
            foreach (var item in _products)
            {
                //trzeba zmienic na porownanie product == product, ale id sie nie zgadza poki co
                if (item.Name == product.Name)
                {
                    notFoundInDataStore = false;
                }
            }
            return notFoundInDataStore;
        }
        public int Count()
        {
            // Returns length of product list
            if(_products != null)
            {
                return _products.Count();
            }
            return 0;
        }
        public async Task ClearAll()
        {
            // Removes all products from the product list
            if (_products != null)
            {
                for(int i = _products.Count - 1; i >= 0; i--)
                {
                    await DeleteItemAsync(_products[i]);
                }
            }
        }
        public Product GetItemByUrl(string url)
        {
            return _products.FirstOrDefault(s => s.ImageUrl == url);
        }

    }
}

