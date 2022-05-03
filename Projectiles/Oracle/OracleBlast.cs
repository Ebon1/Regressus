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
    public class OracleBlast : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Knowledge Bolt");
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * ((255 - Projectile.alpha) / 255f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects effects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * 35, 65, 35), Color.White, Projectile.rotation, new Vector2(65, 35) / 2, 1, effects, 0f);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Projectiles/Oracle/OracleBlast_Glow").Value, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * 35, 65, 35), Color.White, Projectile.rotation, new Vector2(65, 35) / 2, 1, effects, 0f);
            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            return false;
        }
        public override void SetDefaults()
        {
            Projectile.width = 65;
            Projectile.height = 33;
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
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
            }
        }
    }
    public class OracleBlast2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shadow Bolt");
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
            Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * 35, 65, 35), Color.White, Projectile.rotation, new Vector2(65, 35) / 2, 1, effects, 0f);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Projectiles/Oracle/OracleBlast2_Glow").Value, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * 35, 65, 35), Color.White, Projectile.rotation, new Vector2(65, 35) / 2, 1, effects, 0f);
            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            return false;
        }
        public override void SetDefaults()
        {
            Projectile.width = 65;
            Projectile.height = 33;
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
                Projectile.velocity *= 1.1f;
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
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
            }
        }
    }
}