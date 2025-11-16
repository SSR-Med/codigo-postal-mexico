namespace Core.Dtos.CodigoPostal
{
    public class ColoniaDto
    {
        /// <summary>
        /// El nombre de la colonia
        /// </summary>
        /// <example>Centro</example>
        public required string Nombre { get; set; }
        /// <summary>
        /// El código postal de la colonia
        /// </summary>
        /// <example>06000</example>
        public required string CodigoPostal { get; set; }
    }
}