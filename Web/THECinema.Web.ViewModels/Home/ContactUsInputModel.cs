﻿namespace THECinema.Web.ViewModels.Home
{
    using System.ComponentModel.DataAnnotations;

    public class ContactUsInputModel
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        [MinLength(5)]
        public string Message { get; set; }
    }
}
