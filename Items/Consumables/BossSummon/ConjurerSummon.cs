using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Regressus.Items.Consumables.BossSummon
{
    public class ConjurerSummon : ModItem
    {
        public override string Texture => "Regressus/Extras/Line";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sussy Conjurer Item");
            Tooltip.SetDefault("Not Consumable\nSummons The Conjurer\nCan only be used in the caverns.");
            ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 13;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Red;
            Item.value = Item.sellPrice(gold: 5);
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.rare = 1;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ZoneRockLayerHeight && !NPC.AnyNPCs(ModContent.NPCType<NPCs.Minibosses.TheConjurer>());
        }

        public override bool? UseItem(Player player)
        {
            NPC.NewNPCDirect(player.GetSource_ItemUse(Item), player.Center - (Microsoft.Xna.Framework.Vector2.UnitY * 200), ModContent.NPCType<NPCs.Minibosses.TheConjurer>());
            return true;
        }
    }
}
