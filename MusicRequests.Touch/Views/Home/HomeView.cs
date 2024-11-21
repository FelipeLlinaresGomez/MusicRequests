using MusicRequests.Core.ViewModels;
using MusicRequests.Touch.Helpers;
using MusicRequests.Touch.Styles;
using MusicRequests.Touch.Views.Base;
using MvvmCross.Binding.BindingContext;

namespace MusicRequests.Touch.Views
{
    public class HomeView : BaseMusicRequestsViewController<HomeViewModel>
    {
        ActivityView _activityView;

        UIScrollView _scrollView;
        UIView _contentView;

        UIStackView _vstackContent;

        UILabel _lblBienvenido;
        UILabel _lblBienvenidoNombre;

        public override void LoadView()
        {
            base.LoadView();
            CreateControls();
            SetupLayout();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            MvvmCrossBind();
        }

        public override void ViewWillAppear(bool animated)
        {
            SetLogoNavigationBar = true;
            base.ViewWillAppear(animated);
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            SetStyles();
        }

        #region Create controls

        private void CreateControls()
        {
            _activityView = new ActivityView(Dimen.ScreenBounds);

            CreateContent();

            View.AddSubview(_activityView);
        }

        private void CreateContent()
        {
            _scrollView = new UIScrollView();
            _contentView = new UIView();

            _lblBienvenido = Templates.MultilineLabel();
            _lblBienvenidoNombre = Templates.MultilineLabel();

            _vstackContent = new UIStackView(new UIView[] {
                _lblBienvenido,
                _lblBienvenidoNombre
            })
            {
                Axis = UILayoutConstraintAxis.Vertical,
                Spacing = Margin.Small
            };


            _contentView.AddSubviews(_vstackContent);
            _scrollView.AddSubview(_contentView);
            View.AddSubview(_scrollView);
        }

        #endregion

        private void SetupLayout()
        {
            _scrollView.SetupScroll(_contentView);
            _scrollView.ContentInset = View.SafeAreaInsets;

            _lblBienvenido.ToAutoLayout();
            _lblBienvenidoNombre.ToAutoLayout();

            _vstackContent.BindToView(_contentView, margins: new AutoLayoutHelper.Margins(all: Margin.Medium, bottom: Margin.MediumLarge));
        }

        private void MvvmCrossBind()
        {
            var set = this.CreateBindingSet<HomeView, HomeViewModel>();

            set.Bind(_activityView).For(v => v.IsLoading).To(vm => vm.Cargando);
            set.Bind(_activityView).For(v => v.Text).To(vm => vm.CargandoStr);

            set.Bind(this).For(v => v.Title).To(vm => vm.Title);

            this.BindLanguage(_lblBienvenido, "Bienvenido");
            //set.Bind(_lblBienvenidoNombre).To(vm => vm.BienvenidoNombre);

            set.Apply();
        }

        private void SetStyles()
        {
            Theme.Background(View);

            _lblBienvenido.Font = Fonts.MusicRequestsFont.BoldOfSize(20);
            _lblBienvenido.TextColor = Colors.Black;

            _lblBienvenidoNombre.Font = Fonts.MusicRequestsFont.MediumOfSize(14);
            _lblBienvenidoNombre.TextColor = Colors.Gray55;
        }

        public override UIScrollView GetScrollViewForKeyBoard() => _scrollView;
    }
}