using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ApiPipeline.Results
{
    public class AgentWithETagResponse:  IHttpActionResult
    {
        private string[] _agent;
        private HttpRequestMessage _request;

        public AgentWithETagResponse(string[] agent, HttpRequestMessage request)
        {
            _agent = agent;
            _request = request;
        }


        //public Task<System.Net.Http.HttpResponseMessage> ExexuteAsync(
        //    System.Threading.CancellationToken cancellationToken)
        //{
            
        //}

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage  reponse;
            if (_agent == null)
            {
                reponse = _request.CreateResponse(HttpStatusCode.NotFound);
            }
            else
            {
                if (GetETag() == _agent[1].ToString())
                {
                    reponse = _request.CreateResponse(HttpStatusCode.NotModified);
                }
                else
                {
                    reponse = _request.CreateResponse(HttpStatusCode.OK, _agent);
                    SetETag(reponse, @"""" + _agent[1]+ @"""");
                }
            }
            return Task.FromResult(reponse);

        }

        private void SetETag(HttpResponseMessage reponse, string value)
        {
            reponse.Headers.ETag = new EntityTagHeaderValue(value);
        }

        private string GetETag()
        {
            var etag = _request.Headers.IfNoneMatch.FirstOrDefault();
            string etagValue;

            if (etag != null)
            {
                etagValue = etag.ToString().Replace(@"""", "");
            }
            else
            {
                etagValue = null;
            }
            return etagValue;
        }
    }


}