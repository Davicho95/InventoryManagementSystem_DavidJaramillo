using Dominio.Enums;

namespace Aplicacion.Dto.StockMovement
{
    public class CrearMovimientoStockDto
    {
        public MovementType Tipo { get; set; }
        public int Cantidad { get; set; }
        public string Motivo { get; set; } = string.Empty;
    }
}
