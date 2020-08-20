using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;

// Most of this interop code taken from:
// http://www.codeproject.com/KB/vista/Vista_TaskDialog_Wrapper.aspx

namespace TaskDialogInterop
{
	/// <summary>
	/// Specifies the standard buttons that are displayed on a task dialog.
	/// </summary>
	[Flags]
	public enum TaskDialogCommonButtons
	{
		/// <summary>
		/// The message box displays no buttons.
		/// </summary>
		None = 0,
		/// <summary>
		/// The message box displays an OK button. If clicked, the task dialog will return DialogResult.OK.
		/// </summary>
		OK = 0x0001,
		/// <summary>
		/// The message box displays a Yes button. If clicked, the task dialog will return DialogResult.Yes.
		/// </summary>
		Yes = 0x0002,
		/// <summary>
		/// The message box displays a No button. If clicked, the task dialog will return DialogResult.No.
		/// </summary>
		No = 0x0004,
		/// <summary>
		/// The message box displays a Cancel button. If clicked, the task dialog will return DialogResult.Cancel.
		/// If this button is specified, the dialog box will respond to typical cancel actions (Alt-F4 and Escape).
		/// </summary>
		Cancel = 0x0008,
		/// <summary>
		/// The message box displays a Retry button. If clicked, the task dialog will return DialogResult.Retry.
		/// </summary>
		Retry = 0x0010,
		/// <summary>
		/// The message box displays a Close button. If clicked, the task dialog will return this value.
		/// </summary>
		Close = 0x0020,
		/// <summary>
		/// The message box displays Yes and No buttons.
		/// </summary>
		YesNo = Yes | No,
		/// <summary>
		/// The message box displays Yes, No, and Cancel buttons.
		/// </summary>
		YesNoCancel = Yes | No | Cancel,
		/// <summary>
		/// The message box displays OK and Cancel buttons.
		/// </summary>
		OKCancel = OK | Cancel,
		/// <summary>
		/// The message box displays Retry and Cancel buttons.
		/// </summary>
		RetryCancel = Retry | Cancel
	}

	/// <summary>
	/// Specifies pre-defined system icons supported by the task dialog.
	/// </summary>
	[SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")] // Type comes from CommCtrl.h
	public enum TaskDialogIcon : uint
	{
		/// <summary>
		/// No icon will be displayed.
		/// </summary>
		None = 0,
		/// <summary>
		/// An exclamation-point icon appears in the task dialog.
		/// </summary>
		Warning = 0xFFFF, // MAKEINTRESOURCEW(-1)
		/// <summary>
		/// A stop-sign icon appears in the task dialog.
		/// </summary>
		Error = 0xFFFE, // MAKEINTRESOURCEW(-2)
		/// <summary>
		/// An icon consisting of a lowercase letter i in a circle appears in the task dialog.
		/// </summary>
		Information = 0xFFFD, // MAKEINTRESOURCEW(-3)
		/// <summary>
		/// A shield icon appears in the task dialog.
		/// </summary>
		Shield = 0xFFFC, // MAKEINTRESOURCEW(-4)
	}

	/// <summary>
	/// Specifies notifications when handling a task dialog callback. 
	/// </summary>
	public enum TaskDialogNotification
	{
		/// <summary>
		/// Sent by the Task Dialog once the dialog has been created and before it is displayed.
		/// The value returned by the callback is ignored.
		/// </summary>
		Created = (int)TASKDIALOG_NOTIFICATIONS.TDN_CREATED,

		//// Spec is not clear what this is so not supporting it.
		///// <summary>
		///// Sent by the Task Dialog when a navigation has occurred.
		///// The value returned by the callback is ignored.
		///// </summary>   
		// Navigated = (int)TASKDIALOG_NOTIFICATIONS.TDN_NAVIGATED,

		/// <summary>
		/// Sent by the Task Dialog when the user selects a button or command link in the task dialog.
		/// The button ID corresponding to the button selected will be available in the
		/// TaskDialogNotificationArgs. To prevent the Task Dialog from closing, the application must
		/// return true, otherwise the Task Dialog will be closed and the button ID returned to via
		/// the original application call.
		/// </summary>
		ButtonClicked = (int)TASKDIALOG_NOTIFICATIONS.TDN_BUTTON_CLICKED,            // wParam = Button ID

		/// <summary>
		/// Sent by the Task Dialog when the user clicks on a hyperlink in the Task Dialog’s content.
		/// The string containing the HREF of the hyperlink will be available in the
		/// TaskDialogNotificationArgs. To prevent the TaskDialog from shell executing the hyperlink,
		/// the application must return TRUE, otherwise ShellExecute will be called.
		/// </summary>
		HyperlinkClicked = (int)TASKDIALOG_NOTIFICATIONS.TDN_HYPERLINK_CLICKED,            // lParam = (LPCWSTR)pszHREF

		/// <summary>
		/// Sent by the Task Dialog approximately every 200 milliseconds when TaskDialog.CallbackTimer
		/// has been set to true. The number of milliseconds since the dialog was created or the
		/// notification returned true is available on the TaskDialogNotificationArgs. To reset
		/// the tickcount, the application must return true, otherwise the tickcount will continue to
		/// increment.
		/// </summary>
		Timer = (int)TASKDIALOG_NOTIFICATIONS.TDN_TIMER,            // wParam = Milliseconds since dialog created or timer reset

		/// <summary>
		/// Sent by the Task Dialog when it is destroyed and its window handle no longer valid.
		/// The value returned by the callback is ignored.
		/// </summary>
		Destroyed = (int)TASKDIALOG_NOTIFICATIONS.TDN_DESTROYED,

		/// <summary>
		/// Sent by the Task Dialog when the user selects a radio button in the task dialog.
		/// The button ID corresponding to the button selected will be available in the
		/// TaskDialogNotificationArgs.
		/// The value returned by the callback is ignored.
		/// </summary>
		RadioButtonClicked = (int)TASKDIALOG_NOTIFICATIONS.TDN_RADIO_BUTTON_CLICKED,            // wParam = Radio Button ID

		/// <summary>
		/// Sent by the Task Dialog once the dialog has been constructed and before it is displayed.
		/// The value returned by the callback is ignored.
		/// </summary>
		DialogConstructed = (int)TASKDIALOG_NOTIFICATIONS.TDN_DIALOG_CONSTRUCTED,

		/// <summary>
		/// Sent by the Task Dialog when the user checks or unchecks the verification checkbox.
		/// The verificationFlagChecked value is available on the TaskDialogNotificationArgs.
		/// The value returned by the callback is ignored.
		/// </summary>
		VerificationClicked = (int)TASKDIALOG_NOTIFICATIONS.TDN_VERIFICATION_CLICKED,             // wParam = 1 if checkbox checked, 0 if not, lParam is unused and always 0

		/// <summary>
		/// Sent by the Task Dialog when the user presses F1 on the keyboard while the dialog has focus.
		/// The value returned by the callback is ignored.
		/// </summary>
		Help = (int)TASKDIALOG_NOTIFICATIONS.TDN_HELP,

		/// <summary>
		/// Sent by the task dialog when the user clicks on the dialog's expando button.
		/// The expanded value is available on the TaskDialogNotificationArgs.
		/// The value returned by the callback is ignored.
		/// </summary>
		ExpandoButtonClicked = (int)TASKDIALOG_NOTIFICATIONS.TDN_EXPANDO_BUTTON_CLICKED            // wParam = 0 (dialog is now collapsed), wParam != 0 (dialog is now expanded)
	}

	/// <summary>
	/// Progress bar state.
	/// </summary>
	[SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")] // Comes from CommCtrl.h PBST_* values which don't have a zero.
	public enum TaskDialogProgressBarState
	{
		/// <summary>
		/// Sets the progress bar to the normal state.
		/// </summary>
		Normal = (int)PROGRESS_STATES.PBST_NORMAL,

		/// <summary>
		/// Sets the progress bar to the error state.
		/// </summary>
		Error = (int)PROGRESS_STATES.PBST_ERROR,

		/// <summary>
		/// Sets the progress bar to the paused state.
		/// </summary>
		Paused = (int)PROGRESS_STATES.PBST_PAUSE
	}

	/// <summary>
	/// A custom button for the TaskDialog.
	/// </summary>
	[SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")] // Would be unused code as not required for usage.
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	public struct TaskDialogButton
	{
		/// <summary>
		/// The ID of the button. This value is returned by TaskDialog.Show when the button is clicked.
		/// </summary>
		private int buttonId;

		/// <summary>
		/// The string that appears on the button.
		/// </summary>
		[MarshalAs(UnmanagedType.LPWStr)]
		private string buttonText;

		/// <summary>
		/// Initialize the custom button.
		/// </summary>
		/// <param name="id">The ID of the button. This value is returned by TaskDialog.Show when
		/// the button is clicked. Typically this will be a value in the DialogResult enum.</param>
		/// <param name="text">The string that appears on the button.</param>
		public TaskDialogButton(int id, string text)
		{
			buttonId = id;
			buttonText = text;
		}

		/// <summary>
		/// The ID of the button. This value is returned by TaskDialog.Show when the button is clicked.
		/// </summary>
		public int ButtonId
		{
			get => buttonId;
            set => buttonId = value;
        }

		/// <summary>
		/// The string that appears on the button.
		/// </summary>
		public string ButtonText
		{
			get => buttonText;
            set => buttonText = value;
        }
	}

	/// <summary>
	/// Arguments passed to the TaskDialog callback.
	/// </summary>
	public class TaskDialogNotificationArgs
	{
		/// <summary>
		/// Gets what the TaskDialog callback is a notification of.
		/// </summary>
		public TaskDialogNotification Notification { get; internal set; }
		/// <summary>
		/// Gets the button ID if the notification is about a button. This a DialogResult
		/// value or the ButtonID member of a TaskDialogButton set in the
		/// TaskDialog.Buttons member.
		/// </summary>
		public int ButtonId { get; internal set; }
		/// <summary>
		/// Gets the button index if the notification is about a button.
		/// </summary>
		public int ButtonIndex { get; internal set; }
		/// <summary>
		/// Gets the HREF string of the hyperlink the notification is about.
		/// </summary>
		public string Hyperlink { get; internal set; }
		/// <summary>
		/// Gets the number of milliseconds since the dialog was opened or the last time the
		/// callback for a timer notification reset the value by returning true.
		/// </summary>
		public uint TimerTickCount { get; internal set; }
		/// <summary>
		/// Gets the state of the verification flag when the notification is about the verification flag.
		/// </summary>
		public bool VerificationFlagChecked { get; internal set; }
		/// <summary>
		/// Gets the state of the dialog expando when the notification is about the expando.
		/// </summary>
		public bool Expanded { get; internal set; }
		/// <summary>
		/// Gets the configuration options for the dialog.
		/// </summary>
		/// <remarks>
		/// Changes to any of the options will be ignored.
		/// </remarks>
		public TaskDialogOptions Config { get; internal set; }
	}

	internal class NativeTaskDialog
	{
		/// <summary>
		/// The string to be used for the dialog box title. If this parameter is NULL, the filename of the executable program is used.
		/// </summary>
		private string windowTitle;

		/// <summary>
		/// The string to be used for the main instruction.
		/// </summary>
		private string mainInstruction;

		/// <summary>
		/// The string to be used for the dialog’s primary content. If the EnableHyperlinks member is true,
		/// then this string may contain hyperlinks in the form: <A HREF="executablestring">Hyperlink Text</A>. 
		/// WARNING: Enabling hyperlinks when using content from an unsafe source may cause security vulnerabilities.
		/// </summary>
		private string content;

		/// <summary>
		/// Specifies the push buttons displayed in the dialog box.  This parameter may be a combination of flags.
		/// If no common buttons are specified and no custom buttons are specified using the Buttons member, the
		/// dialog box will contain the OK button by default.
		/// </summary>
		private TaskDialogCommonButtons commonButtons;

		/// <summary>
		/// Specifies a built in icon for the main icon in the dialog. If this is set to none
		/// and the CustomMainIcon is null then no main icon will be displayed.
		/// </summary>
		private TaskDialogIcon mainIcon;

		/// <summary>
		/// Specifies a custom in icon for the main icon in the dialog. If this is set to none
		/// and the CustomMainIcon member is null then no main icon will be displayed.
		/// </summary>
		private Icon customMainIcon;

		/// <summary>
		/// Specifies a built in icon for the icon to be displayed in the footer area of the
		/// dialog box. If this is set to none and the CustomFooterIcon member is null then no
		/// footer icon will be displayed.
		/// </summary>
		private TaskDialogIcon footerIcon;

		/// <summary>
		/// Specifies a custom icon for the icon to be displayed in the footer area of the
		/// dialog box. If this is set to none and the CustomFooterIcon member is null then no
		/// footer icon will be displayed.
		/// </summary>
		private Icon customFooterIcon;

		/// <summary>
		/// Specifies the custom push buttons to display in the dialog. Use CommonButtons member for
		/// common buttons; OK, Yes, No, Retry and Cancel, and Buttons when you want different text
		/// on the push buttons.
		/// </summary>
		private TaskDialogButton[] buttons;

		/// <summary>
		/// Specifies the radio buttons to display in the dialog.
		/// </summary>
		private TaskDialogButton[] radioButtons;

		/// <summary>
		/// The flags passed to TaskDialogIndirect.
		/// </summary>
		private TASKDIALOG_FLAGS flags;

		/// <summary>
		/// Indicates the default button for the dialog. This may be any of the values specified
		/// in ButtonId members of one of the TaskDialogButton structures in the Buttons array,
		/// or one a DialogResult value that corresponds to a buttons specified in the CommonButtons Member.
		/// If this member is zero or its value does not correspond to any button ID in the dialog,
		/// then the first button in the dialog will be the default. 
		/// </summary>
		private int defaultButton;

		/// <summary>
		/// Indicates the default radio button for the dialog. This may be any of the values specified
		/// in ButtonId members of one of the TaskDialogButton structures in the RadioButtons array.
		/// If this member is zero or its value does not correspond to any radio button ID in the dialog,
		/// then the first button in RadioButtons will be the default.
		/// The property NoDefaultRadioButton can be set to have no default.
		/// </summary>
		private int defaultRadioButton;

		/// <summary>
		/// The string to be used to label the verification checkbox. If this member is null, the
		/// verification checkbox is not displayed in the dialog box.
		/// </summary>
		private string verificationText;

		/// <summary>
		/// The string to be used for displaying additional information. The additional information is
		/// displayed either immediately below the content or below the footer text depending on whether
		/// the ExpandFooterArea member is true. If the EnableHyperlinks member is true, then this string
		/// may contain hyperlinks in the form: <A HREF="executablestring">Hyperlink Text</A>.
		/// WARNING: Enabling hyperlinks when using content from an unsafe source may cause security vulnerabilities.
		/// </summary>
		private string expandedInformation;

		/// <summary>
		/// The string to be used to label the button for collapsing the expanded information. This
		/// member is ignored when the ExpandedInformation member is null. If this member is null
		/// and the CollapsedControlText is specified, then the CollapsedControlText value will be
		/// used for this member as well.
		/// </summary>
		private string expandedControlText;

		/// <summary>
		/// The string to be used to label the button for expanding the expanded information. This
		/// member is ignored when the ExpandedInformation member is null.  If this member is null
		/// and the ExpandedControlText is specified, then the ExpandedControlText value will be
		/// used for this member as well.
		/// </summary>
		private string collapsedControlText;

		/// <summary>
		/// The string to be used in the footer area of the dialog box. If the EnableHyperlinks member
		/// is true, then this string may contain hyperlinks in the form: <A HREF="executablestring">
		/// Hyperlink Text</A>.
		/// WARNING: Enabling hyperlinks when using content from an unsafe source may cause security vulnerabilities.
		/// </summary>
		private string footer;

		/// <summary>
		/// The callback that receives messages from the Task Dialog when various events occur.
		/// </summary>
		private TaskDialogCallback callback;

		/// <summary>
		/// Reference that is passed to the callback.
		/// </summary>
		private object callbackData;

		private TaskDialogOptions config;

		/// <summary>
		/// Specifies the width of the Task Dialog’s client area in DLU’s. If 0, Task Dialog will calculate the ideal width.
		/// </summary>
		private uint width;

		/// <summary>
		/// Creates a default native Task Dialog.
		/// </summary>
		public NativeTaskDialog()
		{
			Reset();
		}

		/// <summary>
		/// Returns true if the current operating system supports TaskDialog. If false TaskDialog.Show should not
		/// be called as the results are undefined but often results in a crash.
		/// </summary>
		public static bool IsAvailableOnThisOS
		{
			get
			{
				OperatingSystem os = Environment.OSVersion;
				if (os.Platform != PlatformID.Win32NT)
					return false;
				return (os.Version.CompareTo(RequiredOSVersion) >= 0);
			}
		}

		/// <summary>
		/// The minimum Windows version needed to support TaskDialog.
		/// </summary>
		public static Version RequiredOSVersion => new Version(6, 0, 5243);

        /// <summary>
		/// The string to be used for the dialog box title. If this parameter is NULL, the filename of the executable program is used.
		/// </summary>
		public string WindowTitle
		{
			get => windowTitle;
            set => windowTitle = value;
        }

		/// <summary>
		/// The string to be used for the main instruction.
		/// </summary>
		public string MainInstruction
		{
			get => mainInstruction;
            set => mainInstruction = value;
        }

		/// <summary>
		/// The string to be used for the dialog’s primary content. If the EnableHyperlinks member is true,
		/// then this string may contain hyperlinks in the form: <A HREF="executablestring">Hyperlink Text</A>. 
		/// WARNING: Enabling hyperlinks when using content from an unsafe source may cause security vulnerabilities.
		/// </summary>
		public string Content
		{
			get => content;
            set => content = value;
        }

		/// <summary>
		/// Specifies the push buttons displayed in the dialog box. This parameter may be a combination of flags.
		/// If no common buttons are specified and no custom buttons are specified using the Buttons member, the
		/// dialog box will contain the OK button by default.
		/// </summary>
		public TaskDialogCommonButtons CommonButtons
		{
			get => commonButtons;
            set => commonButtons = value;
        }

		/// <summary>
		/// Specifies a built in icon for the main icon in the dialog. If this is set to none
		/// and the CustomMainIcon is null then no main icon will be displayed.
		/// </summary>
		public TaskDialogIcon MainIcon
		{
			get => mainIcon;
            set => mainIcon = value;
        }

		/// <summary>
		/// Specifies a custom in icon for the main icon in the dialog. If this is set to none
		/// and the CustomMainIcon member is null then no main icon will be displayed.
		/// </summary>
		public Icon CustomMainIcon
		{
			get => customMainIcon;
            set => customMainIcon = value;
        }

		/// <summary>
		/// Specifies a built in icon for the icon to be displayed in the footer area of the
		/// dialog box. If this is set to none and the CustomFooterIcon member is null then no
		/// footer icon will be displayed.
		/// </summary>
		public TaskDialogIcon FooterIcon
		{
			get => footerIcon;
            set => footerIcon = value;
        }

		/// <summary>
		/// Specifies a custom icon for the icon to be displayed in the footer area of the
		/// dialog box. If this is set to none and the CustomFooterIcon member is null then no
		/// footer icon will be displayed.
		/// </summary>
		public Icon CustomFooterIcon
		{
			get => customFooterIcon;
            set => customFooterIcon = value;
        }

		/// <summary>
		/// Specifies the custom push buttons to display in the dialog. Use CommonButtons member for
		/// common buttons; OK, Yes, No, Retry and Cancel, and Buttons when you want different text
		/// on the push buttons.
		/// </summary>
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")] // Style of use is like single value. Array is of value types.
		[SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")] // Returns a reference, not a copy.
		public TaskDialogButton[] Buttons
		{
			get => buttons;
            set
			{
                buttons = value ?? throw new ArgumentNullException(nameof(value));
			}
		}

		/// <summary>
		/// Specifies the radio buttons to display in the dialog.
		/// </summary>
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")] // Style of use is like single value. Array is of value types.
		[SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")] // Returns a reference, not a copy.
		public TaskDialogButton[] RadioButtons
		{
			get => radioButtons;

            set
			{
                radioButtons = value ?? throw new ArgumentNullException(nameof(value));
			}
		}

		/// <summary>
		/// Enables hyperlink processing for the strings specified in the Content, ExpandedInformation
		/// and FooterText members. When enabled, these members may be strings that contain hyperlinks
		/// in the form: <A HREF="executablestring">Hyperlink Text</A>. 
		/// WARNING: Enabling hyperlinks when using content from an unsafe source may cause security vulnerabilities.
		/// Note: Task Dialog will not actually execute any hyperlinks. Hyperlink execution must be handled
		/// in the callback function specified by Callback member.
		/// </summary>
		public bool EnableHyperlinks
		{
			get => (flags & TASKDIALOG_FLAGS.TDF_ENABLE_HYPERLINKS) != 0;
            set => SetFlag(TASKDIALOG_FLAGS.TDF_ENABLE_HYPERLINKS, value);
        }

		/// <summary>
		/// Indicates that the dialog should be able to be closed using Alt-F4, Escape and the title bar’s
		/// close button even if no cancel button is specified in either the CommonButtons or Buttons members.
		/// </summary>
		public bool AllowDialogCancellation
		{
			get => (flags & TASKDIALOG_FLAGS.TDF_ALLOW_DIALOG_CANCELLATION) != 0;
            set => SetFlag(TASKDIALOG_FLAGS.TDF_ALLOW_DIALOG_CANCELLATION, value);
        }

		/// <summary>
		/// Indicates that the buttons specified in the Buttons member should be displayed as command links
		/// (using a standard task dialog glyph) instead of push buttons.  When using command links, all
		/// characters up to the first new line character in the ButtonText member (of the TaskDialogButton
		/// structure) will be treated as the command link’s main text, and the remainder will be treated
		/// as the command link’s note. This flag is ignored if the Buttons member has no entires.
		/// </summary>
		public bool UseCommandLinks
		{
			get => (flags & TASKDIALOG_FLAGS.TDF_USE_COMMAND_LINKS) != 0;
            set => SetFlag(TASKDIALOG_FLAGS.TDF_USE_COMMAND_LINKS, value);
        }

		/// <summary>
		/// Indicates that the buttons specified in the Buttons member should be displayed as command links
		/// (without a glyph) instead of push buttons. When using command links, all characters up to the
		/// first new line character in the ButtonText member (of the TaskDialogButton structure) will be
		/// treated as the command link’s main text, and the remainder will be treated as the command link’s
		/// note. This flag is ignored if the Buttons member has no entires.
		/// </summary>
		public bool UseCommandLinksNoIcon
		{
			get => (flags & TASKDIALOG_FLAGS.TDF_USE_COMMAND_LINKS_NO_ICON) != 0;
            set => SetFlag(TASKDIALOG_FLAGS.TDF_USE_COMMAND_LINKS_NO_ICON, value);
        }

		/// <summary>
		/// Indicates that the string specified by the ExpandedInformation member should be displayed at the
		/// bottom of the dialog’s footer area instead of immediately after the dialog’s content. This flag
		/// is ignored if the ExpandedInformation member is null.
		/// </summary>
		public bool ExpandFooterArea
		{
			get => (flags & TASKDIALOG_FLAGS.TDF_EXPAND_FOOTER_AREA) != 0;
            set => SetFlag(TASKDIALOG_FLAGS.TDF_EXPAND_FOOTER_AREA, value);
        }

		/// <summary>
		/// Indicates that the string specified by the ExpandedInformation member should be displayed
		/// when the dialog is initially displayed. This flag is ignored if the ExpandedInformation member
		/// is null.
		/// </summary>
		public bool ExpandedByDefault
		{
			get => (flags & TASKDIALOG_FLAGS.TDF_EXPANDED_BY_DEFAULT) != 0;
            set => SetFlag(TASKDIALOG_FLAGS.TDF_EXPANDED_BY_DEFAULT, value);
        }

		/// <summary>
		/// Indicates that the verification checkbox in the dialog should be checked when the dialog is
		/// initially displayed. This flag is ignored if the VerificationText parameter is null.
		/// </summary>
		public bool VerificationFlagChecked
		{
			get => (flags & TASKDIALOG_FLAGS.TDF_VERIFICATION_FLAG_CHECKED) != 0;
            set => SetFlag(TASKDIALOG_FLAGS.TDF_VERIFICATION_FLAG_CHECKED, value);
        }

		/// <summary>
		/// Indicates that a Progress Bar should be displayed.
		/// </summary>
		public bool ShowProgressBar
		{
			get => (flags & TASKDIALOG_FLAGS.TDF_SHOW_PROGRESS_BAR) != 0;
            set => SetFlag(TASKDIALOG_FLAGS.TDF_SHOW_PROGRESS_BAR, value);
        }

		/// <summary>
		/// Indicates that an Marquee Progress Bar should be displayed.
		/// </summary>
		public bool ShowMarqueeProgressBar
		{
			get => (flags & TASKDIALOG_FLAGS.TDF_SHOW_MARQUEE_PROGRESS_BAR) != 0;
            set => SetFlag(TASKDIALOG_FLAGS.TDF_SHOW_MARQUEE_PROGRESS_BAR, value);
        }

		/// <summary>
		/// Indicates that the TaskDialog’s callback should be called approximately every 200 milliseconds.
		/// </summary>
		public bool CallbackTimer
		{
			get => (flags & TASKDIALOG_FLAGS.TDF_CALLBACK_TIMER) != 0;
            set => SetFlag(TASKDIALOG_FLAGS.TDF_CALLBACK_TIMER, value);
        }

		/// <summary>
		/// Indicates that the TaskDialog should be positioned (centered) relative to the owner window
		/// passed when calling Show. If not set (or no owner window is passed), the TaskDialog is
		/// positioned (centered) relative to the monitor.
		/// </summary>
		public bool PositionRelativeToWindow
		{
			get => (flags & TASKDIALOG_FLAGS.TDF_POSITION_RELATIVE_TO_WINDOW) != 0;
            set => SetFlag(TASKDIALOG_FLAGS.TDF_POSITION_RELATIVE_TO_WINDOW, value);
        }

		/// <summary>
		/// Indicates that the TaskDialog should have right to left layout.
		/// </summary>
		public bool RightToLeftLayout
		{
			get => (flags & TASKDIALOG_FLAGS.TDF_RTL_LAYOUT) != 0;
            set => SetFlag(TASKDIALOG_FLAGS.TDF_RTL_LAYOUT, value);
        }

		/// <summary>
		/// Indicates that the TaskDialog should have no default radio button.
		/// </summary>
		public bool NoDefaultRadioButton
		{
			get => (flags & TASKDIALOG_FLAGS.TDF_NO_DEFAULT_RADIO_BUTTON) != 0;
            set => SetFlag(TASKDIALOG_FLAGS.TDF_NO_DEFAULT_RADIO_BUTTON, value);
        }

		/// <summary>
		/// Indicates that the TaskDialog can be minimised. Works only if there if parent window is null. Will enable cancellation also.
		/// </summary>
		public bool CanBeMinimized
		{
			get => (flags & TASKDIALOG_FLAGS.TDF_CAN_BE_MINIMIZED) != 0;
            set => SetFlag(TASKDIALOG_FLAGS.TDF_CAN_BE_MINIMIZED, value);
        }

		/// <summary>
		/// Indicates the default button for the dialog. This may be any of the values specified
		/// in ButtonId members of one of the TaskDialogButton structures in the Buttons array,
		/// or one a DialogResult value that corresponds to a buttons specified in the CommonButtons Member.
		/// If this member is zero or its value does not correspond to any button ID in the dialog,
		/// then the first button in the dialog will be the default. 
		/// </summary>
		public int DefaultButton
		{
			get => defaultButton;
            set => defaultButton = value;
        }

		/// <summary>
		/// Indicates the default radio button for the dialog. This may be any of the values specified
		/// in ButtonId members of one of the TaskDialogButton structures in the RadioButtons array.
		/// If this member is zero or its value does not correspond to any radio button ID in the dialog,
		/// then the first button in RadioButtons will be the default.
		/// The property NoDefaultRadioButton can be set to have no default.
		/// </summary>
		public int DefaultRadioButton
		{
			get => defaultRadioButton;
            set => defaultRadioButton = value;
        }

		/// <summary>
		/// The string to be used to label the verification checkbox. If this member is null, the
		/// verification checkbox is not displayed in the dialog box.
		/// </summary>
		public string VerificationText
		{
			get => verificationText;
            set => verificationText = value;
        }

		/// <summary>
		/// The string to be used for displaying additional information. The additional information is
		/// displayed either immediately below the content or below the footer text depending on whether
		/// the ExpandFooterArea member is true. If the EnameHyperlinks member is true, then this string
		/// may contain hyperlinks in the form: <A HREF="executablestring">Hyperlink Text</A>.
		/// WARNING: Enabling hyperlinks when using content from an unsafe source may cause security vulnerabilities.
		/// </summary>
		public string ExpandedInformation
		{
			get => expandedInformation;
            set => expandedInformation = value;
        }

		/// <summary>
		/// The string to be used to label the button for collapsing the expanded information. This
		/// member is ignored when the ExpandedInformation member is null. If this member is null
		/// and the CollapsedControlText is specified, then the CollapsedControlText value will be
		/// used for this member as well.
		/// </summary>
		public string ExpandedControlText
		{
			get => expandedControlText;
            set => expandedControlText = value;
        }

		/// <summary>
		/// The string to be used to label the button for expanding the expanded information. This
		/// member is ignored when the ExpandedInformation member is null.  If this member is null
		/// and the ExpandedControlText is specified, then the ExpandedControlText value will be
		/// used for this member as well.
		/// </summary>
		public string CollapsedControlText
		{
			get => collapsedControlText;
            set => collapsedControlText = value;
        }

		/// <summary>
		/// The string to be used in the footer area of the dialog box. If the EnableHyperlinks member
		/// is true, then this string may contain hyperlinks in the form: <A HREF="executablestring">
		/// Hyperlink Text</A>.
		/// WARNING: Enabling hyperlinks when using content from an unsafe source may cause security vulnerabilities.
		/// </summary>
		public string Footer
		{
			get => footer;
            set => footer = value;
        }

		/// <summary>
		/// width of the Task Dialog's client area in DLU's. If 0, Task Dialog will calculate the ideal width.
		/// </summary>
		public uint Width
		{
			get => width;
            set => width = value;
        }

		/// <summary>
		/// The callback that receives messages from the Task Dialog when various events occur.
		/// </summary>
		public TaskDialogCallback Callback
		{
			get => callback;
            set => callback = value;
        }

		/// <summary>
		/// Reference that is passed to the callback.
		/// </summary>
		public object CallbackData
		{
			get => callbackData;
            set => callbackData = value;
        }

		internal TaskDialogOptions Config
		{
			get => config;
            set => config = value;
        }

		/// <summary>
		/// Resets the Task Dialog to the state when first constructed, all properties set to their default value.
		/// </summary>
		public void Reset()
		{
			windowTitle = null;
			mainInstruction = null;
			content = null;
			commonButtons = 0;
			mainIcon = TaskDialogIcon.None;
			customMainIcon = null;
			footerIcon = TaskDialogIcon.None;
			customFooterIcon = null;
			buttons = new TaskDialogButton[0];
			radioButtons = new TaskDialogButton[0];
			flags = 0;
			defaultButton = 0;
			defaultRadioButton = 0;
			verificationText = null;
			expandedInformation = null;
			expandedControlText = null;
			collapsedControlText = null;
			footer = null;
			callback = null;
			callbackData = null;
			width = 0;
		}

		/// <summary>
		/// Creates, displays, and operates a task dialog. The task dialog contains application-defined messages, title,
		/// verification check box, command links and push buttons, plus any combination of predefined icons and push buttons
		/// as specified on the other members of the class before calling Show.
		/// </summary>
		/// <returns>The result of the dialog, either a DialogResult value for common push buttons set in the CommonButtons
		/// member or the ButtonID from a TaskDialogButton structure set on the Buttons member.</returns>
		public int Show()
		{
            return Show(IntPtr.Zero, out bool _, out int _);
		}
		/// <summary>
		/// Creates, displays, and operates a task dialog. The task dialog contains application-defined messages, title,
		/// verification check box, command links and push buttons, plus any combination of predefined icons and push buttons
		/// as specified on the other members of the class before calling Show.
		/// </summary>
		/// <param name="owner">Owner window the task Dialog will modal to.</param>
		/// <returns>The result of the dialog, either a DialogResult value for common push buttons set in the CommonButtons
		/// member or the ButtonID from a TaskDialogButton structure set on the Buttons member.</returns>
		public int Show(System.Windows.Window owner)
		{
            return Show((owner == null ? IntPtr.Zero : new System.Windows.Interop.WindowInteropHelper(owner).Handle), out bool _, out int _);
		}
		/// <summary>
		/// Creates, displays, and operates a task dialog. The task dialog contains application-defined messages, title,
		/// verification check box, command links and push buttons, plus any combination of predefined icons and push buttons
		/// as specified on the other members of the class before calling Show.
		/// </summary>
		/// <param name="hwndOwner">Owner window the task Dialog will modal to.</param>
		/// <returns>The result of the dialog, either a DialogResult value for common push buttons set in the CommonButtons
		/// member or the ButtonID from a TaskDialogButton structure set on the Buttons member.</returns>
		public int Show(IntPtr hwndOwner)
		{
            return Show(hwndOwner, out bool _, out int _);
		}
		/// <summary>
		/// Creates, displays, and operates a task dialog. The task dialog contains application-defined messages, title,
		/// verification check box, command links and push buttons, plus any combination of predefined icons and push buttons
		/// as specified on the other members of the class before calling Show.
		/// </summary>
		/// <param name="owner">Owner window the task Dialog will modal to.</param>
		/// <param name="verificationFlagChecked">Returns true if the verification checkbox was checked when the dialog
		/// was dismissed.</param>
		/// <returns>The result of the dialog, either a DialogResult value for common push buttons set in the CommonButtons
		/// member or the ButtonID from a TaskDialogButton structure set on the Buttons member.</returns>
		public int Show(System.Windows.Window owner, out bool verificationFlagChecked)
		{
            return Show((owner == null ? IntPtr.Zero : new System.Windows.Interop.WindowInteropHelper(owner).Handle), out verificationFlagChecked, out int _);
		}
		/// <summary>
		/// Creates, displays, and operates a task dialog. The task dialog contains application-defined messages, title,
		/// verification check box, command links and push buttons, plus any combination of predefined icons and push buttons
		/// as specified on the other members of the class before calling Show.
		/// </summary>
		/// <param name="hwndOwner">Owner window the task Dialog will modal to.</param>
		/// <param name="verificationFlagChecked">Returns true if the verification checkbox was checked when the dialog
		/// was dismissed.</param>
		/// <returns>The result of the dialog, either a DialogResult value for common push buttons set in the CommonButtons
		/// member or the ButtonID from a TaskDialogButton structure set on the Buttons member.</returns>
		public int Show(IntPtr hwndOwner, out bool verificationFlagChecked)
		{
			// We have to call a private version or PreSharp gets upset about a unsafe
			// block in a public method. (PreSharp error 56505)
            return PrivateShow(hwndOwner, out verificationFlagChecked, out int _);
		}

		/// <summary>
		/// Creates, displays, and operates a task dialog. The task dialog contains application-defined messages, title,
		/// verification check box, command links and push buttons, plus any combination of predefined icons and push buttons
		/// as specified on the other members of the class before calling Show.
		/// </summary>
		/// <param name="owner">Owner window the task Dialog will modal to.</param>
		/// <param name="verificationFlagChecked">Returns true if the verification checkbox was checked when the dialog
		/// was dismissed.</param>
		/// <param name="radioButtonResult">The radio botton selected by the user.</param>
		/// <returns>The result of the dialog, either a DialogResult value for common push buttons set in the CommonButtons
		/// member or the ButtonID from a TaskDialogButton structure set on the Buttons member.</returns>
		public int Show(System.Windows.Window owner, out bool verificationFlagChecked, out int radioButtonResult)
		{
			return Show((owner == null ? IntPtr.Zero : new System.Windows.Interop.WindowInteropHelper(owner).Handle), out verificationFlagChecked, out radioButtonResult);
		}

		/// <summary>
		/// Creates, displays, and operates a task dialog. The task dialog contains application-defined messages, title,
		/// verification check box, command links and push buttons, plus any combination of predefined icons and push buttons
		/// as specified on the other members of the class before calling Show.
		/// </summary>
		/// <param name="hwndOwner">Owner window the task Dialog will modal to.</param>
		/// <param name="verificationFlagChecked">Returns true if the verification checkbox was checked when the dialog
		/// was dismissed.</param>
		/// <param name="radioButtonResult">The radio botton selected by the user.</param>
		/// <returns>The result of the dialog, either a DialogResult value for common push buttons set in the CommonButtons
		/// member or the ButtonID from a TaskDialogButton structure set on the Buttons member.</returns>
		public int Show(IntPtr hwndOwner, out bool verificationFlagChecked, out int radioButtonResult)
		{
			// We have to call a private version or PreSharp gets upset about a unsafe
			// block in a public method. (PreSharp error 56505)
			return PrivateShow(hwndOwner, out verificationFlagChecked, out radioButtonResult);
		}

		/// <summary>
		/// Creates, displays, and operates a task dialog. The task dialog contains application-defined messages, title,
		/// verification check box, command links and push buttons, plus any combination of predefined icons and push buttons
		/// as specified on the other members of the class before calling Show.
		/// </summary>
		/// <param name="hwndOwner">Owner window the task Dialog will modal to.</param>
		/// <param name="verificationFlagChecked">Returns true if the verification checkbox was checked when the dialog
		/// was dismissed.</param>
		/// <param name="radioButtonResult">The radio botton selected by the user.</param>
		/// <returns>The result of the dialog, either a DialogResult value for common push buttons set in the CommonButtons
		/// member or the ButtonID from a TaskDialogButton structure set on the Buttons member.</returns>
		private int PrivateShow(IntPtr hwndOwner, out bool verificationFlagChecked, out int radioButtonResult)
		{
			verificationFlagChecked = false;
			radioButtonResult = 0;
			int result;
			TASKDIALOGCONFIG cfg = new TASKDIALOGCONFIG();

			try
			{
				cfg.cbSize = (uint)Marshal.SizeOf(typeof(TASKDIALOGCONFIG));
				cfg.hwndParent = hwndOwner;
				cfg.dwFlags = flags;
				cfg.dwCommonButtons = commonButtons;

				if (!string.IsNullOrEmpty(windowTitle))
				{
					cfg.pszWindowTitle = windowTitle;
				}

				cfg.MainIcon = (IntPtr)mainIcon;
				if (customMainIcon != null)
				{
					cfg.dwFlags |= TASKDIALOG_FLAGS.TDF_USE_HICON_MAIN;
					cfg.MainIcon = customMainIcon.Handle;
				}

				if (!string.IsNullOrEmpty(mainInstruction))
				{
					cfg.pszMainInstruction = mainInstruction;
				}

				if (!string.IsNullOrEmpty(content))
				{
					cfg.pszContent = content;
				}

				TaskDialogButton[] customButtons = buttons;
				if (customButtons.Length > 0)
				{
					// Hand marshal the buttons array.
					int elementSize = Marshal.SizeOf(typeof(TaskDialogButton));
					cfg.pButtons = Marshal.AllocHGlobal(elementSize * customButtons.Length);
					for (int i = 0; i < customButtons.Length; i++)
					{
                        unsafe // Unsafe because of pointer arithmatic.
                        {
                            byte* p = (byte*)cfg.pButtons;
                            Marshal.StructureToPtr(customButtons[i], (IntPtr)(p + (elementSize * i)), false);
                        }

                        cfg.cButtons++;
					}
				}

				TaskDialogButton[] customRadioButtons = radioButtons;
				if (customRadioButtons.Length > 0)
				{
					// Hand marshal the buttons array.
					int elementSize = Marshal.SizeOf(typeof(TaskDialogButton));
					cfg.pRadioButtons = Marshal.AllocHGlobal(elementSize * customRadioButtons.Length);
					for (int i = 0; i < customRadioButtons.Length; i++)
					{
                        unsafe // Unsafe because of pointer arithmatic.
                        {
                            byte* p = (byte*)cfg.pRadioButtons;
                            Marshal.StructureToPtr(customRadioButtons[i], (IntPtr)(p + (elementSize * i)), false);
                        }

                        cfg.cRadioButtons++;
					}
				}

				cfg.nDefaultButton = defaultButton;
				cfg.nDefaultRadioButton = defaultRadioButton;

				if (!string.IsNullOrEmpty(verificationText))
				{
					cfg.pszVerificationText = verificationText;
				}

				if (!string.IsNullOrEmpty(expandedInformation))
				{
					cfg.pszExpandedInformation = expandedInformation;
				}

				if (!string.IsNullOrEmpty(expandedControlText))
				{
					cfg.pszExpandedControlText = expandedControlText;
				}

				if (!string.IsNullOrEmpty(collapsedControlText))
				{
					cfg.pszCollapsedControlText = CollapsedControlText;
				}

				cfg.FooterIcon = (IntPtr)footerIcon;
				if (customFooterIcon != null)
				{
					cfg.dwFlags |= TASKDIALOG_FLAGS.TDF_USE_HICON_FOOTER;
					cfg.FooterIcon = customFooterIcon.Handle;
				}

				if (!string.IsNullOrEmpty(footer))
				{
					cfg.pszFooter = footer;
				}

				// If our user has asked for a callback then we need to ask for one to
				// translate to the friendly version.
				if (callback != null)
				{
					cfg.pfCallback = PrivateCallback;
				}

				////config.lpCallbackData = this.callbackData; // How do you do this? Need to pin the ref?
				cfg.cxWidth = width;

				// The call all this mucking about is here for.
				UnsafeNativeMethods.TaskDialogIndirect(ref cfg, out result, out radioButtonResult, out verificationFlagChecked);
			}
			finally
			{
				// Free the unmanged memory needed for the button arrays.
				// There is the possiblity of leaking memory if the app-domain is destroyed in a non clean way
				// and the hosting OS process is kept alive but fixing this would require using hardening techniques
				// that are not required for the users of this class.
				if (cfg.pButtons != IntPtr.Zero)
				{
					int elementSize = Marshal.SizeOf(typeof(TaskDialogButton));
					for (int i = 0; i < cfg.cButtons; i++)
					{
                        unsafe
                        {
                            byte* p = (byte*)cfg.pButtons;
                            Marshal.DestroyStructure((IntPtr)(p + (elementSize * i)), typeof(TaskDialogButton));
                        }
					}

					Marshal.FreeHGlobal(cfg.pButtons);
				}

				if (cfg.pRadioButtons != IntPtr.Zero)
				{
					int elementSize = Marshal.SizeOf(typeof(TaskDialogButton));
					for (int i = 0; i < cfg.cRadioButtons; i++)
					{
                        unsafe
                        {
                            byte* p = (byte*)cfg.pRadioButtons;
                            Marshal.DestroyStructure((IntPtr)(p + (elementSize * i)), typeof(TaskDialogButton));
                        }
					}

					Marshal.FreeHGlobal(cfg.pRadioButtons);
				}
			}

			return result;
		}

		/// <summary>
		/// The callback from the native Task Dialog. This prepares the friendlier arguments and calls the simplier callback.
		/// </summary>
		/// <param name="hwnd">The window handle of the Task Dialog that is active.</param>
		/// <param name="msg">The notification. A TaskDialogNotification value.</param>
		/// <param name="wparam">Specifies additional noitification information.  The contents of this parameter depends on the value of the msg parameter.</param>
		/// <param name="lparam">Specifies additional noitification information.  The contents of this parameter depends on the value of the msg parameter.</param>
		/// <param name="refData">Specifies the application-defined value given in the call to TaskDialogIndirect.</param>
		/// <returns>A HRESULT. It's not clear in the spec what a failed result will do.</returns>
		private int PrivateCallback([In] IntPtr hwnd, [In] uint msg, [In] UIntPtr wparam, [In] IntPtr lparam, [In] IntPtr refData)
		{
			TaskDialogCallback cbk = this.callback;
			if (cbk != null)
			{
				// Prepare arguments for the callback to the user we are insulating from Interop casting sillyness.

				// Future: Consider reusing a single ActiveTaskDialog object and mark it as destroyed on the destry notification.
				ActiveTaskDialog activeDialog = new ActiveTaskDialog(hwnd);
                TaskDialogNotificationArgs args = new TaskDialogNotificationArgs
                {
                    Config = config, Notification = (TaskDialogNotification) msg
                };
                switch (args.Notification)
				{
					case TaskDialogNotification.ButtonClicked:
					case TaskDialogNotification.RadioButtonClicked:
						args.ButtonId = (int)wparam;

						// The index, ideally, should be -1 or something whenever the
						//dialog was closed by non-common-button means such as Alt+F4
						//or using the Close action on the System menu or the red X
						
						// I can, with little trouble, detect this for the emulated dialog,
						//however the native dialog gives me no indication and in fact
						//simply reports a buttonId of 2 (Cancel) regardless of whether
						//the actual Cancel button was used or one of the above alt methods.

						// If I could hook into the native dialogs messages and detect:
						// WM_SYSCOMMAND with WParam of SC_CLOSE
						// ...then I could tell for sure, but I'm not sure how to listen
						//in on its messages. My Win32-fu not good enough.

						// For now, I will have the emulated dialog simply pretend like it
						//cannot tell either until I can figure out a way to determine it
						//with the native dialog, too.

						if (args.ButtonId > 100)
							args.ButtonIndex = args.ButtonId % TaskDialog.CUSTOM_BUTTON_ID_OFFSET;
						else
							args.ButtonIndex = TaskDialog.GetButtonIndexForCommonButton(args.Config.CommonButtons, args.ButtonId);
						break;
					case TaskDialogNotification.HyperlinkClicked:
						args.Hyperlink = Marshal.PtrToStringUni(lparam);
						break;
					case TaskDialogNotification.Timer:
						args.TimerTickCount = (uint)wparam;
						break;
					case TaskDialogNotification.VerificationClicked:
						args.VerificationFlagChecked = (wparam != UIntPtr.Zero);
						break;
					case TaskDialogNotification.ExpandoButtonClicked:
						args.Expanded = (wparam != UIntPtr.Zero);
						break;
				}

				bool result = cbk(activeDialog, args, callbackData);

				return (result ? 1 : 0);
			}

			return 0; // false;
		}

		/// <summary>
		/// Helper function to set or clear a bit in the flags field.
		/// </summary>
		/// <param name="flag">The Flag bit to set or clear.</param>
		/// <param name="value">True to set, false to clear the bit in the flags field.</param>
		private void SetFlag(TASKDIALOG_FLAGS flag, bool value)
		{
			if (value)
			{
				flags |= flag;
			}
			else
			{
				flags &= ~flag;
			}
		}
	}
}
