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

namespace SzybkoOdziez.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductCommentsPage : ContentPage
    {
        private ProductCommentsViewModel _viewModel;
        private Product _product;
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
            if (CheckIfCommentUnique(comment))
            {
                _product.Comments.Add(comment);
                _viewModel.OnProductCommentsOpen(_product);
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
    }
}