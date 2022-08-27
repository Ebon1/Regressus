using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.GameContent;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.ObjectModel;
namespace Regressus.Items.Weapons.Summon.Minions
{
    public class IceLantern : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cloudburst");
        }
        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 72;
            Item.crit = 25;
            Item.damage = 18;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.noMelee = true;
            Item.mana = 5;
            Item.DamageType = DamageClass.Summon;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item9;
            Item.rare = ItemRarityID.LightRed;
            Item.shootSpeed = 1f;
            Item.shoot = ModContent.ProjectileType<IceLanternP>();
            Item.buffType = ModContent.BuffType<Buffs.Minions.IceLanternB>();
            Item.buffTime = 60;
        }
        float i;
        float[] oldAngle = new float[50];
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (!player.HasBuff<Buffs.Minions.IceLanternB>())
                i = 0;
            if (i != player.maxMinions)
                i++;
            float angle = 2f * (float)Math.PI / player.maxMinions * i;
            oldAngle[(int)i] = angle;
            Projectile.NewProjectile(source, player.Center, Vector2.Zero, type, damage, knockback, player.whoAmI, i, angle);
            foreach (Projectile p in Main.projectile)
            {
                if (p.active && p.type == type && p.owner == player.whoAmI)
                    p.ai[1] = oldAngle[(int)p.ai[0]];
            }
            return false;
        }
    }
    public class IceLanternP : ModProjectile
    {


        public override string Texture => "Regressus/Projectiles/Minibosses/Vargant/Hail1";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hhail");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 42;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 500;
            Projectile.minion = true;
            Projectile.minionSlots = .5f;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
        }
        Vector2 center;
        public override void AI()
        {
            Player p = Main.player[Projectile.owner];
            Projectile.ai[1] += 2f * (float)Math.PI / 600f * 10f;
            Projectile.ai[1] %= 2f * (float)Math.PI;
            if (Main.mouseRight)
                center = Main.MouseWorld;
            else
                center = p.Center;
            int dist = (Main.mouseRight ? 60 : 100);
            Projectile.Center = center + dist * new Vector2((float)Math.Cos(Projectile.ai[1]), (float)Math.Sin(Projectile.ai[1]));
            Projectile.rotation += MathHelper.ToRadians(5);

            if (p.HasBuff<Buffs.Minions.IceLanternB>())
                Projectile.timeLeft = 10;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.Reload(BlendState.Additive);
            Texture2D tex = ModContent.Request<Texture2D>("Regressus/Projectiles/Minibosses/Vargant/Hail1").Value;
            Texture2D glow = ModContent.Request<Texture2D>("Regressus/Projectiles/Minibosses/Vargant/Hail1_Glow").Value;
            var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
            {
                Main.spriteBatch.Draw(glow, Projectile.oldPos[i] - Main.screenPosition + new Vector2(Projectile.width / 2f, Projectile.height / 2f), new Rectangle(0, 0, glow.Width, glow.Height), Color.LightBlue * (1f - fadeMult * i), Projectile.oldRot[i], glow.Size() / 2, Projectile.scale * (ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type], Projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            }

            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, tex.Width, tex.Height), Color.White, Projectile.rotation, tex.Size() / 2, Projectile.scale, Projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            return false;
        }
    }
}
