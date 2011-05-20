using System;

using Microsoft.Xna.Framework;
using SurfaceTower.Model.Gun;

namespace SurfaceTower.Model
{
  public class GameModel : BaseModel
  {
    private bool first;

    public void FirstUpdate(GameTime gameTime)
    {
      // [ To be removed once audio is merged in [
      Music.TimeSignature = new TimeSignature(4, 4);
      Music.Tempo = 60;
      Music.ClicksPerBeat = 8;
      // ]]

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
