using Microsoft.AspNetCore.Mvc;

namespace sr.Controllers
{
   
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

   

        
        [HttpPost]
        public async Task<IActionResult> SendNotification(string targetUserId, string message)
        {
            await _notificationService.SendNotificationAsync(targetUserId, message);
            return Json(new { success = true });
        }
    }
}
