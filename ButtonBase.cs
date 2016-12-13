// Decompiled with JetBrains decompiler
// Type: System.Windows.Controls.Primitives.ButtonBase
// Assembly: PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 52A06291-BD08-4D35-BA33-FF8E851C715F
// Assembly location: C:\Windows\Microsoft.NET\assembly\GAC_MSIL\PresentationFramework\v4.0_4.0.0.0__31bf3856ad364e35\PresentationFramework.dll

using MS.Internal.Commands;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;
using System.ComponentModel;
using System.Security;
using System.Windows.Input;
using System.Windows.Media;

namespace System.Windows.Controls.Primitives
{
  /// <summary>Represents the base class for all <see cref="T:System.Windows.Controls.Button" /> controls. </summary>
  [DefaultEvent("Click")]
  [Localizability(LocalizationCategory.Button)]
  public abstract class ButtonBase : ContentControl, ICommandSource
  {
    /// <summary>Identifies the <see cref="E:System.Windows.Controls.Primitives.ButtonBase.Click" /> routed event. </summary>
    /// <returns>The identifier for the <see cref="E:System.Windows.Controls.Primitives.ButtonBase.Click" /> routed event.</returns>
    public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (ButtonBase));
    /// <summary>Identifies the routed <see cref="P:System.Windows.Controls.Primitives.ButtonBase.Command" /> dependency property. </summary>
    /// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.ButtonBase.Command" /> dependency property.</returns>
    [CommonDependencyProperty]
    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof (ICommand), typeof (ButtonBase), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, new PropertyChangedCallback(ButtonBase.OnCommandChanged)));
    /// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.ButtonBase.CommandParameter" /> dependency property. </summary>
    /// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.ButtonBase.CommandParameter" /> dependency property.</returns>
    [CommonDependencyProperty]
    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof (object), typeof (ButtonBase), (PropertyMetadata) new FrameworkPropertyMetadata((object) null));
    /// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.ButtonBase.CommandTarget" /> dependency property. </summary>
    /// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.ButtonBase.CommandTarget" /> dependency property.</returns>
    [CommonDependencyProperty]
    public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register("CommandTarget", typeof (IInputElement), typeof (ButtonBase), (PropertyMetadata) new FrameworkPropertyMetadata((object) null));
    internal static readonly DependencyPropertyKey IsPressedPropertyKey = DependencyProperty.RegisterReadOnly("IsPressed", typeof (bool), typeof (ButtonBase), (PropertyMetadata) new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, new PropertyChangedCallback(ButtonBase.OnIsPressedChanged)));
    /// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.ButtonBase.IsPressed" /> dependency property. </summary>
    /// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.ButtonBase.IsPressed" /> dependency property.</returns>
    [CommonDependencyProperty]
    public static readonly DependencyProperty IsPressedProperty = ButtonBase.IsPressedPropertyKey.DependencyProperty;
    /// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.ButtonBase.ClickMode" /> dependency property. </summary>
    /// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.ButtonBase.ClickMode" /> dependency property.</returns>
    public static readonly DependencyProperty ClickModeProperty = DependencyProperty.Register("ClickMode", typeof (ClickMode), typeof (ButtonBase), (PropertyMetadata) new FrameworkPropertyMetadata((object) ClickMode.Release), new ValidateValueCallback(ButtonBase.IsValidClickMode));

    private bool IsInMainFocusScope
    {
      get
      {
        Visual focusScope = FocusManager.GetFocusScope((DependencyObject) this) as Visual;
        if (focusScope != null)
          return VisualTreeHelper.GetParent((DependencyObject) focusScope) == null;
        return true;
      }
    }

    /// <summary>Gets a value that indicates whether a <see cref="T:System.Windows.Controls.Primitives.ButtonBase" /> is currently activated.  </summary>
    /// <returns>true if the <see cref="T:System.Windows.Controls.Primitives.ButtonBase" /> is activated; otherwise false. The default is false.</returns>
    [Browsable(false)]
    [Category("Appearance")]
    [ReadOnly(true)]
    public bool IsPressed
    {
      get
      {
        return (bool) this.GetValue(ButtonBase.IsPressedProperty);
      }
      protected set
      {
        this.SetValue(ButtonBase.IsPressedPropertyKey, BooleanBoxes.Box(value));
      }
    }

    /// <summary>Gets or sets the command to invoke when this button is pressed.  </summary>
    /// <returns>A command to invoke when this button is pressed. The default value is null.</returns>
    [Bindable(true)]
    [Category("Action")]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public ICommand Command
    {
      get
      {
        return (ICommand) this.GetValue(ButtonBase.CommandProperty);
      }
      set
      {
        this.SetValue(ButtonBase.CommandProperty, (object) value);
      }
    }

    /// <summary>Gets the value of the <see cref="P:System.Windows.ContentElement.IsEnabled" /> property.</summary>
    /// <returns>true if the control is enabled; otherwise, false.</returns>
    protected override bool IsEnabledCore
    {
      get
      {
        if (base.IsEnabledCore)
          return this.CanExecute;
        return false;
      }
    }

    /// <summary>Gets or sets the parameter to pass to the <see cref="P:System.Windows.Controls.Primitives.ButtonBase.Command" /> property.  </summary>
    /// <returns>Parameter to pass to the <see cref="P:System.Windows.Controls.Primitives.ButtonBase.Command" /> property.</returns>
    [Bindable(true)]
    [Category("Action")]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public object CommandParameter
    {
      get
      {
        return this.GetValue(ButtonBase.CommandParameterProperty);
      }
      set
      {
        this.SetValue(ButtonBase.CommandParameterProperty, value);
      }
    }

    /// <summary>Gets or sets the element on which to raise the specified command.  </summary>
    /// <returns>Element on which to raise a command.</returns>
    [Bindable(true)]
    [Category("Action")]
    public IInputElement CommandTarget
    {
      get
      {
        return (IInputElement) this.GetValue(ButtonBase.CommandTargetProperty);
      }
      set
      {
        this.SetValue(ButtonBase.CommandTargetProperty, (object) value);
      }
    }

    /// <summary>Gets or sets when the <see cref="E:System.Windows.Controls.Primitives.ButtonBase.Click" /> event occurs.  </summary>
    /// <returns>When the <see cref="E:System.Windows.Controls.Primitives.ButtonBase.Click" /> event occurs. The default value is <see cref="F:System.Windows.Controls.ClickMode.Release" />. </returns>
    [Bindable(true)]
    [Category("Behavior")]
    public ClickMode ClickMode
    {
      get
      {
        return (ClickMode) this.GetValue(ButtonBase.ClickModeProperty);
      }
      set
      {
        this.SetValue(ButtonBase.ClickModeProperty, (object) value);
      }
    }

    private bool IsSpaceKeyDown
    {
      get
      {
        return this.ReadControlFlag(Control.ControlBoolFlags.IsSpaceKeyDown);
      }
      set
      {
        this.WriteControlFlag(Control.ControlBoolFlags.IsSpaceKeyDown, value);
      }
    }

    private bool CanExecute
    {
      get
      {
        return !this.ReadControlFlag(Control.ControlBoolFlags.CommandDisabled);
      }
      set
      {
        if (value == this.CanExecute)
          return;
        this.WriteControlFlag(Control.ControlBoolFlags.CommandDisabled, !value);
        this.CoerceValue(UIElement.IsEnabledProperty);
      }
    }

    /// <summary>Occurs when a <see cref="T:System.Windows.Controls.Button" /> is clicked. </summary>
    [Category("Behavior")]
    public event RoutedEventHandler Click
    {
      add
      {
        this.AddHandler(ButtonBase.ClickEvent, (Delegate) value);
      }
      remove
      {
        this.RemoveHandler(ButtonBase.ClickEvent, (Delegate) value);
      }
    }

    static ButtonBase()
    {
      EventManager.RegisterClassHandler(typeof (ButtonBase), AccessKeyManager.AccessKeyPressedEvent, (Delegate) new AccessKeyPressedEventHandler(ButtonBase.OnAccessKeyPressed));
      KeyboardNavigation.AcceptsReturnProperty.OverrideMetadata(typeof (ButtonBase), (PropertyMetadata) new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));
      InputMethod.IsInputMethodEnabledProperty.OverrideMetadata(typeof (ButtonBase), (PropertyMetadata) new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));
      UIElement.IsMouseOverPropertyKey.OverrideMetadata(typeof (ButtonBase), (PropertyMetadata) new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
      UIElement.IsEnabledProperty.OverrideMetadata(typeof (ButtonBase), (PropertyMetadata) new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
    }

    /// <summary>Raises the <see cref="E:System.Windows.Controls.Primitives.ButtonBase.Click" /> routed event. </summary>
    protected virtual void OnClick()
    {
      this.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent, (object) this));
      CommandHelpers.ExecuteCommandSource((ICommandSource) this);
    }

    /// <summary>Called when the <see cref="P:System.Windows.Controls.Primitives.ButtonBase.IsPressed" /> property changes.</summary>
    /// <param name="e">The data for <see cref="T:System.Windows.DependencyPropertyChangedEventArgs" />.</param>
    protected virtual void OnIsPressedChanged(DependencyPropertyChangedEventArgs e)
    {
      Control.OnVisualStatePropertyChanged((DependencyObject) this, e);
    }

    internal void AutomationButtonBaseClick()
    {
      this.OnClick();
    }

    private static bool IsValidClickMode(object o)
    {
      ClickMode clickMode = (ClickMode) o;
      switch (clickMode)
      {
        case ClickMode.Press:
        case ClickMode.Release:
          return true;
        default:
          return clickMode == ClickMode.Hover;
      }
    }

    /// <summary> Called when the rendered size of a control changes. </summary>
    /// <param name="sizeInfo">Specifies the size changes.</param>
    protected internal override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
      base.OnRenderSizeChanged(sizeInfo);
      if (!this.IsMouseCaptured || Mouse.PrimaryDevice.LeftButton != MouseButtonState.Pressed || this.IsSpaceKeyDown)
        return;
      this.UpdateIsPressed();
    }

    private static void OnIsPressedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((ButtonBase) d).OnIsPressedChanged(e);
    }

    private static void OnAccessKeyPressed(object sender, AccessKeyPressedEventArgs e)
    {
      if (e.Handled || e.Scope != null || e.Target != null)
        return;
      e.Target = (UIElement) sender;
    }

    private void UpdateIsPressed()
    {
      Point position = Mouse.PrimaryDevice.GetPosition((IInputElement) this);
      if (position.X >= 0.0 && position.X <= this.ActualWidth && (position.Y >= 0.0 && position.Y <= this.ActualHeight))
      {
        if (this.IsPressed)
          return;
        this.SetIsPressed(true);
      }
      else
      {
        if (!this.IsPressed)
          return;
        this.SetIsPressed(false);
      }
    }

    private void SetIsPressed(bool pressed)
    {
      if (pressed)
        this.SetValue(ButtonBase.IsPressedPropertyKey, BooleanBoxes.Box(pressed));
      else
        this.ClearValue(ButtonBase.IsPressedPropertyKey);
    }

    private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((ButtonBase) d).OnCommandChanged((ICommand) e.OldValue, (ICommand) e.NewValue);
    }

    private void OnCommandChanged(ICommand oldCommand, ICommand newCommand)
    {
      if (oldCommand != null)
        this.UnhookCommand(oldCommand);
      if (newCommand == null)
        return;
      this.HookCommand(newCommand);
    }

    private void UnhookCommand(ICommand command)
    {
      CanExecuteChangedEventManager.RemoveHandler(command, new EventHandler<EventArgs>(this.OnCanExecuteChanged));
      this.UpdateCanExecute();
    }

    private void HookCommand(ICommand command)
    {
      CanExecuteChangedEventManager.AddHandler(command, new EventHandler<EventArgs>(this.OnCanExecuteChanged));
      this.UpdateCanExecute();
    }

    private void OnCanExecuteChanged(object sender, EventArgs e)
    {
      this.UpdateCanExecute();
    }

    private void UpdateCanExecute()
    {
      if (this.Command != null)
        this.CanExecute = CommandHelpers.CanExecuteCommandSource((ICommandSource) this);
      else
        this.CanExecute = true;
    }

    /// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.MouseLeftButtonDown" /> routed event that occurs when the left mouse button is pressed while the mouse pointer is over this control.</summary>
    /// <param name="e">The event data. </param>
    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
      if (this.ClickMode != ClickMode.Hover)
      {
        e.Handled = true;
        this.Focus();
        if (e.ButtonState == MouseButtonState.Pressed)
        {
          this.CaptureMouse();
          if (this.IsMouseCaptured)
          {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
              if (!this.IsPressed)
                this.SetIsPressed(true);
            }
            else
              this.ReleaseMouseCapture();
          }
        }
        if (this.ClickMode == ClickMode.Press)
        {
          bool flag = true;
          try
          {
            this.OnClick();
            flag = false;
          }
          finally
          {
            if (flag)
            {
              this.SetIsPressed(false);
              this.ReleaseMouseCapture();
            }
          }
        }
      }
      base.OnMouseLeftButtonDown(e);
    }

    /// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.MouseLeftButtonUp" /> routed event that occurs when the left mouse button is released while the mouse pointer is over this control. </summary>
    /// <param name="e">The event data.</param>
    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
      if (this.ClickMode != ClickMode.Hover)
      {
        e.Handled = true;
        bool flag = !this.IsSpaceKeyDown && this.IsPressed && this.ClickMode == ClickMode.Release;
        if (this.IsMouseCaptured && !this.IsSpaceKeyDown)
          this.ReleaseMouseCapture();
        if (flag)
          this.OnClick();
      }
      base.OnMouseLeftButtonUp(e);
    }

    /// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.MouseMove" /> routed event that occurs when the mouse pointer moves while over this element.</summary>
    /// <param name="e">The event data.</param>
    protected override void OnMouseMove(MouseEventArgs e)
    {
      base.OnMouseMove(e);
      if (this.ClickMode == ClickMode.Hover || !this.IsMouseCaptured || (Mouse.PrimaryDevice.LeftButton != MouseButtonState.Pressed || this.IsSpaceKeyDown))
        return;
      this.UpdateIsPressed();
      e.Handled = true;
    }

    /// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.LostMouseCapture" /> routed event that occurs when this control is no longer receiving mouse event messages. </summary>
    /// <param name="e">The event data for the <see cref="E:System.Windows.Input.Mouse.LostMouseCapture" /> event.</param>
    protected override void OnLostMouseCapture(MouseEventArgs e)
    {
      base.OnLostMouseCapture(e);
      if (e.OriginalSource != this || this.ClickMode == ClickMode.Hover || this.IsSpaceKeyDown)
        return;
      if (this.IsKeyboardFocused && !this.IsInMainFocusScope)
        Keyboard.Focus((IInputElement) null);
      this.SetIsPressed(false);
    }

    /// <summary>Provides class handling for the <see cref="P:System.Windows.Controls.Primitives.ButtonBase.ClickMode" /> routed event that occurs when the mouse enters this control. </summary>
    /// <param name="e">The event data for the <see cref="E:System.Windows.Input.Mouse.MouseEnter" /> event.</param>
    protected override void OnMouseEnter(MouseEventArgs e)
    {
      base.OnMouseEnter(e);
      if (!this.HandleIsMouseOverChanged())
        return;
      e.Handled = true;
    }

    /// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.MouseLeave" /> routed event that occurs when the mouse leaves an element. </summary>
    /// <param name="e">The event data for the <see cref="E:System.Windows.Input.Mouse.MouseLeave" /> event.</param>
    protected override void OnMouseLeave(MouseEventArgs e)
    {
      base.OnMouseLeave(e);
      if (!this.HandleIsMouseOverChanged())
        return;
      e.Handled = true;
    }

    private bool HandleIsMouseOverChanged()
    {
      if (this.ClickMode != ClickMode.Hover)
        return false;
      if (this.IsMouseOver)
      {
        this.SetIsPressed(true);
        this.OnClick();
      }
      else
        this.SetIsPressed(false);
      return true;
    }

    /// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.KeyDown" /> routed event that occurs when the user presses a key while this control has focus.</summary>
    /// <param name="e">The event data.</param>
    protected override void OnKeyDown(KeyEventArgs e)
    {
      base.OnKeyDown(e);
      if (this.ClickMode == ClickMode.Hover)
        return;
      if (e.Key == Key.Space)
      {
        if ((Keyboard.Modifiers & (ModifierKeys.Alt | ModifierKeys.Control)) == ModifierKeys.Alt || this.IsMouseCaptured || e.OriginalSource != this)
          return;
        this.IsSpaceKeyDown = true;
        this.SetIsPressed(true);
        this.CaptureMouse();
        if (this.ClickMode == ClickMode.Press)
          this.OnClick();
        e.Handled = true;
      }
      else if (e.Key == Key.Return && (bool) this.GetValue(KeyboardNavigation.AcceptsReturnProperty))
      {
        if (e.OriginalSource != this)
          return;
        this.IsSpaceKeyDown = false;
        this.SetIsPressed(false);
        if (this.IsMouseCaptured)
          this.ReleaseMouseCapture();
        this.OnClick();
        e.Handled = true;
      }
      else
      {
        if (!this.IsSpaceKeyDown)
          return;
        this.SetIsPressed(false);
        this.IsSpaceKeyDown = false;
        if (!this.IsMouseCaptured)
          return;
        this.ReleaseMouseCapture();
      }
    }

    /// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.KeyUp" /> routed event that occurs when the user releases a key while this control has focus.</summary>
    /// <param name="e">The event data for the <see cref="E:System.Windows.UIElement.KeyUp" /> event.</param>
    protected override void OnKeyUp(KeyEventArgs e)
    {
      base.OnKeyUp(e);
      if (this.ClickMode == ClickMode.Hover || e.Key != Key.Space || (!this.IsSpaceKeyDown || (Keyboard.Modifiers & (ModifierKeys.Alt | ModifierKeys.Control)) == ModifierKeys.Alt))
        return;
      this.IsSpaceKeyDown = false;
      if (this.GetMouseLeftButtonReleased())
      {
        bool flag = this.IsPressed && this.ClickMode == ClickMode.Release;
        if (this.IsMouseCaptured)
          this.ReleaseMouseCapture();
        if (flag)
          this.OnClick();
      }
      else if (this.IsMouseCaptured)
        this.UpdateIsPressed();
      e.Handled = true;
    }

    /// <summary> Called when an element loses keyboard focus. </summary>
    /// <param name="e">The event data for the <see cref="E:System.Windows.IInputElement.LostKeyboardFocus" /> event.</param>
    protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
    {
      base.OnLostKeyboardFocus(e);
      if (this.ClickMode == ClickMode.Hover || e.OriginalSource != this)
        return;
      if (this.IsPressed)
        this.SetIsPressed(false);
      if (this.IsMouseCaptured)
        this.ReleaseMouseCapture();
      this.IsSpaceKeyDown = false;
    }

    /// <summary>Responds when the <see cref="P:System.Windows.Controls.AccessText.AccessKey" /> for this control is called. </summary>
    /// <param name="e">The event data for the <see cref="E:System.Windows.Input.AccessKeyManager.AccessKeyPressed" /> event.</param>
    protected override void OnAccessKey(AccessKeyEventArgs e)
    {
      if (e.IsMultiple)
        base.OnAccessKey(e);
      else
        this.OnClick();
    }

    [SecurityCritical]
    [SecurityTreatAsSafe]
    private bool GetMouseLeftButtonReleased()
    {
      return InputManager.Current.PrimaryMouseDevice.LeftButton == MouseButtonState.Released;
    }

    internal override void ChangeVisualState(bool useTransitions)
    {
      if (!this.IsEnabled)
        VisualStateManager.GoToState((FrameworkElement) this, "Disabled", useTransitions);
      else if (this.IsPressed)
        VisualStateManager.GoToState((FrameworkElement) this, "Pressed", useTransitions);
      else if (this.IsMouseOver)
        VisualStateManager.GoToState((FrameworkElement) this, "MouseOver", useTransitions);
      else
        VisualStateManager.GoToState((FrameworkElement) this, "Normal", useTransitions);
      if (this.IsKeyboardFocused)
        VisualStateManager.GoToState((FrameworkElement) this, "Focused", useTransitions);
      else
        VisualStateManager.GoToState((FrameworkElement) this, "Unfocused", useTransitions);
      base.ChangeVisualState(useTransitions);
    }
  }
}
