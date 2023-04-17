using System;
using System.Collections.Generic;
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

        public App()
        {
            InitializeComponent();
            allProductDataStore = new AllProductDataStore();
            wishlistDataStore = new WishlistDataStore();
            shoppingCartDataStore = new ShoppingCartDataStore();

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
