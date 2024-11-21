using System;
using Android.Content;
using Android.Util;
using Android.Widget;
using Android.Graphics;
using Java.Lang;
using Android.Graphics.Drawables;

namespace MusicRequests.Droid
{
	public class CircleImageView : ImageView {

		private ImageView.ScaleType SCALE_TYPE = ImageView.ScaleType.CenterCrop;
		private Bitmap.Config BITMAP_CONFIG = Bitmap.Config.Argb8888;
		private int COLORDRAWABLE_DIMENSION = 2;
					  
		private static int DEFAULT_BORDER_WIDTH = 0;
		private static Color DEFAULT_BORDER_COLOR = Color.Black;
		private static Color DEFAULT_FILL_COLOR = Color.Transparent;
		private static bool DEFAULT_BORDER_OVERLAY = false;

		private RectF mDrawableRect = new RectF();
		private RectF mBorderRect = new RectF();

		private Matrix mShaderMatrix = new Matrix();
		private Paint mBitmapPaint = new Paint();
		private Paint mBorderPaint = new Paint();
		private Paint mFillPaint = new Paint();

		private Color mBorderColor = DEFAULT_BORDER_COLOR;
		private int mBorderWidth = DEFAULT_BORDER_WIDTH;
		private Color mFillColor = DEFAULT_FILL_COLOR;

		private Bitmap mBitmap;
		private BitmapShader mBitmapShader;
		private int mBitmapWidth;
		private int mBitmapHeight;

		private float mDrawableRadius;
		private float mBorderRadius;

		private ColorFilter mColorFilter;

		private bool mReady;
		private bool mSetupPending;
		private bool mBorderOverlay;
		private bool mDisableCircularTransformation;

        public bool DrawBorder
        {
            set
            {
                if(value)
                {
                    mBorderWidth = DEFAULT_BORDER_WIDTH;
                }
                else
                {
                    mBorderWidth = 0;
                }
            }
        }

		public CircleImageView (IntPtr a, Android.Runtime.JniHandleOwnership b) : base (a, b)
		{
		}

		public CircleImageView(Context context):base(context) {
			Init();
		}

		public CircleImageView(Context context, IAttributeSet attrs):base(context, attrs) {
		
			Init ();
		}

		public CircleImageView(Context context, IAttributeSet attrs, int defStyle) : base (context, attrs, defStyle)
		{

//			TypedArray a = context.ObtainStyledAttributes(attrs, Resource.Styleable.CircleImageView, defStyle, 0);
//
//			mBorderWidth = a.GetDimensionPixelSize(Resource.styleable.CircleImageView_civ_border_width, DEFAULT_BORDER_WIDTH);
//			mBorderColor = a.GetColor(R.styleable.CircleImageView_civ_border_color, DEFAULT_BORDER_COLOR);
//			mBorderOverlay = a.GetBoolean(R.styleable.CircleImageView_civ_border_overlay, DEFAULT_BORDER_OVERLAY);
//			mFillColor = a.GetColor(R.styleable.CircleImageView_civ_fill_color, DEFAULT_FILL_COLOR);

//			a.Recycle();

			Init();
		}

		private void Init() {
			mBorderWidth = 4;
			mBorderColor = Context.Resources.GetColor (Resource.Color.ic_blue);
			base.SetScaleType(SCALE_TYPE);
			mReady = true;

			if (mSetupPending) {
				Setup();
				mSetupPending = false;
			}
		}

		public override ScaleType GetScaleType ()
		{
			return SCALE_TYPE;
		}
			
			
		//public override void SetScaleType(ScaleType scaleType) {
		//	if (scaleType != SCALE_TYPE) {
		//		throw new IllegalArgumentException(System.String.Format("ScaleType {0} not supported.", scaleType));
		//	}
		//}


		public override void SetAdjustViewBounds(bool adjustViewBounds) {
			if (adjustViewBounds) {
				throw new IllegalArgumentException("adjustViewBounds not supported.");
			}
		}

		protected override void OnDraw(Canvas canvas) {
			if (mDisableCircularTransformation) {
				base.OnDraw(canvas);
				return;
			}

			if (mBitmap == null) {
				return;
			}

			if (mFillColor != Color.Transparent) {
				canvas.DrawCircle(mDrawableRect.CenterX(), mDrawableRect.CenterY(), mDrawableRadius, mFillPaint);
			}
			canvas.DrawCircle(mDrawableRect.CenterX(), mDrawableRect.CenterY(), mDrawableRadius, mBitmapPaint);
			if (mBorderWidth > 0) {
				canvas.DrawCircle(mBorderRect.CenterX(), mBorderRect.CenterY(), mBorderRadius, mBorderPaint);
			}
		}
			
		protected override void OnSizeChanged(int w, int h, int oldw, int oldh) {
			base.OnSizeChanged(w, h, oldw, oldh);
			Setup();
		}

		public int GetBorderColor() {
			return mBorderColor;
		}

		public void SetBorderColor(int borderColor) {
			if (borderColor == mBorderColor) {
				return;
			}
			mBorderColor = Context.Resources.GetColor(borderColor);
			mBorderPaint.Color = mBorderColor;
			Invalidate();
		}
			

		public int GetBorderWidth() {
			return mBorderWidth;
		}

		public void SetBorderWidth(int borderWidth) {
			if (borderWidth == mBorderWidth) {
				return;
			}

			mBorderWidth = borderWidth;
			Setup();
		}

		public bool IsBorderOverlay() {
			return mBorderOverlay;
		}

		public void SetBorderOverlay(bool borderOverlay) {
			if (borderOverlay == mBorderOverlay) {
				return;
			}

			mBorderOverlay = borderOverlay;
			Setup();
		}

		public bool IsDisableCircularTransformation() {
			return mDisableCircularTransformation;
		}

		public void SetDisableCircularTransformation(bool disableCircularTransformation) {
			if (mDisableCircularTransformation == disableCircularTransformation) {
				return;
			}

			mDisableCircularTransformation = disableCircularTransformation;
			InitializeBitmap();
		}
			
		public override void SetImageBitmap(Bitmap bm) {
			base.SetImageBitmap(bm);
			InitializeBitmap();
		}
			
		public override void SetImageDrawable(Drawable drawable) {
			base.SetImageDrawable(drawable);
			InitializeBitmap();
		}
			
		public override void SetImageResource(int resId) {
			base.SetImageResource(resId);
			InitializeBitmap();
		}

		public override void SetImageURI (Android.Net.Uri uri)
		{
			base.SetImageURI (uri);
			InitializeBitmap();
		}			

		public override void SetColorFilter(ColorFilter cf) {
			if (cf == mColorFilter) {
				return;
			}

			mColorFilter = cf;
			ApplyColorFilter();
			Invalidate();
		}

		public override ColorFilter ColorFilter {
			get {
				return mColorFilter;
			}
		}			

		private void ApplyColorFilter() {
			if (mBitmapPaint != null) {
				mBitmapPaint.SetColorFilter(mColorFilter);
			}
		}

		private Bitmap GetBitmapFromDrawable(Drawable drawable) {
			if (drawable == null) {
				return null;
			}

			if (drawable is BitmapDrawable) {
				return ((BitmapDrawable) drawable).Bitmap;
			}

			try {
				Bitmap bitmap;

				if (drawable.GetType() == typeof(ColorDrawable)) {
					bitmap = Bitmap.CreateBitmap(COLORDRAWABLE_DIMENSION, COLORDRAWABLE_DIMENSION, BITMAP_CONFIG);
				} else {
					bitmap = Bitmap.CreateBitmap(drawable.IntrinsicWidth, drawable.IntrinsicHeight, BITMAP_CONFIG);
				}

				Canvas canvas = new Canvas(bitmap);
				drawable.SetBounds(0, 0, canvas.Width, canvas.Height);
				drawable.Draw(canvas);
				return bitmap;
			} catch (Java.Lang.Exception e) {
				return null;
			}
		}

		private void InitializeBitmap() {
			if (mDisableCircularTransformation) {
				mBitmap = null;
			} else {
				mBitmap = GetBitmapFromDrawable(Drawable);
			}
			Setup();
		}

		private void Setup() {
			if (!mReady) {
				mSetupPending = true;
				return;
			}

			if (Width == 0 && Height == 0) {
				return;
			}

			if (mBitmap == null) {
				Invalidate();
				return;
			}

			mBitmapShader = new BitmapShader(mBitmap, Shader.TileMode.Clamp, Shader.TileMode.Clamp);

			mBitmapPaint.AntiAlias = (true);
			mBitmapPaint.SetShader(mBitmapShader);

			mBorderPaint.SetStyle(Paint.Style.Stroke);
			mBorderPaint.AntiAlias = (true);
			mBorderPaint.Color = mBorderColor;
			mBorderPaint.StrokeWidth = (mBorderWidth);

			mFillPaint.SetStyle(Paint.Style.Fill);
			mFillPaint.AntiAlias = (true);
			mFillPaint.Color = mFillColor;

			mBitmapHeight = mBitmap.Height;
			mBitmapWidth = mBitmap.Width;

			mBorderRect.Set(calculateBounds());
			mBorderRadius = Java.Lang.Math.Min((mBorderRect.Height() - mBorderWidth) / 2.0f, (mBorderRect.Width() - mBorderWidth) / 2.0f);

			mDrawableRect.Set(mBorderRect);
			if (!mBorderOverlay && mBorderWidth > 0) {
				mDrawableRect.Inset(mBorderWidth - 1.0f, mBorderWidth - 1.0f);
			}
			mDrawableRadius = Java.Lang.Math.Min(mDrawableRect.Height() / 2.0f, mDrawableRect.Width() / 2.0f);

			ApplyColorFilter();
			UpdateShaderMatrix();
			Invalidate();
		}

		private RectF calculateBounds() {
			int availableWidth  = Height- PaddingLeft - PaddingRight;
			int availableHeight = Height - PaddingTop - PaddingBottom;

			int sideLength = Java.Lang.Math.Min(availableWidth, availableHeight);

			float left = PaddingLeft + (availableWidth - sideLength) / 2f;
			float top = PaddingTop + (availableHeight - sideLength) / 2f;

			return new RectF(left, top, left + sideLength, top + sideLength);
		}

		private void UpdateShaderMatrix() {
			float scale;
			float dx = 0;
			float dy = 0;

			mShaderMatrix.Set(null);

			if (mBitmapWidth * mDrawableRect.Height() > mDrawableRect.Width() * mBitmapHeight) {
				scale = mDrawableRect.Height() / (float) mBitmapHeight;
				dx = (mDrawableRect.Width() - mBitmapWidth * scale) * 0.5f;
			} else {
				scale = mDrawableRect.Width() / (float) mBitmapWidth;
				dy = (mDrawableRect.Height() - mBitmapHeight * scale) * 0.5f;
			}

			mShaderMatrix.SetScale(scale, scale);
			mShaderMatrix.PostTranslate((int) (dx + 0.5f) + mDrawableRect.Left, (int) (dy + 0.5f) + mDrawableRect.Top);

			mBitmapShader.SetLocalMatrix(mShaderMatrix);
		}
	}
}

