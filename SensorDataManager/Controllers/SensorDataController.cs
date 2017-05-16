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
using System.Web.Mvc;
using SensorDataManager.Models;
using System.Globalization;

namespace SensorDataManager.Controllers
{
    public class SensorDataController : ApiController
    {
        private SensorDataSqlEntities db = new SensorDataSqlEntities();
     
           TimeZoneInfo info = TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time");


        // GET: api/SensorData
        public IQueryable<SensorData> Get()
        {
            return db.SensorData;
        }

        // GET: api/SensorData/5
        //[ResponseType(typeof(SensorData))]
        public IQueryable<SensorData> Get(string dataSource)
        {
            var vandaag = DateTime.Now;
            var vanDateTime = new DateTime(vandaag.Year, vandaag.Month, vandaag.Day);
            var totDateTime = new DateTime(vandaag.Year, vandaag.Month, vandaag.Day,23,59,59);
            var results= db.SensorData.Where(w => w.DeviceId == dataSource && w.TimeStamp >= vanDateTime && w.TimeStamp <= totDateTime);
            foreach (var result in results) {
                result.TimeStamp = TimeZoneInfo.ConvertTimeFromUtc(result.TimeStamp, info);
            }
            return results;            
        }

        // GET: api/SensorData/5
        //[ResponseType(typeof(SensorData))]
        public IQueryable<SensorData> Get(string dataSource, string van, string tot)
        {
            var vanDateTime = DateTime.ParseExact(van,"yyyyMMdd", CultureInfo.CurrentCulture);
            var totDateTime = DateTime.ParseExact(tot, "yyyyMMdd", CultureInfo.CurrentCulture);
            return db.SensorData.Where(w => w.DeviceId == dataSource && w.TimeStamp >= vanDateTime && w.TimeStamp <=totDateTime);
        }

        // PUT: api/SensorData/5
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

        // POST: api/SensorData
        public IHttpActionResult PostSensorData(DataModel sensorData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SensorData.Add(new SensorData
            {
                DeviceId = sensorData.DeviceId,
                TimeStamp = DateTime.Now,
                Payload = sensorData.Payload.ToString()
            });
            db.SaveChanges();

            return Ok();
        }

        // DELETE: api/SensorData/5
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