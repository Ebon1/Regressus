using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Projectiles.Sentry;
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
using Terraria.Audio;
using Terraria.GameContent.UI.Chat;

namespace Regressus.Items.Weapons.Magic
{
    public class SproutingPan : ModItem
    {
        public override string Texture => "Regressus/Extras/Empty";
        public override void SetStaticDefaults() => Tooltip.SetDefault("Right click the summoned peashooters to upgrade them to a higher tier, this will consume one star for each tier.\nThere's a Zombie on Your Lawn!");

        public override void SetDefaults()
        {
            Item.DefaultToMagicWeapon(ModContent.ProjectileType<PvZ>(), 40, 0);
            Item.damage = 75;
            Item.noUseGraphic = true;
            Item.mana = 20;
            Item.reuseDelay = 50;
        }
        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            if (line.Index == 0)
            {
                MiscDrawingMethods.DrawFlawlessRarity(line);
                return false;
            }
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, Main.MouseWorld, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }
    }
}
