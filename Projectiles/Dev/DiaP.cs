using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Items.Dev;
using Regressus.Projectiles.Magic;
using Terraria.GameContent;
using System.Collections.Generic;
using System.IO;

namespace Regressus.Projectiles.Dev
{
    public class DiaP : ModProjectile
    {
        public override string Texture => "Regressus/Items/Dev/_DiaItem";
        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 10;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
        }
        float rot = MathHelper.ToRadians(-22.5f);
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || !player.channel || player.dead || player.CCed || player.noItems)
            {
                Projectile.timeLeft = 0;
                return;
            }
            player.ChangeDir(Projectile.velocity.X < 0 ? -1 : 1);
            player.heldProj = Projectile.whoAmI;
            Projectile.Center = new Vector2(player.itemLocation.X, player.itemLocation.Y - 14f);
            Projectile.direction = Projectile.spriteDirection = player.direction;
            player.itemTime = 2;
            player.itemAnimation = 2; ;
            Projectile.timeLeft = 2;
            Projectile.ai[1] += 0.025f;
            for (int i = 1; i < 7; i += 2)
            {
                if (Main.rand.Next(2) == 0)
                {
                    Vector2 vector3 = Projectile.Center + Main.rand.NextVector2CircularEdge(25 * (i * 2 + 1), 25 * (i * 2 + 1));
                    int num14 = Dust.NewDust(vector3, 0, 0, 267, 0f, 0f, 0, Main.DiscoColor);
                    Main.dust[num14].position = vector3;
                    Main.dust[num14].noGravity = true;
                    Main.dust[num14].scale = 0.3f;
                    Main.dust[num14].fadeIn = Main.rand.NextFloat() * 1.2f * Projectile.ai[1] * i;
                    Main.dust[num14].velocity = Vector2.Zero;
                    Main.dust[num14].scale *= Projectile.ai[1] * i;
                    Main.dust[num14].position += Main.dust[num14].velocity * -5f;
                    if (num14 != 6000)
                    {
                        Dust dust = Dust.CloneDust(num14);
                        dust.scale /= 2f;
                        dust.fadeIn *= 0.85f;
                        dust.color = new Color(255, 255, 255, 255);
                    }
                }
            }
            if (Projectile.ai[1] > 1.2f)
            {
                Projectile.ai[1] = 0;
            }
            if (Projectile.ai[1] == 0.025f)
            {
                rot += MathHelper.ToRadians(22.5f);
                for (int i = 0; i < 3; i++)
                {
                    float angle = 2f * (float)Math.PI / 3f * i;
                    Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, (Vector2.UnitX * 10f).RotatedBy(angle + rot), ModContent.ProjectileType<DiaP2>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                }
            }
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = RegreUtils.GetExtraTexture("PulseCircle");
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            for (int i = 1; i < 4; i++)
            {
                float scale = i * Projectile.ai[1] * 0.4f;
                float alpha = MathHelper.Clamp((float)Math.Sin(Projectile.ai[1] * Math.PI), 0, 1);
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Main.DiscoColor * alpha, 0, texture.Size() / 2, scale, SpriteEffects.None, 0);
            }
            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
        }
    }
    public class DiaP2 : ModProjectile
    {
        public override string Texture => "Regressus/Extras/Empty";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 30;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 5;
            Projectile.height = 5;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 500;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            default(Effects.Prims.TestRainbowTrail).Draw(Main.projectile[Projectile.whoAmI]);
            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            return false;
        }
        public override void PostDraw(Color lightColor)
        {
            if (npc != Vector2.Zero)
            {
                Projectile.ai[0] += 0.1f;
                if (Projectile.ai[0] >= 1f)
                {
                    npc = Vector2.Zero;
                    Projectile.ai[0] = 0;
                }
                Texture2D texture = RegreUtils.GetExtraTexture("PulseCircle");
                float scale = Projectile.ai[0] * 0.4f;
                float alpha = MathHelper.Clamp((float)Math.Sin(Projectile.ai[0] * Math.PI), 0, 1);
                RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
                Main.EntitySpriteDraw(texture, npc - Main.screenPosition, null, Main.DiscoColor * alpha, 0, texture.Size() / 2, scale, SpriteEffects.None, 0);
                RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            }
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
            writer.Write(Projectile.localAI[1]);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
            Projectile.localAI[1] = reader.ReadSingle();
        }
        Vector2 npc;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (npc == Vector2.Zero)
                npc = target.Center;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Vector2 move = Vector2.Zero;
            float distance = 5050f;
            bool target = false;
            for (int k = 0; k < 200; k++)
            {
                if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5)
                {
                    Vector2 newMove = Main.npc[k].Center - Projectile.Center;
                    float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                    if (distanceTo < distance)
                    {
                        move = newMove;
                        distance = distanceTo;
                        target = true;
                    }
                }
            }
            if (target && Projectile.timeLeft > 45)
            {
                AdjustMagnitude(ref move);
                Projectile.velocity = (8 * Projectile.velocity + move) / 5f;
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
            if (magnitude > 8f)
            {
                vector *= 8f / magnitude;
            }
        }
    }
}
