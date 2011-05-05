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
        //Indices
        int[] indices;
        //Camera Matrices
        private Matrix viewMatrix;
        private Matrix projectionMatrix;



        public View3D(BaseModel baseModel, GraphicsDeviceManager graphicsManager, ContentManager content)
        {
            this.baseModel = baseModel;
            this.graphicsManager = graphicsManager;
            this.content = content;
            device = graphicsManager.GraphicsDevice;
            //Initialize background color
            device.Clear(Color.DarkGoldenrod);
            effect = content.Load<Effect>("simpleEffects");
            vertDecleration = new VertexDeclaration(device, VertexPositionColor.VertexElements);
            SetUpCamera();
            device.RenderState.FillMode = FillMode.WireFrame;
        }
        private void SetUpCamera()
        {
            viewMatrix = Matrix.CreateLookAt(new Vector3(0, 50, 0), new Vector3(0, 0, 0), new Vector3(0, 0, -1));
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, device.Viewport.AspectRatio, 1.0f, 300.0f);
        }

        //setting up vertices, test/example NON GAME CODE
        VertexPositionColor[] vertices;
        private void setUpVertices()
        {
            vertices = new VertexPositionColor[4];

            vertices[0].Position = new Vector3(0f, 0f, 0f);
            vertices[0].Color = Color.White;
            vertices[1].Position = new Vector3(0f, 0f, -2f);
            vertices[1].Color = Color.White;
            vertices[2].Position = new Vector3(2f, 0f, -2f);
            vertices[2].Color = Color.White;
            vertices[3].Position = new Vector3(2f, 0f, 0f);
            vertices[3].Color = Color.White;
        }
        private void setUpIndices()
        {
            indices = new int[9];

            indices[0] = 1;
            indices[1] = 3;
            indices[2] = 0;
            indices[3] = 2;
            indices[4] = 3;
            indices[5] = 1;
            indices[6] = 2;
            indices[7] = 3;
            indices[8] = 0;
        }


        public void draw(GameTime gameTime)
        {
            //Specify the camera to the shader file
            effect.Parameters["xView"].SetValue(viewMatrix);
            effect.Parameters["xProjection"].SetValue(projectionMatrix);
            effect.Parameters["xWorld"].SetValue(Matrix.Identity);

            setUpVertices();
            setUpIndices();
            device.Clear(Color.DarkSlateBlue);
            effect.CurrentTechnique = effect.Techniques["ColoredNoShading"];
            effect.Begin();
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Begin();
                device.VertexDeclaration = vertDecleration;
                device.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertices, 0, vertices.Length, indices, 0, indices.Length / 3);
                pass.End();
            }
            effect.End();
            
        }
    }
}
