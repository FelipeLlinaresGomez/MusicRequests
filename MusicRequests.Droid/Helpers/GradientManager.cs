using System;
using Android.Content;
using Android.Graphics;

namespace MusicRequests.Droid
{
	public class GradientManager
	{
		private int _height;

		public GradientManager(int height)
		{
			_height = height;
		}

		public SweepGradient GetSweepGradient()
		{
			SweepGradient gradient;

				gradient = new SweepGradient(
					0,
					_height / 4 * 2,
					GetColorArray(), // Colors to draw gradient
						null // Position is undefined
				);

			// Return the SweepGradient
			return gradient;
		}

	
		// Custom method to generate random color array
		protected int[] GetColorArray()
		{
			int[] colors = new int[2];
			colors[1] = Color.HSVToColor(255, new float[] { 0, 0, 0 });
			colors[0] = Color.HSVToColor(255, new float[] { 0, 0, 50 });


			// Return the color array
			return colors;
		
		}

	}
}
