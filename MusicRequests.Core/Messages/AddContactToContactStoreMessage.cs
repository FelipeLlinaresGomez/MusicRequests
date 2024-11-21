using MusicRequests.Core.ViewModels;
using MvvmCross.Plugin.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicRequests.Core.Messages
{
    public class AddContactToContactStoreMessage : MvxMessage
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }

        public AddContactToContactStoreMessage(object sender, string name, string phoneNumber) : base(sender)
        {
            Name = name;
            PhoneNumber = phoneNumber;
        }
    }
}
