using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace TaskDialogInterop
{
	/// <summary>
	/// A WPF implementation of the native Windows Task Dialog.
	/// </summary>
	[TemplatePart(Name = ContentTextBlockTemplateName, Type = typeof(TextBlock))]
	[TemplatePart(Name = ContentExpandedTextBlockTemplateName, Type = typeof(TextBlock))]
	[TemplatePart(Name = FooterTextBlockTemplateName, Type = typeof(TextBlock))]
	[TemplatePart(Name = FooterExpandedTextBlockTemplateName, Type = typeof(TextBlock))]
	[TemplatePart(Name = VerificationCheckBoxTemplateName, Type = typeof(UIElement))]
	public partial class EmulatedTaskDialog : Window
	{
		private const string ContentTextBlockTemplateName = "PART_ContentText";
		private const string ContentExpandedTextBlockTemplateName = "PART_ContentExpandedText";
		private const string FooterTextBlockTemplateName = "PART_FooterText";
		private const string FooterExpandedTextBlockTemplateName = "PART_FooterExpandedText";
		private const string VerificationCheckBoxTemplateName = "PART_VerificationCheckBox";

		static EmulatedTaskDialog()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(EmulatedTaskDialog), new FrameworkPropertyMetadata(typeof(EmulatedTaskDialog)));
		}

		/// <summary>
		/// Identifies the <see cref="P:MainInstructionStyle" /> dependency property.
		/// </summary>
		public static readonly DependencyProperty MainInstructionStyleProperty =
			DependencyProperty.Register("MainInstructionStyle", typeof(Style), typeof(EmulatedTaskDialog), new PropertyMetadata(null));
		/// <summary>
		/// Identifies the <see cref="P:ContentTextStyle" /> dependency property.
		/// </summary>
		public static readonly DependencyProperty ContentTextStyleProperty =
			DependencyProperty.Register("ContentTextStyle", typeof(Style), typeof(EmulatedTaskDialog), new PropertyMetadata(null));
		/// <summary>
		/// Identifies the <see cref="P:ContentExpandedTextStyle" /> dependency property.
		/// </summary>
		public static readonly DependencyProperty ContentExpandedTextStyleProperty =
			DependencyProperty.Register("ContentExpandedTextStyle", typeof(Style), typeof(EmulatedTaskDialog), new PropertyMetadata(null));
		/// <summary>
		/// Identifies the <see cref="P:ProgressBarStyle" /> dependency property.
		/// </summary>
		public static readonly DependencyProperty ProgressBarStyleProperty =
			DependencyProperty.Register("ProgressBarStyle", typeof(Style), typeof(EmulatedTaskDialog), new PropertyMetadata(null));
		/// <summary>
		/// Identifies the <see cref="P:RadioButtonStyle" /> dependency property.
		/// </summary>
		public static readonly DependencyProperty RadioButtonStyleProperty =
			DependencyProperty.Register("RadioButtonStyle", typeof(Style), typeof(EmulatedTaskDialog), new PropertyMetadata(null));
		/// <summary>
		/// Identifies the <see cref="P:CommandLinkStyle" /> dependency property.
		/// </summary>
		public static readonly DependencyProperty CommandLinkStyleProperty =
			DependencyProperty.Register("CommandLinkStyle", typeof(Style), typeof(EmulatedTaskDialog), new PropertyMetadata(null));
		/// <summary>
		/// Identifies the <see cref="P:ButtonsAreaStyle" /> dependency property.
		/// </summary>
		public static readonly DependencyProperty ButtonsAreaStyleProperty =
			DependencyProperty.Register("ButtonsAreaStyle", typeof(Style), typeof(EmulatedTaskDialog), new PropertyMetadata(null));
		/// <summary>
		/// Identifies the <see cref="P:ShowDetailsButtonStyle" /> dependency property.
		/// </summary>
		public static readonly DependencyProperty ShowDetailsButtonStyleProperty =
			DependencyProperty.Register("ShowDetailsButtonStyle", typeof(Style), typeof(EmulatedTaskDialog), new PropertyMetadata(null));
		/// <summary>
		/// Identifies the <see cref="P:VerificationCheckBoxStyle" /> dependency property.
		/// </summary>
		public static readonly DependencyProperty VerificationCheckBoxStyleProperty =
			DependencyProperty.Register("VerificationCheckBoxStyle", typeof(Style), typeof(EmulatedTaskDialog), new PropertyMetadata(null));
		/// <summary>
		/// Identifies the <see cref="P:VerificationTextStyle" /> dependency property.
		/// </summary>
		public static readonly DependencyProperty VerificationTextStyleProperty =
			DependencyProperty.Register("VerificationTextStyle", typeof(Style), typeof(EmulatedTaskDialog), new PropertyMetadata(null));
		/// <summary>
		/// Identifies the <see cref="P:CommandButtonStyle" /> dependency property.
		/// </summary>
		public static readonly DependencyProperty CommandButtonStyleProperty =
			DependencyProperty.Register("CommandButtonStyle", typeof(Style), typeof(EmulatedTaskDialog), new PropertyMetadata(null));
		/// <summary>
		/// Identifies the <see cref="P:FooterAreaStyle" /> dependency property.
		/// </summary>
		public static readonly DependencyProperty FooterAreaStyleProperty =
			DependencyProperty.Register("FooterAreaStyle", typeof(Style), typeof(EmulatedTaskDialog), new PropertyMetadata(null));
		/// <summary>
		/// Identifies the <see cref="P:FooterInnerAreaStyle" /> dependency property.
		/// </summary>
		public static readonly DependencyProperty FooterInnerAreaStyleProperty =
			DependencyProperty.Register("FooterInnerAreaStyle", typeof(Style), typeof(EmulatedTaskDialog), new PropertyMetadata(null));
		/// <summary>
		/// Identifies the <see cref="P:FooterExpandedAreaStyle" /> dependency property.
		/// </summary>
		public static readonly DependencyProperty FooterExpandedAreaStyleProperty =
			DependencyProperty.Register("FooterExpandedAreaStyle", typeof(Style), typeof(EmulatedTaskDialog), new PropertyMetadata(null));
		/// <summary>
		/// Identifies the <see cref="P:FooterExpandedInnerAreaStyle" /> dependency property.
		/// </summary>
		public static readonly DependencyProperty FooterExpandedInnerAreaStyleProperty =
			DependencyProperty.Register("FooterExpandedInnerAreaStyle", typeof(Style), typeof(EmulatedTaskDialog), new PropertyMetadata(null));

		private TextBlock _contentText;
		private TextBlock _contentExpandedInfo;
		private TextBlock _footerText;
		private TextBlock _footerExpandedInfo;
		private UIElement _verificationCheckBox;

		/// <summary>
		/// Initializes a new instance of the <see cref="EmulatedTaskDialog"/> class.
		/// </summary>
		public EmulatedTaskDialog()
		{
			Loaded += TaskDialog_Loaded;
			SourceInitialized += TaskDialog_SourceInitialized;
			KeyDown += TaskDialog_KeyDown;
			Closing += TaskDialog_Closing;
			Closed += TaskDialog_Closed;
		}

		/// <summary>
		/// Gets or sets the style for the main instruction text.
		/// </summary>
		public Style MainInstructionStyle
		{
			get => (Style)GetValue(MainInstructionStyleProperty);
            set => SetValue(MainInstructionStyleProperty, value);
        }
		/// <summary>
		/// Gets or sets the style for the content text.
		/// </summary>
		public Style ContentTextStyle
		{
			get => (Style)GetValue(ContentTextStyleProperty);
            set => SetValue(ContentTextStyleProperty, value);
        }
		/// <summary>
		/// Gets or sets the style for the expanded content text.
		/// </summary>
		public Style ContentExpandedTextStyle
		{
			get => (Style)GetValue(ContentExpandedTextStyleProperty);
            set => SetValue(ContentExpandedTextStyleProperty, value);
        }
		/// <summary>
		/// Gets or sets the style for the progress bar.
		/// </summary>
		public Style ProgressBarStyle
		{
			get => (Style)GetValue(ProgressBarStyleProperty);
            set => SetValue(ProgressBarStyleProperty, value);
        }
		/// <summary>
		/// Gets or sets the style for radio buttons.
		/// </summary>
		public Style RadioButtonStyle
		{
			get => (Style)GetValue(RadioButtonStyleProperty);
            set => SetValue(RadioButtonStyleProperty, value);
        }
		/// <summary>
		/// Gets or sets the style for command links.
		/// </summary>
		public Style CommandLinkStyle
		{
			get => (Style)GetValue(CommandLinkStyleProperty);
            set => SetValue(CommandLinkStyleProperty, value);
        }
		/// <summary>
		/// Gets or sets the style for the buttons area.
		/// </summary>
		public Style ButtonsAreaStyle
		{
			get => (Style)GetValue(ButtonsAreaStyleProperty);
            set => SetValue(ButtonsAreaStyleProperty, value);
        }
		/// <summary>
		/// Gets or sets the style for the show details button.
		/// </summary>
		public Style ShowDetailsButtonStyle
		{
			get => (Style)GetValue(ShowDetailsButtonStyleProperty);
            set => SetValue(ShowDetailsButtonStyleProperty, value);
        }
		/// <summary>
		/// Gets or sets the style for the verification check box.
		/// </summary>
		public Style VerificationCheckBoxStyle
		{
			get => (Style)GetValue(VerificationCheckBoxStyleProperty);
            set => SetValue(VerificationCheckBoxStyleProperty, value);
        }
		/// <summary>
		/// Gets or sets the style for the verification check box text.
		/// </summary>
		public Style VerificationTextStyle
		{
			get => (Style)GetValue(VerificationTextStyleProperty);
            set => SetValue(VerificationTextStyleProperty, value);
        }
		/// <summary>
		/// Gets or sets the style for normal command buttons.
		/// </summary>
		public Style CommandButtonStyle
		{
			get => (Style)GetValue(CommandButtonStyleProperty);
            set => SetValue(CommandButtonStyleProperty, value);
        }
		/// <summary>
		/// Gets or sets the style for the footer area.
		/// </summary>
		public Style FooterAreaStyle
		{
			get => (Style)GetValue(FooterAreaStyleProperty);
            set => SetValue(FooterAreaStyleProperty, value);
        }
		/// <summary>
		/// Gets or sets the style for the footer area.
		/// </summary>
		public Style FooterInnerAreaStyle
		{
			get => (Style)GetValue(FooterInnerAreaStyleProperty);
            set => SetValue(FooterInnerAreaStyleProperty, value);
        }
		/// <summary>
		/// Gets or sets the style for the expanded footer area.
		/// </summary>
		public Style FooterExpandedAreaStyle
		{
			get => (Style)GetValue(FooterExpandedAreaStyleProperty);
            set => SetValue(FooterExpandedAreaStyleProperty, value);
        }
		/// <summary>
		/// Gets or sets the style for the expanded footer area.
		/// </summary>
		public Style FooterExpandedInnerAreaStyle
		{
			get => (Style)GetValue(FooterExpandedInnerAreaStyleProperty);
            set => SetValue(FooterExpandedInnerAreaStyleProperty, value);
        }

		/// <summary>
		/// When overridden in a derived class, is invoked whenever application code
		/// or internal processes call System.Windows.FrameworkElement.ApplyTemplate().
		/// </summary>
		public override void OnApplyTemplate()
		{	
			base.OnApplyTemplate();

			_contentText = GetTemplateChild(ContentTextBlockTemplateName) as TextBlock;
			_contentExpandedInfo = GetTemplateChild(ContentExpandedTextBlockTemplateName) as TextBlock;
			_footerText = GetTemplateChild(FooterTextBlockTemplateName) as TextBlock;
			_footerExpandedInfo = GetTemplateChild(FooterExpandedTextBlockTemplateName) as TextBlock;
			_verificationCheckBox = GetTemplateChild(VerificationCheckBoxTemplateName) as UIElement;
		}

		private EmulatedTaskDialogViewModel ViewModel => DataContext as EmulatedTaskDialogViewModel;

        private void TaskDialog_Loaded(object sender, RoutedEventArgs e)
		{
			if (ViewModel != null)
			{
				ViewModel.PropertyChanged += ViewModel_PropertyChanged;
				ViewModel.RequestClose += ViewModel_RequestClose;
				ViewModel.RequestVerificationFocus += ViewModel_RequestVerificationFocus;

				WindowStartupLocation = ViewModel.StartPosition;

				// Attempt to set initial focus on any button marked IsDefault
				var defaultButton = GetDefaultButton(this);

                defaultButton?.Focus();

                ConvertToHyperlinkedText(_contentText, ViewModel.Content);
				ConvertToHyperlinkedText(_contentExpandedInfo, ViewModel.ContentExpandedInfo);
				ConvertToHyperlinkedText(_footerExpandedInfo, ViewModel.FooterExpandedInfo);
				ConvertToHyperlinkedText(_footerText, ViewModel.FooterText);

				//if (ViewModel.NormalButtons.Count == 0)
				//    this.MaxWidth = 462;
				//else if (ViewModel.NormalButtons.Count == 4)
				//    this.MaxWidth = 600;
				//else if (ViewModel.NormalButtons.Count == 5)
				//    this.MaxWidth = 660;
				//else if (ViewModel.NormalButtons.Count == 6)
				//    this.MaxWidth = 720;
				//else if (ViewModel.NormalButtons.Count > 6)
				//    this.MaxWidth = 800;

				//// Footer only shows the secondary white top border when the buttons section is visible
				//FooterInner.BorderThickness = new Thickness(
				//    FooterInner.BorderThickness.Left,
				//    ((ButtonsArea.Visibility == System.Windows.Visibility.Visible) ? 1 : 0),
				//    FooterInner.BorderThickness.Right,
				//    FooterInner.BorderThickness.Bottom);

				// Play the appropriate sound
				switch (ViewModel.MainIconType)
				{
                    case TaskDialogIcon.Warning:
						System.Media.SystemSounds.Exclamation.Play();
						break;
					case TaskDialogIcon.Error:
						System.Media.SystemSounds.Hand.Play();
						break;
					case TaskDialogIcon.Information:
						System.Media.SystemSounds.Asterisk.Play();
						break;
				}

				ViewModel.NotifyShown();
			}
		}
		private void TaskDialog_SourceInitialized(object sender, EventArgs e)
		{
			if (ViewModel != null)
			{
				ViewModel.NotifyConstructed();
				ViewModel.NotifyCreated();

				if (ViewModel.AllowDialogCancellation)
				{
					SafeNativeMethods.SetWindowIconVisibility(this, false);
				}
				else
				{
					// This also hides the icon, too
					SafeNativeMethods.SetWindowCloseButtonVisibility(this, false);
				}
			}
		}
		private void TaskDialog_KeyDown(object sender, KeyEventArgs e)
		{
			if (ViewModel != null)
			{
				// Block Alt-F4 if it isn't allowed
				if (!ViewModel.AllowDialogCancellation
					&& e.Key == Key.System && e.SystemKey == Key.F4)
					e.Handled = true;

				// Handel Esc manually if the override has been set
				if (ViewModel.AllowDialogCancellation
					&& e.Key == Key.Escape)
				{
					e.Handled = true;
					DialogResult = false;
					Close();
				}
			}
		}
		private void TaskDialog_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			ViewModel.NotifyClosing();
			e.Cancel = ViewModel.ShouldCancelClosing();
		}
		private void TaskDialog_Closed(object sender, EventArgs e)
		{
			ViewModel.NotifyClosed();
		}
		private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Content")
				ConvertToHyperlinkedText(_contentText, ViewModel.Content);
			if (e.PropertyName == "ContentExpandedInfo")
				ConvertToHyperlinkedText(_contentExpandedInfo, ViewModel.ContentExpandedInfo);
			if (e.PropertyName == "FooterExpandedInfo")
				ConvertToHyperlinkedText(_footerExpandedInfo, ViewModel.FooterExpandedInfo);
			if (e.PropertyName == "FooterText")
				ConvertToHyperlinkedText(_footerText, ViewModel.FooterText);
		}
		private void ViewModel_RequestClose(object sender, EventArgs e)
		{
			Close();
		}
		private void ViewModel_RequestVerificationFocus(object sender, EventArgs e)
		{
			_verificationCheckBox.Focus();
		}
		private void Hyperlink_Click(object sender, EventArgs e)
        {
            if (!(sender is Hyperlink hyperlink)) return;
            string uri = hyperlink.Tag.ToString();

            if (ViewModel.HyperlinkCommand.CanExecute(uri))
                ViewModel.HyperlinkCommand.Execute(uri);
        }

		private void ConvertToHyperlinkedText(TextBlock textBlock, string text)
		{
			foreach (Inline inline in textBlock.Inlines)
			{
				if (inline is Hyperlink hyperlink)
				{
					hyperlink.Click -= Hyperlink_Click;
				}
			}

			textBlock.Inlines.Clear();

			if (string.IsNullOrEmpty(text))
				return;

			List<Hyperlink> hyperlinks = new List<Hyperlink>();

			foreach (Match match in TaskDialog.HyperlinkCaptureRegex.Matches(text))
			{
				var hyperlink = new Hyperlink();

				hyperlink.Inlines.Add(match.Groups["text"].Value);
				hyperlink.Tag = match.Groups["link"].Value;
				hyperlink.Click += Hyperlink_Click;

				hyperlinks.Add(hyperlink);
			}

			string[] substrings = TaskDialog.HyperlinkRegex.Split(text);

			for (int i = 0; i < substrings.Length; i++)
			{
				textBlock.Inlines.Add(substrings[i]);

				if (i < hyperlinks.Count)
					textBlock.Inlines.Add(hyperlinks[i]);
			}
		}
		private Button GetDefaultButton(DependencyObject parent)
		{
			var childrenCount = VisualTreeHelper.GetChildrenCount(parent);

			for (int i = 0; i < childrenCount; i++)
			{
				var child = VisualTreeHelper.GetChild(parent, i);

                var childType = child.GetType();

                if (typeof(Button).IsAssignableFrom(childType))
                {
                    if (child is Button button && button.IsDefault)
                        return button;
                }

                // Test its children
				var childButton = GetDefaultButton(child);

				if (childButton != null)
					return childButton;
			}

			return null;
		}
	}
}
