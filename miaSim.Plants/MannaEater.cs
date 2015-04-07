using System;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;

using miaGame;
using miaSim.Foundation;

namespace miaSim.Plants
{
	/// <summary>
	/// Configuration of a MannaEater
	/// </summary>
	public class MannaEaterDns
	{
		public MannaEaterDns()
		{
			MinExtension = 0.01;
			MaxExtension = 0.1;
		}

		// fix definitions
		public double MinExtension { get; private set; }
		public double MaxExtension { get; private set; }

	}

	/// <summary>
	/// representation of MannaEater
	/// </summary>
	public class MannaEater : WorldItemBase
	{
		#region ================== Member variables =========================

		// DNS properties
		private MannaEaterDns mDns;

		private IList<WorldItemBase> mConnections;

		#endregion

		#region ================== Constructor/Destructor ===================

		public MannaEater(WorldItemBaseIteraction interaction, Rect position, MannaEaterDns dns)
			: base(interaction, "MannaEater", position)
		{
			mDns = dns;
		}

		#endregion

		#region ================== Properties ===============================

		public override int ZPosition
		{
			get
			{
				return base.ZPosition + 1;
			}
		}


		#endregion

		#region ================== Methods ==================================

		public static WorldItemBase CreateRandomized(WorldItemBaseIteraction interaction)
		{
			var dns = new MannaEaterDns();
			var position = new Rect(new Point(Utils.NextRandom(), Utils.NextRandom()), new Size(dns.MinExtension, dns.MinExtension));
			return new MannaEater(interaction, position, dns);
		}

		public override void Update()
		{
			Rect oldPosition = Position;

			if (Position.Width < mDns.MaxExtension && Position.Height < mDns.MaxExtension)
			{
				// eat
				var intersects = WorldInteraction.GetIntersectItems(this, typeof(Manna));

				foreach (var intersect in intersects)
				{
					var manna = intersect as Manna;

					// get 1% of the manna size
					var part = 0.1;
					var widthTransfer = Math.Min(manna.Position.Width, this.Position.Width) * part;
					var heightTransfer = Math.Min(manna.Position.Height, this.Position.Height) * part;

					if (manna.ChangeSize(-widthTransfer, -heightTransfer))
					{
						this.ChangeSize(widthTransfer, heightTransfer);
					}
				}

				//this.ChangeSize(-0.00001);

				if (Position.Width < mDns.MinExtension || Position.Height < mDns.MinExtension)
				{
					WorldInteraction.RemoveItem(this);
				}
			}
			else
			{
				// create 2 children and die
				WorldInteraction.AddItem(CreateChild());
				WorldInteraction.AddItem(CreateChild());

				WorldInteraction.RemoveItem(this);
			}
		}

		private MannaEater CreateChild()
		{
			var newItem = CreateRandomized(WorldInteraction) as MannaEater;
			newItem.Position = Position;
			newItem.SetSize(mDns.MinExtension, mDns.MinExtension);
			newItem.Move(Utils.NextRandom(Position.Left, Position.Right), Utils.NextRandom(Position.Top, Position.Bottom));
			return newItem;
		}

		public override void Tell(Message message)
		{
		}

		public override void Draw(PaintContext context)
		{
			var brush = Brushes.Red;
			context.DrawRectangle(brush, Position);
		}


		#endregion
	}
}
