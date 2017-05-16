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
using SensorDataManager.Data;

namespace SensorDataManager.Controllers
{
    public class DataSourceController : ApiController
    {
        private SensorDataSqlEntities db = new SensorDataSqlEntities();

        // GET: api/DataSource
        public string[] GetSensorData()
        {
            return db.SensorData.Select(s => s.DeviceId).Distinct().ToArray();
        }

        // GET: api/DataSource/5
        [ResponseType(typeof(SensorData))]
        public IHttpActionResult GetSensorData(int id)
        {
            SensorData sensorData = db.SensorData.Find(id);
            if (sensorData == null)
            {
                return NotFound();
            }

            return Ok(sensorData);
        }

        // PUT: api/DataSource/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSensorData(int id, SensorData sensorData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sensorData.Id)
            {
                return BadRequest();
            }

            db.Entry(sensorData).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SensorDataExists(id))
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

        // POST: api/DataSource
        [ResponseType(typeof(SensorData))]
        public IHttpActionResult PostSensorData(SensorData sensorData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SensorData.Add(sensorData);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = sensorData.Id }, sensorData);
        }

        // DELETE: api/DataSource/5
        [ResponseType(typeof(SensorData))]
        public IHttpActionResult DeleteSensorData(int id)
        {
            SensorData sensorData = db.SensorData.Find(id);
            if (sensorData == null)
            {
                return NotFound();
            }

            db.SensorData.Remove(sensorData);
            db.SaveChanges();

            return Ok(sensorData);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SensorDataExists(int id)
        {
            return db.SensorData.Count(e => e.Id == id) > 0;
        }       
    }
}