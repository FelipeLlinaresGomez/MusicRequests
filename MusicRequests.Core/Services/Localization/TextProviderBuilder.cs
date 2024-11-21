using MusicRequests.Core.AppManagers.Base;
using MusicRequests.Core.Helpers;
using MusicRequests.Core.Services.Base;
using MusicRequests.Core.ViewModels;
using MvvmCross.Plugin.JsonLocalization;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MusicRequests.Core.Services
{
    public class TextProviderBuilder : MvxTextProviderBuilder
    {
        public TextProviderBuilder() : base(Constants.GeneralNamespace, Constants.RootFolderForResources)
        {
        }

        protected override IDictionary<string, string> ResourceFiles
        {
            get
            {
                var result = new List<TypeInfo>();

                var assemblyTypes = this.GetType().GetTypeInfo()
                    .Assembly
                    .DefinedTypes;

                var localizedViewModels = assemblyTypes
                    .Where(t => t.Name.EndsWith("ViewModel"));
                result.AddRange(localizedViewModels);

                var localizableManagers = assemblyTypes
                    .Where(t => t.Name.EndsWith("Manager") && t.IsAssignableFrom(typeof(BaseLocalizableManager)));
                result.AddRange(localizableManagers);

                var localizableServices = assemblyTypes
                    .Where(t => t.Name.EndsWith("Service") && t.IsAssignableFrom(typeof(BaseLocalizableService)));
                result.AddRange(localizableServices);

                // Ojo, si hubieran duplicados en alguno de los 3 listados (que no deberia) estamos ante un problema
                var dic = result.ToDictionary(t => t.Name, t => t.Name);
                dic[Constants.Shared] = Constants.Shared;

                return dic;
            }
        }
    }
}

