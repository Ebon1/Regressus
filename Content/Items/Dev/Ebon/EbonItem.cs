using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Projectiles.Dev;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Chat;
using Terraria.Audio;
using Regressus.Common;

namespace Regressus.Content.Items.Dev.Ebon
{
    public class EbonItem : ModItem
    {
        public override string Texture => "Regressus/Extras/Empty";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ebon's mystery box");
            Tooltip.SetDefault("- It's a box!\n- What's inside?\n- Inside jokes.\nDedicated to Ebon.");
        }
        public override void SetDefaults()
        {
            Item.Size = Vector2.One;
            Item.damage = 69;
            Item.useTime = Item.useAnimation = 20;
            Item.reuseDelay = 10;
            Item.shoot = ModContent.ProjectileType<EbonP1>();
            Item.shootSpeed = 1;
            Item.useStyle = ItemUseStyleID.HiddenAnimation;
        }
        ParticleSystem sys = new();
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<EbonP1>()] == 0
                && player.ownedProjectileCounts[ModContent.ProjectileType<EbonP3>()] == 0
                && player.ownedProjectileCounts[ModContent.ProjectileType<EbonP4>()] == 0
                && player.ownedProjectileCounts[ModContent.ProjectileType<EbonP5>()] == 0;
        }
        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            if (line.Index == 0)
            {
                MiscDrawingMethods.DrawDevName(line, sys);
                return false;
            }
            if (line.Text == "Dedicated to Ebon.")
            {
                MiscDrawingMethods.DrawDevName(line, sys);
                return false;
            }
            return true;
        }
        int attackType = -1;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            attackType++;
            if (attackType >= 6)
            {
                attackType = 0;
            }
            switch (attackType)
            {
                case 0: //Exol
                    SoundStyle a = new("Regressus/Sounds/Custom/ExolIntro");
                    SoundStyle b = new("Regressus/Sounds/Custom/ExolSummon");
                    SoundEngine.PlaySound(a);
                    SoundEngine.PlaySound(b);
                    Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<EbonP1>(), damage, knockback, player.whoAmI);
                    break;
                case 1: ///Duke rainbow
                    SoundStyle c = new("Regressus/Sounds/Custom/DukeRainbow");
                    SoundEngine.PlaySound(c);
                    Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<EbonP1>(), damage, knockback, player.whoAmI);
                    break;
                case 2: //Ebonfly
                    break;
                case 3: //djungelskog
                    break;
                case 4: //bossling
                    break;
            }
            return false;
        }
    }
}
