using Microsoft.Xna.Framework;

namespace SurfaceTower
{
  public static class Constants
  {
    public const float BASE_IMPROVOCITY = 1f; // Base improvocity per kill, multiplied by enemy size.
    public const float BULLET_HOMING_TURNSPEED = 1; // Perpendicular acceleration relative to velocity
    public const int BULLET_LIFE = 20; // Seconds
    public const int BULLET_VELOCITY = 200; // Metres per second
    public const int HP_LIMIT_CRITICAL_TOWER = 100;
    public const int MAIN_TURRET_DEFAULT_POWER = 10;
    public const float MAIN_TURRET_RADIUS = 100.0f;
    public const float PROGRESSION_RATE = 1; // Ranks per bar
    public const int TOWER_DEFAULT_HEALTH = 1000;
    public const int TOWER_REGENERATION = 5; // Regen per beat
    public const int TURRET_DEFAULT_POWER = 20;
    public const float TURRET_WIDTH = 50;
    public const float TURRET_HEIGHT = 30;
    public const float TURRET_TURNSPEED = 1f; // Radians per second
    public const float UPDATES_PER_SECOND = 60;
    public const float MAX_ENEMY_COUNT = 200;
    // This is the maximum amount of enemies which can be on the screen and still have a new wave spawn.
    public const int LARGEST_ENEMIES = 30;
    public const int BURN_DAMAGE = 1;
  }
}
