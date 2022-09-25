using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Projectiles.Dev;
using Terraria.DataStructures;
using Terraria.GameContent;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.ObjectModel;
using Terraria.UI.Chat;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using ReLogic.Graphics;
using Terraria.Chat;
using Terraria.Audio;
using Terraria.GameContent.UI.Chat;

namespace Regressus.Projectiles.Sentry
{
    public class PvZ : ModProjectile
    {
        public override string Texture => "Regressus/Projectiles/Sentry/PvZBase";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Peashooter");
        }
        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(32, 28);
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.aiStyle = 1;
            Projectile.timeLeft = 20 * 60;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;
            return true;
        }
        public override void PostDraw(Color lightColor)
        {
            Texture2D tex = RegreUtils.GetTexture("Projectiles/Sentry/PvZHead" + (Tier + 1));
            Main.EntitySpriteDraw(tex, Projectile.Top - Main.screenPosition, tex.Frame(), lightColor, rot, tex.Size() / 2, 1, dir == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public int Tier;
        float rot;
        int dir = 1;
        int Counter;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 vel = RegreUtils.FromAToB(Projectile.Top, Main.MouseWorld) * 10;
            SoundStyle a = new SoundStyle("Regressus/Sounds/Custom/PvZShoot");
            if (Tier == 0)
            {
                if (++Projectile.ai[1] >= 60)
                {
                    SoundEngine.PlaySound(a, Projectile.Center);
                    Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Top, vel, ModContent.ProjectileType<PvZP>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    Projectile.ai[1] = 0;
                }
            }
            else if (Tier == 1)
            {
                Projectile.ai[1]++;
                if (Projectile.ai[1] == 20)
                {
                    SoundEngine.PlaySound(a, Projectile.Center);
                    Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Top, vel, ModContent.ProjectileType<PvZP>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                }
                if (Projectile.ai[1] >= 40)
                {
                    SoundEngine.PlaySound(a, Projectile.Center);
                    Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Top, vel, ModContent.ProjectileType<PvZP>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    Projectile.ai[1] = 0;
                }
            }
            else if (Tier == 2)
            {
                if (++Projectile.ai[1] >= 10)
                {
                    SoundEngine.PlaySound(a, Projectile.Center);
                    Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Top, vel, ModContent.ProjectileType<PvZP>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    Projectile.ai[1] = 0;
                }
            }



            rot = RegreUtils.FromAToB(Projectile.Top, Main.MouseWorld).ToRotation();
            dir = RegreUtils.FromAToB(Projectile.Top, Main.MouseWorld).X > 0 ? 1 : -1;
            if (dir == -1)
                rot += MathHelper.Pi;

            if (Counter >= 1)
                Counter--;
            Projectile.rotation = 0;
            if (Main.mouseRight && Counter == 0)
            {
                if (Tier != 2)
                {
                    if (Projectile.getRect().Intersects(new Rectangle((int)Main.MouseWorld.X, (int)Main.MouseWorld.Y, 5, 5)))
                    {
                        if (player.HasItem(ItemID.FallenStar) && player.ConsumeItem(ItemID.FallenStar))
                        {
                            Projectile.timeLeft = 20 * 60;
                            Counter = 120;
                            Projectile.ai[1] = 0;
                            Tier++;
                        }
                    }
                }
            }
        }
    }
    public class PvZP : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(5, 5);
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.aiStyle = 1;
            Projectile.timeLeft = 500;
            Projectile.tileCollide = true;
        }
        public override void Kill(int timeLeft)
        {
            SoundStyle a = new SoundStyle("Regressus/Sounds/Custom/PvZHit");
            SoundEngine.PlaySound(a, Projectile.Center);

            Color newColor7 = Color.White;
            for (int num613 = 0; num613 < 7; num613++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Grass, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 150, default(Color), 0.8f);
            }
            for (float num614 = 0f; num614 < 1f; num614 += 0.125f)
            {
                Dust.NewDustPerfect(Projectile.Center, DustID.Grass, Vector2.UnitY.RotatedBy(num614 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (4f + Main.rand.NextFloat() * 4f), 150, newColor7).noGravity = true;
            }
            for (float num615 = 0f; num615 < 1f; num615 += 0.25f)
            {
                Dust.NewDustPerfect(Projectile.Center, DustID.Grass, Vector2.UnitY.RotatedBy(num615 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (2f + Main.rand.NextFloat() * 3f), 150, Color.Gold).noGravity = true;
            }
        }
    }
}
