using Microsoft.AspNetCore.Mvc; // Това пространство от имена съдържа класове и интерфейси, които са основни за работата на ASP.NET Core MVC (Model-View-Controller) framework-а.
using MvcTransplantation_kidney.Models; // Това е пространство от имена, дефинирано в рамките на самото приложение (специфично за проекта). То съдържа модели, които представляват данните в приложението.
using System.Diagnostics; // Това пространство от имена предоставя класове за работа с диагностични функции, като например логване и проследяване на изпълнението на кода. Класът 'Activity', който се използва в метода 'Error', е част от 'System.Diagnostics' и служи за проследяване на информация за изпълнението на програмата, като текущия идентификатор на заявката (RequestId).


// Контролерът 'HomeController' е дефиниран в пространството от имена 'MvcTransplantation_kidney.Controllers'.
// Контролерите в ASP.NET Core MVC наследяват класа 'Controller', което им позволява да обработват HTTP заявки и да връщат отговори.
namespace MvcTransplantation_kidney.Controllers
{
    // Дефиниция на контролера
    public class HomeController : Controller
    {
        // Конструкторът на класа получава като параметър интерфейс ILogger<HomeController>, който се използва за логване на събития в рамките на контролера.
        private readonly ILogger<HomeController> _logger;

        // Конструктор на контролера
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Връща изглед, който обикновено е началната страница на приложението. 'IActionResult' е стандартен тип за резултат на метод, който връща HTML отговор.
        public IActionResult Index()
        {
            return View();
        }

        // Връща изглед, който е страница за поверителност.
        public IActionResult Privacy()
        {
            return View();
        }


        // Връща изглед, който съдържа информация за грешка, използвайки модел 'ErrorViewModel'. Атрибутът 'ResponseCache' гарантира, че отговорът не се кешира.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
