using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
namespace Regressus.Projectiles.Melee
{
    public class AnimosityP : HeldSwordV2
    {
        public override void SetExtraDefaults()
        {
            Projectile.width = 76;
            Projectile.height = 82;
        }
        public override string Texture => "Regressus/Items/Weapons/Melee/Animosity";
        public override void PostDraw(Color lightColor)
        {
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            Player player = Main.player[Projectile.owner];
            Texture2D slash = ModContent.Request<Texture2D>("Regressus/Extras/Extras2/slash_02").Value;
            float mult = Ease(Utils.GetLerpValue(0f, swingTime, Projectile.timeLeft));
            float alpha = (float)Math.Sin(mult * Math.PI);
            Vector2 pos = player.Center + Projectile.velocity * (45f - mult * 45);
            Main.spriteBatch.Draw(slash, pos - Main.screenPosition, null, new Color(255, 44, 44) * alpha, Projectile.velocity.ToRotation() - MathHelper.PiOver2, slash.Size() / 2, Projectile.scale / 1.95f, SpriteEffects.None, 0f);
            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Vector2 pos = target.Center + Main.rand.NextVector2CircularEdge(Main.screenHeight / 1.65f, Main.screenHeight / 1.65f);
            Vector2 vel = RegreUtils.FromAToB(pos, target.Center, true) * 15f;
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), pos, vel, ModContent.ProjectileType<AnimosityP2>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
        }
    }
    public class AnimosityP2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 6;
        }
        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(64, 86);
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.timeLeft = 80;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            float progress = Utils.GetLerpValue(0, 80, Projectile.timeLeft);
            return Color.White * MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (++Projectile.frameCounter > 5)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 5)
                    Projectile.frame = 0;
            }
        }
    }
}
