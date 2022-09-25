using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Regressus.Projectiles.Minibosses.Conjurer;
using Terraria.DataStructures;
using Regressus.NPCs.UG;
using Regressus.Projectiles;
using ReLogic.Content;
using Terraria.GameContent.Bestiary;
using Regressus.Projectiles.Oracle;
using Terraria.Audio;
using static System.Formats.Asn1.AsnWriter;
using System.Text.Encodings.Web;
using Microsoft.Xna.Framework.Graphics.PackedVector;

namespace Regressus.NPCs.Minibosses
{
    public class TheConjurer : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 12;
            NPCID.Sets.ShouldBeCountedAsBoss[Type] = true;
            NPCID.Sets.TrailCacheLength[Type] = 10;
            NPCID.Sets.TrailingMode[Type] = 0;
        }
        public override Color? GetAlpha(Color drawColor) => Color.White;
        SoundStyle hitSound = new("Regressus/Sounds/Custom/GhostScream")
        { Pitch = -0.1f, PitchVariance = 0.1f };
        public override void SetDefaults()
        {
            NPC.CloneDefaults(ModContent.NPCType<Apparition>());
            NPC.lifeMax = 2000;
            NPC.Size = new Vector2(112, 124);
            NPC.dontTakeDamage = false;
            NPC.defense = 8;
            NPC.defense = 8;
            NPC.aiStyle = 0;
            NPC.knockBackResist = 0;
            NPC.boss = true;
            NPC.HitSound = hitSound;
            Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Conjurer");
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 pos, Color drawColor)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.Npc[Type].Value;
            Vector2 drawOrigin = new Vector2((tex.Width / 2) * 0.5F, (tex.Height / Main.npcFrameCount[NPC.type]) * 0.5F);
            Vector2 drawPos = new Vector2(
                NPC.position.X - pos.X + (NPC.width / 4) - (tex.Width / 4) * NPC.scale / 4 + drawOrigin.X * NPC.scale,
                NPC.position.Y - pos.Y + NPC.height - tex.Height * NPC.scale / Main.npcFrameCount[NPC.type] + 4f + drawOrigin.Y * NPC.scale + NPC.gfxOffY
                );
            var fadeMult = 1f / NPCID.Sets.TrailCacheLength[Type];
            for (int i = 0; i < NPC.oldPos.Length; i++)
            {
                Vector2 drawPosOld = new Vector2(
                    NPC.oldPos[i].X - pos.X + (NPC.width / 4) - (tex.Width / 4) * NPC.scale / 4 + drawOrigin.X * NPC.scale,
                    NPC.oldPos[i].Y - pos.Y + NPC.height - tex.Height * NPC.scale / Main.npcFrameCount[NPC.type] + 4f + drawOrigin.Y * NPC.scale + NPC.gfxOffY
                    );
                Main.spriteBatch.Draw(tex, drawPosOld, NPC.frame, Color.White * (1f - fadeMult * i) * 0.25f * alpha, NPC.oldRot[i], drawOrigin, NPC.scale, NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            }
            Main.spriteBatch.Draw(tex, drawPos, NPC.frame, Color.White * alpha, NPC.rotation, drawOrigin, NPC.scale, NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);

            return false;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
                new FlavorTextBestiaryInfoElement("A malevolent occultist of the caverns, and the Blind One's right hand. With his overwhelming dark powers, he is a force to be reckoned with."),
            });
        }
        public override void FindFrame(int frameHeight)
        {
            int frameWidth = 112;
            NPC.frame.Width = frameWidth;
            NPC.frameCounter++;
            if (NPC.frameCounter >= 5)
            {
                if (AIState == Intro)
                {
                    if (NPC.frame.Y == 0 && !NPC.IsABestiaryIconDummy)
                    {
                        RegreSystem.ChangeCameraPos(NPC.Center, 75, 1.9f);
                    }
                    NPC.frame.X = 0;
                    if (NPC.frame.Y < frameHeight * 10)
                    {
                        NPC.frame.Y += frameHeight;
                    }
                    else
                    {
                        NPC.frame.X = 2 * 112;
                        AIState = Banshee;
                        NPC.frameCounter = 0;
                        NPC.frame.Y = 0;
                    }
                }
                else if (AIState != Ghastroot && AIState != Intro && AIState != Bulwark)
                {
                    NPC.frame.X = 2 * 112;
                    if (NPC.frame.Y < frameHeight * 5)
                    {
                        NPC.frame.Y += frameHeight;
                    }
                    else
                    {
                        NPC.frame.Y = 0;
                        NPC.frameCounter = 0;
                    }
                }
                else if (AIState == Ghastroot)
                    if (NPC.frame.Y < frameHeight * 11)
                    {
                        NPC.frame.Y += frameHeight;
                    }
                    else
                    {
                        NPC.frame.Y = 0;
                        NPC.frameCounter = 0;
                    }
                else if (AIState == Bulwark && AITimer2 == 1)
                {
                    NPC.frame.X = 3 * 112;
                    if (NPC.frame.Y < frameHeight * 2)
                    {
                        NPC.frame.Y += frameHeight;
                    }
                    else
                    {
                        NPC.frame.Y = 0;
                        NPC.frameCounter = 0;
                    }
                }
                NPC.frameCounter = 0;
            }
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (AIState == Banshee)
            {
                Texture2D texture = RegreUtils.GetExtraTexture("PulseCircle");
                RegreUtils.Reload(spriteBatch, BlendState.Additive);
                for (int i = 1; i < 9; i++)
                {
                    float scale = i * NPC.ai[3] * 0.4f;
                    float _alpha = Utils.GetLerpValue(1, 0, NPC.ai[3]);
                    spriteBatch.Draw(texture, NPC.Center - screenPos, null, Color.Crimson * _alpha, 0, texture.Size() / 2, scale, SpriteEffects.None, 0f);
                }
                RegreUtils.Reload(spriteBatch, BlendState.AlphaBlend);
            }
        }
        private const int Intro = 0;
        private const int Banshee = -1;
        private const int Ghastroot = 1;
        private const int Bulwark = 2;
        private const int Lich = 3;
        private const int Conjurer1 = 4;
        private const int Conjurer2 = 5;
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
        public float AITimer2
        {
            get => NPC.ai[2];
            set => NPC.ai[2] = value;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneRockLayerHeight && spawnInfo.Player.ZonePurity)
                return 0.015f;
            return 0;
        }
        float alpha = 1;
        public override void AI()
        {
            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];
            if (!player.active || player.dead)
                NPC.timeLeft = 2;
            if (NPC.life < 100)
            {
                alpha = Utils.GetLerpValue(0, 100, NPC.life);
            }
            if (AIState == Banshee)
            {
                player.velocity = Vector2.Zero;
                if (++AITimer2 == 30)
                {
                    RegreSystem.ChangeCameraPos(player.Center, 60, 1.85f);
                    NPC.Center = player.Center - Vector2.UnitX * player.direction * 145;
                }
                if (AITimer2 == 90)
                {
                    if (player.direction != NPC.direction)
                    {
                        AITimer++;
                        AITimer2 = 0;
                        NPC.ai[3] = 0;
                    }
                    else
                    {
                        RegreSystem.ScreenShakeAmount = 15f;
                        Terraria.Audio.SoundStyle ae = new Terraria.Audio.SoundStyle("Regressus/Sounds/Custom/GhostScream")
                        {
                            Pitch = -0.5f
                        };
                        Terraria.Audio.SoundEngine.PlaySound(ae);
                    }
                }
                if (AITimer2 == 110)
                {
                    player.Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(player.name + " was consumed by the conjurer!"), 25, 0);
                }
                if (AITimer2 >= 205)
                {
                    AITimer++;
                    AITimer2 = 0;
                    NPC.ai[3] = 0;
                }
                if (AITimer2 > 90)
                {
                    NPC.ai[3] = (float)Math.Sin((double)((AITimer2 - 90) / 115)) * 2;
                }
                if (AITimer == 2)
                {
                    AITimer = 0;
                    AITimer2 = 0;
                    NPC.ai[3] = 0;
                    NPC.frameCounter = 0;
                    NPC.frame.Y = 0;
                    NPC.frame.X = 112;
                    AIState = Ghastroot;
                }
            }
            else if (AIState == Ghastroot)
            {
                AITimer++;
                if (player.Center.Distance(NPC.Center) > (16 * 2) && AITimer >= 30)
                    NPC.velocity = RegreUtils.FromAToB(NPC.Center, player.Center, true) * 1.75f;
                else
                    NPC.velocity = Vector2.Zero;
                if (NPC.frame.Y == 7 * NPC.frame.Height && NPC.frameCounter == 1)
                {
                    float rotation = MathHelper.ToRadians(45);
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item9);
                    Vector2 pos = NPC.Center - Vector2.UnitY * 23;
                    for (int i = 0; i < 2; i++)
                    {
                        Vector2 perturbedSpeed = (RegreUtils.FromAToB(NPC.Center, player.Center) * 9.5f).RotatedBy(Main.rand.NextFloat(-rotation, rotation));
                        Vector2 perturbedSpeed1 = (RegreUtils.FromAToB(NPC.Center, player.Center) * 9.5f).RotatedBy(Main.rand.NextFloat(-rotation, rotation));
                        perturbedSpeed1.Normalize();

                        Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, perturbedSpeed, ModContent.ProjectileType<ConjurerFireball>(), 15, 1.5f, player.whoAmI);
                    }
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, RegreUtils.FromAToB(NPC.Center, player.Center) * 9.5f, ModContent.ProjectileType<ConjurerFireball>(), 15, 1.5f, player.whoAmI);

                    AITimer2 = 0;
                }
                if (AITimer >= 180)
                {
                    AITimer = 0;
                    AITimer2 = 0;
                    NPC.ai[3] = 0;
                    NPC.frameCounter = 0;
                    AIState = Bulwark;
                    NPC.velocity = Vector2.Zero;
                }
            }
            else if (AIState == Bulwark)
            {
                AITimer++;
                if (AITimer == 1)
                {
                    NPC.ai[3] = 1;
                }
                else if (AITimer == 101)
                {
                    NPC.ai[3] = -1;
                }
                if (AITimer == 100)
                {

                    AITimer2 = 2;
                }
                if (AITimer2 == 0)
                {
                    NPC.damage = 0;
                    NPC.velocity = Vector2.Zero;
                    NPC.Center = Vector2.Lerp(NPC.Center, player.Center + Vector2.UnitX * 340 * NPC.ai[3], 0.035f);
                }
                if (AITimer2 == 3)
                {
                    AITimer++;
                    Vector2 rainPos = new Vector2(Main.screenPosition.X + Main.screenWidth * Main.rand.NextFloat(), Main.screenPosition.Y);
                    if (AITimer % 5 == 0)
                    {
                        Projectile p = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), rainPos, Vector2.UnitY * 10, ModContent.ProjectileType<Projectiles.Melee.AnimosityP2>(), 25, 1);
                        p.hostile = true;
                        p.friendly = false;
                    }
                }
                if (AITimer2 == 1 || AITimer2 == 3)
                    NPC.direction = NPC.velocity.X > 1 ? 1 : -1;
                if (NPC.collideX && AITimer2 == 1)
                {
                    RegreSystem.ScreenShakeAmount = 15;
                    NPC.velocity = Vector2.Zero;
                    AITimer2 = 3;
                }
                if (AITimer2 == 2)
                {
                    NPC.frame.X = 112 * 3;
                    NPC.noTileCollide = false;
                    Terraria.Audio.SoundStyle ae = new Terraria.Audio.SoundStyle("Regressus/Sounds/Custom/GhostScream")
                    {
                        Pitch = -0.25f
                    };
                    Terraria.Audio.SoundEngine.PlaySound(ae);
                    AITimer2 = 1;
                    NPC.damage = 15;
                    Vector2 vector9 = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y + (NPC.height * 0.5f));

                    float rotation2 = (float)Math.Atan2((vector9.Y) - (player.Center.Y), (vector9.X) - (player.Center.X));
                    NPC.velocity.X = (float)(Math.Cos(rotation2) * 65) * -1;
                }
                if (AITimer >= 200)
                {
                    NPC.noTileCollide = true;
                    NPC.damage = 0;
                    AIState = Lich;
                    NPC.aiStyle = 0;
                    AITimer = 0;
                    AITimer2 = 0;
                    NPC.ai[3] = 0;
                }
            }
            else if (AIState == Lich)
            {
                AITimer++;
                if (AITimer == 1 || AITimer == 100)
                    for (int i = 0; i < 3; i++)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<ConjuCandle>(), 0, 0, player.whoAmI, 0, 0);
                    }
                if (AITimer >= 200)
                {
                    AIState = Banshee;
                    AITimer = 0;
                    AITimer2 = 0;
                    NPC.ai[3] = 0;
                }
            }
        }
    }
}
