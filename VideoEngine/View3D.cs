using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SurfaceTower.Model;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SurfaceTower.VideoEngine
{
    public class View3D : View
    {
        //The Game Model being displayed
        private BaseModel baseModel;
        private GraphicsDeviceManager graphicsManager;
        //Direct link to the graphics device, a useful shortcut
        private GraphicsDevice device;
        //Holds the content, eg Effects, Fonts, Sprites etc
        private ContentManager content;
        //The effect being applied to the rendered vertices
        private Effect effect; 
        //Vertex Decleration
        private VertexDeclaration vertDecleration;

        public View3D(BaseModel baseModel, GraphicsDeviceManager graphicsManager, ContentManager content)
        {
            this.baseModel = baseModel;
            this.graphicsManager = graphicsManager;
            this.content = content;
            device = graphicsManager.GraphicsDevice;
            //Initialize background color
            device.Clear(Color.DarkGoldenrod);
            effect = content.Load<Effect>("simpleEffects");
            //Specifies which technique from the effect file we'll be using
            effect.CurrentTechnique = effect.Techniques["Pretransformed"];

            vertDecleration = new VertexDeclaration(device, VertexPositionColor.VertexElements);
            
            

            
        }

        //setting up vertices, test/example NON GAME CODE
        VertexPositionColor[] vertices;
        private void setUpVertices()
        {
            vertices = new VertexPositionColor[3];
            vertices[0].Position = new Vector3(-0.5f, -0.5f, 0f);
            vertices[0].Color = Color.Red;
            vertices[1].Position = new Vector3(0, 0.5f, 0f);
            vertices[1].Color = Color.Green;
            vertices[2].Position = new Vector3(0.5f, -0.5f, 0f);
            vertices[2].Color = Color.Yellow;
        }


        public void draw(GameTime gameTime){
            setUpVertices();
            device.Clear(Color.DarkSlateBlue);
            effect.CurrentTechnique = effect.Techniques["Pretransformed"];
            effect.Begin();
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Begin();
                device.VertexDeclaration = vertDecleration;
                device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertices, 0, 1);
                pass.End();
            }
            effect.End();
            
        }
    }
}
