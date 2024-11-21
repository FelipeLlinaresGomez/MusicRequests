using System;
using MusicRequests.Core.ViewModels;
using MvvmCross.Presenters.Hints;

namespace MusicRequests.Core.Presenters
{
    public class LogOutPresenterHint : MvxPopToRootPresentationHint
    {
		public Type ViewModelType { get; set; }
		public LogOutPresenterHint (Type type)
		{
			ViewModelType = type;	
		}
	}
	public class LogOutPresenterHint<TViewModel> : LogOutPresenterHint where TViewModel : IBaseViewModel
	{
		public LogOutPresenterHint () : base(typeof(TViewModel)) { }
	}
}

