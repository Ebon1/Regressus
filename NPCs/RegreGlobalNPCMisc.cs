using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics;
using Regressus.NPCs.Bosses.Oracle;
using Regressus.Items.Ammo;
using Regressus.Projectiles.Ranged;
using Regressus.Items.Weapons.Ranged;

namespace Regressus.NPCs
{
    public class RegreGlobalNPCShop : GlobalNPC
    {
        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.TravellingMerchant)
            {
                if (Main.rand.NextBool(10))
                {
                    shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Weapons.Melee.TheBaguette>());
                    nextSlot++;
                }
            }
        }
    }
    public class RegreGlobalNPCMisc : GlobalNPC
    {
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }
        public bool starshroomed;
        public bool hellshot;
        public override void ResetEffects(NPC npc)
        {
            starshroomed = false;
            hellshot = false;
        }
        public override void OnKill(NPC npc)
        {
            if (Main.hardMode && Main.LocalPlayer.RollLuck(200) == 0 && npc.HasPlayerTarget && npc.lifeMax > 5 && !npc.friendly && npc.value > 0f)
            {
                if ((double)(npc.position.Y / 16f) > (Main.rockLayer + (double)(Main.maxTilesY * 2)) / 3.0 && !Main.player[npc.target].ZoneDungeon && Main.rand.NextBool(2))
                {
                    Item.NewItem(npc.GetSource_DropAsItem(), npc.getRect(), ModContent.ItemType<Items.Weapons.Ranged.Guns.Hellshot>(), 1, noBroadcast: false);
                }
            }
            if (npc.type == NPCID.Bunny && Main.LocalPlayer.RollLuck(10) == 0)
            {
                Item.NewItem(npc.GetSource_DropAsItem(), npc.getRect(), ModContent.ItemType<Items.Consumables.Food.Carrot>(), 1, noBroadcast: false);
            }
        }
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.WallofFlesh)
            {
                IItemDropRule a = ItemDropRule.Common(ModContent.ItemType<Firestorm>(), 4, 100, 500);
                npcLoot.Add(a);
            }
        }
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (starshroomed)
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                npc.lifeRegen -= 23;
                if (damage < 8)
                {
                    damage = 8;
                }
            }
            if (hellshot)
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                npc.lifeRegen -= 40;
                damage = 1;
            }
        }
        /*
        Vector2[] oldCenter = new Vector2[250], oldVelocity = new Vector2[250];
        int[] oldLife = new int[250];
        float[] oldRotation = new float[250];
        public bool reverseTime;
        int thing;
        public override void ResetEffects(NPC npc)
        {
            reverseTime = false;
        }
        public override void AI(NPC npc)
        {
            if (!reverseTime)
                thing = 0;
            for (int num16 = oldCenter.Length - 1; num16 > 0; num16--)
            {
                oldCenter[num16] = oldCenter[num16 - 1];
            }
            oldCenter[0] = npc.Center;
            for (int num16 = oldVelocity.Length - 1; num16 > 0; num16--)
            {
                oldVelocity[num16] = oldVelocity[num16 - 1];
            }
            oldVelocity[0] = npc.velocity;
            for (int num16 = oldLife.Length - 1; num16 > 0; num16--)
            {
                oldLife[num16] = oldLife[num16 - 1];
            }
            oldLife[0] = npc.life;
            for (int num16 = oldRotation.Length - 1; num16 > 0; num16--)
            {
                oldRotation[num16] = oldRotation[num16 - 1];
            }
            oldRotation[0] = npc.rotation;
            base.AI(npc);
        }
        public override bool PreAI(NPC npc)
        {
            if (reverseTime)
            {
                thing++;
                Main.NewText(thing);
                if (thing < oldCenter.Length)
                {
                    if (oldCenter[thing] != Vector2.Zero)
                        npc.Center = oldCenter[thing];
                }
                if (thing < oldRotation.Length)
                {
                    npc.rotation = oldRotation[thing];
                }
                if (thing < oldLife.Length)
                {
                    if (oldLife[thing] != 0)
                        npc.life = oldLife[thing];
                }
                /*for (int k = 0; k < oldVelocity.Length; k++)
                {
                    if (oldVelocity[k] != Vector2.Zero)
                        continue;
                    npc.velocity = oldVelocity[k];
                }*/
        /*for (int k = oldVelocity.Length; k > -1; k--)
        {
            if (oldVelocity[k] != Vector2.Zero)
                continue;
            npc.velocity = oldVelocity[k];
        }*/
        /*for (int k = oldLife.Length; k > -1; k--)
        {
            if (oldLife[k] != 0)
                continue;
            npc.life = oldLife[k];
        }
        return false;
    }
    return base.PreAI(npc);
}
*/
    }
}
