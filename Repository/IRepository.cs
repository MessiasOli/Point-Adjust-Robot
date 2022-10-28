namespace Point_Adjust_Robot.Core.Interface
{
    public interface IRepository <T> where T : class
    {
        public T Get(T model);
        public T Get(string id);
        public Task<T> GetByParams(params string[] param);
        public bool Add(T model);
        public bool Update(T model);
        public bool Delete(T model);

    }
}
