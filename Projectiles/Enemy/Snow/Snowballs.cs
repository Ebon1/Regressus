using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace Regressus.Projectiles.Enemy.Snow
{
    public class MrBallHat : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.netImportant = true;
        }
        float rot = 1;
        public override void AI()
        {
            NPC center = Main.npc[(int)Projectile.ai[0]];
            if (center.type != ModContent.NPCType<NPCs.Snow.SnowballWisp>() || !center.active)
            {
                Projectile.ai[1] = 1;
            }
            Projectile.timeLeft = 5;
            if (Projectile.ai[1] == 0)
            {
                Projectile.Center = center.Center - new Vector2(-12 * center.direction, 18).RotatedBy(center.rotation);
                Projectile.rotation = center.rotation;
                Projectile.direction = Projectile.spriteDirection = center.direction;
            }
            else
            {
                Projectile.tileCollide = true;
                rot += 0.015f;
                Projectile.rotation += 0.1f;
                Projectile.velocity = new Vector2((float)Math.Sin(rot * 5) * rot * 2, 2.55f);
            }
        }

    }
    public class Snowball1 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.hostile = true;
            Projectile.netImportant = true;
        }
        public override void AI()
        {
            if (Main.rand.NextFloat() < 0.11627907f)
            {
                Dust dust;
                Vector2 position = Projectile.Center;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width / 2, Projectile.height / 2, DustID.SnowBlock, 0f, 0f, 0, new Color(255, 255, 255), 1.3953489f / 2)];
            }

            NPC center = Main.npc[(int)Projectile.ai[0]];
            if (!center.active || center.type != ModContent.NPCType<NPCs.Snow.SnowballWisp>())
            {
                Projectile.Kill();
            }
            Projectile.timeLeft = 2;
            if (Projectile.localAI[0] == 0)
            {
                Projectile.ai[1] += 2f * (float)Math.PI / 600f * 3f;
                Projectile.ai[1] %= 2f * (float)Math.PI;
                Projectile.Center = center.Center + 64 * new Vector2((float)Math.Cos(Projectile.ai[1]), (float)Math.Sin(Projectile.ai[1]));
            }
            else
            {
                Projectile.tileCollide = true;

            }
        }
    }
    public class Snowball2 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.hostile = true;
            Projectile.netImportant = true;
        }
        public override void AI()
        {
            if (Main.rand.NextFloat() < 0.11627907f)
            {
                Dust dust;
                Vector2 position = Projectile.Center;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width / 2, Projectile.height / 2, DustID.SnowBlock, 0f, 0f, 0, new Color(255, 255, 255), 0.25f / 2)];
            }

            NPC center = Main.npc[(int)Projectile.ai[0]];
            if (!center.active || center.type != ModContent.NPCType<NPCs.Snow.SnowballWisp>())
            {
                Projectile.Kill();
            }
            Projectile.timeLeft = 2;
            if (Projectile.localAI[0] == 0)
            {
                Projectile.ai[1] += 2f * (float)Math.PI / 600f * 3f;
                Projectile.ai[1] %= 2f * (float)Math.PI;
                Projectile.Center = center.Center + 64 * new Vector2((float)Math.Cos(Projectile.ai[1]), (float)Math.Sin(Projectile.ai[1]));
            }
            else
            {
                Projectile.tileCollide = true;

            }
        }
    }
    public class Snowball3 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.hostile = true;
            Projectile.netImportant = true;
        }
        public override void AI()
        {
            if (Main.rand.NextFloat() < 0.11627907f)
            {
                Dust dust;
                Vector2 position = Projectile.Center;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width / 2, Projectile.height / 2, DustID.SnowBlock, 0f, 0f, 0, new Color(255, 255, 255), 0.56f / 2)];
            }

            NPC center = Main.npc[(int)Projectile.ai[0]];
            if (!center.active || center.type != ModContent.NPCType<NPCs.Snow.SnowballWisp>())
            {
                Projectile.Kill();
            }
            Projectile.timeLeft = 2;
            if (Projectile.localAI[0] == 0)
            {
                Projectile.ai[1] += 2f * (float)Math.PI / 600f * 3f;
                Projectile.ai[1] %= 2f * (float)Math.PI;
                Projectile.Center = center.Center + 64 * new Vector2((float)Math.Cos(Projectile.ai[1]), (float)Math.Sin(Projectile.ai[1]));
            }
            else
            {
                Projectile.tileCollide = true;

            }
        }
    }
}
