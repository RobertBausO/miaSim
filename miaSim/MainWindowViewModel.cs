using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using miaGame;
using miaSim.Annotations;
using miaSim.Foundation;
using miaSim.Plants;

namespace miaSim
{
	internal class MainWindowViewModel : INotifyPropertyChanged
	{
		#region ================== Member variables =========================

		public event PropertyChangedEventHandler PropertyChanged;

		private const int NumberOfInitItems = 10;
		private const int UpdateCycleInMs = 500;

		private GameCanvas mCanvas;

		private string mDisplayText;
		private readonly World mWorld;

		private readonly Timer mTimer;

		#endregion

		#region ================== Constructor/Destructor ===================

		public MainWindowViewModel(GameCanvas canvas)
		{
			DisplayText = "Init ...";

			mCanvas = canvas;

			var list = new List<Func<IWorldItem>> { Tree.CreateRandomTree };
			mWorld = World.Create(NumberOfInitItems, list);
			mWorld.Start();
			mCanvas.Init(mWorld, new Painter());

			mTimer = new Timer(s => UpdateView(), null, UpdateCycleInMs, 0);
		}

		#endregion

		#region ================== Properties ===============================

		public string DisplayText
		{
			get { return mDisplayText; }
			set
			{
				if (value == mDisplayText) return;
				mDisplayText = value;
				OnPropertyChanged();
			}
		}

		#endregion

		#region ================== Methods ==================================

		private void UpdateView()
		{
			UpdateDisplayText();
			mCanvas.Update();
		}

		private void UpdateDisplayText()
		{
			var text = new StringBuilder();

			text.Append("LoopsPerSecond = " + mWorld.LoopsPerSecond);
			text.Append(Environment.NewLine);

			var list = mWorld.GetSnapshotOfItems();

			foreach (var item in list)
			{
				text.Append(item.GetDisplayText());
				text.Append(Environment.NewLine);
			}

			DisplayText = text.ToString();

			mTimer.Change(UpdateCycleInMs, 0);
		}

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion

	}
}
