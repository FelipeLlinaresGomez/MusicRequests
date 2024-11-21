//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Android.App;
//using Android.Content;
//using Android.Database;
//using Android.Provider;
//using MusicRequests.Core.Models;
//using MusicRequests.Core.Services;
//using MvvmCross;
//using MvvmCross.Platforms.Android;

//namespace MusicRequests.Droid.Services
//{
//    public class ContactService : IContactService
//    {
//        public void AddContactToContactStore(string name, string number)
//        {
//            var intent = new Intent(Intent.ActionInsert);
//            intent.SetType(ContactsContract.Contacts.ContentType);
//            intent.PutExtra(ContactsContract.Intents.Insert.Name, name);
//            intent.PutExtra(ContactsContract.Intents.Insert.Phone, number);
//            Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity.StartActivity(intent);
//        }

//        public async Task<List<ContactoBizum>> GetAllContacts()
//        {
//            List<ContactoBizum> contactData = new List<ContactoBizum>();

//            await Task.Run(() =>
//            {
//                ICursor cursor = null;
//                try
//                {
//                    cursor = Application.Context.ContentResolver.Query(ContactsContract.Contacts.ContentUri,
//                        null,
//                        null, null, null);
//                    while (cursor != null && cursor.MoveToNext())
//                    {
//                        try
//                        {
//                            string contactId = cursor.GetString(cursor.GetColumnIndex(ContactsContract.CommonDataKinds.Callable.InterfaceConsts.Id));
//                            string name = cursor.GetString(cursor.GetColumnIndex(ContactsContract.CommonDataKinds.Callable.InterfaceConsts.DisplayName));
//                            string fullName = cursor.GetString(cursor.GetColumnIndex(ContactsContract.CommonDataKinds.Callable.InterfaceConsts.DisplayNameAlternative));
//                            string subname = "";

//                            if (!string.IsNullOrEmpty(fullName))
//                            {
//                                var names = fullName.Split(',');
//                                if (names.Length == 2)
//                                {
//                                    name = names[1].Substring(1);
//                                    subname = names[0];
//                                }
//                            }

//                            string hasPhone = cursor.GetString(cursor.GetColumnIndex(ContactsContract.CommonDataKinds.Callable.InterfaceConsts.HasPhoneNumber));
//                            if (int.Parse(hasPhone) > 0)
//                            {
//                                ICursor phones = null;
//                                try
//                                {
//                                    phones = Application.Context.ContentResolver.Query(ContactsContract.CommonDataKinds.Phone.ContentUri, null, ContactsContract.CommonDataKinds.Callable.InterfaceConsts.ContactId + " = " + contactId, null, null);
//                                    while (phones != null && phones.MoveToNext())
//                                    {
//                                        string phoneNumber = phones.GetString(phones.GetColumnIndex(ContactsContract.CommonDataKinds.Phone.Number));
//                                        ContactoBizum cotactoBizum = new ContactoBizum { Telefono = phoneNumber, Nombre = name, Apellidos = subname };
//                                        // Avoid duplicates
//                                        if (contactData.FirstOrDefault(x => x.Telefono.Equals(phoneNumber)) == null)
//                                        {
//                                            contactData.Add(cotactoBizum);
//                                        }
//                                    }
//                                }
//                                finally
//                                {
//                                    phones?.Close();
//                                }
//                            }
//                        }
//                        catch (Exception)
//                        {
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    System.Diagnostics.Debug.WriteLine(ex);
//                }
//                finally
//                {
//                    cursor?.Close();
//                }
//            });

//            return contactData;
//        }


//    }
//}