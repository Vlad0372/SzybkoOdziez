using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using SzybkoOdziez.Droid;
using System.Threading.Tasks;
using SzybkoOdziez.Services;
using System.IO;
using Android.Content.Res;
using Android;
using System.Reflection;

[assembly: Dependency(typeof(ImgArrayGetterService))]
namespace SzybkoOdziez.Droid
{
    public class ImgArrayGetterService : IImgArrayGetterService
    {      
        public List<string> GetImgArrayStreamAsync()
        {
            FieldInfo[] resourceDrawableImgs = typeof(Resource.Drawable).GetFields();
            List<string> nameList = new List<string>();

            for (int i = 0; i< resourceDrawableImgs.Length; i++)
            {
                if ((resourceDrawableImgs[i].Name[0] == 'm' || resourceDrawableImgs[i].Name[0] == 'w') && resourceDrawableImgs[i].Name[1] == '_')
                {
                    nameList.Add(resourceDrawableImgs[i].Name);
                }              
            }

            return nameList;
        }
    }
}