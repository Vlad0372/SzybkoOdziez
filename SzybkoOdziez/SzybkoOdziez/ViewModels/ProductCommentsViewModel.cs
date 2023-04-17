using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SzybkoOdziez.Models;

namespace SzybkoOdziez.ViewModels
{
    public class ProductCommentsViewModel : BaseViewModel
    {
        public ObservableCollection<Comment> Comments { get; set; }

        public ProductCommentsViewModel()
        {
            Comments = new ObservableCollection<Comment>();
        }

        public ProductCommentsViewModel(ObservableCollection<Comment> comments)
        {
            Comments = comments;
        }

        public async void OnProductCommentsOpen(Product product)
        {
            await LoadCommentsAsync(product);
        }

        private async Task LoadCommentsAsync(Product product)
        {
            Comments.Clear();
            foreach (var comment in product.Comments)
            {
                Comments.Add(comment);
            }
        }

    }
}