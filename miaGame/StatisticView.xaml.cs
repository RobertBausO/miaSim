using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace miaGame
{
	/// <summary>
	/// Interaction logic for StatisticView.xaml
	/// </summary>
	public partial class StatisticView
	{
		#region ================== Member variables =========================

		public static FrameworkPropertyMetadata MetaData = new FrameworkPropertyMetadata("unknown", 
																FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnStatisticDataPropertyChanged);

		public static DependencyProperty StatisticDataProperty = DependencyProperty.Register("StatisticData", typeof(string), typeof(StatisticView), MetaData);

		#endregion

		#region ================== Constructor/Destructor ===================

		public StatisticView()
		{
			InitializeComponent();
		}

		#endregion

		#region ================== Properties ===============================

		public string StatisticData
		{
			get { return GetValue(StatisticDataProperty) as string; }
			set
			{
				SetValue(StatisticDataProperty, value);
			}
		}

		#endregion

		#region ================== Methods ==================================

		private static void OnStatisticDataPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			var view = obj as StatisticView;

			if (view != null)
			{
				view.Dispatcher.BeginInvoke(new Action(view.InvalidateVisual), DispatcherPriority.Input, null);
			}
		}

		protected override void OnRender(DrawingContext context)
		{
			base.OnRender(context);

			var width = ActualWidth;
			var height = ActualHeight;

			context.DrawEllipse(Brushes.Wheat, new Pen(Brushes.Red, 1.0),
				new Point(width / 2.0, height / 2.0), width / 2.0, height / 2.0);


			if (!string.IsNullOrEmpty(StatisticData))
			{
				var sizeInPixel = height / 7.0;

				var text = new FormattedText(StatisticData,
					 CultureInfo.CurrentUICulture,
					 FlowDirection.LeftToRight, new Typeface("Arial"),
						 sizeInPixel, Brushes.Teal) { TextAlignment = TextAlignment.Left };

				context.DrawText(text, new Point(0, 0));
			}
		}

		#endregion

	}
}
