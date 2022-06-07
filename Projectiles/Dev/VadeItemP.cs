using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Items.Dev;
using Regressus.Projectiles.Melee;
using Terraria.GameContent;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace Regressus.Projectiles.Dev
{
    public class VadeItemP : HeldSword
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tyrfing");
        }
        public override void SetDefaults()
        {
            SwingTime = 20;
            holdOffset = 42.5f;
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = SwingTime;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 7;
        }
        public override float Lerp(float x)
        {
            return (float)(x < 0.5 ? 16 * x * x * x * x * x : 1 - Math.Pow(-2 * x + 2, 5) / 2);
        }
        public override void PostDraw(Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation + (Projectile.ai[0] == -1 ? 0 : MathHelper.PiOver2 * 3), texture.Size() / 2, Projectile.scale, Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D glow = ModContent.Request<Texture2D>("Regressus/Projectiles/Dev/VadeItemP_Glow").Value;
            Player player = Main.player[Projectile.owner];
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            Texture2D slash = ModContent.Request<Texture2D>("Regressus/Extras/Extras2/slash_02").Value;
            float mult = Lerp(Utils.GetLerpValue(0f, SwingTime, Projectile.timeLeft));
            float alpha = MathHelper.Clamp((float)Math.Sin(mult * Math.PI) * 3f, 0, 1);
            Vector2 pos = player.Center + Projectile.velocity * ((holdOffset * 0.75f) - mult * (holdOffset * 0.75f));
            Main.spriteBatch.Draw(slash, pos - Main.screenPosition, null, Main.DiscoColor * alpha, Projectile.velocity.ToRotation() - MathHelper.PiOver2, slash.Size() / 2, Projectile.scale / 1.95f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(glow, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, glow.Width, glow.Height), Main.DiscoColor, Projectile.rotation + (Projectile.ai[0] == -1 ? 0 : MathHelper.PiOver2 * 3), glow.Size() / 2, Projectile.scale * 1.05f, Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            return false;
            /*Projectile.localAI[0]++;
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            Texture2D glow = ModContent.Request<Texture2D>("Regressus/Projectiles/Melee/Foreshadow_Glow").Value;
            var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
            {
                if (i == Projectile.localAI[0])
                    continue;
                Main.spriteBatch.Draw(glow, Projectile.oldPos[i] - Main.screenPosition + new Vector2(Projectile.width / 2f, Projectile.height / 2f), new Rectangle(0, 0, glow.Width, glow.Height), Color.DarkViolet * (1f - fadeMult * i), Projectile.oldRot[i], glow.Size() / 2, Projectile.scale * (ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type], Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            }

            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            return false;*/
        }
    }
    public class VadeItemP2 : ModProjectile
    {
        public override string Texture => "Regressus/Projectiles/Dev/VadeItemP";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tyrfing");
        }
        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 500;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.scale = 1.25f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 7;
        }
        public override void AI()
        {
            Projectile.velocity *= 1.05f;
            Projectile.rotation += MathHelper.ToRadians(10f);
        }
        public override void PostDraw(Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White * 0.5f, Projectile.rotation, texture.Size() / 2, Projectile.scale, Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D glow = ModContent.Request<Texture2D>("Regressus/Projectiles/Dev/VadeItemP_Glow").Value;
            Player player = Main.player[Projectile.owner];
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            Texture2D slash = ModContent.Request<Texture2D>("Regressus/Extras/Extras2/slash_02").Value;
            //float mult = Lerp(Utils.GetLerpValue(0f, 500, Projectile.timeLeft));
            //float alpha = MathHelper.Clamp((float)Math.Sin(mult * Math.PI) * 3f, 0, 1);
            //Vector2 pos = player.Center + Projectile.velocity * ((holdOffset * 0.75f) - mult * (holdOffset * 0.75f));
            //Main.spriteBatch.Draw(slash, pos - Main.screenPosition, null, Main.DiscoColor * alpha, Projectile.velocity.ToRotation() - MathHelper.PiOver2, slash.Size() / 2, Projectile.scale / 1.95f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(glow, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, glow.Width, glow.Height), Main.DiscoColor, Projectile.rotation, glow.Size() / 2, Projectile.scale * 1.05f, Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            return false;
            /*Projectile.localAI[0]++;
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            Texture2D glow = ModContent.Request<Texture2D>("Regressus/Projectiles/Melee/Foreshadow_Glow").Value;
            var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
            {
                if (i == Projectile.localAI[0])
                    continue;
                Main.spriteBatch.Draw(glow, Projectile.oldPos[i] - Main.screenPosition + new Vector2(Projectile.width / 2f, Projectile.height / 2f), new Rectangle(0, 0, glow.Width, glow.Height), Color.DarkViolet * (1f - fadeMult * i), Projectile.oldRot[i], glow.Size() / 2, Projectile.scale * (ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type], Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            }

            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            return false;*/
        }
    }
    public class VadeItemP3 : ModProjectile
    {
        public override string Texture => "Regressus/Items/Dev/VadeItem";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tyrfing");
            ProjectileID.Sets.MinionTargettingFeature[Type] = true;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 25;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 500;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.scale = 1.15f;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minionSlots = 1f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 7;
        }
        List<int> projectiles = new List<int>();
        float _angle;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!CheckActive(player))
            {
                return;
            }
            SearchForTargets(player, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter);
            if (foundTarget)
            {
                Projectile.rotation += MathHelper.ToRadians(10f);
                Projectile.velocity = (targetCenter - Projectile.Center) * 0.045f;
            }
            else if (!foundTarget || player.Center.Distance(Projectile.Center) > 1500)
            {
                Projectile.rotation = _angle;
                _angle += 2f * (float)Math.PI / 600f * 10;
                _angle %= 2f * (float)Math.PI;
                Projectile.velocity = (player.Center + 200 * new Vector2((float)Math.Cos(_angle), (float)Math.Sin(_angle)) - Projectile.Center) * 0.085f;
            }
            Projectile.direction = 1;
        }
        private bool CheckActive(Player owner)
        {
            if (owner.dead || !owner.active)
            {
                owner.ClearBuff(ModContent.BuffType<Buffs.Minions.Tyrfing>());

                return false;
            }

            if (owner.HasBuff(ModContent.BuffType<Buffs.Minions.Tyrfing>()))
            {
                Projectile.timeLeft = 2;
            }

            return true;
        }

        void SearchForTargets(Player owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter)
        {
            distanceFromTarget = 700f;
            targetCenter = Projectile.position;
            foundTarget = false;

            if (owner.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[owner.MinionAttackTargetNPC];
                float between = Vector2.Distance(npc.Center, Projectile.Center);

                if (between < 2000f)
                {
                    distanceFromTarget = between;
                    targetCenter = npc.Center;
                    foundTarget = true;
                }
            }

            if (!foundTarget)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];

                    if (npc.CanBeChasedBy())
                    {
                        float between = Vector2.Distance(npc.Center, Projectile.Center);
                        bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
                        bool inRange = between < distanceFromTarget;
                        bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
                        bool closeThroughWall = between < 100f;

                        if (((closest && inRange) || !foundTarget) && (lineOfSight || closeThroughWall))
                        {
                            distanceFromTarget = between;
                            targetCenter = npc.Center;
                            foundTarget = true;
                        }
                    }
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D glow = ModContent.Request<Texture2D>("Regressus/Projectiles/Dev/VadeItemP_Glow").Value;
            Player player = Main.player[Projectile.owner];
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
            {
                Main.spriteBatch.Draw(glow, Projectile.oldPos[i] - Main.screenPosition + new Vector2(Projectile.width / 2f, Projectile.height / 2f), new Rectangle(0, 0, glow.Width, glow.Height), Main.DiscoColor * (1f - fadeMult * i), Projectile.oldRot[i], glow.Size() / 2, Projectile.scale * (ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type], SpriteEffects.None, 0f);
            }

            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}
