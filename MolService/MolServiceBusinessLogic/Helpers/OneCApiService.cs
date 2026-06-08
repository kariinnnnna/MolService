using Microsoft.Extensions.Configuration;
using MolServiceBusinessLogic.Models.OneC;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MolServiceBusinessLogic.Helpers
{
    public class OneCApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _inventoryUrl;
        private readonly string _materialStocksUrl;
        private readonly bool _useBasicAuth;

        public OneCApiService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;

            _inventoryUrl = configuration["OneC:InventoryUrl"]
                ?? throw new Exception("Не указан адрес OneC:InventoryUrl в appsettings.json");

            _materialStocksUrl = configuration["OneC:MaterialStocksUrl"]
                ?? throw new Exception("Не указан адрес OneC:MaterialStocksUrl в appsettings.json");

            _useBasicAuth = bool.TryParse(
                configuration["OneC:UseBasicAuth"],
                out var useBasicAuth) && useBasicAuth;
        }

        public async Task<OneCInventoryResponse> GetInventoryAsync(
            string username,
            string password)
        {
            return await SendOneCRequestAsync<OneCInventoryResponse>(
                _inventoryUrl,
                username,
                password);
        }

        public async Task<OneCMaterialStockResponse> GetMaterialStocksAsync(
            string username,
            string password)
        {
            return await SendOneCRequestAsync<OneCMaterialStockResponse>(
                _materialStocksUrl,
                username,
                password);
        }

        private async Task<T> SendOneCRequestAsync<T>(
            string url,
            string username,
            string password)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, url);

            if (_useBasicAuth)
            {
                var credentials = Convert.ToBase64String(
                    Encoding.UTF8.GetBytes($"{username}:{password}"));

                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Basic", credentials);
            }

            request.Headers.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"Ошибка запроса к 1С: {(int)response.StatusCode}. " +
                    $"Адрес: {url}. Ответ: {content}");
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var result = JsonSerializer.Deserialize<T>(content, options);

            if (result == null)
            {
                throw new Exception(
                    $"Не удалось разобрать ответ от 1С. Адрес: {url}. Ответ: {content}");
            }

            return result;
        }
    }
}