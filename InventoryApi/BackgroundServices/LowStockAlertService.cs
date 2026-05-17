using Microsoft.EntityFrameworkCore;
using Persistencia.Contexto;

namespace InventoryApi.BackgroundServices
{
    public class LowStockAlertService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<LowStockAlertService> _logger;
        private readonly int _umbralBajoStock = 10;
        private readonly TimeSpan _intervalo = TimeSpan.FromSeconds(10);

        public LowStockAlertService(
            IServiceScopeFactory scopeFactory,
            ILogger<LowStockAlertService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Low-stock alert service started. Checking every {Intervalo} minutes.",
                _intervalo.TotalMinutes);

            while (!stoppingToken.IsCancellationRequested)
            {
                await VerificarStockBajoAsync();
                await Task.Delay(_intervalo, stoppingToken);
            }
        }

        private async Task VerificarStockBajoAsync()
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var productosBajoStock = await context.Products
                    .Where(p => p.QuantityInStock <= _umbralBajoStock)
                    .OrderBy(p => p.QuantityInStock)
                    .ToListAsync();

                if (!productosBajoStock.Any())
                {
                    _logger.LogInformation("Low-stock check completed. All products have sufficient stock.");
                    return;
                }

                _logger.LogWarning(
                    "Low-stock alert: {Count} product(s) below threshold of {Umbral} units.",
                    productosBajoStock.Count,
                    _umbralBajoStock);

                foreach (var producto in productosBajoStock)
                {
                    _logger.LogWarning(
                        "LOW STOCK — Product: {Nombre} | SKU: {SKU} | Stock: {Stock} units",
                        producto.Name,
                        producto.SKU,
                        producto.QuantityInStock);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during low-stock check.");
            }
        }
    }
}
