namespace DalApi
{
    public interface ICrud<T> where T : struct
    {
        public int Add(T t);
        public void Delete(int id);
        public void Update(T t);
        public IEnumerable<T?> Get(Func<T?, bool>? func = null);
        public T Get(int id);
        public T GetIf(Func<T?, bool>? func);
    }
}
