using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NAPA.Database;
using NAPA.Models;

namespace NAPA.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationContext context;

        public AdminController(ApplicationContext _context)
        {
            context = _context;
        }
        [HttpGet("user")]
        public IActionResult Index(string? fromDate, string? toDate)
        {
            DateTime dateFrom = DateTime.MinValue;
            if (!string.IsNullOrEmpty(fromDate))
            {

                DateTime.TryParse(fromDate, out dateFrom);
            }
            DateTime dateTo = DateTime.MaxValue;
            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime.TryParse(toDate, out dateTo);
            }

            List<User> result = context.Users.Where(i => i.CreatedDate >= dateFrom).Where(i => i.CreatedDate <= dateTo).ToList();
            return Ok(result);
        }
        public IActionResult AboutUsers()
        {
            return View(context.Users.ToList());
        }
        public IActionResult ProductAudit()
        {
            List<Audit> result = context.AuditLogs.ToList();
            return View(result);
        }
    }
}
