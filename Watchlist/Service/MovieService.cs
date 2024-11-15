using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Watchlist.Entities;
using Watchlist.Response;

namespace Watchlist.Service
{
    public class MovieService : IMovieService
    {
        private readonly string _dataFilePath = Path.Combine(FileSystem.Current.AppDataDirectory, "movies.json");
        private readonly string _picturesFolderPath = Path.Combine(FileSystem.Current.AppDataDirectory, "Pictures");

        public MovieService()
        {
            Directory.CreateDirectory(_picturesFolderPath);
            Directory.CreateDirectory(Path.GetDirectoryName(_dataFilePath));
        }

        public async Task<ServiceResponse<List<Movie>>> GetMoviesAsync()
        {
            try
            {
                if (!File.Exists(_dataFilePath))
                    return new ServiceResponse<List<Movie>> { Data = new List<Movie>(), Success = true };

                var jsonData = await File.ReadAllTextAsync(_dataFilePath);
                var movies = JsonSerializer.Deserialize<List<Movie>>(jsonData);
                return new ServiceResponse<List<Movie>> { Data = movies, Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<Movie>> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ServiceResponse<Movie>> GetMovieByIdAsync(int id)
        {
            var moviesResponse = await GetMoviesAsync();
            if (!moviesResponse.Success)
                return new ServiceResponse<Movie> { Success = false, Message = moviesResponse.Message };

            var movie = moviesResponse.Data!.FirstOrDefault(m => m.Id == id);
            return movie != null
                ? new ServiceResponse<Movie> { Data = movie, Success = true }
                : new ServiceResponse<Movie> { Success = false, Message = "Movie not found." };
        }

        public async Task<ServiceResponse<bool>> AddMovieAsync(Movie movie, string picturePath)
        {
            try
            {
                var moviesResponse = await GetMoviesAsync();
                if (!moviesResponse.Success)
                    return new ServiceResponse<bool> { Success = false, Message = moviesResponse.Message };

                var movies = moviesResponse.Data;
                movie.Id = movies.Count > 0 ? movies.Max(m => m.Id) + 1 : 1;

                if (!string.IsNullOrEmpty(picturePath))
                {
                    var pictureName = $"{Guid.NewGuid()}{Path.GetExtension(picturePath)}";
                    var destinationPath = Path.Combine(_picturesFolderPath, pictureName);
                    File.Copy(picturePath, destinationPath);
                    movie.PicturePath = destinationPath;
                }

                movies.Add(movie);
                await SaveMoviesToFileAsync(movies);

                return new ServiceResponse<bool> { Data = true, Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool> { Success = false, Message = ex.Message };
            }
        }
        public async Task<ServiceResponse<bool>> UpdateMovieAsync(Movie movie, string picturePath)
        {
            try
            {
                var moviesResponse = await GetMoviesAsync();
                if (!moviesResponse.Success)
                    return new ServiceResponse<bool> { Success = false, Message = moviesResponse.Message };

                var movies = moviesResponse.Data;
                var existingMovie = movies!.FirstOrDefault(m => m.Id == movie.Id);
                if (existingMovie == null)
                    return new ServiceResponse<bool> { Success = false, Message = "Movie not found" };

                existingMovie.Title = movie.Title;
                existingMovie.Genre = movie.Genre;
                existingMovie.ReleaseYear = movie.ReleaseYear;
                existingMovie.Status = movie.Status;
                existingMovie.Rating = movie.Rating;
                existingMovie.Notes = movie.Notes;

                // Update picture if provided
                if (!string.IsNullOrEmpty(picturePath))
                {
                    var pictureName = $"{Guid.NewGuid()}{Path.GetExtension(picturePath)}";
                    var destinationPath = Path.Combine(_picturesFolderPath, pictureName);
                    File.Copy(picturePath, destinationPath, true);
                    existingMovie.PicturePath = destinationPath;
                }

                await SaveMoviesToFileAsync(movies);

                return new ServiceResponse<bool> { Data = true, Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ServiceResponse<bool>> DeleteMovieAsync(int id)
        {
            try
            {
                var moviesResponse = await GetMoviesAsync();
                if (!moviesResponse.Success)
                    return new ServiceResponse<bool> { Success = false, Message = moviesResponse.Message };

                var movies = moviesResponse.Data;
                var movie = movies.FirstOrDefault(m => m.Id == id);
                if (movie == null)
                    return new ServiceResponse<bool> { Success = false, Message = "Movie not found" };

                if (!string.IsNullOrEmpty(movie.PicturePath) && File.Exists(movie.PicturePath))
                {
                    File.Delete(movie.PicturePath);
                }

                movies.Remove(movie);
                await SaveMoviesToFileAsync(movies);

                return new ServiceResponse<bool> { Data = true, Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool> { Success = false, Message = ex.Message };
            }
        }

        private async Task SaveMoviesToFileAsync(List<Movie> movies)
        {
            var jsonData = JsonSerializer.Serialize(movies);
            await File.WriteAllTextAsync(_dataFilePath, jsonData);
        }
    }
}
