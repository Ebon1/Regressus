using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;
using Regressus.Items.Ammo;
using Terraria.GameContent.Bestiary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Audio;
using Regressus.Dusts;
using System;
using Terraria.GameContent;
using System.Linq;
using XPT.Core.Audio.MP3Sharp.Decoding;

namespace Regressus.NPCs.Overworld
{
    public class Hyperlad : ModNPC
    {



        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
        }
        public override void SetDefaults()
        {
            NPC.height = 28;
            NPC.width = 34;
            NPC.damage = 0;
            NPC.friendly = false;
            NPC.lifeMax = 100;
            NPC.defense = 2;
            NPC.aiStyle = -1;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
        }
        public override void FindFrame(int frameHeight)
        {

            NPC.frameCounter++;
            if (NPC.frameCounter < 5)
            {
                NPC.frame.Y = 0 * frameHeight;
            }
            else if (NPC.frameCounter < 10)
            {
                NPC.frame.Y = 1 * frameHeight;
            }
            else if (NPC.frameCounter < 15)
            {
                NPC.frame.Y = 2 * frameHeight;
            }
            else if (NPC.frameCounter < 20)
            {
                NPC.frame.Y = 3 * frameHeight;
            }
            else
            {
                NPC.frameCounter = 0;
            }
        }
        public override Color? GetAlpha(Color drawColor)
        {
            return Color.White;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D a = RegreUtils.GetExtraTexture("star_07");
            spriteBatch.Draw(a, NPC.Center - Main.screenPosition + new Vector2(4, 10), null, Color.Red * NPC.ai[1], Main.GameUpdateCount * 0.003f, a.Size() / 2, 0.1f, SpriteEffects.None, 0);
            spriteBatch.Draw(a, NPC.Center - Main.screenPosition + new Vector2(-4, 10), null, Color.Red * NPC.ai[1], Main.GameUpdateCount * 0.003f, a.Size() / 2, 0.1f, SpriteEffects.None, 0);
        }
        /*OK
UH
SPAWNS UNDERGROUND
IDLIN
WHEN IT SEES PLAYER
GETS A MEME LENS FLARE IN ITS EYE
JUMPS
SPINS IN AIR
TURNS INTO A BOUNCING HYPER SPEED COMET*/
        public override void OnSpawn(IEntitySource source)
        {
            NPC.direction = Main.rand.Next(new int[] { -1, 1 });
        }
        public override void AI()
        {
            Player player = Main.player[NPC.target];
            NPC.TargetClosest(false);
            if (NPC.ai[0] == 0)
            {
                if (Main.rand.NextBool(100))
                    NPC.direction = -NPC.direction;
                NPC.spriteDirection = NPC.direction;
                NPC.velocity.X = 7 * NPC.direction;
                if (NPC.Center.Distance(player.Center) < 300)
                {
                    NPC.ai[0] = 1;
                    NPC.ai[1] = 1;
                    NPC.velocity = Vector2.Zero;
                }
            }
            else
            {
                NPC.ai[1] -= .025f;
                if (NPC.ai[1] <= 0)
                {
                    NPC.life = 0;
                    NPC.checkDead();
                    Projectile.NewProjectile(NPC.GetSource_Death(), NPC.Center, new Vector2(RegreUtils.FromAToB(NPC.Center, player.Center).X * 5, -10), ModContent.ProjectileType<HyperComet>(), 10, 0);
                    Projectile.NewProjectile(NPC.GetSource_Death(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<HyperSHockwave>(), 10, 0);
                    Color newColor7 = Color.CornflowerBlue;
                    for (int num613 = 0; num613 < 7; num613++)
                    {
                        Dust.NewDust(NPC.position, NPC.width, NPC.height, 58, NPC.velocity.X * 0.1f, NPC.velocity.Y * 0.1f, 150, default, 0.8f);
                    }
                    for (float num614 = 0f; num614 < 1f; num614 += 0.125f)
                    {
                        Dust.NewDustPerfect(NPC.Center, 278, Vector2.UnitY.RotatedBy(num614 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (4f + Main.rand.NextFloat() * 4f), 150, newColor7).noGravity = true;
                    }
                    for (float num615 = 0f; num615 < 1f; num615 += 0.25f)
                    {
                        Dust.NewDustPerfect(NPC.Center, 278, Vector2.UnitY.RotatedBy(num615 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (2f + Main.rand.NextFloat() * 3f), 150, Color.Gold).noGravity = true;
                    }
                    Vector2 vector52 = new Vector2(Main.screenWidth, Main.screenHeight);
                    if (NPC.Hitbox.Intersects(Utils.CenteredRectangle(Main.screenPosition + vector52 / 2f, vector52 + new Vector2(400f))))
                    {
                        for (int num616 = 0; num616 < 7; num616++)
                        {
                            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, Main.rand.NextVector2CircularEdge(0.5f, 0.5f) * NPC.velocity.Length(), Utils.SelectRandom(Main.rand, 16, 17, 17, 17, 17, 17, 17, 17));
                        }
                    }
                }
            }
        }
    }
    public class HyperComet : ModProjectile
    {
        public override string Texture => RegreUtils.Empty;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 50;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D a = RegreUtils.GetExtraTexture("explosion");
            Main.spriteBatch.Reload(BlendState.Additive);
            var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Main.spriteBatch.Draw(a, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition, null, Color.Purple * (1f - fadeMult * i), 0, a.Size() / 2, 0.1f * (1f - fadeMult * i), SpriteEffects.None, 0);
                Main.spriteBatch.Draw(a, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition, null, Color.White * (1f - fadeMult * i), 0, a.Size() / 2, 0.09f * (1f - fadeMult * i), SpriteEffects.None, 0);
            }
            Main.spriteBatch.Draw(a, Projectile.Center - Main.screenPosition, null, Color.Purple, 0, a.Size() / 2, 0.1f, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(a, Projectile.Center - Main.screenPosition, null, Color.White, 0, a.Size() / 2, 0.09f, SpriteEffects.None, 0);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }
        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.tileCollide = true;
            Projectile.aiStyle = 14;
            AIType = ProjectileID.BouncyGlowstick;
            Projectile.hostile = true;
            Projectile.timeLeft = 400;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.timeLeft > 60)
                Projectile.velocity.Y = Main.rand.NextFloat(-10, -4);
            else
                Projectile.velocity = Vector2.Zero;
            return false;
        }
        int dir;
        public override void Kill(int timeLeft)
        {
            Color newColor7 = Color.CornflowerBlue;
            for (int num613 = 0; num613 < 7; num613++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 58, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 150, default, 0.8f);
            }
            for (float num614 = 0f; num614 < 1f; num614 += 0.125f)
            {
                Dust.NewDustPerfect(Projectile.Center, 278, Vector2.UnitY.RotatedBy(num614 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (4f + Main.rand.NextFloat() * 4f), 150, newColor7).noGravity = true;
            }
            for (float num615 = 0f; num615 < 1f; num615 += 0.25f)
            {
                Dust.NewDustPerfect(Projectile.Center, 278, Vector2.UnitY.RotatedBy(num615 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (2f + Main.rand.NextFloat() * 3f), 150, Color.Gold).noGravity = true;
            }
            Vector2 vector52 = new Vector2(Main.screenWidth, Main.screenHeight);
            if (Projectile.Hitbox.Intersects(Utils.CenteredRectangle(Main.screenPosition + vector52 / 2f, vector52 + new Vector2(400f))))
            {
                for (int num616 = 0; num616 < 7; num616++)
                {
                    Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, Main.rand.NextVector2CircularEdge(0.5f, 0.5f) * Projectile.velocity.Length(), Utils.SelectRandom(Main.rand, 16, 17, 17, 17, 17, 17, 17, 17));
                }
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            dir = Projectile.velocity.X > 0 ? 1 : -1;
        }
        public override void AI()
        {
            if (Projectile.timeLeft < 60)
            {
                Projectile.velocity *= 0.5f;
                Projectile.aiStyle = -1;
            }
            else
            {
                Projectile.direction = dir;
                Projectile.velocity.X = Projectile.direction * 5;
            }

        }
    }
    public class HyperSHockwave : ModProjectile
    {
        public override string Texture => RegreUtils.Empty;
        public override void SetDefaults()
        {
            Projectile.height = 300;
            Projectile.width = 300;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.ai[1] = 1;
        }
        public override void PostAI()
        {
            if (Projectile.ai[1] == 1)
                Projectile.damage = 0;
        }
        public override bool ShouldUpdatePosition() => false;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = RegreUtils.GetExtraTexture("explosion");
            Main.spriteBatch.Reload(BlendState.Additive);
            float alpha = MathHelper.Lerp(1, 0, Projectile.ai[0]);
            for (int i = 0; i < 2; i++)
                Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.Purple * alpha, Projectile.rotation, tex.Size() / 2, Projectile.ai[0], SpriteEffects.None, 0);
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White * alpha, Projectile.rotation, tex.Size() / 2, Projectile.ai[0] * 0.9f, SpriteEffects.None, 0);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }
        public override void AI()
        {
            Projectile.ai[0] += 0.05f;
            if (Projectile.ai[0] > 1)
                Projectile.Kill();
        }
    }
}
