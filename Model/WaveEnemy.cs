using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SurfaceTower.Model
{
  public class WaveEnemy : Enemy
  {
    Vector2 normal;
    int add = 1;
    Vector2 displacement;
    Vector2 waveVelocity = Vector2.Zero;

    public WaveEnemy(Vector2 location, float orientation, int size, int health, Vector2 velocity, Color colour) :
      base(location, orientation, size, health, velocity, colour)
    {
      normal = Vector2.Transform(velocity/velocity.Length(), Matrix.CreateRotationZ((float)Math.PI / 2));
      displacement = normal * 50;
      App.Instance.Model.Music.Beat += new EventHandler(OnBeat);
    }

    void OnBeat(object sender, EventArgs e)
    {
      add *= -1;
    }

    public override void Move()
    {
      waveVelocity += Acceleration / Constants.UPDATES_PER_SECOND;
      displacement += waveVelocity / Constants.UPDATES_PER_SECOND;
      Acceleration = -100 * displacement;
      base.Move();
    }
  }
}
