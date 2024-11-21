using System;
namespace MusicRequests.Droid
{
	public interface ISoftKeyboardAwareFragment
	{
		void OnKeyboardShown();
		void OnKeyboardHidden();
	}
}

