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

			MaxMovement = 0.001;
		}

		// fix definitions
		public double MinExtension { get; private set; }
		public double MaxExtension { get; private set; }

		public double MaxMovement { get; private set; }

	}

	/// <summary>
	/// representation of MannaEater
	/// </summary>
	public class MannaEater : WorldItemBase
	{
		#region ================== Member variables =========================

		// DNS properties
		private MannaEaterDns mDns;

		private Vector mMovement;

		#endregion

		#region ================== Constructor/Destructor ===================

		public MannaEater(WorldItemBaseIteraction interaction, Rect position, Vector movement, MannaEaterDns dns)
			: base(interaction, "MannaEater", position)
		{
			mDns = dns;
			mMovement = movement;
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
			var position = new Rect(new Point(Utils.NextRandom(), Utils.NextRandom()), new Size(2*dns.MinExtension, 2*dns.MinExtension));
			var movement = new Vector(Utils.NextRandom(-dns.MaxMovement / 2.0, dns.MaxMovement / 2.0), Utils.NextRandom(-dns.MaxMovement / 2.0, dns.MaxMovement / 2.0));

			return new MannaEater(interaction, position, movement, dns);
		}

		public override void Update()
		{
			// smaller than max size
			if (Position.Width < mDns.MaxExtension && Position.Height < mDns.MaxExtension)
			{
				double area = this.Area();

				// eat
				var intersects = WorldInteraction.GetIntersectItems(this, null); 

				foreach (var intersect in intersects)
				{
					var partTaken = 0.0;

					if (intersect is Manna)
					{
						// get % of the manna size
						partTaken = 15.0 / 100.0;
					}

					if (intersect is MannaEater)
					{
						// only the bigger one can eat the smaller one
						if (area > intersect.Area())
						{
							// cannibalism included
							partTaken = 5.0 / 100.0;
						}
					}

					if (partTaken > 0.0)
					{
						MoveArea(intersect, this, partTaken);
					}
				}

				// shrink
				ChangeSize(-0.0001);

				if (Area() <  mDns.MinExtension * mDns.MinExtension)
				{
					WorldInteraction.RemoveItem(this);
				}

				// move
				if (!Move(mMovement.X, mMovement.Y))
				{
					var newMovement = new Vector(Utils.NextRandom(-mDns.MaxMovement / 2.0, mDns.MaxMovement / 2.0), Utils.NextRandom(-mDns.MaxMovement / 2.0, mDns.MaxMovement / 2.0));
					mMovement = newMovement;
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

			double moveLeft = Position.Right - (Utils.NextRandom(Position.Left, Position.Right));
			double moveDown = Position.Bottom - Utils.NextRandom(Position.Top, Position.Bottom);

			newItem.Move(moveLeft, moveDown);
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
