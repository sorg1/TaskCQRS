using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

using TaskCQRS.Core;
using TaskCQRS.Domain.Tasks;

using System.Web.Http;
using System.Net;
using System.Net.Http;

namespace TaskCQRS.Services
{
    public class TasksController : ApiController
    {
        private readonly HttpConfiguration configuration;
        private FakeBus _bus;

        public TasksController()
        {
            this.configuration = GlobalConfiguration.Configuration;
            //_bus = Ser
        }

        // GET /services/tasks
        [AcceptVerbs("GET")]
        [ActionName("Tasks")]
        //   [MongoQueryable]
        public Task Get()
        {
            return new Task { Id = Guid.NewGuid(), Name = "Vasya Pushkin!!!" };
        }

        // POST /services/tasks
        [HttpPost]
        [ActionName("Tasks")]
        public HttpResponseMessage Create([FromBody]Task task)
        {
            task.Id = Guid.NewGuid();

            var response = Request.CreateResponse(HttpStatusCode.Created, task, configuration);
            var location = string.Format("/services/tasks?id={0}", task.Id);
            response.Headers.Location = new Uri(Request.RequestUri, location);

            return response;
        }
    }
}
