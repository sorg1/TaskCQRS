using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaskCQRS.Domain;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace TaskCQRS.Projection
{
    public interface IProjectionFacade
    {
        IQueryable<TaskItemDetailsDto> GetTasks();
        TaskItemDetailsDto GetTaskById(Guid id);
    }

    public class ProjectionFacade : IProjectionFacade
    {
        private readonly MongoCollection<TaskItemDetailsDto> collection;

        public ProjectionFacade()
        {
            var client = new MongoClient("mongodb://localhost");
            var server = client.GetServer();
            var database = server.GetDatabase("TaskList");

            collection = database.GetCollection<TaskItemDetailsDto>("SHARED_" + "Tasks");
        }

        public IQueryable<TaskItemDetailsDto> GetTasks()
        {
            return collection.FindAll().AsQueryable();
        }

        public TaskItemDetailsDto GetTaskById(Guid id)
        {
            return collection.FindOne(Query.And(Query<TaskItemDetailsDto>.EQ(d => d.Id, id), Query<TaskItemDetailsDto>.EQ(d => d.Id, id)));
        }
    }
}
