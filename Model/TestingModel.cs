using System;

using Microsoft.Xna.Framework;
using SurfaceTower.Model.Gun;
using SurfaceTower.Model.EventArguments;

namespace SurfaceTower.Model
{
  class TestingModel : BaseModel
  {
    #region Properties

    private bool first;

    #endregion

    public void FirstUpdate(GameTime gameTime)
    {
      // Some simple demo stuff.
      players[0].IsActive = true;
      players[0].Orientation = 45;

      // Audio folks, something like this should be in your initialize, not mine!
      Music.TimeSignature = new TimeSignature(4, 4);
      Music.Tempo = 60;
      Music.ClicksPerBeat = 8;
      Music.Start(gameTime.TotalRealTime);

      Music.Beat += new EventHandler(OnBeat);


      //For toy version, put some enemy on the stage, at a constant velocity towards the tower.
      for (int i = 0; i < 3; i++)
      {
        double bulletAngle = 0;
        Spawn(new Enemy(new Vector2 (200 + 50 *(i % 10),200 + 50 * (i + 5 % 10)), 0, 10, 10000, new Vector2(10,-10)));
        Bullets.Add(new Bullet(new Vector2 (i,0), new Vector2(100 * (float) Math.Cos(bulletAngle), 100 * (float) Math.Sin(bulletAngle)), 1, Effects.Homing|Effects.Pierce, i % 4));
      }
    }

  

    /// <summary>
    /// Called in response to the Music.Beat signal.
    /// </summary>
    /// <param name="sender">The Music object which sent the Beat signal.</param>
    /// <param name="e">Always null</param>
    public void OnBeat(object sender, EventArgs e)
    {
      // Some demo stuff.
      players[0].Orientation += 18;
    }

    public override void OnUpdate(GameTime gameTime)
    {
      if (!first)
      {
        FirstUpdate(gameTime);
        first = true;
      }

      base.OnUpdate(gameTime);
    }
  }
}
