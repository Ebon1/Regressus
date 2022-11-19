using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Projectiles.Magic;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Regressus.NPCs.Minibosses;
using System;
using Terraria.Audio;

namespace Regressus.Items.Weapons.Magic
{
    public class AngelicWand : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("magic wand.");
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 72;
            Item.crit = 25;
            Item.damage = 18;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.reuseDelay = 25;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.mana = 10;
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item9;
            Item.rare = ItemRarityID.Orange;
            Item.shootSpeed = 4;
            Item.shoot = ModContent.ProjectileType<LuminaryPF>();
        }
        int attacks;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            attacks++;
            if (attacks >= 5)
            {
                Vector2 vel = velocity;
                vel.Normalize();
                Projectile.NewProjectile(source, position + velocity, vel, ModContent.ProjectileType<LuminaryBeamF>(), damage * 2, knockback, player.whoAmI);
                attacks = 0;
            }
            else
                for (int i = 0; i < 2; i++)
                {
                    Projectile.NewProjectile(source, position + Main.rand.NextVector2CircularEdge(100, 100), Main.rand.NextVector2Unit() * Item.shootSpeed, type, damage, knockback, player.whoAmI);
                }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Items.DivineLight>(25)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
    public class LuminaryBeamF : LuminaryBeamBase
    {
        public override void Extra()
        {
            Projectile.friendly = true;
            Projectile.hostile = false;
            MAX_TIME = 26;
            Projectile.timeLeft = (int)MAX_TIME;
        }
    }
    public class LuminaryPF : ModProjectile
    {
        public override string Texture => "Regressus/NPCs/Minibosses/HomingScythe";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 8;
        }
        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 300;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(new("Regressus/Sounds/Custom/LuminaryConjure"));
        }
        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(5);
            Projectile.direction = Projectile.spriteDirection = -1;
            if (Projectile.timeLeft > 45)
            {
                if (++Projectile.frameCounter % 5 == 0 && Projectile.frame < 7)
                    Projectile.frame++;
            }
            else
            {
                if (++Projectile.frameCounter % 5 == 0 && Projectile.frame > 0)
                    Projectile.frame--;
            }
            Vector2 move = Vector2.Zero;
            float distance = 5050f;
            bool target = false;
            for (int k = 0; k < Main.maxNPCs; k++)
            {
                if (Main.npc[k].active && !Main.npc[k].friendly && !Main.npc[k].dontTakeDamage)
                {
                    Vector2 newMove = Main.npc[k].Center - Projectile.Center;
                    float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                    if (distanceTo < distance)
                    {
                        move = newMove;
                        distance = distanceTo;
                        target = true;
                    }
                }
            }
            if (++Projectile.ai[0] % 5 == 0 && target && Projectile.timeLeft > 45 && Projectile.timeLeft < 255)
            {
                AdjustMagnitude(ref move);
                Projectile.velocity = (6.2f * Projectile.velocity + move) / 6.2f;
                AdjustMagnitude(ref Projectile.velocity);
            }
            if (Projectile.timeLeft < 45)
            {
                Projectile.velocity *= 0.95f;
            }
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 6.2f)
            {
                vector *= 6.2f / magnitude;
            }
        }
    }
}
