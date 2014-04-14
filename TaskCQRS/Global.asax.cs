using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using TaskCQRS.Domain;
using TaskCQRS.Domain.Tasks;
using TaskCQRS.Core;

namespace TaskCQRS
{
    // Примечание: Инструкции по включению классического режима IIS6 или IIS7 
    // см. по ссылке http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            
            var bus = new FakeBus();

            var storage = new EventStore(bus);

            var rep = new Repository<TaskItem>(storage);

            var commands = new CommandHandlers(rep);

            bus.RegisterHandler<CreateTask>(commands.Handle);
            bus.RegisterHandler<RenameTask>(commands.Handle);
            bus.RegisterHandler<RemoveTask>(commands.Handle);

            var detail = new TaskItemDetailView();

            bus.RegisterHandler<TaskCreated>(detail.Handle);
            bus.RegisterHandler<TaskRenamed>(detail.Handle);
            bus.RegisterHandler<TaskRemoved>(detail.Handle);

            var list = new TaskListView();

            bus.RegisterHandler<TaskCreated>(list.Handle);
            bus.RegisterHandler<TaskRenamed>(list.Handle);
            bus.RegisterHandler<TaskRemoved>(list.Handle);

         //   FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
           
         //   BundleConfig.RegisterBundles(BundleTable.Bundles);

            ServiceLocator.Bus = bus;

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}