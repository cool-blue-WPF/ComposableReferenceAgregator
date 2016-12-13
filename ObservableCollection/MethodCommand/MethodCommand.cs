using System;
using System.Windows.Input;
using System.Windows;
using System.Reflection;
using System.Windows.Data;
using System.ComponentModel;
using System.Windows.Markup;


namespace MethodCommandNS

{
	[ContentProperty("Arguments")]
	public class MethodCommand
		: Freezable, // Enable ElementName and DataContext bindings

			ICommand,
			INotifyPropertyChanged

	{
		// The name of the method to call on Invoke

		public string MethodName { get; set; }


		// When this is set, exceptions during invoke are caught, and the exception 
		// is set as the Exception property

		[DefaultValue(true)]
		public bool CatchExceptions { get; set; }


		// If there is an exception during a command invoke, and CatchExceptions
		// is set, this will have the exception object.

		public Exception Exception

		{
			get { return _exception; }


			private set

			{
				_exception = value;

				FirePropertyChanged("Exception");
			}
		}

		Exception _exception;


		// This holds the arguments to be passed to the method.
		// This is Freezable so that the DataContext and ElementName bindings
		// can work correctly.

		#region DP FreezableCollection<MethodArgument> Arguments

		public FreezableCollection<MethodArgument> Arguments

		{
			get
			{
				return (FreezableCollection<MethodArgument>) GetValue(ArgumentsProperty);
			}
		}

		public static readonly DependencyProperty ArgumentsProperty =
			DependencyProperty.Register(
				"Arguments", typeof(FreezableCollection<MethodArgument>), 
				typeof(MethodCommand), null);

		#endregion

		// This is a private DP that’s used to get the inherited DataContext
		// (see it used in the MethodCommand constructor).

		private static readonly DependencyProperty ElementDataContextProperty =
			DependencyProperty.Register("ElementDataContext", 
				typeof(object),
				typeof(MethodCommand), null);


		// The Target property specifies the object on which to invoke the method.
		// If this is null, invoke the method on the DataContext

		#region DP object Target

		public object Target

		{
			get { return (object) GetValue(TargetProperty); }

			set { SetValue(TargetProperty, value); }
		}

		public static readonly DependencyProperty TargetProperty =
			DependencyProperty.Register("Target", 
				typeof(object), 
				typeof(MethodCommand),
				null);

		#endregion

		// Constructor

		public MethodCommand()

		{
			// The Arguments property is read-only and never null

			SetValue(ArgumentsProperty, new FreezableCollection<MethodArgument>());


			// Set a default Binding onto the private ElementDataContextProperty.
			// A default binding just binds to the inherited DataContext.  This is how
			// MethodCommand typically gets the object on which to invoke the method.

			BindingOperations.SetBinding(
				this,
				ElementDataContextProperty,
				new Binding());


			// By default, catch exceptions that are raised by the method.

			CatchExceptions = true;
		}


		// Fire the PropertyChanged event when the Exception property changes.

		public event PropertyChangedEventHandler PropertyChanged;

		void FirePropertyChanged(string propertyName)

		{
			if (PropertyChanged != null)

			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}


		// We need to implement this method because we’re a Freezzable subtype.

		protected override Freezable CreateInstanceCore()

		{
			throw new NotImplementedException();
		}


		// Implement the CanExecute members of ICommand

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)

		{
			// We’ll always be enabled to be invokes.

			return true;
		}


		// ICommand.Execute implementation.  This version calls ExecuteImpl, which does the
		// the actual call.  But this method wraps that call in a try/finally, depending on
		// the value of CatchExceptions.

		public void Execute(object parameter)

		{
			// Clear out any exception of a former invocation.

			Exception = null;


			if (CatchExceptions)

			{
				// Invoke the method in a try/finally

				try

				{
					ExecuteImpl(parameter);
				}

				catch (Exception e)

				{
					// Any exceptions will likely come back as a 
					// TargetInvocationException, but the original exception
					// is more interesting.

					if (e is TargetInvocationException)

					{
						if (e.InnerException != null)

							Exception = e.InnerException;

						else

							Exception = e;
					}

					else

						Exception = e;
				}
			}


			// Otherwise, CatchExceptions isn’t set, so just forward the call

			else

			{
				ExecuteImpl(parameter);
			}
		}


		// ExecuteImpl is where we actually invoke the method.

		void ExecuteImpl(object parameter)

		{
			// See if the Target property is set

			object target = Target;

			if (target == null)

			{
				// If not, look for an inherited DataContext

				target = GetValue(ElementDataContextProperty);
			}


			// We must have a target, either from Target or DataContext

			if (target == null)

			{
				throw new InvalidOperationException(
					"MethodCommand target not found (must set either Target or DataContext)");
			}


			// Get the method to be called.  Note that this doesn’t support
			// overloaded methods, but it could be updated to do so.


			MethodInfo methodInfo = target.GetType().GetMethod(MethodName);

			if (methodInfo == null)

			{
				throw new InvalidOperationException("Method " + MethodName +
				                                    " couldn’t be found on type " +
				                                    target.GetType().Name + "");
			}


			// Copy the Arguments to an array


			object[] arguments = new object[Arguments.Count];


			for (int i = 0; i < Arguments.Count; i++)

				arguments[i] = Arguments[i].Value;


			// Invoke the method


			methodInfo.Invoke(target, arguments);
		}
	}


	// The MethodArgument class plugs into MethodCommand.Arguments

	public class MethodArgument
		: Freezable // Enable ElementName and DataContext bindings

	{
		public MethodArgument()
		{
		}


		// The value of a method argument 

		public object Value

		{
			get { return (object) GetValue(ValueProperty); }

			set { SetValue(ValueProperty, value); }
		}

		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(object), typeof(MethodArgument),
				null);


		// We need to implement this method since we are a subtype of Freezable.  But since
		// we don’t need to support cloning, we won’t implement it.

		protected override Freezable CreateInstanceCore()

		{
			throw new NotImplementedException();
		}
	}
}