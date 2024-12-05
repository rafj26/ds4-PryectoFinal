
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using GuessingGame.API.Models;

namespace GuessingGame.API.Services
{
    public class GameService
    {
        private readonly string _connectionString;

        public GameService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public async Task SaveGameResult(GameResult result)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Usando Dapper para ejecutar el procedimiento almacenado
                await connection.ExecuteAsync(
                    "sp_SaveGameResult",
                    new
                    {
                        result.PlayerName,
                        result.GuessCount,
                        result.TimeTaken
                    },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<IEnumerable<Player>> GetTopPlayers()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Usando Dapper para ejecutar el procedimiento almacenado
                return await connection.QueryAsync<Player>(
                    "sp_GetTopPlayers",
                    commandType: CommandType.StoredProcedure
                );
            }
        }
    }
}