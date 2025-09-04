namespace LibraryProject.Helpers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using LibraryProject.Logger;

	public abstract class Container<T>
        where T : IEntity
    {
        private int _nextId = -1;

        protected Container(ILogger logger)
        {
            Logger = logger;
        }

        public List<T> Entities { get; } = new List<T>();

        public int Count => Entities.Count;

        public int HighestId
        {
            get
            {
                if (_nextId == -1)
                {
                    var max = Entities
                        .Select(e => Convert.ToInt32(e.Id))
                        .DefaultIfEmpty(-1)
                        .Max();

                    _nextId = (max < 0) ? 0 : max + 1;
                    return _nextId;
                }

                return ++_nextId;
            }
        }

        public ILogger Logger { get; }

        public virtual IEnumerable<T> FindAll(
            Func<T, string> func,
            string valueToCompare)
        {
            foreach (var entity in Entities)
            {
                if (func.Invoke(entity) == valueToCompare)
                    yield return entity;
            }
        }

        public virtual T Find(
            Func<T, string> func,
            string valueToCompare)
        {
            foreach (var entity in Entities)
            {
                if (func.Invoke(entity) == valueToCompare)
                    return entity;
            }

            return default;
        }

        public virtual void Add(T t)
        {
            var entry = Find(
                x => x.Id,
                t.Id);

            if (Equals(entry, default(T)))
            {
                Entities.Add(t);
            }
        }

        public abstract void InitEntities(
            ElementConfig element);

        public abstract void Create(
            IReadOnlyCollection<ElementConfig> elements);
    }
}