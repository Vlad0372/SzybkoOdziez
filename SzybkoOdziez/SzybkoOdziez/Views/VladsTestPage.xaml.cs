using Android.Content;
using Android.App;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration;
using System.IO;
using Java.Util;
using System.Linq;
using Android.Graphics.Drawables;
using Android;
using Java.IO;

namespace SzybkoOdziez.Views
{
    public partial class VladsTestPage : ContentPage
    {
        public VladsTestPage()
        {
            InitializeComponent();        
        }
        private void OnlikeClicked(object sender, EventArgs args)
        {
            mainClothesImg.Source = GetRandImgPath();
        }
        private void OnDislikeClicked(object sender, EventArgs args)
        {
            mainClothesImg.Source = GetRandImgPath();
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