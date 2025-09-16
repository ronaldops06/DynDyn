using System.Threading.Tasks;
using Api.Domain.Entities;
using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Repository
{
    public interface IDeviceRepository : IRepository<DeviceEntity>
    {
        /// <summary>
        /// Método responsável por retornar o dispositivo com base na UK.
        /// </summary>
        /// <param name="userId">Identificador do usuário</param>
        /// <param name="physicalDeviceId">Identificador do dispositivo físico</param>
        /// <returns>Entidade de device <see cref="DeviceEntity"/></returns>
        Task<DeviceEntity> SelectByUkAsync(int userId, string physicalDeviceId);
        
        /// <summary>
        /// Método responsável por alterar o estado das entidades dependentes para que não ocorra erro ao salvar a entidade principal.
        /// </summary>
        /// <param name="DeviceEntity">Entidade a ter os dependetes com status alterado.</param>
        void UnchangedParentDevice(DeviceEntity deviceEntity);
    }
}