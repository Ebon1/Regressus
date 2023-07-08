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
using System.Threading.Tasks;
using Regressus.Items.Ammo;
using Terraria.Audio;
using System.Security.Cryptography.X509Certificates;
using static System.Formats.Asn1.AsnWriter;

namespace Regressus.Items.Weapons.Ranged
{
    public class ChildGrenade : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ladgrenade");
            Tooltip.SetDefault("explosive child");
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 12;
            Item.crit = 10;
            Item.damage = 25;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.reuseDelay = 30;
            Item.noMelee = true;
            Item.consumable = true;
            Item.autoReuse = false;
            Item.maxStack = 99999;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.rare = ItemRarityID.Red;
            Item.shootSpeed = 20f;
            Item.shoot = ModContent.ProjectileType<ChildGrenadeP>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe(5).AddIngredient(ModContent.ItemType<Starspore>()).Register();
        }
    }
    public class ChildGrenadeP : ModProjectile
    {
        public override string Texture => "Regressus/Items/Weapons/Ranged/ChildGrenade";
        public override void SetDefaults()
        {
            Projectile.width = 46;
            Projectile.height = 44;
            Projectile.aiStyle = 2;
            Projectile.friendly = true;
            Projectile.CritChance = 0;
            Projectile.hostile = false;
            Projectile.timeLeft = 400;
            Projectile.tileCollide = true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.position = Projectile.oldPosition;
            Projectile.velocity = Vector2.Zero;
            Projectile.aiStyle = -1;
            if (Projectile.timeLeft > 50)
                Projectile.timeLeft = 50;
            return false;
        }
        public override void PostDraw(Color lightColor)
        {
            float progress = Utils.GetLerpValue(0, 50, Projectile.timeLeft);
            float scale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI), 0, 1);
            Texture2D tex = RegreUtils.GetExtraTexture("star_01");
            Main.spriteBatch.Reload(BlendState.Additive);
            if (Projectile.timeLeft < 50)
                Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.Lerp(Color.Gold, Color.White, scale), Projectile.rotation, tex.Size() / 2, scale * 0.5f, SpriteEffects.None, 0);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
        }
        public override void AI()
        {
            float progress = Utils.GetLerpValue(0, 50, Projectile.timeLeft);
            float scale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);
            if (Projectile.timeLeft < 5)
                Projectile.Kill();
        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(new SoundStyle("Regressus/Sounds/Custom/Hyper"), Projectile.Center);
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ChildGrenadeP2>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
        }
    }
    public class ChildGrenadeP2 : ModProjectile
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
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Confused, 200);
        }
        public override bool ShouldUpdatePosition() => false;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = RegreUtils.GetExtraTexture("explosion");
            Main.spriteBatch.Reload(BlendState.Additive);
            float alpha = MathHelper.Lerp(1, 0, Projectile.ai[0]);
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.Lerp(Color.Gold, Color.White, alpha) * alpha * 1.9f, Main.GameUpdateCount * 0.003f, tex.Size() / 2, Projectile.ai[0] * 1.1f, SpriteEffects.None, 0);
            //Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White * alpha * 2, Projectile.rotation, tex.Size() / 2, Projectile.ai[0], SpriteEffects.None, 0);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }
        public override void AI()
        {
            Projectile.ai[0] += 0.05f;
            if (Projectile.ai[0] > 1)
                Projectile.Kill();
        }
    }
}
