﻿using System;

using Microsoft.Xna.Framework;
using SurfaceTower.Model.Gun;

namespace SurfaceTower.Model
{
  public class GameModel : BaseModel
  {
    private bool first;

    public void FirstUpdate(GameTime gameTime)
    {
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
