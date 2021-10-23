using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;

namespace Regressus.Projectiles.Curserisen
{
    public class CurserisenSeed : ModProjectile
    {
        public override string Texture => "Regressus/Projectiles/Curserisen/CurserisenOrb";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Curserisen Seed");
        }
        public override void SetDefaults()
        {
            Projectile.height = 12;
            Projectile.width = 12;
            Projectile.tileCollide = true;
            AIType = ProjectileID.Bullet;
            Projectile.hostile = false;
            Projectile.friendly = true;
        }
        public override void AI()
        {
            Projectile.velocity.Y += 0.07f;
        }
    }
    public class CurserisenSap : ModProjectile
    {
        public override string Texture => "Regressus/Projectiles/Curserisen/CurserisenOrb";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Curserisen Sap");
        }
        public override void SetDefaults()
        {
            Projectile.height = 12;
            Projectile.width = 12;
            Projectile.tileCollide = true;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.penetrate = 5;
            Projectile.timeLeft = 200;
            Projectile.usesLocalNPCImmunity = true;
        }
        public override void AI()
        {
            double AngularVelocity = MathHelper.TwoPi * Projectile.velocity.Length();
            Projectile.velocity.Y += 0.2f;
            Projectile.rotation += (float)AngularVelocity;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("Regressus/Projectiles/Curserisen/CurserisenOrb").Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity *= .5f;
            return false;
        }
    }
}