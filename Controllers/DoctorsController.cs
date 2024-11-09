using Microsoft.AspNetCore.Mvc; // Включва основните класове и методи за работа с MVC (Model-View-Controller) архитектурата в ASP.NET Core.
using Microsoft.AspNetCore.Mvc.Rendering; // Предоставя функционалности за генериране на HTML елементи и други помощни методи в изгледите, като например списъци, dropdown менюта и други HTML елементи.
using Microsoft.EntityFrameworkCore; // Включва функционалности на Entity Framework Core, което е ORM (Object-Relational Mapping) инструмент за взаимодействие с бази данни.
using MvcTransplantation_kidney.Models; // Включва моделите, дефинирани в проекта 'MvcTransplantation_kidney'.


// Контролерът 'DoctorsController' е дефиниран в пространството от имена 'MvcTransplantation_kidney.Controllers'.
// Контролерите в ASP.NET Core MVC наследяват класа 'Controller', което им позволява да обработват HTTP заявки и да връщат отговори.
namespace MvcTransplantation_kidney.Controllers
{
    // Дефиниция на контролера
    public class DoctorsController : Controller
    {
        private readonly ApplicationDbContext _context; // Контекст на базата данни, чрез който контролерът има достъп до данните на докторите.

        // Конструктор на контролера
        public DoctorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Doctors
        // Декларира асинхронен метод, който връща 'Task<IActionResult>'. 'IActionResult' е интерфейс, който представлява резултат от действие (action) в контролера.
        // Връща изглед, който показва списък с всички доктори в системата
        public async Task<IActionResult> Index()
        {
            // Заявка към базата данни, която извлича всички записи от таблицата с доктори 'Doctors'.
            // Метода 'ToListAsync()' преобразува резултата от заявката към списък 'List' асинхронно.
            // 'await' означава, че методът ще изчака асинхронното изпълнение на заявката, преди да продължи нататък.
            // Връща изглед (HTML страница), който ще бъде изпратен на клиента (браузъра).
            return View(await _context.Doctors.ToListAsync());
        }

        // GET: Doctors/Details/5
        // Връща детайли за определен доктор, базиран на подаденото 'id'. Ако 'id' е null или доктор с такова 'id' не съществува, връща се грешка "Not Found".
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(m => m.DoctorId == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // GET: Doctors/Create
        // Връща формуляр за създаване на нов доктор.
        public IActionResult Create()
        {
            ViewBag.Users = new SelectList(_context.Users, "UserId", "UserId");
            ViewBag.Specialties = new SelectList(new List<string> { "Nephrologist", "Specialist in Nephrology" });
            return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Обработва изпратените данни от формуляра за създаване на доктор. Ако моделът е валиден, добавя данните на доктора в базата данни и ги записва.
        public async Task<IActionResult> Create([Bind("DoctorId,UserId,FirstName,LastName,Specialty")] Doctor doctor)
        {
            // Логване на грешки в ModelState. Ако моделът не е валиден (ModelState.IsValid е false), грешките се логват в конзолата. Това е полезно за отстраняване на проблеми по време на разработка, като показва конкретни съобщения за грешки.
            if (ModelState.IsValid)
            {
                _context.Add(doctor);
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

            ViewBag.Users = new SelectList(_context.Users, "UserId", "UserId",doctor.UserId);
            ViewBag.Specialties = new SelectList(new List<string> { "Nephrologist", "Specialist in Nephrology" });
            return View(doctor);
        }

        // GET: Doctors/Edit/5
        // Връща изглед за редактиране на съществуващ доктор, идентифициран чрез 'id'.  Ако 'id' е null или доктор с такова 'id не съществува, връща се "Not Found".
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            ViewBag.Users = new SelectList(_context.Users, "UserId", "UserId", doctor.UserId);
            ViewBag.Specialties = new SelectList(new List<string> { "Nephrologist", "Specialist in Nephrology" });
            return View(doctor);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DoctorId,UserId,FirstName,LastName,Specialty")] Doctor doctor)
        {
            // Проверка за съществуването на доктора. Ако данните на доктора, които се опитваме да актуализирме, вече не съществуват в базата данни, методът връща "Not Found".
            if (id != doctor.DoctorId)
            {
                return NotFound();
            }

            // Проверка на валидността на данните.  Ако 'ModelState' е валидно, контролерът се опитва да актуализира данните на доктора в базата данни. Ако има конфликт при актуализацията (напр. конкурентно записване), се хвърля изключение 'DbUpdateConcurrencyException'.
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doctor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorExists(doctor.DoctorId))
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

            ViewBag.Users = new SelectList(_context.Users, "UserId", "UserId", doctor.UserId);
            ViewBag.Specialties = new SelectList(new List<string> { "Nephrologist", "Specialist in Nephrology" });
            return View(doctor);
        }

        // GET: Doctors/Delete/5
        // Връща изгледа за изтриване на данните на доктора, идентифициран с 'id'. Ако 'id' е null или доктор с такова 'id не съществува, връща се "Not Found".
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(m => m.DoctorId == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        // Окончателно изтриване на доктор от базата данни в ASP.NET Core MVC приложение.
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id); // Чрез използване на метода 'FindAsync', системата търси доктор в базата данни, който съответства на подадения 'id'.
            if (doctor != null)
            {
                _context.Doctors.Remove(doctor); // Проверка дали доктора съществува
            }

            await _context.SaveChangesAsync(); // Записване на промените в базата данни
            return RedirectToAction(nameof(Index)); //Пренасочване към 'Index'
        }

        // Този метод 'DoctorExists' е помощен метод, използван за проверка дали съществува доктор в базата данни с определен 'id'.
        private bool DoctorExists(int id)
        {
            return _context.Doctors.Any(e => e.DoctorId == id);
        }
    }
}
