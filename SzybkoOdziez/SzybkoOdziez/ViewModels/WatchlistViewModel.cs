using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SzybkoOdziez.Models;
using SzybkoOdziez.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SzybkoOdziez.ViewModels
{
    public class WatchlistViewModel : BaseViewModel
    {
        public ObservableCollection<ProductInfo> Products { get; set; }

        public WatchlistViewModel()
        {
            List<string> imgsNameList = DependencyService.Get<IImgArrayGetterService>().GetImgArrayStreamAsync();

            Products = new ObservableCollection<ProductInfo>
            {
            new ProductInfo { Id = 1, Name = "HAHAHA", Description = "OKEJ, CZY TO DZIALA?!?!", Price = "5.00 zl", Url = "@drawable/" + imgsNameList[1] + ".jpg" }
            };

            
        }
    }
}