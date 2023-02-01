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
        public bool starmycel;
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
            CantEatBaguette = false;
            hasEncounteredMoth = false;
            AcornFairyPet = false;
            ginnungagapHide = false;
            bladeSummon = false;
            starmycel = false;

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
        /*public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            /*for (int i = 0; i < notFirstTimeEnteringBiome.Length; i++)
            {
                packet.Write(notFirstTimeEnteringBiome[i]);
            }
            */
        /* for (int i = 0; i < AerialBudItem.Length; i++)
         {
             packet.Write(AerialBudItem[i]);
             packet.Write(AerialBudItemStack[i]);
         }
         packet.Write(AerialBudsGiven);
         packet.Write(hasEncounteredMoth);
         packet.Write(CantEatBaguette);*/


        /*packet.Send(toWho, fromWho);
    }*/
        public override void SaveData(TagCompound tag)
        {
            //for (int i = 0; i < notFirstTimeEnteringBiome.Length; i++)
            //    tag.Set("Biome" + i, notFirstTimeEnteringBiome[i]);
            var boolList = tag.GetList<string>("boolList");
            /*for (int i = 0; i < AerialBudItem.Length; i++)
            {
                tag.Set("ItemBud" + i, AerialBudItem[i]);
                tag.Set("StackBud" + i, AerialBudItemStack[i]);
            }
            tag.Set("Buds", AerialBudsGiven);*/
            //tag.Set("Moth", hasEncounteredMoth);
            //tag.Set("Baguette", CantEatBaguette);

            if (hasEncounteredMoth)
                boolList.Add("Moth");
            if (CantEatBaguette)
                boolList.Add("Baguette");
            tag["boolList"] = boolList;
        }
        public override void LoadData(TagCompound tag)
        {
            //for (int i = 0; i < notFirstTimeEnteringBiome.Length; i++)
            //    notFirstTimeEnteringBiome[i] = tag.GetBool("Biome" + i);
            /*for (int i = 0; i < AerialBudItem.Length; i++)
            {
                AerialBudItem[i] = tag.GetInt("ItemBud" + i);
                AerialBudItemStack[i] = tag.GetInt("StackBud" + i);
            }
            AerialBudsGiven = tag.GetInt("Buds");*/
            //hasEncounteredMoth = tag.GetBool("Moth");
            //CantEatBaguette = tag.GetBool("Baguette");

            var boolList = tag.GetList<string>("boolList");
            CantEatBaguette = boolList.Contains("Baguette");
            hasEncounteredMoth = boolList.Contains("Moth");
        }
        int starladDelay=80*60;
        public override void OnHitByNPC(NPC npc, int damage, bool crit)
        {
            if (starmycel && starladDelay >= 80 * 60)
            {
             
                for(float y = -2; y < 2; y++)
                {
                    Projectile.NewProjectileDirect(Player.GetSource_FromThis(),this.Player.position-new Vector2(150f,120*y),new Vector2(5f,0.8f), ModContent.ProjectileType<StarladProjectile>(),12,3f);
                }
                starladDelay = 0;
            }
        }

        



        float voidDelay;

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (starmycel && proj.minion)
            {
                damage += Math.Max(1, damage / 10);
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (ginnungagap && voidDelay <= 0)
            {
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


            if (starmycel)
            {
                starladDelay++;
                Player.manaRegenBonus += Math.Max(1, Player.manaRegenBonus/10);
            }

            if (bossTextProgress > 0)
                bossTextProgress--;
            if (bossTextProgress == 0 && bossMaxProgress != 0)
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
