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
      for (int i = 0; i < 100; i++)
      {
        Spawn(new Enemy(new Vector2(100 + 50 * (i % 10), 100 + 50 * (i / 10)), 0, 10, 1, Vector2.Zero));
      }
      for (int i = 0; i < 100; i++)
      {
          Bullets.Add(new Bullet(new Vector2(10*i, 5*i^2), 200 * Vector2.One, 1, Effects.Homing , 0));
      }
      turrets.Add(new Turret(new Vector2(400, 50), 0));
        
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
