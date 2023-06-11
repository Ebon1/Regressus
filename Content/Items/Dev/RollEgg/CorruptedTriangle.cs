using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Projectiles.Dev;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using Terraria.Audio;
using Regressus.Common;

namespace Regressus.Content.Items.Dev.RollEgg
{
    public class CorruptedTriangle : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Corrupted Triangle");
            Tooltip.SetDefault("\"Long live the new fresh!\"\nRight click for an alt attack\nDedicated to Roll'eG.");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 8));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.knockBack = 10f;
            Item.width = 36;
            Item.height = 50;
            Item.crit = 45;
            Item.damage = 90;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Generic;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.rare = ItemRarityID.Purple;
            Item.shootSpeed = 45f;
            Item.shoot = ModContent.ProjectileType<CorruptedTriangleP1>();
            Item.reuseDelay = 50;
        }
        ParticleSystem sys = new();
        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            if (line.Index == 0)
            {
                MiscDrawingMethods.DrawDevName(line, sys);
                return false;
            }
            if (line.Text == "Dedicated to Roll'eG.")
            {
                MiscDrawingMethods.DrawDevName(line, sys);
                return false;
            }
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1 && player.ownedProjectileCounts[ModContent.ProjectileType<CorruptedTriangleSummon1>()] < 4;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        int dir = 1;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            dir = -dir;
            if (player.altFunctionUse == 2)
            {
                for (int i = 0; i < 4; i++)
                {
                    SoundStyle a = new SoundStyle("Regressus/Sounds/Custom/RollSpawn");
                    float angle = 2f * (float)Math.PI / 4f * i;
                    SoundEngine.PlaySound(a);
                    Vector2 pos = player.Center + 20 * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                    Projectile proj = Projectile.NewProjectileDirect(source, new Vector2(position.X + velocity.X / 2, position.Y + velocity.Y / 2), velocity, ModContent.ProjectileType<CorruptedTriangleSummon1>(), damage, knockback, player.whoAmI, 0, angle);

                    proj.localAI[1] = angle;
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                    Dust.NewDustDirect(position + velocity, 5, 5, DustType<Dusts.RollParticle>(), -velocity.X / 10, -velocity.Y / 10);
                Projectile p = Projectile.NewProjectileDirect(source, new Vector2(position.X + velocity.X / 2, position.Y + velocity.Y / 2), velocity, type, damage, knockback, player.whoAmI, 0);
                p.localAI[0] = 45f * dir;
            }
            return false;
        }
    }
}
