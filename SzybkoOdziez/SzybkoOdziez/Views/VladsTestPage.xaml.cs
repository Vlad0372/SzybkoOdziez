using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace SzybkoOdziez.Views
{
    public partial class VladsTestPage : ContentPage
    {
        public VladsTestPage()
        {
            InitializeComponent();        
        }
        private async void OnDislikeClicked(object sender, EventArgs args)
        {
            mainClothesImg.Source = GetRandImgPath();

            await dislikeButton.ScaleTo(0.75, 100);
            await dislikeButton.ScaleTo(1, 100);        
        }
        private async void OnLikeClicked(object sender, EventArgs args)
        {
            mainClothesImg.Source = GetRandImgPath();

            await likeButton.ScaleTo(0.75, 100);
            await likeButton.ScaleTo(1, 100);         
        }    
        private string GetRandImgPath()
        {
            var random = new System.Random();
            
            List<string> imgsNameList = DependencyService.Get<IImgArrayGetterService>().GetImgArrayStreamAsync();

            int index = random.Next(imgsNameList.Count);
            
            return  "@drawable/" + imgsNameList[index] + ".jpg";
        }
    }
}