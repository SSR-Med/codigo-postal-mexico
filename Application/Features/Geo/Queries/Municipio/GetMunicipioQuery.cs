namespace Application.Features.Geo.Queries.Municipio
{
    using Core.Dtos.CodigoPostal;
    using MediatR;
    public record GetMunicipioQuery: IRequest<CityDto>
    {
        /// <summary>
        /// El código del estado
        /// </summary>
        /// <example>09</example>
        public required string CodigoEstado { get; init; }
    }
}
