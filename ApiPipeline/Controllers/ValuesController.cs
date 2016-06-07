using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ApiPipeline.Filter;
using ApiPipeline.Results;
using log4net;

namespace ApiPipeline.Controllers
{
    public class ValuesController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (ValuesController));
        [Route("")]
        public IHttpActionResult Get()
        {
            Log.Info("In api/Values");
            //throw new NotImplementedException();
            return Ok(new[] {"Value1", "Value2"});
        }


        [ValidateFilter]
        [CachingFilter(15)]
        public IHttpActionResult Get(int id)
        {
            Log.Info("In api/Values/id");
            //throw new NotImplementedException();
            var agent = new[] {"ID1", id.ToString(), DateTime.Now.ToString(CultureInfo.InvariantCulture)};
            return Ok(agent);
            //return new AgentWithETagResponse(agent, this.Request);
        }

    }
}
