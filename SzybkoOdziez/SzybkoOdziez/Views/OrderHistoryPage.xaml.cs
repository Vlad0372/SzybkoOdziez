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



namespace SzybkoOdziez.Views
{
    public partial class OrderHistoryPage : ContentPage
    {
        private OrderHistoryViewModel _viewModel; 
        public ObservableCollection<Order> Orders { get; set; }

        public OrderHistoryPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new OrderHistoryViewModel();
            //additionalItemsExistImg.Visibility
           
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnOrderHistoryOpen();

            Orders = _viewModel.Orders;
        }
    }
}