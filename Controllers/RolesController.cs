using Microsoft.AspNetCore.Mvc; // Това пространство от имена съдържа класове и интерфейси, които са основни за работата на ASP.NET Core MVC (Model-View-Controller) framework-а.
using Microsoft.EntityFrameworkCore; // Съдържа класове за работа с Entity Framework Core, ORM, който се използва за достъп до база данни.
using MvcTransplantation_kidney.Models; // Това е пространство от имена, дефинирано в рамките на самото приложение (специфично за проекта). То съдържа модели, които представляват данните в приложението.

// Контролерът 'RolesController' е дефиниран в пространството от имена 'MvcTransplantation_kidney.Controllers'.
// Контролерите в ASP.NET Core MVC наследяват класа 'Controller', което им позволява да обработват HTTP заявки и да връщат отговори.
namespace MvcTransplantation_kidney.Controllers
{

    // Дефиниция на контролера
    public class RolesController : Controller
    {
        private readonly ApplicationDbContext _context; // Контекст на базата данни, чрез който контролерът има достъп до ролевите данни.

        // Конструктор на контролера
        public RolesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Roles
        // Връща изглед, който показва списък с всички роли в системата
        public async Task<IActionResult> Index()
        {
            // Заявка към базата данни, която извлича всички записи от таблицата с потребители 'Roles'.
            // Метода 'ToListAsync()' преобразува резултата от заявката към списък 'List' асинхронно.
            // 'await' означава, че методът ще изчака асинхронното изпълнение на заявката, преди да продължи нататък.
            // Връща изглед (HTML страница), който ще бъде изпратен на клиента (браузъра).
            return View(await _context.Roles.ToListAsync());
        }

        // GET: Roles/Details/5
        // Връща детайли за определена роля, базирана на подаденото 'id'. Ако 'id' е null или роля с такова 'id' не съществува, връща се грешка "Not Found".
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Roles
                .FirstOrDefaultAsync(m => m.RoleId == id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // GET: Roles/Create
        // Връща формуляр за създаване на нова роля.
        public IActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Обработва изпратените данни от формуляра за създаване на роля. Ако моделът е валиден, добавя ролята в базата данни и я записва.
        public async Task<IActionResult> Create([Bind("RoleId,RoleName")] Role role)
        {
            // Логване на грешки в ModelState. Ако моделът не е валиден (ModelState.IsValid е false), грешките се логват в конзолата. Това е полезно за отстраняване на проблеми по време на разработка, като показва конкретни съобщения за грешки.
            if (ModelState.IsValid)
            {
                _context.Add(role);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Debugging purpose: log ModelState errors
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            return View(role);
        }

        // GET: Roles/Edit/5
        // Връща изглед за редактиране на съществуваща роля, идентифицирана чрез 'id'.  Ако 'id' е null или роля с такова 'id не съществува, връща се "Not Found".
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        
        
        public async Task<IActionResult> Edit(int id, [Bind("RoleId,RoleName")] Role role)
        {
            // Проверка за съществуването на ролята. Ако ролята, която се опитвате да актуализирате, вече не съществува в базата данни, методът връща "Not Found".
            if (id != role.RoleId)
            {
                return NotFound();
            }


            // Проверка на валидността на данните.  Ако 'ModelState' е валидно, контролерът се опитва да актуализира роля в базата данни. Ако има конфликт при актуализацията (напр. конкурентно записване), се хвърля изключение 'DbUpdateConcurrencyException'.
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(role);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleExists(role.RoleId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // Пренасочване след успех. След успешна актуализация, методът пренасочва към 'Index'.
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        // GET: Roles/Delete/5
        // Връща изгледа за изтриване на ролята, идентифицирана с 'id'. Ако 'id' е null или роля с такова 'id не съществува, връща се "Not Found".
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Roles
                .FirstOrDefaultAsync(m => m.RoleId == id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        // Окончателно изтриване на роля от базата данни в ASP.NET Core MVC приложение.
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var role = await _context.Roles.FindAsync(id); // Чрез използване на метода 'FindAsync', системата търси роля в базата данни, която съответства на подадения 'id'.
            if (role != null)
            {
                _context.Roles.Remove(role); // Проверка дали ролята съществува
            }

            await _context.SaveChangesAsync(); // Записване на промените в базата данни
            return RedirectToAction(nameof(Index)); //Пренасочване към 'Index'
        }

        // Този метод 'RoleExists' е помощен метод, използван за проверка дали съществува роля в базата данни с определен 'id'.
        private bool RoleExists(int id)
        {
            return _context.Roles.Any(e => e.RoleId == id);
        }
    }
}
