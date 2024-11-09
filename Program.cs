using Microsoft.AspNetCore.Identity; // ������� ���������������, �������� � ���������� �� ������������� � ������� �������������, ���� ������������, ��������, ���� � ����� � ASP.NET Core ����������.
using Microsoft.EntityFrameworkCore; // ������� ��������������� �� Entity Framework Core, ����� � ORM (Object-Relational Mapping) ���������� �� �������������� � ���� �����.
using MvcTransplantation_kidney.Models; // ������� ��������, ���������� � ������� 'MvcTransplantation_kidney'.


// ���� ��� ��� ������� ���� ��������� �� `WebApplicationBuilder`, ����� ����� �� ������������� � ���������� �� ��� ���������� � ASP.NET Core.
var builder = WebApplication.CreateBuilder(args);


// ���� ��� ��� ���������� `PasswordHasher<User>` ���� ���������� �� ���������� `IPasswordHasher<User>` � DI ���������� �� ASP.NET Core � ����������� `Scoped`, ����� ��������,
// �� �� ���� �������� ��� ��������� �� ����� HTTP ������.
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// ������ �� ������ ��� ����������.
builder.Services.AddControllersWithViews();

// ���� ��� ��� ����������� `ApplicationDbContext` �� ���������� �� SQL Server ���� ���� �����, ���� ������� �������� �� ���������������� ���� � ��� "DefaultConnection".
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ���� ��� ��� �������� �������� ����� �� ��� ������������ �� ��������������, �������� � `builder`, ����� �� ����������.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


// ����������� middleware �� ASP.NET Core ������������:
// 'app.UseHttpsRedirection();' - ���������� ������������ ��� HTTPS.
// 'app.UseStaticFiles();' -  ��������� ������������ �� �������� ������� (���� CSS � JavaScript).
// 'app.UseRouting();' - �������� ��������������� �� ������.
// 'app.UseAuthentication();' � 'app.UseAuthorization();' - ��������� �������������� � ����������� �� �����������.
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// ���� ��� ����������� ������� �� ������������ � ASP.NET Core ������������, ��������� �������� ������� �� ������� `{controller=Home}/{action=Index}/{id?}`,
// ����� ��������, �� �� ������������ �� �� �������� ����������� "Home" � ���������� "Index", � `id` � ���������� ���������.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ���� ��� ������� ������� �� ������������ � ��� "doctors", ����� ������ URL ������� `"Doctors/{action=Index}/{id?}"` ��� ���������� "Doctors",
// � `Index` ���� �������� �� ������������ � `id` ���� ���������� ���������.
app.MapControllerRoute(
    name:"doctors",
    pattern:"Doctors/{action=Index}/{id?}",
    defaults:new {controller="Doctors"}
    );

// ���� ��� �������� ������� � ��� "patients", ����� ������ URL ������� `"Patients/{action=Index}/{id?}"` ��� ���������� "Patients",
// ���� ������ `Index` ���� �������� �� ������������ � `id` ���� ���������� ���������.
app.MapControllerRoute(
    name:"patients",
    pattern:"Patients/{action=Index}/{id?}",
    defaults:new {controller="Patients"}
    );

// ���� ��� �������� ������� � ��� "medicalrecords", ����� ������ URL ������� `"MedicalRecords/{action=Index}/{id?}"` ��� ���������� "MedicalRecords",
// ���� ������ `Index` ���� �������� �� ������������ � `id` ���� ���������� ���������.
app.MapControllerRoute(
    name:"medicalrecords",
    pattern:"MedicalRecords/{action=Index}/{id?}",
    defaults:new {controller= "MedicalRecords"}
    );

// ���� ��� �������� ������� � ��� "users", ����� ������ URL ������� `"Users/{action=Index}/{id?}"` ��� ���������� "Users",
// � `Index` ���� �������� �� ������������ � `id` ���� ���������� ���������.
app.MapControllerRoute(
    name:"users",
    pattern: "Users/{action=Index}/{id?}",
    defaults: new {controller="Users"}
    );

// ���� ��� �������� ������� � ��� "roles", ����� ������ URL ������� `"Roles/{action=Index}/{id?}"` ��� ���������� "Roles",
// ��������� `Index` ���� �������� �� ������������ � `id` ���� ���������� ���������.
app.MapControllerRoute(
    name:"roles",
    pattern:"Roles/{action=Index}/{id?}",
    defaults:new {controller="Roles"}
    );

// ���� ��� �������� ������� � ��� "permissions", ����� ������ URL ������� `"Permissions/{action=Index}/{id?}"` ��� ���������� "Permissions",
// � `Index` ���� �������� �� ������������ � `id` ���� ���������� ���������.
app.MapControllerRoute(
    name: "permissions",
    pattern: "Permissions/{action=Index}/{id?}",
    defaults:new {controller= "Permissions"}
    );

// ���� ��� �������� ������� � ��� "messages", ����� ������ URL ������� `"Messages/{action=Index}/{id?}"` ��� ���������� "Messages",
// � `Index` ���� �������� �� ������������ � `id` ���� ���������� ���������.
app.MapControllerRoute(
    name:"messages",
    pattern:"Messages/{action=Index}/{id?}",
    defaults:new {controller="Messages"}
    );

// ���� ��� �������� ��� ������������, ���� ������� �� ��������� HTTP ������ ������ �������������� � ����������, �������� ����� ����.
app.Run();
