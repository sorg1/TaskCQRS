using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
//using System.Web.Mvc;
using TaskCQRS.Core;
using TaskCQRS.Domain;
using TaskCQRS.Domain.Tasks;

using System.Web.Http;
using System.Net;
using System.Net.Http;

using TaskCQRS.Projection;

namespace TaskCQRS.Services
{
    //[HandleError]
    public class TasksController : ApiController
    {
        private readonly HttpConfiguration configuration;
        private FakeBus _bus;
        //private ReadModelFacade _readmodel;
        private readonly ProjectionFacade tasksProjection;
        

        public TasksController()
        {
            this.configuration = GlobalConfiguration.Configuration;
            _bus = ServiceLocator.Bus;
            tasksProjection = new ProjectionFacade();
            
        }

        // GET /services/tasks
        [AcceptVerbs("GET")]
        [ActionName("Tasks")]
        //   [MongoQueryable]
        public IQueryable<TaskItemDetailsDto> Get()
        {
            return tasksProjection.GetTasks(); 
            //return new Task { Id = Guid.NewGuid(), Name = "Vasya Pushkin!!!" };
        }

        [HttpGet]
        [ActionName("Tasks")]
        //   [MongoQueryable]
        public TaskItemDetailsDto Get([FromUri]Guid id)
        {
            return tasksProjection.GetTaskById(id);
            //return new Task { Id = Guid.NewGuid(), Name = "Vasya Pushkin!!!" };
        }

        // POST /services/tasks
        [HttpPost]
        [ActionName("Tasks")]
        public HttpResponseMessage Create([FromBody]Task task)
        {
            task.Id = Guid.NewGuid();

            _bus.Send(new CreateTask( task.Id, task.Name ));
            var response = Request.CreateResponse(HttpStatusCode.Created, task, configuration);
            var location = string.Format("/services/tasks/{0}", task.Id);
            response.Headers.Location = new Uri(Request.RequestUri, location);

            return response;
        }

        [HttpPut]
        [ActionName("Tasks")]
        public HttpResponseMessage Update([FromUri] Guid id, [FromBody] Task task)
        {
            _bus.Send(new RenameTask(id, task.Name, task.Version));
            return Request.CreateResponse(HttpStatusCode.Accepted, task, configuration);
        }

        [HttpDelete]
        [ActionName("Tasks")]
        public HttpResponseMessage Delete([FromUri] Guid id, [FromUri]int version = 0)
        {
             _bus.Send(new RemoveTask(id, -1));
             return Request.CreateResponse(HttpStatusCode.Accepted, string.Format("Task with id={0} has been deleted", id), configuration);
        }
    }
}
