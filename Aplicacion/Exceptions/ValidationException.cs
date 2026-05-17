namespace Aplicacion.Exceptions
{
    public class ValidationException : Exception
    {
        public List<string> Errors { get; }
        public ValidationException() : base("Por favor, valide la información ingresada.")
        {
            Errors = new List<string>();
        }

        public ValidationException(IEnumerable<string> failures) : this()
        {
            foreach (var failure in failures)
            {
                Errors.Add(failure);
            }
        }
    }
}
