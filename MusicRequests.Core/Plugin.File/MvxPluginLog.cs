// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MusicRequests.Core.ViewModels;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.Logging;

namespace MusicRequests.Plugin.File
{
    internal static class MvxPluginLog
    {
        internal static ILogger<BaseViewModel> Instance { get; } = Mvx.IoCProvider.Resolve<ILogger<BaseViewModel>>();
    }
}
