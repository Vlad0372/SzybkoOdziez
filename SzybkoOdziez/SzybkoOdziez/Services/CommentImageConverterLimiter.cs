using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace MyApp.Services
{
    public class CommentImageConverterLimiter : ICommentImageConverterLimiter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var imageUrls = value as IEnumerable<string>;
            if (imageUrls != null && imageUrls.Any())
            {
                return imageUrls.Take(4);
            }
            return Enumerable.Empty<string>();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}