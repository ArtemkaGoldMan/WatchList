using Watchlist.ViewModel;

namespace Watchlist.Pages
{
    public partial class MovieDataPage : ContentPage
    {
        public MovieDataPage(MovieDataViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
