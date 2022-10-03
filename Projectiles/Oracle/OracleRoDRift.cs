using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace Regressus.Projectiles.Oracle
{
    public class OracleRoDRift : ModProjectile
    {
        public override string Texture => "Regressus/Extras/Empty";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spacetime rift");
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            float mult = (0.55f + (float)Math.Sin(Main.GlobalTimeWrappedHourly/* * 2*/) * 0.1f);
            float scale = Projectile.scale * 2 * mult;
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Extras/vortex").Value, Projectile.Center - Main.screenPosition, null, Color.White, -Main.GameUpdateCount * 0.0075f, new Vector2(256, 256) / 2, scale, SpriteEffects.None, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            return false;
        }
        float MAX_TIME = 1000;
        public override void SetDefaults()
        {
            Projectile.width = 256 / 4;
            Projectile.height = 256 / 4;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.damage = 0;
            Projectile.timeLeft = 1000;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            float progress = Utils.GetLerpValue(0, MAX_TIME, Projectile.timeLeft);
            Projectile.scale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 6, 0, 1);
            Player player = Main.player[Projectile.owner];
            if (player.dead && Projectile.timeLeft > 100)
                Projectile.timeLeft = 100;
            if (!player.dead && player.active && Projectile.scale == 1)
            {
                if (player.Center.Distance(Projectile.Center) < Projectile.width)
                {
                    player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " did a bad no no idk."), 999999999, 0);
                }
                Vector2 moveTo = Projectile.Center - player.Center;
                float factor = 0.04f;
                player.velocity = moveTo * factor;
                player.controlLeft = false;
                player.controlRight = false;
                player.controlJump = false;
                player.controlDown = false;
                player.controlUseItem = false;
            }
        }
    }
}