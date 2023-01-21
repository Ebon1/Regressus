using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace Regressus.Projectiles.Enemy.UG
{
    public class PhantasmalArrow : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 18;
            Projectile.aiStyle = 0;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 500;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            float progress = Utils.GetLerpValue(0, 500, Projectile.timeLeft);
            float scale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);
            return Color.DarkViolet * scale;
        }
        public override void AI()
        {
            NPC owner = Main.npc[(int)Projectile.ai[0]];
            if (Projectile.aiStyle == 0)
            {
                if (!owner.active && Projectile.aiStyle == 0)
                    return;
                if (owner.ai[0] != 2 && Projectile.aiStyle == 0)
                {
                    Projectile.velocity = Vector2.Zero;
                    Projectile.Center = owner.Center - Vector2.UnitY * 5;
                    Projectile.direction = Projectile.spriteDirection = -owner.direction;
                }
                else
                {
                    Projectile.Kill();
                    Vector2 vel = RegreUtils.FromAToB(Projectile.Center, Main.player[owner.target].Center) * 13f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, vel, ModContent.ProjectileType<PhantasmalArrow2>(), Projectile.damage, 0f);
                }
            }
        }
    }
    public class PhantasmalArrow2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            Texture2D glow = ModContent.Request<Texture2D>("Regressus/Projectiles/Enemy/UG/PhantasmalArrow").Value;
            var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
            {
                Main.spriteBatch.Draw(glow, Projectile.oldPos[i] - Main.screenPosition + new Vector2(Projectile.width / 2f, Projectile.height / 2f), new Rectangle(0, 0, glow.Width, glow.Height), Color.DarkViolet * (1f - fadeMult * i), Projectile.oldRot[i], glow.Size() / 2, Projectile.scale * (ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type], SpriteEffects.FlipHorizontally, 0f);
            }

            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            return true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.DarkViolet;
        }
        public override string Texture => "Regressus/Projectiles/Enemy/UG/PhantasmalArrow";
        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 18;
            Projectile.aiStyle = 0;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 500;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.direction = Projectile.spriteDirection = -1;
            Vector2 move = Vector2.Zero;
            float distance = 5050f;
            bool target = false;
            for (int k = 0; k < 200; k++)
            {
                if (Main.player[k].active)
                {
                    Vector2 newMove = Main.player[k].Center - Projectile.Center;
                    float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                    if (distanceTo < distance)
                    {
                        move = newMove;
                        distance = distanceTo;
                        target = true;
                    }
                }
            }
            if (++Projectile.ai[0] % 5 == 0 && target && Projectile.timeLeft > 45 && Projectile.timeLeft < 450)
            {
                AdjustMagnitude(ref move);
                Projectile.velocity = (6.2f * Projectile.velocity + move) / 6.2f;
                AdjustMagnitude(ref Projectile.velocity);
            }
            if (Projectile.timeLeft < 45)
            {
                Projectile.velocity *= 0.95f;
            }
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 6.2f)
            {
                vector *= 6.2f / magnitude;
            }
        }
    }
}
