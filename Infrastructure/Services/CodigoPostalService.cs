namespace Infrastructure.Services
{
    using Core.Dtos.AppSettingsDto;
    using Core.Dtos.CodigoPostal;
    using Core.Exceptions;
    using Core.Interfaces.Services;
    using Microsoft.Extensions.Options;
    using Microsoft.Playwright;
    public class CodigoPostalService: ICodigoPostalService
    {
        private IPlaywright? _playwright;
        private IBrowser? _browser;
        private IPage? _page;
        private CodigoPostalSettingDto _settings;

        public CodigoPostalService(IOptions<CodigoPostalSettingDto> settings)
        {
            _settings = settings.Value;
        }

        public async Task InitializeAsync()
        {
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true
            });
            _page = await _browser.NewPageAsync();
            
            await NavigateWithRetryAsync(_settings.BaseUrl!, _settings.EstadoSelect!);
        }

        private async Task NavigateWithRetryAsync(string url, string selectorToWait)
        {
            Exception? lastException = null;

            for (int attempt = 0; attempt < _settings.RetryCount; attempt++)
            {
                try
                {
                    if (attempt == 0)
                    {
                        await _page!.GotoAsync(url, new PageGotoOptions
                        {
                            WaitUntil = WaitUntilState.DOMContentLoaded,
                            Timeout = _settings.TimeoutPage
                        });
                    }
                    else
                    {
                        await Task.Delay(_settings.RetryDelayMilliseconds);
                        await _page!.ReloadAsync(new PageReloadOptions
                        {
                            WaitUntil = WaitUntilState.DOMContentLoaded,
                            Timeout = _settings.TimeoutPage
                        });
                    }

                    await _page!.WaitForSelectorAsync(selectorToWait, new PageWaitForSelectorOptions
                    {
                        Timeout = _settings.TimeoutElement
                    });
                    
                    return;
                }
                catch (Exception ex)
                {
                    lastException = ex;
                }
            }

            throw new Exception(
                $"No se pudo cargar la página '{url}' después de {_settings.RetryCount} intentos. " +
                $"Selector esperado: '{selectorToWait}'.",
                lastException
            );
        }

        public async Task<IEnumerable<BaseDto>> GetStateList()
        {
            await _page!.WaitForSelectorAsync(_settings.EstadoSelect!);
            return await GetOptionsFromSelectAsync(_settings.EstadoSelect!, "00");
        }

        public async Task<CityDto> GetCitiesByStateCode(string stateCode)
        {
            await ValidateOptionExistsAsync(_settings.EstadoSelect!, stateCode, "estado");
            
            var estadoNombre = await GetOptionTextAsync(_settings.EstadoSelect!, stateCode);
            
            await SelectOptionWithPostbackAsync(_settings.EstadoSelect!, stateCode);
            
            await WaitForSelectorVisibleAsync(_settings.MunicipioSelect!);
            
            var municipios = await GetOptionsFromSelectAsync(_settings.MunicipioSelect!, "000");

            return new CityDto
            {
                Codigo = stateCode,
                Nombre = estadoNombre,
                Municipios = municipios
            };
        }

        public async Task<NeighborhoodDto> GetNeighborhoodsByCityCode(string stateCode, string cityCode)
        {
            CityDto city = await GetCitiesByStateCode(stateCode);
            string cityName = city.Municipios
                .FirstOrDefault(m => m.Codigo == cityCode)?.Nombre!;

            await ValidateOptionExistsAsync(_settings.MunicipioSelect!, cityCode, "municipio");
            await SelectOptionWithPostbackAsync(_settings.MunicipioSelect!, cityCode);

            await ClickSearchButton();

            await WaitForSelectorVisibleAsync(_settings.ResultTable!);

            var colonias = await ExtractColoniasFromTableAsync();

            return new NeighborhoodDto
            {
                Codigo = cityCode,
                Nombre = cityName,
                Colonias = colonias
            };
        }

        private async Task ClickSearchButton()
        {
            await Task.WhenAll(
                _page!.WaitForLoadStateAsync(LoadState.NetworkIdle, new PageWaitForLoadStateOptions 
                { 
                    Timeout = _settings.TimeoutPage 
                }),
                _page.ClickAsync(_settings.SearchButton!)
            );
        }

        private async Task ValidateOptionExistsAsync(string selector, string value, string fieldName)
        {
            var optionExists = await _page!.Locator($"{selector} option[value='{value}']").CountAsync() > 0;
            
            if (!optionExists)
            {
                throw new ConflictException($"El código de {fieldName} '{value}' no existe o no es válido.");
            }
        }

        private async Task<string> GetOptionTextAsync(string selector, string value)
        {
            var optionText = await _page!.Locator($"{selector} option[value='{value}']").InnerTextAsync();
            return optionText.Trim().ToUpper();
        }

        private async Task SelectOptionWithPostbackAsync(string selector, string value)
        {
            await Task.WhenAll(
                _page!.WaitForLoadStateAsync(LoadState.NetworkIdle, new PageWaitForLoadStateOptions 
                { 
                    Timeout = _settings.TimeoutPage 
                }),
                _page.SelectOptionAsync(selector, value)
            );
        }

        private async Task WaitForSelectorVisibleAsync(string selector)
        {
            await _page!.WaitForSelectorAsync(selector, new PageWaitForSelectorOptions
            {
                Timeout = _settings.TimeoutElement,
                State = WaitForSelectorState.Visible
            });
            
            await Task.Delay(500);
        }

        private async Task<List<BaseDto>> GetOptionsFromSelectAsync(string selector, string excludeValue)
        {
            var options = await _page!.Locator($"{selector} option").AllAsync();

            var optionTasks = options.Select(async option =>
            {
                var codigo = await option.GetAttributeAsync("value");
                var nombre = await option.InnerTextAsync();
                return new { Codigo = codigo, Nombre = nombre.Trim().ToUpper() };
            });

            var optionResults = await Task.WhenAll(optionTasks);

            return optionResults
                .Where(r => r.Codigo != excludeValue)
                .Select(r => new BaseDto
                {
                    Codigo = r.Codigo!,
                    Nombre = r.Nombre
                })
                .ToList();
        }

        private async Task<List<ColoniaDto>> ExtractColoniasFromTableAsync()
        {
            var rows = await _page!.Locator($"{_settings.ResultTable}").AllAsync();

            var coloniaTasks = rows.Select(async row =>
            {
            var cells = await row.Locator("td").AllAsync();
            
            if (cells.Count >= 2)
            {
                var codigoPostal = (await cells[0].InnerTextAsync()).Trim();
                var nombreColonia = (await cells[1].InnerTextAsync()).Trim().ToUpper();

                if (!string.IsNullOrWhiteSpace(codigoPostal) && !string.IsNullOrWhiteSpace(nombreColonia))
                {
                return new ColoniaDto
                {
                    CodigoPostal = codigoPostal,
                    Nombre = nombreColonia
                };
                }
            }
            return null;
            });

            var colonias = (await Task.WhenAll(coloniaTasks)).Where(c => c != null).ToList()!;

            if (colonias.Count == 0)
            {
            throw new ConflictException("No se encontraron colonias.");
            }

            return colonias!;
        }
    }
}