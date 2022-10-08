using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Regressus.Dusts
{
    public class GinnungagapDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.alpha = 255;
            dust.noLight = true;
            dust.noGravity = true;
            base.OnSpawn(dust);
        }
        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.scale -= 0.03f;
            dust.velocity *= 0.99f;
            if (dust.scale <= 0)
                dust.active = false;
            Collision.TileCollision(dust.position, dust.velocity, 50, 50, true);
            return false;
        }
        public static void DrawAll(SpriteBatch sb)
        {
            foreach (Dust d in Main.dust)
            {
                if (d.type == ModContent.DustType<GinnungagapDust>() && d.active)
                {
                    Texture2D tex = ModContent.Request<Texture2D>("Regressus/Dusts/GinnungagapDust").Value;
                    sb.Draw(tex, d.position - Main.screenPosition, null, Color.White * 0.5f, 0, tex.Size() / 2, d.scale, SpriteEffects.None, 0);
                }
            }
        }
    }
}
