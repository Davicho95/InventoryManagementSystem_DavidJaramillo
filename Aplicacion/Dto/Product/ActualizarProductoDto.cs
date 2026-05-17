namespace Aplicacion.Dto.Product
{
    public class ActualizarProductoDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public int CantidadEnStock { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
