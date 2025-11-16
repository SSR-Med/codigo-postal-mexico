namespace Core.Dtos.CodigoPostal
{
    public class BaseDto
    {
        /// <summary>
        /// El código del elemento
        /// </summary>
        /// <example>09</example>
        public required string Codigo { get; set; }
        /// <summary>
        /// El nombre completo del elemento
        /// </summary>
        /// <example>CIUDAD DE MÉXICO</example>
        public required string Nombre { get; set; }
    }
}