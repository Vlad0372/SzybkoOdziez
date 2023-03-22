using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration;
using System.IO;
using System.Linq;


namespace SzybkoOdziez.Views
{
    public partial class VladsTestPage : ContentPage
    {
        public VladsTestPage()
        {
            InitializeComponent();
            ChoseRandImg();
           
        }
        private async void OnDislikeClicked(object sender, EventArgs args)
        {
            await dislikeButton.ScaleTo(0.75, 100);
            await dislikeButton.ScaleTo(1, 100);
        }
        private async void OnLikeClicked(object sender, EventArgs args)
        {
            await likeButton.ScaleTo(0.75, 100);
            await likeButton.ScaleTo(1, 100);
        }

        private void LoadImg()
        {
            //mainClothesImg.Source = "pack://application:,,,/Clothes/M/boots/item1.jpg";
            //mainClothesImg.Source = new BitmapImage(new Uri(chosenStickerPath, UriKind.Relative));
            mainClothesImg.Source = "@drawable/icon_about.png";
            //var bruh = Resource.Drawable.mt
            

        }
        private string ChoseRandImg()
        {
           //var bruh1 = System.IO.Directory.GetFiles("C:/Users/Vladislav/Desktop/Clothes/W/accessories");

            int[] range = { 1, 2, 3 };
            string[] sex = { "m", "w" };
            string[] category = { "acc", "boots", "pants", "tops", "underwear" };

            // Use whatever folder path you want here, the special folder is just an example
            //Assembly assembly = Assembly.GetExecutingAssembly();
            //var InternalPath = "android.resource://" + Android.App.Application.Context.PackageName;
            //var Path = (System.Environment.CurrentDirectory + "/Resources/Drawable");
            //var files = Directory.EnumerateFiles(Path);
            //var beruh = App.Current.Resources["Drawable"];
            return "bruh";
        }

        private string[] GetAllTxt()
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            string folderName = string.Format("{0}.Resources.drawable", executingAssembly.GetName().Name);
            return executingAssembly
                .GetManifestResourceNames()
                .Where(r => r.StartsWith(folderName) && r.EndsWith(".jpg"))
                .Select(r => r.Substring(folderName.Length + 1))
                .ToArray();
        }
    }
}