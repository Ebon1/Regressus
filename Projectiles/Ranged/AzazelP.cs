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
using Terraria.Audio;

namespace Regressus.Projectiles.Ranged
{
    public class AzazelP : ModProjectile
    {
        public override string Texture => "Regressus/Items/Weapons/Ranged/AzazelsDagger";
        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(50, 50);
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.timeLeft = 500;
            Projectile.aiStyle = 1;
        }
        bool runOnce;
        float _rot;
        bool canFlash, collided;
        NPC npc;
        Vector2 pos;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            npc = target;
            _rot = Projectile.rotation;
            Projectile.timeLeft = 90;
            canFlash = true;
            Projectile.velocity = (target.Center - Projectile.Center) * 0.75f;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            collided = true;
            _rot = Projectile.rotation;
            pos = Projectile.Center;
            Projectile.velocity *= 0.75f;
            return false;
        }
        public override void Kill(int timeLeft)
        {
            if (npc != null)
            {
                Vector2 pos = npc.type != ModContent.NPCType<NPCs.Bosses.Oracle.TheOracle>() && (npc.Size.X > 50 || npc.Size.Y > 50) ? Projectile.Center : npc.Center;
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), pos, Vector2.Zero, ModContent.ProjectileType<AzazelP2>(), Projectile.damage * 2, Projectile.knockBack, Projectile.owner);
            }
        }
        public override void AI()
        {
            if (!canFlash && !collided)
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            else
                Projectile.rotation = _rot;
            if (npc != null)
            {
                if (npc.active && !npc.dontTakeDamage)
                {
                    Projectile.Center = npc.Center - Projectile.velocity * 2f;
                    Projectile.gfxOffY = npc.gfxOffY;
                    Projectile.ai[1] += 0.025f;
                    if (Projectile.ai[1] >= 1.5f)
                    {
                        Projectile.Kill();
                    }
                }
                else
                {
                    Projectile.Kill();
                }
            }
            if (collided)
            {
                Projectile.timeLeft -= 10;
            }
        }
        public override void PostDraw(Color lightColor)
        {
            if (canFlash && !collided)
            {
                Texture2D a = RegreUtils.GetTexture("Projectiles/Ranged/AzazelP_Glow");
                RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
                Main.EntitySpriteDraw(a, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 60, 60), Color.Orange * MathHelper.Clamp(2 * Projectile.ai[1], 0, 1), Projectile.rotation, a.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(a, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 60, 60), Color.White * Projectile.ai[1], Projectile.rotation, a.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
                RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            }
        }
    }
    public class AzazelP2 : ModProjectile
    {

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
        public override void AI()
        {
            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 5)
                {
                    Projectile.Kill();
                }
            }
            float progress = Utils.GetLerpValue(0, 26, Projectile.timeLeft);
            Projectile.ai[1] = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 5, 0, 1);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D a = RegreUtils.GetTexture("Projectiles/Ranged/AzazelP2");
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            Main.EntitySpriteDraw(a, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * 65, 65, 65), Color.Orange * MathHelper.Clamp(2 * Projectile.ai[1], 0, 1), Projectile.rotation, Projectile.Size / 2, Projectile.scale, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(a, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * 65, 65, 65), Color.White * Projectile.ai[1], Projectile.rotation, Projectile.Size / 2, Projectile.scale, SpriteEffects.None, 0);
            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            return false;
        }
    }
}
