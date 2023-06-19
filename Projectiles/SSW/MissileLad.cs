using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Regressus.Dusts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using XPT.Core.Audio.MP3Sharp.Decoding;

namespace Regressus.Projectiles.SSW
{
    public class MissileLad : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Star Rocket");

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            Main.projFrames[Projectile.type] = 2;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.friendly = false;
            Projectile.hostile = true;

            Projectile.scale = 1f;
            Projectile.Size = new Vector2(30, 24);

            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;

            Projectile.aiStyle = -1;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void Kill(int timeLeft)
        {
            if (Projectile.frame == 0)
            {
                Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<StarExplosion>(), Projectile.damage, Projectile.knockBack);
            }
            else
            {
                Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<StarExplosion2>(), Projectile.damage, Projectile.knockBack);
            }
        }
        public override void AI()
        {
            if (Projectile.ai[1] != 0)
                Projectile.frame = (int)Projectile.ai[1];
            Player player = Main.player[Projectile.owner];
            if (Projectile.ai[0] == 0)
            {
                Projectile.ai[0] = 1;
            }

            if (Projectile.frame == 0)
            {

                Dust.NewDustPerfect(Projectile.Center - new Vector2(0, 24).RotatedBy(Projectile.rotation - MathHelper.PiOver2), ModContent.DustType<Smoke>(), Projectile.velocity, 0, new Color(255, 177, 0), 0.025f).noGravity = true;
            }
            else
            {
                //Projectile.velocity *= 1.1f;
                Dust.NewDustPerfect(Projectile.Center - new Vector2(0, 24).RotatedBy(Projectile.rotation - MathHelper.PiOver2), ModContent.DustType<Smoke>(), Projectile.velocity, 0, new Color(161, 31, 197), 0.05f).noGravity = true;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, RegreUtils.FromAToB(Projectile.Center, player.Center) * 10 * (Projectile.frame + 1), 0.01f);
        }
    }
    public class StarExplosion : ModProjectile
    {
        public override string Texture => RegreUtils.Empty;
        public override void SetDefaults()
        {
            Projectile.height = 300;
            Projectile.width = 300;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
        }
        public override bool ShouldUpdatePosition() => false;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = RegreUtils.GetExtraTexture("explosion");
            Main.spriteBatch.Reload(BlendState.Additive);
            float alpha = MathHelper.Lerp(0.5f, 0, Projectile.ai[0]);
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, new Color(255, 177, 0) * alpha * 1.9f, Main.GameUpdateCount * 0.003f, tex.Size() / 2, Projectile.ai[0] * 1.1f, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White * alpha * 2, Projectile.rotation, tex.Size() / 2, Projectile.ai[0], SpriteEffects.None, 0);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }
        public override void AI()
        {
            Projectile.ai[0] += 0.05f;
            if (Projectile.ai[0] > 0.5f)
                Projectile.Kill();
        }
    }
    public class StarExplosion2 : ModProjectile
    {
        public override string Texture => RegreUtils.Empty;
        public override void SetDefaults()
        {
            Projectile.height = 300;
            Projectile.width = 300;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Main.LocalPlayer.GetModPlayer<RegrePlayer>().FlashScreen(Projectile.Center, 20);
            RegreSystem.ScreenShakeAmount = 15;
            Main.LocalPlayer.velocity = RegreUtils.FromAToB(Projectile.Center, Main.LocalPlayer.Center) * 30;
        }
        public override bool ShouldUpdatePosition() => false;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = RegreUtils.GetExtraTexture("explosion");
            Texture2D tex2 = RegreUtils.GetExtraTexture("vortex3");
            Main.spriteBatch.Reload(BlendState.Additive);
            float alpha = MathHelper.Lerp(3, 0, Projectile.ai[0]);
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White * alpha, Projectile.rotation, tex.Size() / 2, Projectile.ai[0], SpriteEffects.None, 0);
            Main.spriteBatch.Draw(tex2, Projectile.Center - Main.screenPosition, null, new Color(161, 31, 197) * alpha, Main.GameUpdateCount * 0.003f, tex2.Size() / 2, Projectile.ai[0] * 1.1f, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(tex2, Projectile.Center - Main.screenPosition, null, Color.Purple * alpha, -Main.GameUpdateCount * 0.003f, tex2.Size() / 2, Projectile.ai[0] * 1.1f, SpriteEffects.None, 0);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }
        public override void AI()
        {
            Projectile.ai[0] += 0.025f;
            if (Projectile.ai[0] > 3)
                Projectile.Kill();
        }
    }
}
