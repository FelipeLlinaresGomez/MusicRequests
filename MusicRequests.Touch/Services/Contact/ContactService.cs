//using MusicRequests.Core.Services;
//using MusicRequests.Core.Models;
//using System.Text.RegularExpressions;

//namespace MusicRequests.Touch.Services
//{
//    public class ContactService : IContactService
//    {
//        public void AddContactToContactStore(string name, string number)
//        {

//        }

//        public async Task<List<ContactoBizum>> GetAllContacts()
//        {
//            var todosContactos = await Microsoft.Maui.ApplicationModel.Communication.Contacts.GetAllAsync();
//            var result = new List<ContactoBizum>();

//            if (todosContactos != null)
//            {
//                foreach (var contacto in todosContactos)
//                {
//                    if (contacto.Phones != null)
//                    {
//                        foreach (var telefono in contacto.Phones)
//                        {
//                            result.Add(new ContactoBizum
//                            {
//                                Nombre = contacto.GivenName,
//                                Apellidos = contacto.FamilyName,
//                                Telefono = Regex.Replace(telefono.PhoneNumber, @"^(\+)|\D", "$1")
//                            });
//                        }
//                    }
//                }
//            }

//            return result;
//        }
//    }
//}
