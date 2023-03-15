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

namespace Regressus.NPCs.Overworld
{
    public class Obeselad :  ModNPC
    {
        enum StateID
        {
            Spawn,
            Walking,
            Standing,
            Bounced,
            Tripped
        };

        StateID state = StateID.Spawn;

        float heightMod;
        float widthMod;

        int storedDirection;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 16;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0) { Velocity = 1f };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults()
        {
            NPC.lifeMax = 5;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.knockBackResist = 1f;

            NPC.Size = new Vector2(72f, 36f);
            NPC.scale = 1f;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = Item.sellPrice(0, 0, 0, 90);

            NPC.aiStyle = -1;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Visuals.Moon,
                new FlavorTextBestiaryInfoElement("Someone please write the bestiary log for this fatass."),
            });
        }

        public override void AI()
        {
            Player player = Main.LocalPlayer;

            if (state == StateID.Spawn)
            {
                NPC.TargetClosest(true);
                storedDirection = NPC.direction;
                state = StateID.Walking;
            }

            NPC.spriteDirection = NPC.direction;
            Lighting.AddLight(NPC.Center, new Color(241, 212, 62).ToVector3() * 0.5f);

            if (NPC.Center.Distance(player.Center) <= 30f && player.velocity.Y > 0)
            {
                state = StateID.Bounced;
                SoundEngine.PlaySound(new SoundStyle("Regressus/Sounds/Custom/ObeseladBounce"), NPC.Center);

                player.velocity.Y = -10f;
            }

            if (NPC.Center.Distance(player.Center) >= 100f)
            {
                if (state != StateID.Tripped && state != StateID.Bounced)
                {
                    NPC.direction = storedDirection;
                }

                if (state == StateID.Walking)
                {
                    NPC.velocity.X += NPC.direction * 0.05f;

                    if (NPC.collideX)
                    {
                        NPC.velocity.X = -NPC.oldVelocity.X;
                        storedDirection = -storedDirection;
                    }
                }
                else
                {
                    NPC.velocity.X *= 0.7f;
                }
            }
            else
            {
                if (state != StateID.Tripped && state != StateID.Bounced)
                {
                    NPC.TargetClosest(true);
                }

                NPC.velocity.X *= 0.9f;
            }

            if (NPC.velocity.X >= 1f)
            {
                NPC.velocity.X = 1f;
            }

            if (NPC.velocity.X <= -1f)
            {
                NPC.velocity.X = -1f;
            }
        }

        public override void OnKill()
        {
            for (int k = 0; k < 20; k++)
            {
                Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, ModContent.DustType<Mushroom>(), 0f, 0f, 0, default, 1f).noGravity = true;
            }

            for (int k = 0; k < 3; k++)
            {
                if (Main.netMode != NetmodeID.Server) { Gore.NewGore(NPC.GetSource_Death(), NPC.position, Main.rand.NextVector2CircularEdge(0.5f, 0.5f) * NPC.velocity.Length(), 16, 1f); }
            }

            if (Main.netMode != NetmodeID.Server) { Gore.NewGore(NPC.GetSource_Death(), NPC.position, Vector2.Zero, Mod.Find<ModGore>("ObeseladCap").Type, 1f); }
        }

        public override void FindFrame(int frameHeight)
        {
            heightMod += (1 - heightMod) / 5f;
            widthMod += (1 - widthMod) / 5f;

            if (state == StateID.Bounced)
            {
                heightMod = 0.5f;
                widthMod = 2f;

                NPC.frame.Y = 8 * frameHeight;
                state = StateID.Tripped;
            }
            else if (state == StateID.Tripped)
            {
                NPC.frameCounter += 1f;

                if (NPC.frameCounter >= 5)
                {
                    NPC.frameCounter = 0;
                    NPC.frame.Y += frameHeight;

                    if (NPC.frame.Y >= 16 * frameHeight)
                    {
                        heightMod = 1.5f;
                        widthMod = 0.5f;

                        NPC.frame.Y = 0 * frameHeight;
                        state = StateID.Walking;
                    }
                }
            }
            else
            {
                NPC.frameCounter += (NPC.velocity.Length() * 1f);

                if (NPC.frameCounter >= 5)
                {
                    NPC.frameCounter = 0;
                    NPC.frame.Y += frameHeight;

                    if (NPC.frame.Y >= 8 * frameHeight) { NPC.frame.Y = 0 * frameHeight; }
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneOverworldHeight && !Main.dayTime)
                return 0.15f;
            return 0;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            IItemDropRule a = ItemDropRule.Common(ModContent.ItemType<Starspore>(), 3, 5, 35);
            npcLoot.Add(a);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {

            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;

            Vector2 position = NPC.Center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY) + new Vector2(0f, 20f);

            SpriteEffects spriteEffects = SpriteEffects.None;

            if (NPC.spriteDirection > 0)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }

            if (NPC.IsABestiaryIconDummy)
            {
                return true;
            }

            spriteBatch.Draw(texture, position, NPC.frame, Color.White, NPC.rotation, new Vector2(NPC.frame.Size().X / 2f, NPC.frame.Size().Y), new Vector2(widthMod, heightMod), spriteEffects, 0);

            return false;
        }
    }
}
