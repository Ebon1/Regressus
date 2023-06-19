using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Regressus.Dusts;
using Regressus.Effects;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace Regressus.Items
{
    public class StarRocket : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons the Starfall"
                + "\n'Off to the stars'");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.shootSpeed = 5f;

            Item.shoot = ModContent.ProjectileType<StarRocketPro>();
            Item.Size = new Vector2(26, 50);
            Item.scale = 1f;

            Item.useTime = Item.useAnimation = 45;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useTurn = false;

            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.Green;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int k = 0; k < 5; k++)
            {
                Projectile.NewProjectile(source, position, velocity.RotatedByRandom(360), type, damage, knockback, player.whoAmI, -k * 20);
            }

            return false;
        }
    }

    public class StarRocketPro : ModProjectile
    {
        public override string Texture => "Regressus/Items/StarRocket";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Star Rocket");

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.friendly = false;
            Projectile.hostile = false;

            Projectile.scale = 1f;
            Projectile.Size = new Vector2(26, 26);

            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;

            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            Projectile.ai[0]++;

            if (Projectile.ai[0] == 60f)
            {
                SoundEngine.PlaySound(SoundID.Item110);
            }

            if (Projectile.ai[0] >= 60f)
            {
                Projectile.tileCollide = false;

                Projectile.velocity += (Projectile.rotation - MathHelper.PiOver2).ToRotationVector2();

                Dust.NewDustPerfect(Projectile.Center - new Vector2(0, -26).RotatedBy(Projectile.rotation), DustID.Torch, Main.rand.NextFloat(-4, 4).ToRotationVector2(), 0, default, 2).noGravity = true;
                Dust.NewDustPerfect(Projectile.Center - new Vector2(0, -26).RotatedBy(Projectile.rotation), ModContent.DustType<Smoke>(), Main.rand.NextFloat(-4, 4).ToRotationVector2(), 0, new Color(255, 177, 0), 1).noGravity = true;
            }
            else
            {
                Projectile.rotation = Projectile.velocity.X * 0.2f;

                Projectile.velocity *= 0.97f;
            }

            if (Projectile.ai[0] >= 100f)
            {
                Projectile.Kill();
            }

            Lighting.AddLight(Projectile.Center, new Color(241, 212, 62).ToVector3() * 0.5f);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon) { Projectile.velocity.X = -oldVelocity.X; }
            if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon) { Projectile.velocity.Y = -oldVelocity.Y; }

            return false;
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];

            if (player.ownedProjectileCounts[Type] <= 1)
            {
                //RegreUtils.SetBossTitle(150, "-Starfall-", new Color(255, 230, 130), "Where gamers meet the stars", BossTitleStyleID.SSW);
                RegreUtils.SetBossTitle(150, "-Starfall-", new Color(255, 230, 130), "The Cosmic Cascade", BossTitleStyleID.SSW);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int frameY = frameHeight * Projectile.frame;

            Rectangle sourceRectangle = new Rectangle(0, frameY, texture.Width, frameHeight);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Vector2 position = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Color color = Projectile.GetAlpha(Color.White);

            SpriteEffects spriteEffects = SpriteEffects.None;

            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 oldPosition = Projectile.oldPos[k] + (sourceRectangle.Size() / 2f) - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);

                Main.EntitySpriteDraw(texture, oldPosition, sourceRectangle, color, Projectile.rotation, origin, new Vector2(1f - (Projectile.velocity.Length() * 0.01f), 1f + (Projectile.velocity.Length() * 0.01f)), spriteEffects, 0);
            }

            Main.EntitySpriteDraw(texture, position, sourceRectangle, color, Projectile.rotation, origin, new Vector2(1f - (Projectile.velocity.Length() * 0.01f), 1f + (Projectile.velocity.Length() * 0.01f)), spriteEffects, 0);

            return false;
        }
    }

    public class Starfall : ModBiome
    {
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Starfall");

        public override SceneEffectPriority Priority => SceneEffectPriority.Event;

        public override bool IsBiomeActive(Player player)
        {
            return (player.ZoneNormalSpace || player.ZoneOverworldHeight || player.ZoneDirtLayerHeight) && !Main.dayTime;
        }

        public override void OnEnter(Player player)
        {
            SkyManager.Instance.Activate("SSWBackground", default(Vector2));
        }

        public override void OnLeave(Player player)
        {
            SkyManager.Instance.Deactivate("SSWBackground", default(Vector2));
        }
    }
}
