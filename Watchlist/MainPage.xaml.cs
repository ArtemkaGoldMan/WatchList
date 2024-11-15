using Watchlist.ViewModel;

namespace Watchlist
{
    public partial class MainPage : ContentPage
    {
        private readonly MoviesViewModel _viewModel;
        public MainPage(MoviesViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadMoviesCommand.ExecuteAsync(null);
        }
    }

}
