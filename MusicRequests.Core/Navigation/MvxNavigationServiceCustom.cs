using System;
using Microsoft.Extensions.Logging;
using MvvmCross.Core;
using MvvmCross.Exceptions;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.IoC;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Navigation.EventArguments;
using MvvmCross.Presenters.Hints;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using System.Linq;

namespace MusicRequests.Core.ViewModels
{
    public class MvxNavigationServiceCustom : IMvxNavigationServiceCustom
    {
        private readonly IMvxIoCProvider _iocProvider;

        private readonly Lazy<ILogger?> _log = new Lazy<ILogger>(() => MvxLogHost.GetLog<MvxNavigationService>());

        public IMvxViewDispatcher ViewDispatcher { get; }

        protected Lazy<IMvxViewsContainer> ViewsContainer { get; }

        protected Dictionary<Regex, Type> Routes { get; } = new Dictionary<Regex, Type>();


        protected IMvxViewModelLoader ViewModelLoader { get; set; }

        protected ConditionalWeakTable<IMvxViewModel, TaskCompletionSource<object?>> TaskCompletionResults { get; } = new ConditionalWeakTable<IMvxViewModel, TaskCompletionSource<object>>();


        public event EventHandler<IMvxNavigateEventArgs>? WillNavigate;

        public event EventHandler<IMvxNavigateEventArgs>? DidNavigate;

        public event EventHandler<IMvxNavigateEventArgs>? WillClose;

        public event EventHandler<IMvxNavigateEventArgs>? DidClose;

        public event EventHandler<ChangePresentationEventArgs>? WillChangePresentation;

        public event EventHandler<ChangePresentationEventArgs>? DidChangePresentation;

        public MvxNavigationServiceCustom(IMvxViewModelLoader viewModelLoader, IMvxViewDispatcher viewDispatcher, IMvxIoCProvider iocProvider)
        {
            _iocProvider = iocProvider;
            ViewModelLoader = viewModelLoader;
            ViewDispatcher = viewDispatcher;
            ViewsContainer = new Lazy<IMvxViewsContainer>(() => _iocProvider.Resolve<IMvxViewsContainer>());
        }

        public void LoadRoutes(IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException("assemblies");
            }
            Routes.Clear();
            foreach (MvxNavigationAttribute item in assemblies.SelectMany((Assembly a) => a.GetCustomAttributes<MvxNavigationAttribute>()))
            {
                Routes.Add(new Regex(item.UriRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant), item.ViewModelOrFacade);
            }
        }

        protected virtual bool TryGetRoute(string path, out KeyValuePair<Regex, Type> entry)
        {
            string path2 = path;
            ValidateArguments(path2);
            try
            {
                List<KeyValuePair<Regex, Type>> list = Routes.Where<KeyValuePair<Regex, Type>>((KeyValuePair<Regex, Type> t) => t.Key.IsMatch(path2)).ToList();
                switch (list.Count)
                {
                    case 0:
                        entry = default(KeyValuePair<Regex, Type>);
                        _log.Value?.Log(LogLevel.Trace, "Unable to find routing for {path}", path2);
                        return false;
                    case 1:
                        entry = list[0];
                        return true;
                    default:
                        {
                            List<KeyValuePair<Regex, Type>> list2 = list.Where((KeyValuePair<Regex, Type> t) => t.Key.Match(path2).Groups.Count == 1).ToList();
                            if (list2.Count == 1)
                            {
                                entry = list2[0];
                                return true;
                            }
                            _log.Value?.Log(LogLevel.Warning, "The following regular expressions match the provided url ({count}), each RegEx must be unique (otherwise try using IMvxRoutingFacade): {matches}", list.Count - 1, string.Join(", ", list.Select((KeyValuePair<Regex, Type> t) => t.Key.ToString())));
                            entry = default(KeyValuePair<Regex, Type>);
                            return false;
                        }
                }
            }
            catch (Exception exception)
            {
                _log.Value?.Log(LogLevel.Error, exception, "Unable to determine routability");
                entry = default(KeyValuePair<Regex, Type>);
                return false;
            }
        }

        protected virtual IDictionary<string, string> BuildParamDictionary(Regex regex, Match match)
        {
            if (regex == null)
            {
                throw new ArgumentNullException("regex");
            }
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            for (int i = 1; i < match.Groups.Count; i++)
            {
                Group group = match.Groups[i];
                string key = regex.GroupNameFromNumber(i);
                string value = group.Value;
                dictionary.Add(key, value);
            }
            return dictionary;
        }

        protected virtual async Task<MvxViewModelInstanceRequest> NavigationRouteRequest(string path, IMvxBundle? presentationBundle = null)
        {
            ValidateArguments(path);
            if (!TryGetRoute(path, out var entry))
            {
                throw new MvxException("Navigation route request could not be obtained for path: " + path);
            }
            Regex key = entry.Key;
            Match match = key.Match(path);
            IDictionary<string, string> dictionary = BuildParamDictionary(key, match);
            MvxBundle bundle = new MvxBundle(dictionary);
            Type viewModelType = entry.Value;
            MvxViewModelInstanceRequest request = new MvxViewModelInstanceRequest(viewModelType)
            {
                PresentationValues = presentationBundle?.SafeGetData(),
                ParameterValues = bundle.SafeGetData()
            };
            if (viewModelType.GetInterfaces().Contains<Type>(typeof(IMvxNavigationFacade)))
            {
                IMvxNavigationFacade mvxNavigationFacade = (IMvxNavigationFacade)_iocProvider.IoCConstruct(viewModelType);
                try
                {
                    MvxViewModelRequest mvxViewModelRequest = await mvxNavigationFacade.BuildViewModelRequest(path, dictionary).ConfigureAwait(continueOnCapturedContext: false);
                    if (mvxViewModelRequest == null)
                    {
                        throw new MvxException("MvxNavigationService: Facade did not return a valid MvxViewModelRequest.");
                    }
                    request.ViewModelType = mvxViewModelRequest.ViewModelType;
                    if (mvxViewModelRequest.ParameterValues != null)
                    {
                        request.ParameterValues = mvxViewModelRequest.ParameterValues;
                    }
                    MvxViewModelInstanceRequest mvxViewModelInstanceRequest = mvxViewModelRequest as MvxViewModelInstanceRequest;
                    if (mvxViewModelInstanceRequest != null)
                    {
                        request.ViewModelInstance = mvxViewModelInstanceRequest.ViewModelInstance ?? ViewModelLoader.LoadViewModel(request, null);
                        return request;
                    }
                    request.ViewModelInstance = ViewModelLoader.LoadViewModel(request, null);
                    return request;
                }
                catch (Exception exception)
                {
                    throw exception.MvxWrap(string.Format("{0}: Exception thrown while processing URL: {1} with RoutingFacade: {2}", "MvxNavigationService", path, viewModelType));
                }
            }
            request.ViewModelInstance = ViewModelLoader.LoadViewModel(request, null);
            return request;
        }

        protected async Task<MvxViewModelInstanceRequest> NavigationRouteRequest<TParameter>(string path, TParameter param, IMvxBundle? presentationBundle = null) where TParameter : notnull
        {
            ValidateArguments(path, param);
            if (!TryGetRoute(path, out var entry))
            {
                throw new MvxException("Navigation route request could not be obtained for path: " + path);
            }
            Regex key = entry.Key;
            Match match = key.Match(path);
            IDictionary<string, string> dictionary = BuildParamDictionary(key, match);
            MvxBundle bundle = new MvxBundle(dictionary);
            Type viewModelType = entry.Value;
            MvxViewModelInstanceRequest request = new MvxViewModelInstanceRequest(viewModelType)
            {
                PresentationValues = presentationBundle?.SafeGetData(),
                ParameterValues = bundle.SafeGetData()
            };
            if (viewModelType.GetInterfaces().Contains<Type>(typeof(IMvxNavigationFacade)))
            {
                IMvxNavigationFacade mvxNavigationFacade = (IMvxNavigationFacade)_iocProvider.IoCConstruct(viewModelType);
                try
                {
                    MvxViewModelRequest mvxViewModelRequest = await mvxNavigationFacade.BuildViewModelRequest(path, dictionary).ConfigureAwait(continueOnCapturedContext: false);
                    if (mvxViewModelRequest == null)
                    {
                        throw new MvxException("MvxNavigationService: Facade did not return a valid MvxViewModelRequest.");
                    }
                    request.ViewModelType = mvxViewModelRequest.ViewModelType;
                    if (mvxViewModelRequest.ParameterValues != null)
                    {
                        request.ParameterValues = mvxViewModelRequest.ParameterValues;
                    }
                    request.ViewModelInstance = ViewModelLoader.LoadViewModel(request, param, null);
                    return request;
                }
                catch (Exception exception)
                {
                    exception.MvxWrap(string.Format("{0}: Exception thrown while processing URL: {1} with RoutingFacade: {2}", "MvxNavigationService", path, viewModelType));
                    return request;
                }
            }
            request.ViewModelInstance = ViewModelLoader.LoadViewModel(request, param, null);
            return request;
        }

        public virtual Task<bool> CanNavigate(string path)
        {
            KeyValuePair<Regex, Type> entry;
            return Task.FromResult(TryGetRoute(path, out entry));
        }

        public virtual Task<bool> CanNavigate<TViewModel>() where TViewModel : IMvxViewModel
        {
            return Task.FromResult(ViewsContainer.Value.GetViewType(typeof(TViewModel)) != null);
        }

        public virtual Task<bool> CanNavigate(Type viewModelType)
        {
            return Task.FromResult(ViewsContainer.Value.GetViewType(viewModelType) != null);
        }

        protected virtual async Task<bool> Navigate(MvxViewModelRequest request, IMvxViewModel viewModel, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            ValidateArguments(request, viewModel);
            MvxNavigateEventArgs args = new MvxNavigateEventArgs(viewModel, NavigationMode.Show, cancellationToken);
            OnWillNavigate(this, args);
            if (args.Cancel)
            {
                return false;
            }
            if (!(await ViewDispatcher.ShowViewModel(request).ConfigureAwait(continueOnCapturedContext: false)))
            {
                return false;
            }
            if (viewModel.InitializeTask?.Task != null)
            {
                await viewModel.InitializeTask!.Task.ConfigureAwait(continueOnCapturedContext: false);
            }
            OnDidNavigate(this, args);
            return true;
        }

        protected virtual async Task<TResult?> Navigate<TResult>(MvxViewModelRequest request, IMvxViewModelResultCustom<TResult> viewModel, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken)) where TResult : class
        {
            IMvxViewModelResultCustom<TResult> viewModel2 = viewModel;
            ValidateArguments(request, viewModel2);
            bool hasNavigated = false;
            TaskCompletionSource<object?> tcs = new TaskCompletionSource<object>();
            if (cancellationToken != default(CancellationToken))
            {
                cancellationToken.Register(delegate {
                    if (hasNavigated && !tcs.Task.IsCompleted)
                    {
                        Task.Run(() => Close(viewModel2, null, CancellationToken.None), CancellationToken.None);
                    }
                });
            }
            MvxNavigateEventArgs args = new MvxNavigateEventArgs(viewModel2, NavigationMode.Show, cancellationToken);
            OnWillNavigate(this, args);
            viewModel2.CloseCompletionSource = tcs;
            TaskCompletionResults.Add(viewModel2, tcs);
            cancellationToken.ThrowIfCancellationRequested();
            hasNavigated = await ViewDispatcher.ShowViewModel(request).ConfigureAwait(continueOnCapturedContext: false);
            if (!hasNavigated)
            {
                return null;
            }
            if (viewModel2.InitializeTask?.Task != null)
            {
                await viewModel2.InitializeTask!.Task.ConfigureAwait(continueOnCapturedContext: false);
            }
            OnDidNavigate(this, args);
            try
            {
                return (TResult)(await tcs.Task.ConfigureAwait(continueOnCapturedContext: false));
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected virtual async Task<TResult?> Navigate<TParameter, TResult>(MvxViewModelRequest request, IMvxViewModelCustom<TParameter, TResult> viewModel, TParameter param, IMvxBundle? presentationBundle = null, IMvxNavigateEventArgs? args = null, CancellationToken cancellationToken = default(CancellationToken)) where TParameter : notnull where TResult : class
        {
            IMvxViewModelCustom<TParameter, TResult> viewModel2 = viewModel;
            ValidateArguments(request, viewModel2, param);
            bool hasNavigated = false;
            if (cancellationToken != default(CancellationToken))
            {
                cancellationToken.Register(delegate {
                    if (hasNavigated)
                    {
                        Task.Run(() => Close(viewModel2, null, CancellationToken.None), CancellationToken.None);
                    }
                });
            }
            if (args == null)
            {
                args = new MvxNavigateEventArgs(viewModel2, NavigationMode.Show, cancellationToken);
            }
            OnWillNavigate(this, args);
            TaskCompletionSource<object?> tcs = new TaskCompletionSource<object>();
            viewModel2.CloseCompletionSource = tcs;
            TaskCompletionResults.Add(viewModel2, tcs);
            cancellationToken.ThrowIfCancellationRequested();
            hasNavigated = await ViewDispatcher.ShowViewModel(request).ConfigureAwait(continueOnCapturedContext: false);
            if (viewModel2.InitializeTask?.Task != null)
            {
                await viewModel2.InitializeTask!.Task.ConfigureAwait(continueOnCapturedContext: false);
            }
            OnDidNavigate(this, args);
            try
            {
                return (TResult)(await tcs.Task.ConfigureAwait(continueOnCapturedContext: false));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public virtual async Task<bool> Navigate(string path, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            MvxViewModelInstanceRequest mvxViewModelInstanceRequest = await NavigationRouteRequest(path, presentationBundle).ConfigureAwait(continueOnCapturedContext: false);
            if (mvxViewModelInstanceRequest.ViewModelInstance == null)
            {
                _log.Value?.Log(LogLevel.Warning, "Navigation Route Request doesn't have a ViewModelInstance");
                return false;
            }
            return await Navigate(mvxViewModelInstanceRequest, mvxViewModelInstanceRequest.ViewModelInstance, presentationBundle, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        }

        public virtual async Task<bool> Navigate<TParameter>(string path, TParameter param, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken)) where TParameter : notnull
        {
            MvxViewModelInstanceRequest mvxViewModelInstanceRequest = await NavigationRouteRequest(path, param, presentationBundle).ConfigureAwait(continueOnCapturedContext: false);
            if (mvxViewModelInstanceRequest.ViewModelInstance == null)
            {
                _log.Value?.Log(LogLevel.Warning, "Navigation Route Request doesn't have a ViewModelInstance");
                return false;
            }
            return await Navigate(mvxViewModelInstanceRequest, mvxViewModelInstanceRequest.ViewModelInstance, presentationBundle, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        }

        public virtual async Task<TResult?> Navigate<TResult>(string path, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken)) where TResult : class
        {
            MvxViewModelInstanceRequest mvxViewModelInstanceRequest = await NavigationRouteRequest(path, presentationBundle).ConfigureAwait(continueOnCapturedContext: false);
            if (mvxViewModelInstanceRequest.ViewModelInstance == null)
            {
                _log.Value?.Log(LogLevel.Warning, "Navigation Route Request doesn't have a ViewModelInstance");
                return null;
            }
            return await Navigate(mvxViewModelInstanceRequest, (IMvxViewModelResultCustom<TResult>)mvxViewModelInstanceRequest.ViewModelInstance, presentationBundle, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        }

        public virtual async Task<TResult?> Navigate<TParameter, TResult>(string path, TParameter param, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken)) where TParameter : notnull where TResult : class
        {
            MvxViewModelInstanceRequest mvxViewModelInstanceRequest = await NavigationRouteRequest(path, param, presentationBundle).ConfigureAwait(continueOnCapturedContext: false);
            if (mvxViewModelInstanceRequest.ViewModelInstance == null)
            {
                _log.Value?.Log(LogLevel.Warning, "Navigation Route Request doesn't have a ViewModelInstance");
                return null;
            }
            return await Navigate(mvxViewModelInstanceRequest, (IMvxViewModelCustom<TParameter, TResult>)mvxViewModelInstanceRequest.ViewModelInstance, param, presentationBundle, null, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        }

        public virtual Task<bool> Navigate(Type viewModelType, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            MvxViewModelInstanceRequest mvxViewModelInstanceRequest = new MvxViewModelInstanceRequest(viewModelType)
            {
                PresentationValues = presentationBundle?.SafeGetData()
            };
            mvxViewModelInstanceRequest.ViewModelInstance = ViewModelLoader.LoadViewModel(mvxViewModelInstanceRequest, null);
            return Navigate(mvxViewModelInstanceRequest, mvxViewModelInstanceRequest.ViewModelInstance, presentationBundle, cancellationToken);
        }

        public virtual Task<bool> Navigate<TParameter>(Type viewModelType, TParameter param, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken)) where TParameter : notnull
        {
            MvxViewModelInstanceRequest mvxViewModelInstanceRequest = new MvxViewModelInstanceRequest(viewModelType)
            {
                PresentationValues = presentationBundle?.SafeGetData()
            };
            mvxViewModelInstanceRequest.ViewModelInstance = ViewModelLoader.LoadViewModel(mvxViewModelInstanceRequest, param, null);
            return Navigate(mvxViewModelInstanceRequest, mvxViewModelInstanceRequest.ViewModelInstance, presentationBundle, cancellationToken);
        }

        public virtual Task<TResult?> Navigate<TResult>(Type viewModelType, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken)) where TResult : class
        {
            MvxViewModelInstanceRequest mvxViewModelInstanceRequest = new MvxViewModelInstanceRequest(viewModelType)
            {
                PresentationValues = presentationBundle?.SafeGetData()
            };
            mvxViewModelInstanceRequest.ViewModelInstance = (IMvxViewModelResultCustom<TResult>)ViewModelLoader.LoadViewModel(mvxViewModelInstanceRequest, null);
            return Navigate(mvxViewModelInstanceRequest, (IMvxViewModelResultCustom<TResult>)mvxViewModelInstanceRequest.ViewModelInstance, presentationBundle, cancellationToken);
        }

        public virtual Task<TResult?> Navigate<TParameter, TResult>(Type viewModelType, TParameter param, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken)) where TParameter : notnull where TResult : class
        {
            MvxNavigateEventArgs mvxNavigateEventArgs = new MvxNavigateEventArgs(NavigationMode.Show, cancellationToken);
            MvxViewModelInstanceRequest mvxViewModelInstanceRequest = new MvxViewModelInstanceRequest(viewModelType)
            {
                PresentationValues = presentationBundle?.SafeGetData()
            };
            mvxViewModelInstanceRequest.ViewModelInstance = (IMvxViewModelCustom<TParameter, TResult>)ViewModelLoader.LoadViewModel(mvxViewModelInstanceRequest, param, null);
            mvxNavigateEventArgs.ViewModel = mvxViewModelInstanceRequest.ViewModelInstance;
            return Navigate(mvxViewModelInstanceRequest, (IMvxViewModelCustom<TParameter, TResult>)mvxViewModelInstanceRequest.ViewModelInstance, param, presentationBundle, mvxNavigateEventArgs, cancellationToken);
        }

        public virtual Task<bool> Navigate<TViewModel>(IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken)) where TViewModel : IMvxViewModel
        {
            return Navigate(typeof(TViewModel), presentationBundle, cancellationToken);
        }

        public virtual Task<bool> Navigate<TViewModel, TParameter>(TParameter param, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken)) where TViewModel : IMvxViewModel<TParameter> where TParameter : notnull
        {
            return Navigate(typeof(TViewModel), param, presentationBundle, cancellationToken);
        }

        public virtual Task<TResult?> Navigate<TViewModel, TResult>(IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken)) where TViewModel : IMvxViewModelResultCustom<TResult> where TResult : class
        {
            return Navigate<TResult>(typeof(TViewModel), presentationBundle, cancellationToken);
        }

        public virtual Task<TResult?> Navigate<TViewModel, TParameter, TResult>(TParameter param, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken)) where TViewModel : IMvxViewModelCustom<TParameter, TResult> where TParameter : notnull where TResult : class
        {
            return Navigate<TParameter, TResult>(typeof(TViewModel), param, presentationBundle, cancellationToken);
        }

        public virtual Task<bool> Navigate(IMvxViewModel viewModel, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            MvxViewModelInstanceRequest request = new MvxViewModelInstanceRequest(viewModel)
            {
                PresentationValues = presentationBundle?.SafeGetData()
            };
            ViewModelLoader.ReloadViewModel(viewModel, request, null);
            return Navigate(request, viewModel, presentationBundle, cancellationToken);
        }

        public virtual Task<bool> Navigate<TParameter>(IMvxViewModel<TParameter> viewModel, TParameter param, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken)) where TParameter : notnull
        {
            MvxViewModelInstanceRequest request = new MvxViewModelInstanceRequest(viewModel)
            {
                PresentationValues = presentationBundle?.SafeGetData()
            };
            ViewModelLoader.ReloadViewModel(viewModel, param, request, null);
            return Navigate(request, viewModel, presentationBundle, cancellationToken);
        }

        public virtual Task<TResult?> Navigate<TResult>(IMvxViewModelResultCustom<TResult> viewModel, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken)) where TResult : class
        {
            MvxViewModelInstanceRequest request = new MvxViewModelInstanceRequest(viewModel)
            {
                PresentationValues = presentationBundle?.SafeGetData()
            };
            ViewModelLoader.ReloadViewModel(viewModel, request, null);
            return Navigate(request, viewModel, presentationBundle, cancellationToken);
        }

        public virtual Task<TResult?> Navigate<TParameter, TResult>(IMvxViewModelCustom<TParameter, TResult> viewModel, TParameter param, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken)) where TParameter : notnull where TResult : class
        {
            MvxNavigateEventArgs args = new MvxNavigateEventArgs(viewModel, NavigationMode.Show, cancellationToken);
            MvxViewModelInstanceRequest request = new MvxViewModelInstanceRequest(viewModel)
            {
                PresentationValues = presentationBundle?.SafeGetData()
            };
            ViewModelLoader.ReloadViewModel(viewModel, param, request, null);
            return Navigate(request, viewModel, param, presentationBundle, args, cancellationToken);
        }

        public virtual async Task<bool> ChangePresentation(MvxPresentationHint hint, CancellationToken cancellationToken = default(CancellationToken))
        {
            ValidateArguments(hint);
            _log.Value?.Log(LogLevel.Trace, "Requesting presentation change");
            ChangePresentationEventArgs args = new ChangePresentationEventArgs(hint, cancellationToken);
            OnWillChangePresentation(this, args);
            if (args.Cancel)
            {
                return false;
            }
            bool flag = await ViewDispatcher.ChangePresentation(hint).ConfigureAwait(continueOnCapturedContext: false);
            args.Result = flag;
            OnDidChangePresentation(this, args);
            return flag;
        }

        public virtual async Task<bool> Close(IMvxViewModel viewModel, CancellationToken cancellationToken = default(CancellationToken))
        {
            ValidateArguments(viewModel);
            MvxNavigateEventArgs args = new MvxNavigateEventArgs(viewModel, NavigationMode.Close, cancellationToken);
            OnWillClose(this, args);
            if (args.Cancel)
            {
                return false;
            }
            bool result = await ViewDispatcher.ChangePresentation(new MvxClosePresentationHint(viewModel)).ConfigureAwait(continueOnCapturedContext: false);
            OnDidClose(this, args);
            return result;
        }

        public virtual async Task<bool> Close<TResult>(IMvxViewModelResultCustom<TResult> viewModel, TResult? result, CancellationToken cancellationToken = default(CancellationToken)) where TResult : class
        {
            ValidateArguments(viewModel);
            TaskCompletionResults.TryGetValue(viewModel, out var tcs);
            viewModel.CloseCompletionSource = null;
            try
            {
                bool flag = await Close(viewModel, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                if (flag)
                {
                    tcs?.TrySetResult(result);
                    TaskCompletionResults.Remove(viewModel);
                }
                else
                {
                    viewModel.CloseCompletionSource = tcs;
                }
                return flag;
            }
            catch (Exception exception)
            {
                tcs?.TrySetException(exception);
                return false;
            }
        }

        protected virtual void OnWillNavigate(object sender, IMvxNavigateEventArgs e)
        {
            this.WillNavigate?.Invoke(sender, e);
        }

        protected virtual void OnDidNavigate(object sender, IMvxNavigateEventArgs e)
        {
            this.DidNavigate?.Invoke(sender, e);
        }

        protected virtual void OnWillClose(object sender, IMvxNavigateEventArgs e)
        {
            this.WillClose?.Invoke(sender, e);
        }

        protected virtual void OnDidClose(object sender, IMvxNavigateEventArgs e)
        {
            this.DidClose?.Invoke(sender, e);
        }

        protected virtual void OnWillChangePresentation(object sender, ChangePresentationEventArgs e)
        {
            this.WillChangePresentation?.Invoke(sender, e);
        }

        protected virtual void OnDidChangePresentation(object sender, ChangePresentationEventArgs e)
        {
            this.DidChangePresentation?.Invoke(sender, e);
        }

        private static void ValidateArguments<TParameter>(MvxViewModelRequest request, IMvxViewModel viewModel, TParameter param) where TParameter : notnull
        {
            ValidateArguments(request, viewModel);
            if (param == null)
            {
                throw new ArgumentNullException("param");
            }
        }

        private static void ValidateArguments(MvxViewModelRequest request, IMvxViewModel viewModel)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            if (viewModel == null)
            {
                throw new ArgumentNullException("viewModel");
            }
        }

        private static void ValidateArguments<TParameter>(string path, TParameter param) where TParameter : notnull
        {
            ValidateArguments(path);
            if (param == null)
            {
                throw new ArgumentNullException("param");
            }
        }

        private static void ValidateArguments(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }
        }

        private static void ValidateArguments(MvxPresentationHint hint)
        {
            if (hint == null)
            {
                throw new ArgumentNullException("hint");
            }
        }

        private static void ValidateArguments(IMvxViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException("viewModel");
            }
        }

        private static void ValidateArguments<TResult>(IMvxViewModelResultCustom<TResult> viewModel) where TResult : notnull
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException("viewModel");
            }
        }
    }
}
