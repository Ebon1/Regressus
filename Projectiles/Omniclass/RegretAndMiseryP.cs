using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Audio;
using Regressus.Items.Weapons;
using Mono.Cecil;
using System.Collections.Generic;

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
            Projectile.velocity *= 1.02f;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            SoundEngine.PlaySound(RegretAndMisery.sussySound2, Main.LocalPlayer.position);
        }
    }
    public class RegretAndMiseryP2 : ModProjectile
    {
        public override string Texture => "Regressus/Extras/sussyomg";
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
            Texture2D a = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(a, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White * alpha);
            return false;
        }
        //public override void PostDraw(Color lightColor)
        //{
        //    Texture2D a = TextureAssets.Projectile[Projectile.type].Value;
        //    Main.spriteBatch.Draw(a, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White * Projectile.scale);
        //}
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 22;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.scale = 2;
            Projectile.hostile = false;
            Projectile.scale = 10f;
            Projectile.timeLeft = 100;
            Projectile.hide = true;
            Projectile.tileCollide = false;
            color = new Color(Main.rand.Next(256), Main.rand.Next(256), Main.rand.Next(256));
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCs.Add(Projectile.whoAmI);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * alpha;
        }
        float alpha;
        public override void AI()
        {
            float progress = Utils.GetLerpValue(0, 100, Projectile.timeLeft);
            alpha = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);
            Projectile.position = Main.screenPosition;
            if (Projectile.timeLeft < 50 && Main.player[Projectile.owner].channel)
                Projectile.timeLeft = 50;
            if (++Projectile.ai[1] > 20)
            {
                Projectile.ai[1] = 0;
                SoundEngine.PlaySound(RegretAndMisery.sussySound2);
            }
            if (++Projectile.ai[0] > 5)
            {
                Projectile.ai[0] = 0;
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Main.screenPosition, RegreUtils.FromAToB(Main.screenPosition, Main.MouseWorld) * 15f, ModContent.ProjectileType<RegretAndMiseryP>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
        }
    }
}
