using Terraria;
using Terraria.ModLoader;
using Regressus.Projectiles.Sentry;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace Regressus.Content.Items.Misc.SproutingPan
{
    public class SproutingPan : ModItem
    {
        public override void SetStaticDefaults() => Tooltip.SetDefault("Right click the summoned peashooters to upgrade them to a higher tier, this will consume one star for each tier.\nThere's a Zombie on Your Lawn!");

        public override void SetDefaults()
        {
            Item.DefaultToMagicWeapon(ModContent.ProjectileType<PvZ>(), 40, 0);
            Item.damage = 75;
            Item.DamageType = DamageClass.Summon;
            Item.noUseGraphic = true;
            Item.mana = 20;
            Item.rare = 7;
            Item.reuseDelay = 50;
        }
        /*public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            if (line.Index == 0)
            {
                MiscDrawingMethods.DrawFlawlessRarity(line);
                return false;
            }
            return true;
        }*/
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, Main.MouseWorld, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }
    }
}
