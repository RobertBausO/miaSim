namespace miaSim
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		private MainWindowViewModel mViewModel;

		public MainWindow()
		{
			InitializeComponent();
			mViewModel = new MainWindowViewModel(gameCanvas);
			DataContext = mViewModel;
		}

		private void OnListBoxSzenesMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (mViewModel.SelectedSzene != null)
			{
				mViewModel.Start(mViewModel.SelectedSzene);
			}
		}
	}
}
