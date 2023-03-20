using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Regressus.Projectiles.Ranged
{
    public class MalignantFission : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Malignant Shotgun");
        }

        public override void SetDefaults()
        {
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.Size = new Vector2(14);
            Projectile.scale = 1f;

            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;

            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            Projectile.rotation += Projectile.velocity.Length() * 0.05f;
            Projectile.velocity.Y += 0.1f;

            Projectile.ai[0] += 1f;

            if (Projectile.ai[0] >= 30f + Main.rand.NextFloat(-5f, 5f))
            {
                Projectile.Kill();
            }
        }

        public override void Kill(int timeLeft)
        {
            RegreSystem.ScreenShakeAmount = 5f;
            SoundEngine.PlaySound(new SoundStyle("Regressus/Sounds/Custom/FireHit"));
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<MalignantFissionExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

            for (int k = 0; k < 20; k++)
            {
                Dust.NewDustPerfect(Projectile.Center, DustID.GemRuby, Main.rand.NextVector2Circular(1f, 1f) * 10, 0, default, 2f).noGravity = true;
            }
        }
    }

    public class MalignantFissionExplosion : ModProjectile
    {
        bool initilize = true;

        Vector2 spawnPosition;

        public override string Texture => "Regressus/Extras/circlething";

        public override bool ShouldUpdatePosition() => false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Malignant Shotgun");
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
