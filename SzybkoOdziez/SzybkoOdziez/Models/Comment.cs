using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SzybkoOdziez.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; } 
        public List<string> CommentImgUrls { get; set; }
        public string CommentImageSource { get; set; } 
    }

}
