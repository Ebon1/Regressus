using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Regressus.Projectiles.Ranged;

namespace Regressus.Projectiles.SSW
{
    public class BigAssBall : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 146;
            Projectile.height = 148;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.hostile = true;
            Projectile.netImportant = true;
            Projectile.timeLeft = 340;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
            writer.Write(Projectile.localAI[1]);
            writer.Write(prog);
        }

        float prog;
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
            Projectile.localAI[1] = reader.ReadSingle();
            prog = reader.ReadSingle();
        }
        public override void AI()
        {
            NPC center = Main.npc[(int)Projectile.ai[0]];
            if (!center.active || center.type != ModContent.NPCType<NPCs.Bosses.Starshroom.StarshroomWitch>())
            {
                Projectile.Kill();
            }
            Player player = Main.player[center.target];
            if (Projectile.timeLeft % 30 == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    float angle = RegreUtils.CircleDividedEqually(i, 3);
                    Vector2 vel = Main.rand.NextVector2Unit().RotatedBy(angle) * 15;
                    Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, vel, ModContent.ProjectileType<StarshroomDustP>(), Projectile.damage, 0, player.whoAmI);
                    p.friendly = false;
                    p.hostile = true;
                }
            }
            if (Projectile.timeLeft > 40)
            {
                prog = MathHelper.Clamp(prog + 0.05f, 0, 6);
                Projectile.rotation += MathHelper.ToRadians(prog);
                Projectile.ai[1] += 2f * (float)Math.PI / 200f * 1 * prog;
                Projectile.ai[1] %= 2f * (float)Math.PI;
                Projectile.Center = Vector2.Lerp(Projectile.Center, center.Center + 400f * new Vector2((float)Math.Cos(Projectile.ai[1]), (float)Math.Sin(Projectile.ai[1])), 0.025f * prog);
            }
            else
            {
                Projectile.velocity = Vector2.Zero;
                Projectile.Center = Vector2.Lerp(Projectile.Center, center.Center, 0.1f);
                Projectile.scale = MathHelper.Lerp(Projectile.scale, 0, 0.1f);
            }
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCs.Add(index);
        }
        public override bool PreDraw(ref Color drawColor)
        {
            NPC npc = Main.npc[(int)Projectile.ai[0]];
            Vector2 neckOrigin = npc.Center;
            Vector2 center = Projectile.Center;
            Vector2 distToProj = neckOrigin - Projectile.Center;
            float projRotation = distToProj.ToRotation() - 1.57f;
            float distance = distToProj.Length();
            while (distance > 6 && !float.IsNaN(distance))
            {
                distToProj.Normalize();
                distToProj *= 6;
                center += distToProj;
                distToProj = neckOrigin - center;
                distance = distToProj.Length();

                //Draw chain
                Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Projectiles/SSW/BallChain").Value, center - Main.screenPosition,
                    new Rectangle(0, 0, 18, 6), Lighting.GetColor((int)center.X / 16, (int)center.Y / 16), projRotation,
                    new Vector2(18 * 0.5f, 6 * 0.5f), 1f, SpriteEffects.None, 0);
            }
            return true;

        }
    }
}
