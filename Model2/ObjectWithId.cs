using System;

namespace Model
{
    public abstract class ObjectWithId
    {
        public int Id { get; }

        protected ObjectWithId() => Id = IdGenerator.Instance.GetId();
    }



    public class IdGenerator
    {
        private static Lazy<IdGenerator> _instance = new Lazy<IdGenerator>(() => new IdGenerator());
        public static IdGenerator Instance => _instance.Value;

        private IdGenerator()
        {
        }

        private int _curId = 10000;

        public int GetId() => ++_curId;
    }
}