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
      // width and height are set to the bottom-right of the screen
      int width = App.Instance.GraphicsDevice.Viewport.Width;
      int height = App.Instance.GraphicsDevice.Viewport.Height;
      //if there is room for more enemies, and it has been long enough since the last spawn
      if ((model.Living.Count < Constants.MAX_ENEMY_COUNT) &&
        (model.LastUpdate.TotalMilliseconds - lastSpawn >= Constants.SPAWN_INTERVAL))
      {
        //record the new last spawn time
        lastSpawn = model.LastUpdate.TotalMilliseconds;
        //determine how many enemies will spawn - limit set in Constants.cs
        int numEnemies = 1 + model.NumberOfPlayers * (int)Math.Round(Constants.MAX_SPAWNING * random.NextDouble());
        for (int i = 1; i <= numEnemies; i++)
        {
          //random enemy size - limit set in Constants.cs
          int enemySize = 1 + (int)Math.Round(Constants.LARGEST_ENEMIES * random.NextDouble());
          //enemies spawn on a circle around the centre - angle determines the random place on that circle
          double angle = Math.PI * random.NextDouble() * 2;
          //x and y position of the enemy is on the circle, which is outside of the screen.
          int x = width/2 + (int)(width * Math.Cos(angle));
          int y = height/2 + (int)(height * Math.Sin(angle));
          //spawn an enemy which will move towards the centre (and therefore onto the screen)
          model.Spawn(new Enemy(new Vector2(x, y), 0f, enemySize, enemySize * 10, 0.1f * (model.Tower.Location - new Vector2(x,y)), Color.DarkSeaGreen));
        }
      }
    }


  }
}
