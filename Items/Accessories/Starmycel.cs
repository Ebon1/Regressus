using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Projectiles.Magic;
using Terraria.DataStructures;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Regressus.Projectiles.Minibosses.Vagrant;
using System.ComponentModel.DataAnnotations;
using Regressus.Items.Weapons.Ranged;
using Terraria.Audio;

namespace Regressus.Items.Accessories
{
    public class Starmycel : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starmycel");
            Tooltip.SetDefault("Boosts mana regen and summon damage by %10.\nSummons Starlads when hit by an enemy with a 80 second cooldown.");
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<RegrePlayer>().starmycel = true;
            player.manaRegenBonus += Math.Max(1, player.manaRegenBonus / 10);
        }

  
        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 58;
            Item.accessory = true;
            Item.rare = ItemRarityID.Lime;
        }

    }
    public class StarladProjectile : ModProjectile
    {
        public override string Texture => "Regressus/Items/Accessories/Starlad";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starlad");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, TorchID.UltraBright);
            if (++Projectile.frameCounter >=5)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 8)
                {
                    Projectile.frame = 0;
                }
            }
            
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 24; i++)
            {
                Vector2 velo = Vector2.UnitX.RotatedBy(MathHelper.ToRadians(360/24*i)) * 5;
             
                Dust.NewDustDirect(Projectile.Center, 12, 12, Main.rand.NextBool() ? 57 : 58, velo.X, velo.Y);
            }
            SoundEngine.PlaySound(SoundID.Item4,Projectile.Center);
        }


        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);

            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            SpriteEffects effects = Projectile.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, new Rectangle(0, Projectile.frame * Projectile.height, Projectile.width, Projectile.height), color, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0);
            }
            Main.EntitySpriteDraw(texture, (Projectile.position - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY), new Rectangle(0, Projectile.frame * Projectile.height, Projectile.width, Projectile.height), lightColor, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0);
            Dust.NewDustDirect(Projectile.BottomLeft+new Vector2(12,-8), 8, 8, Main.rand.NextBool() ? 57 : 58);
            return false;
        }
        public override void SetDefaults()
        {
            Projectile.width = 68;
            Projectile.height = 56;
            Projectile.aiStyle = 0;
            Projectile.penetrate = 999999;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.damage = 12;
            Projectile.timeLeft = 360;
            Projectile.tileCollide = false;
            
            
        }

    }
}
