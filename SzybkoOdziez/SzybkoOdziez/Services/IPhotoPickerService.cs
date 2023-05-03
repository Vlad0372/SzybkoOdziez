using Android.Media;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SzybkoOdziez.Services
{
    public interface IPhotoPickerService
    {
        Task<Stream> GetImageStreamAsync();
    }
}
