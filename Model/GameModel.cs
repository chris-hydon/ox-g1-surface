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
      Music.Bar += new EventHandler(OnBar);
      Tower.ZeroHealth += new EventHandler(GameOver);
      spawner = new Spawner();
      Music.Start(gameTime.TotalRealTime);
    }

    void GameOver(object sender, EventArgs e)
    {
      // Stop sending click/beat/bar effects - music may still be playing though (e.g. outro).
      Music.Stop();
    }

    void OnBar(object sender, EventArgs e)
    {
      int prog = (Progress / 10);
      int[] turrets = new int[4] {0, 0, 0, 0};
      int totalTurrets = 0;

      foreach (Turret t in Turrets)
      {
        turrets[t.PlayerId]++;
        totalTurrets++;
      }

      if (prog > totalTurrets)
      {
        int playerId = -1;
        if (turrets[0] == 0 && Players[0].IsActive) playerId = 0;
        else if (turrets[1] == 0 && Players[1].IsActive) playerId = 1;
        else if (turrets[2] == 0 && Players[2].IsActive) playerId = 2;
        else if (turrets[3] == 0 && Players[3].IsActive) playerId = 3;
        else if (turrets[0] == 1 && Players[0].IsActive && prog > 4) playerId = 0;
        else if (turrets[1] == 1 && Players[1].IsActive && prog > 4) playerId = 1;
        else if (turrets[2] == 1 && Players[2].IsActive && prog > 4) playerId = 2;
        else if (turrets[3] == 1 && Players[3].IsActive && prog > 4) playerId = 3;

        if (playerId != -1)
        {
          Vector2 originPoint = Tower.Location;
          switch (playerId)
          {
            case 0:
              originPoint -= new Vector2(0, originPoint.Y / 4);
              break;
            case 1:
              originPoint += new Vector2(originPoint.X / 4, 0);
              break;
            case 2:
              originPoint += new Vector2(0, originPoint.Y / 4);
              break;
            case 3:
              originPoint -= new Vector2(originPoint.X / 4, 0);
              break;
          }
          CreateTurret(originPoint, playerId);
        }
      }
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
