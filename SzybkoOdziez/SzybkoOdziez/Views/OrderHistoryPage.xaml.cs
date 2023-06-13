using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using SzybkoOdziez.Models;
using SzybkoOdziez.Services;
using SzybkoOdziez.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SzybkoOdziez.Views;
using System.Collections.Generic;
using System.Linq;
using Oracle.ManagedDataAccess.Client;
using Java.Util;

namespace SzybkoOdziez.Views
{
    public partial class OrderHistoryPage : ContentPage
    {
        private OrderHistoryViewModel _viewModel; 
        public ObservableCollection<Order> Orders { get; set; }

        bool guestMode = true;

        public OrderHistoryPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new OrderHistoryViewModel();

            OrderList.ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical)
            {
                ItemSpacing = 15
            };
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnOrderHistoryOpen();
            var app = (App)Application.Current;
            guestMode = app.guestMode;

            if (guestMode)
            {
                await DisplayAlert("Guest user!", "By uzyskać dostęp do historii swoich zamówień, musisz się zalogować!", "Ok");
            }

            Orders = _viewModel.Orders;
        }

        private async void DokonajZwrotu_Tapped(object sender, EventArgs e)
        {
            var tappedLabel = (Label)sender;
            var tappedOrder = (Order)tappedLabel.BindingContext;
            //tappedOrder.orderState = 1;
            var app = (App)Application.Current;
            var orderHistoryDataStore = await app.orderHistoryDataStore.GetItemsAsync();

            if (orderHistoryDataStore.Contains(tappedOrder)){
                await app.orderHistoryDataStore.DeleteItemAsync(tappedOrder);
                _viewModel.OnOrderHistoryOpen();
            }

            await DisplayAlert("Zwrot", "Dokonano zwrotu tego zamówienia", "Ok");

        }
        private void ShowFullOrder_Tapped(object sender, EventArgs e)
        {
            var tappedLabel = (Label)sender;
            var tappedOrder = (Order)tappedLabel.BindingContext;

            Navigation.PushAsync(new OrderFullInfoPage(tappedOrder.Id.ToString()));       
            Navigation.RemovePage(this);
        }
    }
}