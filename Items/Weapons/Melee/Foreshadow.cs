using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Projectiles.Melee;
using Terraria.DataStructures;
using Terraria.GameContent;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.ObjectModel;
using Terraria.UI.Chat;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using ReLogic.Graphics;
using Terraria.Chat;
using Terraria.GameContent.UI.Chat;

namespace Regressus.Items.Weapons.Melee
{
    public class Foreshadow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Foreshadow");
            Tooltip.SetDefault("Slices rifts in reality to a grim future.");
        }
        public override void SetDefaults()
        {
            Item.knockBack = 10f;
            Item.width = Item.height = 82;
            Item.crit = 45;
            Item.damage = 185;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Yellow;
            Item.shootSpeed = 1f;
            Item.shoot = ModContent.ProjectileType<ForeshadowP>();
        }
        public int dir = 1, attacks = -1;
        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
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
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            dir = -dir;
            if (player.altFunctionUse == 2)
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<ForeshadowP3>(), damage, knockback, player.whoAmI, dir, attacks);
            else
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, dir, attacks);
            return false;
        }
    }
}
