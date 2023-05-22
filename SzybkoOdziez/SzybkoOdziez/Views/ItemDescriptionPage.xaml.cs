using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SzybkoOdziez.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SzybkoOdziez.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDescriptionPage : ContentPage
    {
        private Product _product = new Product();

        public ItemDescriptionPage()
        {
            InitializeComponent();
        }
        public ItemDescriptionPage(Product product)
        {
            InitializeComponent();
            _product = product;
            if (_product != null)
            {
                sizePicker.Title = Convert.ToString(_product.Size);
                producerLabel.Text = _product.Producer;
                colorLabel.Text = _product.Color;
                seasonLabel.Text = _product.Season;
                materialLabel.Text = _product.Material;
                patternLabel.Text = _product.Pattern;
                modelLabel.Text = _product.Model;
                priceLabel.Text = Convert.ToString(_product.Price)+" zł";
                
                if (_product.Category == "Buty")
                {
                    var sizeList = new List<string>();
                    for (int i = 36; i <= 50; i++)
                    {
                        sizeList.Add(Convert.ToString(i));
                    }
                    sizePicker.ItemsSource = sizeList;
                }
                else
                {
                    if (_product.Category == "Spodnie" || _product.Category == "T-shirt" || _product.Category == "Marynarka" || _product.Category == "Kurtka" || _product.Category == "Koszula" || _product.Category == "Bluza")
                    {
                        var sizeList = new List<string>();
                        for (int i = 165; i <= 190; i++)
                        {
                            sizeList.Add(Convert.ToString(i));
                        }
                        sizePicker.ItemsSource = sizeList;
                    }
                    else
                    {
                        if (_product.Category == "Bielizna")
                        {
                            var sizeList = new List<string>();
                            for (int i = 34; i <= 44; i++)
                            {
                                sizeList.Add(Convert.ToString(i));
                            }
                            sizePicker.ItemsSource = sizeList;
                        }
                        else
                        {
                            if (_product.Category == "Czapka")
                            {
                                var sizeList = new List<string>();
                                for (int i = 53; i <= 63; i++)
                                {
                                    sizeList.Add(Convert.ToString(i));
                                }
                                sizePicker.ItemsSource = sizeList;
                            }
                            else
                            {
                                if (_product.Category == "Paski")
                                {
                                    var sizeList = new List<string>();
                                    for (int i = 75; i <= 125; i++)
                                    {
                                        sizeList.Add(Convert.ToString(i));
                                    }
                                    sizePicker.ItemsSource = sizeList;
                                }
                                else
                                {
                                    if (_product.Category == "Spódnice")
                                    {
                                        var sizeList = new List<string>();
                                        for (int i = 68; i <= 86; i++)
                                        {
                                            sizeList.Add(Convert.ToString(i));
                                        }
                                        sizePicker.ItemsSource = sizeList;
                                    }
                                    else
                                    {
                                        if (_product.Category == "Szalik")
                                        {
                                            var sizeList = new List<string>();
                                            for (int i = 53; i <= 62; i++)
                                            {
                                                sizeList.Add(Convert.ToString(i));
                                            }
                                            sizePicker.ItemsSource = sizeList;
                                        }
                                        else
                                        {
                                            if (_product.Category == "Rękawice")
                                            {
                                                var sizeList = new List<string>();
                                                for (int i = 17; i <= 27; i++)
                                                {
                                                    sizeList.Add(Convert.ToString(i));
                                                }
                                                sizePicker.ItemsSource = sizeList;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        
                    }
                }
            }
        }

        public async void OnLabelCommentsTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProductCommentsPage(_product));  
        }
    }
}