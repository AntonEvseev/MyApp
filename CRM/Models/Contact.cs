using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class Contact
    {
        public int ID { get; set; }

        [Display(Name="Фамилия")]
        public string Surname { get; set; }

        [Display(Name="Имя"), Required]
        public string Name { get; set; }

        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }

        [Display(Name="Email")]
        public string Email { get; set; }

        [RegularExpression("\\+380+\\d+", ErrorMessage="Введите номер в фортмате +380ХХХХХХХХХ")]
        [Display(Name = "Телефон"), MaxLength(13), Required]
        public string Phone { get; set; }

        [Display(Name = "Фото профиля")]
        public byte[] Image { get; set; }

        public string ImageMimeType { get; set; }

        public int? CompanyId { get; set; }
        public virtual Company Companies { get; set; }
    }
}