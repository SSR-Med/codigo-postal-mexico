namespace Application.Features.Geo.Queries.Estado
{
    using Core.Dtos.CodigoPostal;
    using MediatR;
    public class GetEstadoQuery: IRequest<IEnumerable<BaseDto>>;
}