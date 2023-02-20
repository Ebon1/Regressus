using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using IL.Terraria;
using On.Terraria;
using Main = Terraria.Main;

namespace Regressus.Projectiles
{
    public class StarladProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pebble");
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 12;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 500;
            Projectile.tileCollide = true;
        }
        
        private const int TimerSlot = 1;
        public float AITimer
        {
            get => Projectile.ai[TimerSlot];
            set => Projectile.ai[TimerSlot] = value;
        }
        public override void AI()
        {
            Vector2 move = Vector2.Zero;
            float distance = 200f;
            bool target = false;

            AITimer++;
            if (AITimer < 60)
            {
                Projectile.Center = Vector2.Lerp(Projectile.Center, new Vector2(Projectile.Center.X, Projectile.Center.Y), AITimer / 60);
            }
            if (AITimer > 60 && AITimer < 125)
            {
                for (int num622 = 0; num622 < 4; num622++)
                {
                    //Dust d = Main.dust[Dust.NewDust(Projectile.Center, Projectile.width / 3, Projectile.height / 3, 57, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 150, default, 1.2f)];
                    //d.noGravity = true;
                }
            }
            if (AITimer == 60)
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item9, Projectile.Center);
                Projectile.velocity.X *= 0.98f;
                Vector2 vector9 = new Vector2(Projectile.position.X + Projectile.width * 0.5f, Projectile.position.Y + Projectile.height * 0.5f);
                {
                    for (int k = 0; k < 200; k++)
                    {
                        if (!(!Main.npc[k].active || Main.npc[k].dontTakeDamage || Main.npc[k].friendly))
                        {
                            Vector2 newMove = Main.npc[k].Center - Projectile.Center;
                            float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                            if (distanceTo < distance)
                            {
                                move = newMove;
                                distance = distanceTo;
                                target = true;
                            }
                        }
                    }
                }
                if (target)
                {
                    AdjustMagnitude(ref move);
                    Projectile.velocity = (10 * Projectile.velocity + move) / 11f;
                    AdjustMagnitude(ref Projectile.velocity);
                }
            }

            if (AITimer == 145)
            {
                Projectile.velocity = Vector2.Zero;
            }
            if (AITimer >= 185)
                AITimer = 0;
        }
        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 15f)
            {
                vector *= 15f / magnitude;
            }
        }
    }
}