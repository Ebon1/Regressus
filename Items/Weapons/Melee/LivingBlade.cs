using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Projectiles.Melee;
//using Regressus.Projectiles.Minions;

namespace Regressus.Items.Weapons.Melee
{
    public class LivingBlade : ModItem
    {
        int swingDir = 0;
        int mode = 0;
        public override void SetDefaults()
        {
            Item.width = Item.height = 64;
            Item.damage = 100;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.shoot = ModContent.ProjectileType<LivingBladeHeld>();
            Item.shootSpeed = 1;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Melee;
            Item.reuseDelay = 10;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.Yellow;
            Item.channel = true;
        }
        /*public override bool AltFunctionUse(Player player) => true;
        public override void UpdateInventory(Player player)
        {
            if (mode == 1)
            {
                player.GetModPlayer<RegrePlayer>().bladeSummon = true;
                int type = ModContent.ProjectileType<LivingBladeSummon>();
                if (player.ownedProjectileCounts[type] < 1)
                {
                    Projectile.NewProjectile(default, player.Center, Vector2.Zero, type, Item.damage * 3, Item.knockBack, player.whoAmI);
                }
            }
        }*/
        /*public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 0)
            {
                if (mode == 0)
                {
                    Item.DamageType = DamageClass.Melee;
                    return true;
                }
                else
                {
                    Item.DamageType = DamageClass.Summon;
                    return false;
                }
            }
            else
            {
                mode = (mode + 1) % 2;
            }
            return base.UseItem(player);
        }*/
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (Main.rand.NextBool(3))
                Item.NewItem(player.GetSource_FromThis(), target.getRect(), ModContent.ItemType<Items.Consumables.LivingBladePickup>());
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (mode == 0)
            {
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0, swingDir);
                swingDir = (swingDir + 1) % 2;
            }
            return false;
        }
    }
}
