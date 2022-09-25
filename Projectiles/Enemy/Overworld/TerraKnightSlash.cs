
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Regressus.NPCs.Overworld;
using Microsoft.Xna.Framework.Graphics.PackedVector;

namespace Regressus.Projectiles.Enemy.Overworld
{
    internal class TerraKnightSlash : ModProjectile
    {
        public override string Texture => "Regressus/Extras/Extras2/slash_02";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terragrimoire");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 10;
            Projectile.friendly = false;
            Projectile.timeLeft = 400;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.scale = 0.25f;
            Projectile.tileCollide = false;
        }
        float alpha;
        public override void AI()
        {
            float progress = Utils.GetLerpValue(0, 500, Projectile.timeLeft);
            alpha = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            Projectile.velocity *= 1.015f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            sb.Reload(BlendState.Additive);
            Texture2D glow = RegreUtils.GetExtraTexture("Extras2/slash_01");
            Texture2D tex = RegreUtils.GetExtraTexture("Extras2/slash_02");
            var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
            {
                if (i == Projectile.localAI[0])
                    continue;
                Main.spriteBatch.Draw(glow, Projectile.oldPos[i] - Main.screenPosition + new Vector2(Projectile.width / 2f, Projectile.height / 2f), new Rectangle(0, 0, glow.Width, glow.Height), new Color(0, 255, Main.DiscoB) * (1f - fadeMult * i) * 0.75f * alpha, Projectile.oldRot[i], glow.Size() / 2, Projectile.scale, SpriteEffects.None, 0f);
            }
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, tex.Width, tex.Height), new Color(0, 255, Main.DiscoB) * 0.75f * alpha, Projectile.rotation, tex.Size() / 2, Projectile.scale, SpriteEffects.None, 0f);
            sb.Reload(BlendState.AlphaBlend);
            return false;
        }
    }
}
