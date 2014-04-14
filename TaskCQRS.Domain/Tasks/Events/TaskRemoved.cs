using System;

using TaskCQRS.Core;

namespace TaskCQRS.Domain.Tasks
{
    public class TaskRemoved : Event
    {
        public Guid Id;

        public TaskRemoved(Guid id)
        {
            Id = id;
        }
    }
}