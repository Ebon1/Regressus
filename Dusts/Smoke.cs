using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Regressus.Dusts
{
    public class Smoke : ModDust
    {

        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.scale *= Main.rand.NextFloat(0.8f, 2f);
            dust.frame = new Rectangle(0, Main.rand.Next(3) * 19, 19, 19);
            dust.rotation = Main.rand.NextFloat(6.28f);
        }

        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            Color gray = new Color(44, 73, 115);
            Color black = new Color(3, 0, 38);
            Color color;

            if (dust.alpha < 120) { color = Color.Lerp(dust.color, gray, dust.alpha / 120f); }
            else if (dust.alpha < 180) { color = Color.Lerp(gray, black, (dust.alpha - 120) / 60f); }
            else { color = black; }

            return color * ((255 - dust.alpha) / 255f);
        }

        public override bool Update(Dust dust)
        {
            dust.velocity *= 0.98f;
            dust.velocity.X *= 0.95f;
            dust.scale += 0.05f;

            if (dust.alpha > 100)
            {
                dust.alpha += 10;
            }
            else
            {
                Lighting.AddLight(dust.position, dust.color.ToVector3() * 0.1f);
                dust.alpha += 4;
            }

            dust.position += dust.velocity;

            if (dust.alpha >= 255)
                dust.active = false;

            return false;
        }
    }
}
