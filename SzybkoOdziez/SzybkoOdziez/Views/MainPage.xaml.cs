﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SzybkoOdziez.Models;
using Xamarin.Forms;

namespace SzybkoOdziez.Views
{
    public partial class MainPage : ContentPage
    {
        private List<ProductInfo> productsList = new List<ProductInfo>();

        //list of liked products, used in Watchlist Page
        private ObservableCollection<ProductInfo> likedProductsList = new ObservableCollection<ProductInfo>();
        private void ShowMore(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ItemDescriptionPage());
        }
        public MainPage()
        {
            InitializeComponent();
            InitProductInfoList();
        }
        private async void OnDislikeClicked(object sender, EventArgs args)
        {
            var randProduct = GetRandProductInfo();
            productUrl.Source = randProduct.Url;
            productName.Text = randProduct.Name;
            productDesc.Text = randProduct.Description;
            productPrice.Text = randProduct.Price;

            await dislikeButton.ScaleTo(0.75, 100);
            await dislikeButton.ScaleTo(1, 100);        
        }
        private async void OnLikeClicked(object sender, EventArgs args)
        {
            //TODO zrefaktoryzowac zeby bralo product z productDataStore
            var app = (App)Application.Current;
            var wishlistDataStore = app.wishlistDataStore;
            var wishlistIEnumerable = await wishlistDataStore.GetItemsAsync();

            var currentProduct = new Product
            {
                Id = wishlistIEnumerable.Count() + 1,
                ImageUrl = new ImageSourceConverter().ConvertToInvariantString(productUrl.Source),
                Name = productName.Text,
                //Price = productPrice.Text,
                Price = Convert.ToDecimal(productPrice.Text.Split(',').First()),
                Description = productDesc.Text,
            };

            if (wishlistDataStore.CheckInDataStore(currentProduct))
            {
                await wishlistDataStore.AddItemAsync(currentProduct);
            }

            //var helperList = (ObservableCollection<ProductInfo>)Application.Current.Properties["likedProductsList"];
            //var helperProduct = new ProductInfo { Id = helperList.Count + 1, Name = productName.Text, Description = productDesc.Text, Price = productPrice.Text, Url = new ImageSourceConverter().ConvertToInvariantString(productUrl.Source) };

            ////powinno byc cos takiego, ale trzeba naprawic id
            ////if (!helperList.Contains(helperProduct))
            ////{
            ////    Application.Current.Properties.Remove("likedProductsList");
            ////    helperList.Add(helperProduct);
            ////    Application.Current.Properties.Add("likedProductsList", likedProductsList);
            ////    await Application.Current.SavePropertiesAsync();
            ////}

            ////!---------------- DO USUNIECIA PO POPRAWIENIU POBIERANIA/PRZYPISYWANIA ID ---------------!
            //bool notFoundInWishlist = true;
            //foreach (var helper in helperList)
            //{
            //    if (helperProduct.Name == helper.Name)
            //    {
            //        notFoundInWishlist = false;
            //    }
            //}
            //if (notFoundInWishlist)
            //{
            //    Application.Current.Properties.Remove("likedProductsList");
            //    helperList.Add(helperProduct);
            //    Application.Current.Properties.Add("likedProductsList", likedProductsList);
            //    await Application.Current.SavePropertiesAsync();
            //}
            //!----------------------------------- DO USUNIECIA ---------------------------------------!




            var randProduct = GetRandProductInfo();
            productUrl.Source = randProduct.Url;
            productName.Text = randProduct.Name;
            productDesc.Text = randProduct.Description;
            productPrice.Text = randProduct.Price;

            await likeButton.ScaleTo(0.75, 100);
            await likeButton.ScaleTo(1, 100);         
        }    
        private void InitProductInfoList()
        {
            List<string> imgsNameList = DependencyService.Get<IImgArrayGetterService>().GetImgArrayStreamAsync();
            
            for (int i = 0; i < imgsNameList.Count; i++)
            {
                var currentProduct = new ProductInfo();

                currentProduct.Id = i;
                currentProduct.Name = "prod_name_" + i;
                currentProduct.Description = "disc_" + i;
                currentProduct.Url = "@drawable/" + imgsNameList[i] + ".jpg";
                currentProduct.Price = ((i + 1 * 100 % 15) * 10).ToString() + ",00 zł";

                productsList.Add(currentProduct);
            }
        }
        private string GetRandImgPath()
        {
            var random = new System.Random();
            
            List<string> imgsNameList = DependencyService.Get<IImgArrayGetterService>().GetImgArrayStreamAsync();

            int index = random.Next(imgsNameList.Count);
            
            return  "@drawable/" + imgsNameList[index] + ".jpg";
        }
        private ProductInfo GetRandProductInfo()
        {
            var random = new System.Random();

            int index = random.Next(productsList.Count);

            return productsList[index];
        }
    }
}