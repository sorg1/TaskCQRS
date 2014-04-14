using System;

using TaskCQRS.Core;

namespace TaskCQRS.Domain.Tasks
{
    public class TaskRenamed : Event
    {
        public Guid Id;
        public readonly string Name;

        public TaskRenamed(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
