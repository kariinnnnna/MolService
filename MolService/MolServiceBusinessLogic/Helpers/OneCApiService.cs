using MolServiceBusinessLogic.Models.OneC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MolServiceBusinessLogic.Helpers
{
    public class OneCApiService
    {
        private readonly HttpClient _httpClient;

        private const string InventoryUrl = "http://172.20.1.61/bgu_new/hs/BGU_OS_Data/inventoryNumbers";

        public OneCApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<OneCInventoryResponse> GetInventoryAsync(string username, string password)
        {
            var credentials = Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{username}:{password}"));

            using var request = new HttpRequestMessage(HttpMethod.Get, InventoryUrl);

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Basic", credentials);

            request.Headers.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Ошибка запроса к 1С: {(int)response.StatusCode}. {content}");
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var result = JsonSerializer.Deserialize<OneCInventoryResponse>(content, options);

            if (result == null)
            {
                throw new Exception("Не удалось разобрать ответ от 1С.");
            }

            return result;
        }
    }
}
