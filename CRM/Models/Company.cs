using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class Company
    {
         public int Id { get; set; }
        [Display(Name="Название компании"), Required]
        public string Name { get; set; }

        [Display(Name = "Адрес")]
        public string Adress { get; set; }

        [Display(Name="Email раб.")]
        public string Email { get; set; }

        [RegularExpression("\\+380+\\d+", ErrorMessage="Введите номер в фортмате +380ХХХХХХХХХ")]
        [Display(Name = "Раб. тел."), MaxLength(13), Required]
        public string Phone { get; set; }

        [Display(Name = "Логотип компании")]
        public byte[] Image { get; set; }

        public string ImageMimeType { get; set; }
 
    public virtual ICollection<Contact> Contacts { get; set; }
 
    public Company()
    {
        Contacts = new List<Contact>();
    }
    public override string ToString()
    {
        return Name;
    }
    }
}