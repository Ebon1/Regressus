using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Regressus.NPCs.Overworld;

namespace Regressus.Projectiles.Enemy.Overworld
{
    public class TerraKnightSword : ModProjectile
    {
        public override string Texture => "Regressus/Items/Weapons/Melee/EarthDivider";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terragrimoire");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 25;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 82;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        Vector2 pos;
        float factor;
        public float Lerp(float x)
        {
            return (float)(x < 0.5
  ? (1 - Math.Sqrt(1 - Math.Pow(2 * x, 2))) / 2
  : (Math.Sqrt(1 - Math.Pow(-2 * x + 2, 2)) + 1) / 2);
        }
        int SwingTime = 37;
        int a, b;
        /*public override void OnSpawn(IEntitySource source)
        {
            NPC npc = Main.npc[(int)Projectile.ai[0]];
            Projectile.Center = npc.Center - Vector2.UnitY * 70;
        }*/
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            /*
            NPC npc = Main.npc[(int)Projectile.ai[0]];
            Projectile.timeLeft = 2;
            Projectile.spriteDirection = a;
            if (!npc.active || npc.type != ModContent.NPCType<TerraKnight>())
                Projectile.Kill();
            Vector2 moveTo = pos - Projectile.Center;
            if (++b < 60)
            {
                Projectile.Center = npc.Center - Vector2.UnitY * 100;
            }
            if (npc.Center.Distance(Main.player[npc.target].Center) < 750f)
            {
                if (++a < 80)
                {
                    Projectile.velocity *= 0.96f;
                    Projectile.rotation += MathHelper.ToRadians(10f);
                }
                else if (a == 80)
                {
                    Projectile.rotation = (Main.player[npc.target].Center - Projectile.Center).ToRotation() + MathHelper.PiOver4;
                    Vector2 c = (Main.player[npc.target].Center - Projectile.Center);
                    c.Normalize();
                    Projectile.velocity = c * 15f;
                }
                else if (a >= 100)
                {
                    a = 0;
                }
                /*Projectile.ai[1]--;
                    if (Projectile.ai[1] <= 0)
                    {
                        a = -a;
                        Projectile.ai[1] = SwingTime;
                    }
                    float start = Projectile.velocity.ToRotation() - ((MathHelper.PiOver2) - 0.2f);
                    float end = Projectile.velocity.ToRotation() + ((MathHelper.PiOver2) - 0.2f);
                    float progress = Lerp(Utils.GetLerpValue(0f, SwingTime, Projectile.ai[1]));
                    float rot = a == 1 ? start.AngleLerp(end, progress) : start.AngleLerp(end, 1f - progress);
                    Vector2 pos = Projectile.Center;
                    pos += rot.ToRotationVector2();
                    Projectile.rotation = (pos - moveTo).ToRotation() + MathHelper.PiOver4;
            }
            else
            {
                pos = npc.Center - Vector2.UnitY * 100;
                factor = .21f;
                Projectile.ai[1] = SwingTime;
                Projectile.velocity = moveTo * factor;
                a = 0;
            }*/
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.localAI[0]++;
            if (Projectile.localAI[0] > 0)
            {
                Texture2D glow = ModContent.Request<Texture2D>("Regressus/Projectiles/Melee/EarthDivider_Glow").Value;
                Player player = Main.player[Projectile.owner];
                RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
                var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
                for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
                {
                    if (i == Projectile.localAI[0])
                        continue;
                    Main.spriteBatch.Draw(glow, Projectile.oldPos[i] - Main.screenPosition + new Vector2(Projectile.width / 2f, Projectile.height / 2f), new Rectangle(0, 0, glow.Width, glow.Height), new Color(0, 255, Main.DiscoB) * (1f - fadeMult * i) * 0.75f, Projectile.oldRot[i] + (a == 1 ? 0 : MathHelper.PiOver2 * 3), glow.Size() / 2, Projectile.scale * (ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type], a == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
                }

                RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
                Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation + (a == 1 ? 0 : MathHelper.PiOver2 * 3), texture.Size() / 2, Projectile.scale, a == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            }
            return false;
        }
    }
}
