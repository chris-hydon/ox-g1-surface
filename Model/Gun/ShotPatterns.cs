﻿using System;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace SurfaceTower.Model.Gun
{
  public struct ShotPattern
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

    private Node head = null;
    public ShotPatterns Add(ShotPattern pattern)
    {
      Node oldHead = head;
      head = new Node();
      head.me = pattern;
      head.next = oldHead;
      return this;
    }

    public IEnumerator<ShotPattern> GetEnumerator()
    {
      return new ShotPatternEnum(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return new ShotPatternEnum(this);
    }

    #region Node

    private class Node
    {
      public ShotPattern me;
      public Node next;
    }

    #endregion

    #region Enumerator

    public class ShotPatternEnum : IEnumerator<ShotPattern>
    {
      ShotPatterns parent;
      Node enumHead = null;

      public ShotPatternEnum(ShotPatterns parent)
      {
        this.parent = parent;
      }

      public ShotPattern Current
      {
        get { return enumHead.me; }
      }

      object IEnumerator.Current
      {
        get { return enumHead.me; }
      }

      public bool MoveNext()
      {
        if (enumHead == null)
        {
          enumHead = parent.head;
        }
        else
        {
          enumHead = enumHead.next;
        }
        return enumHead != null;
      }

      public void Reset()
      {
        enumHead = null;
      }

      public void Dispose()
      {
        enumHead = null;
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

    #endregion
  }
}
