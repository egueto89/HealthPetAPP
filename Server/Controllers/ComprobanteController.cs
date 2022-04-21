using HealthPetAPP.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using HealthPetAPP.Server.Services;

namespace HealthPetAPP.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ComprobanteController : ControllerBase
    {

        private readonly IDbConnection _dbConnection;

        private readonly ILogger<ComprobanteController> _logger;
        private readonly IEmailSenderService _emailSenderService;
        public ComprobanteController(ILogger<ComprobanteController> logger, IDbConnection dbConnection, IEmailSenderService emailSenderService)
        {
            _logger = logger;
            _dbConnection = dbConnection;
            _emailSenderService = emailSenderService;
        }

        [HttpGet("{idCita}")]
        public async Task<DatosComprobante> ObtenerDatosComprobante(int idCita)
        {
            try
            {
                _logger.LogInformation("Obteniendo Cita por ID");
                var datosComprobante = await _dbConnection.QuerySingleOrDefaultAsync<DatosComprobante>(@"
                       SELECT CONCAT(DU.Nombre,' ', DU.Apellidos) AS NombreDueno, DU.Correo AS CorreoDueno, ANI.Nombre AS NombreMascota , CI.Fecha FROM 
                                Cita CI 
                                INNER JOIN Dueno DU  ON CI.IdDueno = DU.IdDueno
                                INNER JOIN Animal ANI ON CI.IdAnimal = ANI.IdAnimal
                                WHERE CI.IdCita=@idCita
                    ", new { idCita});


                _logger.LogInformation("Cita Cargada");

                return datosComprobante;
            }
            catch (Exception ex)
            {

                _logger.LogError($"Error Cargando cita {ex.Message}");

                return new DatosComprobante();
            }
        }

        [HttpPost]

       public async Task<int> EnvioCorreo(DatosCorreo entidad)
        {
            int resgistros = 1;
            try
            {
                resgistros =  await _emailSenderService.SendEmailAsync(entidad);
                _logger.LogInformation("Ejecución Realizada");

            }
            catch (Exception ex)
            {

                _logger.LogError($"Ocurrió un error en el proceso {ex.Message}");
            }

            return resgistros;
        }

    }
}
