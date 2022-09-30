using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Regressus.Projectiles.Melee;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
namespace Regressus.Projectiles.Enemy.Overworld
{
    public class TerraKnightP : ModProjectile
    {
        public int SwingTime = 30;
        public float holdOffset = 65;
        public override void SetDefaults()
        {
            Projectile.timeLeft = 30;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.width = 76;
            Projectile.height = 82;
            Projectile.tileCollide = false;
            Projectile.hide = true;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCs.Add(Projectile.whoAmI);
        }
        public float Ease(float f)
        {
            return 1 - (float)Math.Pow(2, 10 * f - 10);
        }
        public float ScaleFunction(float progress)
        {
            return 0.7f + (float)Math.Sin(progress * Math.PI) * 0.5f;
        }
        public override void AI()
        {
            AttachToPlayer();
        }
        public override bool ShouldUpdatePosition() => false;
        public void AttachToPlayer()
        {
            NPC player = Main.npc[(int)Projectile.ai[1]];
            if (!player.active)
            {
                return;
            }
            int direction = (int)Projectile.ai[0];
            float swingProgress = Ease(Utils.GetLerpValue(0f, SwingTime, Projectile.timeLeft));
            float defRot = Projectile.velocity.ToRotation();
            float start = defRot - (MathHelper.PiOver2 + MathHelper.PiOver4);
            float end = defRot + (MathHelper.PiOver2 + MathHelper.PiOver4);
            float rotation = direction == 1 ? start + MathHelper.Pi * 3 / 2 * swingProgress : end - MathHelper.Pi * 3 / 2 * swingProgress;
            Vector2 position = player.Center +
                rotation.ToRotationVector2() * holdOffset * ScaleFunction(swingProgress);
            Projectile.Center = position;
            Projectile.rotation = (position - player.Center).ToRotation() + MathHelper.PiOver4;

            player.direction = (Projectile.velocity.X < 0 ? -1 : 1);
        }
        public override string Texture => "Regressus/Items/Weapons/Melee/EarthDivider";
        public override void PostDraw(Color lightColor)
        {
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            NPC player = Main.npc[(int)Projectile.ai[1]];
            Texture2D slash = ModContent.Request<Texture2D>("Regressus/Extras/Extras2/slash_02").Value;
            float mult = Ease(Utils.GetLerpValue(0f, SwingTime, Projectile.timeLeft));
            float alpha = (float)Math.Sin(mult * Math.PI);
            Vector2 pos = player.Center + Projectile.velocity * (45f - mult * 45);
            Main.spriteBatch.Draw(slash, pos - Main.screenPosition, null, new Color(0, 255, Main.DiscoB) * alpha, Projectile.velocity.ToRotation() - MathHelper.PiOver2, slash.Size() / 2, Projectile.scale / 1.95f, SpriteEffects.None, 0f);
            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D glow = ModContent.Request<Texture2D>("Regressus/Projectiles/Melee/EarthDivider_Glow").Value;
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            int direction = (int)Projectile.ai[0];
            float swingProgress = Ease(Utils.GetLerpValue(0f, SwingTime, Projectile.timeLeft));
            float defRot = Projectile.velocity.ToRotation();
            float start = defRot - (MathHelper.PiOver2 + MathHelper.PiOver4);
            float end = defRot + (MathHelper.PiOver2 + MathHelper.PiOver4);
            float rotation = direction == 1 ? start + MathHelper.Pi * 3 / 2 * swingProgress : end - MathHelper.Pi * 3 / 2 * swingProgress;
            Vector2 pos2 = (player.Center + Vector2.UnitX * player.direction * holdOffset).RotatedBy(rotation);
            //Main.spriteBatch.Draw(glow, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, glow.Width, glow.Height), new Color(0, 255, Main.DiscoB), Projectile.rotation + (Projectile.ai[0] == -1 ? 0 : MathHelper.PiOver2 * 3), glow.Size() / 2, Projectile.scale * 1.05f, Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            Main.spriteBatch.Draw(texture, pos2 - Main.screenPosition, null, Color.White, Projectile.rotation + (Projectile.ai[0] == -1 ? 0 : MathHelper.PiOver2 * 3), texture.Size() / 2, Projectile.scale, Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
        }
    }
}
