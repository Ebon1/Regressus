using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Projectiles.Magic;
using Terraria.DataStructures;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Regressus.Projectiles.Minibosses.Vagrant;
using System.ComponentModel.DataAnnotations;
using Regressus.Items.Weapons.Ranged;

namespace Regressus.Items.Accessories
{
    public class InfectedIdol : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Maggots burst out from killed enemies.");
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<RegrePlayer>().infectedIdol = true;
        }
        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 72;
            Item.accessory = true;

            //Item.useAnimation = 60;
            //Item.useTime = 60;
            //Item.reuseDelay = 60;
            //Item.noMelee = true;
            //Item.autoReuse = false;
            //Item.mana = 15;
            //Item.DamageType = DamageClass.Magic;
            Item.rare = ItemRarityID.Lime;
            //Item.shootSpeed = 0.1f;
            //Item.shoot = ModContent.ProjectileType<GinnungagapP>();
        }
    }
    public class InfectedMaggot : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 10;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 500;
            Projectile.penetrate = 5;
            Projectile.tileCollide = true;
        }
        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Main.rand.NextVector2Unit(), ProjectileID.SporeCloud, 15, 0, Projectile.owner);
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(Projectile.Center, 22, 10, DustID.Sand);
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = -oldVelocity * 0.75f;
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.velocity *= 0.8f;
        }
        public override void AI()
        {
            Projectile.direction = Projectile.spriteDirection = Projectile.velocity.X > 0 ? 1 : -1;
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
            }
        }
    }
}
