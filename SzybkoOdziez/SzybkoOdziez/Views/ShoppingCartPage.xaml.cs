using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SzybkoOdziez.Models;
using SzybkoOdziez.ViewModels;

namespace SzybkoOdziez.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShoppingCartPage : ContentPage
    {
        ShoppingCartViewModel _viewModel;
        public ShoppingCartPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ShoppingCartViewModel();
        }
        private void Kliknienie_zamowienia(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Zatwierdzenie_zamowienie());
        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            TappedEventArgs tappedEventArgs = (TappedEventArgs)e;

            ProductInfo produkt_informacje = ((ShoppingCartViewModel)BindingContext).Items.
                FirstOrDefault(prod => prod.Id == (int)tappedEventArgs.Parameter);

            ((ShoppingCartViewModel)BindingContext).Items.Remove(produkt_informacje);
        }
    }
}
