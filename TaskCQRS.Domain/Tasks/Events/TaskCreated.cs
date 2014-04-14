using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaskCQRS.Core;

namespace TaskCQRS.Domain.Tasks
{
    public class TaskCreated : Event
    {
        public Guid Id;
		public readonly string Name;

        public TaskCreated(Guid id, string name)
        {
			Id = id;
			Name = name;
        }
    }
}
