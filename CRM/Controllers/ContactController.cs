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
    public class ContactController : Controller
    {
        CRMContext db = new CRMContext();

        // GET: Contact
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var contacts = db.Contacts.Include(c => c.Companies);
                return View(contacts.ToList());
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
                Contact contact = db.Contacts.Find(ID);
                return View(contact);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // проверка на правильность запроса
        public ActionResult Create(Contact contact, HttpPostedFileBase uploadImage)
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
                    contact.Image = imageData;
                    contact.ImageMimeType = uploadImage.ContentType;//DELETE

                    db.Contacts.Add(contact);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                return View(contact);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public FileContentResult GetImage(int Id)
        {
                Contact contact = db.Contacts.FirstOrDefault(g => g.ID == Id);
                if (contact != null)
                {
                    return File(contact.Image, contact.ImageMimeType);
                }
                else
                {
                    return null;
                }
        }

        [HttpGet]
        public ActionResult EditContact(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id == null)
                {
                    return HttpNotFound();
                }
                Contact contact = db.Contacts.Find(id);
                if (contact != null)
                {
                    return View(contact);
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
        public ActionResult EditContact(Contact contact, HttpPostedFileBase uploadImage)
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
                    contact.Image = imageData;
                    contact.ImageMimeType = uploadImage.ContentType;//DELETE
                    db.Entry(contact).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(contact);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
    }
}