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

		private const int NumberOfInitItems = 100;

		private readonly GameCanvas mCanvas;

		private double mWorldThrottleInMs;
		private double mUpdateViewEachXUpdate;

		private readonly World mWorld;
		private int mCylceCount;

		#endregion

		#region ================== Constructor/Destructor ===================

		public MainWindowViewModel(GameCanvas canvas)
		{
			WorldThrottleInMs = 0;
			UpdateViewEachXUpdate = 5;

			mCanvas = canvas;

			var list = new List<Func<IWorldItemIteraction, IWorldItem>> { Lawn.CreateRandomTree };
			mWorld = World.Create(NumberOfInitItems, list);
			mWorld.UpdateDone += OnWorldUpdateDone;
			mCylceCount = 0;
			mCanvas.Init(new Painter(mWorld));

			// Big bang
			mWorld.Start();
		}

		#endregion

		#region ================== Properties ===============================

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

			if (mCylceCount%(int) UpdateViewEachXUpdate == 0)
			{
				var text = new StringBuilder();
				text.Append("WorldUpdateCycles = " + mCylceCount);
				text.Append(Environment.NewLine);
				text.Append("WorldUpdatesPerSecond = " + Utils.Double2String(obj.LoopsPerSecond));
				text.Append(Environment.NewLine);
				text.Append("WorldThrottleInMs = " + (int)WorldThrottleInMs);
				text.Append(Environment.NewLine);
				text.Append("DispalyUpdate every x-Update = " + (int)UpdateViewEachXUpdate);
				text.Append(Environment.NewLine);

				mWorld.Info = text.ToString();

				mCanvas.Update();
			}
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
