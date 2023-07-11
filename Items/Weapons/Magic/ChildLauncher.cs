using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Projectiles.Magic;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Regressus.NPCs.Minibosses;
using System;
using Terraria.Audio;
using Regressus.Projectiles.SSW;
using Microsoft.Xna.Framework.Graphics;
using Regressus.Dusts;

namespace Regressus.Items.Weapons.Magic
{
    public class ChildLauncher : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Harness the power of children to kill your opponents");
        }

        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 72;
            Item.crit = 25;
            Item.damage = 10;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.reuseDelay = 25;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.mana = 5;
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item9;
            Item.rare = ItemRarityID.Orange;
            Item.shootSpeed = 10;
            Item.shoot = ModContent.ProjectileType<MissileLadF>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
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
    public class MissileLadF : ModProjectile
    {
        public override string Texture => "Regressus/Projectiles/SSW/MissileLad";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Star Rocket");

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            Main.projFrames[Projectile.type] = 2;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.scale = 1f;
            Projectile.Size = new Vector2(30, 24);

            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;

            Projectile.aiStyle = -1;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void Kill(int timeLeft)
        {
            if (Projectile.frame == 0)
            {
                Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<StarExplosionF>(), Projectile.damage, Projectile.knockBack);
            }
            else
            {
                Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<StarExplosionF2>(), Projectile.damage, Projectile.knockBack);
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.frame = Main.rand.NextBool(100000) ? 1 : 0;
        }
        public override void AI()
        {
            if (Projectile.ai[1] != 0)
                Projectile.frame = (int)Projectile.ai[1];
            Player player = Main.player[Projectile.owner];
            Projectile.ai[0]++;
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.frame == 0)
            {

                Dust.NewDustPerfect(Projectile.Center - new Vector2(0, 24).RotatedBy(Projectile.rotation - MathHelper.PiOver2), ModContent.DustType<Smoke>(), Projectile.velocity, 0, new Color(255, 177, 0), 0.025f).noGravity = true;
            }
            else
            {
                //Projectile.velocity *= 1.1f;
                Dust.NewDustPerfect(Projectile.Center - new Vector2(0, 24).RotatedBy(Projectile.rotation - MathHelper.PiOver2), ModContent.DustType<Smoke>(), Projectile.velocity, 0, new Color(161, 31, 197), 0.05f).noGravity = true;
            }

        }
    }
    public class StarExplosionF : ModProjectile
    {
        public override string Texture => RegreUtils.Empty;
        public override void SetDefaults()
        {
            Projectile.height = 150;
            Projectile.width = 150;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
        }
        public override bool ShouldUpdatePosition() => false;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = RegreUtils.GetExtraTexture("explosion");
            Main.spriteBatch.Reload(BlendState.Additive);
            float alpha = MathHelper.Lerp(0.5f, 0, Projectile.ai[0]);
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, new Color(255, 177, 0) * alpha * 1.9f, Main.GameUpdateCount * 0.003f, tex.Size() / 2, Projectile.ai[0] * 1.1f, SpriteEffects.None, 0);
            //Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White * alpha * 2, Projectile.rotation, tex.Size() / 2, Projectile.ai[0], SpriteEffects.None, 0);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }
        public override void AI()
        {
            Projectile.ai[0] += 0.05f;
            if (Projectile.ai[0] > 0.5f)
                Projectile.Kill();
        }
    }
    public class StarExplosionF2 : ModProjectile
    {
        public override string Texture => RegreUtils.Empty;
        public override void SetDefaults()
        {
            Projectile.height = 300;
            Projectile.width = 300;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Main.LocalPlayer.GetModPlayer<RegrePlayer>().FlashScreen(Projectile.Center, 70);
            RegreSystem.ScreenShakeAmount = 17;
            SoundEngine.PlaySound(new SoundStyle("Regressus/Sounds/Custom/HyperNuke"), Projectile.Center);
        }
        public override bool ShouldUpdatePosition() => false;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = RegreUtils.GetExtraTexture("explosion");
            Texture2D tex2 = RegreUtils.GetExtraTexture("vortex3");
            Main.spriteBatch.Reload(BlendState.Additive);
            float alpha = MathHelper.Lerp(1, 0, Projectile.ai[0] / 6);
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White * alpha, Projectile.rotation, tex.Size() / 2, Projectile.ai[0], SpriteEffects.None, 0);
            Main.spriteBatch.Draw(tex2, Projectile.Center - Main.screenPosition, null, new Color(161, 31, 197) * alpha, Main.GameUpdateCount * 0.003f, tex2.Size() / 2, Projectile.ai[0] * 1.1f, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(tex2, Projectile.Center - Main.screenPosition, null, Color.Purple * alpha, -Main.GameUpdateCount * 0.003f, tex2.Size() / 2, Projectile.ai[0] * 1.1f, SpriteEffects.None, 0);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }
        public override void AI()
        {
            Projectile.ai[0] += 0.1f;
            if (Projectile.ai[0] > 4)
                Projectile.Kill();
        }
    }
}
