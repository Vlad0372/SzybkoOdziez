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
        public ObservableCollection<ProductInfo> listaProduktu { get; set; }
        public ShoppingCartViewModel()
        {
            Title = "Koszyk Karola";

            listaProduktu = new ObservableCollection<ProductInfo>();
            listaProduktu.Add(new ProductInfo() { ProduktId = 1, NazwaProduktu = "Bluza", Cena = "250 zł", ImageUrl = "bluza.jpg" });
            listaProduktu.Add(new ProductInfo() { ProduktId = 2, NazwaProduktu = "Czapka", Cena = "15 zł", ImageUrl = "czapka.jpg" });
            listaProduktu.Add(new ProductInfo() { ProduktId = 3, NazwaProduktu = "Buty", Cena = "100 zł", ImageUrl = "buty.jpg" });
            listaProduktu.Add(new ProductInfo() { ProduktId = 4, NazwaProduktu = "Dresy", Cena = "200 zł", ImageUrl = "dresy.jpg" });
            listaProduktu.Add(new ProductInfo() { ProduktId = 5, NazwaProduktu = "Kurtka", Cena = "500 zł", ImageUrl = "kurtka.jpg" });
        }
    }
}