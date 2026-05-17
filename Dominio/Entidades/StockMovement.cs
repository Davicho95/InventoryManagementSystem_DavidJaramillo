using Dominio.Enums;

namespace Dominio.Entidades
{
    public class StockMovement
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public MovementType Type { get; set; }
        public int Quantity { get; set; }
        public string Reason { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }

        #region Navegaciones
        public virtual Product Product { get; set; } = null!;
        #endregion
    }
}
