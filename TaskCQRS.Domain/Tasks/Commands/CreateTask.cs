using System;

using TaskCQRS.Core;

namespace TaskCQRS.Domain.Tasks
{
     public class CreateTask : Command
    {
         public Guid Id;
		public readonly string Name;

        public CreateTask(Guid id, string name)
        {
			Id = id;
			Name = name;
        }
    }
}
