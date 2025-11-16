namespace Core.Dtos.CodigoPostal
{
    public class CityDto : BaseDto
    {
        public required IEnumerable<BaseDto> Municipios { get; set; }
    }
}