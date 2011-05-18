using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SurfaceTower.Model
{
  class CircleGenerator : IGenerator
   {
    protected Random random = new Random();
    protected BaseModel model = App.Instance.Model;
    // width and height are set to the width and height of the screen
    int width = App.Instance.GraphicsDevice.Viewport.Width;
    int height = App.Instance.GraphicsDevice.Viewport.Height;
    int amount;
    bool done = false;

    public bool Done
    {
      get { return done; }
    }
    public CircleGenerator(int amount)
    {
      this.amount = amount;
    }
    
    public void Generate()
    {
      for (int i = 1; i <= amount; i++)
      {
        //random enemy size - limit set in Constants.cs
        int enemySize = (int)(1 + model.LastUpdate.TotalMinutes + Math.Round(Constants.LARGEST_ENEMIES * random.NextDouble()));
        
        //enemies spawn on a circle around the centre - angle determines the random place on that circle
        double angle = Math.PI * random.NextDouble() * 2;
        //x and y position of the enemy is on the circle, which is outside of the screen.
        
        int x = width/2 + (int)(width * Math.Cos(angle));
        int y = height/2 + (int)(height * Math.Sin(angle));
        //spawn an enemy which will move towards the centre (and therefore onto the screen)
        model.Spawn(new SpiralEnemy(new Vector2(x, y), 0f, enemySize, enemySize * 10, 0.1f * (2*(model.Tower.Location - new Vector2(x,y))), Color.DarkSeaGreen));
        done = true;
      }
    }
  }
}
