using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Regressus.NPCs;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Regressus.Projectiles.Enemy.Overworld;
using Terraria.Audio;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace Regressus.NPCs.Ocean
{
    public class Seapostle : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 7;
        }
        public override void SetDefaults()
        {
            NPC.width = 56;
            NPC.height = 76;
            NPC.lifeMax = 60;
            NPC.defense = 2;
            NPC.damage = 15;
            NPC.knockBackResist = 0f;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.lavaImmune = true;
        }
        public override Color? GetAlpha(Color drawColor)
        {
            return Color.White;
        }
        /*public override bool PreDraw(SpriteBatch spriteBatch, Vector2 pos, Color drawColor)
        {
            float num66 = 0;
            Vector2 vector11 = new Vector2((float)(TextureAssets.Npc[Type].Value.Width / 2), (float)(TextureAssets.Npc[Type].Value.Height / Main.npcFrameCount[NPC.type] / 2));
            SpriteEffects spriteEffects = SpriteEffects.None;
            SpriteEffects spriteEffects2;
            if (NPC.spriteDirection == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
                //spriteEffects2 = SpriteEffects.None;
            }
            Microsoft.Xna.Framework.Rectangle frame6 = NPC.frame;
            float num212 = 1f - (float)NPC.life * 2 / (float)NPC.lifeMax;
            num212 *= num212;
            for (int i = 0; i < 8; i++)
            {
                float angle = 2f * (float)Math.PI / 8f * i;
                Vector2 position9 = Main.player[NPC.target].Center + (Main.player[NPC.target].Center - NPC.Center).RotatedBy(angle);
                spriteEffects2 = i % 2 != 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                float rot = 0;
                if (AIState == attack)
                    rot = RegreUtils.FromAToB(position9, Main.player[NPC.target].Center).ToRotation() + MathHelper.PiOver2;
                Main.spriteBatch.Draw(TextureAssets.Npc[Type].Value, position9 - pos, new Microsoft.Xna.Framework.Rectangle?(frame6), Color.White * 0.95f, rot, vector11, NPC.scale, spriteEffects2, 0);
            }
            return false;
        }*/
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (AIState == idle)
            {
                if (NPC.frameCounter >= 5)
                {
                    NPC.frame.Y += NPC.height;
                    if (NPC.frame.Y >= 4 * NPC.height)
                        NPC.frame.Y = 0;
                    NPC.frameCounter = 0;
                }
            }
            else
            {
                if (NPC.frameCounter >= 5)
                {
                    NPC.frame.Y += NPC.height;
                    if (NPC.frame.Y >= 7 * NPC.height)
                        NPC.frame.Y = 4 * NPC.height;
                    NPC.frameCounter = 0;
                }
            }
        }
        const int idle = 0, attack = 1;
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
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (flash != Vector2.Zero)
            {
                RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
                float progress = Utils.GetLerpValue(0f, 25, AITimer2);
                Texture2D glow = ModContent.Request<Texture2D>("Regressus/Extras/crosslight").Value;
                float mult = (0.55f + (float)Math.Sin(Main.GlobalTimeWrappedHourly/* * 2*/) * 0.3f);
                Main.spriteBatch.Draw(glow, flash - Main.screenPosition, null, Color.White, Main.GameUpdateCount * 0.0025f, glow.Size() / 2, 0.65f * mult, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(glow, flash - Main.screenPosition, null, Color.Red, Main.GameUpdateCount * 0.0025f, glow.Size() / 2, 0.75f * mult, SpriteEffects.None, 0f);
                RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            }
        }
        Vector2 flash;
        public override void OnSpawn(IEntitySource source)
        {
            for (int i = 0; i < 8; i++)
            {
                float b = MathHelper.ToRadians(360);
                NPC a = NPC.NewNPCDirect(source, NPC.Center + (Vector2.One * 250).RotatedByRandom(b), ModContent.NPCType<CloneSeapostle>());
                a.ai[3] = NPC.whoAmI;
                a.ai[2] = b;
                a.ai[0] = i;
            }
        }
        public override void AI()
        {
            Player player = Main.player[NPC.target];
            NPC.direction = player.Center.X > NPC.Center.X ? 1 : -1;
            NPC.spriteDirection = NPC.direction;
            NPC.TargetClosest();
            NPC.noGravity = true;
            if (AIState == idle)
            {
                AITimer++;
                AITimer2 += 0.01f;
                Vector2 moveTo = player.Center;
                NPC.velocity = RegreUtils.FromAToB(NPC.Center, player.Center, false) * 0.0025f;
                if (AITimer >= 250)
                {
                    AITimer2 = 0;
                    AIState = attack;
                    AITimer = 0;
                    NPC.velocity = Vector2.Zero;
                    NPC.frame.Y = NPC.height * 4;
                    for (int i = 0; i < 7; i++)
                    {
                        float angle = 2f * (float)MathHelper.Pi / 4 * i;
                        Projectile a = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center, Vector2.One.RotatedBy(angle), ProjectileID.Bubble, 0, 0f);
                        a.friendly = false;
                        a.hostile = true;
                    }
                }
            }
            else if (AIState == attack)
            {
                AITimer++;
                if (AITimer == 25)
                {
                    flash = NPC.Center;
                    SoundEngine.PlaySound(SoundID.Roar);
                }
                if (AITimer < 50)
                {
                    NPC.velocity = Vector2.Zero;
                    NPC.rotation = RegreUtils.FromAToB(NPC.Center, Main.player[NPC.target].Center).ToRotation() + MathHelper.PiOver2;
                }
                if (AITimer == 50)
                {
                    flash = Vector2.Zero;
                    Vector2 vector9 = NPC.Center;
                    {
                        float rotation2 = (float)Math.Atan2((vector9.Y) - (player.Center.Y), (vector9.X) - (player.Center.X));
                        NPC.velocity.X = (float)(Math.Cos(rotation2) * 22) * -1;
                        NPC.velocity.Y = (float)(Math.Sin(rotation2) * 22) * -1;
                    }
                }
                if (AITimer >= 65)
                {
                    AITimer3++;
                    AITimer2 = 0;
                    AIState = idle;
                    AITimer = 0;
                    NPC.rotation = 0;
                    NPC.velocity = Vector2.Zero;
                }

            }
        }
    }
    public class CloneSeapostle : ModNPC
    {
        public override string Texture => "Regressus/NPCs/Ocean/Seapostle";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Seapostle");
            Main.npcFrameCount[Type] = 7;
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new NPCID.Sets.NPCBestiaryDrawModifiers(0) { Hide = true, });
        }
        public override void SetDefaults()
        {
            NPC.width = 56;
            NPC.height = 76;
            NPC.lifeMax = 50;
            NPC.defense = 2;
            NPC.damage = 0;
            NPC.knockBackResist = 0f;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.aiStyle = 0;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.lavaImmune = true;
            NPC.dontTakeDamage = true;
        }
        public override Color? GetAlpha(Color drawColor)
        {
            return Color.White * 0.7f;
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter >= 5)
            {
                NPC.frame.Y += NPC.height;
                if (NPC.frame.Y >= 4 * NPC.height)
                    NPC.frame.Y = 0;
                NPC.frameCounter = 0;
            }
        }
        const int idle = 0, attack = 1;
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
        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            if (!Main.npc[(int)AITimer3].active)
                NPC.life = 0;
            NPC npc = Main.npc[(int)AITimer3];
            AITimer++;
            Vector2 moveTo = player.Center + (Vector2.One * 350).RotatedBy(AITimer2);
            NPC.velocity = RegreUtils.FromAToB(NPC.Center, player.Center, false) * 0.0018f;
            if (AITimer >= 250)
            {
                if (npc.ai[3] == AIState)
                {
                    npc.Center = NPC.Center;
                    for (int i = 0; i < 7; i++)
                    {
                        float angle = 2f * (float)MathHelper.Pi / 7 * i;
                        Projectile a = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), npc.Center, Vector2.One.RotatedBy(angle), ProjectileID.Bubble, 0, 0f);
                        a.friendly = false;
                        a.hostile = true;
                    }
                    NPC.life = 0;
                }
                AITimer = 0;
                NPC.velocity = Vector2.Zero;
            }
            /*else
            {
                AITimer++;
                if (AITimer == 25)
                {
                    SoundEngine.PlaySound(SoundID.Roar);
                }
                if (AITimer < 50)
                    NPC.rotation = RegreUtils.FromAToB(NPC.Center, Main.player[NPC.target].Center).ToRotation() + MathHelper.PiOver2;
                Vector2 moveTo = player.Center + (Vector2.One * 350).RotatedBy(AITimer2);
                if (AITimer == 50)
                    NPC.velocity = RegreUtils.FromAToB(NPC.Center, moveTo) * 30f;
                if (AITimer >= 80)
                {
                    AIState = idle;
                    AITimer = 0;
                    NPC.rotation = 0;
                    NPC.velocity = Vector2.Zero;
                }

            }*/
        }
    }
}
