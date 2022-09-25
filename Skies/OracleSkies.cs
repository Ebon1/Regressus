using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using System;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.GameContent;
using ReLogic.Content;
using Terraria.Graphics.Shaders;

namespace Regressus.Skies
{
    public class OracleSkyP2 : CustomSky
    {

        private bool isActive;
        private float intensity;
        private struct Star
        {
            public Vector2 Position;

            public float Depth;

            public float SinOffset;

            public float AlphaFrequency;

            public float AlphaAmplitude;
        }
        private Asset<Texture2D> _starTexture;
        private Star[] _stars;
        int frame;
        int frameCounter;
        public override void Update(GameTime gameTime)
        {
            if (isActive)
            {
                if (++frameCounter >= 5)
                {
                    if (frame != 4)
                    {
                        frame++;
                    }
                    else
                    {
                        frame = 0;
                    }
                    frameCounter = 0;
                }
            }
            if (isActive && intensity < 1f)
            {
                intensity += 0.01f;
            }
            else if (!isActive && intensity > 0)
            {
                intensity -= 0.01f;
            }
        }
        public override void OnLoad()
        {
            _starTexture = ModContent.Request<Texture2D>("Regressus/Extras/Oracle_Eye", AssetRequestMode.ImmediateLoad);
        }
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            Texture2D _bgTexture = Main.Assets.Request<Texture2D>("Images/Misc/NebulaSky/Beam", (AssetRequestMode)1).Value;

            spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black * intensity);
            //spriteBatch.Draw(_bgTexture, new Rectangle(-Main.screenWidth / 2, 0, Main.screenWidth * 2, Main.screenHeight), Color.Purple /** Math.Min(1f, (Main.screenPosition.Y - 800f) / 1000f*/* 0.25f * intensity/*)*/);
            if (maxDepth >= 0 && minDepth < 0)
            {
                //spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black);
            }
            /*int num = -1;
            int num2 = 0;
            for (int i = 0; i < _stars.Length; i++)
            {
                float depth = _stars[i].Depth;
                if (num == -1 && depth < maxDepth)
                {
                    num = i;
                }
                if (depth <= minDepth)
                {
                    break;
                }
                num2 = i;
            }
            if (num == -1)
            {
                return;
            }
            float num3 = Math.Min(1f, (Main.screenPosition.Y - 1000f) / 1000f);
            Vector2 vector3 = Main.screenPosition + new Vector2(Main.screenWidth >> 1, Main.screenHeight >> 1);
            Rectangle rectangle = new Rectangle(-1000, -1000, 4000, 4000);
            for (int j = num; j < num2; j++)
            {
                Vector2 vector4 = new Vector2(1f / _stars[j].Depth, 1.1f / _stars[j].Depth);
                Vector2 position = (_stars[j].Position - vector3)/* * vector4*/
            /*+vector3 - Main.screenPosition;
            if (rectangle.Contains((int)position.X, (int)position.Y))
            {
                float value = (float)Math.Sin(_stars[j].AlphaFrequency * Main.GlobalTimeWrappedHourly + _stars[j].SinOffset) * _stars[j].AlphaAmplitude + _stars[j].AlphaAmplitude;
                float num4 = (float)Math.Sin(_stars[j].AlphaFrequency * Main.GlobalTimeWrappedHourly * 5f + _stars[j].SinOffset) * 0.1f - 0.1f;
                value = MathHelper.Clamp(value, 0f, 1f);
                Texture2D value2 = _starTexture.Value;
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
                spriteBatch.Draw(value2, position, new Rectangle(0, frame * 70, 68, 70), Color.White * num3 *//* value * *//*
            0.8f * (1f - num4) * intensity, 0f, new Vector2(value2.Width >> 1, value2.Height >> 1), (vector4.X * 0.5f + 0.5f)/* * (value * 0.3f + 0.7f)*,//* SpriteEffects.None, 0f);
            /*Main.spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }*/
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            Texture2D vignette1 = ModContent.Request<Texture2D>("Regressus/Extras/Vignette_big").Value;
            spriteBatch.Draw(vignette1, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.DarkViolet * intensity);
            Texture2D a = ModContent.Request<Texture2D>("Regressus/Extras/Empty").Value;
            Effect effect = Regressus.ScreenDistort;
            effect.Parameters["screenPosition"].SetValue(Main.screenPosition);
            effect.Parameters["noiseTex"].SetValue(ModContent.Request<Texture2D>("Regressus/Extras/seamlessNoise").Value);
            effect.Parameters["distortionMultiplier"].SetValue(0.75f * intensity);
            effect.Parameters["screenSize"].SetValue(new Vector2(Main.screenWidth, Main.screenHeight) * -15f);
            effect.Parameters["alpha"].SetValue(intensity);
            effect.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly);
            effect.CurrentTechnique.Passes[0].Apply();
            Main.spriteBatch.Draw(a, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White * intensity);
            Main.spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public override float GetCloudAlpha()
        {
            return 0;
        }
        public override void Activate(Vector2 position, params object[] args)
        {
            isActive = true;
            int num = 10;
            int num2 = 10;
            _stars = new Star[num * num2];
            int num3 = 0;
            for (int i = 0; i < num; i++)
            {
                float num4 = (float)i / (float)num;
                for (int j = 0; j < num2; j++)
                {
                    float num5 = (float)j / (float)num2;
                    _stars[num3].Position.X = num4 * (float)Main.maxTilesX * 16f;
                    _stars[num3].Position.Y = num5 * ((float)Main.worldSurface * 16f + 2000f) - 1000f;
                    _stars[num3].Depth = Main.rand.NextFloat() * 8f + 1.5f;
                    _stars[num3].SinOffset = Main.rand.NextFloat() * 6.28f;
                    _stars[num3].AlphaAmplitude = Main.rand.NextFloat() * 5f;
                    _stars[num3].AlphaFrequency = Main.rand.NextFloat() + 1f;
                    num3++;
                }
            }
            Array.Sort(_stars, SortMethod);
        }
        private int SortMethod(Star meteor1, Star meteor2)
        {
            return meteor2.Depth.CompareTo(meteor1.Depth);
        }

        public override void Deactivate(params object[] args)
        {
            isActive = false;
        }

        public override void Reset()
        {
            isActive = false;
        }

        public override bool IsActive()
        {
            return isActive || intensity > 0;
        }
    }
    public class OracleSkyP1 : CustomSky
    {

        private bool isActive;
        private float intensity;
        private struct LightPillar
        {
            public Vector2 Position;

            public float Depth;
        }

        private LightPillar[] _pillars;
        public static Vector2[] lightPillarPos = new Vector2[40];
        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < _pillars.Length; i++)
            {
                lightPillarPos[i] = _pillars[i].Position;
            }
            if (isActive && intensity < 1f)
            {
                intensity += 0.01f;
            }
            else if (!isActive && intensity > 0)
            {
                intensity -= 0.01f;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            Texture2D _beamTexture = Main.Assets.Request<Texture2D>("Images/Misc/NebulaSky/Beam", (AssetRequestMode)1).Value;
            spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black * 0.25f);
            int num = -1;
            int num2 = 0;
            for (int i = 0; i < _pillars.Length; i++)
            {
                float depth = _pillars[i].Depth;
                if (num == -1 && depth < maxDepth)
                {
                    num = i;
                }
                if (depth <= minDepth)
                {
                    break;
                }
                num2 = i;
            }
            if (num == -1)
            {
                return;
            }
            Vector2 vector3 = Main.screenPosition + new Vector2(Main.screenWidth >> 1, Main.screenHeight >> 1);
            Rectangle rectangle = new Rectangle(-1000, -1000, 4000, 4000);
            float num3 = Math.Min(1f, (Main.screenPosition.Y - 1000f) / 1000f);
            for (int j = num; j < num2; j++)
            {
                Vector2 vector4 = new Vector2(1f / _pillars[j].Depth, 0.9f / _pillars[j].Depth);
                Vector2 position = _pillars[j].Position;
                position = (position - vector3) * vector4 + vector3 - Main.screenPosition;
                if (rectangle.Contains((int)position.X, (int)position.Y))
                {
                    float num4 = vector4.X * 450f;
                    spriteBatch.Draw(_beamTexture, position, null, Color.DeepSkyBlue * num3 * intensity, 0f, Vector2.Zero, new Vector2(num4 / 70f, num4 / 45f), SpriteEffects.None, 0f);
                }
            }
        }

        public override float GetCloudAlpha()
        {
            return 0;
        }

        public override void Activate(Vector2 position, params object[] args)
        {
            isActive = true;
            _pillars = new LightPillar[40];
            for (int i = 0; i < _pillars.Length; i++)
            {
                _pillars[i].Position.X = (float)i / (float)_pillars.Length * ((float)Main.maxTilesX * 16f + 20000f) + Main.rand.NextFloat() * 40f - 20f - 20000f;
                _pillars[i].Position.Y = Main.rand.NextFloat() * 200f - 2000f;
                _pillars[i].Depth = Main.rand.NextFloat() * 8f + 7f;
            }
            Array.Sort(_pillars, SortMethod);
        }
        private int SortMethod(LightPillar pillar1, LightPillar pillar2)
        {
            return pillar2.Depth.CompareTo(pillar1.Depth);
        }

        public override void Deactivate(params object[] args)
        {
            isActive = false;
        }

        public override void Reset()
        {
            isActive = false;
        }

        public override bool IsActive()
        {
            return isActive || intensity > 0;
        }
    }
}
