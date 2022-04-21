using HealthPetAPP.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
namespace HealthPetAPP.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TipoAnimalController : ControllerBase
    {

        private readonly IDbConnection _dbConnection;

        private readonly ILogger<TipoAnimalController> _logger;

        public TipoAnimalController(ILogger<TipoAnimalController> logger, IDbConnection dbConnection)
        {
            _logger = logger;
            _dbConnection = dbConnection;
        }

        [HttpGet]
        public async Task<IEnumerable<TipoAnimal>> ObtenerTodos()
        {
            try
            {
                _logger.LogInformation("Obteniendo TipoAnimal");
                var tipoAnimales = await _dbConnection.QueryAsync<TipoAnimal>(@"
                    select IdTipoAnimal, Descripcion  from TipoAnimal
                    ");


                     _logger.LogInformation("TipoAnimals Cargadas");

                return tipoAnimales;
            }
            catch (Exception ex)
            {

                _logger.LogError($"Error Cargando tipoAnimales {ex.Message}");

                return new List<TipoAnimal>();
            }
        }


        [HttpGet("{id}")]
        public async Task<TipoAnimal> ObtenerEspecifico(int id)
        {
            try
            {
                _logger.LogInformation("Obteniendo TipoAnimal por ID");
                var tipoAnimal = await _dbConnection.QuerySingleOrDefaultAsync<TipoAnimal>(@"
                    select IdTipoAnimal, Descripcion  from TipoAnimal where IdTipoAnimal =@Id
                    ", new { id});


                _logger.LogInformation("TipoAnimal Cargada");

                return tipoAnimal;
            }
            catch (Exception ex)
            {

                _logger.LogError($"Error Cargando tipoAnimal {ex.Message}");

                return new TipoAnimal();
            }
        }

       [HttpPost]

       public async Task InsertarTipoAnimal(TipoAnimal entidad)
        {

            try
            {
                string query = "INSERT INTO TipoAnimal(Descripcion) VALUES(@Descripcion)";
                var parametros = new DynamicParameters();
                parametros.Add("Descripcion", entidad.Descripcion, DbType.String);

                _logger.LogInformation("Se realiza la ejecución de la inserción");
                // devuelve la cantidad defilas afectadas.
                var result=   await _dbConnection.ExecuteAsync(query, parametros);

                _logger.LogInformation("Ejecución Realizada");

            }
            catch (Exception ex)
            {

                _logger.LogError($"Ocurrió un error en el proceso {ex.Message}");
            }
        }


        [HttpPut]
        public async Task ActualizaTipoAnimal(TipoAnimal entidad)
        {

            try
            {
                string query = "UPDATE TipoAnimal SET Descripcion = @Descripcion WHERE IdTipoAnimal = @IdTipoAnimal";
                var parametros = new DynamicParameters();
                parametros.Add("Descripcion", entidad.Descripcion, DbType.String);
                parametros.Add("IdTipoAnimal", entidad.IdTipoAnimal, DbType.Int32);
                _logger.LogInformation("Se realiza la ejecución de la actualización");
                // devuelve la cantidad defilas afectadas.
                var result = await _dbConnection.ExecuteAsync(query, parametros);

                _logger.LogInformation("Ejecución Realizada");

            }
            catch (Exception ex)
            {

                _logger.LogError($"Ocurrió un error en el proceso {ex.Message}");
            }
        }

        [HttpDelete("{id}")]

        public async Task Eliminar(int id)
        {
            try
            {

                string query = "DELETE FROM  TipoAnimal WHERE IdTipoAnimal = @IdTipoAnimal ";
                var parametros = new DynamicParameters();
                parametros.Add("IdTipoAnimal", id, DbType.Int32);

                _logger.LogInformation("Se realiza la ejecución de la eliminación");
                // devuelve la cantidad defilas afectadas.
                var result = await _dbConnection.ExecuteAsync(query, parametros);

                _logger.LogInformation("Ejecución Realizada");
            }
                catch (Exception ex )
            {
                _logger.LogError($"Ocurrió un error en el proceso {ex.Message}");
            }
        }
    }
}
