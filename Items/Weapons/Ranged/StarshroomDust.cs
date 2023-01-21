using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Projectiles.Ranged;
using Terraria.DataStructures;
using Terraria.GameContent;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.ObjectModel;
using Regressus.Projectiles.SSW;
using Regressus.Buffs.Debuffs;

namespace Regressus.Items.Weapons.Ranged
{
    public class StarshroomDust : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Star Sac");
            Tooltip.SetDefault("Contains a sample of the witch's life effence");
        }
        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 72;
            Item.crit = 25;
            Item.damage = 30;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.rare = ItemRarityID.Orange;
            Item.shootSpeed = 12.5f;
            Item.shoot = ModContent.ProjectileType<SporesacF>();
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient<Items.Ammo.Starspore>(25).AddIngredient(ItemID.Mushroom, 35).AddTile(TileID.Anvils).Register();
        }
    }
    public class SporesacF : ModProjectile
    {
        public override string Texture => "Regressus/Projectiles/SSW/Sporesac";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sporesac");
        }
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 40;
            Projectile.aiStyle = 2;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 350;
            Projectile.tileCollide = true;

        }
        public override void Kill(int timeLeft)
        {
            Projectile a = Projectile.NewProjectileDirect(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<MushExplosion>(), 15, 0);
            a.friendly = true;
            a.hostile = false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<StarSacDebuff>(), 100);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.timeLeft > 0)
            {
                Projectile a = Projectile.NewProjectileDirect(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<MushExplosion>(), 15, 0);
                a.friendly = true;
                a.hostile = false;
                float progress = Utils.GetLerpValue(0, 350, Projectile.timeLeft);
                float speed = MathHelper.Lerp(3, 8, progress) * Main.rand.NextFloat(0.795f, 1f);
                if (Projectile.velocity.X != oldVelocity.X)
                    Projectile.velocity = -oldVelocity * 0.9f;
                else
                    Projectile.velocity = new Vector2(Projectile.direction * speed, speed * -2.5f);
                return false;
            }
            return true;
        }
        public override void AI()
        {
            Dust.NewDustPerfect(Projectile.Center + new Vector2(0, -20).RotatedBy(Projectile.rotation), DustID.Enchanted_Gold);
        }
    }
}
