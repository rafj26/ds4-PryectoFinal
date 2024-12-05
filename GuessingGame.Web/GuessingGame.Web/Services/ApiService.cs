using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using GuessingGame.Web.Models;

namespace GuessingGame.Web.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public ApiService()
        {
            _baseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44354/");
        }

        public async Task SaveGameResult(GameResult result)
        {
            await _httpClient.PostAsJsonAsync("api/game/save", result);
        }

        public async Task<List<Player>> GetTopPlayers()
        {
            var response = await _httpClient.GetAsync("api/game/top");
            return await response.Content.ReadAsAsync<List<Player>>();
        }
    }
}