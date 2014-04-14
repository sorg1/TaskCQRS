using System;
using System.Collections.Generic;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using NUnit.Framework;
using ServiceStack;
using TaskCQRS.Domain;
using TaskCQRS.Domain.Tasks;

namespace TaskCQRS.IntegrationTest
{
    [TestFixture]
    public class IntegrationTest
    {
        [Test]
        public void Test_all_REST_methods()
        {
            var restClient = (IRestClient)new JsonServiceClient("http://localhost:52196");
            var allTasks = restClient.Get<IEnumerable<TaskItemListDto>>("services/tasks");

            //Assert.That(allTasks.Count, Is.EqualTo(0));

            int cnt = allTasks.ToList().Count;

            //var task = new Task { Id = Guid.NewGuid(), Name = "test"};
            var task = new Task { Name = "test" };

            var newTask = restClient.Post<Task>("services/tasks", task);

            task.Id = newTask.Id;

            Assert.That(newTask.Name, Is.EqualTo("test"));

            allTasks = restClient.Get<IEnumerable<TaskItemListDto>>("services/tasks");
            Assert.That(allTasks.ToList().Count, Is.EqualTo(cnt + 1));

            
            /*
            try
            {

                var zeroTask = restClient.Get<Task>("/Tasks/zeroTask");
            }
            catch (BindingException e)
            {
            }*/
            //Assert.(zeroTask);

            var singleTask = restClient.Get<TaskItemDetailsDto>("services/tasks/" + task.Id);
            Assert.That(singleTask.Name, Is.EqualTo("test"));
            
            singleTask.Name = "Update Name";
            restClient.Put<Task>("services/tasks/" + task.Id, singleTask);

            singleTask = restClient.Get<TaskItemDetailsDto>("services/tasks/" + task.Id);
            Assert.That(singleTask.Name, Is.EqualTo("Update Name"));

            singleTask.Name = "Two Update";
            restClient.Put<Task>("services/tasks/" + task.Id, singleTask);

            singleTask = restClient.Get<TaskItemDetailsDto>("services/tasks/" + task.Id);
            Assert.That(singleTask.Name, Is.EqualTo("Two Update"));

            //restClient.Delete("services/tasks/" + singleTask.Id + "/version=" + singleTask.Version);
             // restClient.Delete<Task>("services/tasks/" + singleTask.Id + "/version=" + singleTask.Version);//, task.Version);
            restClient.Delete<string>("services/tasks/" + task.Id + "/version=" + task.Version);//, task.Version);

            allTasks = restClient.Get<IEnumerable<TaskItemListDto>>("services/tasks");
            Assert.That(allTasks.ToList().Count, Is.EqualTo(cnt));
        }
    }
}
