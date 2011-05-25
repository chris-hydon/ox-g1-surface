using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SurfaceTower.Model
{
  public class SpiralEnemy : Enemy
  {
    Vector2 initialVelocity;

    /// <summary>
    /// A SpiralEnemy will spiral inwards, towards the tower.
    /// </summary>
    public SpiralEnemy(Vector2 location, int size, int health, Vector2 velocity, int player) :
      base(location, size, health, Vector2.Transform(App.Instance.Model.Tower.Location - location, Matrix.CreateRotationZ((float)Math.PI / 2)), player)
    {
      //InitialVelocity needs to be set to enable the spiralling.
      initialVelocity = Vector2.Transform(App.Instance.Model.Tower.Location - location, Matrix.CreateRotationZ((float)Math.PI / 2));
    }

    public override void Move()
    {
      Vector2 diff = App.Instance.Model.Tower.Location - Location;
      float difflen = diff.Length();
      diff.Normalize();
      //Acceleration has 2 components: one points towards the centre to get a circular effect, the other slows the enemy to make the spiral effect.
      acceleration = 0.55f * (((initialVelocity.Length() * initialVelocity.Length()) / difflen) * diff) - ((float)0.25 * Velocity);
      base.Move();
    }
  }
}
