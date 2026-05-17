namespace InventoryWeb.Models
{
    public enum MovementType { Inbound, Outbound }

    public class MovimientoStockDto
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public MovementType Tipo { get; set; }
        public int Cantidad { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
    }

    public class CrearMovimientoDto
    {
        public MovementType Tipo { get; set; } = MovementType.Inbound;
        public int Cantidad { get; set; }
        public string Motivo { get; set; } = string.Empty;
    }
}
