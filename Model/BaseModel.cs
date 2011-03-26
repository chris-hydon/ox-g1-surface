using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using SurfaceTower.Model.EventArguments;

namespace SurfaceTower.Model
{
  public class BaseModel
  {
    private bool first;
    private Music music = new Music();
    private MainTurret[] players = new MainTurret[4] { new MainTurret(0), new MainTurret(1), new MainTurret(2), new MainTurret(3) };
    private ICollection<Turret> turrets = new LinkedList<Turret>();

    #region Properties

    public Music Music
    {
      get { return music; }
    }

    public MainTurret[] Players
    {
      get { return players; }
    }

    public ICollection<Turret> Turrets
    {
      get { return turrets; }
    }

    #endregion

    #region Events

    public event EventHandler<UpdateArgs> Update;

    #endregion

    #region Methods

    public void FirstUpdate(GameTime gameTime)
    {
      // Some simple demo stuff.
      players[0].IsActive = true;
      players[0].Orientation = 45;

      // Audio folks, something like this should be in your initialize, not mine!
      Music.TimeSignature = new TimeSignature(4, 4);
      Music.Tempo = 60;
      Music.ClicksPerBeat = 8;
      Music.Start(gameTime.TotalRealTime);

      Music.Beat += new EventHandler(OnBeat);
    }

    public void OnUpdate(GameTime gameTime)
    {
      if (!first)
      {
        FirstUpdate(gameTime);
        first = true;
      }
      Update(this, new UpdateArgs(gameTime.TotalRealTime));
    }

    /// <summary>
    /// Called in response to the Music.Beat signal.
    /// </summary>
    /// <param name="sender">The Music object which sent the Beat signal.</param>
    /// <param name="e">Always null</param>
    public void OnBeat(object sender, EventArgs e)
    {
      // Some demo stuff.
      players[0].Orientation += 18;
    }

    #endregion
  }
}
