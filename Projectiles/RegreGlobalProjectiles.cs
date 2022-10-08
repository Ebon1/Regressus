using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Audio;
using Terraria.DataStructures;
using Regressus.Projectiles.Minibosses.Vagrant;

namespace Regressus.Projectiles
{
    public class RegreGlobalProjectiles : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override void PostAI(Projectile projectile)
        {
            if (projectile.friendly && projectile.DamageType == DamageClass.Magic && projectile.type != ProjectileID.RainbowBack && projectile.type != ProjectileID.RainbowFront && projectile.type != ModContent.ProjectileType<LightningF>())
            {
                if (Main.player[projectile.owner].GetModPlayer<RegrePlayer>().ginnungagap && !Main.player[projectile.owner].GetModPlayer<RegrePlayer>().ginnungagapHide)
                {
                    Dust.NewDustPerfect(projectile.Center, ModContent.DustType<Dusts.GinnungagapDust>(), Vector2.Zero, 150, Color.White, Main.rand.NextFloat(1, 1.75f)).noGravity = true;

                }
            }
        }
    }
}
