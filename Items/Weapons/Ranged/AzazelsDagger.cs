using System;
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

namespace Regressus.Items.Weapons.Ranged
{
    public class AzazelsDagger : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Azazel's Dagger");
        }
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.crit = 23;
            Item.damage = 25;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.noMelee = true;
            Item.consumable = false;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.rare = ItemRarityID.Orange;
            Item.shootSpeed = 10f;
            Item.shoot = ModContent.ProjectileType<AzazelP>();
        }
    }
}
