using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using SurfaceTower.Model.Gun;

namespace SurfaceTower.VideoEngine.MenuDrawers
{ 
    public class MenuManager
    {
        private ICollection<IMenu> menus = new LinkedList<IMenu>();

        public MenuManager()
        {
            menus.Add(new UGMenu(new Vector2(300,300), 0));
            foreach (MainGun p in App.Instance.Model.Players)
            {
               p.UpgradeReady+=new EventHandler(p_UpgradeReady);
            }
        }

        void  p_UpgradeReady(object sender, EventArgs e)
        {
            MainGun p = (MainGun)sender;
            menus.Add(new UGMenu(p.Location, p.PlayerId));
        }   

        public void Draw(SpriteBatch sb)
        {
            foreach (IMenu m in menus)
            {
                if (m != null)
                {
                    m.Draw(sb);
                }
            }
        }


    }
}
