/*using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Regressus.NPCs.Bosses.Oracle;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace Regressus.Projectiles.Oracle
{
    public class OracleHoming : ModProjectile
    {
        public override string Texture => "Regressus/Empty";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("");
        }
        public override bool PreDraw(ref Color lightColor)
        {
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Extras/clock").Value, arenaCenter - Main.screenPosition, null, Color.White, 0, ModContent.Request<Texture2D>("Regressus/Extras/clock").Value.Size() / 2, TheOracle._arenaScale + .5f, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Extras/clockHand1").Value, arenaCenter - Main.screenPosition, null, Color.White, MathHelper.ToRadians(45), ModContent.Request<Texture2D>("Regressus/Extras/clockHand1").Value.Size() / 2, TheOracle._arenaScale + .5f, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Extras/clockHand2").Value, arenaCenter - Main.screenPosition, null, Color.White, 0, ModContent.Request<Texture2D>("Regressus/Extras/clockHand2").Value.Size() / 2, TheOracle._arenaScale + .5f, SpriteEffects.None, 0);
            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            return false;
        }
        public override void SetDefaults()
        {
            Projectile.width = 5;
            Projectile.height = 5;
            Projectile.aiStyle = 0;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
        }
        Vector2 arenaCenter;
        public override void AI()
        {
            NPC center = Main.npc[(int)Projectile.ai[0]];
            if (!center.active || center.type != ModContent.NPCType<NPCs.Bosses.Oracle.TheOracle>())
                Projectile.Kill();
            arenaCenter = TheOracle._arenaCenter;
            Projectile.Center = Main.player[center.target].Center;
            Projectile.timeLeft = 2;
        }
    }
}*/