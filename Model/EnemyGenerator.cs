using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SurfaceTower.Model
{
  class EnemyGenerator
   {
    public static EnemyGenerator Instance = new EnemyGenerator();
    protected Random random = new Random();
    protected BaseModel model = App.Instance.Model;
    protected double lastSpawn;
    
    public void Update()
    {
      int width = App.Instance.GraphicsDevice.Viewport.Width;
      int height = App.Instance.GraphicsDevice.Viewport.Height;
      if ((model.Living.Count < Constants.MAX_ENEMY_COUNT) &&
        (model.LastUpdate.TotalMilliseconds - lastSpawn >= Constants.SPAWN_INTERVAL))
      {
        lastSpawn = model.LastUpdate.TotalMilliseconds;
        int numEnemies = 1 + model.NumberOfPlayers * (int)Math.Round(Constants.MAX_SPAWNING * random.NextDouble());
        for (int i = 1; i <= numEnemies; i++)
        {
          int enemySize = 1 + (int)Math.Round(Constants.LARGEST_ENEMIES * random.NextDouble());
          double angle = Math.PI * random.NextDouble() * 2;
          int x = width/2 + (int)(width/2 * Math.Cos(angle));
          int y = height/2 + (int)(height/2 * Math.Sin(angle));
          model.Spawn(new Enemy(new Vector2(x, y), 0f, enemySize, enemySize * 10, 0.1f * (model.Tower.Location - new Vector2(x,y)), Color.DarkSeaGreen));
        }
      }
    }


  }
}
