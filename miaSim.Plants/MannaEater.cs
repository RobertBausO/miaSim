using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

using miaGame;
using miaSim.Foundation;
using miaSim.Tools;

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

			MaxMovement = 0.01;
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

		public MannaEater(IWorldItemBaseIteraction interaction, Rect position, Vector movement, MannaEaterDns dns)
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

		public override void Update()
		{
			if (WillDie())
			{
				Die();
				return;
			}

			if (CanReproduce())
			{
				Reproduce();
				return;
			}

			// Eat
			List<WorldItemBase> toBeEaten;
			if (CanEat(out toBeEaten))
			{
				Eat(toBeEaten);
			}
			else
			{
				// shrink, because nothing to eat
				ChangeSize(-mDns.MinExtension / 10.0, -mDns.MinExtension / 10.0);
			}

			// Move
			if (!Move(mMovement.X, mMovement.Y))
			{
				CalculateNewMovementDirection();
			}
		}

		public static WorldItemBase CreateRandomized(IWorldItemBaseIteraction interaction)
		{
			var dns = new MannaEaterDns();
			var position = new Rect(new Point(SimRandom.NextRandom(), SimRandom.NextRandom()), new Size(2*dns.MinExtension, 2*dns.MinExtension));
			var movement = new Vector(SimRandom.NextRandom(-dns.MaxMovement / 2.0, dns.MaxMovement / 2.0), SimRandom.NextRandom(-dns.MaxMovement / 2.0, dns.MaxMovement / 2.0));

			return new MannaEater(interaction, position, movement, dns);
		}

		private bool CanReproduce()
		{
			return Position.Width >= mDns.MaxExtension && Position.Height >= mDns.MaxExtension;
		}

		private void Reproduce()
		{
			// create 2 children and die
			var child1 = new MannaEater(WorldInteraction, Position, mMovement, new MannaEaterDns());
			var child2 = new MannaEater(WorldInteraction, Position, mMovement, new MannaEaterDns());

			child1.CalculateNewMovementDirection();
			child2.CalculateNewMovementDirection();

			// set to left/upper and right/lower corner
			child1.Position = new Rect(Position.Location, new Size(Position.Width / 2.0, Position.Height / 2.0));
			child2.Position = new Rect(Position.Left + Position.Width / 2.0, 
												Position.Top + Position.Height / 2.0, 
												Position.Width / 2.0, 
												Position.Height / 2.0);

			WorldInteraction.AddItem(child1);
			WorldInteraction.AddItem(child2);

			WorldInteraction.RemoveItem(this);
		}

		private bool CanEat(out List<WorldItemBase> toBeEaten)
		{
			var intersects = WorldInteraction.GetIntersectItems(this, null);
			toBeEaten = intersects.OfType<Manna>().Cast<WorldItemBase>().ToList();
			return toBeEaten.Count > 0;
		}

		private void Eat(IEnumerable<WorldItemBase> toBeEaten)
		{
			foreach (var food in toBeEaten)
			{
				// can eat maximal the min extension
				MoveArea(food, this, mDns.MinExtension * mDns.MinExtension);
			}
		}

		private bool WillDie()
		{
			return Area() < mDns.MinExtension*mDns.MinExtension;
		}

		private void Die()
		{
			WorldInteraction.RemoveItem(this);
		}

		private void CalculateNewMovementDirection()
		{
			var newMovement = new Vector(SimRandom.NextRandom(-mDns.MaxMovement/2.0, mDns.MaxMovement/2.0), SimRandom.NextRandom(-mDns.MaxMovement/2.0, mDns.MaxMovement/2.0));
			mMovement = newMovement;
		}

		public override void Tell(Message message)
		{
		}

		public override void Draw(PaintContext context)
		{
			var brush = Brushes.Transparent;
			var pen = new Pen(Brushes.Red, 1.0);
			context.DrawRectangle(brush, pen, Position);
		}


		#endregion
	}
}
