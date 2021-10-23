using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Regressus.Items.Weapons.Magic
{
    public class PlantStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sap Belcher");
            Tooltip.SetDefault("Fires spreads of lingering sap.");
            Item.staff[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.height = 42;
            Item.width = 42;
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.damage = 12;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.mana = 7;
            Item.noMelee = true;
            Item.knockBack = 1f;
            Item.shoot = ModContent.ProjectileType<Projectiles.Curserisen.CurserisenSap>();
            Item.shootSpeed = 6f;
            Item.autoReuse = true;
            Item.UseSound = SoundID.NPCDeath19;
            Item.rare = ItemRarityID.Green;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity);

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
        }
        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(20));
                Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
            }
            return false;
        }
    }
}