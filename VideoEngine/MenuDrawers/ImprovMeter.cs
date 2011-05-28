using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Surface.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SurfaceTower.VideoEngine.MenuDrawers
{
    public class ImprovMeter
    {
        private float improvosity;
        private Contact tokenPos;
        private int player;

        Texture2D semicirc = App.Instance.Content.Load<Texture2D>("semi");
        public ImprovMeter(int player)
        {
            this.player = player;
        }

        public void Draw(SpriteBatch sb)
        {
            tokenPos = App.Instance.Controller.GetContactByPlayerId(player);
            if (tokenPos != null)
            {
                
                improvosity = App.Instance.Model.Players.ElementAt(player).Improvocity;
                if (improvosity < 0.5f)
                {
                    sb.Draw(semicirc, new Vector2(tokenPos.X, tokenPos.Y), null, Color.HotPink, 0, new Vector2(0, semicirc.Height / 2), 1.02f*tokenPos.Bounds.Width / semicirc.Width, SpriteEffects.None, 0);
                    sb.Draw(semicirc, new Vector2(tokenPos.X, tokenPos.Y), null, Color.Black, 0, new Vector2(semicirc.Width, semicirc.Height / 2), 1.02f*tokenPos.Bounds.Width / semicirc.Width, SpriteEffects.FlipHorizontally, 0);
                    sb.Draw(semicirc, new Vector2(tokenPos.X, tokenPos.Y), null, Color.Black, (float)Math.PI*improvosity*2, new Vector2(0, semicirc.Height / 2), 1.04f*tokenPos.Bounds.Width / semicirc.Width, SpriteEffects.None, 0);
                }
                if (improvosity >= 0.5f)
                {
                   
                    sb.Draw(semicirc, new Vector2(tokenPos.X, tokenPos.Y), null, Color.HotPink, 0, new Vector2(semicirc.Width, semicirc.Height / 2), 1.02f*tokenPos.Bounds.Width / semicirc.Width, SpriteEffects.FlipHorizontally, 0);
                    sb.Draw(semicirc, new Vector2(tokenPos.X, tokenPos.Y), null, Color.Black, (float)Math.PI * improvosity * 2, new Vector2(0, semicirc.Height / 2), 1.02f*tokenPos.Bounds.Width / semicirc.Width, SpriteEffects.None, 0);
                    sb.Draw(semicirc, new Vector2(tokenPos.X, tokenPos.Y), null, Color.HotPink, 0, new Vector2(0, semicirc.Height / 2), 1.02f*tokenPos.Bounds.Width / semicirc.Width, SpriteEffects.None, 0);

                }
            }
        }

    }
}
