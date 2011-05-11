using Microsoft.Xna.Framework;

namespace SurfaceTower
{
  public static class Constants
  {
    public const float BULLET_HOMING_TURNSPEED = 1; // Perpendicular acceleration relative to velocity
    public const int BULLET_LIFE = 20; // Seconds
    public const int BULLET_VELOCITY = 200; // Metres per second
    public const int MAIN_TURRET_DEFAULT_POWER = 10;
    public const float MAIN_TURRET_RADIUS = 20.0f;
    public const int TURRET_DEFAULT_POWER = 5;
    public const float TURRET_RADIUS = 10.0f;
    public const float TURRET_TURNSPEED = 1f; // Radians per second
    public const float UPDATES_PER_SECOND = 60;
  }
}
