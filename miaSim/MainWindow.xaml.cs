using System.Windows;

namespace miaSim
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow 
	{
		public MainWindow()
		{
			InitializeComponent();
			DataContext = new MainWindowViewModel(gameCanvas);
		}
	}
}
