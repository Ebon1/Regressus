using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Regressus.Tiles.Trophies;
using Regressus.Tiles.Relics;

namespace Regressus.Items.Tiles.Relics
{
	public class LuminaryRelicItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Luminary Relic");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<LuminaryRelic>());

			Item.width = 32;
			Item.height = 32;
			Item.maxStack = 9999;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 1);
		}
	}
}
