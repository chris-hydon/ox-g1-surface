using Microsoft.Surface.Core;

namespace SurfaceTower.Controller
{
  public interface ITouchHandler
  {
    void Press(ContactData contact, int playerId);
    void Release(ContactData contact, int playerId);
    void Touch(ContactData contact, int playerId);
  }
}
