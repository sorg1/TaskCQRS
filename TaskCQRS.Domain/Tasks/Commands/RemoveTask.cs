using System;

using TaskCQRS.Core;

namespace TaskCQRS.Domain.Tasks
{
    public class RemoveTask : Command
    {
        public Guid Id;
        public readonly int OriginalVersion;

        public RemoveTask(Guid id, int originalVersion)
        {
            Id = id;
            OriginalVersion = originalVersion;
        }
    }
}
