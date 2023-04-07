using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SzybkoOdziez.Models;

namespace SzybkoOdziez.Services
{
    using Android;
    using Android.Content.Res;
    using Android.Content.Res.Loader;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    namespace MyApp.Services
    {
        public class ProductLoader
        {
            private readonly ProductDataStore _dataStore = new ProductDataStore();

            public ProductLoader(ProductDataStore dataStore)
            {
                _dataStore = dataStore;
            }

            public async Task LoadAllProductsFromDrawableAsync()
            {
                List<string> imgsNameList = DependencyService.Get<IImgArrayGetterService>().GetImgArrayStreamAsync();

                var products = new List<Product>();
                for (int i = 0; i < imgsNameList.Count; i++)
                {
                    var productName = Path.GetFileNameWithoutExtension(imgsNameList[i]);
                    var product = new Product
                    {
                        Id = i,
                        //zmienic imageurl mejbi dunno
                        ImageUrl = "@drawable/" + imgsNameList[i] + ".jpg",
                        Name = productName,
                        Description = "Lorem ipsum",
                        Price = new Random().Next(8, 250),
                    };
                    products.Add(product);
                }
                await _dataStore.AddItemListAsync(products);



            }
        }
    }


}
