using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SurfaceTower.Model.Generator
{
  public enum EnemyType
  {
    Regular,
    Wave
  }

  public abstract class AbstractGenerator : IGenerator
  {
    protected Random random = new Random();
    private int enemySize;
    private int enemySizeVariance;
    private int enemyHealth;
    private int waveSize;
    private int wavesLeft;
    private int frequency;
    private float multiplayerAdjustment;
    private int generated;
    private EnemyType enemyType;
    private int clickCounter = 0;

    #region Properties

    protected int ScreenWidth
    {
      get { return App.Instance.GraphicsDevice.Viewport.Width; }
    }

    protected int ScreenHeight
    {
      get { return App.Instance.GraphicsDevice.Viewport.Height; }
    }

    /// <summary>
    /// Whether this generator is depleted or not.
    /// </summary>
    public bool Done
    {
      get { return WavesLeft <= 0; }
    }

    /// <summary>
    /// Average size of each enemy spawned.
    /// </summary>
    public int EnemySize
    {
      get { return enemySize; }
      set { enemySize = value; }
    }

    /// <summary>
    /// Amount by which to vary the size.
    /// </summary>
    public int EnemySizeVariance
    {
      get { return enemySizeVariance; }
      set { enemySizeVariance = value; }
    }

    /// <summary>
    /// Amount of health to give each enemy.
    /// </summary>
    public int EnemyHealth
    {
      get { return enemyHealth; }
      set { enemyHealth = value; }
    }

    /// <summary>
    /// The type of enemy to spawn.
    /// </summary>
    public EnemyType EnemyType
    {
      get { return enemyType; }
      set { enemyType = value; }
    }

    /// <summary>
    /// Number of enemies per wave, per player.
    /// </summary>
    public int WaveSize
    {
      get { return waveSize; }
      set { waveSize = value; }
    }

    /// <summary>
    /// Actual number of enemies per wave.
    /// </summary>
    public int EnemiesPerWave
    {
      get { return (int) (waveSize * Math.Pow(multiplayerAdjustment, App.Instance.Model.NumberOfPlayers - 1)); }
    }

    /// <summary>
    /// Number of waves remaining before this generator is depleted.
    /// </summary>
    public int WavesLeft
    {
      get { return wavesLeft; }
    }

    /// <summary>
    /// How often a wave should be spawned (once every [Frequency] clicks).
    /// </summary>
    public int Frequency
    {
      get { return frequency; }
      set { frequency = value; }
    }

    /// <summary>
    /// Multiplier to be applied to waveSize for each additional player beyond the first.
    /// </summary>
    public float MultiplayerAdjustment
    {
      get { return multiplayerAdjustment; }
      set { multiplayerAdjustment = value; }
    }

    #endregion

    #region Methods

    /// <summary>
    /// A generator for creating enemies. This class provides functionality common to most generators.
    /// </summary>
    /// <param name="waves">Number of waves to spawn before expiring the generator.</param>
    public AbstractGenerator(int waves)
    {
      wavesLeft = waves;
    }

    protected virtual Enemy NextEnemy(Vector2 location, float orientation, Vector2 velocity, Color colour)
    {
      generated++;

      // Pick a size for the enemy.
      int size = EnemySize + random.Next(-EnemySizeVariance, EnemySizeVariance);

      switch (enemyType)
      {
        case EnemyType.Regular:
          return new Enemy(location, orientation, size, EnemyHealth, velocity, colour);
        case EnemyType.Wave:
          return new WaveEnemy(location, orientation, size, EnemyHealth, velocity, colour);
        default:
          throw new NotImplementedException();
      }
    }

    /// <summary>
    /// Checks to see if this generator should perform its spawn behaviour this click or not.
    /// Call it from the Generate method. Based on frequency.
    /// </summary>
    /// <returns>True if spawning should happen, false otherwise.</returns>
    protected bool ShouldGenerate()
    {
      if (clickCounter++ % frequency == 0)
      {
        generated = 0;
        wavesLeft--;
        return true;
      }
      return false;
    }

    /// <summary>
    /// Decides whether or not this generator should spawn a new enemy this generate cycle.
    /// </summary>
    /// <returns>True if a new enemy should spawn, false otherwise.</returns>
    protected bool HasNextEnemy()
    {
      return (EnemiesPerWave > generated);
    }

    #endregion

    #region Abstract methods

    public abstract void Generate();

    #endregion
  }
}
