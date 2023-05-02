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
        List<Comet> comets = new List<Comet>();
        bool active;
        class Comet
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
                    alpha = (float)Math.Sin((2 * (progress) - (Math.PI * 0.25f)));
                    float lerp = progress / (float)Math.PI;
                    position = new Vector2(MathHelper.Lerp(pos[0].X, pos[1].X, lerp), MathHelper.Lerp(pos[0].Y, pos[1].Y, lerp));
                    dead = progress >= Math.PI;
                    return true;
                }
                return false;
            }
        }
        public override void Activate(Vector2 position, params object[] args)
        {
            comets.Clear();
            active = true;
        }
        public override bool IsActive() => active;
        public override void Update(GameTime gameTime)
        {
            if (active && !Main.gamePaused)
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
            }
        }
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            Texture2D tex = ModContent.Request<Texture2D>("Regressus/Effects/CometBG").Value;
            for (int a = 0; a < comets.Count; a++)
            {
                Vector2 vec = new Vector2(1, 0.9f);
                Vector2 value2 = Main.screenPosition + new Vector2((float)(Main.screenWidth >> 1), (float)(Main.screenHeight >> 1));
                Vector2 pos = (comets[a].position - value2) * vec + value2 - Main.screenPosition;
                if (Main.screenPosition.Y < 10000f)
                    spriteBatch.Draw(tex, pos, new Rectangle(0, 0, tex.Width, tex.Height), (new Color(255 + Main.ColorOfTheSkies.R, 187 + Main.ColorOfTheSkies.G, 0 + Main.ColorOfTheSkies.B, Main.ColorOfTheSkies.A) * comets[a].alpha), (float)Math.PI * 0.25f, new Vector2(tex.Width, tex.Height), comets[a].distance * 0.03f, SpriteEffects.None, 0);
            }
        }
        public override void Reset()
        {
            comets.Clear();
            active = false;
        }
        public override void Deactivate(params object[] args)
        {
            active = false;
        }
    }
}