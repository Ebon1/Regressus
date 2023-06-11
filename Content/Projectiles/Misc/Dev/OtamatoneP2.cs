using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
namespace Regressus.Projectiles.Dev
{
    public class OtamatoneP2 : ModProjectile
    {
        public override string Texture => "Regressus/Projectiles/Dev/OtamatoneP";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Otamatone");
            Main.projFrames[Projectile.type] = 2;
        }
        public int KillTimer;
        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 56;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;

            Projectile.alpha = 100;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * ((255 - Projectile.alpha) / 255f);
        }

        public override void AI()
        {
            if (++KillTimer >= 40)
            {
                Projectile.velocity *= 1.12f;
            }
            else if (++KillTimer >= 120)
            {
                Projectile.Kill();
            }
            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 2)
                {
                    Projectile.frame = 0;
                }
            }
            {
                Lighting.AddLight(Projectile.position, 0.25f, 0, 0.5f);
            }
            Projectile.direction = Projectile.spriteDirection = Projectile.velocity.X > 0 ? 1 : -1;
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
            }


        }
    }
}