using NLog;
using SensorDataApi.Data;
using SensorDataApi.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace SensorDataApi.Controllers
{
    //[EnableCors(origins: "http://sensordataapp.azurewebsites.net", headers: "*", methods: "*")]
    //[EnableCors(origins: "http://localhost:4200,http://sensordataapp.azurewebsites.net", headers: "*", methods: "*")]
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DataSourceController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private SensorDataSqlEntities db = new SensorDataSqlEntities();

        [HttpGet]
        [Route("api/DataSource/NewDatasources")]
        public DataSource[] NewDatasources()
        {
            var query = db.SensorData
            .Where(w => !db.DataSource.Any(a => a.DeviceId == w.DeviceId))
            .Select(s => s.DeviceId).Distinct().ToArray();

            return query.Select(deviceId => new DataSource
            {
                DeviceId = deviceId
            }).ToArray();
        }

        // GET: api/DataSource/5
        public IQueryable<DataSource> Get()
        {
            logger.Info($"GET: {Request.RequestUri} called");
            var qsp = Request.GetQueryNameValuePairs();
            string channel = qsp.LastOrDefault(x => x.Key == "channel").Value;

            IQueryable<DataSource> query = db.DataSource;
            if (!string.IsNullOrEmpty(channel))
            {
                int id;
                if (int.TryParse(channel, out id))
                {
                    query = query.Where(w => w.ChannelId == id).AsQueryable();
                }
            }

            logger.Info($"GET: {Request.RequestUri} finished");
            logger.Info($"{query.Count()} items retrieved");

            List<DataSource> results = new List<DataSource>();
            foreach (var item in query)
            {
                results.Add(new DataSource
                {
                    Id = item.Id,
                    DeviceId = item.DeviceId,
                    Description = item.Description,
                    ChannelId = item.ChannelId
                }
                );
            }
            return results.AsQueryable();
        }

        // GET: api/SensorData/5
        [ResponseType(typeof(Channel))]
        [Route("api/DataSource/Types")]
        public IQueryable<ChannelViewModel> GetDatasourceType()
        {
            logger.Info($"GET: {Request.RequestUri} called");

            var query = db.Channel;

            var results = new List<ChannelViewModel>();
            foreach (var item in query)
            {
                results.Add(new ChannelViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Created = item.Created
                });

            }

            logger.Info($"GET: {Request.RequestUri} finished");
            logger.Info($"{results.Count()} items retrieved");
            return results.AsQueryable();
        }

        // PUT: api/DataSource/5
        [ResponseType(typeof(void))]
        [Route("api/DataSource/{id}")]
        public IHttpActionResult Put(int id, DataSource datasource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != datasource.Id)
            {
                return BadRequest();
            }

            db.Entry(datasource).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                logger.Error(ex.Message);
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
        [ResponseType(typeof(void))]
        public IHttpActionResult Post(DataSource dataSource)
        {
            logger.Info($"POST: {Request.RequestUri} called");
            logger.Info("Storing Datasource: Id={0}, DeviceId={1}, Type={2}, Description={3}",
                dataSource.Id,
                dataSource.DeviceId,
                dataSource.ChannelId,
                dataSource.Description);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DataSource.Add(dataSource);
            db.SaveChanges();

            logger.Info("Datasource added: Id={0}, DeviceId={1}, Type={2}, Description={3}",
                dataSource.Id,
                dataSource.DeviceId,
                dataSource.ChannelId,
                dataSource.Description);
            return Ok();
        }

        // DELETE: api/DataSource/5
        [HttpDelete]
        [Route("api/DataSource/{id}")]
        public IHttpActionResult Delete(int id)
        {
            logger.Info($"DELETE: {Request.RequestUri} called");
            logger.Info("Deleting Datasource: Id={0}", id);
            DataSource dataSource = db.DataSource.Find(id);
            if (dataSource == null)
            {
                logger.Info("Datasource to be deleted not found: Id={0}", id);
                return NotFound();
            }

            db.DataSource.Remove(dataSource);
            db.SaveChanges();
            logger.Info("Datasource deleted: Id={0}", id);
            return Ok();
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
