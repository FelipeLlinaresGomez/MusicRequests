using System;
using Android.Views;
using Android.Content.Res;
using Android.Content;
using Android.Util;
using Android.Graphics;
using Android.Animation;
using System.Diagnostics;
using Android.Views.Animations;
using MusicRequests.Droid.Helpers;
using Java.Interop;

namespace MusicRequests.Droid
{
	public interface IOnProgressViewListener {
		 void OnFinish();
	
		 void OnProgressUpdate(float progress);
	}

	public abstract class ProgressView : View, IProgressShape {

		public float progress = 0;
		protected float strokeWidth;
		protected float backgroundStrokeWidth;
		protected Color backgroundColor;
	    protected Color backgroundColorMedium;
		protected Color backgroundColorEnding;
		protected Color color;
		protected int height;
		protected int width;
		protected int min;
		protected Paint backgroundPaint;
		protected Paint foregroundPaint;
		private string PROGRESS = "Loading...";
		protected int startPosInDegrees = (int)ProgressStartPoint.DEFAULT;
		private ObjectAnimator objAnimator;
		protected ProgressViewTextData text_data = new ProgressViewTextData(Color.LightGray, 42);
		private IOnProgressViewListener listenr;
		protected bool isShadow_background, isShadow_progress;
		protected float maximum_progress = 100f;
		private Color shaderColor;
		protected ColorsHelper colorHelper;
		protected int[] gradColors;
		protected bool isRoundEdge;


		public abstract void Init();

		protected void DefaultValuesInit()
		{
			strokeWidth = Resources.GetDimension(Resource.Dimension.text_tinier);
			backgroundStrokeWidth = Resources.GetDimension(Resource.Dimension.text_tinier);
			backgroundColor = Resources.GetColor(Resource.Color.ic_green);
			backgroundColorMedium = Resources.GetColor(Resource.Color.ic_orange);
			backgroundColorEnding = Resources.GetColor(Resource.Color.ic_red);
            
			color = Resources.GetColor(Resource.Color.ic_blue);
			shaderColor = Resources.GetColor(Resource.Color.ic_blue);
		}

		public void setOnProgressViewListener(IOnProgressViewListener listener) {
			this.listenr = listener;
		}

		public ProgressView (IntPtr a, Android.Runtime.JniHandleOwnership b) : base (a, b)
		{

		}

		public ProgressView(Context context) : base(context) {
			DefaultValuesInit ();
			Init();
			colorHelper = new ColorsHelper();
		}

		public ProgressView(Context context, IAttributeSet attrs):base(context, attrs) {
			DefaultValuesInit ();
			InitTypedArray(context, attrs);
			Init();
		}

		public ProgressView(Context context, IAttributeSet attrs, int defStyle):base(context, attrs, defStyle) {
			DefaultValuesInit ();
			Init();
		}

		private void InitTypedArray(Context context, IAttributeSet attrs) {
			//TypedArray typedArray = Context.Theme.ObtainStyledAttributes(
			//	attrs, Resource.Styleable.CircleProgressBar, 0, 0);
			//try {
			//	progress = typedArray.GetFloat(
			//		Resource.Styleable.CircleProgressBar_progress, progress);
			//	strokeWidth = typedArray.GetDimension(
			//		Resource.Styleable.CircleProgressBar_progress_width, strokeWidth);
			//	backgroundStrokeWidth = typedArray.GetDimension(
			//		Resource.Styleable.CircleProgressBar_bar_width,
			//		backgroundStrokeWidth);
			//	var colorInt = typedArray.GetInt(
			//		Resource.Styleable.CircleProgressBar_progress_color, color);
			//	color = Context.Resources.GetColor(colorInt);
			//	var backgroundColorInt = typedArray.GetInt(
			//		Resource.Styleable.CircleProgressBar_bar_color, backgroundColor);
			//	backgroundColor = Context.Resources.GetColor(backgroundColorInt);

			//	var textColorInt = typedArray.GetInt(
			//		Resource.Styleable.CircleProgressBar_text_color,
			//		text_data.textColor);
			//	text_data.textColor = Context.Resources.GetColor(textColorInt); 

			//	text_data.textSize = typedArray
			//		.GetInt(Resource.Styleable.CircleProgressBar_text_size,
			//			text_data.textSize);
			//} 
			//catch(Exception e)
			//{
			//	var msg = e.Message;
			//}
			//finally {
			//	typedArray.Recycle();
			//}
			SetShadowLayer();
			colorHelper = new ColorsHelper();
		}

		private void SetShadowLayer() {
//			if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.HONEYCOMB) {
//				setLayerType(LAYER_TYPE_SOFTWARE, backgroundPaint);
//				setLayerType(LAYER_TYPE_SOFTWARE, foregroundPaint);
//
//			}
		}
			
		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec) {
			min = SetDimensions(widthMeasureSpec, heightMeasureSpec);
		}

		protected int SetDimensions(int widthMeasureSpec, int heightMeasureSpec) {
			height = GetDefaultSize(SuggestedMinimumHeight, heightMeasureSpec);
			width = GetDefaultSize(SuggestedMinimumWidth, widthMeasureSpec);

			int smallerDimens = Math.Min(width, height);
			SetMeasuredDimension(smallerDimens, smallerDimens);
			return smallerDimens;
		}

		[Export]
		protected float getProgress() {
			return progress;
		}

		[Export]
		public void setProgress(float progress) {
			setProgressInView(progress);
			Invalidate ();
		}

		private void setProgressInView(float progress) {
			this.progress = (progress <= maximum_progress) ? progress : maximum_progress;
			Invalidate();
			trackProgressInView(progress);
		}

		public void resetProgressBar() {
			setProgress(0);
			InitBackgroundColor();
		}

		public void setWidth(int circleWidth) {
			ViewGroup.LayoutParams lparams = this.LayoutParameters;
			lparams.Width = circleWidth;
			RequestLayout();
		}

		public float getWidthProgressBarLine() {
			return strokeWidth;
		}

		public void setWidthProgressBarLine(float strokeWidth) {
			this.strokeWidth = strokeWidth;
			foregroundPaint.StrokeWidth=(strokeWidth);
			Invalidate();
			RequestLayout();
		}

		public float getWidthProgressBackground() {
			return backgroundStrokeWidth;
		}

		public void setWidthProgressBackground(float backgroundStrokeWidth) {
			this.backgroundStrokeWidth = backgroundStrokeWidth;
			backgroundPaint.StrokeWidth = strokeWidth;
			Invalidate();
			RequestLayout();
		}
			
		protected void InitForegroundColor() {
			foregroundPaint = new Paint (PaintFlags.AntiAlias);
			foregroundPaint.Color = (color);
			foregroundPaint.SetStyle(Paint.Style.Stroke);
			foregroundPaint.StrokeWidth = (strokeWidth);
			if (isRoundEdge) {
				foregroundPaint.StrokeCap = (Paint.Cap.Round);
			}
			// if (isShadow_progress)
			//foregroundPaint.setShadowLayer(1, 2, 4, shaderColor);

		}

		protected void InitBackgroundColor() {
			backgroundPaint = new Paint(PaintFlags.AntiAlias);
			backgroundPaint.Color = (backgroundColor);
			backgroundPaint.SetStyle(Paint.Style.Stroke);
			backgroundPaint.StrokeWidth = (backgroundStrokeWidth);
			if (isShadow_background)
				backgroundPaint.SetShadowLayer(2, 2, 4, shaderColor);

		}

		public void setRoundEdgeProgress(bool isRoundEdge) {
			this.isRoundEdge = isRoundEdge;
			Init();
		}
			
		public int getProgressColor() {
			return color;
		}

		public void setProgressColor(Color color) {
			this.color = color;
			foregroundPaint.Color = (color);
			Invalidate();
			RequestLayout();
		}
			
		public int getBackgroundColor() {
			return backgroundColor;
		}

		public void setBackgroundColor(Color backgroundColor) {
			this.backgroundColor = backgroundColor;
			backgroundPaint.Color = (backgroundColor);
			Invalidate();
			RequestLayout();
		}

        public int getBackgroundColorMedium() {
			return backgroundColorMedium;
		}

		public void setBackgroundColorMedium(Color backgroundColorMedium) {
			this.backgroundColorMedium = backgroundColorMedium;

		}

        public int getBackgroundColorEnding() {
			return backgroundColorEnding;
		}

		public void setBackgroundColorEnding(Color backgroundColorEnding) {
			this.backgroundColorEnding = backgroundColorEnding;

		}

			
		private void initAnimator(float progres) {
			objAnimator = ObjectAnimator.OfFloat(this, PROGRESS, progres);
			objAnimator.SetInterpolator(new DecelerateInterpolator());
			trackProgressInView(progress);
		}

		private void trackProgressInView(float progress) {
			if (listenr != null) {
				listenr.OnProgressUpdate(progress);
				if (progress >= maximum_progress) {
					listenr.OnFinish();
				}
			}
		}
			
		public void setProgressIndeterminateAnimation(int animSpeedMillisec) {

			initAnimator(maximum_progress);
			objAnimator.SetDuration(animSpeedMillisec);
			objAnimator.RepeatCount = (ValueAnimator.Infinite);

			objAnimator.Start();
		}
			
		public void cancelAnimation() {
			if (objAnimator != null) {
				objAnimator.Cancel();
			}
		}
			
		public class ProgressViewTextData {
			public Color textColor;
			public int textSize;
			public bool isWithText;
			public string progressText;

			public ProgressViewTextData(Color textColor, int textSize) {
				this.textColor = textColor;
				this.textSize = textSize;
			}

		}

		public int getTextColor() {
			return this.text_data.textColor;
		}

		public void setTextColor(Color textColor) {
			this.text_data.textColor = textColor;
			Invalidate();
		}

		public int getTextSize() {
			return this.text_data.textSize;
		}
			
		public void setTextSize(int textSize) {
			this.text_data.textSize = textSize;
		}
			
		public void setText(String text) {
			this.text_data.isWithText = true;
			text_data.progressText = text;
			Invalidate();
		}
			
		public void setText(String text, Color color) {
			this.text_data.isWithText = true;
			text_data.progressText = text;
			this.text_data.textColor = color;
			Invalidate();
		}

		public void setText(String text, int textSize, Color color) {
			this.text_data.isWithText = true;
			this.text_data.progressText = text;
			this.text_data.textColor = color;
			this.text_data.textSize = textSize;
			Invalidate();
		}

		protected void drawText(Canvas canvas) {
			if (text_data.isWithText)
				colorHelper.DrawTextCenter(canvas, text_data.progressText,
					text_data.textColor, text_data.textSize, min);

		}

		protected void drawTextLine(Canvas canvas) {
			if (text_data.isWithText)
				colorHelper.DrawTextCenter(canvas, text_data.progressText,
					text_data.textColor, text_data.textSize, width);

		}

		//TODO:*******************	SHADOW ***********************

		//public boolean isShadowInBackground() {
		//return isShadow_background;
		//}

		//public void setShadowInBackground(boolean isShadow_background) {
		//	this.isShadow_background = isShadow_background;
		//	init();

		//}
		//public void setShadowInBackground(boolean isShadow_background, String hexColor) {
		//this.isShadow_background = isShadow_background;
		//convertStringToIntColor(hexColor);
		//	init();

		//}


		//public boolean isShadowInProgress() {
		//	return isShadow_progress;
		//}
		//public void setShadowInProgress(boolean isShadow_progress, String hexColor) {
		//	this.isShadow_progress = isShadow_progress;
		//	convertStringToIntColor(hexColor);
		//	init();

		//}
		//public void setShadowInProgress(boolean isShadow_progress) {
		//	this.isShadow_progress = isShadow_progress;
		//	init();
		//}
		private void convertStringToIntColor(String hexColor) {
			if (hexColor != null) {
				try {
					this.shaderColor = Color.ParseColor(hexColor);

				} catch (Exception e) {
					Debug.WriteLine (e.Message);
				}

			}
		}

		public abstract Android.Graphics.Drawables.ShapeType SetType (Android.Graphics.Drawables.ShapeType type);

	}
}

