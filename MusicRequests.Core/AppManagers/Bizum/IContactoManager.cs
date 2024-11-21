using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicRequests.Core.Managers
{
	public interface IContactoManager
    {
		/// <summary>
		/// Resetea el proceso de envio de dinero
		/// </summary>
		void CleanCache();

		///// <summary>
		///// Recupera los contactos de un usuario
		///// </summary>
		///// <returns>Conctactos de la agenda</returns>
		//Task<List<ContactoItemViewModel>> GetAllContacts();

		///// <summary>
		///// Recupera los usuarios frecuentes con los que se realizan transacciones
		///// </summary>
		///// <returns>Contactos frecuentes.</returns>
		//Task<List<ContactoItemViewModel>> GetContactosFrecuentes();

		///// <summary>
		///// Anadimos contacto a usuarios frecuentes
		///// </summary>
		//Task AddContactoFrecuente(ContactoItemViewModel contacto);
    }
}
