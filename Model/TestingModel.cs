using System;

using Microsoft.Xna.Framework;
using SurfaceTower.Model.Gun;
using SurfaceTower.Model.EventArguments;
using Microsoft.Xna.Framework.Graphics;

namespace SurfaceTower.Model
{
  class TestingModel : BaseModel
  {
    #region Properties

    private bool first;

    #endregion

    public void FirstUpdate(GameTime gameTime)
    {
      // Audio folks, something like this should be in your initialize, not mine!
      Music.TimeSignature = new TimeSignature(4, 4);
      Music.Tempo = 60;
      Music.ClicksPerBeat = 8;
      Music.Start(gameTime.TotalRealTime);

      Music.Beat += new EventHandler(OnBeat);

      spawner = new Spawner();

      //For toy version, put some enemy on the stage, at a constant velocity towards the tower.
      for (int i = 0; i < 100; i++)
      {
        //Spawn(new Enemy(new Vector2(100 + 50 * (i % 10), 100 + 50 * (i / 10)), 0, 10, 15, Vector2.Zero, Color.Azure));
      }
      Bullets.Add(new Bullet(new Vector2(0, 0), 200 * Vector2.One, 1, Effects.Homing | Effects.Burn | Effects.Pierce, 0));
      Turrets.Add(new Turret(new Vector2(100, 100), 1));
      Turrets.Add(new Turret(new Vector2(500, 500), 2));
      Turrets.Add(new Turret(new Vector2(100, 500), 3));
      Turrets.Add(new Turret(new Vector2(500, 100), 0));
      players[0].IsActive = true;
    }

    /// <summary>
    /// Called in response to the Music.Beat signal.
    /// </summary>
    /// <param name="sender">The Music object which sent the Beat signal.</param>
    /// <param name="e">Always null</param>
    public void OnBeat(object sender, EventArgs e)
    {
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
