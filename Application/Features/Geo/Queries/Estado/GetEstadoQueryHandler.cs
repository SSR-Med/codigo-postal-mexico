namespace Application.Features.Geo.Queries.Estado
{
    using Core.Dtos.CodigoPostal;
    using Core.Interfaces.Services;
    using MediatR;
    public class GetEstadoQueryHandler: IRequestHandler<GetEstadoQuery, IEnumerable<BaseDto>>
    {
        private readonly ICodigoPostalService _codigoPostalService;

        public GetEstadoQueryHandler(ICodigoPostalService codigoPostalService)
        {
            _codigoPostalService = codigoPostalService;
        }

        public async Task<IEnumerable<BaseDto>> Handle(GetEstadoQuery request, CancellationToken cancellationToken)
        {
            await _codigoPostalService.InitializeAsync();
            var estados = await _codigoPostalService.GetStateList();
            return estados;
        }
    }
}