using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Items.Dev;
using Regressus.Projectiles.Melee;
using Terraria.GameContent;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace Regressus.Projectiles.Dev
{
    public class EbonItemP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ebonian Slayer");
            Main.projFrames[Type] = 11;
        }
        public override void SetDefaults()
        {
            Projectile.width = 106;
            Projectile.height = 84;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 10;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        float rot;
        public override void OnSpawn(IEntitySource source)
        {
            rot = Projectile.rotation = Projectile.velocity.ToRotation();
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead || player.CCed || player.noItems)
            {
                return;
            }
            if (Main.mouseLeft)
            {
                player.itemTime = 2;
                player.itemAnimation = 2;
                Projectile.timeLeft = 2;
            }
            Projectile.direction = Projectile.spriteDirection = player.direction;
            if (Projectile.spriteDirection == -1)
                Projectile.rotation = rot + MathHelper.Pi;
            else
                Projectile.rotation = rot;
            player.heldProj = Projectile.whoAmI;
            Vector2 pos = player.RotatedRelativePoint(player.MountedCenter);
            Projectile.Center = pos;
            if (++Projectile.frameCounter >= 5)
            {
                if (Projectile.frame < 11)
                    Projectile.frame++;
                else
                {
                    Projectile.frame = 0;
                    player.ChangeDir(Main.MouseWorld.X < player.Center.X ? -1 : 1);
                    rot = RegreUtils.FromAToB(Projectile.Center, Main.MouseWorld).ToRotation();
                }
                Projectile.frameCounter = 0;
            }
        }
    }
}
