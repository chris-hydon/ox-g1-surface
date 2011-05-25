using System;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace SurfaceTower.Model.Gun
{
  public class ShotPattern
  {
    private float orientMod;
    private Vector2 posMod;
    private Effects effects;

    #region Properties

    public float OrientationModifier
    {
      get { return orientMod; }
    }

    public Vector2 PositionModifier
    {
      get { return posMod; }
    }

    public Effects Effects
    {
      get { return effects; }
      set { effects = value; }
    }

    #endregion

    /// <summary>
    /// A ShotPattern stores the characteristics of a shot.
    /// </summary>
    /// <param name="orientMod">How much to alter the orientation by.</param>
    /// <param name="posMod">How much to alter the position by.</param>
    /// <param name="waves">Number of groups to spawn before expiring the generator.</param>
    public ShotPattern(float orientMod, Vector2 posMod, Effects effects)
    {
      this.orientMod = orientMod;
      this.posMod = posMod;
      this.effects = effects;
    }
  }

  /// <summary>
  /// An add-only collection of shots, as a simple singly linked list with a fluid API.
  /// </summary>
  public class ShotPatterns : IEnumerable<ShotPattern>
  {
    #region Class Definition

    private ShotPattern me;
    private ShotPatterns next = null;

    public ShotPatterns()
    {
    }

    private ShotPatterns(ShotPattern pattern, ShotPatterns next)
    {
      me = pattern;
      this.next = next;
    }

    public ShotPatterns Add(ShotPattern pattern)
    {
      return new ShotPatterns(pattern, this);
    }

    public IEnumerator<ShotPattern> GetEnumerator()
    {
      return new ShotPatternEnum(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return new ShotPatternEnum(this);
    }

    #region Enumerator

    public class ShotPatternEnum : IEnumerator<ShotPattern>
    {
      ShotPatterns parent;
      ShotPatterns current = null;

      public ShotPatternEnum(ShotPatterns parent)
      {
        this.parent = parent;
      }

      public ShotPattern Current
      {
        get { return current.me; }
      }

      object IEnumerator.Current
      {
        get { return current.me; }
      }

      public bool MoveNext()
      {
        if (current == null)
        {
          current = parent;
        }
        else
        {
          current = current.next;
        }
        return current != null;
      }

      public void Reset()
      {
        current = null;
      }

      public void Dispose()
      {
        current = null;
      }
    }

    #endregion

    #endregion

    #region Patterns

    //Fires a single shot straight ahead with no bullet effects or location offset.
    public static ShotPatterns Simple = new ShotPatterns()
      .Add(new ShotPattern(0, Vector2.Zero, Effects.None));
    
    //Fires 2 shots parallel to eachother, 20 pixels apart.
    public static ShotPatterns TwoShot = new ShotPatterns()
      .Add(new ShotPattern(0, new Vector2(0, -10), Effects.None))
      .Add(new ShotPattern(0, new Vector2(0, 10), Effects.None));

    //Fires 2 shots in a V shape and 1 straight ahead through the middle, with no location offset or bullet effects.
    public static ShotPatterns Spread = new ShotPatterns()
      .Add(new ShotPattern(-0.5f, Vector2.Zero, Effects.None))
      .Add(new ShotPattern(0, Vector2.Zero, Effects.None))
      .Add(new ShotPattern(0.5f, Vector2.Zero, Effects.None));

    public static ShotPatterns SingleHoming = new ShotPatterns()
      .Add(new ShotPattern(0, Vector2.Zero, Effects.Homing));

    #endregion
  }
}
