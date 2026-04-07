using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace MolServiceBusinessLogic.Helpers
{
    public class CoreApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public CoreApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<List<CoreClassroomDto>> GetClassroomsAsync()
        {
            var baseUrl = _configuration["CoreApi:BaseUrl"];
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new InvalidOperationException("Не настроен CoreApi:BaseUrl");
            }

            var requestUrl = $"{baseUrl}/Classrooms/GetClassroomList";

            var response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<List<CoreClassroomDto>>();
            return result ?? new List<CoreClassroomDto>();
        }
    }
}
