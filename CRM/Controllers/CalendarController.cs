using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM.Controllers
{
    public class CalendarController : Controller
    {
        CRMContext db = new CRMContext();

        public ActionResult Index()
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

        public JsonResult GetEvents()
        {
            //Here MyDatabaseEntities is our entity datacontext (see Step 4)
            
                var v = db.Events.OrderBy(a => a.StartAt).ToList();
                return new JsonResult { Data = v, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            
        }

        [HttpPost]
        public JsonResult SaveEvent(Event e)
        {
            var status = false;
            if (e.EventId > 0)
            {
                //update event
                var v = db.Events.Where(a => a.EventId == e.EventId).FirstOrDefault();
                if (v != null)
                {
                    v.Title = e.Title;
                    v.StartAt = e.StartAt;
                    v.EndAt = e.EndAt;
                    v.Description = e.Description;
                    v.IsFullDay = e.IsFullDay;
                    v.ThemeColor = e.ThemeColor;
                }
            }
            else
            {
                db.Events.Add(e);
            }
            db.SaveChanges();
            status = true;
            return new JsonResult { Data = new { status = status } };
        }

        [HttpPost]
        public JsonResult DeleteEvent(int eventId)
        {
            var status = false;
            var v = db.Events.Where(a => a.EventId == eventId).FirstOrDefault();
            if (v != null)
            {
                db.Events.Remove(v);
                db.SaveChanges();
                status = true;
            }
            return new JsonResult { Data = new { status = status } };
        }
       
    }
}