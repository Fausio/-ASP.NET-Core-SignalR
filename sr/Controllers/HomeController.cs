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
            string targetUser = "carlos@mail.com";

            var notificacao = new
            {
                texto = $"Você tem uma nova notificação às {DateTime.Now:T}",
                url = Url.Action("Detalhes", "Notificacao", new { id = 123 }) // exemplo
            };

            await _hubContext.Clients.Group(targetUser)
                .SendAsync("ReceiveNotification", notificacao);

            return Ok();
        }

    }
}
