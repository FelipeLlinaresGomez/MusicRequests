using MusicRequests.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Foundation;
using UIKit;
using Contacts;
using ContactsUI;

namespace MusicRequests.Touch.Services
{
    public class ShareService : IShareService
    {
		public void ShareContactoEnlace(string name, string number)
		{
			var contact = new CNMutableContact();

			// Set standard properties
			contact.GivenName = name;
			var workPhone = new CNLabeledValue<CNPhoneNumber>(CNLabelPhoneNumberKey.iPhone, new CNPhoneNumber(number));
			contact.PhoneNumbers = new CNLabeledValue<CNPhoneNumber>[] { workPhone};

			// Save new contact
			var store = new CNContactStore();
			var saveRequest = new CNSaveRequest();
			saveRequest.AddContact(contact, store.DefaultContainerIdentifier);

			NSError error;
			if (store.ExecuteSaveRequest(saveRequest, out error))
			{
				Console.WriteLine("New contact saved");
				UIAlertController confirmAlert = new UIAlertController();
				confirmAlert.Title = "Contacto";
				confirmAlert.Message = string.Format("Nuevo contacto {0} {1} agragado correctamente", name, number);
				confirmAlert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
				UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(confirmAlert, true, null);
			}
			else {
				Console.WriteLine("Save error: {0}", error);
			}
        }
    }

}
