using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using SurfaceTower.VideoEngine.MenuDrawers;

namespace SurfaceTower.VideoEngine
{
    /// <summary>
    /// Creates a line menu in front of a given player.
    /// </summary>
    public abstract class LineMenu : IMenu
    {
        protected int numberofitems;
        private const int ANIMATIONSPEED = 150;
        protected Vector2 position;
        protected Vector2[] offsets;
        protected MenuItem[] items;
        protected float singleItemWidth;
        private Vector2 direction;
        /// <summary>
        /// Represents the animation state:
        /// 1 - opening
        /// 0 - open
        /// -1 - closing
        /// </summary>
        private const int OPENING = 0;
        private const int OPEN = 1;
        private const int CLOSING = 2;
        private const int CLOSED = 3;
        private short animationstate;
     

        public LineMenu(Vector2 position, int playerID)
        {
            //Makes the menu animated in the direction of the player
            switch (playerID)
            {
                case 0:
                    direction = new Vector2(0, -1);
                    break;
                case 1:
                    direction = new Vector2(1, 0);
                    break;
                case 2:
                    direction = new Vector2(0, 1);
                    break;
                case 3:
                    direction = new Vector2(-1, 0);
                    break;
            }
            animationstate = OPENING;
            this.position = position;
         
        }

        public void Draw(SpriteBatch sb)
        {
            int i = 0;
            foreach (MenuItem t in items)
            {
                t.position = position + offsets[i];
                t.Draw(sb);
                i++;
                if (animationstate == CLOSING)
                {
                    if (t.alpha > 15)
                    {
                        t.alpha -= 15;
                    }
                    else
                    {
                        t.alpha = 0;
                    }
                }
            }
            if (animationstate != OPEN)
            {
                updateAnimation();
            }
        }

        public void Close()
        {
            if (animationstate == OPEN)
            {
                animationstate = CLOSING;
            }
        }
        public bool Finished()
        {
            return (animationstate == CLOSED);
        }

        private void updateAnimation()
        {
            if (animationstate == OPENING)
            {
                for (int i = 0; i < offsets.Count<Vector2>(); i++)
                {
                    offsets[i] += (1 / singleItemWidth) * ANIMATIONSPEED * i * direction;
                }
                Vector2 v = offsets.Last() - offsets.First();
                if (v.Length() > (singleItemWidth * numberofitems))
                {
                    animationstate = OPEN;
                }
                return;
            }
            if (animationstate == CLOSING)
            {
                for (int i = 0; i < offsets.Count<Vector2>(); i++)
                {
                    offsets[i] -= (1 / singleItemWidth) * ANIMATIONSPEED * i * direction;
                }
                Vector2 v = offsets.Last() - offsets.First();
                if (v.Length() <= 5)
                {
                    animationstate = CLOSED;
                }
            }
            
        }
    }
}
