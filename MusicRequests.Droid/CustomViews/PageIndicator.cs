﻿
using AndroidX.ViewPager.Widget;

namespace MusicRequests.Droid
{
	public interface PageIndicator : ViewPager.IOnPageChangeListener
	{
		/**
	     * Bind the indicator to a ViewPager.
	     *
	     * @param view
	     */
		void SetViewPager (ViewPager view);

		/**
	     * Bind the indicator to a ViewPager.
	     *
	     * @param view
	     * @param initialPosition
	     */
		void SetViewPager (ViewPager view, int initialPosition);

		/**
	     * <p>Set the current page of both the ViewPager and indicator.</p>
	     *
	     * <p>This <strong>must</strong> be used if you need to set the page before
	     * the views are drawn on screen (e.g., default start page).</p>
	     *
	     * @param item
	     */
		void SetCurrentItem (int item);

		/**
	     * Set a page change listener which will receive forwarded events.
	     *
	     * @param listener
	     */
		void SetOnPageChangeListener (ViewPager.IOnPageChangeListener listener);

		/**
	     * Notify the indicator that the fragment list has changed.
	     */
		void NotifyDataSetChanged ();
	}
}

