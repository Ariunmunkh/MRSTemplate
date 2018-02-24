using DXWebMRCS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

namespace DXWebMRCS.Controllers
{
    public class eServiceController : ApiController
    {
        private UsersContext db = new UsersContext();

        public List<Elearn> GET()
        {
            return db.Elearn.ToList<Elearn>();
        }

        // POST api/eService
        [System.Web.Http.AcceptVerbs("GET", "POST")]

        public async Task<IHttpActionResult> PosteService(int score, string date, int userid, int lessonid)
        {
            eService eservice = new eService();
            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }
            eservice.Score = score;
            eservice.DateTime = date;
            eservice.UserId = userid;
            eservice.LessonId = lessonid;
            db.eService.Add(eservice);
            await db.SaveChangesAsync();


            return CreatedAtRoute("DefaultApi", new { id = eservice.eServiceID }, eservice);
        }
    }
}
