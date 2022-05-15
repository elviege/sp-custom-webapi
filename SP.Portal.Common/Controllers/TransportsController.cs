using Microsoft.SharePoint;
using SP.Portal.Common.Models;
using SP.Portal.Common.Services;
using SP.Portal.Core;
using SP.Portal.Data.Services;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace SP.Portal.Common.Controllers
{
    [RoutePrefix("_webapi/Transports")]
    public class TransportsController : ApiController
    {
        private TransportRequestDataService _transportRequestDataService;

        public TransportsController(TransportRequestDataService transportRequestDataService)
        {
            _transportRequestDataService = transportRequestDataService;
        }

        /// <summary>
        /// Test
        /// </summary>
        [HttpGet]
        [Route("GetTest")]
        public IHttpActionResult GetTest()
        {
            var result = "It's work! ";
            var count = _transportRequestDataService.GetRequestsCount();
            return Json(result + count);
        }
    }
}
