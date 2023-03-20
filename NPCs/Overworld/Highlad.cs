using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Regressus.Dusts;
using Regressus.Items.Ammo;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Regressus.NPCs.Overworld
{
    public class Highlad :  ModNPC
    {
        float heightMod = 1f;
        float widthMod = 1f;

        bool bounced;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5;
        }

        public override void SetDefaults()
        {
            NPC.lifeMax = 5;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.knockBackResist = 1f;

            NPC.Size = new Vector2(20f);
            NPC.scale = 1f;
            NPC.noGravity = true;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.DD2_LightningBugHurt;
            NPC.value = Item.sellPrice(0, 0, 0, 90);

            NPC.aiStyle = -1;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Visuals.Moon,
                new FlavorTextBestiaryInfoElement("The Minilad became so retarded, he gained the ability to fly."),
            });
        }

        public override void AI()
        {
            Player player = Main.LocalPlayer;

            NPC.velocity *= 0.98f;

            Lighting.AddLight(NPC.Center, new Color(241, 212, 62).ToVector3() * 0.5f);

            NPC.ai[0]++;

            if (NPC.ai[0] >= Main.rand.NextFloat(180f, 420f))
            {
                if (!player.HasBuff(BuffID.Shine) || !player.hasMagiluminescence)
                {
                    NPC.velocity = new Vector2(Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f));
                }
                else if (player.HasBuff(BuffID.Shine) || player.hasMagiluminescence)
                {
                    NPC.velocity = NPC.DirectionTo(player.Center) * 5f;
                }

                bounced = false;

                NPC.ai[0] = 0f;
            }

            if (player.itemAnimation == player.itemAnimationMax - 1f && NPC.Center.Distance(player.Center) <= 200f && (!player.HasBuff(BuffID.Shine) || !player.hasMagiluminescence))
            {
                NPC.velocity = NPC.DirectionFrom(player.Center) * 5f;
                bounced = false;

                NPC.ai[0] = 0f;
            }

            if (NPC.collideX)
            {
                NPC.velocity.X = -NPC.oldVelocity.X;
                bounced = true;

                SoundEngine.PlaySound(new SoundStyle("Regressus/Sounds/Custom/ObeseladBounce"), NPC.Center);
            }

            if (NPC.collideY)
            {
                NPC.velocity.Y = -NPC.oldVelocity.Y;
                bounced = true;

                SoundEngine.PlaySound(new SoundStyle("Regressus/Sounds/Custom/ObeseladBounce"), NPC.Center);
            }

            if (!bounced)
            {
                NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;

                heightMod = 1f + (NPC.velocity.Length() * 0.1f);
                widthMod = 1f - (NPC.velocity.Length() * 0.1f);
            }
            else
            {
                NPC.rotation += NPC.velocity.Length() * 0.2f;

                heightMod = 1f;
                widthMod = 1f;
            }

            if (Main.rand.NextBool(50))
            {
                Dust.NewDustPerfect(NPC.Center, DustID.PortalBoltTrail, new Vector2(Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f)), 0, new Color(241, 212, 62), 1f).noGravity = true;
            }
        }

        public override void OnKill()
        {
            for (int k = 0; k < 10; k++)
            {
                Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, ModContent.DustType<Mushroom>(), 0f, 0f, 0, default, 1f).noGravity = true;
                Dust.NewDustPerfect(NPC.Center, DustID.PortalBoltTrail, new Vector2(Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f)), 0, new Color(241, 212, 62), 1f).noGravity = true;
            }

            for (int k = 0; k < 3; k++)
            {
                if (Main.netMode != NetmodeID.Server) { Gore.NewGore(NPC.GetSource_Death(), NPC.position, Main.rand.NextVector2CircularEdge(0.5f, 0.5f) * NPC.velocity.Length(), 16, 1f); }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 1f;

            if (NPC.frameCounter >= 10)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;

                if (NPC.frame.Y >= Main.npcFrameCount[NPC.type] * frameHeight) { NPC.frame.Y = 0 * frameHeight; }
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

            Vector2 position = NPC.Center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY);

            SpriteEffects spriteEffects = SpriteEffects.None;

            spriteBatch.Draw(texture, position, NPC.frame, Color.White * 0.22f, NPC.rotation, NPC.frame.Size() / 2f, new Vector2(widthMod, heightMod) * 1.4f, spriteEffects, 0);
            spriteBatch.Draw(texture, position, NPC.frame, Color.White * 0.45f, NPC.rotation, NPC.frame.Size() / 2f, new Vector2(widthMod, heightMod) * 1.2f, spriteEffects, 0);
            spriteBatch.Draw(texture, position, NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2f, new Vector2(widthMod, heightMod), spriteEffects, 0);

            return false;
        }
    }
}
