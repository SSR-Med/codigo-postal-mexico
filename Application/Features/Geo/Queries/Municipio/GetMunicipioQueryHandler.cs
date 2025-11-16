namespace Application.Features.Geo.Queries.Municipio
{
    using Core.Dtos.CodigoPostal;
    using Core.Interfaces.Services;
    using MediatR;
    public class GetMunicipioQueryHandler: IRequestHandler<GetMunicipioQuery, CityDto>
    {
        private readonly ICodigoPostalService _codigoPostalService;

        public GetMunicipioQueryHandler(ICodigoPostalService codigoPostalService)
        {
            _codigoPostalService = codigoPostalService;
        }

        public async Task<CityDto> Handle(GetMunicipioQuery request, CancellationToken cancellationToken)
        {
            await _codigoPostalService.InitializeAsync();
            var municipios = await _codigoPostalService.GetCitiesByStateCode(request.CodigoEstado);
            return municipios;
        }
    }
}
