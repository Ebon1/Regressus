﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Regressus.Items.Vanity;
using Regressus.NPCs.Snow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace Regressus.Content.Dusts
{
    public class FireDust : ModDust
    {
        public override string Texture => "Regressus/Extras/Empty";
        public override void OnSpawn(Dust dust)
        {
            dust.alpha = 255;
            dust.noLight = true;
            dust.noGravity = true;
            dust.scale = 0.35f;
            dust.customData = Main.rand.Next(1, 3);
            base.OnSpawn(dust);

        }
        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.scale -= 0.03f;
            dust.velocity *= 0.95f;
            if (dust.scale <= 0)
                dust.active = false;
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.type == ModContent.NPCType<SnowballWisp>())
                {
                    if (npc.Center.Distance(dust.position) < npc.width)
                    {

                        for (int num613 = 0; num613 < 5; num613++)
                        {
                            Dust.NewDust(npc.position, npc.width, npc.height, DustID.Ice, Main.rand.NextFloat(-1, 1f) * 0.1f, 0.1f, 150, default(Color), 0.8f);
                        }
                    }
                }
            }

            return false;
        }
        public static void DrawAll(SpriteBatch sb)
        {
            foreach (Dust d in Main.dust)
            {
                if (d.type == ModContent.DustType<FireDust>() && d.active)
                {
                    Texture2D tex = ModContent.Request<Texture2D>("Regressus/Extras/Extras2/fire_0" + d.customData).Value;
                    sb.Draw(tex, d.position - Main.screenPosition, null, Color.White, 0, tex.Size() / 2, d.scale * 0.85f, SpriteEffects.None, 0);
                    sb.Draw(tex, d.position - Main.screenPosition, null, Color.OrangeRed, 0, tex.Size() / 2, d.scale, SpriteEffects.None, 0); ;
                }
            }
        }
    }
    public class ColoredFireDust : ModDust
    {
        public override string Texture => "Regressus/Extras/Empty";
        public override void OnSpawn(Dust dust)
        {
            dust.alpha = 255;
            dust.noLight = true;
            dust.noGravity = true;
            dust.scale = 0.35f;
            dust.customData = Main.rand.Next(1, 3);
            base.OnSpawn(dust);

        }
        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.scale -= 0.03f;
            dust.velocity *= 0.95f;
            if (dust.scale <= 0)
                dust.active = false;

            return false;
        }
        public static void DrawAll(SpriteBatch sb)
        {
            foreach (Dust d in Main.dust)
            {
                if (d.type == ModContent.DustType<ColoredFireDust>() && d.active)
                {
                    Texture2D tex = ModContent.Request<Texture2D>("Regressus/Extras/Extras2/fire_0" + d.customData).Value;
                    sb.Draw(tex, d.position - Main.screenPosition, null, Color.White, 0, tex.Size() / 2, d.scale * 0.85f, SpriteEffects.None, 0);
                    sb.Draw(tex, d.position - Main.screenPosition, null, d.color, 0, tex.Size() / 2, d.scale, SpriteEffects.None, 0); ;
                }
            }
        }
    }
}
