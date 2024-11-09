using Microsoft.AspNetCore.Mvc; // Включва основните класове и методи за работа с MVC (Model-View-Controller) архитектурата в ASP.NET Core.
using Microsoft.AspNetCore.Mvc.Rendering; // Предоставя функционалности за генериране на HTML елементи и други помощни методи в изгледите, като например списъци, dropdown менюта и други HTML елементи.
using Microsoft.EntityFrameworkCore; // Включва функционалности на Entity Framework Core, което е ORM (Object-Relational Mapping) инструмент за взаимодействие с бази данни.
using MvcTransplantation_kidney.Models; // Включва моделите, дефинирани в проекта 'MvcTransplantation_kidney'.


// Контролерът 'MessagesController' е дефиниран в пространството от имена 'MvcTransplantation_kidney.Controllers'.
// Контролерите в ASP.NET Core MVC наследяват класа 'Controller', което им позволява да обработват HTTP заявки и да връщат отговори.
namespace MvcTransplantation_kidney.Controllers
{
    // Дефиниция на контролера
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context; // Контекст на базата данни, чрез който контролерът има достъп до съобщенията.

        public MessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Messages
        // Декларира асинхронен метод, който връща 'Task<IActionResult>'. 'IActionResult' е интерфейс, който представлява резултат от действие (action) в контролера.
        // Връща изглед, който показва списък с всички съобщения в системата
        public async Task<IActionResult> Index()
        {
            // Заявка към базата данни, която извлича всички записи от таблицата със съобщения 'Messages'.
            // Метода 'ToListAsync()' преобразува резултата от заявката към списък 'List' асинхронно.
            // 'await' означава, че методът ще изчака асинхронното изпълнение на заявката, преди да продължи нататък.
            // Връща изглед (HTML страница), който ще бъде изпратен на клиента (браузъра).
            return View(await _context.Messages.ToListAsync());
        }

        // GET: Messages/Details/5
        // Връща детайли за определено съобщение, базиран на подаденото 'id'. Ако 'id' е null или съобщение с такова 'id' не съществува, връща се грешка "Not Found".
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Messages
                .FirstOrDefaultAsync(m => m.MessageId == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // GET: Messages/Create
        // Връща формуляр за създаване на ново съобщение.
        public IActionResult Create()
        {
            ViewData["Patients"] = new SelectList(_context.Patients.ToList(), "PatientId", "LastName");
            ViewData["Doctors"] = new SelectList(_context.Doctors.ToList(), "DoctorId", "LastName");
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Обработва изпратените данни от формуляра за създаване на съобщение. Ако моделът е валиден, добавя съобщението в базата данни и го записва.
        public async Task<IActionResult> Create([Bind("MessageId,DoctorId,PatientId,MessageText,SentDate")] Message message)
        {
            // Логване на грешки в ModelState. Ако моделът не е валиден (ModelState.IsValid е false), грешките се логват в конзолата. Това е полезно за отстраняване на проблеми по време на разработка, като показва конкретни съобщения за грешки.
            if (ModelState.IsValid)
            {
                _context.Add(message);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Patients"] = new SelectList(_context.Patients.ToList(), "PatientId", "LastName");
            ViewData["Doctors"] = new SelectList(_context.Doctors.ToList(), "DoctorId", "LastName");
            return View(message);
        }

        // GET: Messages/Edit/5
        // Връща изглед за редактиране на съществуващо съобщение, идентифициран чрез 'id'.  Ако 'id' е null или съобщение с такова 'id не съществува, връща се "Not Found".
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Messages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            ViewData["Patients"] = new SelectList(_context.Patients.ToList(), "PatientId", "LastName");
            ViewData["Doctors"] = new SelectList(_context.Doctors.ToList(), "DoctorId", "LastName");
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MessageId,DoctorId,PatientId,MessageText,SentDate")] Message message)
        {
            // Проверка за съществуването на съобщение. Ако съобщението, което се опитваме да актуализирме, вече не съществува в базата данни, методът връща "Not Found".
            if (id != message.MessageId)
            {
                return NotFound();
            }

            // Проверка на валидността на данните.  Ако 'ModelState' е валидно, контролерът се опитва да актуализира съобщението в базата данни. Ако има конфликт при актуализацията (напр. конкурентно записване), се хвърля изключение 'DbUpdateConcurrencyException'.
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(message);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageExists(message.MessageId))
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

            ViewData["Patients"] = new SelectList(_context.Patients.ToList(), "PatientId", "LastName");
            ViewData["Doctors"] = new SelectList(_context.Doctors.ToList(), "DoctorId", "LastName");
            return View(message);
        }

        // GET: Messages/Delete/5
        // Връща изгледа за изтриване на съобщение, идентифициран с 'id'. Ако 'id' е null или съобщението с такова 'id не съществува, връща се "Not Found".
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Messages
                .FirstOrDefaultAsync(m => m.MessageId == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        // Окончателно изтриване на съобщение от базата данни в ASP.NET Core MVC приложение.
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var message = await _context.Messages.FindAsync(id); // Чрез използване на метода 'FindAsync', системата търси съобщение в базата данни, което съответства на подаденето 'id'.
            if (message != null)
            {
                _context.Messages.Remove(message); // Проверка дали съобщението съществува
            }

            await _context.SaveChangesAsync(); // Записване на промените в базата данни
            return RedirectToAction(nameof(Index)); //Пренасочване към 'Index'
        }

        // Този метод 'MessageExists' е помощен метод, използван за проверка дали съществува съобщение в базата данни с определено 'id'.
        private bool MessageExists(int id)
        {
            return _context.Messages.Any(e => e.MessageId == id);
        }
    }
}
