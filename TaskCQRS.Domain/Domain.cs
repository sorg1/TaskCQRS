﻿using System;
using System.Collections.Generic;

using TaskCQRS.Core;
using TaskCQRS.Domain.Tasks;

namespace TaskCQRS.Domain
{
    public class TaskItem : AggregateRoot
    {
        private bool _activated;
        private Guid _id;

        private void Apply(TaskCreated e)
        {
            _id = e.Id;
            _activated = true;
        }

        private void Apply(TaskRemoved e)
        {
            _activated = false;
        }
        public void ChangeName(string newName)
        {
            if (string.IsNullOrEmpty(newName)) throw new ArgumentException("newName");
            ApplyChange(new TaskRenamed(_id, newName));
        }

        public void Remove()
        {
            if (!_activated) throw new InvalidOperationException("already removed");
            ApplyChange(new TaskRemoved(_id));
        }

/*
        

        public void Remove(int count)
        {
            if (count <= 0) throw new InvalidOperationException("cant remove negative count from inventory");
            ApplyChange(new ItemsRemovedFromInventory(_id, count));
        }
        
        public void CheckIn(int count)
        {
            if (count <= 0) throw new InvalidOperationException("must have a count greater than 0 to add to inventory");
            ApplyChange(new ItemsCheckedInToInventory(_id, count));
        }

        public void Deactivate()
        {
            if (!_activated) throw new InvalidOperationException("already deactivated");
            ApplyChange(new InventoryItemDeactivated(_id));
        }
*/
        public override Guid Id
        {
            get { return _id; }
        }

        public TaskItem()
        {
            // used to create in repository ... many ways to avoid this, eg making private constructor
        }

        public TaskItem(Guid id, string name)
        {
            ApplyChange(new TaskCreated(id, name));
        }
    }
    public abstract class AggregateRoot
    {
        private readonly List<Event> _changes = new List<Event>();

        public abstract Guid Id { get; }
        public int Version { get; internal set; }

        public IEnumerable<Event> GetUncommittedChanges()
        {
            return _changes;
        }

        public void MarkChangesAsCommitted()
        {
            _changes.Clear();
        }

        public void LoadsFromHistory(IEnumerable<Event> history)
        {
            foreach (var e in history) ApplyChange(e, false);
        }

        protected void ApplyChange(Event @event)
        {
            ApplyChange(@event, true);
        }

        private void ApplyChange(Event @event, bool isNew)
        {
            this.AsDynamic().Apply(@event);
            if (isNew) _changes.Add(@event);
        }
    }

    public interface IRepository<T> where T : AggregateRoot, new()
    {
        void Save(AggregateRoot aggregate, int expectedVersion);
        T GetById(Guid id);
    }

    public class Repository<T> : IRepository<T> where T : AggregateRoot, new() //shortcut you can do as you see fit with new()
    {
        private readonly IEventStore _storage;

        public Repository(IEventStore storage)
        {
            _storage = storage;
        }

        public void Save(AggregateRoot aggregate, int expectedVersion)
        {
            _storage.SaveEvents(aggregate.Id, aggregate.GetUncommittedChanges(), expectedVersion);
        }

        public T GetById(Guid id)
        {
            var obj = new T();//lots of ways to do this
            var e = _storage.GetEventsForAggregate(id);
            obj.LoadsFromHistory(e);
            return obj;
        }
    }

}
