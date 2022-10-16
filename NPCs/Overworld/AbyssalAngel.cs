using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Regressus.NPCs;
using ReLogic.Content;
using Terraria.GameContent.Bestiary;
using Regressus.Projectiles.Enemy.Overworld;

using System.Security.Cryptography.X509Certificates;
using Regressus.Projectiles.Minibosses.Vagrant;

namespace Regressus.NPCs.Overworld
{
    public class AbyssalAngel : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 9;
        }
        public override void SetDefaults()
        {
            NPC.width = 116;
            NPC.height = 88;
            NPC.lifeMax = 100;
            NPC.defense = 3;
            NPC.damage = 0;
            NPC.knockBackResist = 0.5f;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.aiStyle = -1;
            NPC.noGravity = NPC.noTileCollide = true;
            NPC.lavaImmune = true;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneOverworldHeight && !Main.dayTime)
                return 0.02f;
            return 0;
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter >= 5)
            {
                if (NPC.frame.Y >= 8 * frameHeight)
                    NPC.frame.Y = 0;
                NPC.frame.Y += frameHeight;
                NPC.frameCounter = 0;
            }
        }
        public float AIState
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }
        public float AITimer
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
        }
        const int Idle = 0, Attack = 1;
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Main.EntitySpriteDraw(RegreUtils.GetTexture("NPCs/Overworld/AbyssalAngel_Glow"), NPC.Center - screenPos, NPC.frame, Color.White, NPC.rotation, NPC.Size / 2, NPC.scale, SpriteEffects.None, 0);
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    Dust.NewDust(NPC.Center, 32, 32, 109, Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1));
                }
                for (int i = 0; i < 3; i++)
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, Vector2.UnitY, ModContent.GoreType<AngelGore>());
            }
        }
        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            if (AIState == Idle)
            {
                AITimer++;
                NPC.velocity = RegreUtils.FromAToB(NPC.Center, player.Center - Vector2.UnitY * 100f, false) * 0.018f;
                NPC.rotation = NPC.velocity.X * 0.05f;
                if (AITimer >= 245)
                {
                    AITimer = 0;
                    NPC.velocity = Vector2.Zero;
                    AIState = Attack;
                }
            }
            else if (AIState == Attack)
            {
                AITimer++;
                if (AITimer == 1)
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<AngelP2>(), 0, 0f, player.whoAmI);
                if (AITimer >= 150)
                {
                    AITimer = 0;
                    AIState = Idle;
                }
            }
        }
    }
    public class AngelP : ModProjectile
    {
        public override string Texture => "Regressus/Projectiles/Dev/PokerfaceP";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 25;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        float alpha = 1f;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            alpha -= 0.05f;
            return alpha <= 0f;
        }
        public override void SetDefaults()
        {
            Projectile.height = 3;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 3;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 500;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.aiStyle = -1;
            Projectile.scale = 0.75f;
        }
        Vector2 baseCenter, baseCenter2;
        bool runOnce;
        public override void AI()
        {
            if (!runOnce)
            {
                runOnce = true;
                baseCenter = Projectile.Center;
                baseCenter2 = Main.player[Projectile.owner].Center;
            }
            if (alpha != 1f)
                alpha -= 0.05f;
            if (alpha <= 0f)
                Projectile.Kill();
            Projectile.ai[1]++;
            if (Projectile.ai[1] < 25)
            {
                Projectile.velocity.X += 0.2f * Projectile.ai[0];
            }
            if (Projectile.ai[1] > 25)
                if (baseCenter.Distance(baseCenter2) < 300f)
                    Projectile.velocity.X -= 0.2f * Projectile.ai[0];
                else
                    Projectile.velocity.X += 0.2f * Projectile.ai[0];
            if (Projectile.ai[1] >= 45 && Projectile.ai[1] < 55)
                Projectile.velocity.Y *= 0.98f;
            if (Projectile.ai[1] == 55)
            {
                Projectile.velocity.Y = 20f;
                Projectile.velocity.X = 0f;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            Texture2D glow = ModContent.Request<Texture2D>("Regressus/Extras/glow2").Value;
            var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
            for (int j = 0; j < 2; j++)
                for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
                {
                    Main.spriteBatch.Draw(glow, Projectile.oldPos[i] - Main.screenPosition + new Vector2(Projectile.width / 2f, Projectile.height / 2f), new Rectangle(0, 0, glow.Width, glow.Height), Color.White * alpha * (1f - fadeMult * i), Projectile.oldRot[i], glow.Size() / 2, Projectile.scale * .25f * (ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type], Projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
                }

            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            return false;
        }
    }
    public class AngelP2 : ModProjectile
    {
        public override string Texture => "Regressus/Projectiles/Dev/PokerfaceP";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 25;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.height = 10;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 10;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 80;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.aiStyle = -1;
        }
        public override void AI()
        {
            float prog = Utils.GetLerpValue(0, 80, Projectile.timeLeft);
            Projectile.ai[1] = MathHelper.Clamp((float)Math.Sin(prog * Math.PI) * 3, 0, 1);
            Projectile.scale += 0.05f;
            if (Projectile.timeLeft == 10)
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item9, Projectile.Center);
                for (int i = -2; i <= 2; i++)
                {
                    Vector2 randomVel = new(i * 3, -7.5f);
                    Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, randomVel, ModContent.ProjectileType<AngelP>(), 10, 0, Projectile.owner, ai0: i);
                    proj.ai[0] = i;
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            Texture2D glow = ModContent.Request<Texture2D>("Regressus/Extras/glow2").Value;
            var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
            for (int j = 0; j < 2; j++)
                for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
                {
                    Main.spriteBatch.Draw(glow, Projectile.oldPos[i] - Main.screenPosition + new Vector2(Projectile.width / 2f, Projectile.height / 2f), new Rectangle(0, 0, glow.Width, glow.Height), Color.White * Projectile.ai[1] * (1f - fadeMult * i), Projectile.oldRot[i], glow.Size() / 2, Projectile.scale * .25f * (ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type], Projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
                }

            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            return false;
        }
    }
}
