using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Terraria.GameInput;
using Regressus.Dusts;
using Terraria.ID;
using Regressus.WorldGeneration;
using Regressus.Items.Accessories;
using Regressus.NPCs.Bosses.Oracle;
using Regressus.Buffs.Debuffs;
using Terraria.Graphics.Effects;
using System;

namespace Regressus
{
    public class RegrePlayer : ModPlayer
    {
        public static RegrePlayer instance;
        public int bossTextProgress, bossMaxProgress/*,
            biomeTextProgress, biomeMaxProgress*/;
        public string bossName;//, biomeName;
        public string bossTitle;//, biomeTitle;
        public int bossStyle;
        //public bool[] notFirstTimeEnteringBiome = new bool[17];
        //public bool[] inBiome = new bool[17];
        public Color bossColor;//, biomeColor;
        public Vector2[] oldCenter = new Vector2[950];
        public int[] oldLife = new int[950], oldDir = new int[950];
        public bool reverseTime;
        public bool starshroomed;
        public bool bladeSummon;
        public bool ginnungagap, ginnungagapHide, infectedIdol;
        int thing;
        public bool CantEatBaguette;

        //Queen Drae's bools
        public bool AcornFairyPet;
        public bool LavaBiome;
        public bool KryptonBiome;
        public bool ArgonBiome;
        public bool XenonBiome;
        public int AerialBudsGiven;
        public int AerialBudsMax = 3;
        public int[] AerialBudItem = new int[3];
        public int[] AerialBudItemStack = new int[3];


        //Weapons
        public int itemCombo;
        public int itemComboReset;
        public int lastSelectedItem;

        public bool ShrineBiome = false;
        public bool hasEncounteredMoth;
        public override void UpdateBadLifeRegen()
        {
            if (starshroomed)
            {
                if (Player.lifeRegen > 0)
                {
                    Player.lifeRegen = 0;
                }
                Player.lifeRegen -= 16;
            }
        }
        public override void ResetEffects()
        {
            instance = Player.GetModPlayer<RegrePlayer>();
            infectedIdol = false;
            starshroomed = false;
            reverseTime = false;
            ginnungagap = false;
            AcornFairyPet = false;
            ginnungagapHide = false;
            bladeSummon = false;

            if (itemComboReset <= 0)
            {
                itemCombo = 0;
                itemComboReset = 0;
            }
            else
            {
                itemComboReset--;
            }
        }


        public override bool PreItemCheck()
        {
            if (Player.selectedItem != lastSelectedItem)
            {
                itemComboReset = 0;
                itemCombo = 0;
                lastSelectedItem = Player.selectedItem;
            }
            if (itemComboReset > 0)
            {
                itemComboReset--;
                if (itemComboReset == 0)
                {
                    itemCombo = 0;
                }
            }

            return true;
        }
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            /*for (int i = 0; i < notFirstTimeEnteringBiome.Length; i++)
            {
                packet.Write(notFirstTimeEnteringBiome[i]);
            }
            */
            for (int i = 0; i < AerialBudItem.Length; i++)
            {
                packet.Write(AerialBudItem[i]);
                packet.Write(AerialBudItemStack[i]);
            }
            packet.Write(AerialBudsGiven);
            packet.Write(hasEncounteredMoth);
            packet.Write(CantEatBaguette);

            packet.Send(toWho, fromWho);
        }
        public override void SaveData(TagCompound tag)
        {
            //for (int i = 0; i < notFirstTimeEnteringBiome.Length; i++)
            //    tag.Set("Biome" + i, notFirstTimeEnteringBiome[i]);
            for (int i = 0; i < AerialBudItem.Length; i++)
            {
                tag.Set("ItemBud" + i, AerialBudItem[i]);
                tag.Set("StackBud" + i, AerialBudItemStack[i]);
            }
            tag.Set("Buds", AerialBudsGiven);
            tag.Set("Moth", hasEncounteredMoth);
            tag.Set("Baguette", CantEatBaguette);
        }
        public override void LoadData(TagCompound tag)
        {
            //for (int i = 0; i < notFirstTimeEnteringBiome.Length; i++)
            //    notFirstTimeEnteringBiome[i] = tag.GetBool("Biome" + i);
            for (int i = 0; i < AerialBudItem.Length; i++)
            {
                AerialBudItem[i] = tag.GetInt("ItemBud" + i);
                AerialBudItemStack[i] = tag.GetInt("StackBud" + i);
            }
            AerialBudsGiven = tag.GetInt("Buds");
            hasEncounteredMoth = tag.GetBool("Moth");
            CantEatBaguette = tag.GetBool("Baguette");
        }
        /*public void BiomeCheck()
        {

            ShrineBiome = ShrineBiomeSystem.IsInOrNearShrine(Player);

            //RegreUtils.SetBiomeTitle("The Chronolands", Color.SlateGray, "End of Time", 0);
            if (!inBiome[0])
            {
                if (Player.ZoneForest)
                {
                    RegreUtils.SetBiomeTitle("The Forest", Color.LightGreen, "Tranquil Grove", 0);
                    if (!notFirstTimeEnteringBiome[0])
                    {
                        notFirstTimeEnteringBiome[0] = true;
                    }
                    inBiome[0] = true;
                }
            }
            else
            {
                if (!Player.ZoneForest)
                {
                    inBiome[0] = false;
                }
            }


            if (!inBiome[1])
            {
                if (Player.ZoneJungle)
                {
                    RegreUtils.SetBiomeTitle("The Jungle", Color.DarkOliveGreen, "Equatorial Lumber", 1);
                    if (!notFirstTimeEnteringBiome[1])
                    {
                        notFirstTimeEnteringBiome[1] = true;
                    }
                    inBiome[1] = true;
                }
            }
            else
            {
                if (!Player.ZoneJungle)
                {
                    inBiome[1] = false;
                }
            }


            if (!inBiome[2])
            {
                if (Player.ZoneCrimson || Player.ZoneCorrupt)
                {
                    RegreUtils.SetBiomeTitle(Player.ZoneCrimson ? "The Crimson" : "The Corruption", Player.ZoneCrimson ? Color.DarkRed : Color.MediumPurple, "Infestation", 2);
                    if (!notFirstTimeEnteringBiome[2])
                    {
                        notFirstTimeEnteringBiome[2] = true;
                    }
                    inBiome[2] = true;
                }
            }
            else
            {
                if (!Player.ZoneCrimson && !Player.ZoneCorrupt)
                {
                    inBiome[2] = false;
                }
            }


            if (!inBiome[3])
            {
                if (Player.ZoneHallow)
                {
                    RegreUtils.SetBiomeTitle("The Hallow", Color.Pink, "Ethereal Wilds", 3);
                    if (!notFirstTimeEnteringBiome[3])
                    {
                        notFirstTimeEnteringBiome[3] = true;
                    }
                    inBiome[3] = true;
                }
            }
            else
            {
                if (!Player.ZoneHallow)
                {
                    inBiome[3] = false;
                }
            }


            if (!inBiome[4])
            {
                if (Player.ZoneGlowshroom)
                {
                    RegreUtils.SetBiomeTitle("Glowing Mushroom", Color.DarkBlue, "Glinting Haven", 4);
                    if (!notFirstTimeEnteringBiome[4])
                    {
                        notFirstTimeEnteringBiome[4] = true;
                    }
                    inBiome[4] = true;
                }
            }
            else
            {
                if (!Player.ZoneGlowshroom)
                {
                    inBiome[4] = false;
                }
            }


            if (!inBiome[5])
            {
                if (Player.ZoneNormalUnderground)
                {
                    RegreUtils.SetBiomeTitle("The Caves", Color.SaddleBrown, "Afflicted Depths", 5);
                    if (!notFirstTimeEnteringBiome[5])
                    {
                        notFirstTimeEnteringBiome[5] = true;
                    }
                    inBiome[5] = true;
                }
            }
            else
            {
                if (!Player.ZoneNormalUnderground)
                {
                    inBiome[5] = false;
                }
            }


            if (!inBiome[6])
            {
                if (Player.ZoneDungeon)
                {
                    RegreUtils.SetBiomeTitle("The Dungeon", Color.DarkGray, "Forgotten Tomb", 6);
                    if (!notFirstTimeEnteringBiome[6])
                    {
                        notFirstTimeEnteringBiome[6] = true;
                    }
                    inBiome[6] = true;
                }
            }
            else
            {
                if (!Player.ZoneDungeon)
                {
                    inBiome[6] = false;
                }
            }


            if (!inBiome[7])
            {
                if (Player.ZoneDesert)
                {
                    RegreUtils.SetBiomeTitle("The Desert", Color.Beige, "Scorched Wasteland", 7);
                    if (!notFirstTimeEnteringBiome[7])
                    {
                        notFirstTimeEnteringBiome[7] = true;
                    }
                    inBiome[7] = true;
                }
            }
            else
            {
                if (!Player.ZoneDesert)
                {
                    inBiome[7] = false;
                }
            }


            if (!inBiome[8])
            {
                if (Player.ZoneSnow)
                {
                    RegreUtils.SetBiomeTitle("The Tundra", Color.LightCyan, "Frozen Barrens", 8);
                    if (!notFirstTimeEnteringBiome[8])
                    {
                        notFirstTimeEnteringBiome[8] = true;
                    }
                    inBiome[8] = true;
                }
            }
            else
            {
                if (!Player.ZoneSnow)
                {
                    inBiome[8] = false;
                }
            }


            if (!inBiome[9])
            {
                if (Player.ZoneBeach)
                {
                    RegreUtils.SetBiomeTitle("The Ocean", Color.LightSkyBlue, "Grand Blue", 9);
                    if (!notFirstTimeEnteringBiome[9])
                    {
                        notFirstTimeEnteringBiome[9] = true;
                    }
                    inBiome[9] = true;
                }
            }
            else
            {
                if (!Player.ZoneBeach)
                {
                    inBiome[9] = false;
                }
            }


            if (!inBiome[10])
            {
                if (Player.ZoneGranite)
                {
                    RegreUtils.SetBiomeTitle("Granite", Color.DarkSlateBlue, "Crackling Geode", 10);
                    if (!notFirstTimeEnteringBiome[10])
                    {
                        notFirstTimeEnteringBiome[10] = true;
                    }
                    inBiome[10] = true;
                }
            }
            else
            {
                if (!Player.ZoneGranite)
                {
                    inBiome[10] = false;
                }
            }


            if (!inBiome[11])
            {
                if (Player.ZoneMarble)
                {
                    RegreUtils.SetBiomeTitle("Marble", Color.GhostWhite, "Haunted Corridors", 11);
                    if (!notFirstTimeEnteringBiome[11])
                    {
                        notFirstTimeEnteringBiome[11] = true;
                    }
                    inBiome[11] = true;
                }
            }
            else
            {
                if (!Player.ZoneMarble)
                {
                    inBiome[11] = false;
                }
            }


            if (!inBiome[12])
            {
                if (RegreSystem.TempleBricks > 30)
                {
                    RegreUtils.SetBiomeTitle("The Temple", Color.Yellow, "Sunlight Monastery", 12);
                    if (!notFirstTimeEnteringBiome[12])
                    {
                        notFirstTimeEnteringBiome[12] = true;
                    }
                    inBiome[12] = true;
                }
            }
            else
            {
                if (RegreSystem.TempleBricks < 30)
                {
                    inBiome[12] = false;
                }
            }


            if (!inBiome[14])
            {
                if (Player.ZoneNormalSpace)
                {
                    RegreUtils.SetBiomeTitle("Space", Color.Pink, "The Great Beyond", 14);
                    if (!notFirstTimeEnteringBiome[14])
                    {
                        notFirstTimeEnteringBiome[14] = true;
                    }
                    inBiome[14] = true;
                }
            }
            else
            {
                if (!Player.ZoneNormalSpace)
                {
                    inBiome[14] = false;
                }
            }


            if (!inBiome[15])
            {
                if (Player.ZoneMeteor)
                {
                    RegreUtils.SetBiomeTitle("The Meteor", Color.Pink, "Heaven's Fall", 15);
                    if (!notFirstTimeEnteringBiome[15])
                    {
                        notFirstTimeEnteringBiome[15] = true;
                    }
                    inBiome[15] = true;
                }
            }
            else
            {
                if (!Player.ZoneMeteor)
                {
                    inBiome[16] = false;
                }
            }
            if (!inBiome[16])
            {
                if (Player.ZoneUnderworldHeight)
                {
                    RegreUtils.SetBiomeTitle("The Underworld", Color.OrangeRed, "Lake of Fire", 16);
                    if (!notFirstTimeEnteringBiome[16])
                    {
                        notFirstTimeEnteringBiome[16] = true;
                    }
                    inBiome[16] = true;
                }
            }
            else
            {
                if (!Player.ZoneUnderworldHeight)
                {
                    inBiome[16] = false;
                }
            }
        }*/
        float voidDelay;
        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (ginnungagap && voidDelay <= 0)
                if (proj.DamageType == DamageClass.Magic && Player.ownedProjectileCounts[ModContent.ProjectileType<GinnungagapP>()] == 0)
                {
                    if (Main.rand.NextBool(10))
                    {
                        foreach (NPC npc in Main.npc)
                        {
                            if (npc.active && npc.knockBackResist != 0f && npc.Center.Distance(Player.Center) < 1020)
                                npc.velocity.Y -= 5f;
                        }
                        voidDelay = 350;
                        Projectile.NewProjectile(proj.GetSource_FromThis(), Player.Center - Vector2.UnitY * 250, Vector2.Zero, ModContent.ProjectileType<GinnungagapP>(), 50, 0, Player.whoAmI);
                    }
                }
        }

        public int flashTime;
        public int flashMaxTime;
        public Vector2 flashPosition;
        public void FlashScreen(Vector2 pos, int time)
        {
            flashMaxTime = time;
            flashTime = time;
            flashPosition = pos;
        }
        public override void PostUpdate()
        {
            // since the effect was working before
            if (flashTime > 0)
            {
                flashTime--;
                if (!Filters.Scene["Regressus:ScreenFlash"].IsActive())
                    Filters.Scene.Activate("Regressus:ScreenFlash", flashPosition);
                Filters.Scene["Regressus:ScreenFlash"].GetShader().UseProgress((float)Math.Sin((float)flashTime / flashMaxTime * Math.PI) * 2f);
                Filters.Scene["Regressus:ScreenFlash"].GetShader().UseTargetPosition(flashPosition); // already added it to luminary but gotta test alr a
            }
            else
            {
                if (Filters.Scene["Regressus:ScreenFlash"].IsActive())
                    Filters.Scene["Regressus:ScreenFlash"].Deactivate();
            }
            if (!Player.HasBuff(ModContent.BuffType<PilgrimBlindness>()))
                if (Filters.Scene["Regressus:Blindness"].IsActive())
                    Filters.Scene["Regressus:Blindness"].Deactivate();
            //BiomeCheck();
            if (voidDelay >= 1)
            {
                voidDelay--;
            }
            if (bossTextProgress > 0)
                bossTextProgress--;
            if (bossTextProgress == 0)
            {
                bossName = null;
                bossTitle = null;
                bossMaxProgress = 0;
                bossStyle = -1;
                bossColor = Color.White;
            }
            /*if (biomeTextProgress > 0)
                biomeTextProgress--;
            if (biomeTextProgress == 0)
            {
                biomeName = null;
                biomeTitle = null;
                biomeMaxProgress = 0;
                biomeColor = Color.White;
            }*/
            if (!reverseTime)
            {
                thing = 0;
                for (int num16 = oldCenter.Length - 1; num16 > 0; num16--)
                {
                    oldCenter[num16] = oldCenter[num16 - 1];
                }
                oldCenter[0] = Player.Center;
                for (int num16 = oldLife.Length - 1; num16 > 0; num16--)
                {
                    oldLife[num16] = oldLife[num16 - 1];
                }
                oldLife[0] = Player.statLife;
                for (int num16 = oldDir.Length - 1; num16 > 0; num16--)
                {
                    oldDir[num16] = oldDir[num16 - 1];
                }
                oldDir[0] = Player.direction;
            }
            else
            {
                thing++;
                if (thing < oldCenter.Length)
                {
                    if (oldCenter[thing] != Vector2.Zero)
                        Player.Center = oldCenter[thing];
                }
                if (thing < oldLife.Length)
                {
                    if (oldLife[thing] != 0)
                        Player.statLife = oldLife[thing];
                }
                if (thing < oldDir.Length)
                {
                    if (oldDir[thing] != 0)
                        Player.direction = oldDir[thing];
                }
            }
        }
    }
}
