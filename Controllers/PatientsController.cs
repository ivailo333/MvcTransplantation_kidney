using Microsoft.AspNetCore.Mvc; // Включва основните класове и методи за работа с MVC (Model-View-Controller) архитектурата в ASP.NET Core.
using Microsoft.EntityFrameworkCore; // Включва функционалности на Entity Framework Core, което е ORM (Object-Relational Mapping) инструмент за взаимодействие с бази данни.
using MvcTransplantation_kidney.Models; // Включва моделите, дефинирани в проекта 'MvcTransplantation_kidney'.


// Контролерът 'PatientsController' е дефиниран в пространството от имена 'MvcTransplantation_kidney.Controllers'.
// Контролерите в ASP.NET Core MVC наследяват класа 'Controller', което им позволява да обработват HTTP заявки и да връщат отговори.
namespace MvcTransplantation_kidney.Controllers
{
    // Дефиниция на контролера
    public class PatientsController : Controller
    {
        private readonly ApplicationDbContext _context; // Контекст на базата данни, чрез който контролерът има достъп до данните на пациентите.

        // Конструктор на контролера
        public PatientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Patients
        // Декларира асинхронен метод, който връща 'Task<IActionResult>'. 'IActionResult' е интерфейс, който представлява резултат от действие (action) в контролера.
        // Връща изглед, който показва списък с всички пациенти в системата
        public async Task<IActionResult> Index()
        {
            // Заявка към базата данни, която извлича всички записи от таблицата с пациенти 'Patients'.
            // Метода 'ToListAsync()' преобразува резултата от заявката към списък 'List' асинхронно.
            // 'await' означава, че методът ще изчака асинхронното изпълнение на заявката, преди да продължи нататък.
            // Връща изглед (HTML страница), който ще бъде изпратен на клиента (браузъра).
            return View(await _context.Patients.ToListAsync());
        }

        // GET: Patients/Details/5
        // Връща детайли за определен пациент, базиран на подаденото 'id'. Ако 'id' е null или пациент с такова 'id' не съществува, връща се грешка "Not Found".
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.PatientId == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patients/Create
        // Връща формуляр за създаване на нов пациент.
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Обработва изпратените данни от формуляра за създаване на пациент. Ако моделът е валиден, добавя данните на пациента в базата данни и ги записва.
        public async Task<IActionResult> Create([Bind("PatientId,FirstName,LastName,DateOfBirth,TransplantDate")] Patient patient)
        {
            // Логване на грешки в ModelState. Ако моделът не е валиден (ModelState.IsValid е false), грешките се логват в конзолата. Това е полезно за отстраняване на проблеми по време на разработка, като показва конкретни съобщения за грешки.
            if (ModelState.IsValid)
            {
                _context.Add(patient);
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

            return View(patient);
        }

        // GET: Patients/Edit/5
        // Връща изглед за редактиране на съществуващ пациент, идентифициран чрез 'id'.  Ако 'id' е null или пациент с такова 'id не съществува, връща се "Not Found".
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PatientId,FirstName,LastName,DateOfBirth,TransplantDate")] Patient patient)
        {
            // Проверка за съществуването на пациента. Ако данните на пациента, които се опитваме да актуализираме, вече не съществува в базата данни, методът връща "Not Found".
            if (id != patient.PatientId)
            {
                return NotFound();
            }

            // Проверка на валидността на данните.  Ако 'ModelState' е валидно, контролерът се опитва да актуализира данните на пациента в базата данни. Ако има конфликт при актуализацията (напр. конкурентно записване), се хвърля изключение 'DbUpdateConcurrencyException'.
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.PatientId))
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
            return View(patient);
        }

        // GET: Patients/Delete/5
        // Връща изгледа за изтриване на данните на пациента, идентифициран с 'id'. Ако 'id' е null или пациент с такова 'id не съществува, връща се "Not Found".
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.PatientId == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        // Окончателно изтриване на пациент от базата данни в ASP.NET Core MVC приложение.
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _context.Patients.FindAsync(id); // Чрез използване на метода 'FindAsync', системата търси пациент в базата данни, който съответства на подадения 'id'.
            if (patient != null)
            {
                _context.Patients.Remove(patient); // Проверка дали пациента съществува
            }

            await _context.SaveChangesAsync(); // Записване на промените в базата данни
            return RedirectToAction(nameof(Index)); //Пренасочване към 'Index'
        }

        // Този метод 'PatientExists' е помощен метод, използван за проверка дали съществува пациент в базата данни с определен 'id'.
        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.PatientId == id);
        }
    }
}
