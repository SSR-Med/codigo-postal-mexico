namespace Application.Features.Geo.Queries.Colonia
{
    using Core.Dtos.CodigoPostal;
    using MediatR;

    public class GetColoniaQuery : IRequest<NeighborhoodDto>
    {
        /// <summary>
        /// El código del estado
        /// </summary>
        /// <example>09</example>
        public required string CodigoEstado { get; set; }
        /// <summary>
        /// El código del municipio
        /// </summary>
        /// <example>001</example>
        public required string CodigoMunicipio { get; set; }
    }
}