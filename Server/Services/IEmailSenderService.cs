using HealthPetAPP.Shared;
using System.Threading.Tasks;

namespace HealthPetAPP.Server.Services
{
    public interface IEmailSenderService
    {
        Task<int> SendEmailAsync(DatosCorreo datosCorreo);
    }
}
