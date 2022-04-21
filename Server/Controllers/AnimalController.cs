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
    public class AnimalController : ControllerBase
    {

        private readonly IDbConnection _dbConnection;

        private readonly ILogger<AnimalController> _logger;

        public AnimalController(ILogger<AnimalController> logger, IDbConnection dbConnection)
        {
            _logger = logger;
            _dbConnection = dbConnection;
        }

        [HttpGet]
        public async Task<IEnumerable<Animal>> ObtenerTodos()
        {
            try
            {
                _logger.LogInformation("Obteniendo Animal");
                var animales = await _dbConnection.QueryAsync<Animal>(@"
                    SELECT IdAnimal, Nombre,AN.IdTipoAnimal, TA.Descripcion AS DesTipoAnimal,Edad,Raza  FROM Animal AN 
                        INNER JOIN TipoAnimal TA ON AN.IdTipoAnimal = TA.IdTipoAnimal
                    ");


                     _logger.LogInformation("Animales Cargadas");

                return animales;
            }
            catch (Exception ex)
            {

                _logger.LogError($"Error Cargando animals {ex.Message}");

                return new List<Animal>();
            }
        }


        [HttpGet("{id}")]
        public async Task<Animal> ObtenerEspecifico(int id)
        {
            try
            {
                _logger.LogInformation("Obteniendo Animal por ID");
                var animal = await _dbConnection.QuerySingleOrDefaultAsync<Animal>(@"
                         SELECT IdAnimal, Nombre,AN.IdTipoAnimal, TA.Descripcion AS DesTipoAnimal,Edad,Raza  
                            FROM Animal AN INNER JOIN TipoAnimal TA 
                            ON AN.IdTipoAnimal = TA.IdTipoAnimal WHERE IdAnimal =@Id
                    ", new { id});


                _logger.LogInformation("Animal Cargada");

                return animal;
            }
            catch (Exception ex)
            {

                _logger.LogError($"Error Cargando animal {ex.Message}");

                return new Animal();
            }
        }

        [HttpGet("buscarAnimal/{id}")]
        public async Task<IEnumerable<Animal>> BuscarAnimal(string id)
        {
            try
            {
                _logger.LogInformation("Obteniendo Animal por cedula dueno");
                var animal = await _dbConnection.QueryAsync<Animal>(@"
                        SELECT Ani.IdAnimal,Ani.Nombre,Ani.IdTipoAnimal,Ani.Edad,Ani.Raza, Due.Cedula as cedulaDueno FROM Animal Ani
                            INNER JOIN DuenoAnimal Dan ON Ani.IdAnimal = Dan.IdAnimal
                            INNER JOIN Dueno Due ON Dan.IdDueno = Due.IdDueno
                            WHERE Dan.IndetinficadorDueno = @Id
                    ", new { id });


                _logger.LogInformation("Animal Cargada");

              

                return animal;
            }
            catch (Exception ex)
            {

                _logger.LogError($"Error Cargando animal {ex.Message}");

                return new List<Animal> ();
            }
        }

        [HttpPost]

       public async Task<int> InsertarAnimal(Animal entidad)
        {
            int result = 0;
            try
            {
                string query = @"INSERT INTO Animal(Nombre, IdTipoAnimal,Edad ,Raza) VALUES(@Nombre, @IdTipoAnimal,@Edad ,@Raza)
                            DECLARE @idAnimal  int =0
                            SET  @idAnimal =CAST(SCOPE_IDENTITY() as int)

                            INSERT INTO DuenoAnimal([IdAnimal],[IdDueno],[IndetinficadorDueno]) VALUES(@idAnimal,@IdDueno,@cedulaDueno)
                             
                            SELECT  @idAnimal
                            ";
                var parametros = new DynamicParameters();
                parametros.Add("Nombre", entidad.Nombre, DbType.String);
                parametros.Add("IdTipoAnimal", entidad.IdTipoAnimal, DbType.Int32);
                parametros.Add("Edad", entidad.Edad, DbType.Byte);
                parametros.Add("Raza", entidad.Raza, DbType.String);
                parametros.Add("IdDueno", entidad.IdDueno, DbType.Int32);
                parametros.Add("cedulaDueno", entidad.cedulaDueno, DbType.String);

                _logger.LogInformation("Se realiza la ejecución de la inserción");
                // devuelve la cantidad defilas afectadas.
                result =   await _dbConnection.QuerySingleAsync<int>(query, parametros);

                _logger.LogInformation("Ejecución Realizada");

            }
            catch (Exception ex)
            {

                _logger.LogError($"Ocurrió un error en el proceso {ex.Message}");
            }

            return result;
        }


        [HttpPut]
        public async Task ActualizaAnimal(Animal entidad)
        {

            try
            {
                string query = @"UPDATE Animal SET
                                     Nombre =@Nombre
                                    ,IdTipoAnimal =@IdTipoAnimal
                                    ,Edad =@Edad
                                    ,Raza = @Raza
                                     WHERE IdAnimal = @IdAnimal";
                var parametros = new DynamicParameters();
                parametros.Add("Nombre", entidad.Nombre, DbType.String);
                parametros.Add("IdTipoAnimal", entidad.IdTipoAnimal, DbType.Int32);
                parametros.Add("Edad", entidad.Edad, DbType.Byte);
                parametros.Add("Raza", entidad.Raza, DbType.String);
                parametros.Add("IdAnimal", entidad.IdAnimal, DbType.Int32);

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

                string query = "DELETE FROM  Animal WHERE IdAnimal = @IdAnimal ";
                var parametros = new DynamicParameters();
                parametros.Add("IdAnimal", id, DbType.Int32);

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
