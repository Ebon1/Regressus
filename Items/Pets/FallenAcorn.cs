using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Regressus.Buffs.Pets;
using Regressus.Projectiles.Pets;

namespace Regressus.Items.Pets
{
    public class FallenAcorn : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            DisplayName.SetDefault("Fairy Stick");
            Tooltip.SetDefault("Summons a forest friend!");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.UseSound = SoundID.NPCHit4;
            Item.value = Item.sellPrice(0, 0, 5, 0);
            Item.rare = 3;
            Item.buffType = ModContent.BuffType<AcornBuff>();
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600, true);
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            type = ModContent.ProjectileType<AcornFairyPet>();
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        /*public override void AddRecipes()
        {
            Mod calamityMod = ModLoader.GetMod("CalamityMod");
            if (calamityMod != null)
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddRecipeGroup("AnyHardmodeDrill", 1);
                recipe.AddIngredient(calamityMod.ItemType("DubiousPlating"), 15);
                recipe.AddIngredient(calamityMod.ItemType("MysteriousCircuitry"), 5);
                recipe.AddIngredient(ModLoader.GetMod("CalamityMod").ItemType("PowerCell"), 10);
                recipe.AddTile(ModLoader.GetMod("CalamityMod").TileType("LaboratoryConsole"));
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }*/
    }
}