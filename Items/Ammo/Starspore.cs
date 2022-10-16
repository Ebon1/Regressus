using System;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Regressus.Items.Ammo
{
    public class Starspore : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Ammo for the Sporesac.");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 14;
            Item.damage = 1;
            Item.DamageType = DamageClass.Ranged;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 2f;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.StarshroomDustP>();
            Item.ammo = Item.type;
        }
    }
}
