using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SurfaceTower.Model
{
  public class BaseModel
  {
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
  }
}
