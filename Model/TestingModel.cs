using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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
        Vector2 enemyPos = new Vector2(50*(i%10), 50*(i/10));
        Vector2 enemyVelocity = Tower.Location - enemyPos;
        enemyVelocity /= enemyVelocity.Length() / 25;
        double bulletAngle = 2 * i * Math.PI / 100;
        Living.Add(new Enemy(enemyPos, 0, 10, 100, enemyVelocity));
        Bullets.Add(new Bullet(Tower.Location, new Vector2(50 * (float) Math.Cos(bulletAngle), 50 * (float) Math.Sin(bulletAngle)), 100, Turret.Effects.None));
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
