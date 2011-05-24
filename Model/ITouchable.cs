using Microsoft.Xna.Framework;
using SurfaceTower.Controller;
using Microsoft.Surface.Core;

namespace SurfaceTower.Model
{
  public interface ITouchable
  {
    /// <summary>
    /// Decides whether target exists within this object or not. (i.e. does
    /// a contact at target mean this object is being touched?)
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    bool InRegion(Contact target);

    /// <summary>
    /// The controller object that should handle a touch.
    /// </summary>
    ITouchHandler Controller { get; }
  }
}
