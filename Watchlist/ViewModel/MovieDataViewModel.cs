using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using Watchlist.Entities;
using Watchlist.Service;
using Microsoft.Maui.Media;
using Watchlist.Message;
using System;

namespace Watchlist.ViewModel
{
    public partial class MovieDataViewModel : ObservableObject
    {
        private readonly IMovieService _movieService;
        private readonly IMessageDialogService _messageDialogService;

        [ObservableProperty]
        private Movie movie = new Movie();

        public MovieDataViewModel(IMovieService movieService, IMessageDialogService messageDialogService)
        {
            _movieService = movieService;
            _messageDialogService = messageDialogService;
        }

        public IAsyncRelayCommand SaveCommand => new AsyncRelayCommand(SaveMovieAsync);
        public IAsyncRelayCommand PickImageCommand => new AsyncRelayCommand(PickImageAsync);

        public void InitializeMovie(Movie existingMovie = null)
        {
            Movie = existingMovie ?? new Movie { ReleaseYear = DateTime.Today };
        }

        private async Task SaveMovieAsync()
        {
            if (Movie.Id == 0)
            {
                var response = await _movieService.AddMovieAsync(Movie, Movie.PicturePath);
                if (response.Success)
                {
                    _messageDialogService.ShowMessage("Movie added successfully.");
                }
                else
                {
                    _messageDialogService.ShowMessage($"Error: {response.Message}");
                    return;
                }
            }
            else
            {
                var response = await _movieService.UpdateMovieAsync(Movie, Movie.PicturePath);
                if (response.Success)
                {
                    _messageDialogService.ShowMessage("Movie updated successfully.");
                }
                else
                {
                    _messageDialogService.ShowMessage($"Error: {response.Message}");
                    return;
                }
            }

            await Shell.Current.GoToAsync("..");
        }

        private async Task PickImageAsync()
        {
            try
            {
                // Check if MediaPicker is supported and available on the device
                if (MediaPicker.IsCaptureSupported)
                {
                    // Pick a photo from the gallery
                    var photo = await MediaPicker.PickPhotoAsync();

                    if (photo != null)
                    {
                        // Get the file path of the selected photo
                        Movie.PicturePath = photo.FullPath;
                        OnPropertyChanged(nameof(Movie.PicturePath)); // Notify UI of the updated image path
                    }
                }
                else
                {
                    _messageDialogService.ShowMessage("Photo gallery access is not supported on this device.");
                }
            }
            catch (Exception ex)
            {
                _messageDialogService.ShowMessage($"Error accessing gallery: {ex.Message}");
            }
        }
    }
}
