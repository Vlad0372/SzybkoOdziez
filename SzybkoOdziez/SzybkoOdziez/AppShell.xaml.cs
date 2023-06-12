﻿using System;
using System.Collections.Generic;
using SzybkoOdziez.ViewModels;
using SzybkoOdziez.Views;
using Xamarin.Forms;

namespace SzybkoOdziez
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(NotificationPage), typeof(NotificationPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            var app = (App)Application.Current;
            app.guestMode = true;
            app.userId = 99;
            app.orderHistoryDataStore.ClearAll();
            app.shoppingCartDataStore.ClearAll();
            app.wishlistDataStore.ClearAll();
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
