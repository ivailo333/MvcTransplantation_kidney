using Microsoft.AspNetCore.Mvc; // Включва основните класове и методи за работа с MVC (Model-View-Controller) архитектурата в ASP.NET Core.
using Microsoft.AspNetCore.Mvc.Rendering; // Предоставя функционалности за генериране на HTML елементи и други помощни методи в изгледите, като например списъци, dropdown менюта и други HTML елементи.
using Microsoft.EntityFrameworkCore; // Включва функционалности на Entity Framework Core, което е ORM (Object-Relational Mapping) инструмент за взаимодействие с бази данни.
using MvcTransplantation_kidney.Models; // Включва моделите, дефинирани в проекта 'MvcTransplantation_kidney'.


// Контролерът 'PermissionsController' е дефиниран в пространството от имена 'MvcTransplantation_kidney.Controllers'.
// Контролерите в ASP.NET Core MVC наследяват класа 'Controller', което им позволява да обработват HTTP заявки и да връщат отговори.
namespace MvcTransplantation_kidney.Controllers
{
    // Дефиниране на контролера
    public class PermissionsController : Controller
    {
        private readonly ApplicationDbContext _context; // Контекст на базата данни, чрез който контролерът има достъп до разрешенията.

        public PermissionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Permissions
        // Декларира асинхронен метод, който връща 'Task<IActionResult>'. 'IActionResult' е интерфейс, който представлява резултат от действие (action) в контролера.
        // Връща изглед, който показва списък с всички разрешения в системата
        public async Task<IActionResult> Index()
        {
            // Заявка към базата данни, която извлича всички записи от таблицата с разрешения 'Permissions'.
            // Метода 'ToListAsync()' преобразува резултата от заявката към списък 'List' асинхронно.
            // 'await' означава, че методът ще изчака асинхронното изпълнение на заявката, преди да продължи нататък.
            // Връща изглед (HTML страница), който ще бъде изпратен на клиента (браузъра).
            return View(await _context.Permissions.ToListAsync());
        }

        // GET: Permissions/Details/5
        // Връща детайли за определено разрешение, базиран на подаденото 'id'. Ако 'id' е null или разрешение с такова 'id' не съществува, връща се грешка "Not Found".
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permission = await _context.Permissions
                .FirstOrDefaultAsync(m => m.PermissionId == id);
            if (permission == null)
            {
                return NotFound();
            }

            return View(permission);
        }

        // GET: Permissions/Create
        // Връща формуляр за създаване на ново разрешение.
        public IActionResult Create()
        {
            ViewBag.Users = new SelectList(_context.Users, "UserId", "UserName");
            return View();
        }

        // POST: Permissions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Обработва изпратените данни от формуляра за създаване на разрешение. Ако моделът е валиден, добавя разрешението в базата данни и го записва.
        public async Task<IActionResult> Create([Bind("PermissionId,UserId,PermissionType")] Permission permission)
        {
            // Логване на грешки в ModelState. Ако моделът не е валиден (ModelState.IsValid е false), грешките се логват в конзолата. Това е полезно за отстраняване на проблеми по време на разработка, като показва конкретни съобщения за грешки.
            if (ModelState.IsValid)
            {
                _context.Add(permission);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            else
            {
                var errors=ModelState.Values.SelectMany(x => x.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       
                }
            }

            ViewBag.Users = new SelectList(_context.Users, "UserId", "UserName", permission.UserId);
            return View(permission);
        }

        // GET: Permissions/Edit/5
        // Връща изглед за редактиране на съществуващо разрешение, идентифициран чрез 'id'.  Ако 'id' е null или разрешение с такова 'id не съществува, връща се "Not Found".
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permission = await _context.Permissions.FindAsync(id);
            if (permission == null)
            {
                return NotFound();
            }

            ViewBag.Users = new SelectList(_context.Users, "UserId", "UserName", permission.UserId);
            return View(permission);
        }

        // POST: Permissions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PermissionId,UserId,PermissionType")] Permission permission)
        {
            // Проверка за съществуването на разрешение. Ако разрешението, което се опитваме да актуализирме, вече не съществува в базата данни, методът връща "Not Found".
            if (id != permission.PermissionId)
            {
                return NotFound();
            }

            // Проверка на валидността на данните.  Ако 'ModelState' е валидно, контролерът се опитва да актуализира разрешението в базата данни. Ако има конфликт при актуализацията (напр. конкурентно записване), се хвърля изключение 'DbUpdateConcurrencyException'.
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(permission);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PermissionExists(permission.PermissionId))
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

            ViewBag.Users = new SelectList(_context.Users, "UserId", "UserName", permission.UserId);
            return View(permission);
        }

        // GET: Permissions/Delete/5
        // Връща изгледа за изтриване на разрешение, идентифициран с 'id'. Ако 'id' е null или разрешението с такова 'id не съществува, връща се "Not Found".
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permission = await _context.Permissions
                .FirstOrDefaultAsync(m => m.PermissionId == id);
            if (permission == null)
            {
                return NotFound();
            }

            return View(permission);
        }

        // POST: Permissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        // Окончателно изтриване на разрешение от базата данни в ASP.NET Core MVC приложение.
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var permission = await _context.Permissions.FindAsync(id); // Чрез използване на метода 'FindAsync', системата търси разрешение в базата данни, което съответства на подаденето 'id'.
            if (permission != null)
            {
                _context.Permissions.Remove(permission); // Проверка дали разрешението съществува
            }

            await _context.SaveChangesAsync(); // Записване на промените в базата данни
            return RedirectToAction(nameof(Index)); //Пренасочване към 'Index'
        }

        // Този метод 'PermissionExists' е помощен метод, използван за проверка дали съществува разрешение в базата данни с определено 'id'.
        private bool PermissionExists(int id)
        {
            return _context.Permissions.Any(e => e.PermissionId == id);
        }
    }
}
