using AuthPracticing.Models.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthPracticing.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly AuthPracticingDbContext _context;

        public EmployeeController(AuthPracticingDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Employees.ToListAsync());
        }
    }
}
