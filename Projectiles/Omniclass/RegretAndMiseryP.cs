using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;


namespace Regressus.Projectiles.Omniclass
{
    public class RegretAndMiseryP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("RegreSUS");
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        Color color;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects effects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            //RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            //Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Extras/glow").Value, Projectile.Center - Main.screenPosition, null, color, 0, new Vector2(512, 512) / 2, Projectile.scale * 0.25f, SpriteEffects.None, 0f);
            //RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 20, 22), color, Projectile.rotation, new Vector2(20, 22) / 2, Projectile.scale, effects, 0f);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Projectiles/Omniclass/RegretAndMiseryP_extra").Value, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 20, 22), Color.White, Projectile.rotation, new Vector2(20, 22) / 2, Projectile.scale, effects, 0f);
            return false;
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 22;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.scale = 2;
            Projectile.hostile = false;
            Projectile.timeLeft = 500;
            Projectile.tileCollide = false;
            color = new Color(Main.rand.Next(256), Main.rand.Next(256), Main.rand.Next(256));
        }
        public override void AI()
        {
            Projectile.direction = Projectile.spriteDirection = Projectile.velocity.X > 0 ? 1 : -1;
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot(Mod, "Sounds/Custom/Sussy2"));
        }
    }
}
