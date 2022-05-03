using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Regressus.Dusts;
namespace Regressus.Projectiles.Oracle
{
    public class OracleEnergyOrb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Energy Orb");
            Main.projFrames[Projectile.type] = 4;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * ((255 - Projectile.alpha) / 255f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects effects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * 48, 46, 48), Color.White, Projectile.rotation, new Vector2(46, 46) / 2, 1, effects, 0f);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Extras/glow").Value, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 512, 512), Color.White, Projectile.rotation, new Vector2(512, 512) / 2, 0.35f, effects, 0f);
            //Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Projectiles/Oracle/OracleBlast_Glow").Value, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * 35, 65, 35), Color.White, Projectile.rotation, new Vector2(65, 35) / 2, 1, effects, 0f);
            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            return false;
        }
        public override void SetDefaults()
        {
            Projectile.width = 46;
            Projectile.height = 46;
            Projectile.aiStyle = 0;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 500;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 1)
            {
                Projectile.velocity *= 1.05f;
            }
            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 4)
                {
                    Projectile.frame = 0;
                }
            }
            Projectile.direction = Projectile.spriteDirection = Projectile.velocity.X > 0 ? 1 : -1;
            //Projectile.rotation = Projectile.velocity.ToRotation();
            /*if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
            }
            */
        }
    }
}