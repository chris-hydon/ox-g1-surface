using Microsoft.Xna.Framework;
using SurfaceTower.Controller;

namespace SurfaceTower.VideoEngine.Touchable
{
  public interface ITouchable
  {
    /// <summary>
    /// Decides whether target exists within this object or not. (i.e. does
    /// a contact at target mean this object is being touched?)
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    bool InRegion(Vector2 target);

    /// <summary>
    /// The controller object that should handle a touch.
    /// </summary>
    ITouchHandler Controller { get; }
  }
}
