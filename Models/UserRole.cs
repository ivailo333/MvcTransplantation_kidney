﻿using System; // Предоставя основни типове данни и функционалност в .NET
using System.ComponentModel.DataAnnotations; // Съдържа атрибути, които се използват за валидация и на данни и описание на метаданни в класовете на модела

namespace MvcTransplantation_kidney.Models // Дефинира се пространства от имена 'MvcTransplantation_kidney.Models', което групира свързани класове и други типове
{

    // Дефинира публичен клас 'UserRole'
    public class UserRole
    {
        [Key] // Първичен ключ 
        public int UserRoleId { 
            get; // Метод за получаване, която връща текущата стойност на свойството 'UserRoleId'
            set; // Метод за задаване, която позволява да се зададе нова стойност на 'UserRoleId'
        }

        [Required] // Задължително поле
        public int UserId { 
            get; // Метод за получаване, която връща текущата стойност на свойството 'UserId'
            set; // Метод за задаване, която позволява да се зададе нова стойност на 'UserId'
        }
        public User User { 
            get; // Метод за получаване, която връща текущата стойност на свойството 'User'
            set; // Метод за задаване, която позволява да се зададе нова стойност на 'User'
        }

        [Required] // Задължително поле
        public int RoleId { 
            get; // Метод за получаване, която връща текущата стойност на свойството 'RoleId'
            set; // Метод за задаване, която позволява да се зададе нова стойност на 'RoleId'
        }
        public Role Role { 
            get; // Метод за получаване, която връща текущата стойност на свойството 'Role'
            set; // Метод за задаване, която позволява да се зададе нова стойност на 'Role'
        }
  
    }
}
