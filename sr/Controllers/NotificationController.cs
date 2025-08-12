using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sr.Data;
using sr.Models;

namespace sr.Controllers
{

    public class NotificationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> ObterNotificacoes()
        {
            var userId = int.Parse(_userManager.GetUserId(User));

            var notificacoes = await _context.UserNotifications
                .Where(nau => nau.ApplicationUserId == userId)
                .Include(nau => nau.Notification)
                .OrderByDescending(nau => nau.Notification.Id)
                .Select(nau => new
                {
                    id = nau.NotificationId,
                    texto = nau.Notification.Text, 
                    url = Url.Action("Detalhes", "Notification", new { id = nau.NotificationId }),
                    isRead = nau.IsRead
                })
                .ToListAsync();

            return Ok(notificacoes);
        }

        [HttpPost]
        public async Task<IActionResult> MarcarComoLida(int id)
        {
            var userId = int.Parse(_userManager.GetUserId(User));

            var notificacaoUsuario = await _context.UserNotifications
                .FirstOrDefaultAsync(nau => nau.NotificationId == id && nau.ApplicationUserId == userId);

            if (notificacaoUsuario != null && !notificacaoUsuario.IsRead)
            {
                notificacaoUsuario.IsRead = true;
                _context.UserNotifications.Update(notificacaoUsuario);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }
}
