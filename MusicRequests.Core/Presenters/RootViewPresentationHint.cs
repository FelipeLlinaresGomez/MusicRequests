using System;
using MvvmCross.ViewModels;
using MusicRequests.Core.ViewModels;

namespace MusicRequests.Core.Presenters
{
	public class RootViewPresentationHint : MvxPresentationHint
	{
		public Type ViewModelType { get; set; }
		public bool Animated { get; set; }
		public bool NavigateBack { get; set; }

		public RootViewPresentationHint (Type type, bool animated, bool navigateBack = false)
		{
			ViewModelType = type;
			Animated = animated;
			NavigateBack = navigateBack;
		}
	}

	public class RootViewPresentationHint<TViewModel> : RootViewPresentationHint where TViewModel : IBaseViewModel
	{
		public RootViewPresentationHint (bool animated = true, bool navigateBack = false) : base(typeof(TViewModel), animated, navigateBack)
		{
			
		}
	}
}

