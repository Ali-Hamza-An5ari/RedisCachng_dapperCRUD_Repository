namespace dapperCRUD.Services
{
    public interface IGenericService<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(int id);
        Task<T> Get(int id);
        Task Delete(int id);
        Task<T> Add(T entity);
        Task<T> Update(T entity);

    }
}
