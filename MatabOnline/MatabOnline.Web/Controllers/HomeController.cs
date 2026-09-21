using Application.AppointmentAgg;
using Application.ContactMessageAgg;
using Application.DoctorAgg;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDoctorService _doctorService;
        private readonly IAppointmentService _appointmentService;
        private readonly IContactMessageService _contactMessageService;

        public HomeController(
            IDoctorService doctorService,
            IAppointmentService appointmentService,
            IContactMessageService contactMessageService)
        {
            _doctorService = doctorService;
            _appointmentService = appointmentService;
            _contactMessageService = contactMessageService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new HomeViewModel { Doctors = await _doctorService.GetAllAsync() };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookAppointment(CreateAppointmentDto appointmentForm)
        {
            if (!ModelState.IsValid)
            {
                // Rebuild the full page model so the view still has its doctor list,
                // but keep the user's typed-in values and the per-field errors.
                var viewModel = new HomeViewModel
                {
                    Doctors = await _doctorService.GetAllAsync(),
                    AppointmentForm = appointmentForm
                };
                return View(nameof(Index), viewModel);
            }

            var (id, error) = await _appointmentService.CreateAsync(appointmentForm);

            if (error is not null)
            {
                ModelState.AddModelError(string.Empty, error);
                var viewModel = new HomeViewModel
                {
                    Doctors = await _doctorService.GetAllAsync(),
                    AppointmentForm = appointmentForm
                };
                return View(nameof(Index), viewModel);
            }

            TempData["AppointmentSuccess"] = "درخواست نوبت شما با موفقیت ثبت شد";
            return RedirectToAction(nameof(Index)); // PRG: success only, so refresh doesn't resubmit
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendContactMessage(CreateContactMessageDto contactForm)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new HomeViewModel
                {
                    Doctors = await _doctorService.GetAllAsync(),
                    ContactForm = contactForm
                };
                return View(nameof(Index), viewModel);
            }

            await _contactMessageService.CreateAsync(contactForm);
            TempData["ContactSuccess"] = "پیام شما با موفقیت ارسال شد";
            return RedirectToAction(nameof(Index));
        }
    }
}
