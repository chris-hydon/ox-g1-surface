using Microsoft.Surface.Core;

namespace SurfaceTower.Controller
{
  public interface ITouchHandler
  {
    void Press(Contact contact, int playerId);
    void Release(Contact contact, int playerId);
    void Touch(int playerId);
  }
}
