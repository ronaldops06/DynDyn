using System.Threading.Tasks;
using Domain.Models;

namespace Api.Domain.Interfaces.Services
{
    /// <summary>
    /// Interface de serviço dos métodos específicos de dispositivo.
    /// </summary>
    public interface IDeviceService : IService<DeviceModel>
    {
        /// <summary>
        /// Método responsável por retornar o dispositivo com base no identificador.
        /// </summary>
        /// <param name="id">Identificador do registro.</param>
        /// <returns>Modelo do dispositivo encontrado <see cref="DeviceModel"/>.</returns>
        Task<DeviceModel> GetById(int id);

        /// <summary>
        /// Método responsável por salvar (inserir/atualizar) o dispositivo.
        /// </summary>
        /// <param name="deviceModel">Modelo do dispositivo a ser salvo <see cref="DeviceModel"/></param>
        /// <returns>Modelo do dispositivo salvo <see cref="DeviceModel"/>.</returns>
        Task<DeviceModel> ExecuteSaveDevice(DeviceModel deviceModel);
    }
}