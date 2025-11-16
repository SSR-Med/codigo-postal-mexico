namespace Core.Interfaces.Services
{
    using Core.Dtos.CodigoPostal;
    public interface ICodigoPostalService
    {
        Task InitializeAsync();
        Task<IEnumerable<BaseDto>> GetStateList();
        Task<CityDto> GetCitiesByStateCode(string stateCode);
        Task<NeighborhoodDto> GetNeighborhoodsByCityCode(string stateCode, string cityCode);
    }
}