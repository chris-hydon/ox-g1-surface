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
      Music.Start(gameTime.TotalRealTime);

      Music.Beat += new EventHandler(OnBeat);

      spawner = new Spawner();
      for (int i = 0; i < 4; i++) { 
        Players[i].IsActive = true;
        Turrets.Add(new Turret(new Vector2((i % 2 + 1) * 250, (float)Math.Ceiling((double)i / 2) * 250), i));
      
      }
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
