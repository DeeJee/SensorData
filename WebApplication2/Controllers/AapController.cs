using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication2.Controllers
{
    public class AapController : ApiController
    {
        // GET: api/Aap
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Aap/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Aap
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Aap/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Aap/5
        public void Delete(int id)
        {
        }
    }
}
