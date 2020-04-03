﻿namespace THECinema.Web.ViewModels.Reviews
{
    using System.ComponentModel.DataAnnotations;

    using THECinema.Data.Models;
    using THECinema.Services.Mapping;

    public class AddReviewInputModel : IMapTo<Review>
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public double Stars { get; set; }

        public int MovieId { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }
    }
}