using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using log4net;

namespace ApiPipeline.Controllers
{
    public class Values2Controller : ApiController
    {
         private static readonly ILog Log = LogManager.GetLogger(typeof(ValuesController));

        public IHttpActionResult Get()
        {
            Log.Info("In api/Values22222222222");
            return Ok(new[] { "222Value1", "2222Value2" });
        }
    }
}
