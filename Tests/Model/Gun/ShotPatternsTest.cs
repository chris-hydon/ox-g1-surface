using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using SurfaceTower.Model;
using SurfaceTower.Model.Gun;

namespace Tests.Model.Gun
{
  /// <summary>
  /// Summary description for ShotPatternsTest
  /// </summary>
  [TestClass]
  public class ShotPatternsTest : Bootstrap
  {
    private TestContext testContextInstance;

    /// <summary>
    /// Gets or sets the test context which provides
    /// information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get { return testContextInstance; }
      set { testContextInstance = value; }
    }

    [TestMethod]
    public void TestEmpty()
    {
      ShotPatterns patterns = new ShotPatterns();
      foreach (ShotPattern shot in patterns)
      {
        Assert.Fail("Empty list returned a member anyway.");
      }
    }

    [TestMethod]
    public void TestOne()
    {
      ShotPatterns patterns = ShotPatterns.Create(new ShotPattern(0, Vector2.Zero, Effects.None));
      int i = 0;
      foreach (ShotPattern shot in patterns)
      {
        i++;
      }
      Assert.AreEqual(i, 1, "Wrong number of shots in iterator.");
    }

    [TestMethod]
    public void TestChange()
    {
      ShotPatterns patterns = ShotPatterns.Create(new ShotPattern(0, Vector2.Zero, Effects.None));
      ShotPattern p = null;
      foreach (ShotPattern pattern in patterns)
      {
        p = pattern;
      }
      foreach (ShotPattern pattern in patterns)
      {
        Assert.AreNotSame(p, pattern, "ShotPattern can be edited externally.");
      }
    }
  }
}
