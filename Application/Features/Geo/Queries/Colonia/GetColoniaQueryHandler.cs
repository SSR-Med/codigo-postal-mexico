namespace Application.Features.Geo.Queries.Colonia
{
    using Core.Dtos.CodigoPostal;
    using Core.Interfaces.Services;
    using MediatR;

    public class GetColoniaQueryHandler : IRequestHandler<GetColoniaQuery, NeighborhoodDto>
    {
        private readonly ICodigoPostalService _codigoPostalService;

        public GetColoniaQueryHandler(ICodigoPostalService codigoPostalService)
        {
            _codigoPostalService = codigoPostalService;
        }

        public async Task<NeighborhoodDto> Handle(GetColoniaQuery request, CancellationToken cancellationToken)
        {
            await _codigoPostalService.InitializeAsync();
            var colonias = await _codigoPostalService.GetNeighborhoodsByCityCode(request.CodigoEstado, request.CodigoMunicipio);
            return colonias;
        }
    }
}