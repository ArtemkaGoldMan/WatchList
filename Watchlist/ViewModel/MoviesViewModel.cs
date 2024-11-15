using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Watchlist.Entities;
using Watchlist.Service;
using Watchlist.Message;
using Watchlist.Pages;
using Microsoft.Maui.Controls;
using System;

namespace Watchlist.ViewModel
{
    public partial class MoviesViewModel : ObservableObject
    {
        private readonly IMovieService _movieService;
        private readonly IMessageDialogService _messageDialogService;

        [ObservableProperty]
        private ObservableCollection<Movie> movies;

        public MoviesViewModel(IMovieService movieService, IMessageDialogService messageDialogService)
        {
            _movieService = movieService;
            _messageDialogService = messageDialogService;
            LoadMoviesCommand = new AsyncRelayCommand(LoadMoviesAsync);
            AddMovieCommand = new AsyncRelayCommand(OpenAddMoviePageAsync);
            DeleteMovieCommand = new AsyncRelayCommand<int>(DeleteMovieAsync);
            UpdateMovieCommand = new AsyncRelayCommand<Movie>(OpenUpdateMoviePageAsync);
        }

        public IAsyncRelayCommand LoadMoviesCommand { get; }
        public IAsyncRelayCommand AddMovieCommand { get; }
        public IAsyncRelayCommand<int> DeleteMovieCommand { get; }
        public IAsyncRelayCommand<Movie> UpdateMovieCommand { get; }

        private async Task LoadMoviesAsync()
        {
            var response = await _movieService.GetMoviesAsync();
            if (response.Success)
            {
                Movies = new ObservableCollection<Movie>(response.Data ?? new List<Movie>());
            }
            else
            {
                _messageDialogService.ShowMessage($"Error: {response.Message}");
            }
        }

        private async Task OpenAddMoviePageAsync()
        {
            // Navigate to MovieDataPage with a new Movie instance for adding a new entry
            await Shell.Current.GoToAsync(nameof(MovieDataPage), new Dictionary<string, object> { { "Movie", new Movie { ReleaseYear = DateTime.Today } } });
        }

        private async Task OpenUpdateMoviePageAsync(Movie movie)
        {
            // Navigate to MovieDataPage with the selected Movie for updating
            await Shell.Current.GoToAsync(nameof(MovieDataPage), new Dictionary<string, object> { { "Movie", movie } });
        }

        public async Task AddMovieAsync(Movie movie)
        {
            var response = await _movieService.AddMovieAsync(movie, movie.PicturePath);
            if (response.Success)
            {
                Movies.Add(movie); // Directly add the new movie to the collection
            }
            else
            {
                _messageDialogService.ShowMessage($"Error: {response.Message}");
            }
        }

        private async Task DeleteMovieAsync(int id)
        {
            var response = await _movieService.DeleteMovieAsync(id);
            if (response.Success)
            {
                await LoadMoviesAsync();
            }
            else
            {
                _messageDialogService.ShowMessage($"Error: {response.Message}");
            }
        }
    }
}
