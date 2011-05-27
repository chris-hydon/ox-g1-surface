using System;

using Microsoft.Xna.Framework;
using SurfaceTower.Model.Shape;
using Microsoft.Xna.Framework.Graphics;

namespace SurfaceTower.Model
{
  public class Invader : Enemy
  {
    protected int region = 0;
    protected float speed = 300f;
    #region Properties
    new public int Player
    {
      get { return player; }
    }
    #endregion

    #region Methods

    public Invader(Vector2 location) : base(location, 60, 5000, Vector2.Zero, -1)
    { }

    public override void Move()
    {
      int width = App.Instance.GraphicsDevice.Viewport.Width;
      int height = App.Instance.GraphicsDevice.Viewport.Height;
      int radius = (int) Math.Round(Constants.MAIN_TURRET_RADIUS);
      int oldregion = region;
      region = 0;
      //region will store the area that the invader is in, using the same system as the Cohen-Sutherland
      //algorithm for clipping an edge against a rectangle
      if (Location.X < width/2 - radius)
      {
        region |= 0x1;
      }
      if (Location.X > width/2 + radius)
      {
        region |= 0x2;
      }
      if (Location.Y < height/2 - radius)
      {
        region |= 0x8;
      }
      if (Location.Y > height/2 + radius)
      {
        region |= 0x4;
      }
      //if the region changed to a side region, then move closer to the tower, in the style of a space invader.
      if (region != oldregion && isAPowerOf2(region))
      {
        Vector2 direction = (Location - App.Instance.Model.Tower.Location);
        direction.Normalize();
        Location -=  direction * 20;
      }
      
      //Set direction of velocity based on the region the invader is in.
      Vector2 dir = Vector2.Zero;
      if (isAPowerOf2(region))
      {
        //region is a side, not a corner: Move orthogonally to the tower.
        switch (region)
        {
          case 1: dir = new Vector2(0, -1); break; //left side
          case 2: dir = new Vector2(0, 1); break; //right side
          case 4: dir = new Vector2(-1, 0); break; //bottom side
          case 8: dir = new Vector2(1, 0); break; //top side
        }
        Acceleration = Vector2.Zero;
      }
      else
      {
        //region is a corner (or is the centre, but invader would be destroyed were it within the tower)
        //so move in a circular motion clockwise, to reach the next side region
        float xMod = (region & 0x2) != 0 ? radius : -radius;
        float yMod = (region & 0x4) != 0 ? radius : -radius;
        dir = Location - (App.Instance.Model.Tower.Location + new Vector2(xMod, yMod));
        dir.Normalize();
        Acceleration = -speed * dir;
        dir = Vector2.Transform(dir, Matrix.CreateRotationZ((float)(Math.PI / 2)));
      }
      //Set velocity
      Velocity = speed * dir;
      base.Move();
      System.Console.WriteLine(Location);
    }


    protected bool isAPowerOf2(int num)
    {
      int nextpow = 1;
      while (nextpow < num)
      {
        nextpow *= 2;
      }
      return (nextpow == num);
    }

    #endregion

  }
}
