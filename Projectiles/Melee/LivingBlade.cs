using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Effects.Prims;

namespace Regressus.Projectiles.Melee
{
    public class LivingBladeHeld : ModProjectile//, IPrimitiveDrawer
    {
        public override string Texture => "Regressus/Items/Weapons/Melee/LivingBlade";
        // pitch is XY roll is YZ yaw is XZ
        int swingTime = 35;
        float holdOffset = 80f;
        public override void SetDefaults()
        {
            Projectile.timeLeft = swingTime;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.width = Projectile.height = 64;
            Projectile.scale = 2f;
        }
        public float Ease(float f)
        {
            return 1 - (float)Math.Pow(2, 10 * f - 10);
        }
        public float ScaleFunction(float progress)
        {
            return 0.7f + (float)Math.Sin(progress * Math.PI) * 0.5f;
        }
        //List<Vector2> vert = new List<Vector2>();
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead || player.CCed || player.noItems)
            {
                return;
            }

            int direction = (int)Projectile.ai[1];
            float swingProgress = Ease(Utils.GetLerpValue(0f, swingTime, Projectile.timeLeft));
            float defRot = Projectile.velocity.ToRotation();
            float start = defRot - (MathHelper.PiOver2 + MathHelper.PiOver4);
            float end = defRot + (MathHelper.PiOver2 + MathHelper.PiOver4);
            float rotation = direction == 1 ? start + MathHelper.Pi * 3 / 2 * swingProgress : end - MathHelper.Pi * 3 / 2 * swingProgress;
            Vector2 position = player.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, rotation - MathHelper.PiOver2) +
                rotation.ToRotationVector2() * holdOffset * ScaleFunction(swingProgress);
            Projectile.Center = position;
            Projectile.rotation = (position - player.Center).ToRotation() + MathHelper.PiOver4;

            player.ChangeDir(Projectile.velocity.X < 0 ? -1 : 1);
            player.heldProj = Projectile.whoAmI;
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, rotation - MathHelper.PiOver2);
            player.itemTime = 2;
            player.itemAnimation = 2;

            /*Vector2 off = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2();
            vert.Add(Projectile.Center - off * 25 * 2 * ScaleFunction(swingProgress));
            vert.Add(Projectile.Center + off * 40 * 2 * ScaleFunction(swingProgress));
            if (vert.Count > 10)
            {
                vert.RemoveAt(0);
                vert.RemoveAt(0);
            }*/
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.NextBool(3))
                Item.NewItem(Projectile.GetSource_FromThis(), target.getRect(), ModContent.ItemType<Items.Consumables.LivingBladePickup>());
        }
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            if (player.active && player.channel && !player.dead && !player.CCed && !player.noItems)
            {
                if (player.whoAmI == Main.myPlayer)
                {
                    Vector2 dir = Vector2.Normalize(Main.MouseWorld - player.Center);
                    Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), player.Center, dir, Projectile.type, Projectile.damage, Projectile.knockBack, player.whoAmI, 0, (Projectile.ai[1] + 1) % 2);
                    proj.rotation = Projectile.rotation;
                    proj.Center = Projectile.Center;
                }
            }
        }
        public override bool ShouldUpdatePosition() => false;
        public override bool PreDraw(ref Color lightColor)
        {
            float swingProgress = Ease(Utils.GetLerpValue(0f, swingTime, Projectile.timeLeft));
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 orig = texture.Size() / 2;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), Color.White, Projectile.rotation, orig, 2 * ScaleFunction(swingProgress), SpriteEffects.None, 0);
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Player player = Main.player[Projectile.owner];
            float rot = Projectile.rotation - MathHelper.PiOver4;
            Vector2 start = player.Center;
            Vector2 end = player.Center + rot.ToRotationVector2() * (Projectile.height + holdOffset * 0.8f);
            float a = 0;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, projHitbox.Width, ref a) && Collision.CanHitLine(player.TopLeft, player.width, player.height, targetHitbox.TopLeft(), targetHitbox.Width, targetHitbox.Height);
        }
        /*public void DrawPrimitives()
        {
            if (vert.Count > 2)
            {
                var dev = Main.graphics.GraphicsDevice;
                RasterizerState prev = dev.RasterizerState;
                dev.RasterizerState = RasterizerState.CullNone;
                List<VertexPositionColorTexture> vertices = new List<VertexPositionColorTexture>();
                for (int i = 0; i < vert.Count - 2; i += 2)
                {
                    if (vert[i] == vert[i + 2] || vert[i + 1] == vert[i + 3]) continue;
                    Vector2 tl = vert[i] - Main.screenPosition;
                    Vector2 bl = vert[i + 1] - Main.screenPosition;

                    Vector2 tr = vert[i + 2] - Main.screenPosition;
                    Vector2 br = vert[i + 3] - Main.screenPosition;

                    float p1 = 1f - (float)i / 10;
                    float p2 = 1f - (float)(i + 2) / 10;

                    vertices.Add(PrimitiveHelper.AsVertex(tl, Color.Yellow, new Vector2(p1, 0)));
                    vertices.Add(PrimitiveHelper.AsVertex(bl, Color.Yellow, new Vector2(p1, 1)));

                    vertices.Add(PrimitiveHelper.AsVertex(tr, Color.Yellow, new Vector2(p2, 0)));
                    vertices.Add(PrimitiveHelper.AsVertex(br, Color.Yellow, new Vector2(p2, 1)));
                }
                Texture2D tex = ModContent.Request<Texture2D>("Regressus/Extras/Extra_209").Value;
                PrimitivePacket packet = new PrimitivePacket(vertices, PrimitiveType.TriangleStrip, vertices.Count, tex);
                packet.Send();
                dev.RasterizerState = prev;
            }
        }*/
    }
}
