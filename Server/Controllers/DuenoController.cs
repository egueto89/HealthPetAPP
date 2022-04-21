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
    public class DuenoController : ControllerBase
    {

        private readonly IDbConnection _dbConnection;

        private readonly ILogger<DuenoController> _logger;

        public DuenoController(ILogger<DuenoController> logger, IDbConnection dbConnection)
        {
            _logger = logger;
            _dbConnection = dbConnection;
        }

        [HttpGet]
        public async Task<IEnumerable<Dueno>> ObtenerTodos()
        {
            try
            {
                _logger.LogInformation("Obteniendo Dueno");
                var duenos = await _dbConnection.QueryAsync<Dueno>(@"
                    SELECT IdDueno,Nombre,Apellidos,Cedula,Telefono,Correo FROM Dueno 
                    ");


                     _logger.LogInformation("Duenos Cargadas");

                return duenos;
            }
            catch (Exception ex)
            {

                _logger.LogError($"Error Cargando duenos {ex.Message}");

                return new List<Dueno>();
            }
        }


        [HttpGet("{id}")]
        public async Task<Dueno> ObtenerEspecifico(int id)
        {
            try
            {
                _logger.LogInformation("Obteniendo Dueno por ID");
                var dueno = await _dbConnection.QuerySingleOrDefaultAsync<Dueno>(@"
                         SELECT IdDueno,Nombre,Apellidos,Cedula,Telefono,Correo FROM Dueno  WHERE IdDueno =@Id
                    ", new { id});


                _logger.LogInformation("Dueno Cargada");

                return dueno;
            }
            catch (Exception ex)
            {

                _logger.LogError($"Error Cargando dueno {ex.Message}");

                return new Dueno();
            }
        }


        [HttpGet("buscarDueno/{id}")]
        public async Task<Dueno> BuscarDueno(string id)
        {
            try
            {
                _logger.LogInformation("Obteniendo Dueno por ID");
                var dueno = await _dbConnection.QuerySingleOrDefaultAsync<Dueno>(@"
                         SELECT IdDueno,Nombre,Apellidos,Cedula,Telefono,Correo FROM Dueno  WHERE Cedula =@Id
                    ", new { id });


                _logger.LogInformation("Dueno Cargada");

                if (dueno ==null)
                {
                    dueno = new Dueno();
                }
                return dueno;
            }
            catch (Exception ex)
            {

                _logger.LogError($"Error Cargando dueno {ex.Message}");

                return new Dueno();
            }
        }



        [HttpPost]

        public async Task<int> InsertarDueno(Dueno entidad)
        {
            int idDueno = 0;
            try
            {
                string query = @"INSERT INTO Dueno(Nombre,Apellidos,Cedula,Telefono,Correo) VALUES(@Nombre,@Apellidos,@Cedula,@Telefono,@Correo) 
                    
                SELECT CAST(SCOPE_IDENTITY() as int)
                    ";
                var parametros = new DynamicParameters();
                parametros.Add("Nombre", entidad.Nombre, DbType.String);
                parametros.Add("Apellidos", entidad.Apellidos, DbType.String);
                parametros.Add("Cedula", entidad.Cedula, DbType.String);
                parametros.Add("Telefono", entidad.Telefono, DbType.String);
                parametros.Add("Correo", entidad.Correo, DbType.String);

                _logger.LogInformation("Se realiza la ejecución de la inserción");
                // devuelve el id del dueno
                idDueno = await _dbConnection.QuerySingleAsync<int>(query, parametros);

                _logger.LogInformation("Ejecución Realizada");

            }
            catch (Exception ex)
            {

                _logger.LogError($"Ocurrió un error en el proceso {ex.Message}");
            }
            return idDueno;
        }


        [HttpPut]
        public async Task ActualizaDueno(Dueno entidad)
        {

            try
            {
                string query = @"UPDATE Dueno SET
                                     Nombre =@Nombre
                                    ,Apellidos =@Apellidos
                                    ,Cedula =@Cedula
                                    ,Telefono = @Telefono
                                    ,Correo = @Correo
                                     WHERE IdDueno = @IdDueno";
                var parametros = new DynamicParameters();
                parametros.Add("Nombre", entidad.Nombre, DbType.String);
                parametros.Add("Apellidos", entidad.Apellidos, DbType.String);
                parametros.Add("Cedula", entidad.Cedula, DbType.String);
                parametros.Add("Telefono", entidad.Telefono, DbType.String);
                parametros.Add("Correo", entidad.Correo, DbType.String);
                parametros.Add("IdDueno", entidad.IdDueno, DbType.Int32);

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

                string query = "DELETE FROM  Dueno WHERE IdDueno = @IdDueno ";
                var parametros = new DynamicParameters();
                parametros.Add("IdDueno", id, DbType.Int32);

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
