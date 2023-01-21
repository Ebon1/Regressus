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
using Regressus.Projectiles.Melee;

namespace Regressus.Items.Weapons.Melee
{
    public class ChroniteSaw : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Slows down enemies on hit.");
        }
        public override void SetDefaults()
        {
            Item.knockBack = 0f;
            Item.width = Item.height = 82;
            Item.crit = 5;
            Item.damage = 20;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.noUseGraphic = true;
            Item.axe = 17;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.LightPurple;
            Item.shootSpeed = 1f;
            Item.shoot = ModContent.ProjectileType<ChroniteSawP>();
        }
    }
}
