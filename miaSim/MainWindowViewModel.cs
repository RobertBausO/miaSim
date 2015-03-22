﻿using System;
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

		private const int NumberOfInitItems = 100;

		private readonly GameCanvas mCanvas;

		private string mDisplayText;
		private double mWorldThrottleInMs;
		private double mUpdateViewEachXUpdate;

		private readonly World mWorld;
		private int mCylceCount;

		#endregion

		#region ================== Constructor/Destructor ===================

		public MainWindowViewModel(GameCanvas canvas)
		{
			DisplayText = "Init ...";

			WorldThrottleInMs = 10;
			UpdateViewEachXUpdate = 5;

			mCanvas = canvas;

			var list = new List<Func<IWorldItem>> { Tree.CreateRandomTree };
			mWorld = World.Create(NumberOfInitItems, list);
			mWorld.UpdateDone += OnWorldUpdateDone;
			mCylceCount = 0;
			mCanvas.Init(mWorld, new Painter());

			// Big bang
			mWorld.Start();
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

		public double WorldThrottleInMs
		{
			get { return mWorldThrottleInMs; }

			set
			{
				if (value == mWorldThrottleInMs) return;
				mWorldThrottleInMs = value;
				OnPropertyChanged();
			}

		}

        public double UpdateViewEachXUpdate
		{
			get { return mUpdateViewEachXUpdate; }

			set
			{
				if (value == mUpdateViewEachXUpdate) return;
				mUpdateViewEachXUpdate = value;
				OnPropertyChanged();
			}

		}

		#endregion

		#region ================== Methods ==================================

		void OnWorldUpdateDone(World obj)
		{
			mCylceCount++;
			if ((int)WorldThrottleInMs > 0)
				Thread.Sleep((int)WorldThrottleInMs);

            UpdateDisplayText();

            if (mCylceCount % (int)UpdateViewEachXUpdate == 0)
                mCanvas.Update();
		}

		private void UpdateDisplayText()
		{
			var text = new StringBuilder();

			text.Append("Cycles = " + mCylceCount);
			text.Append(Environment.NewLine);
			text.Append("WorldThrottleInMs = " + (int)WorldThrottleInMs);
			text.Append(Environment.NewLine);
			text.Append("UpdateCycleInMs = " + (int)UpdateViewEachXUpdate);
			text.Append(Environment.NewLine);

			//var list = mWorld.GetSnapshotOfItems();

			//foreach (var item in list)
			//{
			//	text.Append(item.GetDisplayText());
			//	text.Append(Environment.NewLine);
			//}

			DisplayText = text.ToString();
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
