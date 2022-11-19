/*using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Regressus.NPCs;
using ReLogic.Content;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Bestiary;
using System.Collections.Generic;

namespace Regressus.NPCs.Sky
{
    public class AerialBud : ModNPC //UNFINISHED AS FUCK
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 8;
        }
        public override void SetDefaults()
        {
            NPC.width = 42;
            NPC.height = 60;
            NPC.lifeMax = 350;
            NPC.defense = 10;
            NPC.friendly = true;
            NPC.knockBackResist = 0.2f;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.lavaImmune = true;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            RegrePlayer regrePlayer = Main.LocalPlayer.GetModPlayer<RegrePlayer>();
            if (spawnInfo.Player.ZoneSkyHeight && RegreSystem.CloudTiles > 5 && !NPC.AnyNPCs(NPCType<AerialBud>()) && regrePlayer.AerialBudsGiven < regrePlayer.AerialBudsMax && RegreSystem.AerialBudsCooldown == 0)
                return 0.5f;
            return 0;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.UIInfoProvider = new Terraria.GameContent.Bestiary.CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[Type], quickUnlock: true);
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                new FlavorTextBestiaryInfoElement("A flying plant that wants to help terrarians in their daily chores. How sweet?\n\n" +
                    "Once you've given them an item they'll leave to find more of that item, they'll almost always find what they're looking for. If you've given three of them items they will stop spawning for 5 days.\n\n" +
                    "You can only give them stackable items.")
            });
        }
        public override void FindFrame(int frameHeight)
        {
            if (AIState == Idle || AIState == FlyToItem)
            {
                NPC.frameCounter++;
                if (NPC.frameCounter < 5)
                    NPC.frame.Y = 0 * frameHeight;
                else if (NPC.frameCounter < 10)
                    NPC.frame.Y = 1 * frameHeight;
                else if (NPC.frameCounter < 15)
                    NPC.frame.Y = 2 * frameHeight;
                else if (NPC.frameCounter < 20)
                    NPC.frame.Y = 3 * frameHeight;
                else
                    NPC.frameCounter = 0;
            }
            else
            {
                NPC.frameCounter++;
                if (NPC.frameCounter < 5)
                    NPC.frame.Y = 4 * frameHeight;
                else if (NPC.frameCounter < 10)
                    NPC.frame.Y = 5 * frameHeight;
                else if (NPC.frameCounter < 15)
                    NPC.frame.Y = 6 * frameHeight;
                else if (NPC.frameCounter < 20)
                    NPC.frame.Y = 7 * frameHeight;
                else
                    NPC.frameCounter = 0;
            }
        }
        public const int Idle = 0;
        public const int FlyToItem = 1;
        public const int FlyAway = 2;
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
        List<int> blacklist = new List<int>
        {
            ItemID.SoulofFright,
            ItemID.SoulofMight,
            ItemID.SoulofSight,
            ItemID.ChlorophyteBar,
            ItemID.ChlorophyteOre,
            ItemID.LunarBar,
            ItemID.LunarOre,
        };
        public override void AI()
        {
            NPC.TargetClosest(false);
            RegrePlayer regrePlayer = Main.player[NPC.target].GetModPlayer<RegrePlayer>();
            if (AIState == Idle)
            {
                foreach (Item item in Main.item)
                {
                    if (item.active)
                        if (item.maxStack > 1 && item.damage < 1 && !item.IsACoin && !blacklist.Contains(item.type) && item.buffType == 0 && !item.expert && !item.master && item.Center.Distance(NPC.Center) < 400)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                if (regrePlayer.AerialBudItem[i] != 0)
                                    continue;
                                regrePlayer.AerialBudItem[i] = item.type;
                                regrePlayer.AerialBudItemStack[i] = item.stack;
                                regrePlayer.AerialBudsGiven++;
                                NPC.ai[2] = item.whoAmI;
                                break;
                            }
                            AIState = FlyToItem;
                            AITimer = 0;
                            break;
                        }
                }
            }
            else if (AIState == FlyToItem)
            {
                if (AITimer == 0)
                {
                    Item item = Main.item[(int)NPC.ai[2]];
                    if (item.Center.Distance(NPC.Center) < 50)
                    {
                        item.active = false;
                        AITimer = 1;
                    }
                }
                else
                {
                    AIState = FlyAway;
                }
            }
            else if (AIState == FlyAway)
            {
                AITimer++;
                NPC.velocity.Y = -10;
                if (AITimer > 100)
                {
                    NPC.life = 0;
                }
            }
        }
    }
    public class AerialBudGiveBack : ModProjectile
    {
        public override string Texture => "Regressus/NPCs/Sky/AerialBud";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aerial Bud");
            Main.projFrames[Type] = 8;
        }
        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 60;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 800;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.netUpdate = true;
            Projectile.netImportant = true;
        }
        public override void Kill(int timeLeft)
        {
            int i = (int)Projectile.ai[0];
            RegrePlayer regrePlayer = Main.LocalPlayer.GetModPlayer<RegrePlayer>();
            regrePlayer.AerialBudItem[i] = 0;
            regrePlayer.AerialBudItemStack[i] = 0;
        }
        public override void AI()
        {
            int i = (int)Projectile.ai[0];
            RegrePlayer regrePlayer = Main.LocalPlayer.GetModPlayer<RegrePlayer>();
            if (Projectile.frame == 0)
                Projectile.frame = 4;
            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 7)
                    Projectile.frame = 4;
            }
            if (Projectile.ai[1] == 0)
            {
                if (Projectile.Center.Distance(Main.LocalPlayer.Center) > 45)
                {
                    Projectile.velocity.Y = 4f;
                }
                else
                {
                    Projectile.ai[1] = 1;
                    Projectile.velocity = Vector2.Zero;
                    if (Main.rand.NextBool(30))
                    {
                        Item.NewItem(Projectile.GetSource_FromThis(), Projectile.getRect(), regrePlayer.AerialBudItem[i], regrePlayer.AerialBudItemStack[i]);
                        CombatText.NewText(Projectile.getRect(), Color.LightGreen, ":(", true);
                    }
                    else
                    {
                        Item.NewItem(Projectile.GetSource_FromThis(), Projectile.getRect(), regrePlayer.AerialBudItem[i], regrePlayer.AerialBudItemStack[i] * 2);
                        CombatText.NewText(Projectile.getRect(), Color.LightGreen, ":D", true);
                    }
                }
            }
            else if (Projectile.ai[1] == 1)
            {
                Projectile.velocity.Y = -2f;
            }
        }
    }
}
*/