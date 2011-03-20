using Microsoft.Xna.Framework;

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
    /// A BarRhythm represents the rhythm of a single bar. Its property, notes, is expected to have a length
    /// that is a multiple of (TimeSignature.number * TimeSignature.unit). In notes, false represents a click
    /// where nothing is played for the given voice, while true represents a click where some note is played.
    /// </summary>
    /// <param name="notes">The list of notes to be played.</param>
    public BarRhythm(bool[] notes)
    {
      this.notes = notes;
    }
  }

  public class Music
  {
    private GameTime lastClick, lastBeat, lastBar;
    private TimeSignature timeSignature;
    private Voice[] voices = new Voice[4] { new Voice(0), new Voice(1), new Voice(2), new Voice(3) };

    #region Properties

    public GameTime LastClick
    {
      get { return lastClick; }
    }

    public GameTime LastBeat
    {
      get { return lastBeat; }
    }
    
    public GameTime LastBar
    {
      get { return lastBar; }
    }
    
    public TimeSignature TimeSignature
    {
      get { return timeSignature; }
      set { timeSignature = value; }
    }

    public Voice[] Voices
    {
      get { return voices; }
    }

    #endregion

    #region Voices

    public class Voice
    {
      private bool active = false;
      private BarRhythm currentBarRhythm, nextBarRhythm;
      private int playerId;

      public Voice(int playerId)
      {
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

      #endregion
    }

    #endregion
  }
}
