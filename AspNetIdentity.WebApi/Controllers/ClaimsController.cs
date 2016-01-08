using AspNetIdentity.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace AspNetIdentity.WebApi.Controllers
{
    [RoutePrefix("api/claims")]
    public class ClaimsController : BaseApiController
    {
        [Authorize]
        [Route("")]
        public IHttpActionResult GetClaims()
        {
            var identity = User.Identity as ClaimsIdentity;
            
            var claims = from c in identity.Claims
                         select new MyClaims
                         {
                             Subject = c.Subject.Name,
                             Type = c.Type,
                             Value = c.Value
                         };

            return Ok(claims);
        }


        [Serializable]
        public class MyClaims
        {
            public String Subject { get; set; }
            public String Type { get; set; }
            public String Value { get; set; }            
        }
    }
}
