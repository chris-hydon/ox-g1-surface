using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SurfaceTower.VideoEngine.ParticleEngine
{
    public class PEngine
    {
        const int MAXEMITTERS = 50;
        private ICollection<Emitter> emitters;
        private SimpleView view;
        private Texture2D tex;

        public PEngine(SimpleView view)
        {
            this.view = view;
            emitters = new List<Emitter>(MAXEMITTERS);
            tex = view.content.Load<Texture2D>("particle");
        }

        public void addEmitter(Vector2 position)
        {
            emitters.Add(new Emitter(position, tex));
        }

        public void Update()
        {
            foreach (Emitter e in emitters)
            {
                e.Update();
            }
        }

        public void Draw()
        {
            foreach (Emitter e in emitters)
            {
                e.Draw(view.spritebatch);
            }

        }





    }
}
