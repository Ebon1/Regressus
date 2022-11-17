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
using System.Collections.Generic;
namespace Regressus.NPCs.Ocean
{
    public class Seapostle : ModNPC
    {
        public List<NPC> clones = new List<NPC>();
        NPC teleportTo;
        bool issue;
        bool success = false;
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
            return new Color(drawColor.R - NPC.alpha, drawColor.G - NPC.alpha, drawColor.B - NPC.alpha, drawColor.A - NPC.alpha);
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
            if (flash)
            {
                RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
                float progress = Utils.GetLerpValue(1, 0, AITimer2);
                Texture2D glow = ModContent.Request<Texture2D>("Regressus/Extras/crosslight").Value;
                float mult = (0.55f + (float)Math.Sin(Main.GlobalTimeWrappedHourly/* * 2*/) * 3f);
                Main.spriteBatch.Draw(glow, NPC.Center - Main.screenPosition, null, drawColor * progress, mult, glow.Size() / 2, 0.25f * mult, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(glow, NPC.Center - Main.screenPosition, null, new Color(drawColor.R + 150, drawColor.G, drawColor.B, drawColor.A) * progress, mult, glow.Size() / 2, 0.35f * mult, SpriteEffects.None, 0f);
                RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            }
        }
        bool flash;
        public override void OnSpawn(IEntitySource source)
        {
            for (int i = 0; i < 8; i++)
            {
                float b = MathHelper.ToRadians(360);
                NPC a = NPC.NewNPCDirect(source, NPC.Center + (Vector2.One * 250).RotatedByRandom(b), ModContent.NPCType<CloneSeapostle>());
                clones.Add(a);
                a.ai[3] = NPC.whoAmI;
                a.ai[2] = b;
                a.ai[0] = i;
            }
        }
        public override void AI()
        {
            AITimer3 += (float)Math.PI / 30;
            NPC.alpha = (int)MathHelper.Lerp(45, 90, (float)Math.Sin(AITimer3));
            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];
            Vector2 targ = !player.wet ? NPC.Center + new Vector2(player.Center.X - NPC.Center.X) : player.MountedCenter;
            void BubbleSurprise()
            {
                for (int i = 0; i < 7; i++)
                {
                    float angle = 2f * (float)MathHelper.Pi / 4 * i;
                    Projectile a = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center, Vector2.One.RotatedBy(angle), ProjectileID.Bubble, 0, 0f);
                    a.friendly = false;
                    a.hostile = true;
                }
            }
            void Move()
            {
                SetVel(true);
                SetVel(false);
                SetVel(false);
            }
            void SetVel(bool alt)
            {
                Vector2 to = targ - NPC.Center;
                float rot = (float)Math.Atan(to.Y / to.X) + (to.X < 0 ? (float)Math.PI : 0);
                rot += Main.rand.Next(-45, 45) * (alt ? 0.01f : 0.1f);
                Vector2 towards = new Vector2((float)Math.Cos(rot), (float)Math.Sin(rot));
                towards *= alt ? (to.Length() * 0.0001f) : 0.00625f;
                NPC.velocity += towards;
            }
            NPC.direction = targ.X > NPC.Center.X ? 1 : -1;
            NPC.spriteDirection = NPC.direction;
            NPC.TargetClosest();
            NPC.noGravity = true;
            //Main.NewText($"{Main.tile[(int)NPC.Center.X / 16, (int)NPC.Center.Y / 16].LiquidAmount}");

            bool InWater() { Tile tile = Main.tile[(int)NPC.Center.X / 16, (int)NPC.Center.Y / 16]; return tile.LiquidAmount == 255 || !WorldGen.TileEmpty((int)NPC.Center.X / 16, (int)NPC.Center.Y / 16); }
            if (AIState == idle)
            {
                NPC.noTileCollide = false;
                AITimer++;
                if (InWater())
                {
                    Move();
                    NPC.noGravity = true;
                }
                else
                    NPC.noGravity = false;
                if (AITimer >= 250)
                {
                    AIState = attack;
                    AITimer = 0;
                    NPC.velocity = Vector2.Zero;
                    NPC.frame.Y = NPC.height * 4;
                    BubbleSurprise();
                }
            }
            else if (AIState == attack)
            {
                if(!issue)
                {
                    int attempt = 0;
                    while (!success)
                    {
                        attempt++;
                        NPC npc = Main.rand.Next(clones);
                        if (npc != null && npc.active)
                        {
                            success = true;
                            teleportTo = npc;
                            NPC.Center = teleportTo.Center;
                        }
                        if (attempt >= 10)
                        {
                            success = true;
                            issue = true;
                        }
                    }
                }
                NPC.noTileCollide = true;
                AITimer++;
                AITimer2 -= (float)1 / 50;
                NPC.alpha = 75;
                NPC.velocity *= 0.975f;
                if (AITimer == 25)
                {
                    flash = true;
                    SoundEngine.PlaySound(SoundID.DD2_BetsyScream);
                }
                if (AITimer < 50)
                {
                    NPC.velocity = Vector2.Zero;
                    NPC.rotation = RegreUtils.FromAToB(NPC.Center, Main.player[NPC.target].Center).ToRotation() + MathHelper.PiOver2;
                }
                if (AITimer == 50)
                {
                    Vector2 vector9 = NPC.Center;
                    {
                        float rotation2 = (float)Math.Atan2((vector9.Y) - (player.Center.Y), (vector9.X) - (player.Center.X));
                        NPC.velocity.X = (float)(Math.Cos(rotation2) * 22) * -1;
                        NPC.velocity.Y = (float)(Math.Sin(rotation2) * 22) * -1;
                    }
                }
                if (AITimer > 50)
                {
                    Dust.NewDustDirect(new Vector2(NPC.Center.X + Main.rand.Next(NPC.width / -2, NPC.width / 2), NPC.Center.Y + Main.rand.Next(NPC.height / -2, NPC.height / 2)), 0, 0, DustID.BubbleBlock).noGravity = true;
                    AITimer2 += (float)1 / 25;
                }
                if (AITimer >= 100)
                {
                    AITimer2 = 1;
                    AIState = idle;
                    AITimer = 0;
                    NPC.rotation = 0;
                    NPC.velocity = Vector2.Zero;
                    flash = false;
                    success = false;
                    NPC.Center = teleportTo.Center;
                    BubbleSurprise();
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
            return new Color(drawColor.R - NPC.alpha, drawColor.G - NPC.alpha, drawColor.B - NPC.alpha, drawColor.A - NPC.alpha);
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
            NPC npc = Main.npc[(int)AITimer3];
            NPC.alpha = (int)MathHelper.Lerp(75, 255, AITimer / 90);
            if(AITimer++ < 1)
            {
                SetVel(true);
                SetVel(false);
                SetVel(false);
            }
            void SetVel(bool alt)
            {
                Vector2 to = npc.Center - NPC.Center;
                float rot = (float)Math.Atan(to.Y / to.X) + (to.X < 0 ? (float)Math.PI : 0);
                rot += Main.rand.Next(-45, 45) * (alt ? 0.01f : 0.1f);
                Vector2 towards = new Vector2((float)Math.Cos(rot), (float)Math.Sin(rot));
                towards *= alt ? (to.Length() * 0.01f) : 1.625f;
                NPC.velocity += towards;
            }
            if(AITimer > 60)
            {
                NPC.velocity = Vector2.Zero;
                AITimer = 0;
            }
            /*if (AITimer2 >= 250)
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
                AITimer2 = 0;
                NPC.velocity = Vector2.Zero;
            }
            else
            {
                AITimer2++;
                if (AITimer2 == 25)
                {
                    SoundEngine.PlaySound(SoundID.Roar);
                }
                if (AITimer2 < 50)
                    NPC.rotation = RegreUtils.FromAToB(NPC.Center, Main.player[NPC.target].Center).ToRotation() + MathHelper.PiOver2;
                Vector2 moveTo = player.Center + (Vector2.One * 350).RotatedBy(AITimer2);
                if (AITimer2 == 50)
                    NPC.velocity = RegreUtils.FromAToB(NPC.Center, moveTo) * 30f;
                if (AITimer2 >= 80)
                {
                    AIState = idle;
                    AITimer2 = 0;
                    NPC.rotation = 0;
                    NPC.velocity = Vector2.Zero;
                }

            }*/
            /* NPC.TargetClosest();
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
             else
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
