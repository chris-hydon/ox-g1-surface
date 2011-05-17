using Microsoft.Xna.Framework;

namespace SurfaceTower
{
  public static class Constants
  {
    public const float BASE_IMPROVOCITY = 0.001f; // Base improvocity per kill, multiplied by enemy size.
    public const float BULLET_HOMING_TURNSPEED = 1; // Perpendicular acceleration relative to velocity
    public const int BULLET_LIFE = 20; // Seconds
    public const int BULLET_VELOCITY = 200; // Metres per second
    public const int MAIN_TURRET_DEFAULT_POWER = 10;
    public const float MAIN_TURRET_RADIUS = 100.0f;
    public const int TURRET_DEFAULT_POWER = 500;
    public const float TURRET_RADIUS = 10.0f;
    public const float TURRET_TURNSPEED = 1f; // Radians per second
    public const float UPDATES_PER_SECOND = 60;
    public const float MAX_ENEMY_COUNT = 200;
    // This is multiplied by the number of players - this is the maximum amount of enemies which can spawn per player, per spawn.
    public const int MAX_SPAWNING = 100;
    public const int LARGEST_ENEMIES = 30;
    public const int SPAWN_INTERVAL = 1000;
  }
}
