using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskCQRS.Core;
using TaskCQRS.Domain;
using TaskCQRS.Domain.Tasks;

using TaskCQRS.Projection;

namespace TaskCQRS.Controllers
{
    

    public class HomeController : Controller
    {
        private FakeBus _bus;
        //private ReadModelFacade _readmodel;
        private readonly ProjectionFacade tasksProjection;
        
        public HomeController()
        {
            _bus = ServiceLocator.Bus;
            //_readmodel = new ReadModelFacade();
            tasksProjection = new ProjectionFacade();
        }

        public ActionResult Index()
        {
            ViewData.Model = tasksProjection.GetTasks();

            return View();
        }
        public ActionResult Details(Guid id)
        {
            ViewData.Model = tasksProjection.GetTaskById(id);
            return View();
        }

        [HttpPost]
        public ActionResult Add(string name)
        {
            _bus.Send(new CreateTask(Guid.NewGuid(), name));

            return RedirectToAction("Index");
        }

        public ActionResult Add()
        {
            return View();
        }
        public ActionResult Rename(Guid id)
        {
            ViewData.Model = tasksProjection.GetTaskById(id);
            return View();
        }

        [HttpPost]
        public ActionResult Rename(Guid id, string name, int version)
        {
            var command = new RenameTask(id, name, version);
            _bus.Send(command);

            return RedirectToAction("Index");
        }
        public ActionResult Remove(Guid id, int version)
        {
            _bus.Send(new RemoveTask(id, version));
            return RedirectToAction("Index");
        }
    }
}
