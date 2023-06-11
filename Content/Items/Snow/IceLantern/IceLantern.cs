using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Regressus.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Regressus.Projectiles.Minibosses.Vagrant;
using Terraria.Audio;

namespace Regressus.Items.Weapons.Magic
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
            Item.damage = 8;
            Item.useAnimation = 40;
            Item.useTime = 40;
            Item.reuseDelay = 30;
            Item.noMelee = true;
            Item.mana = 5;
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item9;
            Item.rare = ItemRarityID.LightRed;
            Item.shootSpeed = 1f;
            Item.shoot = ModContent.ProjectileType<IceLanternP>();
            //Item.buffType = ModContent.BuffType<Buffs.Minions.IceLanternB>();
            //Item.buffTime = 60;
        }
        float i;
        float[] oldAngle = new float[50];
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //if (!player.HasBuff<Buffs.Minions.IceLanternB>())
            //    i = 0;
            //if (i != player.maxMinions)
            //    i++;
            //oldAngle[(int)i] = angle;
            for (int i = 0; i < 5; i++)
            {
                float angle = 2f * (float)Math.PI / 5 * i;
                Projectile.NewProjectile(source, player.Center, Vector2.Zero, type, damage, knockback, player.whoAmI, ai1: angle);
            }
            /*foreach (Projectile p in Main.projectile)
            {
                if (p.active && p.type == type && p.owner == player.whoAmI)
                    p.ai[1] = oldAngle[(int)p.ai[0]];
            }*/
            return false;
        }
    }
    public class IceLanternP : ModProjectile
    {
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
            //Projectile.minion = true;
            //Projectile.minionSlots = .5f;
            Projectile.timeLeft = 80;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
        }
        Vector2 center;
        int timer;
        public override void AI()
        {
            Player p = Main.player[Projectile.owner];
            Projectile.ai[1] += 2f * (float)Math.PI / 600f * 10f;
            Projectile.ai[1] %= 2f * (float)Math.PI;
            if (Projectile.ai[0] == 0)
            {
                center = Projectile.Center;
                Projectile.ai[0] = 1;
            }
            //if (Main.mouseRight)
            //    center = Main.MouseWorld;
            //else
            //    center = p.Center;
            int dist = 150;//(Main.mouseRight ? 60 : 100);
            Projectile.Center = center + dist * new Vector2((float)Math.Cos(Projectile.ai[1]), (float)Math.Sin(Projectile.ai[1]));
            Projectile.rotation += MathHelper.ToRadians(5);

            //if (p.HasBuff<Buffs.Minions.IceLanternB>())
            //    Projectile.timeLeft = 10;

            float progress = Utils.GetLerpValue(0, 80, Projectile.timeLeft);
            Projectile.scale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);
            if (++timer > 40)
            {
                timer = -50;
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, RegreUtils.FromAToB(Projectile.Center, center) * 5, ModContent.ProjectileType<IceLanternP2>(), (int)(Projectile.damage * 2), 0, p.whoAmI);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.Reload(BlendState.Additive);
            Texture2D tex = ModContent.Request<Texture2D>("Regressus/Content/Snow/IceLantern/Hail1").Value;
            Texture2D glow = ModContent.Request<Texture2D>("Regressus/Content/Snow/IceLantern/Hail1_Glow").Value;
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
    public class IceLanternP2 : ModProjectile
    {
        public override string Texture => "Regressus/Content/Snow/IceLantern//Hail2";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hail");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * 0.5f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.Reload(BlendState.Additive);
            Texture2D tex = ModContent.Request<Texture2D>("Regressus/Content/Snow/IceLantern/Hail2").Value;
            Texture2D glow = ModContent.Request<Texture2D>("Regressus/Content/Snow/IceLantern/Hail2_Glow").Value;
            var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
            {
                Main.spriteBatch.Draw(glow, Projectile.oldPos[i] - Main.screenPosition + new Vector2(Projectile.width / 2f, Projectile.height / 2f), new Rectangle(0, 0, glow.Width, glow.Height), Color.LightBlue * (1f - fadeMult * i), Projectile.oldRot[i], glow.Size() / 2, Projectile.scale * (ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type], Projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            }

            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, tex.Width, tex.Height), Color.White, Projectile.rotation, tex.Size() / 2, Projectile.scale, Projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            return false;
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.hostile = false;
            Projectile.scale = 0;
            Projectile.timeLeft = 45 / 2;
            Projectile.tileCollide = false;
        }
        public override void Kill(int timeLeft)
        {
            for (int num613 = 0; num613 < 15; num613++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Ice, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 150, default(Color), 0.8f);
            }
            if (Projectile.aiStyle != 2)
                Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<HailExplosion>(), 0, 1);
        }
        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(5);
            if (Projectile.aiStyle != 1)
                Projectile.velocity *= 1.01f;
            float progress = Utils.GetLerpValue(0, 45, Projectile.timeLeft);
            Projectile.scale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);
        }

        public class HailExplosion : ModProjectile
        {
            public override string Texture => "Regressus/Content/Projectiles/Misc/AzazelP2";
            public override void SetStaticDefaults()
            {
                DisplayName.SetDefault("Explosion");
                Main.projFrames[Projectile.type] = 5;
            }
            public override void SetDefaults()
            {
                Projectile.width = 65;
                Projectile.height = 65;
                Projectile.aiStyle = -1;
                Projectile.friendly = true;
                Projectile.hostile = false;
                Projectile.penetrate = -1;
                Projectile.ignoreWater = true;
                Projectile.tileCollide = false;
                Projectile.timeLeft = 26;
            }
            float AAA;
            public override void OnSpawn(IEntitySource source)
            {
                SoundEngine.PlaySound(SoundID.Item14);
            }
            public override void AI()
            {
                if (Projectile.ai[0] == 0)
                {
                    Projectile.ai[0] = 1;
                    for (int num899 = 0; num899 < 4; num899++)
                    {
                        int num900 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default(Color), 1.5f);
                        Main.dust[num900].position = Projectile.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * Projectile.width / 2f;
                    }
                    for (int num901 = 0; num901 < 10; num901++)
                    {
                        int num902 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.MagicMirror, 0f, 0f, 200, default(Color), 2.7f);
                        Main.dust[num902].position = Projectile.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * Projectile.width / 2f;
                        Main.dust[num902].noGravity = true;
                        Dust dust2 = Main.dust[num902];
                        dust2.velocity *= 3f;
                        num902 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.MagicMirror, 0f, 0f, 100, default(Color), 1.5f);
                        Main.dust[num902].position = Projectile.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * Projectile.width / 2f;
                        dust2 = Main.dust[num902];
                        dust2.velocity *= 2f;
                        Main.dust[num902].noGravity = true;
                        Main.dust[num902].fadeIn = 2.5f;
                    }
                    for (int num903 = 0; num903 < 5; num903++)
                    {
                        int num904 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.MagicMirror, 0f, 0f, 0, default(Color), 2.7f);
                        Main.dust[num904].position = Projectile.Center + Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(Projectile.velocity.ToRotation()) * Projectile.width / 2f;
                        Main.dust[num904].noGravity = true;
                        Dust dust2 = Main.dust[num904];
                        dust2.velocity *= 3f;
                    }
                    for (int num905 = 0; num905 < 10; num905++)
                    {
                        int num906 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 0, default(Color), 1.5f);
                        Main.dust[num906].position = Projectile.Center + Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(Projectile.velocity.ToRotation()) * Projectile.width / 2f;
                        Main.dust[num906].noGravity = true;
                        Dust dust2 = Main.dust[num906];
                        dust2.velocity *= 3f;
                    }
                    Projectile.timeLeft = 25;
                }
                /*if (++Projectile.frameCounter >= 5)
                {
                    Projectile.frameCounter = 0;
                    if (++Projectile.frame >= 5)
                    {
                        Projectile.Kill();
                    }
                }
                Main.NewText(AAA);
                float progress = Utils.GetLerpValue(0, 25, Projectile.timeLeft);
                AAA = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);*/
            }
            public override bool PreDraw(ref Color lightColor) => false;
            /*public override bool PreDraw(ref Color lightColor)
            {
                Texture2D a = RegreUtils.GetTexture("Projectiles/Ranged/AzazelP2");
                RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
                Main.EntitySpriteDraw(a, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * 65, 65, 65), Color.LightBlue * AAA, Projectile.rotation, Projectile.Size / 2, Projectile.scale, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(a, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * 65, 65, 65), Color.White * AAA, Projectile.rotation, Projectile.Size / 2, Projectile.scale, SpriteEffects.None, 0);
                RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
                return false;
            }*/
        }
    }
}
