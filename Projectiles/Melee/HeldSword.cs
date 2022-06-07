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
    public abstract class HeldSword : ModProjectile
    {
        public int SwingTime = 0;
        public float holdOffset = 50f;
        public override void SetDefaults()
        {
            Projectile.timeLeft = SwingTime;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public virtual float Lerp(float val)
        {
            return val;
        }
        public virtual void ExtraAI()
        {

        }
        public override void AI()
        {
            ExtraAI();
            AttachToPlayer();
        }
        public override bool ShouldUpdatePosition() => false;
        public void AttachToPlayer()
        {
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;
            if (!player.active || player.dead || player.CCed || player.noItems)
            {
                return;
            }
            float start = Projectile.velocity.ToRotation() - ((MathHelper.PiOver2) - 0.2f);
            float end = Projectile.velocity.ToRotation() + ((MathHelper.PiOver2) - 0.2f);
            float progress = Lerp(Utils.GetLerpValue(0f, SwingTime, Projectile.timeLeft));
            float rot = Projectile.ai[0] == 1 ? start.AngleLerp(end, progress) : start.AngleLerp(end, 1f - progress);
            Vector2 pos = player.RotatedRelativePoint(player.MountedCenter);
            player.ChangeDir(Projectile.velocity.X < 0 ? -1 : 1);
            player.itemRotation = rot * player.direction;
            pos += rot.ToRotationVector2() * holdOffset;
            Projectile.rotation = (pos - player.Center).ToRotation() + MathHelper.PiOver4;
            Projectile.Center = pos;
            player.itemTime = 2;
            player.itemAnimation = 2;
        }
    }
}

