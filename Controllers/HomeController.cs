using Microsoft.AspNetCore.Mvc; // ���� ������������ �� ����� ������� ������� � ����������, ����� �� ������� �� �������� �� ASP.NET Core MVC (Model-View-Controller) framework-�.
using MvcTransplantation_kidney.Models; // ���� � ������������ �� �����, ���������� � ������� �� ������ ���������� (���������� �� �������). �� ������� ������, ����� ������������� ������� � ������������.
using System.Diagnostics; // ���� ������������ �� ����� ���������� ������� �� ������ � ������������ �������, ���� �������� ������� � ������������ �� ������������ �� ����. ������ 'Activity', ����� �� �������� � ������ 'Error', � ���� �� 'System.Diagnostics' � ����� �� ������������ �� ���������� �� ������������ �� ����������, ���� ������� ������������� �� �������� (RequestId).


// ����������� 'HomeController' � ��������� � �������������� �� ����� 'MvcTransplantation_kidney.Controllers'.
// ������������ � ASP.NET Core MVC ���������� ����� 'Controller', ����� �� ��������� �� ���������� HTTP ������ � �� ������ ��������.
namespace MvcTransplantation_kidney.Controllers
{
    // ��������� �� ����������
    public class HomeController : Controller
    {
        // ������������� �� ����� �������� ���� ��������� ��������� ILogger<HomeController>, ����� �� �������� �� ������� �� ������� � ������� �� ����������.
        private readonly ILogger<HomeController> _logger;

        // ����������� �� ����������
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // ����� ������, ����� ���������� � ��������� �������� �� ������������. 'IActionResult' � ���������� ��� �� �������� �� �����, ����� ����� HTML �������.
        public IActionResult Index()
        {
            return View();
        }

        // ����� ������, ����� � �������� �� �������������.
        public IActionResult Privacy()
        {
            return View();
        }


        // ����� ������, ����� ������� ���������� �� ������, ����������� ����� 'ErrorViewModel'. ��������� 'ResponseCache' ���������, �� ��������� �� �� ������.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
