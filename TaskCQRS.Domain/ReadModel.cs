using System;
using System.Collections.Generic;

using TaskCQRS.Core;
using TaskCQRS.Domain.Tasks;
using System.Runtime.Serialization;

namespace TaskCQRS.Domain
{
    public interface IReadModelFacade
    {
        IEnumerable<TaskItemListDto> GetTaskItems();
        TaskItemDetailsDto GetTaskItemDetails(Guid id);
    }
    [DataContract]
    public class TaskItemDetailsDto
    {
        [DataMember(Order = 1)]
        public Guid Id;
        [DataMember(Order = 2)]
        public string Name;
        [DataMember(Order = 3)]
        public int CurrentCount;
        [DataMember(Order = 4)]
        public int Version;

        public TaskItemDetailsDto(Guid id, string name, int currentCount, int version)
        {
            Id = id;
            Name = name;
            CurrentCount = currentCount;
            Version = version;
        }
    }
    [DataContract]
    public class TaskItemListDto
    {
        [DataMember(Order = 1)]
        public Guid Id;
        [DataMember(Order = 2)]
        public string Name;

        public TaskItemListDto(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
    
    public class TaskListView : Handles<TaskCreated>, Handles<TaskRenamed>, Handles<TaskRemoved>
    {
        public void Handle(TaskCreated message)
        {
            BullShitDatabase.list.Add(new TaskItemListDto(message.Id, message.Name));
        }
        
        public void Handle(TaskRenamed message)
        {
            var item = BullShitDatabase.list.Find(x => x.Id == message.Id);
            item.Name = message.Name;
        }

        public void Handle(TaskRemoved message)
        {
            BullShitDatabase.list.RemoveAll(x => x.Id == message.Id);
        }
    }

    public class TaskItemDetailView : Handles<TaskCreated>, Handles<TaskRenamed>, Handles<TaskRemoved>
    {
        public void Handle(TaskCreated message)
        {
            BullShitDatabase.details.Add(message.Id, new TaskItemDetailsDto(message.Id, message.Name, 0, 0));
        }

        private TaskItemDetailsDto GetDetailsItem(Guid id)
        {
            TaskItemDetailsDto d;
            if (!BullShitDatabase.details.TryGetValue(id, out d))
            {
                throw new InvalidOperationException("did not find the original Task this shouldnt happen");
            }
            return d;
        }
        
        public void Handle(TaskRenamed message)
        {
            TaskItemDetailsDto d = GetDetailsItem(message.Id);
            d.Name = message.Name;
            d.Version = message.Version;
        }

        public void Handle(TaskRemoved message)
        {
            BullShitDatabase.details.Remove(message.Id);
        }
    }

    public class ReadModelFacade : IReadModelFacade
    {
        public IEnumerable<TaskItemListDto> GetTaskItems()
        {
            return BullShitDatabase.list;
        }

        public TaskItemDetailsDto GetTaskItemDetails(Guid id)
        {
            try
            {
                var det = BullShitDatabase.details[id];
                return det;
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }
    }

    public static class BullShitDatabase
    {
        public static Dictionary<Guid, TaskItemDetailsDto> details = new Dictionary<Guid, TaskItemDetailsDto>();
        public static List<TaskItemListDto> list = new List<TaskItemListDto>();
    }
}

