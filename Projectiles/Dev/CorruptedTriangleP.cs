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
using Terraria.Audio;
using System.IO;
using Terraria.Graphics.Shaders;


namespace Regressus.Projectiles.Dev
{
    public class CorruptedTriangleP1 : ModProjectile
    {
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
        public override string Texture => "Regressus/Extras/Sprites/RollHand1";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Corrupted Triangle");
            Main.projFrames[Type] = 2;
        }
        public override bool ShouldUpdatePosition() => false;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            SoundStyle hit = new SoundStyle("Regressus/Sounds/Custom/RollHit");
            hit.Volume = 0.25f;
            SoundEngine.PlaySound(hit, target.Center);
        }
        public override void SetDefaults()
        {
            Projectile.height = Projectile.width = 20;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 30;

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
                Dust.NewDustDirect(Projectile.Center, 5, 5, ModContent.DustType<Dusts.RollParticle>(), -Projectile.velocity.RotatedBy(Math.PI / (Projectile.localAI[0] / 5)).X / 5, -Projectile.velocity.RotatedBy(Math.PI / (Projectile.localAI[0] / 5)).Y / 5);
        }

        public override void AI()
        {
            if (Projectile.timeLeft > 25)
                Projectile.frame = Main.rand.Next(2);
            else
                Projectile.frame = 0;
            Player player = Main.player[Projectile.owner];
            Projectile.Center = player.Center + Projectile.velocity / 2;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.ai[0]++;
            if (Projectile.ai[0] == 3)
            {
                Projectile p = Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), new Vector2(Projectile.position.X + Projectile.velocity.X + (float)(Projectile.width / 2), Projectile.position.Y + Projectile.velocity.Y + (float)(Projectile.height / 2)), Projectile.velocity, ModContent.ProjectileType<CorruptedTriangleP2>(), Projectile.damage + 10, Projectile.knockBack, Projectile.owner, 0, Projectile.whoAmI);
                p.localAI[0] = Projectile.localAI[0];
            }
        }
    }
    public class CorruptedTriangleP2 : ModProjectile
    {
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
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            SoundStyle hit = new SoundStyle("Regressus/Sounds/Custom/RollHit");
            hit.Volume = 0.25f;
            SoundEngine.PlaySound(hit, target.Center);
        }
        public override string Texture => "Regressus/Extras/Sprites/RollHand2";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Corrupted Triangle");
            Main.projFrames[Type] = 2;
        }
        public override bool ShouldUpdatePosition() => false;
        public override void SetDefaults()
        {
            Projectile.height = Projectile.width = 40;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 30;

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }


        public override void AI()
        {
            if (Projectile.timeLeft > 25)
                Projectile.frame = Main.rand.Next(2);
            else
                Projectile.frame = 0;
            Projectile owner = Main.projectile[(int)Projectile.ai[1]];
            if (Projectile.timeLeft < 10)
            {
                Projectile.damage = 0;
                if (Projectile.localAI[0] > 0)
                    Projectile.localAI[0] -= 1.5f;
                else
                    Projectile.localAI[0] += 1.5f;
            }
            Projectile.Center = owner.position + Projectile.velocity.RotatedBy(Math.PI / (Projectile.localAI[0] / 2)) + ((owner.Size / 2));
            Projectile.rotation = Projectile.velocity.RotatedBy(Math.PI / (Projectile.localAI[0] / 2)).ToRotation() + MathHelper.PiOver2;
            Projectile.ai[0]++;
            if (Projectile.ai[0] == 3)
            {
                Projectile p = Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), new Vector2(Projectile.position.X + Projectile.velocity.X + (float)(Projectile.width / 2), Projectile.position.Y + Projectile.velocity.Y + (float)(Projectile.height / 2)), Projectile.velocity, ModContent.ProjectileType<CorruptedTriangleP3>(), Projectile.damage + 10, Projectile.knockBack, Projectile.owner, 0, Projectile.whoAmI);
                p.localAI[0] = Projectile.localAI[0];
            }
        }
    }
    public class CorruptedTriangleP3 : ModProjectile
    {
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
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            SoundStyle hit = new SoundStyle("Regressus/Sounds/Custom/RollHit");
            hit.Volume = 0.25f;
            SoundEngine.PlaySound(hit, target.Center);
        }
        public override string Texture => "Regressus/Extras/Sprites/RollHand3";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Corrupted Triangle");
            Main.projFrames[Type] = 2;
        }
        public override bool ShouldUpdatePosition() => false;
        public override void SetDefaults()
        {
            Projectile.height = Projectile.width = 36;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 30;

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }


        public override void AI()
        {
            if (Projectile.timeLeft > 25)
                Projectile.frame = Main.rand.Next(2);
            else
                Projectile.frame = 0;
            if (Projectile.timeLeft < 10)
            {
                Projectile.damage = 0;
                if (Projectile.localAI[0] > 0)
                    Projectile.localAI[0] -= 1.5f;
                else
                    Projectile.localAI[0] += 1.5f;
            }
            Projectile owner = Main.projectile[(int)Projectile.ai[1]];
            Projectile.Center = owner.position + Projectile.velocity.RotatedBy(Math.PI / (Projectile.localAI[0] / 3)) + ((owner.Size / 2));
            Projectile.rotation = Projectile.velocity.RotatedBy(Math.PI / (Projectile.localAI[0] / 3)).ToRotation() + MathHelper.PiOver2;
            Projectile.ai[0]++;
            if (Projectile.ai[0] == 3)
            {
                Projectile p = Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), new Vector2(Projectile.position.X + Projectile.velocity.X + (float)(Projectile.width / 2), Projectile.position.Y + Projectile.velocity.Y + (float)(Projectile.height / 2)), Projectile.velocity, ModContent.ProjectileType<CorruptedTriangleP4>(), Projectile.damage + 10, Projectile.knockBack, Projectile.owner, 0, Projectile.whoAmI);
                p.localAI[0] = Projectile.localAI[0];
            }
        }
    }
    public class CorruptedTriangleP4 : ModProjectile
    {
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
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            SoundStyle hit = new SoundStyle("Regressus/Sounds/Custom/RollHit");
            hit.Volume = 0.25f;
            SoundEngine.PlaySound(hit, target.Center);
        }
        public override string Texture => "Regressus/Extras/Sprites/RollHand4";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Corrupted Triangle");
            Main.projFrames[Type] = 2;
        }
        public override bool ShouldUpdatePosition() => false;
        public override void SetDefaults()
        {
            Projectile.height = Projectile.width = 52;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 30;

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void AI()
        {
            if (Projectile.timeLeft > 25)
                Projectile.frame = Main.rand.Next(2);
            else
                Projectile.frame = 0;
            if (Projectile.timeLeft < 10)
            {
                Projectile.damage = 0;
                if (Projectile.localAI[0] > 0)
                    Projectile.localAI[0] -= 1.5f;
                else
                    Projectile.localAI[0] += 1.5f;
            }
            Projectile owner = Main.projectile[(int)Projectile.ai[1]];
            Projectile.Center = owner.position + Projectile.velocity.RotatedBy(Math.PI / (Projectile.localAI[0] / 4)) + ((owner.Size / 2));
            Projectile.rotation = Projectile.velocity.RotatedBy(Math.PI / (Projectile.localAI[0] / 4)).ToRotation() + MathHelper.PiOver2;
            Projectile.ai[0]++;
            if (Projectile.ai[0] == 3)
            {
                Projectile p = Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), new Vector2(Projectile.position.X + Projectile.velocity.X + (float)(Projectile.width / 2), Projectile.position.Y + Projectile.velocity.Y + (float)(Projectile.height / 2)), Projectile.velocity, ModContent.ProjectileType<CorruptedTriangleP5>(), Projectile.damage + 10, Projectile.knockBack, Projectile.owner, 0, Projectile.whoAmI);
                p.localAI[0] = Projectile.localAI[0];
            }
        }
    }
    public class CorruptedTriangleP5 : ModProjectile
    {
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
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            SoundStyle hit = new SoundStyle("Regressus/Sounds/Custom/RollHit");
            hit.Volume = 0.25f;
            SoundEngine.PlaySound(hit, target.Center);
            for (int i = 0; i < 5; i++)
                Dust.NewDustDirect(Projectile.Center, 5, 5, ModContent.DustType<Dusts.RollParticle>(), Projectile.velocity.X / 5, Projectile.velocity.Y / 5);
        }
        public override string Texture => "Regressus/Extras/Sprites/RollHand5";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Corrupted Triangle");
            Main.projFrames[Type] = 2;
        }
        public override bool ShouldUpdatePosition() => false;
        public override void SetDefaults()
        {
            Projectile.height = Projectile.width = 80;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 30;

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
                Dust.NewDustDirect(Projectile.Center, 5, 5, ModContent.DustType<Dusts.RollParticle>(), -Projectile.velocity.RotatedBy(Math.PI / (Projectile.localAI[0] / 5)).X / 5, -Projectile.velocity.RotatedBy(Math.PI / (Projectile.localAI[0] / 5)).Y / 5);
        }

        public override void AI()
        {
            if (Projectile.timeLeft > 25)
                Projectile.frame = Main.rand.Next(2);
            else
                Projectile.frame = 0;
            if (Projectile.timeLeft < 10)
            {
                Projectile.damage = 0;
                if (Projectile.localAI[0] > 0)
                    Projectile.localAI[0] -= 1.5f;
                else
                    Projectile.localAI[0] += 1.5f;
            }
            Projectile owner = Main.projectile[(int)Projectile.ai[1]];
            Projectile.Center = owner.position + Projectile.velocity.RotatedBy(Math.PI / (Projectile.localAI[0] / 5)) + ((owner.Size / 2));
            Projectile.rotation = Projectile.velocity.RotatedBy(Math.PI / (Projectile.localAI[0] / 5)).ToRotation() + MathHelper.PiOver2;
        }
    }
}
