using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Regressus.Items.Weapons.Melee.LichKnife {

    public class LichKnifeProjectile : ModProjectile {
        private static Asset<Texture2D> chainTexture;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lich Dagger");
            
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.ChainKnife);
            Projectile.width=30;
            Projectile.height=74;
            Projectile.aiStyle=13;
            Projectile.timeLeft=60;
            Projectile.hide=false;
            Projectile.tileCollide=false;
            Projectile.penetrate=-1;
        }

        public int collided{get => (int)Projectile.ai[0];set => Projectile.ai[0]=value;}

   



        public override void AI(){
            Player player=Main.player[Projectile.owner];
            if(Projectile.timeLeft==55){
                Projectile.tileCollide=true;
            }
            if(Projectile.timeLeft<=40){
                collided=1;
            }

      
        }

        public override void Load() { 

			chainTexture = ModContent.Request<Texture2D>("example/Content/Items/LichKnife/LichKnifeChain");
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            int heal=target.boss ? 2 : Main.hardMode ? 7 : 5;
          
            Main.player[Projectile.owner].HealEffect(heal);
            Main.player[Projectile.owner].Heal(heal);


            QuickDustLine(Main.player[Projectile.owner].Center,target.Center,100f,Color.Red);
     

        }


        public override bool PreDrawExtras() {
			Vector2 start = Main.player[Projectile.owner].MountedCenter;
            Vector2 end=Projectile.Center;
			float num1 = Vector2.Distance(start, end);
            Vector2 v = (end - start) / num1;
            float rotation = v.ToRotation();
            Vector2 vector2 = start;
            Vector2 screenPosition = Main.screenPosition;
            

            int index=0;
            float scale=0.75f;

            for (float num2 = 0.0f; (double) num2 <= (double) num1; num2 += chainTexture.Width()*scale)
            {
                float num3 = num2 / num1;
  
                Main.spriteBatch.Draw(chainTexture.Value, vector2 - screenPosition-new Vector2(0,3.5f).RotatedBy(rotation), new Rectangle(0,0,chainTexture.Width(),chainTexture.Height()/3),Color.White, rotation, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
                vector2 = start + num2 * v;
                index=(index+1)%3;
            }


			return false;
		}

        public void QuickDustLine(Vector2 start, Vector2 end, float splits, Color color)
        {
          Dust.QuickDust(start, color).scale = 1f;
          Dust.QuickDust(end, color).scale = 1f;
          float num = 1f / splits;
          for (float amount = 0.0f; (double) amount < 1.0; amount += num)
            Dust.QuickDustSmall(Vector2.Lerp(start, end, amount), color).scale=1f;
        }


        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);

    
            
            Texture2D texture2D=TextureAssets.Projectile[Type].Value;
            Vector2 drawOrigin = new Vector2(texture2D.Width * 0.5f, Projectile.height * 0.5f);
            Main.EntitySpriteDraw(texture2D, (Projectile.position - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY), null, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}