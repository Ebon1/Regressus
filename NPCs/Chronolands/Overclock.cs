using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Regressus.Projectiles.Enemy.Overworld;
using Terraria.DataStructures;
using System.Security.Cryptography;
using Mono.Cecil;
using Regressus.Projectiles;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria.GameContent;

namespace Regressus.NPCs.Chronolands
{
    public class Overclock : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Overclock");
            NPCID.Sets.TrailCacheLength[Type] = 5;
            Main.npcFrameCount[Type] = 10;
            NPCID.Sets.TrailingMode[Type] = 0;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (!NPC.hide)
            {
                Texture2D tex = Terraria.GameContent.TextureAssets.Npc[Type].Value;
                var fadeMult = 1f / NPCID.Sets.TrailCacheLength[Type];
                for (int i = 0; i < NPC.oldPos.Length; i++)
                {
                    SpriteEffects effects = NPC.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                    Main.spriteBatch.Draw(tex, NPC.oldPos[i] - Main.screenPosition + NPC.Size / 2 + Vector2.UnitY * 2, NPC.frame, drawColor * (1f - fadeMult * i), NPC.rotation, NPC.Size / 2, NPC.scale, effects, 0f);
                }
            }
            return true;
        }
        public override void SetDefaults()
        {
            NPC.width = 36;
            NPC.height = 60;
            NPC.lifeMax = 100;
            NPC.defense = 5;
            NPC.damage = 35;
            NPC.knockBackResist = 0f;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.aiStyle = -1;
            NPC.lavaImmune = true;
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
        public float AITimer2
        {
            get => NPC.ai[2];
            set => NPC.ai[2] = value;
        }
        public float AITimer3
        {
            get => NPC.ai[3];
            set => NPC.ai[3] = value;
        }
        const int NoTarget = 0;
        const int Walk = -1;
        const int DualSlash = 1;
        const int Dash = 2;
        public override void FindFrame(int frameHeight)
        {
            if (AIState == Walk)
            {
                NPC.frameCounter++;
                if (NPC.frameCounter % 5 == 0)
                {
                    if (NPC.frame.Y < 9 * NPC.height)
                        NPC.frame.Y += NPC.height;
                    else
                        NPC.frame.Y = 0;
                }
            }
            else
                NPC.frame.Y = 7 * NPC.height;
        }
        float flashAlpha = 1f;
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (!NPC.IsABestiaryIconDummy)
            {
                spriteBatch.Reload(BlendState.Additive);
                spriteBatch.Draw(RegreUtils.GetExtraTexture("glow"), NPC.Center - screenPos, null, Color.DeepSkyBlue * flashAlpha, 0, RegreUtils.GetExtraTexture("glow").Size() / 2, 0.4f, SpriteEffects.None, 0);
                spriteBatch.Draw(RegreUtils.GetExtraTexture("glow"), NPC.Center - screenPos, null, Color.White * flashAlpha, 0, RegreUtils.GetExtraTexture("glow").Size() / 2, 0.35f, SpriteEffects.None, 0);
                spriteBatch.Reload(BlendState.AlphaBlend);
            }
        }
        public override bool PreKill()
        {
            NPC.NewNPCDirect(NPC.GetSource_FromAI(), NPC.Center, ModContent.NPCType<OverclockClock>());
            return true;
        }
        public override void AI()
        {
            if (flashAlpha > 0)
                flashAlpha -= 0.025f;
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            NPC.direction = player.Center.X > NPC.Center.X ? 1 : -1;
            NPC.spriteDirection = NPC.direction;
            if (AIState == NoTarget && NPC.Center.Distance(player.Center) < 500f)
                AIState = Walk;
            if (AIState == Walk)
            {
                AITimer++;
                AITimer3--;
                if (NPC.Center.Distance(player.Center) > 20f)
                {
                    if (NPC.collideY)
                        NPC.velocity.X += 0.01f * NPC.direction;
                    else
                        NPC.velocity.X += 0.0025f * NPC.direction;
                }
                if (Math.Sign(NPC.velocity.X) != NPC.direction || NPC.Center.Distance(player.Center) < 90f)
                {
                    //NPC.frame.Y = 3 * NPC.height;
                    NPC.frameCounter = 0;
                    NPC.velocity.X *= 0.92f;
                }
                if (NPC.collideX && AITimer2 != 1 && AITimer3 <= 0)
                {
                    AITimer3 = 30;
                    AITimer2 = 1;
                }
                if (AITimer2 == 1)
                {
                    AITimer2 = 2;
                    NPC.velocity.Y -= 6.7f;
                }
                if (NPC.velocity.Y < 0 && AITimer3 == 20)
                    NPC.velocity.X = NPC.direction * 2;
                if (AITimer >= 180)
                {
                    AITimer = 0;
                    AITimer2 = 0;
                    AITimer3 = 0;
                    NPC.velocity = Vector2.Zero;
                    NPC.frameCounter = 0;
                    AIState = Dash;
                }
            }
            else if (AIState == Dash)
            {
                AITimer++;
                if (AITimer == 60)
                {
                    AITimer2++;
                }
                if (AITimer2 >= 1)
                    AITimer2++;
                if (AITimer == 85)
                {
                    NPC.ai[3] = 2;
                    AITimer2 = 0;

                    NPC.velocity.X *= 0.98f;
                    Vector2 vector9 = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y + (NPC.height * 0.5f));

                    float rotation2 = (float)Math.Atan2((vector9.Y) - (player.Center.Y), (vector9.X) - (player.Center.X));
                    NPC.velocity.X = (float)(Math.Cos(rotation2) * 20) * -1;
                }
                if (NPC.Center.Distance(player.Center) < 35 || AITimer >= 110)
                {
                    NPC.ai[3] = 1;
                    NPC.velocity.X *= 0.8f;
                }
                /*if (AITimer == 100)
                {
                    AITimer2++;
                    
                }
                if (AITimer == 125)
                {
                    AITimer2 = 0;
                    NPC.Center = flash;
                    NPC.velocity.X = 0;
                    NPC.velocity.Y *= 1.2f;
                }
                if (AITimer == 126)
                {
                    
                }*/
                if (AITimer == 120)
                {
                    NPC.ai[3] = 0;
                    AITimer = 0;

                    AITimer2 = 0;
                    AITimer3 = 0;
                    NPC.velocity = Vector2.Zero;
                    AIState = Walk;
                    NPC.frameCounter = 0;
                }
            }
        }
    }
    public class OverclockClock : ModNPC
    {
        public override string Texture => RegreUtils.Placeholder;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Core");
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new NPCID.Sets.NPCBestiaryDrawModifiers(0) { Hide = true, });
        }
        public override void SetDefaults()
        {
            NPC.width = 36;
            NPC.height = 58;
            NPC.lifeMax = 100;
            NPC.defense = 0;
            NPC.damage = 0;
            NPC.knockBackResist = 0f;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.aiStyle = 0;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.lavaImmune = true;
            NPC.dontTakeDamage = true;
        }
        float bb;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            float progress = Utils.GetLerpValue(0, 200, aaa);
            float clockProg = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);
            rot = MathHelper.Lerp(0, MathHelper.ToRadians(360), bb);
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Extras/glow").Value, NPC.Center - Main.screenPosition, null, Color.DeepSkyBlue * clockProg * 0.5f, 0, ModContent.Request<Texture2D>("Regressus/Extras/glow").Value.Size() / 2, .75f * clockProg, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Extras/clock").Value, NPC.Center - Main.screenPosition, null, Color.DeepSkyBlue * clockProg, 0, ModContent.Request<Texture2D>("Regressus/Extras/clock").Value.Size() / 2, .15f * clockProg, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Extras/clockHand1_notRotated").Value, NPC.Center - Main.screenPosition, null, Color.DeepSkyBlue * clockProg, rot, ModContent.Request<Texture2D>("Regressus/Extras/clockHand1").Value.Size() / 2, .15f * clockProg, SpriteEffects.None, 0);
            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            return false;
        }
        float rot;
        int aaa = 200;
        public override bool PreKill()
        {
            NPC.ai[1] = 1;
            return true;
        }
        float c = 1f;
        public override void OnSpawn(IEntitySource source)
        {
            for (int i = 1; i < 7; i++)
            {
                Projectile a = Projectile.NewProjectileDirect(NPC.InheritSource(NPC), NPC.Center + Vector2.UnitY * 15, Main.rand.NextVector2Unit() * 10, ModContent.ProjectileType<OverclockGore>(), 0, 0, Main.player[NPC.target].whoAmI);
                a.ai[0] = NPC.whoAmI;
                a.ai[1] = i;
            }
        }
        public override void AI()
        {
            NPC.ai[0]++;
            aaa--;
            if (aaa < 175)
                NPC.dontTakeDamage = false;
            if (aaa > 100)
                bb += 0.01f * c;
            else
                bb -= 0.01f * c;
            if (aaa < 120 && aaa > 100)
                c -= 0.05f;
            else if (aaa < 100 && aaa > 80)
                c += 0.05f;
            if (NPC.ai[0] >= 200)
            {
                NPC.life = 0;

                //Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<RippleSmol>(), 0, 0f);
                NPC.checkDead();
                NPC.NewNPCDirect(NPC.InheritSource(NPC), NPC.Center, ModContent.NPCType<Overclock>());
            }
        }
    }
    public class OverclockGore : ModProjectile
    {
        public override string Texture => "Regressus/Gore/Overclock1";
        Vector2[] oldPos = new Vector2[100];
        bool isGoingBack;
        int thing;
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
            Projectile.timeLeft = 200;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D iWillKillMyselfOnThe3rdOfApril2023 = RegreUtils.GetTexture("Gore/Overclock" + Projectile.ai[1]);
            Main.EntitySpriteDraw(iWillKillMyselfOnThe3rdOfApril2023, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, iWillKillMyselfOnThe3rdOfApril2023.Size() / 2, 1f, SpriteEffects.None, 0);
            return false;
        }
        bool nope;
        public override void OnSpawn(IEntitySource source)
        {
        }
        public override void AI()
        {
            NPC owner = Main.npc[(int)Projectile.ai[0]];
            if (!owner.active || owner.life <= 0 || owner.ai[1] == 1)
            {
                nope = true;
            }
            if (!nope)
            {
                if (Projectile.timeLeft > 200)
                    Projectile.timeLeft = 200;
                isGoingBack = Projectile.timeLeft < 100;
                if (Projectile.timeLeft < 130 && Projectile.timeLeft > 100)
                    Projectile.velocity *= 0.95f;

                if (!isGoingBack)
                {
                    Projectile.rotation += 0.25f;
                    Projectile.velocity.Y += 0.15f;
                    for (int num16 = oldPos.Length - 1; num16 > 0; num16--)
                    {
                        oldPos[num16] = oldPos[num16 - 1];
                    }
                    oldPos[0] = Projectile.position;
                }
                else
                {
                    Projectile.rotation -= 0.25f;
                    Projectile.velocity = Vector2.Zero;
                    Projectile.aiStyle = -1;
                    thing++;
                    if (thing < oldPos.Length)
                    {
                        if (oldPos[thing] != Vector2.Zero)
                            Projectile.position = oldPos[thing];
                    }
                }
            }
            else
            {
                Projectile.aiStyle = 2;
            }
        }
    }
}
