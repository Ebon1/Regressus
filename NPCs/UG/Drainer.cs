using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using ReLogic.Content;
using Terraria.GameContent.Bestiary;

namespace Regressus.NPCs.UG
{
    public class Drainer : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Leech Lich");
            Main.npcFrameCount[Type] = 6;
        }
        public override Color? GetAlpha(Color drawColor) => Color.White;
        public override void SetDefaults()
        {
            NPC.CloneDefaults(ModContent.NPCType<Apparition>());
            NPC.lifeMax = 65;
            NPC.Size = new Vector2(76, 96);
            NPC.dontTakeDamage = false;
            NPC.aiStyle = 0;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneRockLayerHeight && spawnInfo.Player.ZonePurity)
                return 0.1f;
            return 0;
        }
        public override void FindFrame(int frameHeight)
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
            else if (NPC.frameCounter < 25)
                NPC.frame.Y = 4 * frameHeight;
            else if (NPC.frameCounter < 30)
                NPC.frame.Y = 5 * frameHeight;
            else
                NPC.frameCounter = 0;
        }
        public override void OnSpawn(IEntitySource source)
        {
            NPC.ai[1] = 6;
            NPC.ai[2] = 1;
        }
        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            float _mult = (float)((float)NPC.life / NPC.lifeMax);
            float mult = MathHelper.Lerp(2, 1, _mult);
            NPC.ai[2] = MathHelper.Lerp(2f, .5f, (float)(Math.Sin(Main.GlobalTimeWrappedHourly) / 2) + 1);
            NPC.ai[1] = MathHelper.Lerp(3, 9, (float)(Math.Sin(Main.GlobalTimeWrappedHourly) / 2) + 1);
            if (player.Center.Distance(NPC.Center) > (16 * 5))
                NPC.velocity = RegreUtils.FromAToB(NPC.Center, player.Center, true) * 2.5f * mult * NPC.ai[2];
            else
                NPC.velocity = Vector2.Zero;
            if (player.Center.Distance(NPC.Center) < (16 * NPC.ai[1]))
            {
                player.Hurt(PlayerDeathReason.ByCustomReason(player.name + "'s life got sucked out of " + (player.Male ? "him." : "her.")), 1, 0, quiet: true);
                if (player.statLife > 5)
                    player.statLife--;
                if (NPC.life < NPC.lifeMax)
                {
                    NPC.life++;
                    CombatText.NewText(NPC.getRect(), CombatText.HealLife, 1);
                }
                Vector2 vel = RegreUtils.FromAToB(player.Center, NPC.Center - Vector2.UnitY * 15, true) * 8;
                Dust d = Dust.NewDustPerfect(player.Center, DustID.LifeDrain, vel);
                d.noGravity = true;
                d.scale = 2;
            }
            for (int i = 0; i < 32; i++)
            {
                float angle = 2f * (float)Math.PI / 32 * i;
                Vector2 pos = NPC.Center + (16 * NPC.ai[1]) * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                Dust d = Dust.NewDustDirect(pos, 1, 1, DustID.LifeDrain);
                d.noGravity = true;
                d.scale = 0.5f;
            }
        }
    }
}