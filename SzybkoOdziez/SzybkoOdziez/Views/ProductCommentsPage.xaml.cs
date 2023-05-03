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

        //private async void PickImage_Clicked(object sender, EventArgs e)
        //{
        //    string filePath = "";
        //    try
        //    {
        //        var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
        //        {
        //            Title = "Pick an image"
        //        });

        //        if (result != null)
        //        {
        //            var stream = await result.OpenReadAsync();
        //            ImageComment.Source = ImageSource.FromStream(() => stream);
        //            Console.Write(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await DisplayAlert("Nie dziala", "ZEPSUTE", "ok...");
        //    }
        //}


        //private async void Button_Clicked(object sender, EventArgs e)
        //{

        //    await TakePhotoAsync();
        //}

        //string PhotoPath;
        //async Task TakePhotoAsync()
        //{
        //    try
        //    {
        //        var photo = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
        //        {
        //            Title = "Please pick a photo to use as an Avatar..."
        //        });
        //        await LoadPhotoAsync(photo);
        //        Console.WriteLine($"CapturePhotoAsync COMPLETED: {PhotoPath}");

        //        await DisplayAlert("in", PhotoPath, "OK");
        //    }
        //    catch (FeatureNotSupportedException fnsEx)
        //    {
        //        // Feature is not supported on the device  
        //    }
        //    catch (PermissionException pEx)
        //    {
        //        // Permissions not granted  
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
        //    }
        //}

        //async Task LoadPhotoAsync(FileResult photo)
        //{
        //    // canceled  
        //    if (photo == null)
        //    {
        //        PhotoPath = null;
        //        return;
        //    }
        //    // save the file into local storage  
        //    var newFile = System.IO.Path.Combine(FileSystem.AppDataDirectory, photo.FileName);


        //    using (var stream = await photo.OpenReadAsync())
        //    using (var newStream = File.OpenWrite(newFile))
        //        await stream.CopyToAsync(newStream);

        //    PhotoPath = newFile;
        //}


        //private async void Button_Clicked2(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        var result = await MediaPicker.PickPhotoAsync();
        //        if (result != null)
        //        {
        //            ImageComment.Source = result.FullPath;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle any exceptions that occur
        //    }
        //} 

    }
}