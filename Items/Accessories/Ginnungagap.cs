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
    public class Ginnungagap : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ginnungagap in a jar");
            Tooltip.SetDefault("Has a chance to summon a Ginnungagap rift upon hitting an enemy with a magic weapon.\nDisable visibilty to remove the vfx.\nThe primordial void in a jar. Literally just in a regular jar.");
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (hideVisual)
                player.GetModPlayer<RegrePlayer>().ginnungagapHide = true;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<RegrePlayer>().ginnungagap = true;
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
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Bottle)
                .AddIngredient(ItemID.SoulofNight, 15)
                .AddIngredient(ItemID.SoulofLight, 15)
                .AddIngredient(ItemID.SoulofSight, 15)
                .AddIngredient(ItemID.SoulofMight, 15)
                .AddIngredient(ItemID.SoulofFright, 15)
                .AddIngredient(ItemID.SoulofFlight, 15)
                .AddIngredient(ModContent.ItemType<StarshroomDust>(), 1)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class GinnungagapP : ModProjectile
    {
        public override string Texture => "Regressus/Extras/Empty";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ginnungagap");
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            float mult = 0.55f + (float)Math.Sin(Main.GlobalTimeWrappedHourly/* * 2*/) * 0.1f;
            float scale = Projectile.scale * 3 * mult;
            const float TwoPi = (float)Math.PI * 2f;
            float scalee = (float)Math.Sin(Main.GlobalTimeWrappedHourly * TwoPi / 2f) * 0.3f + 0.7f;
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Extras/vortex3").Value, Projectile.Center - Main.screenPosition, null, Color.Lerp(new Color(47, 50, 120), new Color(147, 96, 107), scalee) * Projectile.scale * 4, -Main.GameUpdateCount * 0.0075f, new Vector2(1230, 1264) / 2, scale, SpriteEffects.None, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            return false;
        }
        float MAX_TIME = 250;
        public override void SetDefaults()
        {
            Projectile.width = 256 / 4;
            Projectile.height = 256 / 4;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.damage = 0;
            Projectile.timeLeft = (int)MAX_TIME;
            Projectile.CritChance = 0;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Projectile.CritChance = 0;
            if (Main.player[Projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<GinnungagapP>()] > 1)
            {
                Projectile.Kill();
            }
            float progress = Utils.GetLerpValue(0, MAX_TIME, Projectile.timeLeft);
            Projectile.scale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 6, 0, 1) * 0.25f;
            Lighting.AddLight(Projectile.Center, TorchID.UltraBright);
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.boss && npc.knockBackResist != 0f && npc.Center.Distance(Projectile.Center) < 1020)
                {
                    npc.velocity = RegreUtils.FromAToB(Projectile.Center, npc.Center, false, reverse: true) * (0.05f * npc.knockBackResist);
                }
            }


            if (Projectile.scale == 0.25)
                for (int num901 = 0; num901 < 5; num901++)
                {
                    int num902 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width * 4, Projectile.height * 4, DustID.Dirt, 0f, 0f, 200, default, 1.25f);
                    Main.dust[num902].position = Projectile.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * Projectile.width * 4;
                    Main.dust[num902].noGravity = true;
                    Main.dust[num902].velocity = (RegreUtils.FromAToB(Main.dust[num902].position, Projectile.Center) * 6.5f).RotatedBy(-Main.GameUpdateCount * 0.0075f);
                    Dust dust2 = Main.dust[num902];
                    dust2.velocity *= 3f;
                }
            if (Projectile.scale == 0.25)
                for (int num903 = 0; num903 < 2; num903++)
                {
                    int num904 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width * 4, Projectile.height * 4, DustID.Stone, 0f, 0f, 0, default, 1.25f);
                    Main.dust[num904].position = Projectile.Center + Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(Projectile.velocity.ToRotation()) * Projectile.width * 4;
                    Main.dust[num904].noGravity = true;
                    Main.dust[num904].velocity = (RegreUtils.FromAToB(Main.dust[num904].position, Projectile.Center) * 6.5f).RotatedBy(Projectile.rotation);
                    Dust dust2 = Main.dust[num904];
                    dust2.velocity *= 3f;
                }
        }
    }
}
