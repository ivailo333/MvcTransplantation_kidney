using Microsoft.AspNetCore.Mvc; // Включва основните класове и методи за работа с MVC (Model-View-Controller) архитектурата в ASP.NET Core.
using Microsoft.AspNetCore.Mvc.Rendering; // Предоставя функционалности за генериране на HTML елементи и други помощни методи в изгледите, като например списъци, dropdown менюта и други HTML елементи.
using Microsoft.EntityFrameworkCore; // Включва функционалности на Entity Framework Core, което е ORM (Object-Relational Mapping) инструмент за взаимодействие с бази данни.
using MvcTransplantation_kidney.Models; // Включва моделите, дефинирани в проекта 'MvcTransplantation_kidney'.


// Контролерът 'MedicalRecordsController' е дефиниран в пространството от имена 'MvcTransplantation_kidney.Controllers'.
// Контролерите в ASP.NET Core MVC наследяват класа 'Controller', което им позволява да обработват HTTP заявки и да връщат отговори.
namespace MvcTransplantation_kidney.Controllers

{
    // Дефиниция на контролера
    public class MedicalRecordsController : Controller
    {
        private readonly ApplicationDbContext _context; // Контекст на базата данни, чрез който контролерът има достъп до данните на медицинските записи.

        public MedicalRecordsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MedicalRecords
        // Декларира асинхронен метод, който връща 'Task<IActionResult>'. 'IActionResult' е интерфейс, който представлява резултат от действие (action) в контролера.
        // Връща изглед, който показва списък с всички медицински записи в системата
        public async Task<IActionResult> Index()
        {
            // Заявка към базата данни, която извлича всички записи от таблицата с медицинските записи 'MedicalRecords'.
            // Метода 'ToListAsync()' преобразува резултата от заявката към списък 'List' асинхронно.
            // 'await' означава, че методът ще изчака асинхронното изпълнение на заявката, преди да продължи нататък.
            // Връща изглед (HTML страница), който ще бъде изпратен на клиента (браузъра).
            return View(await _context.MedicalRecords.ToListAsync());
        }

        // GET: MedicalRecords/Details/5
        // Връща детайли за определен медицински запис, базиран на подаденото 'id'. Ако 'id' е null или запис с такова 'id' не съществува, връща се грешка "Not Found".
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalRecord = await _context.MedicalRecords
                .FirstOrDefaultAsync(m => m.RecordId == id);
            if (medicalRecord == null)
            {
                return NotFound();
            }

            return View(medicalRecord);
        }

        // GET: MedicalRecords/Create
        // Връща формуляр за създаване на нов медицински запис.
        public IActionResult Create()
        {
            ViewData["Patients"] = new SelectList(_context.Patients.ToList(), "PatientId","LastName");
            ViewData["Doctors"] = new SelectList(_context.Doctors.ToList(), "DoctorId", "LastName");
            return View();
        }

        // POST: MedicalRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Обработва изпратените данни от формуляра за създаване на медицински запис. Ако моделът е валиден, добавя записа в базата данни и го записва.
        public async Task<IActionResult> Create([Bind("RecordId,PatientId,DoctorId,RecordDate,Description")] MedicalRecord medicalRecord)
        {
            // Логване на грешки в ModelState. Ако моделът не е валиден (ModelState.IsValid е false), грешките се логват в конзолата. Това е полезно за отстраняване на проблеми по време на разработка, като показва конкретни съобщения за грешки.
            if (ModelState.IsValid)
            {
                _context.Add(medicalRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Patients"] = new SelectList(_context.Patients.ToList(), "PatientId", "LastName");
            ViewData["Doctors"] = new SelectList(_context.Doctors.ToList(), "DoctorId", "LastName");
            return View(medicalRecord);
        }

        // GET: MedicalRecords/Edit/5
        // Връща изглед за редактиране на съществуващ медицински запис, идентифициран чрез 'id'.  Ако 'id' е null или запис с такова 'id не съществува, връща се "Not Found".
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalRecord = await _context.MedicalRecords.FindAsync(id);
            if (medicalRecord == null)
            {
                return NotFound();
            }

            ViewData["Patients"] = new SelectList(_context.Patients.ToList(), "PatientId", "LastName");
            ViewData["Doctors"] = new SelectList(_context.Doctors.ToList(), "DoctorId", "LastName");
            return View(medicalRecord);
        }

        // POST: MedicalRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RecordId,PatientId,DoctorId,RecordDate,Description")] MedicalRecord medicalRecord)
        {
            // Проверка за съществуването на медицинския запис. Ако записа, който се опитваме да актуализирме, вече не съществува в базата данни, методът връща "Not Found".
            if (id != medicalRecord.RecordId)
            {
                return NotFound();
            }

            // Проверка на валидността на данните.  Ако 'ModelState' е валидно, контролерът се опитва да актуализира медицинския запис в базата данни. Ако има конфликт при актуализацията (напр. конкурентно записване), се хвърля изключение 'DbUpdateConcurrencyException'.
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicalRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicalRecordExists(medicalRecord.RecordId))
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
            return View(medicalRecord);
        }

        // GET: MedicalRecords/Delete/5
        // Връща изгледа за изтриване на медицинския запис, идентифициран с 'id'. Ако 'id' е null или запис с такова 'id не съществува, връща се "Not Found".
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalRecord = await _context.MedicalRecords
                .FirstOrDefaultAsync(m => m.RecordId == id);
            if (medicalRecord == null)
            {
                return NotFound();
            }

            return View(medicalRecord);
        }

        // POST: MedicalRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        // Окончателно изтриване на медицински запис от базата данни в ASP.NET Core MVC приложение.
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicalRecord = await _context.MedicalRecords.FindAsync(id); // Чрез използване на метода 'FindAsync', системата търси медицински запис в базата данни, който съответства на подадения 'id'.
            if (medicalRecord != null)
            {
                _context.MedicalRecords.Remove(medicalRecord); // Проверка дали медицинския запис съществува
            }

            await _context.SaveChangesAsync(); // Записване на промените в базата данни
            return RedirectToAction(nameof(Index)); //Пренасочване към 'Index'
        }


        // Този метод 'MedicalRecordExists' е помощен метод, използван за проверка дали съществува медицински запис в базата данни с определен 'id'.
        private bool MedicalRecordExists(int id)
        {
            return _context.MedicalRecords.Any(e => e.RecordId == id);
        }
    }
}
