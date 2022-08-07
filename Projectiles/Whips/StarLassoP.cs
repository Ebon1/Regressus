using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.GameContent;

namespace Regressus.Projectiles.Whips
{
    public class StarLassoP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsAWhip[Type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 26;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.WhipSettings.Segments = 25;
            Projectile.WhipSettings.RangeMultiplier = 2f;
        }
        private float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.Center = Main.GetPlayerArmPosition(Projectile) + Projectile.velocity * Timer;
            Projectile.spriteDirection = Projectile.velocity.X >= 0f ? 1 : -1;
            Timer++;

            float swingTime = owner.itemAnimationMax * Projectile.MaxUpdates;
            if (Timer >= swingTime || owner.itemAnimation <= 0)
            {
                Projectile.Kill();
                return;
            }
            owner.heldProj = Projectile.whoAmI;
            if (Timer == swingTime / 2)
            {
                List<Vector2> points = Projectile.WhipPointsForCollision;
                Projectile.FillWhipControlPoints(Projectile, points);
                SoundEngine.PlaySound(SoundID.Item153, points[points.Count - 1]);
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.knockBackResist != 0f && !target.boss)
                target.velocity = RegreUtils.FromAToB(target.Center, Main.player[Projectile.owner].Center) * 15f * target.knockBackResist;
            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            Projectile.damage = (int)(damage * 0.7f);
        }
        private void DrawLine(List<Vector2> list)
        {
            //Texture2D texture = RegreUtils.GetExtraTexture("GlowyLine");
            Texture2D texture = TextureAssets.FishingLine.Value;
            Rectangle frame = texture.Frame();
            Vector2 origin = new Vector2(frame.Width / 2, 2);

            Vector2 pos = list[0];
            for (int i = 0; i < list.Count - 1; i++)
            {
                Vector2 element = list[i];
                Vector2 diff = list[i + 1] - element;
                float amount = i / (float)list.Count;
                Color color = Color.Lerp(Color.Yellow, Color.White, amount);
                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);
                //Main.spriteBatch.Reload(BlendState.Additive);
                Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, SpriteEffects.None, 0);
                //Main.spriteBatch.Reload(BlendState.AlphaBlend);

                pos += diff;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            List<Vector2> list = new List<Vector2>();
            Projectile.FillWhipControlPoints(Projectile, list);
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Texture2D texture2 = RegreUtils.GetExtraTexture("Star");
            DrawLine(list);
            Vector2 pos = list[0];
            Vector2 pos2 = list[list.Count - 1];
            SpriteEffects flip = Projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Main.EntitySpriteDraw(texture, pos - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), Color.White, Projectile.rotation, texture.Size() / 2, 1, flip, 0);
            Main.spriteBatch.Reload(BlendState.Additive);
            Main.EntitySpriteDraw(texture2, pos2 - Main.screenPosition, new Rectangle(0, 0, texture2.Width, texture2.Height), Color.White, Projectile.rotation, texture2.Size() / 2, .25f, flip, 0);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }

    }
}
