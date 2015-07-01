using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows.Input;
using miaGame;
using miaSim.Annotations;
using miaSim.Foundation;
using miaSim.Tools;

namespace miaSim
{
	internal class MainWindowViewModel : INotifyPropertyChanged
	{
		#region ================== Member variables =========================

		public event PropertyChangedEventHandler PropertyChanged;

		private List<Szene> mSzenes;
		private readonly GameCanvas mCanvas;

		private double mWorldThrottleInMs;
		private double mUpdateViewEachXUpdate;

		private World mWorld;
		private int mCylceCount;

		private readonly RelayCommand<object> mNextStepCommand;

		private AutoResetEvent mNextStepEvent = new AutoResetEvent(false);

		#endregion

		#region ================== Constructor/Destructor ===================

		public MainWindowViewModel(GameCanvas canvas)
		{
			WorldThrottleInMs = 0;
			UpdateViewEachXUpdate = 1;

			mNextStepCommand = new RelayCommand<object>(DoNextStep);

			mCanvas = canvas;

			Szenes = Plants.Szenes.GetSzeneList();
		}

		#endregion

		#region ================== Properties ===============================

		public double MaxThrottleInMs
		{
			get { return 1000; }
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

		public List<Szene> Szenes
		{
			get { return mSzenes; }
			set
			{
				if (value == mSzenes) return;
				mSzenes = value;
				OnPropertyChanged();
			}
		}

		public Szene SelectedSzene { get; set; }

		public ICommand DoNextStepCommand
		{
			get { return mNextStepCommand; }
		}

		#endregion

		#region ================== Methods ==================================

		public void Start(Szene szene)
		{
			if (mWorld != null)
			{
				mWorld.UpdateDone -= OnWorldUpdateDone;
				mWorld.Stop();
				mWorld = null;
			}

			mWorld = new World(szene);
			mWorld.UpdateDone += OnWorldUpdateDone;
			mCylceCount = 0;
			mCanvas.Init(new Painter(mWorld));

			// Big bang
			mWorld.Start();
		}


		void OnWorldUpdateDone(World obj)
		{
			mCylceCount++;

			if (WorldThrottleInMs == MaxThrottleInMs)
			{
				// wait for click
				mNextStepEvent.WaitOne();
			}
			else
			{
				if ((int)WorldThrottleInMs > 0)
					Thread.Sleep((int)WorldThrottleInMs);
			}

			if (mCylceCount % (int)UpdateViewEachXUpdate == 0)
			{
				var text = new StringBuilder();
				text.Append("WorldUpdateCycles = " + mCylceCount);
				text.Append(Environment.NewLine);
				text.Append("WorldUpdatesPerSecond = " + Conversion.Double2String(obj.LoopsPerSecond));
				text.Append(Environment.NewLine);
				text.Append("WorldThrottleInMs = " + (int)WorldThrottleInMs);
				text.Append(Environment.NewLine);
				text.Append("DispalyUpdate every x-Update = " + (int)UpdateViewEachXUpdate);
				text.Append(Environment.NewLine);

				var dict = new Dictionary<string, int>();

				foreach (var item in mWorld.Items)
				{
					var typeName = item.GetType().Name;

					if (!dict.ContainsKey(typeName))
					{
						dict.Add(typeName, 0);
					}

					dict[typeName]++;
				}

				foreach (var type in dict.Keys)
				{
					text.Append(string.Format("Type={0}; Count ={1}", type, dict[type]));
					text.Append(Environment.NewLine);
				}

				mWorld.Info = text.ToString();

				mCanvas.Update();
			}
		}

		private void DoNextStep(object param)
		{
			mNextStepEvent.Set();
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
