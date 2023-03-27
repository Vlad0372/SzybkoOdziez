using System;
using System.Collections.Generic;
using SzybkoOdziez.Models;
using Xamarin.Forms;

namespace SzybkoOdziez.Views
{
    public partial class VladsTestPage : ContentPage
    {
        private List<ProductInfo> productsList = new List<ProductInfo>();
        public VladsTestPage()
        {
            InitializeComponent(); 
            InitProductInfoList();
        }
        private void ShowItemDescription(object sender, EventArgs args)
        {
            Navigation.PushAsync(new ItemDescriptionPage());
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