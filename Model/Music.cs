using Microsoft.Xna.Framework;
using System;

using SurfaceTower.Model.EventArguments;

namespace SurfaceTower.Model
{
  public struct TimeSignature
  {
    public int number, unit;

    public TimeSignature(int number, int unit)
    {
      this.number = number;
      this.unit = unit;
    }
  }

  public struct BarRhythm
  {
    public readonly bool[] notes;

    /// <summary>
    /// A BarRhythm represents the rhythm of a single bar. Its property, notes, is expected to have length
    /// (TimeSignature.number * clicksPerBeat). In notes, false represents a click where nothing is played
    /// for the given voice, while true represents a click where some note is played.
    /// </summary>
    /// <param name="notes">The list of notes to be played.</param>
    public BarRhythm(bool[] notes)
    {
      this.notes = notes;
    }
  }

  public class Music
  {
    // TimeSpan (since the start of the game) of the most recent click/beat/bar. Set before Click, Beat and Bar events happen.
    private TimeSpan lastClick, lastBeat, lastBar;
    // The current time signature. This defines what a beat is: TimeSignature.unit is the beat unit, so 4 = crotchet, 2 = minim,
    // 8 = quaver, etc.
    private TimeSignature timeSignature;
    // Current tempo in beats per minute.
    private int tempo;
    // Current number of clicks per beat.
    private int clicksPerBeat;
    // Up to four voices: one voice per player. Voices may be inactive.
    private Voice[] voices;

    public Music()
    {
      voices = new Voice[4] { new Voice(this, 0), new Voice(this, 1), new Voice(this, 2), new Voice(this, 3) };
    }

    #region Properties

    public TimeSpan LastClick
    {
      get { return lastClick; }
    }

    public TimeSpan LastBeat
    {
      get { return lastBeat; }
    }
    
    public TimeSpan LastBar
    {
      get { return lastBar; }
    }
    
    public TimeSignature TimeSignature
    {
      get { return timeSignature; }
      set { timeSignature = value; }
    }

    public int Tempo
    {
      get { return tempo; }
      set { tempo = value; }
    }

    public int ClicksPerBeat
    {
      get { return clicksPerBeat; }
      set { clicksPerBeat = value; }
    }

    public Voice[] Voices
    {
      get { return voices; }
    }

    #endregion

    #region Events

    public event EventHandler Bar;
    public event EventHandler Beat;
    public event EventHandler Click;

    #endregion

    #region Methods

    public void Start(TimeSpan time)
    {
      lastClick = lastBeat = lastBar = time;
      App.Instance.Model.Update += new EventHandler<UpdateArgs>(OnUpdate);
    }

    /// <returns>The total duration of each bar, in milliseconds.</returns>
    public int BarDuration()
    {
      // 60000ms per minute, at Tempo beats per minute, times beats per bar.
      return BeatDuration() * TimeSignature.number;
    }

    /// <returns>The total duration of each beat, in milliseconds.</returns>
    public int BeatDuration()
    {
      // 60000ms per minute at Tempo beats per minute.
      return 60000 / Tempo;
    }

    /// <returns>The total duration of each click, in milliseconds.</returns>
    public int ClickDuration()
    {
      return BeatDuration() / ClicksPerBeat;
    }

    /// <summary>
    /// Called whenever the game sends an update event. This method is responsible for determining whether or not a
    /// Click, Beat or Bar event should occur.
    /// </summary>
    /// <param name="sender">The BaseModel object which sent the Update signal.</param>
    /// <param name="args">UpdateArgs object containing the current TimeSpan snapshot.</param>
    public void OnUpdate(object sender, UpdateArgs args)
    {
      TimeSpan time = args.Time;
      if ((time - lastBar).TotalMilliseconds > BarDuration())
      {
        lastClick = lastBeat = lastBar = time;
        if (Bar != null) Bar(this, null);
        if (Beat != null) Beat(this, null);
        if (Click != null) Click(this, null);
      }
      else if ((time - lastBeat).TotalMilliseconds > BeatDuration())
      {
        lastClick = lastBeat = time;
        if (Beat != null) Beat(this, null);
        if (Click != null) Click(this, null);
      }
      else if ((time - lastClick).TotalMilliseconds > ClickDuration())
      {
        lastClick = time;
        if (Click != null) Click(this, null);
      }
    }

    #endregion

    #region Voices

    public class Voice
    {
      private bool active = false;
      private BarRhythm currentBarRhythm, nextBarRhythm;
      private int playerId;
      private Music parent;

      public Voice(Music parent, int playerId)
      {
        this.parent = parent;
        this.playerId = playerId;
      }

      #region Properties

      public BarRhythm CurrentBarRhythm
      {
        get { return currentBarRhythm; }
      }

      public BarRhythm NextBarRhythm
      {
        get { return nextBarRhythm; }
        set
        {
          currentBarRhythm = nextBarRhythm;
          nextBarRhythm = value;
        }
      }

      public int PlayerId
      {
        get { return playerId; }
      }

      public bool IsActive
      {
        get { return active; }
        set
        {
          // Can only be changed via MainTurret!
          if (App.Instance.Model.Players[PlayerId].IsActive != value || active == value)
          {
            throw new InvalidOperationException();
          }
          this.active = value;
        }
      }

      #endregion
    }

    #endregion
  }
}
