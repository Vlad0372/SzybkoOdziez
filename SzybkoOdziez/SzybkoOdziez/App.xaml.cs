using Plugin.FirebasePushNotification;
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
        public int userId = 99;
        public bool guestMode = true;

        public App()
        {
            InitializeComponent();
            allProductDataStore = new AllProductDataStore();
            wishlistDataStore = new WishlistDataStore();
            shoppingCartDataStore = new ShoppingCartDataStore();
            orderHistoryDataStore = new OrderHistoryDataStore();

            // Use the dependency service to get a platform-specific implementation and initialize it.
            DependencyService.Get<INotificationManager>().Initialize();

            DependencyService.Register<DefaultMockDataStore>();
            MainPage = new AppShell();
 
        }
        //void ShowNotification(string title)
        //{
        //    Device.BeginInvokeOnMainThread(() =>
        //    {
        //        DisplayAlert($"Informacja dotycząca zamówienia", "Poszło! Już mamy twoje zamówinie," +
        //            " za niedługo dostaniesz szczegóły dotyczące terminu dostawy ;)", "OK");
        //    });
        //}
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
