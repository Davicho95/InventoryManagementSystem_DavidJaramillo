namespace Dominio.Entidades
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int QuantityInStock { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        #region Navegaciones
        public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
        #endregion
    }
}
