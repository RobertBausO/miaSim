using System;
using System.Windows.Input;

namespace miaSim
{

	public class RelayCommand<T> : ICommand
	{
		#region ================== Member variables =========================

		readonly Action<T> mExecute;
		readonly Predicate<T> mCanExecute;

		#endregion

		#region ================== Constructor/Destructor ===================

		public RelayCommand(Action<T> execute)
			: this(execute, null)
		{
		}

		/// <summary>
		/// Creates a new command.
		/// </summary>
		/// <param name="execute">The execution logic.</param>
		/// <param name="canExecute">The execution status logic.</param>
		public RelayCommand(Action<T> execute, Predicate<T> canExecute)
		{
			if (execute == null)
				throw new ArgumentNullException("execute");

			mExecute = execute;
			mCanExecute = canExecute;
		}

		#endregion

		#region ================== Properties ===============================
		#endregion

		#region ================== Methods ==================================

		public bool CanExecute(object parameter)
		{
			return mCanExecute == null || mCanExecute((T)parameter);
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public void Execute(object parameter)
		{
			mExecute((T)parameter);
		}

		#endregion

	}
}
