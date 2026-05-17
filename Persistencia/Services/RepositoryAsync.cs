using Aplicacion.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistencia.Contexto;

namespace Persistencia.Services
{
    public class RepositoryAsync<T> : IRepositoryAsync<T> where T : class
    {
        private readonly AppDbContext _contexto;

        public RepositoryAsync(AppDbContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<IReadOnlyList<T>> ObtenerTodosAsync()
            => await _contexto.Set<T>().ToListAsync();

        public async Task<T?> ObtenerPorIdAsync(int id)
            => await _contexto.Set<T>().FindAsync(id);

        public async Task<T> AgregarAsync(T entidad)
        {
            await _contexto.Set<T>().AddAsync(entidad);
            await _contexto.SaveChangesAsync();
            return entidad;
        }

        public async Task ActualizarAsync(T entidad)
        {
            _contexto.Set<T>().Update(entidad);
            await _contexto.SaveChangesAsync();
        }

        public async Task EliminarAsync(T entidad)
        {
            _contexto.Set<T>().Remove(entidad);
            await _contexto.SaveChangesAsync();
        }
    }
}
