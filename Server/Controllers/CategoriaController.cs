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
    public class CategoriaController : ControllerBase
    {

        private readonly IDbConnection _dbConnection;

        private readonly ILogger<CategoriaController> _logger;

        public CategoriaController(ILogger<CategoriaController> logger, IDbConnection dbConnection)
        {
            _logger = logger;
            _dbConnection = dbConnection;
        }

        [HttpGet]
        public async Task<IEnumerable<Categoria>> ObtenerTodos()
        {
            try
            {
                _logger.LogInformation("Obteniendo Categoria");
                var categorias = await _dbConnection.QueryAsync<Categoria>(@"
                    select IdCategoria, Descripcion  from Categoria
                    ");


                     _logger.LogInformation("Categorias Cargadas");

                return categorias;
            }
            catch (Exception ex)
            {

                _logger.LogError($"Error Cargando categorias {ex.Message}");

                return new List<Categoria>();
            }
        }


        [HttpGet("{id}")]
        public async Task<Categoria> ObtenerEspecifico(int id)
        {
            try
            {
                _logger.LogInformation("Obteniendo Categoria por ID");
                var categoria = await _dbConnection.QuerySingleOrDefaultAsync<Categoria>(@"
                    select IdCategoria, Descripcion  from Categoria where IdCategoria =@Id
                    ", new { id});


                _logger.LogInformation("Categoria Cargada");

                return categoria;
            }
            catch (Exception ex)
            {

                _logger.LogError($"Error Cargando categoria {ex.Message}");

                return new Categoria();
            }
        }

       [HttpPost]

       public async Task InsertarCategoria(Categoria entidad)
        {

            try
            {
                string query = "INSERT INTO Categoria(Descripcion) VALUES(@Descripcion)";
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
        public async Task ActualizaCategoria(Categoria entidad)
        {

            try
            {
                string query = "UPDATE Categoria SET Descripcion = @Descripcion WHERE IdCategoria = @IdCategoria";
                var parametros = new DynamicParameters();
                parametros.Add("Descripcion", entidad.Descripcion, DbType.String);
                parametros.Add("IdCategoria", entidad.IdCategoria, DbType.Int32);
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

                string query = "DELETE FROM  Categoria WHERE IdCategoria = @IdCategoria ";
                var parametros = new DynamicParameters();
                parametros.Add("IdCategoria", id, DbType.Int32);

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
