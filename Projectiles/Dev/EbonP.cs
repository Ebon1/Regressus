using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using ReLogic.Content;
using Regressus.Effects.Prims;
using Regressus.Projectiles.Melee;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using System.IO;

namespace Regressus.Projectiles.Dev
{
    public class EbonP1 : ModProjectile
    {
        public override string Texture => "Regressus/Extras/Sprites/Exol";
        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(134, 148);
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
        }
        bool a;
        float mult;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D b = RegreUtils.GetExtraTexture("Sprites/Exol");
            Texture2D c = RegreUtils.GetExtraTexture("Sprites/Exol_Glow");
            Main.EntitySpriteDraw(b, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, Projectile.width, Projectile.height), lightColor * mult, Projectile.rotation, Projectile.Size / 2, 1, SpriteEffects.None, 0);
            sb.Reload(BlendState.Additive);
            Main.EntitySpriteDraw(c, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, Projectile.width, Projectile.height), Color.White * mult, Projectile.rotation, Projectile.Size / 2, 1, SpriteEffects.None, 0);
            sb.Reload(BlendState.AlphaBlend);
            return false;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.velocity = RegreUtils.FromAToB(Projectile.Center, player.Center - Vector2.UnitY * 250, false) * 0.018f;
            if (!a)
            {
                for (int i = 0; i < 8; i++)
                {
                    float angle = 2f * (float)Math.PI / 8f * i;
                    Vector2 pos = Projectile.Center + 130 * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                    Projectile poop = Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), pos, Vector2.Zero, ModContent.ProjectileType<EbonP1_2>(), Projectile.damage, Projectile.knockBack, Projectile.owner, angle, Projectile.whoAmI);
                    if (i != 0)
                        poop.localAI[0] = i;
                    else
                        poop.localAI[0] = 0.5f;
                }
                a = true;
            }

            float progress = Utils.GetLerpValue(0, 600, Projectile.timeLeft);
            mult = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 10, 0, 1);
        }
    }
    public class EbonP1_2 : ModProjectile
    {
        public override string Texture => "Regressus/Extras/Sprites/Exol1";
        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(50, 50);
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 500;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
            writer.Write(Projectile.localAI[1]);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
            Projectile.localAI[1] = reader.ReadSingle();
        }
        bool a;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile owner = Main.projectile[(int)Projectile.ai[1]];
            if (Projectile.timeLeft > 50 * Projectile.localAI[0] && !a)
            {
                Projectile.ai[0] += 2f * (float)Math.PI / 600f * 10;
                Projectile.ai[0] %= 2f * (float)Math.PI;
                Projectile.Center = owner.Center + 130 * new Vector2((float)Math.Cos(Projectile.ai[0]), (float)Math.Sin(Projectile.ai[0]));
            }
            else
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
                if (!a)
                {
                    Projectile.velocity = RegreUtils.FromAToB(Projectile.Center, Main.MouseWorld) * 25f;
                    Projectile.timeLeft = 500;
                    a = true;
                }
            }
        }
    }
    public class EbonP2 : ModProjectile
    {
        public override string Texture => "Regressus/Extras/Sprites/Exol";
    }
    public class EbonP3 : ModProjectile
    {
        public override string Texture => "Regressus/Extras/Sprites/Exol";
    }
    public class EbonP4 : ModProjectile
    {
        public override string Texture => "Regressus/Extras/Sprites/Exol";
    }
    public class EbonP5 : ModProjectile
    {
        public override string Texture => "Regressus/Extras/Sprites/Exol";
    }
    public class EbonP6 : ModProjectile
    {
        public override string Texture => "Regressus/Extras/Sprites/Exol";
    }
}
