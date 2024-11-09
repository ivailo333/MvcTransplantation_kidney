// Пространство от имена, съдържащо основните класове и интерфейси за работа с Entity Framework Core (EF Core) в .NET приложения. EF Core е ORM (Object-Relational Mapper), който ви позволява да работите с бази данни, използвайки .NET обекти. Той позволява да се извършват операции с базата данни (като четене, писане, обновяване, изтриване на данни) чрез работа с обекти, вместо директно да се пише SQL команди.
using Microsoft.EntityFrameworkCore;

namespace MvcTransplantation_kidney.Models // Дефинира се пространства от имена 'MvcTransplantation_kidney.Models', което групира свързани класове и други типове
{

    // Класа 'ApplicationDbContext' представлява основния контекст, използван за взаимодействие с базата данни. 
    public class ApplicationDbContext:DbContext
    {

        // Конструкторът приема 'DbContextOptions<ApplicationDbContext>' като параметър и го предава на базовия клас 'DbContext'. Това позволява на EF Core да бъде конфигуриран чрез dependency injection, за да се свърже с конкретна база данни.
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
        }

        // Декларация на DbSet свойства - User, Role, UserRole, Patient, Doctor, MedicalRecord, Message и Permission
        public DbSet<User> Users { 
            get; // Метод за получаване, която връща текущата стойност на DbSet свойството 'User'
            set; // Метод за задаване, която позволява да се зададе нова стойност на 'User'
        }
        public DbSet<Role> Roles { 
            get; // Метод за получаване, която връща текущата стойност на DbSet свойството 'Role'
            set; // Метод за задаване, която позволява да се зададе нова стойност на 'Role'
        }
        public DbSet<UserRole> UserRoles { 
            get; // Метод за получаване, която връща текущата стойност на  DbSet свойството 'UserRole'
            set; // Метод за задаване, която позволява да се зададе нова стойност на 'UserRole'
        }
        public DbSet<Patient> Patients { 
            get; // Метод за получаване, която връща текущата стойност на DbSet свойството 'Patient'
            set; // Метод за задаване, която позволява да се зададе нова стойност на 'Patient'
        }
        public DbSet<Doctor> Doctors { 
            get; // Метод за получаване, която връща текущата стойност на DbSet свойството 'Doctor'
            set; // Метод за задаване, която позволява да се зададе нова стойност на 'Doctor'
        }
        public DbSet<MedicalRecord> MedicalRecords { 
            get; // Метод за получаване, която връща текущата стойност на DbSet свойството 'MedicalRecord'
            set; // Метод за задаване, която позволява да се зададе нова стойност на 'MedicalRecord'
        }
        public DbSet<Message> Messages { 
            get; // Метод за получаване, която връща текущата стойност на DbSet свойството 'Message'
            set; // Метод за задаване, която позволява да се зададе нова стойност на 'Message'
        }  
        public DbSet<Permission> Permissions { 
            get; // Метод за получаване, която връща текущата стойност на DbSet свойството 'Permission'
            set; // Метод за задаване, която позволява да се зададе нова стойност на 'Permission'
        }


        // Метод за конфигуриране на модела на базата данни, преди да бъде създаден. Това включва дефиниране на релации, първични ключове, чужди ключове, и други ограничения.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Базова имплементация на метода, което е добра практика, тъй като позволява на родителския клас да изпълни своята логика.
            base.OnModelCreating(modelBuilder);


            // 'UserRole' е таблица за връзка "много към много" между 'User' и 'Role'.
            modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)             // 'HasKey' задава композитен ключ, състоящ се от 'UserId' и 'RoleId'.
                .WithMany(u => u.UserRoles)        // 'HasOne' и 'WithMany' се използват за конфигуриране на релациите между 'User', 'Role' и 'UserRole'.
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)            // 'HasKey' задава композитен ключ, състоящ се от 'UserId' и 'RoleId'.
                .WithMany(r => r.UserRoles)       // 'HasOne' и 'WithMany' се използват за конфигуриране на релациите между 'User', 'Role' и 'UserRole'.
                .HasForeignKey(ur => ur.RoleId);

            // Конфигуриране на релацията "един към много" между 'User' и 'Permission', където един 'User' може да има много 'Permissions'.
            modelBuilder.Entity<Permission>()
                .HasOne(p => p.User)
                .WithMany(u => u.Permissions)
                .HasForeignKey(p => p.UserId);

            // 'MedicalRecord' има връзка "един към много" с 'Patient' и 'Doctor'.
            modelBuilder.Entity<MedicalRecord>()
                .HasOne(m => m.Patient)
                .WithMany(p => p.MedicalRecords)
                .HasForeignKey(m => m.PatientId)
                .OnDelete(DeleteBehavior.Cascade); // 'OnDelete(DeleteBehavior.Cascade)' означава, че при изтриване на пациент или доктор, всички свързани медицински записи също ще бъдат изтрити.

            // 'MedicalRecord' има връзка "един към много" с 'Patient' и 'Doctor'.
            modelBuilder.Entity<MedicalRecord>()
                .HasOne(m => m.Doctor)
                .WithMany(d => d.MedicalRecords)
                .HasForeignKey(m => m.DoctorId)
                .OnDelete(DeleteBehavior.Cascade); // 'OnDelete(DeleteBehavior.Cascade)' означава, че при изтриване на пациент или доктор, всички свързани медицински записи също ще бъдат изтрити.
        }
    }
}
