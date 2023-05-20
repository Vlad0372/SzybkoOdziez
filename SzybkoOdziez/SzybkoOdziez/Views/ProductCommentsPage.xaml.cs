using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SzybkoOdziez.Services;
using SzybkoOdziez.Models;
using SzybkoOdziez.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Android.Provider;
using System.Windows;
using Acr.UserDialogs;
using Xamarin.Essentials;
using Android.Graphics;
using System.IO;
using static Java.Util.Jar.Attributes;

namespace SzybkoOdziez.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductCommentsPage : ContentPage
    {
        private ProductCommentsViewModel _viewModel;
        private Product _product;
        private string _fullPath;
        //private ObservableCollection<Comment> _comments;


        public ProductCommentsPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ViewModels.ProductCommentsViewModel();
        }
        public ProductCommentsPage(Product product)
        {
            InitializeComponent();
            BindingContext = _viewModel = new ViewModels.ProductCommentsViewModel();

            if (product.Comments == null) { product.Comments = new List<Comment>(); }
            _product = product;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnProductCommentsOpen(_product);
        }

        public async void OnAddCommentButtonClicked(object sender, EventArgs args)
        {
            Comment comment = new Comment();
            comment.Title = CommentTitleEntry.Text;
            comment.Description = CommentTextEditor.Text;
            if(_fullPath != null)
            {
                comment.CommentImageSource = _fullPath;
            }

            if (CheckIfCommentUnique(comment))
            {
                _product.Comments.Add(comment);
                _viewModel.OnProductCommentsOpen(_product);
                _fullPath = null;
            }
            else
            {
                await DisplayAlert("Duplikat!", "Komentarz o takim tytule już istnieje!", "Ok");
            }
        }

        public bool CheckIfCommentUnique(Comment comment)
        {
            bool isUnique = true;
            foreach (Comment productcomment in _product.Comments)
            {
                if (productcomment.Title == comment.Title)
                {
                    isUnique = false;
                }
            }
            return isUnique;
        }


        public async void ButtonSelectImage_Clicked(object sender, EventArgs args)
        {
            //zdjecia komentarzy nie sa nigdzie zapisywane, jedynie zapisywana jest pelna sciezka do zdjecia na urzadzeniu
            try
            {
                var result = await MediaPicker.PickPhotoAsync();
                if (result != null)
                {
                    var stream = await result.OpenReadAsync();
                    ImageSource imageSource = ImageSource.FromStream(() => stream);
                    _fullPath = result.FullPath;
                    
                    //SaveFile(stream);
                }
            }
            catch (FeatureNotSupportedException)
            {
                await DisplayAlert("Error", "Feature not supported exception", "Ok");
            }
            catch (PermissionException)
            {
                await DisplayAlert("Error", "Permission exception", "Ok");
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "Exception", "Ok");
            }
        }

    }
}