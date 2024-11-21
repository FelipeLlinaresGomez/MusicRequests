using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using MvvmCross.Platforms.Android.Binding.Views;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace MusicRequests.Droid
{
    public class MvxListViewWithHeader : MvxListView
    {
        /// <summary>
        /// The default id for the grid header/footer.  This means there is no header/footer
        /// </summary>
        private const int DEFAULT_HEADER_ID = -1;

        private int _footerId;
        private int _headerId;
        private IList<ListView.FixedViewInfo> headers;
        private IList<ListView.FixedViewInfo> footers;

        public MvxListViewWithHeader(Context context, IAttributeSet attrs)
            : base(context, attrs, null)
        {
            IMvxAdapter adapter = new MvxAdapter(context);

            ApplyAttributes(context, attrs);

            var itemTemplateId = MvxAttributeHelpers.ReadListItemTemplateId(context, attrs);
            adapter.ItemTemplateId = itemTemplateId;

            headers = GetHeaders();
            footers = GetFooters();

            //IMvxAdapter headerAdapter = new HeaderMvxAdapter(headers, footers, adapter);

            Adapter = adapter;
        }

        private void ApplyAttributes(Context c, IAttributeSet attrs)
        {
            _headerId = DEFAULT_HEADER_ID;
            _footerId = DEFAULT_HEADER_ID;

            using (var attributes = c.ObtainDisposableStyledAttributes(attrs, Resource.Styleable.ListView))
            {
                foreach (var a in attributes)
                {
                    switch (a)
                    {
                        case Resource.Styleable.ListView_header:
                            _headerId = attributes.GetResourceId(a, DEFAULT_HEADER_ID);
                            break;
                    }
                }
            }
        }

        private IList<ListView.FixedViewInfo> GetFixedViewInfos(int id)
        {
            var viewInfos = new List<ListView.FixedViewInfo>();

            View view = GetBoundView(id);

            if (view != null)
            {
                var info = new ListView.FixedViewInfo(this)
                {
                    Data = null,
                    IsSelectable = true,
                    View = view,
                };
                viewInfos.Add(info);
            }

            return viewInfos;
        }

        private IList<ListView.FixedViewInfo> GetFooters()
        {
            return GetFixedViewInfos(_footerId);
        }

        private IList<ListView.FixedViewInfo> GetHeaders()
        {
            return GetFixedViewInfos(_headerId);
        }

        private View GetBoundView(int id)
        {
            if (id == DEFAULT_HEADER_ID) return null;

            IMvxAndroidBindingContext bindingContext = MvxAndroidBindingContextHelpers.Current();
            var view = bindingContext.BindingInflate(id, null);

            return view;
        }

        //public void SetAdapter(IMvxAdapter adapter)
        //{ 
        //	IMvxAdapter headerAdapter = new HeaderMvxAdapter(headers, footers, adapter);

        //	Adapter = headerAdapter;
        //}

        public new IMvxAdapter Adapter
        {
            get
            {
                return base.Adapter;
            }
            set
            {
                IMvxAdapter headerAdapter = new HeaderMvxAdapter(headers, footers, value);

                base.Adapter = headerAdapter;
            }
        }

        public IList<ListView.FixedViewInfo> GetHeader()
        {
            return headers;
        }
    }
}

