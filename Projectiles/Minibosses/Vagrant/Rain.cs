using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace Regressus.Projectiles.Minibosses.Vagrant
{
    public class Rain1 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rain drop");
        }
        public override Color? GetAlpha(Color lightColor)
        {

            return Color.White;
        }

        public override void PostDraw(Color lightColor)
        {
            if (Projectile.hostile)
                Utils.DrawLine(Main.spriteBatch, Projectile.position, Projectile.position + Projectile.velocity * 250, Color.White * 0f, Color.Red, 1);
        }
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 42;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 500;
            Projectile.tileCollide = true;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            Projectile.velocity *= 1.025f;
        }
    }
    public class Rain2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rain drop");
        }
        public override Color? GetAlpha(Color lightColor)
        {

            return Color.White;
        }
        public override void PostDraw(Color lightColor)
        {
            if (Projectile.hostile)
                Utils.DrawLine(Main.spriteBatch, Projectile.position, Projectile.position + Projectile.velocity * 250, Color.White * 0f, Color.Red, 1);
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 500;
            Projectile.tileCollide = true;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            Projectile.velocity *= 1.025f;
        }
    }
    public class Rain3 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rain drop");
        }
        public override Color? GetAlpha(Color lightColor)
        {

            return Color.White;
        }

        public override void PostDraw(Color lightColor)
        {
            if (Projectile.hostile)
                Utils.DrawLine(Main.spriteBatch, Projectile.position, Projectile.position + Projectile.velocity * 250, Color.White * 0f, Color.Red, 1);
        }
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 32;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 500;
            Projectile.tileCollide = true;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            Projectile.velocity *= 1.025f;
        }
    }
}
