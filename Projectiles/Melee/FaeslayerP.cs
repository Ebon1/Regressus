using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Items.Weapons.Melee;
using Terraria.GameContent;

namespace Regressus.Projectiles.Melee
{
    public class FaeslayerP : ModProjectile
    {
        public override string Texture => "Regressus/Items/Weapons/Melee/Faeslayer";
        Color color;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects effects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Projectiles/Melee/Faeslayer_Glow").Value, Projectile.Center - Main.screenPosition, null, color * Projectile.scale, Projectile.rotation, new Vector2(100, 100) / 2, Projectile.scale, effects, 0f);
            Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 70, 70), Color.White * 0.75f * Projectile.scale, Projectile.rotation, new Vector2(70, 70) / 2, Projectile.scale, effects, 0f);
            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            return false;
        }
        public override void SetDefaults()
        {
            color = new Color(Main.rand.Next(256), Main.rand.Next(256), Main.rand.Next(256));
            Projectile.width = 70;
            Projectile.height = 70;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 135;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            float progress = Utils.GetLerpValue(0, 135, Projectile.timeLeft);
            Projectile.scale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);
            Projectile.direction = Projectile.spriteDirection = Projectile.velocity.X > 0 ? 1 : -1;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.PiOver2;
            }
            Vector2 move = Vector2.Zero;
            float distance = 1250f;
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
            if (target)
            {
                AdjustMagnitude(ref move);
                Projectile.velocity = (12 * Projectile.velocity + move) / 5f;
                AdjustMagnitude(ref Projectile.velocity);
            }
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 8f)
            {
                vector *= 15f / magnitude;
            }
        }
    }
}
