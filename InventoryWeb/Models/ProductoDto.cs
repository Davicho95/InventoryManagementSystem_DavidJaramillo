namespace InventoryWeb.Models
{
    public class ProductoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public int CantidadEnStock { get; set; }
        public decimal PrecioUnitario { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public class CrearProductoDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public int CantidadEnStock { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
