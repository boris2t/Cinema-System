﻿namespace THECinema.Web.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using THECinema.Common;
    using THECinema.Data.Models;
    using THECinema.Services.Data.Contracts;
    using THECinema.Services.Messaging;
    using THECinema.Web.ViewModels.Payments;
    using THECinema.Web.ViewModels.Reservations;

    [Authorize]
    public class PaymentsController : BaseController
    {
        private const string Subject = "A New Reservation at THECinema";

        private readonly IReservationsService reservationsService;
        private readonly IPaymentsService paymentsService;
        private readonly IEmailSender emailSender;
        private readonly UserManager<ApplicationUser> userManager;

        public PaymentsController(
            IReservationsService reservationsService,
            IPaymentsService paymentsService,
            IEmailSender emailSender,
            UserManager<ApplicationUser> userManager)
        {
            this.reservationsService = reservationsService;
            this.paymentsService = paymentsService;
            this.emailSender = emailSender;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Checkout(PaymentTypeInputModel inputModel)
        {
            var seatIds = this.TempData["seatIds"] as IEnumerable<string>;

            await this.reservationsService.MakeSeatsTakenAsync(seatIds, inputModel.ReservationId);
            await this.paymentsService.AddAsync(inputModel);

            if (inputModel.PaymentType == "Cash")
            {
                return this.RedirectToAction("ShowTicket", new { reservationId = inputModel.ReservationId });
            }

            var viewModel = new SimpleReservationViewModel
            {
                ReservationId = inputModel.ReservationId,
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> ShowTicket(string reservationId)
        {
            var viewModel = await this.reservationsService.GetByIdAsync(reservationId);
            if (viewModel == null)
            {
                return this.NotFound();
            }

            var customer = await this.userManager.GetUserAsync(this.User);
            var content = this.reservationsService.GenerateEmailContent(viewModel);

            await this.emailSender.SendEmailAsync(
                GlobalConstants.SystemEmail,
                GlobalConstants.SystemName,
                customer.Email,
                Subject,
                content);

            return this.View(viewModel);
        }
    }
}
