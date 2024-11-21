using MusicRequests.Core.Services;
using MusicRequests.Touch.Styles;
using MusicRequests.Core.Helpers;
using MusicRequests.Core;
using Microsoft.Maui.Devices;

namespace MusicRequests.Touch.Services
{

    public class DeviceService //: IDeviceService
    {
        //public string GetIdentifier()
        //{
        //    if (string.IsNullOrEmpty(Settings.IMEI))
        //    {
        //        var nsUid = UIDevice.CurrentDevice.IdentifierForVendor;
        //        var id = nsUid.AsString();
        //        Settings.IMEI = id;
        //    }
        //    return Settings.IMEI;
        //}

        //public PhoneModel GetPhoneInfo()
        //{
        //    var phoneModel = new PhoneModel()
        //    {
        //        Manufacturer = DeviceInfo.Manufacturer,
        //        Model = DeviceInfo.Model
        //    };

        //    if (phoneModel.Model.ToLower().Contains("iphone"))
        //    {
        //        phoneModel.Type = "Movil";
        //    }
        //    else
        //    {
        //        phoneModel.Type = "Tablet";
        //    }
        //    phoneModel.DeviceOSVersion = DeviceInfo.Version;
        //    phoneModel.DeviceOSVersionString = DeviceInfo.VersionString;

        //    return phoneModel;
        //}

        //public string GetPhoneNumberIdentifier()
        //{
        //    return App.User?.Telefono.Replace(" ", "");
        //}

        //public ScreenSize GetScreenSize()
        //{
        //    return new ScreenSize()
        //    {
        //        Width = Dimensions.WidthWithoutMargins,
        //        Height = Dimensions.HeightWindow,
        //        Density = (double)UIScreen.MainScreen.Scale
        //    };
        //}
    }
}