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
    public class CitaController : ControllerBase
    {

        private readonly IDbConnection _dbConnection;

        private readonly ILogger<CitaController> _logger;

        public CitaController(ILogger<CitaController> logger, IDbConnection dbConnection)
        {
            _logger = logger;
            _dbConnection = dbConnection;
        }

        [HttpGet]
        public async Task<IEnumerable<Cita>> ObtenerTodos()
        {
            try
            {
                _logger.LogInformation("Obteniendo Cita");
                var citas = await _dbConnection.QueryAsync<Cita>(@"
                         SELECT IdCita,Du.IdDueno,CA.IdCategoria
                            ,Hora,Fecha
                            ,Estado, CONCAT(DU.Nombre,' ', DU.Apellidos) AS NombreDueno
                            ,CA.Descripcion AS DesCategoria,Ani.IdAnimal , DU.Cedula AS CedulaDueno
                        FROM Cita CI 
                        INNER JOIN Dueno Du ON CI.IdDueno = DU.IdDueno
                        INNER JOIN Categoria CA ON CI.IdCategoria = CA.IdCategoria 
                        INNER JOIN Animal Ani ON CI.IdAnimal = Ani.IdAnimal 
                    ");


                     _logger.LogInformation("Citas Cargadas");

                return citas;
            }
            catch (Exception ex)
            {

                _logger.LogError($"Error Cargando citas {ex.Message}");

                return new List<Cita>();
            }
        }


        [HttpGet("{id}")]
        public async Task<Cita> ObtenerEspecifico(int id)
        {
            try
            {
                _logger.LogInformation("Obteniendo Cita por ID");
                var cita = await _dbConnection.QuerySingleOrDefaultAsync<Cita>(@"
                       SELECT IdCita,Du.IdDueno,CA.IdCategoria
                            ,Hora,Fecha
                            ,Estado, CONCAT(DU.Nombre,' ', DU.Apellidos) AS NombreDueno
                            ,CA.Descripcion AS DesCategoria,Ani.IdAnimal , DU.Cedula AS CedulaDueno
                        FROM Cita CI 
                        INNER JOIN Dueno Du ON CI.IdDueno = DU.IdDueno
                        INNER JOIN Categoria CA ON CI.IdCategoria = CA.IdCategoria 
                        INNER JOIN Animal Ani ON CI.IdAnimal = Ani.IdAnimal 
                        WHERE IdCita =@Id
                    ", new { id});


                _logger.LogInformation("Cita Cargada");

                return cita;
            }
            catch (Exception ex)
            {

                _logger.LogError($"Error Cargando cita {ex.Message}");

                return new Cita();
            }
        }

        [HttpGet("busquedaCita/{cedula}/{fecha}")]
        public async Task<IEnumerable<Cita>>  BusquedaCita(string cedula, string fecha)
        {
            try
            {
                var fechaParse = DateTime.ParseExact(fecha, "yyyyMMdd",
                                       System.Globalization.CultureInfo.InvariantCulture);

                _logger.LogInformation("Obteniendo Cita por ID");
                var parametros = new DynamicParameters();
                parametros.Add("Cedula", cedula, DbType.String);
                parametros.Add("Fecha", fechaParse, DbType.DateTime);

                var cita = await _dbConnection.QueryAsync<Cita>(@"
                        SELECT IdCita,Du.IdDueno,CA.IdCategoria
                            ,Hora,Fecha
                            ,Estado, CONCAT(DU.Nombre,' ', DU.Apellidos) AS NombreDueno
                            ,CA.Descripcion AS DesCategoria,Ani.IdAnimal, DU.Cedula AS CedulaDueno
                        FROM Cita CI 
                        INNER JOIN Dueno Du ON CI.IdDueno = DU.IdDueno
                        INNER JOIN Categoria CA ON CI.IdCategoria = CA.IdCategoria 
                        INNER JOIN Animal Ani ON CI.IdAnimal = Ani.IdAnimal 
                        WHERE DU.Cedula =@Cedula AND CONVERT(DATE, CI.Fecha) =@Fecha
                        AND CI.Estado in('Agendada', 'Reagendada')
                    ", parametros);


                _logger.LogInformation("Cita buscada");

                return cita;
            }
            catch (Exception ex)
            {

                _logger.LogError($"Error Cargando cita {ex.Message}");

                return new List<Cita>();
            }
        }


        [HttpGet("fechaHoraDisponible/{hora}/{fecha}/{idDueno}/{idCita}")]
        public async Task<int> FechaHoraDisponible(string hora, string fecha, int idDueno, int idCita)
        {
            int fechadisponible = 1;
            try
            {
                var fechaParse = DateTime.ParseExact(fecha, "yyyyMMdd",
                                       System.Globalization.CultureInfo.InvariantCulture);

                _logger.LogInformation("Verifica hora de cita disponible");
                var parametros = new DynamicParameters();
                parametros.Add("Hora", hora, DbType.String);
                parametros.Add("Fecha", fechaParse, DbType.DateTime);
                parametros.Add("IdDueno", idDueno, DbType.Int32);
                parametros.Add("IdCita", idCita, DbType.Int32);
                

                fechadisponible = await _dbConnection.QuerySingleAsync<int>(@"
                        SELECT COUNT(1) FROM Cita WHERE CONVERT(DATE,Fecha) = @Fecha AND Hora = @Hora AND (IdDueno !=IdDueno OR IdCita!=@IdCita)
                    ", parametros);

                

                _logger.LogInformation("Cita buscada");

                return fechadisponible;
            }
            catch (Exception ex)
            {

                _logger.LogError($"Error Cargando cita {ex.Message}");

                return fechadisponible;
            }
        }


        [HttpPost]

       public async Task<int> InsertarCita(Cita entidad)
        {
            int resgistros = 0;
            try
            {
                entidad.Estado = "Agendada";
                string query = @"INSERT INTO Cita(IdDueno,IdCategoria,Hora,Fecha,Estado,IdAnimal) VALUES(@IdDueno,@IdCategoria,@Hora,@Fecha,@Estado,@IdAnimal)
                            
                          SELECT CAST(SCOPE_IDENTITY() as int) 
                        ";
                var parametros = new DynamicParameters();
                parametros.Add("IdDueno", entidad.IdDueno, DbType.Int32);
                parametros.Add("IdAnimal", entidad.IdAnimal, DbType.Int32);
                parametros.Add("IdCategoria", entidad.IdCategoria, DbType.Int32);
                parametros.Add("Hora", entidad.Hora, DbType.String);
                parametros.Add("Fecha", entidad.Fecha, DbType.DateTime);
                parametros.Add("Estado", entidad.Estado, DbType.String);
                _logger.LogInformation("Se realiza la ejecución de la inserción");
                // devuelve la cantidad defilas afectadas.
                resgistros =   await _dbConnection.QuerySingleAsync<int>(query, parametros);

                _logger.LogInformation("Ejecución Realizada");

            }
            catch (Exception ex)
            {

                _logger.LogError($"Ocurrió un error en el proceso {ex.Message}");
            }

            return resgistros;
        }


        [HttpPut]
        public async Task ActualizaCita(Cita entidad)
        {

            try
            {
                //validamos si la cita se va a reagendar

                if (entidad.Fecha == entidad.FechaAnterior && entidad.Hora != entidad.HoraAnterior)
                {
                    entidad.Estado = "Reagendada";
                }


                string query = @"UPDATE Cita SET
                                     IdDueno=@IdDueno
                                    ,IdCategoria =@IdCategoria
                                    ,Hora = @Hora
                                    ,Fecha =@Fecha
                                    ,Estado =@Estado
                                    ,IdAnimal =@IdAnimal
                                     WHERE IdCita = @IdCita";
                var parametros = new DynamicParameters();
                parametros.Add("IdDueno", entidad.IdDueno, DbType.Int32);
                parametros.Add("IdAnimal", entidad.IdAnimal, DbType.Int32);
                parametros.Add("IdCategoria", entidad.IdCategoria, DbType.Int32);
                parametros.Add("Hora", entidad.Hora, DbType.String);
                parametros.Add("Fecha", entidad.Fecha, DbType.DateTime);
                parametros.Add("Estado", entidad.Estado, DbType.String);
                parametros.Add("IdCita", entidad.IdCita, DbType.Int32);

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

        [HttpPut("cancelarCita")]
        public async Task CancelarCita(Cita entidad)
        {

            try
            {
                string query = @"UPDATE Cita SET
                                     Estado =@Estado
                                     WHERE IdCita = @IdCita";
                var parametros = new DynamicParameters();
                parametros.Add("Estado", "Cancelada", DbType.String);
                parametros.Add("IdCita", entidad.IdCita, DbType.Int32);

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

                string query = "DELETE FROM  Cita WHERE IdCita = @IdCita ";
                var parametros = new DynamicParameters();
                parametros.Add("IdCita", id, DbType.Int32);

                _logger.LogInformation("Se realiza la ejecución de la eliminación");
                // devuelve la cantidad de filas afectadas.
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
