using Microsoft.AspNetCore.Mvc;
using Edu_DB_ASP.Models;
using System.Linq;

namespace Edu_DB_ASP.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly EduDbContext _context;

        public NotificationsController(EduDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult MarkAsRead(int NotificationId)
        {
            var notification = _context.Notifications.FirstOrDefault(n => n.NotificationId == NotificationId);
            if (notification != null)
            {
                notification.ReadStatus = true;
                _context.SaveChanges();
            }

            return RedirectToAction("Notifications");
        }
        
        public IActionResult Notifications()
        {
            var notifications = _context.Notifications.ToList();
            return View(notifications);
        }
    }
}