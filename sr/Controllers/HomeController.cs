using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using sr.Data;
using sr.infra;
using sr.Models;

namespace sr.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;
        private readonly IHubContext<SignalHub> _hubContext;

        public HomeController(IHubContext<SignalHub> hubContext, ApplicationDbContext applicationDbContext)
        {
            _hubContext = hubContext;
            _context = applicationDbContext;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //[HttpPost]
        //public async Task<IActionResult> EnviarNotificacao()
        //{
        //    string targetUser = "carlos@mail.com";

        //    var notificacao = new
        //    {
        //        texto = $"Você tem uma nova notificação às {DateTime.Now:T}",
        //        url = Url.Action("Detalhes", "Notificacao", new { id = 123 }) // exemplo
        //    };

        //    await _hubContext.Clients.Group(targetUser).SendAsync("ReceiveNotification", notificacao);

        //    return Ok();
        //}

        [HttpPost]
        public async Task<IActionResult> EnviarNotificacao()
        {
            try
            {
                string targetUser = "carlos@mail.com";
                var targetUserId = await _context.Users.FirstOrDefaultAsync(x => x.Email == targetUser) ?? throw new Exception("user not found");

                // Criar a notificação
                var notificacao = new Notification
                {
                    Text = $"Você tem uma nova notificação às {DateTime.Now:T}",
                    url = Url.Action("Detalhes", "Notificacao", new { id = 123 })  ,
                    NotificationApplicationUsers = new List<NotificationApplicationUser>
                {
                    new NotificationApplicationUser
                    {
                        ApplicationUserId = targetUserId.Id ,
                        IsRead = false
                    }
                }
                };

                // Salvar no banco de dados
                _context.Notifications.Add(notificacao);
                await _context.SaveChangesAsync();

                // Enviar via SignalR
                await _hubContext.Clients.Group(targetUser)
                    .SendAsync("ReceiveNotification", new
                    {
                        id = notificacao.Id,
                        texto = notificacao.Text,
                        url = Url.Action("Detalhes", "Notificacao", new { id = notificacao.Id })
                    });

                return Ok();
            }
            catch (Exception ex)
            {

                throw;
            }
        
        }

    }
}
