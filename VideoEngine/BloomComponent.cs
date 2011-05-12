using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BloomPostprocess
{
    public class BloomComponent : DrawableGameComponent
    {
        #region Fields

        SpriteBatch spriteBatch;

        //Shader to extract brightest parts of screen
        Effect extractEffect;
        //Shader to combine screen image with bloomed/blurred image
        Effect combineEffect;
        //Shader to gaussian blur the extracted parts of the screen. Used in 2 passes, horizontal and vertical to provide a gaussian blur more efficiently.
        Effect gaussBlurEffect;

        ResolveTexture2D resolveTarget;
        //Temporary render targets to construct the bloom in passes
        RenderTarget2D renderTarget1;
        RenderTarget2D renderTarget2;

        GraphicsDevice device;

        //Bloom settings
        //How bright a pixel has to be sampled for bloom (between 0.25 & 0.5 is good)
        private float bloomThreshold;
        
        //how much bluring is applied (between 1 & 10 is good)
        private float blurAmount;
        
        //Intensity of base and bloom mixed together for final sceen (between 0 & 1)
        private float bloomIntensity;
        private float baseIntensity;

        //Controls saturation of the bloom and base, 1 is as normal, 0 unsaturated, >1 more saturated.
        private float bloomSaturation;
        private float baseSaturation;

        public void setBloomSetting(float bloomThreshold, float blurAmount, float bloomIntensity, float baseIntensity, float bloomSaturation, float baseSaturation)
        {
            this.bloomThreshold = bloomThreshold;
            this.blurAmount = blurAmount;
            this.bloomIntensity = bloomIntensity;
            this.baseIntensity = bloomIntensity;
            this.bloomSaturation = bloomSaturation;
            this.baseSaturation = baseSaturation;
        }

        #endregion

        #region Initialization

        public BloomComponent(Game game, GraphicsDevice device)
            : base(game)
        {
            this.device = device;
            LoadContent();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(device);

            //Load the fx files
            extractEffect = Game.Content.Load<Effect>("BloomExtract");
            combineEffect = Game.Content.Load<Effect>("BloomCombine");
            gaussBlurEffect = Game.Content.Load<Effect>("GaussBlur");


            // Look up the resolution and format of our main backbuffer.
            PresentationParameters pp = device.PresentationParameters;

            int width = pp.BackBufferWidth;
            int height = pp.BackBufferHeight;

            SurfaceFormat format = pp.BackBufferFormat;

            //Set how the bloom will behave
            setBloomSetting(0.25f, 4f, 2f, 1f, 2f, 0f);

            // Create a texture for reading back the backbuffer contents.
            resolveTarget = new ResolveTexture2D(device, width, height, 1, format);

            //Can make the backbuffers smaller to increase efficiency, makes bloom less pronounced
            /**
            width /= 2;
            height /= 2;
            */ //for example 

            renderTarget1 = new RenderTarget2D(device, width, height, 1, format);
            renderTarget2 = new RenderTarget2D(device, width, height, 1, format);
        }

        protected override void UnloadContent()
        {
            resolveTarget.Dispose();
            renderTarget1.Dispose();
            renderTarget2.Dispose();
        }


        #endregion

        #region Draw

       //Draws the bloom: Extracts the brightest parts of screen --> Horizontal gaussian blur --> Vertical gaussian blur --> combine this with original screen
        public override void Draw(GameTime gameTime)
        {
            if (!SurfaceTower.App.Instance.ApplicationActivated) return;

            //Create a texture from the screen
            device.ResolveBackBuffer(resolveTarget);

            // Pass 1: Extract only the brightest parts of resolveTarget onto rendertarget1 
            extractEffect.Parameters["BloomThreshold"].SetValue(bloomThreshold);
            DrawToRenderTarget(resolveTarget, renderTarget1, extractEffect);

            // Pass 2: Draw from rendertarger1 to renderTarget2 applying a horizontal gaussian blur.
            SetupGaussBlur(1.0f / (float)renderTarget1.Width, 0);
            DrawToRenderTarget(renderTarget1.GetTexture(), renderTarget2, gaussBlurEffect);

            // Pass 3: Draw from renderTarget2 to rengerTarget1 applying a vertical gaussian blur.
            SetupGaussBlur(0, 1.0f / (float)renderTarget1.Height);
            DrawToRenderTarget(renderTarget2.GetTexture(), renderTarget1, gaussBlurEffect);

            // Pass 4: draw renderTarget1 contents and original scene combined as output, this gives the scene but bloomed
            device.SetRenderTarget(0, null);
            EffectParameterCollection parameters = combineEffect.Parameters;
            parameters["BloomIntensity"].SetValue(bloomIntensity);
            parameters["BaseIntensity"].SetValue(baseIntensity);
            parameters["BloomSaturation"].SetValue(bloomSaturation);
            parameters["BaseSaturation"].SetValue(baseSaturation);
            device.Textures[1] = resolveTarget;
            DrawToRenderTarget(renderTarget1.GetTexture(), null, combineEffect);
        }
        
        //Draws a texture to a renderTarget with an effect, renderTarget null is the screen for output
        void DrawToRenderTarget(Texture2D texture, RenderTarget2D renderTarget, Effect effect)
        {
            int width, height;
            if (renderTarget != null)
            {
                device.SetRenderTarget(0, renderTarget);
                width = renderTarget.Width;
                height = renderTarget.Height;
            }
            else
            {
                width = device.Viewport.Width;
                height = device.Viewport.Height;
            }

            spriteBatch.Begin(SpriteBlendMode.None, SpriteSortMode.Immediate, SaveStateMode.None);
            effect.Begin();
            foreach (EffectPass p in effect.CurrentTechnique.Passes)
            {
                p.Begin();
                // Draw
                spriteBatch.Draw(texture, new Rectangle(0, 0, width, height), Color.White);
                p.End();
            }
            effect.End();
            spriteBatch.End();
            device.SetRenderTarget(0, null);
        
        }
        
        //sets the weightings and offsets within the GaussBlur effect file. Samples along the line generated by (xdir, ydir).
        //To apply a proper Gaussian blur we need to blur twice along perpendicular lines.
        void SetupGaussBlur(float xdir, float ydir)
        {
            // Look up the sample weight and offset effect parameters.
            EffectParameter weightsParameter, offsetsParameter;
            weightsParameter = gaussBlurEffect.Parameters["SampleWeights"];
            offsetsParameter = gaussBlurEffect.Parameters["SampleOffsets"];

            //Number of samples
            int sampleCount = weightsParameter.Elements.Count;

            // Create temporary arrays for computing our filter settings.
            float[] tempWeights = new float[sampleCount];
            Vector2[] tempOffsets = new Vector2[sampleCount];

            //First sample has zero offset
            tempWeights[0] = ComputeGaussian(0);
            tempOffsets[0] = new Vector2(0);

            // Maintain a sum of all the weighting values.
            float totalWeights = tempWeights[0];
            
            //adds 2 samples on each pass
            for (int i = 0; i < sampleCount / 2; i++)
            {
                float weight = ComputeGaussian(i + 1);
                tempWeights[i * 2 + 1] = weight;
                tempWeights[i * 2 + 2] = weight;

                totalWeights += weight * 2;

                float sampleOffset = i * 2 + 1.5f;

                //The pixel being overlayed. One equally far from the current pixel in opposite directions (xdir,ydir), -(xdir,ydir)
                Vector2 delta = new Vector2(xdir, ydir) * sampleOffset;

                // Store texture coordinate offsets for the positive and negative taps.
                tempOffsets[i * 2 + 1] = delta;
                tempOffsets[i * 2 + 2] = -delta;
            }

            // Normalize so that the pixel having blur applied to it remains the same brightness etc
            for (int i = 0; i < tempWeights.Length; i++)
            {
                tempWeights[i] /= totalWeights;
            }

            //Set the values in GaussEffect.fx
            weightsParameter.SetValue(tempWeights);
            offsetsParameter.SetValue(tempOffsets);
        }

        //computes a point on a 1D G(x) gaussian curve, used to set up the blur weighting
        float ComputeGaussian(float n)
        {
            float theta = blurAmount;

            //1D Gaussian function, G(x)
            return (float)((1.0 / Math.Sqrt(2 * Math.PI * theta)) *
                           Math.Exp(-(n * n) / (2 * theta * theta)));
        }

        #endregion
    }
}
