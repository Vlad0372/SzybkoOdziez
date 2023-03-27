using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SzybkoOdziez.Models;
using SzybkoOdziez.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SzybkoOdziez.ViewModels
{
    public class ShoppingCartViewModel : BaseViewModel
    {
        public ObservableCollection<ProductInfo> Items { get; set; }
        public ShoppingCartViewModel()
        {
            Title = "Koszyk Karola";

            Items = new ObservableCollection<ProductInfo>();
            Items.Add(new ProductInfo() { Id = 1, Name = "Bluza", Description = "desc1", Price = "250 zł", Url = "bluza.jpg" });
            Items.Add(new ProductInfo() { Id = 2, Name = "Czapka", Description = "desc2", Price = "15 zł", Url = "czapka.jpg" });
            Items.Add(new ProductInfo() { Id = 3, Name = "Buty", Description = "desc3", Price = "100 zł", Url = "buty.jpg" });
            Items.Add(new ProductInfo() { Id = 4, Name = "Dresy", Description = "desc4", Price = "200 zł", Url = "dresy.jpg" });
            Items.Add(new ProductInfo() { Id = 5, Name = "Kurtka", Description = "desc5", Price = "500 zł", Url = "kurtka.jpg" });
        }
    }
}