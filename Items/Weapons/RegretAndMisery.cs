using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Projectiles.Omniclass;
using Terraria.DataStructures;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace Regressus.Items.Weapons
{
    public class RegretAndMisery : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("RegreSUS");
            Tooltip.SetDefault("STOP POSTING ABOUT AMONG US! I'M TIRED OF SEEING IT! MY FRIENDS ON TIKTOK SEND ME MEMES, ON DISCORD IT'S MEMES!");
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 22;
            Item.crit = 50;
            Item.damage = 135;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.noUseGraphic = false;
            Item.DamageType = DamageClass.Generic;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.rare = ItemRarityID.Purple;
            Item.shootSpeed = 15f;
            Item.shoot = ModContent.ProjectileType<RegretAndMiseryP>();
        }

        public static readonly SoundStyle sussySound = new("Regressus/Sounds/Custom/Sussy", SoundType.Sound)
        {
            Volume = 0.65f,
            Pitch = 0,
            PitchVariance = 0.2f
        };
        public static readonly SoundStyle sussySound2 = new("Regressus/Sounds/Custom/Sussy2", SoundType.Sound)
        {
            Volume = 0.75f,
            Pitch = 0,
            PitchVariance = 0.2f
        };

        int attacks;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (++attacks >= 15)
                attacks = 0;
            // What the fuck is this cringe
            SoundEngine.PlaySound(sussySound, player.position);
            Projectile proj = Main.projectile[Projectile.NewProjectile(source, new Vector2(Main.MouseWorld.X, Main.screenPosition.Y), new Vector2(Main.rand.NextFloat(-2.5f, 2.5f), Item.shootSpeed), type, damage, knockback, player.whoAmI)];
            if (attacks == 14)
            {
                Terraria.Audio.SoundEngine.PlaySound(sussySound2);
                proj.scale = 5f;
                proj.damage = damage * 2;
            }
            return false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine tooltipLine = tooltips.Find((TooltipLine x) => x.Name == "ItemName");
            tooltipLine.OverrideColor = new Color(255, 0, 0, Main.DiscoR);
            TooltipLine tooltipLine2 = tooltips.Find((TooltipLine x) => x.Text == "STOP POSTING ABOUT AMONG US! I'M TIRED OF SEEING IT! MY FRIENDS ON TIKTOK SEND ME MEMES, ON DISCORD IT'S MEMES!");
            tooltipLine2.OverrideColor = new Color(255, 0, 0, Main.DiscoR);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Items.Weapons.Magic.Vision>(1).
                AddIngredient(ItemID.LunarBar, 35).
                AddIngredient(ItemID.SuspiciousLookingEye, 1).
                AddIngredient(ItemID.SuspiciousLookingTentacle, 1).
                AddIngredient(ItemID.FragmentVortex, 25).
                AddIngredient(ItemID.FragmentStardust, 25).
                AddIngredient(ItemID.FragmentSolar, 25).
                AddIngredient(ItemID.FragmentNebula, 25).
                AddTile(TileID.LunarCraftingStation).Register();
        }
    }
}
