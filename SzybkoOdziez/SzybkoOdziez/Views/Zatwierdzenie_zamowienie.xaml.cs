using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SzybkoOdziez.Models;
using SzybkoOdziez.ViewModels;
using SzybkoOdziez.Views;

namespace SzybkoOdziez.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Zatwierdzenie_zamowienie : ContentPage
    {
        ShoppingCartViewModel _viewModel;
        public Zatwierdzenie_zamowienie()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ShoppingCartViewModel();
            
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnShoppingCartOpen();
        }
        public ObservableCollection<string> Items { get; set; }

        private void complition_of_order_Clicked(object sender, EventArgs e)
        {
           Navigation.PushAsync(new CompletionOfTheOrder());
        }
    }
}
