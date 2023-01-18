/*using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Projectiles.Ranged;
using Terraria.DataStructures;
using Terraria.GameContent;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.ObjectModel;
using Regressus.Projectiles;
using Regressus.Dusts;
using Terraria.Audio;

namespace Regressus.Items.Weapons.Ranged
{
    public class TheDistorter : ModItem
    {
        public override string Texture => "Regressus/Extras/Empty";
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Reverberated noises of flatulence.");
        }
        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 72;
            Item.crit = 25;
            Item.damage = 20;
            Item.useTime = 5;
            Item.useAnimation = 25;
            Item.reuseDelay = 20;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.rare = ItemRarityID.Orange;
            Item.shootSpeed = 20f;
            Item.shoot = ModContent.ProjectileType<Lens2>();
        }
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }
        int a = -1;
        Vector2 vel;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(SoundID.Item16);
            a++;
            if (a >= 5)
                a = 0;
            if (a == 0)
                vel = velocity;
            for (int i = 0; i < 5; i++)
                Dust.NewDust(position, 0, 0, DustID.FartInAJar, vel.X, vel.Y);
            Projectile.NewProjectile(source, position, vel, type, damage, knockback, player.whoAmI);
            return false;
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.NextFloat() >= .5f;
        }
    }
}
*/