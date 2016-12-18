using System;
using System.Collections;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Util;

/* 
 * Composable CustomControl with Binding support  
 */

namespace CollectionBinding
{
	public class TargetGroup : TextBox
	{
		private static TargetGroup instance;

		#region Events
		public event EventHandler DebugProbe;

		protected virtual void OnDebugProbe(object sender, EventArgs e)
		{
			EventHandler handler = DebugProbe;
			if (handler != null)
			{
				handler(sender, e);
			}
		}
		public void DebugTrace (object sender, EventArgs args)
		{
			string hostName, targetAtt, itemName;
			const string defaultValue = "un-set";
			var target = args as State;
			if (target != null)
			{
				hostName = target.Target.Host.DefaultIfEmpty(defaultValue + " host name");
				targetAtt = target.Target.TargetAttr.DefaultIfEmpty(defaultValue + " Attr name");
				itemName = target.Target.TargetName.DefaultIfEmpty(defaultValue + " target name");
			}
			else
			{
				hostName = targetAtt = itemName = defaultValue + " value";
			}
			var senderType = sender.GetType();
			Debug.WriteLine("\n^{0} from {1} targetting {2} on {3}^\n",
								senderType, hostName, targetAtt, itemName);
		}

		private static void PropertyChangedCallback (DependencyObject d,
			DependencyPropertyChangedEventArgs e)
		{
			var host = d as TargetGroup;
			if (host == null) return;

			// partition the output for bindings
			instance.OnDebugProbe(instance, new State(() =>
					new State.state
					{
						Host = e.Property.Name,
						TargetName = null,
						TargetAttr = host.TargetAttribute,
						Hash = host.GetHashCode()
					}
				)
			);
		}

		#endregion

		#region AP TargetAttribute

		// todo callback to update

		public static readonly DependencyProperty TargetAttributeProperty =
			DependencyProperty.RegisterAttached(
				"TargetAttribute", typeof(string),
				typeof(TargetGroup),
				new FrameworkPropertyMetadata("CommandParameter",
						FrameworkPropertyMetadataOptions.Inherits,
						PropertyChangedCallback));

		public static void SetTargetAttribute (DependencyObject target, string value)
		{
			target.SetValue(TargetAttributeProperty, value);
		}

		public static string GetTargetAttribute (DependencyObject target)
		{
			return (string)target.GetValue(TargetAttributeProperty);
		}

		public string TargetAttribute
		{
			get { return (string)GetValue(TargetAttributeProperty); }
			set { SetValue(TargetAttributeProperty, value); }
		}

		#endregion

		#region DP Targets

		public static readonly DependencyProperty TargetsProperty =
			DependencyProperty.Register(
				"Targets", typeof(FreezableCollection<FZ>),
				typeof(TargetGroup),
				new PropertyMetadata(default(FreezableCollection<FZ>),
					PropertyChangedCallback));

		public FreezableCollection<FZ> Targets
		{
			get { return (FreezableCollection<FZ>) GetValue(TargetsProperty); }
			set { SetValue(TargetsProperty, value); }
		}

		private static void ColFzReceiverOnChanged(object d, EventArgs e)
		{
			var item = d as FZ;
			if (item == null) return;

			instance.OnDebugProbe(item, item.EventState);	// partition the output for bindings

			instance.AddLogicalChild(item);

			FZChangedCallback.NamedChangedCallback(instance,
				new DependencyPropertyChangedEventArgs(TargetsProperty, null, item));
		}

		protected override IEnumerator LogicalChildren
		{
			get { return Targets.GetEnumerator(); }
		}		

		#endregion

		// Constructor

		public TargetGroup()
		{
			instance = this;

			DebugProbe += DebugTrace;

			Targets = new FreezableCollection<FZ>();
			Targets.Changed += ColFzReceiverOnChanged;
		}

		// bind to standard TextBox templates and styles
		static TargetGroup ()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(TargetGroup),
				new FrameworkPropertyMetadata(typeof(TargetGroup)));
		}
	}

	public class FZ : Freezable
	{
		public string Name { get; set; }

		public int Hash
		{
			get
			{
				return GetHashCode();
			}
		}

		#region DP FrameworkElement Target

		public static readonly DependencyProperty TargetProperty = DependencyProperty
			.Register(
				"Target", typeof(FrameworkElement),
				typeof(FZ), new PropertyMetadata(default(FrameworkElement)));

		public FrameworkElement Target
		{
			get { return (FrameworkElement) GetValue(TargetProperty); }
			set { SetValue(TargetProperty, value); }
		}

		public string ItemName
		{
			get { return Target == null ? null : Target.Name; }
		}

		#endregion

		public string Type { get; set; }

		// todo change to check binding

		#region AP string TargetAttribute

		public static readonly DependencyProperty TargetAttributeProperty;
		public string TargetAttribute
		{
			get { return (string)GetValue(TargetAttributeProperty); }
			set { SetValue(TargetAttributeProperty, value); }
		}


		public object Atrribute
		{
			get { return Utility.GetMemberByName(Target, TargetAttribute); }
			set { Utility.SetMemberByName(Target, TargetAttribute, value); }
		}

		#endregion

		protected override Freezable CreateInstanceCore()
		{
			throw new NotImplementedException();
		}

		public State EventState;

		public FZ()
		{
			Type = "Indirect";
			EventState = new State(() =>
					new State.state
					{
						Host = this.Name,
						TargetName = ItemName,
						TargetAttr = TargetAttribute,
						Hash = GetHashCode()
					}
			);
		}

		static FZ()
		{
			TargetAttributeProperty =
				TargetGroup.TargetAttributeProperty.AddOwner(typeof(FZ));
		}

		private void TargetAttributeChanged(object sender, EventArgs eventArgs)
		{
			TargetAttribute = ((TargetGroup)sender).TargetAttribute;
		}

	}
	public class State : EventArgs
	{
		private readonly Func<state> _getItem;
		public State(Func<state> getItem)
		{
			_getItem = getItem;
		}

		public struct state
		{
			public string Host;
			public string TargetName;
			public string TargetAttr;
			public int Hash;
		}
		public state Target { get { return _getItem(); } }
	}
}