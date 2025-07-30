using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using sr.infra;
using sr.Models;

namespace sr.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IHubContext<SignalHub> _hubContext;

        public HomeController(IHubContext<SignalHub> hubContext)
        {
            _hubContext = hubContext;
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

        [HttpPost]
        public async Task<IActionResult> EnviarNotificacao()
        {
            string targetUser = "carlos@mail.com"; // Exemplo. Use User.Identity.Name se quiser notificar o usuário atual.

            string message = $"Você tem uma nova notificação às {DateTime.Now:T}";

            await _hubContext.Clients.Group(targetUser).SendAsync("ReceiveNotification", message);

            return Ok();
        }
    }
}
