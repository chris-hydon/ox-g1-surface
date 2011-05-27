using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Model
{
  /// <summary>
  /// Summary description for BulletTest
  /// </summary>
  [TestClass]
  public class BulletTest : Bootstrap
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
    public void TestMethod1()
    {
      //
      // TODO: Add test logic	here
      //
    }
  }
}
