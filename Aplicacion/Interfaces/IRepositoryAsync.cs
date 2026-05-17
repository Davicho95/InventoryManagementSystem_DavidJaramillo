namespace Aplicacion.Interfaces
{
    public interface IRepositoryAsync<T> where T : class
    {
        Task<IReadOnlyList<T>> ObtenerTodosAsync();
        Task<T?> ObtenerPorIdAsync(int id);
        Task<T> AgregarAsync(T entidad);
        Task ActualizarAsync(T entidad);
        Task EliminarAsync(T entidad);
    }
}
