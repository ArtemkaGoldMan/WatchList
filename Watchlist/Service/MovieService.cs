using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watchlist.Entities;

namespace Watchlist.Service
{
    public class MovieService : IMovieService
    {
        public Task AddMovieAsync(Movie movie)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMovieAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Movie> GetMovieByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Movie>> GetMovieListAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateMovieAsync(Movie movie)
        {
            throw new NotImplementedException();
        }
    }
}
