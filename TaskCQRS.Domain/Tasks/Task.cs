using System;
using System.Runtime.Serialization;

namespace TaskCQRS.Domain.Tasks
{
    [DataContract]
    public class Task
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
        [DataMember(Order = 2)]
        public string Name { get; set; }
        [DataMember(Order = 3)]
        public int Version { get; set; }
    }
}
