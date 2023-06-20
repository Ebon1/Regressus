using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using System.Collections.Generic;
using Terraria.Graphics.Effects;
using Terraria.GameContent;

namespace Regressus.Effects
{
    public class SSW_Background : CustomSky
    {
        //List<Comet> comets = new List<Comet>();
        bool active;
        /*class Comet
        {
            public Vector2[] pos = new Vector2[2];
            public Vector2 position;
            public int distance;
            public float progress;
            public int speed;
            public float alpha;
            public bool dead;
            public Comet(Vector2 position, int dist, int sp)
            {
                pos[0] = position;
                distance = dist;
                speed = sp;
                pos[1] = pos[0] + (new Vector2(-50, 50) * distance);
            }
            public bool Update()
            {
                if (!dead)
                {
                    progress += ((float)Math.PI) / (speed * 8);
                    alpha = (float)Math.Sin((2 * (progress) - (Math.PI )));
                    float lerp = progress / (float)Math.PI;
                    position = new Vector2(MathHelper.Lerp(pos[0].X, pos[1].X, lerp), MathHelper.Lerp(pos[0].Y, pos[1].Y, lerp));
                    dead = progress >= Math.PI;
                    return true;
                }
                return false;
            }
        }*/
        public Starlad[] starlads;
        public struct Starlad
        {
            public int type;
            public Vector2 pos;
            public float depth;
        }
        public override void Activate(Vector2 position, params object[] args)
        {
            //comets.Clear();
            starlads = new Starlad[50];
            if (starlads[0].pos == Vector2.Zero)
                for (int i = 0; i < starlads.Length; i++)
                {

                    int variant = Main.rand.Next(2);
                    starlads[i].type = variant;
                    starlads[i].pos = new Vector2(Main.rand.NextFloat(Main.screenWidth), Main.rand.NextFloat(Main.screenHeight));
                    starlads[i].depth = Main.rand.NextFloat(0.2f, 0.44f);
                }
            active = true;
        }
        public override bool IsActive() => active;
        public float Intensity;
        public override void Update(GameTime gameTime)
        {
            /*if (active && !Main.gamePaused)
            {
                if (Main.rand.NextBool(3))
                    comets.Add(new Comet(new Vector2(Main.LocalPlayer.Center.X + Main.rand.Next(-1000, 1000), (int)(500 + (10000 * ((Main.rand.Next(22, 44) * 0.01f))))), Main.rand.Next(3, 39), Main.rand.Next(4, 16)));
                for (int a = 0; a < comets.Count; a++)
                {
                    Comet comet = comets[a];
                    if (!comet.dead)
                        comet.Update();
                    else
                        comets.Remove(comet);
                }
            }*/
            if (active)
            {
                Intensity = Math.Min(1f, 0.01f + Intensity);
            }
            else
            {
                Intensity = Math.Max(0f, Intensity - 0.01f);
            }

        }
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {

            Texture2D Tex = RegreUtils.GetExtraTexture("sswbg2");
            Texture2D Tex2 = RegreUtils.GetExtraTexture("sswbg");

            Texture2D starlad = RegreUtils.GetExtraTexture("starlad");
            Texture2D minilad = RegreUtils.GetExtraTexture("minilad");

            /*Texture2D tex = ModContent.Request<Texture2D>("Regressus/Effects/CometBG").Value;
            for (int a = 0; a < comets.Count; a++)
            {
                Vector2 vec = new Vector2(1, 0.9f);
                Vector2 value2 = Main.screenPosition + new Vector2((float)(Main.screenWidth >> 1), (float)(Main.screenHeight >> 1));
                Vector2 pos = (comets[a].position - value2) * vec + value2 - Main.screenPosition;
                if (Main.screenPosition.Y < 10000f)
                    spriteBatch.Draw(tex, pos, new Rectangle(0, 0, tex.Width, tex.Height), (new Color(255 + Main.ColorOfTheSkies.R, 187 + Main.ColorOfTheSkies.G, 0 + Main.ColorOfTheSkies.B, Main.ColorOfTheSkies.A) * comets[a].alpha) * 0.75f, (float)Math.PI , new Vector2(tex.Width, tex.Height), comets[a].distance * 0.03f, SpriteEffects.None, 0);
            }*/

            if (Main.screenPosition.Y < 10000f)
                for (int i = 0; i < starlads.Length; i++)
                {
                    if (starlads[i].type == 0)
                    {
                        starlads[i].pos.X += starlads[i].depth * 3;
                        if (starlads[i].pos.X > Main.screenWidth + 100)
                        {

                            int variant = Main.rand.Next(2);
                            starlads[i].type = variant;
                            starlads[i].depth = Main.rand.NextFloat(0.2f, 0.44f);
                            starlads[i].pos.X = variant == 0 ? -100 : Main.rand.NextFloat(Main.screenWidth);
                            starlads[i].pos.Y = variant == 0 ? Main.rand.NextFloat(Main.screenHeight) : -100;
                        }
                    }
                    else
                    {
                        starlads[i].pos.Y += starlads[i].depth * 3;
                        if (starlads[i].pos.Y > Main.screenHeight + 100)
                        {

                            int variant = Main.rand.Next(2);
                            starlads[i].type = variant;
                            starlads[i].depth = Main.rand.NextFloat(0.2f, 0.44f);
                            starlads[i].pos.X = variant == 0 ? -100 : Main.rand.NextFloat(Main.screenWidth);
                            starlads[i].pos.Y = variant == 0 ? Main.rand.NextFloat(Main.screenHeight) : -100;
                        }
                    }
                    /*if (starlads[i].texture == "MythosOfMoonlight/Textures/star2")
                    {
                        spriteBatch.Draw(Tex3, starlads[i].pos, null, Color.White * Intensity * 0.5f * starlads[i].depth, 0, new Vector2(Tex3.Width, Tex3.Height / 2), starlads[i].depth * 0.1f, SpriteEffects.None, 0);
                    }
                    */
                    Texture2D tex = starlads[i].type == 0 ? starlad : minilad;
                    spriteBatch.Draw(tex, starlads[i].pos, null, Color.White * Intensity * 0.5f * starlads[i].depth, starlads[i].type == 0 ? 0 : Main.GameUpdateCount * 0.01f * starlads[i].depth, tex.Size() / 2, starlads[i].depth * (starlads[i].type == 0 ? 0.5f : 1), SpriteEffects.None, 0);
                    //Dust.NewDustPerfect(starlads[i].pos + Main.screenPosition, 1);
                }
            if (maxDepth >= 3.40282347E+38f && minDepth < 3.40282347E+38f)
                spriteBatch.Draw(Tex, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), null, Color.White * Intensity * 0.35f, 0, Vector2.Zero, SpriteEffects.None, 0);

            if (Main.screenPosition.Y < 10000f)
                spriteBatch.Draw(Tex2, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), null, Color.White * Intensity * 0.05f, 0, Vector2.Zero, SpriteEffects.None, 0);

        }
        public override void Reset()
        {
            //comets.Clear();
            active = false;
        }
        public override void Deactivate(params object[] args)
        {
            active = false;
        }
    }
}