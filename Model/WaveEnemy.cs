using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SurfaceTower.Model
{
  /// <summary>
  /// A WaveEnemy moves towards the centre, dodging from side to side.
  /// </summary>
  public class WaveEnemy : Enemy
  {
    Vector2 normal;
    Vector2 displacement;
    Vector2 waveVelocity = Vector2.Zero;

    public WaveEnemy(Vector2 location, int size, int health, Vector2 velocity, int player) :
      base(location, size, health, velocity, player)
    {
      normal = Vector2.Transform(velocity/velocity.Length(), Matrix.CreateRotationZ((float)Math.PI / 2));
      displacement = normal * 50;
    }

    /// <summary>
    /// WaveEnemies move with their usual velocity plus simple harmonic motion.
    /// The formula is -x^2 * d, where d is the displacement from the middle of
    /// the wave and x is the number of radians per second to oscilate.
    /// </summary>
    public override void Move()
    {
      waveVelocity += Acceleration / Constants.UPDATES_PER_SECOND;
      displacement += waveVelocity / Constants.UPDATES_PER_SECOND;
      Acceleration = -100 * displacement;
      base.Move();
    }
  }
}
