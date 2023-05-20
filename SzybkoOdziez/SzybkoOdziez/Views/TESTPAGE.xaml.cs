using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SzybkoOdziez.ViewModels;
using SzybkoOdziez.Models;

namespace SzybkoOdziez.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TESTPAGE : ContentPage
    {
        private ProductCommentsViewModel _viewModel;
        private Product _product;
        //private ObservableCollection<Comment> _comments;


        public TESTPAGE()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ViewModels.ProductCommentsViewModel();
        }
        public TESTPAGE(Product product)
        {
            InitializeComponent();
            BindingContext = _viewModel = new ViewModels.ProductCommentsViewModel();
            _product = product;
            Comment comment = new Comment();
            comment.Title = "SEX";
            comment.Content = "AHHAHAHAHHAHAHHAn\n\n\nHAHAHHAHAHA";
            _product.Comments.Add(comment);
            Comment comment2 = new Comment();
            comment2.Title = "CLEAN CODE";
            comment2.Content = "IS NOT REAL\nIT IS A MYTH\n\n\nTHEY ARE LYING";
            _product.Comments.Add(comment2);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnProductCommentsOpen(_product);
        }
    }
}