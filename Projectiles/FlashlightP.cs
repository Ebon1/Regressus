using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Items.Dev;
using Regressus.Projectiles.Melee;
using Terraria.GameContent;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace Regressus.Projectiles
{
    public class FlashlightP : ModProjectile
    {
        public override string Texture => "Regressus/Items/Flashlight";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flashlight");
        }
        public override void SetDefaults()
        {
            Projectile.height = 14;
            Projectile.width = 34;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Texture2D tex2 = RegreUtils.GetExtraTexture("cone3");
            SpriteEffects effects = player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 34, 14), lightColor, Projectile.rotation, Projectile.Size / 2, Projectile.scale, effects, 0);
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            Main.spriteBatch.Draw(tex2, Projectile.Center + Projectile.rotation.ToRotationVector2() * 90f - Main.screenPosition, null, Color.White * 0.5f, Projectile.rotation, tex2.Size() / 2, Projectile.scale, effects, 0);
            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            return false;
        }
        public override void PostDraw(Color lightColor)
        {
            DelegateMethods.v3_1 = new Color(20, 63, 128).ToVector3();
            Terraria.Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.rotation.ToRotationVector2() * 175, Projectile.width * 3, new Terraria.Utils.TileActionAttempt(DelegateMethods.CastLight));
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (player.channel)
            {
                Projectile.timeLeft = 2;
                player.itemTime = 2;
                player.itemAnimation = 2;
            }
            player.heldProj = Projectile.whoAmI;
            Projectile.Center = player.RotatedRelativePoint(player.MountedCenter) + Projectile.rotation.ToRotationVector2() * 25f;
            Projectile.rotation = RegreUtils.FromAToB(player.Center, Main.MouseWorld).ToRotation();
            player.itemRotation = Projectile.rotation * player.direction;
            player.ChangeDir(Main.MouseWorld.X < player.Center.X ? -1 : 1);
            //  Lighting.AddLight(Projectile.Center, TorchID.White);
        }
    }
}
