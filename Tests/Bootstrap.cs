using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using SurfaceTower;
using SurfaceTower.Model;

namespace Tests
{
  public class Bootstrap
  {
    private TestingApp app;
    private GameTime gameTime;
    private static TimeSpan[] interval = new TimeSpan[3] { new TimeSpan(0, 0, 0, 0, 17), new TimeSpan(0, 0, 0, 0, 16), new TimeSpan(0, 0, 0, 0, 17) };
    private int intervals = 0;

    #region Properties

    protected BaseModel Model
    {
      get { return app.Model; }
    }

    #endregion

    #region Methods

    public Bootstrap()
    {
      app = new TestingApp();
    }

    [TestInitialize]
    public virtual void SetUp()
    {
      app.Restart();
      gameTime = new GameTime();
    }

    protected void Update()
    {
      intervals = (intervals + 1) % 3;
      gameTime = new GameTime(
        gameTime.TotalRealTime + Bootstrap.interval[intervals], Bootstrap.interval[intervals],
        gameTime.TotalGameTime + Bootstrap.interval[intervals], Bootstrap.interval[intervals],
        false
      );
      Model.OnUpdate(gameTime);
    }

    protected void UpdateUntilClick()
    {
      Assert.AreNotEqual(Model.NumberOfPlayers, 0, "Cannot perform any time-based testing without any players.");
      TimeSpan t = Model.Music.LastClick;
      while (t == Model.Music.LastClick)
      {
        Update();
      }
    }

    protected void UpdateUntilBeat()
    {
      Assert.AreNotEqual(Model.NumberOfPlayers, 0, "Cannot perform any time-based testing without any players.");
      TimeSpan t = Model.Music.LastBeat;
      while (t == Model.Music.LastBeat)
      {
        Update();
      }
    }

    protected void UpdateUntilBar()
    {
      Assert.AreNotEqual(Model.NumberOfPlayers, 0, "Cannot perform any time-based testing without any players.");
      TimeSpan t = Model.Music.LastBar;
      while (t == Model.Music.LastBar)
      {
        Update();
      }
    }

    #endregion
  }
}
