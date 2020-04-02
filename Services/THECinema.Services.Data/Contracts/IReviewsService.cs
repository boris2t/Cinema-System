﻿namespace THECinema.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using THECinema.Data.Models;
    using THECinema.Web.ViewModels.Reviews;

    public interface IReviewsService
    {
        Task AddAsync(AddReviewInputModel inputModel);

        IEnumerable<T> GetAllByMovieId<T>(int movieId);
    }
}
