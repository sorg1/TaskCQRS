using System;

using TaskCQRS.Core;

namespace TaskCQRS.Domain.Tasks
{
    public class RenameTask : Command
    {
        public Guid Id;
        public readonly string Name;
        public readonly int OriginalVersion;

        public RenameTask(Guid id, string name, int originalVersion)
        {
            Id = id;
            Name = name;
            OriginalVersion = originalVersion;
        }
    }
}

