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
            Comment comment = new Comment();
            comment.Title = "XD";
            comment.Description = "Lorem ipsum i tak dalej";
            _product.Comments.Add(comment);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnProductCommentsOpen(_product);
        }

        public void OnAddCommentButtonClicked(object sender, EventArgs args)
        {
            Comment comment = new Comment();
            comment.Title = CommentTitleEntry.Text;
            comment.Description = CommentTextEditor.Text;
            _product.Comments.Add(comment);
        }
    }
}