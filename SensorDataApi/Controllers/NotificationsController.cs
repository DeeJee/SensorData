using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using SensorDataApi.Data;
using SensorDataApi.Attributes;

namespace SensorDataApi.Controllers
{
    public class NotificationsController : ApiController
    {
        private SensorDataSqlEntities db = new SensorDataSqlEntities();

        // GET: api/Notifications
        public IQueryable<Notifications> GetNotifications()
        {
            return db.Notifications;
        }

        // GET: api/Notifications/5
        [ResponseType(typeof(Notifications))]
        public IHttpActionResult GetNotifications(int id)
        {
            Notifications notifications = db.Notifications.Find(id);
            if (notifications == null)
            {
                return NotFound();
            }

            return Ok(notifications);
        }

        // PUT: api/Notifications/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutNotifications(int id, Notifications notifications)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != notifications.Id)
            {
                return BadRequest();
            }

            db.Entry(notifications).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotificationsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Notifications
        [ResponseType(typeof(Notifications))]
        [BasicAuthenticationAttribute]
        public IHttpActionResult PostNotifications(Notifications notifications)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Notifications.Add(notifications);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = notifications.Id }, notifications);
        }

        // DELETE: api/Notifications/5
        [ResponseType(typeof(Notifications))]
        public IHttpActionResult DeleteNotifications(int id)
        {
            Notifications notifications = db.Notifications.Find(id);
            if (notifications == null)
            {
                return NotFound();
            }

            db.Notifications.Remove(notifications);
            db.SaveChanges();

            return Ok(notifications);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NotificationsExists(int id)
        {
            return db.Notifications.Count(e => e.Id == id) > 0;
        }
    }
}