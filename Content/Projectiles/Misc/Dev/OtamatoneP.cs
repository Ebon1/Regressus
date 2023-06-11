using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent;

namespace Regressus.Projectiles.Dev
{
    public class OtamatoneP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 56;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.netImportant = true;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 vector = player.RotatedRelativePoint(player.MountedCenter);
            float rotOffset = 0;
            if (Projectile.spriteDirection == -1)
            {
                rotOffset = (float)Math.PI;
            }
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }
            Projectile.soundDelay--;
            if (Projectile.soundDelay == 1)
            {
                float shootToX = Main.MouseWorld.X - Projectile.Center.X;
                float shootToY = Main.MouseWorld.Y - Projectile.Center.Y;
                float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
                distance = 3f / distance;
                shootToX *= distance * 5;
                shootToY *= distance * 5;
                int proj = Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center.X, Projectile.Center.Y, shootToX, shootToY, ModContent.ProjectileType<OtamatoneP2>(), Projectile.damage - 20, Projectile.knockBack, Main.myPlayer, 0, 0);
                Main.projectile[proj].timeLeft = 300;
                Main.projectile[proj].netUpdate = true;
                Projectile.netUpdate = true;
            }
            if (Projectile.soundDelay <= 0)
            {
                switch (Main.rand.Next(1, 5))
                {
                    case 1:
                        SoundEngine.PlaySound(new SoundStyle("Regressus/Sounds/Custom/Otamatone"));
                        break;
                    case 2:
                        SoundEngine.PlaySound(new SoundStyle("Regressus/Sounds/Custom/Otamatone2"));
                        break;
                    case 3:
                        SoundEngine.PlaySound(new SoundStyle("Regressus/Sounds/Custom/Otamatone3"));
                        break;
                    case 4:
                        SoundEngine.PlaySound(new SoundStyle("Regressus/Sounds/Custom/Otamatone4"));
                        break;
                }
                Projectile.soundDelay = 45;
            }
            if (Main.myPlayer == Projectile.owner)
            {
                Item heldItem = player.inventory[player.selectedItem];
                if (player.channel && !player.noItems && !player.CCed)
                {
                    float num27 = 1f;
                    if (heldItem.shoot == Projectile.type)
                    {
                        num27 = heldItem.shootSpeed * Projectile.scale;
                    }
                    Vector2 vector16 = Main.MouseWorld - vector;
                    vector16.Normalize();
                    if (vector16.HasNaNs())
                    {
                        vector16 = Vector2.UnitX * player.direction;
                    }
                    vector16 *= num27;
                    if (vector16.X != Projectile.velocity.X || vector16.Y != Projectile.velocity.Y)
                    {
                        Projectile.netUpdate = true;
                    }
                    Projectile.velocity = vector16;
                }
                else
                {
                    Projectile.Kill();
                }
            }
            Vector2 vector17 = Projectile.position + Projectile.velocity * 3f;
            Projectile.Center = player.Center;
            Projectile.rotation = Projectile.velocity.ToRotation() + rotOffset;
            Projectile.spriteDirection = Projectile.direction;
            Projectile.timeLeft = 2;
            player.ChangeDir(Projectile.direction);
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = (float)Math.Atan2(Projectile.velocity.Y * Projectile.direction, Projectile.velocity.X * Projectile.direction);
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            int size = 10;
            Vector2 vector = Projectile.Center + Projectile.velocity;
            hitbox = new Rectangle((int)vector.X - size, (int)vector.Y - size, size * 2, size * 2);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * ((255 - Projectile.alpha) / 255f);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (projHitbox.Intersects(targetHitbox))
            {
                return true;
            }
            float _ = float.NaN;
            Vector2 beamEndPos = Projectile.Center + Projectile.velocity;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, beamEndPos, 22 * Projectile.scale, ref _);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            int textureHeight = texture.Height / Main.projFrames[Projectile.type];
            Player player = Main.player[Projectile.owner];
            Vector2 vector = Projectile.Center + new Vector2(0, 8f);
            SpriteEffects effects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Main.EntitySpriteDraw(texture, vector - Main.screenPosition, new Rectangle(0, Projectile.frame * textureHeight, texture.Width, textureHeight), lightColor, Projectile.rotation, new Vector2(texture.Width / 2f, textureHeight / 2f), Projectile.scale, effects, 0);
            return false;
        }
    }
}