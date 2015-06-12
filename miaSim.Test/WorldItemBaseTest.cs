using System;
using System.Windows;
using miaSim.Foundation;
using miaSim.Test.Mocks;
using miaSim.Test.Stubs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace miaSim.Test
{
	[TestClass]
	public class WorldItemBaseTest
	{
		[TestMethod]
		public void Eat()
		{
			const double epsilon = 0.000001;

			// -------------
			// Arrange
			// -------------
			var victim = new  WorldItem(new WorldItemInteractionMock(), "Victim", new Rect(0,0,0.1,0.2));
			var robber = new WorldItem(new WorldItemInteractionMock(), "Robber", new Rect(0.5, 0.5, 0.25, 0.48));

			var orgVictimPosition = victim.Position;
			var orgRobberPosition = robber.Position;

			var orgVictimArea = victim.Area();
			var orgRobberArea = robber.Area();

			// -------------
			// Act
			// -------------
			const double percentageTaken = 30.0 / 100.0;
			WorldItemBase.MoveArea(victim, robber, percentageTaken);

			// -------------
			// Assert
			// -------------

			// percentage shrink
			AssertAreNearlyEqual(victim.Area(), orgVictimArea * (1-percentageTaken), epsilon);

			// moved to robber
			AssertAreNearlyEqual(victim.Area() - orgVictimArea, orgRobberArea - robber.Area(), epsilon);

			// relation stays the same
			AssertAreNearlyEqual(orgVictimPosition.Width / orgVictimPosition.Height, victim.Position.Width / victim.Position.Height, epsilon);
			AssertAreNearlyEqual(orgRobberPosition.Width / orgRobberPosition.Height, robber.Position.Width / robber.Position.Height, epsilon);

		}


		private void AssertAreNearlyEqual(double a, double b, double epsilon)
		{
			var difference = Math.Abs(a - b);

			if (difference >= epsilon)
			{
				Assert.Fail("values are not nearly equal; difference=" + difference);
			}
		}
	}
}
