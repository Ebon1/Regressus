using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Items.Weapons.Magic;
using Terraria.Audio;
using Terraria.GameContent;


namespace Regressus.Projectiles.Dev
{
    public class PokerfaceP2 : ModProjectile
    {
        public override string Texture => "Regressus/Projectiles/Dev/PokerfaceP";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 25;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.height = 3;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 3;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 500;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        float time, frequencyMultiplier, amplitude;
        bool runOnce;
        Vector2 initialCenter, initialVel;
        public override void AI()
        {
            if (!runOnce)
            {
                initialCenter = Projectile.Center;
                initialVel = Projectile.velocity;
                runOnce = true;
            }
            RegreUtils.SineMovement(Projectile, initialCenter, initialVel, 0.15f, 60);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            Texture2D glow = ModContent.Request<Texture2D>("Regressus/Extras/glow2").Value;
            var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
            {
                Main.spriteBatch.Draw(glow, Projectile.oldPos[i] - Main.screenPosition + new Vector2(Projectile.width / 2f, Projectile.height / 2f), new Rectangle(0, 0, glow.Width, glow.Height), Color.Cyan * (1f - fadeMult * i), Projectile.oldRot[i], glow.Size() / 2, Projectile.scale * .25f * (ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type], Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            }

            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            return false;
        }
    }
}
