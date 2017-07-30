using NLog;
using SensorDataApi.Data;
using SensorDataApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace SensorDataApi.Controllers
{
    public class SensorDataController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private SensorDataSqlEntities db = new SensorDataSqlEntities();

        TimeZoneInfo info = TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time");
        CultureInfo nederland = CultureInfo.CurrentCulture;


        // GET: api/SensorData
        public IQueryable<SensorData> Get()
        {
            logger.Info($"GET: {Request.RequestUri} called");
            return db.SensorData.Take(1000);
        }

        // GET: api/SensorData/5
        [ResponseType(typeof(SensorData))]
        public IQueryable<SensorData> Get(string dataSource)
        {
            DateTime vanDateTime;
            DateTime totDateTime;
            var results = db.SensorData.Where(w => w.DeviceId == dataSource);

            IEnumerable<KeyValuePair<string, string>> allUrlKeyValues = ControllerContext.Request.GetQueryNameValuePairs();
            string vanDatum = allUrlKeyValues.FirstOrDefault(x => x.Key == "van").Value;
            string totDatum = allUrlKeyValues.FirstOrDefault(x => x.Key == "tot").Value;

            if (string.IsNullOrEmpty(vanDatum) && string.IsNullOrEmpty(totDatum))
            {
                logger.Info($"GET: {Request.RequestUri} called");
                var vandaag = DateTime.Now.Date;
                vanDateTime = new DateTime(vandaag.Year, vandaag.Month, vandaag.Day);
                totDateTime = new DateTime(vandaag.Year, vandaag.Month, vandaag.Day, 23, 59, 59);
                var maxDate = db.SensorData.Where(w => w.DeviceId == dataSource).Max(m => m.TimeStamp);
                results = results.Where(w => DbFunctions.TruncateTime(w.TimeStamp) == maxDate.Date);
            }
            else
            {
                if (!string.IsNullOrEmpty(vanDatum))
                {
                    vanDateTime = DateTime.ParseExact(vanDatum, "yyyy-MM-dd", nederland);
                    results=results.Where(w => DbFunctions.TruncateTime(w.TimeStamp) >= vanDateTime);
                }
                if (!string.IsNullOrEmpty(totDatum))
                {
                    totDateTime = DateTime.ParseExact(totDatum, "yyyy-MM-dd", nederland);
                    results=results.Where(w => DbFunctions.TruncateTime(w.TimeStamp) <= totDateTime);
                }
            }



            foreach (var result in results)
            {
                result.TimeStamp = TimeZoneInfo.ConvertTimeFromUtc(result.TimeStamp, info);
            }

            logger.Info($"GET: {Request.RequestUri} finished");
            logger.Info($"{results.Count()} items retrieved");
            return results;
        }

        //// GET: api/SensorData/5
        //[ResponseType(typeof(SensorData))]
        //public IQueryable<SensorData> Get(string dataSource, string van, string tot)
        //{
        //    logger.Info($"GET: {Request.RequestUri} called");
        //    var vanDateTime = DateTime.ParseExact(van, "yyyyMMdd", CultureInfo.CurrentCulture);
        //    var totDateTime = DateTime.ParseExact(tot, "yyyyMMdd", CultureInfo.CurrentCulture);

        //    var results = db.SensorData.Where(w => w.DeviceId == dataSource && w.TimeStamp >= vanDateTime && w.TimeStamp <= totDateTime);

        //    logger.Info($"GET: {Request.RequestUri} finished");
        //    logger.Info($"{results.Count()} items retrieved");
        //    return results;
        //}

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
            logger.Info($"POST: {Request.RequestUri} called");
            if (!ModelState.IsValid)
            {
                logger.Info("Bad request");
                return BadRequest(ModelState);
            }

            db.SensorData.Add(new SensorData
            {
                DeviceId = sensorData.DeviceId,
                TimeStamp = DateTime.Now,
                Payload = sensorData.Payload.ToString()
            });
            db.SaveChanges();

            logger.Info("Dataset added");
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
