﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SzybkoOdziez.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SzybkoOdziez.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (userLogin.Text == "admin" && userPass.Text == "admin")
            {

                await Shell.Current.GoToAsync("//MainPage");
                Navigation.RemovePage(this);


            }
            else
            {
                DisplayAlert("UPS...!", "Podałeś złe hasło albo nazwę użytkownika", "Spróbuj ponownie");
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegistrationPage());
            Navigation.RemovePage(this);
        }
    }
}