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
using Acr.UserDialogs;

namespace SzybkoOdziez.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderConfirmationPage : ContentPage
    {
        ShoppingCartViewModel _viewModel;
        public ObservableCollection<Product> Products { get; set; }
        private Order currentOrder { get; set; }

        public OrderConfirmationPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ShoppingCartViewModel();

            currentOrder = new Order();
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

            //=============== to Vladek dodal ===================== 
          
            var app = (App)Application.Current;
            var orderHistoryDataStore = app.orderHistoryDataStore;
            int orderId = 0;
            Random rnd = new Random();

            if (orderHistoryDataStore != null && orderHistoryDataStore.Count() < 1) 
            {
                orderId = 0;
            }
            else
            {
                orderId = orderHistoryDataStore.GetLastItem().Id;
            }

            currentOrder = new Order
            {
                Id = orderId,
                Number = rnd.Next(10000, 99999).ToString(),
                CreatedDate = DateTime.Now.ToString("dd.MM.yyy"),
                TotalPrice = Products.Sum(p => p.Price),
                Products = Products
            };

            if (orderHistoryDataStore.CheckInDataStore(currentOrder))
            {
                await orderHistoryDataStore.AddItemAsync(currentOrder);  
            }
           
            //=============== to Vladek dodal ===================== 
            
            //// oblicz łączną wartość
  
            //// ustaw tekst etykiety
           TotalLabel.Text = currentOrder.TotalPrice.ToString();
           

        }
        public ObservableCollection<string> Items { get; set; }

        private void complition_of_order_Clicked(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new OrderCompletionPage());
            Navigation.PushAsync(new OrderCompletionPage(currentOrder));
            Navigation.RemovePage(this);

        }
        
    }
}
