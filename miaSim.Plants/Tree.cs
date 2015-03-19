using miaSim.Foundation;

namespace miaSim.Plants
{
    public class Tree : WorldItem
    {
		 #region ================== Member variables =========================

		 private const float MaxExtension = 0.1f;
	    private float mGrow = 0.0001f;

		 #endregion

		 #region ================== Constructor/Destructor ===================

		 public Tree(Location location, Extension extension)
			 : base("Tree", location, extension)
		 {
		 }

		 #endregion

		 #region ================== Properties ===============================
		 #endregion

		 #region ================== Methods ==================================

	    public static IWorldItem CreateRandomTree()
	    {
			 var item = new Tree(new Location(Utils.NextRandom(), Utils.NextRandom()),
										new Extension(Utils.NextRandom(MaxExtension), Utils.NextRandom(MaxExtension)));

		    return item;
	    }

	    public override void Update(double msSinceLastUpdate)
	    {
		    Extension.Width += mGrow;
		    Extension.Height += mGrow;

			 if (Extension.Width > MaxExtension || Extension.Height > MaxExtension)
		    {
			    mGrow = - mGrow;
		    }

			 if (Extension.Width < 0)
			 {
				 Extension.Width = 0;
			 }

		    if (Extension.Height < 0)
		    {
			    Extension.Height = 0;
		    }
	    }

	    public override string GetDisplayText()
	    {
		    return base.GetDisplayText();
	    }

	    #endregion
    }
}
