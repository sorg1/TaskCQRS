using System;
using System.Collections.Generic;

using TaskCQRS.Core;
using TaskCQRS.Domain;
using TaskCQRS.Domain.Tasks;
using System.Runtime.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace TaskCQRS.Projection.Impl
{
    public class TaskProjection : Handles<TaskCreated>, Handles<TaskRenamed>, Handles<TaskRemoved>
    {
        private readonly MongoCollection<TaskItemDetailsDto> collection;

        public TaskProjection(MongoDatabase database)
        {
            collection = database.GetCollection<TaskItemDetailsDto>("SHARED_" + "Tasks");
        }

        public TaskItemDetailsDto GetTaskById(Guid id)
        {
            return collection.FindOne(Query.And(Query<TaskItemDetailsDto>.EQ(d => d.Id, id), Query<TaskItemDetailsDto>.EQ(d => d.Id, id)));
        }
        /*
        private TaskItemDetailsDto GetDetailsItem(Guid id)
        {
            TaskItemDetailsDto d = collection.FindOneByIdAs<TaskItemDetailsDto>(id);
            if (d.Equals(null))
            {
                throw new InvalidOperationException("did not find the original Task this shouldnt happen");
            }
            return d;
        }*/

        public void Handle(TaskCreated message)
        {
            collection.Insert(new TaskItemDetailsDto(message.Id, message.Name, 0, 0));
        }

        public void Handle(TaskRenamed message)
        {
            collection.Update(Query<TaskItemDetailsDto>.EQ(e => e.Id, message.Id), Update<TaskItemDetailsDto>.
                Set(e => e.Name, message.Name).
                Set(e => e.Version, message.Version));
        }

        public void Handle(TaskRemoved message)
        {
            collection.Remove(Query<TaskItemDetailsDto>.EQ(e => e.Id, message.Id));
        }
    }
}
