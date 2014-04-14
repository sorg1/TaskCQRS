using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TaskCQRS.Core;
using TaskCQRS.Domain.Tasks;

namespace TaskCQRS.Domain
{
    public class CommandHandlers : Handles<CreateTask>, Handles<RenameTask>, Handles<RemoveTask>
    {
        private readonly IRepository<TaskItem> _repository;

        public CommandHandlers(IRepository<TaskItem> repository)
        {
            _repository = repository;
        }
        public void Handle(CreateTask message)
        {
            var item = new TaskItem(message.Id, message.Name);
            _repository.Save(item, -1);
        }
        public void Handle(RenameTask message)
        {
            var item = _repository.GetById(message.Id);
            item.ChangeName(message.Name);
            _repository.Save(item, message.OriginalVersion);
        }
        public void Handle(RemoveTask message)
        {
            var item = _repository.GetById(message.Id);
            item.Remove();
            _repository.Save(item, message.OriginalVersion);
        }
    }
}
