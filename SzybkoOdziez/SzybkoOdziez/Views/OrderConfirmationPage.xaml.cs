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
using System.Collections.Generic;

namespace SzybkoOdziez.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderConfirmationPage : ContentPage
    {
        ShoppingCartViewModel _viewModel;
        public ObservableCollection<Product> Products { get; set; }
        public OrderConfirmationPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ShoppingCartViewModel();


            //List<Product> products = new List<Product>();//lista produktów
            //Products = new ObservableCollection<Product>(products);

            //// oblicz łączną wartość
            //decimal total = products.Sum(p => p.Price);

            //// ustaw tekst etykiety
            //TotalLabel.Text += total.ToString();
           
            //foreach (var item in products)
            //{
            //    Console.WriteLine("xD");
            //}
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnShoppingCartOpen();

            Products = _viewModel.Products;

           
            //List<Product> products = 
            //Products = new ObservableCollection<Product>(products);
            //var govno = ProductsListView.BindingContext;
            //// oblicz łączną wartość
            decimal total = Products.Sum(p => p.Price);

            //// ustaw tekst etykiety
           TotalLabel.Text = total.ToString();


        }
        public ObservableCollection<string> Items { get; set; }

        private void complition_of_order_Clicked(object sender, EventArgs e)
        {
           Navigation.PushAsync(new OrderCompletionPage());
           
        }
    }
}
