using System;

using Microsoft.Xna.Framework;
using SurfaceTower.Model.Gun;

namespace SurfaceTower.Model
{
  //This is intended to be the BaseModel for the final application.
  public class GameModel : BaseModel
  {
    private bool first;

    public void FirstUpdate(GameTime gameTime)
    {
      Music.Beat += new EventHandler(Tower.OnBeat);
      spawner = new Spawner();
      Music.Start(gameTime.TotalRealTime);
    }

    public override void OnUpdate(GameTime gameTime)
    {
      if (NumberOfPlayers > 0)
      {
        if (!first)
        {
          FirstUpdate(gameTime);
          first = true;
        }

        base.OnUpdate(gameTime);
      }
    }

    public override bool PlayerLeave(int player)
    {
      bool ret = base.PlayerLeave(player);
      if (NumberOfPlayers == 0)
      {
        App.Instance.Restart();
      }

      return ret;
    }

    public override void Restart()
    {
      first = false;
      base.Restart();
    }
  }
}
