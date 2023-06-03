using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Common;
using Microsoft.Xna.Framework.Graphics;

namespace Regressus.Projectiles.Ranged
{
    public class FrigidArrow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            base.DisplayName.SetDefault("Frigid Arrow");
        }

        public override void SetDefaults()
        {
            Projectile.width = 25;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 0)
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            for (int i = 0; i < 10; i++)
            {
                Dust dust;
                dust = Main.dust[Terraria.Dust.NewDust(Projectile.Center, 2, 2, DustID.BlueCrystalShard, 0f, 0f, 0, new Color(255, 0, 201), 1f)];
                dust.noGravity = true;
            }

            Lighting.AddLight(Projectile.position, TorchID.Ice);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[Projectile.owner];

            player.GetModPlayer<RegrePlayer>().itemCombo++;
            player.GetModPlayer<RegrePlayer>().itemComboReset = 480;

            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<IcyBurst>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
        }
    }

    public class IcyBurst : ModProjectile
    {
        bool initilize = true;

        Vector2 spawnPosition;

        public override string Texture => "Regressus/Assets/Textures/PulseCircle"; //Shmircle

        public override bool ShouldUpdatePosition() => false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("burp");
        }

        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.Size = new Vector2(10f);
            Projectile.scale = 0.01f;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.aiStyle = -1;
            Projectile.timeLeft = 100;
        }

        public override void AI()
        {
            if (initilize)
            {
                spawnPosition = Projectile.Center;

                initilize = false;
            }

            Projectile.Center = spawnPosition;

            Projectile.scale += 0.02f;
            Projectile.Size += new Vector2(10f);
            Projectile.alpha += 10;

            if (Projectile.alpha >= 255)
            {
                Projectile.Kill();
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int frameY = frameHeight * Projectile.frame;

            Rectangle sourceRectangle = new Rectangle(0, frameY, texture.Width, frameHeight);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Vector2 position = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Color color = Projectile.GetAlpha(new Color(255, 0, 0, 0));

            Main.EntitySpriteDraw(texture, position, sourceRectangle, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }
    }
}
