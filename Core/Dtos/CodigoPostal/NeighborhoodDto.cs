namespace Core.Dtos.CodigoPostal
{
    public class NeighborhoodDto : BaseDto
    {
        public required IEnumerable<ColoniaDto> Colonias { get; set; }
    }
}