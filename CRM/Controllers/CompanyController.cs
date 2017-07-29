using CRM.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CRM.Controllers
{
    public class CompanyController : Controller
    {
        CRMContext db = new CRMContext();

        
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View(db.Companies);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult Create()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult Details(int ID)
        {
            if (User.Identity.IsAuthenticated)
            {
                //Contact contact = db.Contacts
                //    .FirstOrDefault(c => c.ID == ID);
                Company company = db.Companies.Find(ID);
                return View(company);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // проверка на правильность запроса
        public ActionResult Create(Company company, HttpPostedFileBase uploadImage)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid && uploadImage != null)
                {
                    byte[] imageData = null;
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                    }
                    // установка массива байтов
                    company.Image = imageData;
                    company.ImageMimeType = uploadImage.ContentType;
                    db.Companies.Add(company);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(company);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public FileContentResult GetImage(int Id)
        {
            Company company = db.Companies.FirstOrDefault(g => g.Id == Id);
            if (company != null)
            {
                return File(company.Image, company.ImageMimeType);
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        public ActionResult EditCompany(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id == null)
                {
                    return HttpNotFound();
                }
                Company company = db.Companies.Find(id);
                if (company != null)
                {
                    return View(company);
                }
                return HttpNotFound();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCompany(Company company, HttpPostedFileBase uploadImage)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid && uploadImage != null)
                {
                    byte[] imageData = null;
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                    }
                    // установка массива байтов
                    company.Image = imageData;
                    company.ImageMimeType = uploadImage.ContentType;
                    db.Entry(company).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(company);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        //Метод для подгрузки компаний Ajaxом
        //public JsonResult GetSearchValue(string term)
        //{
        //    List<Company> allsearch = db.Companies.Where(x => x.Name.Contains(term)).AsEnumerable().Select(x => new Company
        //    {
        //        Id = x.Id,
        //        Name = x.Name
        //    }).ToList();
        //    //Добавить обработку исключений
        //    return new JsonResult { Data = allsearch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //}

        //работает
        //public ActionResult GetSearchValue(string term)
        //{
        //    var models = db.Companies.Where(x => x.Name.Contains(term))
        //                     .Select(a => new
        //                     {
        //                         value = a.Id,
        //                         label = a.Name
        //                     }).Distinct();
        //   return Json(models, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult GetSearchValue(string term)
        {
            var models = db.Companies.Where(x => x.Name.Contains(term))
                             .Select(a => new
                             {
                                 //Id = a.Id.ToString(),
                                 //value = a.Name,
                                 //value = a.Id.ToString(),
                                 Id = a.Id.ToString(),
                                 value = a.Id,
                                 label = a.Name
                                 
                             }).ToList();
            return Json(models, JsonRequestBehavior.AllowGet);
        }
    }
}