using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Projectiles.Dev;
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

namespace Regressus.Items.Dev
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
                && player.ownedProjectileCounts[ModContent.ProjectileType<EbonP2>()] == 0
                && player.ownedProjectileCounts[ModContent.ProjectileType<EbonP3>()] == 0
                && player.ownedProjectileCounts[ModContent.ProjectileType<EbonP4>()] == 0
                && player.ownedProjectileCounts[ModContent.ProjectileType<EbonP5>()] == 0
                && player.ownedProjectileCounts[ModContent.ProjectileType<EbonP6>()] == 0;
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
                case 1: //Ebonfly
                    break;
                case 2: //Duke rainbow
                    break;
                case 3: //amogi sussy haha
                    break;
                case 4: //djungelskog
                    break;
                case 5: //bossling
                    break;
            }
            return false;
        }
    }
}
