using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Items.Weapons.Melee;
using Terraria.GameContent;
using Regressus.Dusts;
using Regressus.Items.Weapons.Magic;
using Terraria.DataStructures;

namespace Regressus.Projectiles.Melee
{
    public class LuminantLanceP : ModProjectile
    {
        public override string Texture => "Regressus/Items/Weapons/Melee/LuminantLance";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Luminant Lance");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 25;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        float holdOffset;
        int SwingTime;
        public override void SetDefaults()
        {
            SwingTime = 25;
            holdOffset = 15f;
            Projectile.width = 54;
            Projectile.height = 94;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = SwingTime;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
        }
        public float Lerp(float x)
        {
            float c1 = 1.70158f;
            float c2 = c1 * 1.525f;

            return (float)(x < 0.5f
              ? (Math.Pow(2 * x, 2) * ((c2 + 1) * 2 * x - c2)) / 2
              : (Math.Pow(2 * x - 2, 2) * ((c2 + 1) * (x * 2 - 2) + c2) + 2) / 2);
        }
        public override void AI()
        {
            AttachToPlayer();
        }
        bool runOnce;
        float rot;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Projectile.ai[1] <= 0)
            {
                Projectile.ai[1] = 40;
                for (int i = 0; i < 2; i++)
                {
                    float angle = RegreUtils.CircleDividedEqually(i, 2);
                    Vector2 pos = target.Center + (Vector2.One * 150).RotatedBy(angle);
                    Vector2 vel = RegreUtils.FromAToB(pos, target.Center) * 5;
                    Projectile.NewProjectile(Projectile.InheritSource(Projectile), pos, vel, ModContent.ProjectileType<LuminaryPF>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                }
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.ai[1] = 40;
        }
        public override bool ShouldUpdatePosition() => false;
        public void AttachToPlayer()
        {
            Projectile.ai[1]--;
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;
            if (!player.active || player.dead || player.CCed || player.noItems)
            {
                return;
            }
            if (!runOnce)
            {
                rot = Projectile.velocity.ToRotation();
                runOnce = true;
            }
            float progress = Lerp(Utils.GetLerpValue(0f, SwingTime, Projectile.timeLeft));
            //holdOffset = 35 * (progress + 0.25f);
            if (player.channel)
                Projectile.timeLeft = 2;
            //holdOffset = 0;
            Vector2 pos = player.RotatedRelativePoint(player.MountedCenter);
            player.ChangeDir(Projectile.velocity.X < 0 ? -1 : 1);
            player.itemRotation = rot * player.direction;
            pos += rot.ToRotationVector2() * holdOffset;
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, rot - MathHelper.PiOver2);
            //Projectile.rotation = (pos - player.Center).ToRotation() + MathHelper.PiOver2 - MathHelper.PiOver4 * Projectile.spriteDirection;
            Projectile.Center = pos;
            Projectile.rotation += MathHelper.ToRadians(15);
            player.itemTime = 2;
            player.itemAnimation = 2;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.localAI[0]++;
            if (Projectile.localAI[0] > 0)
            {
                Player player = Main.player[Projectile.owner];
                RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
                Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation + (Projectile.ai[0] == -1 ? 0 : MathHelper.PiOver2 * 3), texture.Size() / 2, Projectile.scale, Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            }
            return false;
        }
    }
}
