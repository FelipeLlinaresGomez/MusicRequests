using System;
using Android.Graphics;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Util;

namespace MusicRequests.Droid
{
	public enum ProgressStartPoint {

		DEFAULT = -90, LEFT = 180, RIGHT = 0, BOTTOM = 90
	}


	public class CircleProgressBar : ProgressView {

		private int PADDING = 3;
		private RectF rectF;
		private float bottom;
		private float right;
		private float top;
		private float left;
		private float angle;
		private int radius;
		private bool isGradientColor;
		private bool isSweepGradientColor;
		private Color colorStart, colorEnd;

		public CircleProgressBar (IntPtr a, Android.Runtime.JniHandleOwnership b) : base (a, b)
		{

		}

		public CircleProgressBar(Context context):base(context) {

		}

		public CircleProgressBar(Context context, IAttributeSet attrs, int defStyle):base(context, attrs, defStyle){

		}

		public CircleProgressBar(Context context, IAttributeSet attrs):base(context, attrs) {

		}

	
		public override void Init() {
			rectF = new RectF();

			InitBackgroundColor();
			InitForegroundColor();

		}

		//public void setRadialGradient() {
		//	ColorsHelper.setRadialGradientPaint(foregroundPaint, left, top, min);
		//}

		protected override void OnDraw(Canvas canvas){
			drawGradientColor();
			drawStrokeCircle(canvas);

		}

		public void ResetCircleProgressView()
		{
			setBackgroundColor(Resources.GetColor(Resource.Color.ic_green));
			Invalidate();
		}

		private void drawStrokeCircle(Canvas canvas) {

			canvas.DrawOval(rectF, backgroundPaint);
			angle = 360 * getProgress() / maximum_progress;
		    if(angle<-270)
                setBackgroundColor(backgroundColorEnding);
            else if(angle < -90)
                setBackgroundColor(backgroundColorMedium);
			
            canvas.DrawArc(rectF, startPosInDegrees, angle, false, foregroundPaint);
			drawText(canvas);
		}


		private void drawGradientColor() {
			//if (isGradientColor) {
			//	setLinearGradientProgress(gradColors);
			//}
			//if (isSweepGradientColor) {
			//	setSweepGradPaint();
			//}
		}

		public void setLinearGradientProgress(bool isGradientColor) {
			this.isGradientColor = isGradientColor;
		}

		public void setLinearGradientProgress(bool isGradientColor, int[] colors) {
			this.isGradientColor = isGradientColor;
			gradColors = colors;

		}

		public void setSweepGradientProgress(bool isGradientColor, Color colorStart, Color colorEnd) {
			this.isSweepGradientColor = isGradientColor;
			this.colorStart = colorStart;
			this.colorEnd = colorEnd;


		}

		private void setSweepGradPaint() {
			if (colorStart != 0 && colorEnd != 0 && colorHelper != null) {
				colorHelper.SetSweepGradientPaint(foregroundPaint, min / 2, min / 2, colorStart, colorEnd);
			}
		}

		private void setLinearGradientProgress(int[] gradColors) {
			if (gradColors != null)
				colorHelper.SetGradientPaint(foregroundPaint, left, top, right, bottom, gradColors);
			else
				colorHelper.SetGradientPaint(foregroundPaint, left, top, right, bottom);

		}

		public void setCircleViewPadding(int padding) {
			PADDING = padding;
			Invalidate();
		}

		public int getPadding() {
			return PADDING;
		}
		
			
		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec) {
			
			base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

			left = 0 + strokeWidth / 2;
			top = 0 + strokeWidth / 2;
			right = min - strokeWidth / 2;
			bottom = min - strokeWidth / 2;
			rectF.Set(left + PADDING, top + PADDING, right - PADDING, bottom
				- PADDING);
		}
			
		public int getProgressStartPosition() {
			return startPosInDegrees;
		}

		public void setStartPositionInDegrees(int degrees) {
			this.startPosInDegrees = degrees;
		}

		public void setStartPositionInDegrees(ProgressStartPoint position) {
			this.startPosInDegrees = (int)position;
		}

		public override ShapeType SetType(ShapeType type) {
			return ShapeType.Oval;
		}

	}
}

