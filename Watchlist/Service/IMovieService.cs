using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watchlist.Entities;
using Watchlist.Message;
using Watchlist.Response;

namespace Watchlist.Service
{
    public interface IMovieService
    {
        Task<ServiceResponse<List<Movie>>> GetMoviesAsync();
        Task<ServiceResponse<Movie>> GetMovieByIdAsync(int id);
        Task<ServiceResponse<bool>> AddMovieAsync(Movie movie, string picturePath);
        Task<ServiceResponse<bool>> UpdateMovieAsync(Movie movie, string picturePath);
        Task<ServiceResponse<bool>> DeleteMovieAsync(int id);
    }

}
