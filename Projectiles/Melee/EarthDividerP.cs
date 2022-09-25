using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Items.Weapons.Melee;
using Terraria.GameContent;

namespace Regressus.Projectiles.Melee
{
    public class EarthDividerP : ModProjectile
    {
        public override string Texture => "Regressus/Items/Weapons/Melee/EarthDivider";
        public override bool ShouldUpdatePosition() => Projectile.ai[1] == 4;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Earth Divider");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        int maxTime = 25;
        public override void SetDefaults()
        {
            if (Projectile.ai[1] == 4)
                maxTime = 250;
            else
                maxTime = 25;
            Projectile.timeLeft = maxTime; ;
            Projectile.width = Projectile.height = 82;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }
        bool runOnce;
        float _rot;
        bool canFlash;
        public Vector2 flashPos, firstPos, playerPos;
        NPC npc;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!canFlash && Projectile.ai[1] == 4)
            {
                npc = target;
                _rot = Projectile.rotation;
                Projectile.timeLeft = 90;
                canFlash = true;
                Projectile.velocity = (target.Center - Projectile.Center) * 0.75f;
                flashPos = Projectile.Center;
            }
        }
        /*public override bool OnTileCollide(Vector2 oldVelocity)
        {
            _rot = Projectile.rotation;
            Projectile.timeLeft = 50;
            collided = true;
            canFlash = true;
            Projectile.velocity *= 0.5f;
            flashPos = Projectile.Center;
            return false;
        }*/
        public float Lerp(float x)
        {
            return x < 0.5f ? 8 * x * x * x * x : 1 - (float)Math.Pow(-2 * x + 2, 4) / 2;
        }
        float scale;
        public float ScaleFunction(float progress)
        {
            return 0.7f + (float)Math.Sin(progress * Math.PI) * 0.5f;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;
            if (!player.active || player.dead || player.CCed || player.noItems)
            {
                Projectile.Kill();
            }
            if (!runOnce)
            {
                playerPos = player.Center;
                firstPos = Main.MouseWorld;
                Projectile.localAI[0] = -1;
                if (Projectile.ai[1] == 4)
                {
                    player.itemTime = 25;
                    player.itemAnimation = 25;
                    maxTime = 230;
                }
                else
                    maxTime = 25;
                Projectile.timeLeft = maxTime;
                runOnce = true;
            }
            float offset = 55;
            if (Projectile.ai[1] == 4)
            {
                scale = Projectile.scale;
                offset = 70;
                Projectile.aiStyle = 0;
                //Projectile.tileCollide = Projectile.position.Y >= playerPos.Y;
                if (!canFlash)
                {
                    Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
                    if (Projectile.position.Y <= playerPos.Y)
                        Projectile.velocity *= 1.075f;
                }
                else
                {
                    Projectile.rotation = _rot;
                    if (npc != null)
                    {
                        if (npc.active && !npc.dontTakeDamage)
                        {
                            Projectile.Center = npc.Center - Projectile.velocity * 2f;
                            Projectile.gfxOffY = npc.gfxOffY;
                        }
                        else
                        {
                            Projectile.Kill();
                        }
                    }
                    else
                    {
                        Projectile.velocity = Vector2.Zero;
                        Projectile.Center = flashPos;
                    }
                }
            }
            else
            {
                int direction = (int)Projectile.ai[0];
                float swingProgress = Lerp(Utils.GetLerpValue(0f, maxTime, Projectile.timeLeft));
                float defRot = Projectile.velocity.ToRotation();
                float start = defRot - (MathHelper.PiOver2 + MathHelper.PiOver4);
                float end = defRot + (MathHelper.PiOver2 + MathHelper.PiOver4);
                float rotation = direction == 1 ? start + MathHelper.Pi * 3 / 2 * swingProgress : end - MathHelper.Pi * 3 / 2 * swingProgress;
                Vector2 position = player.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, rotation - MathHelper.PiOver2) +
                    rotation.ToRotationVector2() * offset * ScaleFunction(swingProgress);
                Projectile.Center = position;
                Projectile.rotation = (position - player.Center).ToRotation() + MathHelper.PiOver4;

                player.ChangeDir(Projectile.velocity.X < 0 ? -1 : 1);
                player.heldProj = Projectile.whoAmI;
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, rotation - MathHelper.PiOver2);
                player.itemTime = 2;
                player.itemAnimation = 2;
            }
        }
        public override void PostDraw(Color lightColor)
        {
            if (canFlash)
            {
                RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
                float progress = Utils.GetLerpValue(0f, 50, Projectile.timeLeft);
                Texture2D glow = ModContent.Request<Texture2D>("Regressus/Extras/crosslight").Value;
                float mult = (0.55f + (float)Math.Sin(Main.GlobalTimeWrappedHourly/* * 2*/) * 0.3f);
                for (int i = 0; i < 2; i++)
                    Main.spriteBatch.Draw(glow, flashPos - Main.screenPosition, null, Color.White * Math.Clamp(progress * 2.5f, 0, maxTime), Main.GameUpdateCount * 0.0025f, glow.Size() / 2, 1 * mult, Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f); ;
                RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            }
            if (Projectile.ai[1] == 4)
            {
                RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
                float progress = Utils.GetLerpValue(0f, 30, Projectile.timeLeft - 200);
                Texture2D glow = ModContent.Request<Texture2D>("Regressus/Extras/crosslight").Value;
                float mult = (0.55f + (float)Math.Sin(Main.GlobalTimeWrappedHourly/* * 2*/) * 0.3f);
                Main.spriteBatch.Draw(glow, firstPos - Main.screenPosition, null, Color.White * Math.Clamp(progress * 2.5f, 0, maxTime), Main.GameUpdateCount * 0.0025f, glow.Size() / 2, 1 * mult, Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f); ;
                RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.localAI[0]++;
            if (Projectile.localAI[0] > 0)
            {
                Texture2D glow = ModContent.Request<Texture2D>("Regressus/Projectiles/Melee/EarthDivider_Glow").Value;
                Player player = Main.player[Projectile.owner];
                RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
                if (Projectile.ai[1] != 4)
                {
                    Texture2D slash = ModContent.Request<Texture2D>("Regressus/Extras/Extras2/slash_02").Value;
                    float mult = Lerp(Utils.GetLerpValue(0f, maxTime, Projectile.timeLeft));
                    float alpha = (float)Math.Sin(mult * Math.PI);
                    Vector2 pos = player.Center + Projectile.velocity * (35f - mult * 35f);
                    Main.spriteBatch.Draw(slash, pos - Main.screenPosition, null, new Color(0, 255, Main.DiscoB) * alpha, Projectile.velocity.ToRotation() - MathHelper.PiOver2, slash.Size() / 2, Projectile.scale / 1.95f, SpriteEffects.None, 0f);
                    Main.spriteBatch.Draw(glow, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, glow.Width, glow.Height), new Color(0, 255, Main.DiscoB), Projectile.rotation + (Projectile.ai[0] == -1 ? 0 : MathHelper.PiOver2 * 3), glow.Size() / 2, Projectile.scale * 1.05f, Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
                }
                else
                {
                    var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
                    for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
                    {
                        if (i == Projectile.localAI[0])
                            continue;
                        Main.spriteBatch.Draw(glow, Projectile.oldPos[i] - Main.screenPosition + new Vector2(Projectile.width / 2f, Projectile.height / 2f), new Rectangle(0, 0, glow.Width, glow.Height), new Color(0, 255, Main.DiscoB) * (1f - fadeMult * i), Projectile.oldRot[i] + (Projectile.ai[0] == -1 ? 0 : MathHelper.PiOver2 * 3), glow.Size() / 2, Projectile.scale * 0.75f/* * (ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type]*/, Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
                    }
                }

                RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
                Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation + (Projectile.ai[0] == -1 ? 0 : MathHelper.PiOver2 * 3), texture.Size() / 2, Projectile.scale, Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            }
            return false;
        }
    }
}
