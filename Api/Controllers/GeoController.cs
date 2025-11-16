namespace Api.Controllers
{
    using Application.Features.Geo.Queries.Colonia;
    using Application.Features.Geo.Queries.Estado;
    using Application.Features.Geo.Queries.Municipio;
    using Core.Dtos.CodigoPostal;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    [ApiController]
    [Route("api")]
    public class GeoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GeoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("estados")]
        [SwaggerOperation(Summary = "Obtiene la lista de estados.", Description = "Obtiene la lista de estados.")]
        public async Task<ActionResult<IEnumerable<BaseDto>>> GetEstados()
        {
            var estados = await _mediator.Send(new GetEstadoQuery());
            return Ok(estados);
        }

        [HttpGet("estados/{codigo_estado}/municipios")]
        [SwaggerOperation(Summary = "Obtiene la lista de municipios por estado.", Description = "Obtiene la lista de municipios según el código del estado.")]
        public async Task<ActionResult<CityDto>> GetMunicipios([FromRoute] GetMunicipioQuery query)
        {
            var municipios = await _mediator.Send(query);
            return Ok(municipios);
        }

        [HttpGet("estados/{codigo_estado}/municipios/{codigo_municipio}/colonias")]
        [SwaggerOperation(Summary = "Obtiene la lista de colonias por municipio.", Description = "Obtiene la lista de colonias según el código del estado y el código del municipio.")]
        public async Task<ActionResult<NeighborhoodDto>> GetColonias([FromRoute] GetColoniaQuery query)
        {
            var colonias = await _mediator.Send(query);
            return Ok(colonias);
        }
    }
}