using Microsoft.AspNetCore.Identity; // Включва функционалности, свързани с управление на потребителите и тяхната идентификация, като регистриране, вписване, роли и права в ASP.NET Core приложение.
using Microsoft.EntityFrameworkCore; // Включва функционалности на Entity Framework Core, което е ORM (Object-Relational Mapping) инструмент за взаимодействие с бази данни.
using MvcTransplantation_kidney.Models; // Включва моделите, дефинирани в проекта 'MvcTransplantation_kidney'.


// Този ред код създава нова инстанция на `WebApplicationBuilder`, който служи за конфигуриране и изграждане на уеб приложение в ASP.NET Core.
var builder = WebApplication.CreateBuilder(args);


// Този ред код регистрира `PasswordHasher<User>` като реализация на интерфейса `IPasswordHasher<User>` в DI контейнера на ASP.NET Core с времетраене `Scoped`, което означава,
// че ще бъде създаден нов екземпляр за всяка HTTP заявка.
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// Добавя на услуги към контейнера.
builder.Services.AddControllersWithViews();

// Този ред код конфигурира `ApplicationDbContext` за използване на SQL Server като база данни, като извлича връзката от конфигурационния файл с име "DefaultConnection".
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Този ред код изгражда финалния обект на уеб приложението от конфигурацията, зададена в `builder`, готов за стартиране.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


// Конфигурира middleware за ASP.NET Core приложението:
// 'app.UseHttpsRedirection();' - принуждава пренасочване към HTTPS.
// 'app.UseStaticFiles();' -  позволява обслужването на статични файлове (като CSS и JavaScript).
// 'app.UseRouting();' - активира маршрутизацията на заявки.
// 'app.UseAuthentication();' и 'app.UseAuthorization();' - активират удостоверяване и авторизация на потребители.
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Този код конфигурира маршрут за контролерите в ASP.NET Core приложението, задавайки основния маршрут на формата `{controller=Home}/{action=Index}/{id?}`,
// което означава, че по подразбиране ще се използва контролерът "Home" и действието "Index", а `id` е опционален параметър.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Този код създава маршрут за контролерите с име "doctors", който мапира URL шаблона `"Doctors/{action=Index}/{id?}"` към контролера "Doctors",
// с `Index` като действие по подразбиране и `id` като опционален параметър.
app.MapControllerRoute(
    name:"doctors",
    pattern:"Doctors/{action=Index}/{id?}",
    defaults:new {controller="Doctors"}
    );

// Този код дефинира маршрут с име "patients", който мапира URL шаблона `"Patients/{action=Index}/{id?}"` към контролера "Patients",
// като задава `Index` като действие по подразбиране и `id` като опционален параметър.
app.MapControllerRoute(
    name:"patients",
    pattern:"Patients/{action=Index}/{id?}",
    defaults:new {controller="Patients"}
    );

// Този код дефинира маршрут с име "medicalrecords", който мапира URL шаблона `"MedicalRecords/{action=Index}/{id?}"` към контролера "MedicalRecords",
// като задава `Index` като действие по подразбиране и `id` като опционален параметър.
app.MapControllerRoute(
    name:"medicalrecords",
    pattern:"MedicalRecords/{action=Index}/{id?}",
    defaults:new {controller= "MedicalRecords"}
    );

// Този код дефинира маршрут с име "users", който мапира URL шаблона `"Users/{action=Index}/{id?}"` към контролера "Users",
// с `Index` като действие по подразбиране и `id` като опционален параметър.
app.MapControllerRoute(
    name:"users",
    pattern: "Users/{action=Index}/{id?}",
    defaults: new {controller="Users"}
    );

// Този код дефинира маршрут с име "roles", който мапира URL шаблона `"Roles/{action=Index}/{id?}"` към контролера "Roles",
// задавайки `Index` като действие по подразбиране и `id` като опционален параметър.
app.MapControllerRoute(
    name:"roles",
    pattern:"Roles/{action=Index}/{id?}",
    defaults:new {controller="Roles"}
    );

// Този код дефинира маршрут с име "permissions", който мапира URL шаблона `"Permissions/{action=Index}/{id?}"` към контролера "Permissions",
// с `Index` като действие по подразбиране и `id` като опционален параметър.
app.MapControllerRoute(
    name: "permissions",
    pattern: "Permissions/{action=Index}/{id?}",
    defaults:new {controller= "Permissions"}
    );

// Този код дефинира маршрут с име "messages", който мапира URL шаблона `"Messages/{action=Index}/{id?}"` към контролера "Messages",
// с `Index` като действие по подразбиране и `id` като опционален параметър.
app.MapControllerRoute(
    name:"messages",
    pattern:"Messages/{action=Index}/{id?}",
    defaults:new {controller="Messages"}
    );

// Този код стартира уеб приложението, като започва да обработва HTTP заявки според конфигурацията и маршрутите, зададени преди това.
app.Run();
