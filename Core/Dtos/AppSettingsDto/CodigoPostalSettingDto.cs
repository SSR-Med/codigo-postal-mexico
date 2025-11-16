namespace Core.Dtos.AppSettingsDto
{
    public class CodigoPostalSettingDto
    {
        public string? BaseUrl { get; set; }
        public string? EstadoSelect { get; set; }
        public string? MunicipioSelect { get; set; }
        public int TimeoutPage { get; set; }
        public int TimeoutElement { get; set; }
        public int RetryCount { get; set; }
        public int RetryDelayMilliseconds { get; set; }
        public string? SearchButton { get; set; }
        public string? ResultTable { get; set; }
    }
}