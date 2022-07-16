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
using System.IO;
using Terraria.Graphics.Shaders;
using Terraria.Audio;

namespace Regressus.Projectiles.Dev
{
    public class CorruptedTriangleSummon1 : ModProjectile
    {
        public override string Texture => "Regressus/Extras/Sprites/RollHand1";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Corrupted Triangle");
            Main.projFrames[Type] = 2;
        }
        public override bool ShouldUpdatePosition() => false;
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
        public override void SetDefaults()
        {
            Projectile.height = Projectile.width = 20;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 180;
            Projectile.scale = 1.5f;

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            SoundStyle hit = new SoundStyle("Regressus/Sounds/Custom/RollHit");
            hit.Volume = 0.25f;
            SoundEngine.PlaySound(hit, target.Center);
        }
        public override void AI()
        {

            float progress = Utils.GetLerpValue(0, 180, Projectile.timeLeft);
            if (Projectile.timeLeft > 155)
            {
                Projectile.frame = Main.rand.Next(2);
            }
            else
            {
                Projectile.frame = 0;
            }
            float speedScale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);
            Player player = Main.player[Projectile.owner];
            //Projectile.Center = new Vector2(player.Center.X + Projectile.velocity.X / 2, player.Center.Y + Projectile.velocity.Y / 2);
            Projectile.ai[1] += 2f * (float)Math.PI / 600f * (10 * speedScale);
            Projectile.ai[1] %= 2f * (float)Math.PI;
            Projectile.Center = player.Center + (30) * new Vector2((float)Math.Cos(Projectile.ai[1]), (float)Math.Sin(Projectile.ai[1]));
            Projectile.rotation = new Vector2((float)Math.Cos(Projectile.localAI[0]), (float)Math.Sin(Projectile.localAI[0])).ToRotation() + MathHelper.PiOver2;
            Projectile.ai[0]++;
            if (Projectile.ai[0] == 5)
            {
                Projectile proj = Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), new Vector2(Projectile.position.X + Projectile.velocity.X + (float)(Projectile.width / 2), Projectile.position.Y + Projectile.velocity.Y + (float)(Projectile.height / 2)), Projectile.velocity, ModContent.ProjectileType<CorruptedTriangleSummon2>(), Projectile.damage + 10, Projectile.knockBack, Projectile.owner, 0, Projectile.whoAmI);
                proj.localAI[0] = Projectile.localAI[1];
                proj.localAI[1] = Projectile.localAI[1];
            }
        }
    }
    public class CorruptedTriangleSummon2 : ModProjectile
    {
        public override string Texture => "Regressus/Extras/Sprites/RollHand2";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Corrupted Triangle");
            Main.projFrames[Type] = 2;
        }
        public override bool ShouldUpdatePosition() => false;
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
        public override void SetDefaults()
        {
            Projectile.height = Projectile.width = 40;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 180;
            Projectile.scale = 1.5f;


        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            SoundStyle hit = new SoundStyle("Regressus/Sounds/Custom/RollHit");
            hit.Volume = 0.25f;
            SoundEngine.PlaySound(hit, target.Center);
        }
        public override void AI()
        {

            float progress = Utils.GetLerpValue(0, 180, Projectile.timeLeft);
            if (Projectile.timeLeft > 155)
            {
                Projectile.frame = Main.rand.Next(2);
            }
            else
            {
                Projectile.frame = 0;
            }
            float speedScale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);
            Player player = Main.player[Projectile.owner];
            Projectile owner = Main.projectile[(int)Projectile.ai[1]];
            Projectile.localAI[0] += 2f * (float)Math.PI / 600f * (10 * speedScale);
            Projectile.localAI[0] %= 2f * (float)Math.PI;
            Projectile.Center = player.Center + (50) * new Vector2((float)Math.Cos(Projectile.localAI[0]), (float)Math.Sin(Projectile.localAI[0])).RotatedBy(MathHelper.Pi / (11.25f / 2));
            Projectile.rotation = new Vector2((float)Math.Cos(Projectile.localAI[0]), (float)Math.Sin(Projectile.localAI[0])).RotatedBy(MathHelper.Pi / (11.25f / 2)).ToRotation() + MathHelper.PiOver2;
            Projectile.ai[0]++;
            if (Projectile.ai[0] == 5)
            {
                Projectile proj = Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), new Vector2(Projectile.position.X + Projectile.velocity.X + (float)(Projectile.width / 2), Projectile.position.Y + Projectile.velocity.Y + (float)(Projectile.height / 2)), Projectile.velocity, ModContent.ProjectileType<CorruptedTriangleSummon3>(), Projectile.damage + 10, Projectile.knockBack, Projectile.owner, 0, Projectile.whoAmI);
                proj.localAI[0] = Projectile.localAI[1];
                proj.localAI[1] = Projectile.localAI[1];
            }
        }
    }
    public class CorruptedTriangleSummon3 : ModProjectile
    {
        public override string Texture => "Regressus/Extras/Sprites/RollHand3";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Corrupted Triangle");
            Main.projFrames[Type] = 2;
        }
        public override bool ShouldUpdatePosition() => false;
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
        public override void SetDefaults()
        {
            Projectile.height = Projectile.width = 36;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 180;
            Projectile.scale = 1.5f;


        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            SoundStyle hit = new SoundStyle("Regressus/Sounds/Custom/RollHit");
            hit.Volume = 0.25f;

            SoundEngine.PlaySound(hit, target.Center);
        }
        public override void AI()
        {

            float progress = Utils.GetLerpValue(0, 180, Projectile.timeLeft);
            if (Projectile.timeLeft > 155)
            {
                Projectile.frame = Main.rand.Next(2);
            }
            else
            {
                Projectile.frame = 0;
            }
            float speedScale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);
            Player player = Main.player[Projectile.owner];
            Projectile owner = Main.projectile[(int)Projectile.ai[1]];
            Projectile.localAI[0] += 2f * (float)Math.PI / 600f * (10 * speedScale);
            Projectile.localAI[0] %= 2f * (float)Math.PI;
            Projectile.Center = player.Center + (80) * new Vector2((float)Math.Cos(Projectile.localAI[0]), (float)Math.Sin(Projectile.localAI[0])).RotatedBy(MathHelper.Pi / (11.25f / 3));
            Projectile.rotation = new Vector2((float)Math.Cos(Projectile.localAI[0]), (float)Math.Sin(Projectile.localAI[0])).RotatedBy(MathHelper.Pi / (11.25f / 3)).ToRotation() + MathHelper.PiOver2;
            Projectile.ai[0]++;
            if (Projectile.ai[0] == 5)
            {
                Projectile proj = Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), new Vector2(Projectile.position.X + Projectile.velocity.X + (float)(Projectile.width / 2), Projectile.position.Y + Projectile.velocity.Y + (float)(Projectile.height / 2)), Projectile.velocity, ModContent.ProjectileType<CorruptedTriangleSummon4>(), Projectile.damage + 10, Projectile.knockBack, Projectile.owner, 0, Projectile.whoAmI);
                proj.localAI[0] = Projectile.localAI[1];
                proj.localAI[1] = Projectile.localAI[1];
            }
        }
    }
    public class CorruptedTriangleSummon4 : ModProjectile
    {
        public override string Texture => "Regressus/Extras/Sprites/RollHand4";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Corrupted Triangle");
            Main.projFrames[Type] = 2;
        }
        public override bool ShouldUpdatePosition() => false;
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
        public override void SetDefaults()
        {
            Projectile.height = Projectile.width = 52;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 180;
            Projectile.scale = 1.5f;


        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void AI()
        {

            float progress = Utils.GetLerpValue(0, 180, Projectile.timeLeft);
            if (Projectile.timeLeft > 155)
            {
                Projectile.frame = Main.rand.Next(2);
            }
            else
            {
                Projectile.frame = 0;
            }
            float speedScale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);
            Player player = Main.player[Projectile.owner];
            Projectile owner = Main.projectile[(int)Projectile.ai[1]];
            Projectile.localAI[0] += 2f * (float)Math.PI / 600f * (10 * speedScale);
            Projectile.localAI[0] %= 2f * (float)Math.PI;
            Projectile.Center = player.Center + (120) * new Vector2((float)Math.Cos(Projectile.localAI[0]), (float)Math.Sin(Projectile.localAI[0])).RotatedBy(MathHelper.Pi / (11.25f / 4));
            Projectile.rotation = new Vector2((float)Math.Cos(Projectile.localAI[0]), (float)Math.Sin(Projectile.localAI[0])).RotatedBy(MathHelper.Pi / (11.25f / 4)).ToRotation() + MathHelper.PiOver2;
            Projectile.ai[0]++;
            if (Projectile.ai[0] == 5)
            {
                Projectile proj = Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), new Vector2(Projectile.position.X + Projectile.velocity.X + (float)(Projectile.width / 2f), Projectile.position.Y + Projectile.velocity.Y + (float)(Projectile.height / 2f)), Projectile.velocity, ModContent.ProjectileType<CorruptedTriangleSummon5>(), Projectile.damage + 10, Projectile.knockBack, Projectile.owner, 0, Projectile.whoAmI);
                proj.localAI[0] = Projectile.localAI[1];
                proj.localAI[1] = Projectile.localAI[1];
            }
        }
    }
    public class CorruptedTriangleSummon5 : ModProjectile
    {
        public override string Texture => "Regressus/Extras/Sprites/RollHand5";
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
        public override void SetDefaults()
        {
            Projectile.height = Projectile.width = 80;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 180;
            Projectile.scale = 1.75f;


        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
                Dust.NewDustDirect(Projectile.Center, 5, 5, ModContent.DustType<Dusts.RollParticle>(), Projectile.velocity.X / 5, Projectile.velocity.Y / 5);
        }

        public override void AI()
        {

            float progress = Utils.GetLerpValue(0, 180, Projectile.timeLeft);
            if (Projectile.timeLeft > 155)
            {
                Projectile.frame = Main.rand.Next(2);
            }
            else
            {
                Projectile.frame = 0;
            }
            float speedScale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);
            Player player = Main.player[Projectile.owner];
            Projectile owner = Main.projectile[(int)Projectile.ai[1]];
            Projectile.ai[0]++;
            Projectile.localAI[0] += 2f * (float)Math.PI / 600f * (10 * speedScale);
            Projectile.localAI[0] %= 2f * (float)Math.PI;
            Projectile.Center = player.Center + (160) * new Vector2((float)Math.Cos(Projectile.localAI[0]), (float)Math.Sin(Projectile.localAI[0])).RotatedBy(MathHelper.Pi / (11.25f / 5));
            Projectile.rotation = new Vector2((float)Math.Cos(Projectile.localAI[0]), (float)Math.Sin(Projectile.localAI[0])).RotatedBy(MathHelper.Pi / (11.25f / 5)).ToRotation() + MathHelper.PiOver2;
        }
    }
}
