using miaSim.Foundation;

namespace miaSim.Plants
{
    public class Tree : WorldItem
    {
		 #region ================== Member variables =========================

		 private const float MaxExtension = 0.1f;
	    private float mGrow = 0.0001f;

	    private readonly Extension mMaxExtension;

		 #endregion

		 #region ================== Constructor/Destructor ===================

		 public Tree(Location location, Extension maxExtension)
			 : base("Tree", location, new Extension(0.0f,0.0f))
		 {
			 mMaxExtension = maxExtension;
		 }

		 #endregion

		 #region ================== Properties ===============================
		 #endregion

		 #region ================== Methods ==================================

	    public static IWorldItem CreateRandomTree()
	    {
		    var location = new Location(Utils.NextRandom(), Utils.NextRandom());
		    var maxExtension = new Extension(Utils.NextRandom(MaxExtension), Utils.NextRandom(MaxExtension));

			 return new Tree(location, maxExtension);
	    }

	    public override void Update(double msSinceLastUpdate)
	    {
		    Extension.Width += mGrow;
		    Extension.Height += mGrow;

			 if (Extension.Width > mMaxExtension.Width || Extension.Height > mMaxExtension.Height)
		    {
			    mGrow = 0;
		    }
	    }

	    #endregion
    }
}
