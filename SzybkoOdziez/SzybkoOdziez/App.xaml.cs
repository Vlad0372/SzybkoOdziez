using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SzybkoOdziez.Services;
using SzybkoOdziez.Services.MyApp.Services;
using SzybkoOdziez.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SzybkoOdziez
{
    public partial class App : Application
    {
        public AllProductDataStore allProductDataStore;
        public WishlistDataStore wishlistDataStore;
        public ShoppingCartDataStore shoppingCartDataStore;
        public OrderHistoryDataStore orderHistoryDataStore;
        public string connectionString = "Data Source=(DESCRIPTION=" +
            "(ADDRESS=(PROTOCOL=TCP)(HOST=217.173.198.135)(PORT=1521))" +
            "(CONNECT_DATA=(SERVICE_NAME=tpdb)));" + "" +
            "User Id=s100824;Password=Sddb2023;";

        public App()
        {
            InitializeComponent();
            allProductDataStore = new AllProductDataStore();
            wishlistDataStore = new WishlistDataStore();
            shoppingCartDataStore = new ShoppingCartDataStore();
            orderHistoryDataStore = new OrderHistoryDataStore();

            DependencyService.Register<DefaultMockDataStore>();
            MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
            var productLoader = new ProductLoader(allProductDataStore);
            await productLoader.LoadAllProductsFromDrawableAsync();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
