using Dominio.Enums;

namespace Aplicacion.Dto.StockMovement
{
    public class MovimientoStockDto
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public MovementType Tipo { get; set; }
        public int Cantidad { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
    }
}
