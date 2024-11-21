using System;
using CoreGraphics;
using Foundation;
using MusicRequests.Touch.Styles;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using UIKit;

namespace MusicRequests.Touch.Views.Controls
{
    public class TextInputPicker : TextInputView, INotifyPropertyChanged
    {
        private PickerDataTextInput pickerDataModel;
        private UIPickerView picker;

        const int heightPickerView = 216;

        public ICommand SelectedCommand { get; set; }

        private string selectedValue;
        public virtual string SelectedValue
        {
            get => selectedValue;
            set
            {
                if (value != selectedValue)
                {
                    selectedValue = value;
                    UpdateIndexIfNeeded(value);

                    Text = value;
                    if (value != null)
                    {
                        SelectedCommand?.Execute(value);
                        NotifyPropertyChanged();
                    }
                }
            }
        }

        private IList<string> _items;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// The items to show up in the picker
        /// </summary>
        public virtual IList<string> Items
        {
            get => _items;
            set
            {
                _items = value;
                pickerDataModel.Items = value;
            }
        }

        public UIImageView ArrowImageView { get; private set; }

        protected PickerDataTextInput PickerViewModel
        {
            get => pickerDataModel;
            set
            {
                pickerDataModel = value;
                picker.Model = value;
            }
        }

        public TextInputPicker() : base()
        {
            Initialize();
        }

        public TextInputPicker(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        public TextInputPicker(CGRect frame) : base(frame)
        {
            Initialize();
        }

        protected TextInputPicker(NSObjectFlag t) : base(t)
        {
            Initialize();
        }

        protected internal TextInputPicker(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            _hidePlaceholderWhenHasFocus = false;
            pickerDataModel = new PickerDataTextInput();
            CreateComboPicker();
            CreateToolbar();
            CreateRightView();
            TintColor = UIColor.Clear;
        }

        private void CreateRightView()
        {
            ArrowImageView = new UIImageView(UIImage.FromFile("icons/ic_dropdown_arrow.png")?.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate))
            {
                ContentMode = UIViewContentMode.ScaleAspectFit,
                TintColor = Colors.Primary
            };

            RightView = new UIView(new CGRect(0, 0, ArrowImageView.Bounds.Width + Margin.Small, ArrowImageView.Bounds.Height))
            {
                UserInteractionEnabled = false
            };
            RightView.AddSubview(ArrowImageView);
            RightViewMode = UITextFieldViewMode.Always;
        }

        private void CreateComboPicker()
        {
            var pickerFrame = new CGRect(0,
                          0,
                          (int)Dimen.WidthScreen,
                          heightPickerView);

            picker = new UIPickerView(pickerFrame)
            {
                Model = pickerDataModel,
                UserInteractionEnabled = true,
            };

            InputView = picker;
        }

        private void CreateToolbar()
        {
            UIToolbar toolbar;


            toolbar = new UIToolbar(new CGRect(0, 0, Dimen.WidthScreen, 44))
            {
                BarStyle = UIBarStyle.Default,
                Translucent = true,
                UserInteractionEnabled = true
            };
            toolbar.SizeToFit();
            toolbar.BackgroundColor = UIColor.White;

            UIBarButtonItem btnCancel;
            UIBarButtonItem flexibleSpace;
            UIBarButtonItem btnDone;
            UIBarButtonItem[] btnItemsDesde;

            btnCancel = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, OnCancel);

            flexibleSpace = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);

            btnDone = new UIBarButtonItem(UIBarButtonSystemItem.Done, Done);

            btnItemsDesde = new UIBarButtonItem[] { btnCancel, flexibleSpace, btnDone };
            toolbar.SetItems(btnItemsDesde, true);
            InputAccessoryView = toolbar;

            // Desactivamos los botones de undo y redo en iPad
            InputAssistantItem.LeadingBarButtonGroups = new UIBarButtonItemGroup[] { };
            InputAssistantItem.TrailingBarButtonGroups = new UIBarButtonItemGroup[] { };
        }

        protected void UpdateIndexIfNeeded(string value)
        {
            PickerViewModel.UpdateIndexIfNeeded(picker, value);
        }

        private void Done(object sender, EventArgs e)
        {
            if (Items != null)
            {
                SelectedValue = pickerDataModel.SelectedItem;
            }
            ResignFirstResponder();

            NextResponderInForm?.BecomeFirstResponder();
            OnDone?.Invoke(this, null);
        }

        private void OnCancel(object sender, EventArgs e)
        {
            ResignFirstResponder();
            NextResponder?.BecomeFirstResponder();
        }

        public override bool BecomeFirstResponder()
        {
            var result = base.BecomeFirstResponder();
            if (result)
            {
                ArrowImageView.Transform = CGAffineTransform.MakeScale(1, -1);
            }

            return result;
        }

        public override bool ResignFirstResponder()
        {
            var result = base.ResignFirstResponder();
            if (result)
                ArrowImageView.Transform = CGAffineTransform.MakeScale(1, 1);
            return result;
        }

        public override UITextSelectionRect[] GetSelectionRects(UITextRange range)
        {
            return new UITextSelectionRect[0];
        }

        public override CGRect GetCaretRectForPosition(UITextPosition position)
        {
            return CGRect.Empty;
        }

        protected class PickerDataTextInput : UIPickerViewModel
        {

            /// <summary>
            /// The items to show up in the picker
            /// </summary>
            public IList<string> Items { get; set; } = new List<string>();

            /// <summary>
            /// The current selected item
            /// </summary>
            public string SelectedItem
            {
                get
                {
                    var count = Items?.Count ?? 0;
                    if (selectedIndex >= 0 && selectedIndex < count)
                    {
                        return Items[selectedIndex];
                    }
                    return null;
                }
            }

            int selectedIndex = 0;
            private nfloat heigthCell = 40f;


            /// <summary>
            /// Called by the picker to determine how many rows are in a given spinner item
            /// </summary>
            public override nint GetRowsInComponent(UIPickerView picker, nint component)
            {
                return Items?.Count ?? 0;
            }

            /// <summary>
            /// called by the picker to get the text for a particular row in a particular
            /// spinner item
            /// </summary>
            public override string GetTitle(UIPickerView picker, nint row, nint component)
            {
                return Items[(int)row];
            }

            /// <summary>
            /// called by the picker to get the number of spinner items
            /// </summary>
            public override nint GetComponentCount(UIPickerView picker)
            {
                return 1;
            }

            /// <summary>
            /// called when a row is selected in the spinner
            /// </summary>
            public override void Selected(UIPickerView picker, nint row, nint component)
            {
                selectedIndex = (int)row;
            }

            public void UpdateIndexIfNeeded(UIPickerView picker, string value)
            {
                int index = -1;
                for (int i = 0; i < Items?.Count; i++)
                {
                    if (Items[i] == value)
                    {
                        index = i;
                        break;
                    }
                }

                if (index >= 0)
                {
                    picker.Select(index, 0, false);
                    selectedIndex = index;
                }
            }

            public override nfloat GetRowHeight(UIPickerView pickerView, nint component)
            {
                return heigthCell;
            }

            public override UIView GetView(UIPickerView pickerView, nint row, nint component, UIView view)
            {
                var label = new UILabel(new CGRect(0, 0, Dimen.WidthScreen, 0))
                {
                    Text = GetTitle(pickerView, row, component),
                    TextAlignment = UITextAlignment.Center,
                    Lines = 0,
                    LineBreakMode = UILineBreakMode.WordWrap,
                    Font = Fonts.MusicRequestsFont.OfSize(13),
                    TextColor = Colors.Black
                };
                label.SizeToFit();
                var multiplier = label.Frame.Height / 22;
                heigthCell = 40 * multiplier;
                return label;
            }
        }
    }
}

