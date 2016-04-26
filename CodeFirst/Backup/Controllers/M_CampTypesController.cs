using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CodeFirst.Models;

namespace CodeFirst.Controllers
{
   // [Authorize]
    public class M_CampTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: M_CampTypes
        public ActionResult Index()
        {
            return View(db.M_CampTypes.ToList());
        }

        // GET: M_CampTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            M_CampTypes m_CampTypes = db.M_CampTypes.Find(id);
            if (m_CampTypes == null)
            {
                return HttpNotFound();
            }
            return View(m_CampTypes);
        }

        // GET: M_CampTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: M_CampTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CTId,Name,Description,IsActive")] M_CampTypes m_CampTypes)
        {
            if (ModelState.IsValid)
            {
                db.M_CampTypes.Add(m_CampTypes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(m_CampTypes);
        }

        // GET: M_CampTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            M_CampTypes m_CampTypes = db.M_CampTypes.Find(id);
            if (m_CampTypes == null)
            {
                return HttpNotFound();
            }
            return View(m_CampTypes);
        }

        // POST: M_CampTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CTId,Name,Description,IsActive")] M_CampTypes m_CampTypes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(m_CampTypes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(m_CampTypes);
        }

        // GET: M_CampTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            M_CampTypes m_CampTypes = db.M_CampTypes.Find(id);
            if (m_CampTypes == null)
            {
                return HttpNotFound();
            }
            return View(m_CampTypes);
        }

        // POST: M_CampTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            M_CampTypes m_CampTypes = db.M_CampTypes.Find(id);
            db.M_CampTypes.Remove(m_CampTypes);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
