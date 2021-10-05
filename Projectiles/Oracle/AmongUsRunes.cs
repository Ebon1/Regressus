using Terraria;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Regressus.NPCs.Bosses.Oracle;

namespace Regressus.Projectiles.Oracle
{
    public class AmongUsRunes : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rune");
            Main.projFrames[Projectile.type] = 10;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
            writer.Write(Projectile.localAI[1]);
        }


        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new(texture.Width * 0.5f, texture.Height * 0.5f);
            Vector2 drawPos = Projectile.position - Main.screenPosition + drawOrigin;
            Main.EntitySpriteDraw(texture, drawPos, texture.Frame(2, 10, (Projectile.localAI[0] == 420 ? 1 : 0), Projectile.frame), Color.White, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
            Projectile.localAI[1] = reader.ReadSingle();
        }
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.tileCollide = false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * ((255 - Projectile.alpha) / 255f);
        }
        public override void AI()
        {
            Projectile.rotation = 0;
            if (Projectile.localAI[0] == 69 || Projectile.localAI[0] == 420)
            {
                if (Projectile.ai[0] == 0)
                {
                    Projectile.frame = (int)Projectile.localAI[1];
                    Projectile.ai[0] = 1;
                }
            }
            else
            {
                if (Projectile.ai[0] == 0)
                {
                    Projectile.frame = Main.rand.Next(10);
                    Projectile.ai[0] = 1;
                }
            }
            Projectile.velocity *= 1.05f;
        }
    }
    public class AmongUsRunes2 : ModProjectile
    {
        public override string Texture => "Regressus/Projectiles/Oracle/AmongUsRunes";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rune");
            Main.projFrames[Projectile.type] = 10;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
            writer.Write(Projectile.localAI[1]);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new(texture.Width * 0.5f, texture.Height * 0.5f);
            Vector2 drawPos = Projectile.position - Main.screenPosition + drawOrigin;
            Main.EntitySpriteDraw(texture, drawPos, texture.Frame(2, 10, (Projectile.localAI[0] == 3 || Projectile.localAI[1] == 400 ? 1 : 0), Projectile.frame), Color.White, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
        private Vector2 angle;
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
            Projectile.localAI[1] = reader.ReadSingle();
        }
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.hostile = true;
            Projectile.netImportant = true;
        }
        private int timer = 0;
        private int thing = 0;
        private int timerBeforeDeath;
        public override void AI()
        {
            NPC center = Main.npc[(int)Projectile.ai[0]];
            if (!center.active || center.type != ModContent.NPCType<NPCs.Bosses.Oracle.OracleBoss>())
            {
                Projectile.Kill();
            }
            if (thing == 0)
            {
                Projectile.frame = Main.rand.Next(10);
                thing = 1;
            }
            Projectile.timeLeft = 2;
            if (Projectile.localAI[0] == 1 || Projectile.localAI[0] == 3)
            {
                timerBeforeDeath = (int)Projectile.localAI[1];
                Projectile.ai[1] += 2f * (float)Math.PI / 600 * -3;
                Projectile.ai[1] %= 2f * (float)Math.PI;
                Projectile.Center = OracleBoss.realNpcCenter + 150 * new Vector2((float)Math.Cos(Projectile.ai[1]), (float)Math.Sin(Projectile.ai[1]));
            }
            else
            {
                timerBeforeDeath = (int)Projectile.localAI[1];
                Projectile.ai[1] += 2f * (float)Math.PI / 600 * 3;
                Projectile.ai[1] %= 2f * (float)Math.PI;
                Projectile.Center = OracleBoss.realNpcCenter + 100 * new Vector2((float)Math.Cos(Projectile.ai[1]), (float)Math.Sin(Projectile.ai[1]));
            }
            if (++timer == timerBeforeDeath)
            {
                for (int i = 0; i < 200; i++)
                {
                    Player target = Main.player[i];
                    float shootToX = target.position.X + (float)target.width * 0.5f - Projectile.Center.X;
                    float shootToY = target.position.Y - Projectile.Center.Y;
                    if (Projectile.localAI[1] == 400)
                    {
                        angle = new Vector2((float)Math.Cos(Projectile.ai[1]), (float)Math.Sin(Projectile.ai[1])); ;
                        shootToX = angle.X;
                        shootToY = angle.Y;
                    }
                    float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

                    if (target.active)
                    {
                        distance = 3f / distance;
                        shootToX *= distance * 1.5f;
                        shootToY *= distance * 1.5f;
                        int proj = Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center.X, Projectile.Center.Y, shootToX, shootToY, ModContent.ProjectileType<AmongUsRunes>(), 20, Projectile.knockBack, Main.myPlayer, 0, 0);
                        Main.projectile[proj].timeLeft = 190;
                        if (Projectile.localAI[0] == 3)
                        {
                            Main.projectile[proj].localAI[0] = 420;
                        }
                        else if (Projectile.localAI[1] == 400)
                        {
                            Main.projectile[proj].localAI[0] = 420;
                        }
                        else
                        {
                            Main.projectile[proj].localAI[0] = 69;
                        }
                        Main.projectile[proj].localAI[1] = Projectile.frame;
                        Main.projectile[proj].netUpdate = true;
                        Main.projectile[proj].hostile = true;
                        Main.projectile[proj].friendly = false;
                        Projectile.netUpdate = true;
                        Projectile.Kill();
                    }
                }
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * ((255 - Projectile.alpha) / 255f);
        }
    }
}