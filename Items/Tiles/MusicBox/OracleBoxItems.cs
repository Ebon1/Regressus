using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Projectiles.Magic;
using Terraria.DataStructures;
using Terraria.GameContent;
using System.Collections.Generic;
using ReLogic.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.ObjectModel;
using Terraria.UI.Chat;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using ReLogic.Graphics;
using Terraria.Chat;
using Terraria.GameContent.UI.Chat;
using Terraria.GameContent.Creative;
using Regressus.Tiles.MusicBox;
using Terraria.Utilities;

namespace Regressus.Items.Tiles.MusicBox
{
    public class OracleBox1Item : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Music Box (The Oracle, Phase One)");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Sounds/Music/oracle"), ModContent.ItemType<OracleBox1Item>(), ModContent.TileType<OracleBox1>());
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<OracleBox1>();
            Item.width = 24;
            Item.height = 24;
            Item.rare = ItemRarityID.Purple;
            Item.value = 100000;
            Item.accessory = true;
        }
        /*public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            if (line.Index == 0)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Main.UIScaleMatrix);
                Regressus.Galaxy.CurrentTechnique.Passes[0].Apply();
                Regressus.Galaxy.Parameters["galaxy"].SetValue(ModContent.Request<Texture2D>("Regressus/Extras/starSky2").Value);
                var font = FontAssets.MouseText.Value;
                //Utils.DrawBorderString(Main.spriteBatch, line.text, new Vector2(line.X, line.Y), Color.White, 1);
                DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, font, line.Text, new Vector2(line.X, line.Y), Color.White);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
                return false;
            }
            return true;
        }*/
        public override bool? PrefixChance(int pre, UnifiedRandom rand)
        {
            return false;
        }
        public override int ChoosePrefix(UnifiedRandom rand)
        {
            return -1;
        }
    }
    public class OracleBox2Item : ModItem
    {
        public override bool? PrefixChance(int pre, UnifiedRandom rand)
        {
            return false;
        }
        public override int ChoosePrefix(UnifiedRandom rand)
        {
            return -1;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Music Box (The Oracle, Phase Two)");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Sounds/Music/oracle2"), ModContent.ItemType<OracleBox2Item>(), ModContent.TileType<OracleBox2>());
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<OracleBox2>();
            Item.width = 24;
            Item.height = 24;
            Item.rare = ItemRarityID.Purple;
            Item.value = 100000;
            Item.accessory = true;
        }
        /*public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            if (line.Index == 0)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Main.UIScaleMatrix);
                Regressus.Galaxy.CurrentTechnique.Passes[0].Apply();
                Regressus.Galaxy.Parameters["galaxy"].SetValue(ModContent.Request<Texture2D>("Regressus/Extras/starSky2").Value);
                var font = FontAssets.MouseText.Value;
                //Utils.DrawBorderString(Main.spriteBatch, line.text, new Vector2(line.X, line.Y), Color.White, 1);
                DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, font, line.Text, new Vector2(line.X, line.Y), Color.White);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
                return false;
            }
            return true;
        }*/
    }
}