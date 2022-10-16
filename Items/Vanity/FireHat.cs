using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Projectiles;
using Regressus.Dusts;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using System.Security.Cryptography;

namespace Regressus.Items.Vanity
{
    public class FireHat : ModItem
    {
        public override string Texture => "Regressus/Extras/Empty";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Literally Just Fire");
            Tooltip.SetDefault("Makes you look hot!\n \n \n \n \n \n \n \n \n \nQuite Literally.");
        }
        public override void SetDefaults()
        {
            Item.DefaultToAccessory();
            Item.vanity = true;
            Item.canBePlacedInVanityRegardlessOfConditions = true;
            Item.rare = ItemRarityID.Blue;
        }
        public override void UpdateVanity(Player player)
        {
            Dust.NewDust(player.Center - Microsoft.Xna.Framework.Vector2.UnitY * (player.height / 4), 0, 0, ModContent.DustType<FireDust>(), Main.windSpeedTarget * 4, -10f);
        }
        public override void UpdateEquip(Player player)
        {
            Dust.NewDust(player.Center - Microsoft.Xna.Framework.Vector2.UnitY * (player.height / 4), 0, 0, ModContent.DustType<FireDust>(), Main.windSpeedTarget * 4, -10f);
        }
        ParticleSystem2 sys = new();
        ParticleSystem2 sys2 = new();
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            sys.CreateParticle((part) =>
            {
                if (part.ai[0] < 0)
                {
                    part.dead = true;
                }
                part.ai[0]--;
                part.scale = 0.3f * (part.ai[0] / 10);
                part.alpha = (part.ai[0] / 10);
            }, new[]
            {
                    ModContent.Request<Texture2D>("Regressus/Extras/Extras2/fire_02").Value
            }, (part, spriteBatch, position) =>
            {
                Color c = Main.hslToRgb((float)Math.Sin(Main.GlobalTimeWrappedHourly + part.ai[0] / 60 * Math.PI) / 2 + 0.5f, 1f, 0.5f);
                spriteBatch.Reload(BlendState.Additive);
                spriteBatch.Draw(part.textures[0], position, null, Color.White * part.alpha, part.rotation, part.textures[0].Size() / 2, part.scale * 0.85f, SpriteEffects.None, 0f);
                spriteBatch.Draw(part.textures[0], position, null, Color.OrangeRed * part.alpha, part.rotation, part.textures[0].Size() / 2, part.scale, SpriteEffects.None, 0f);
                spriteBatch.Reload(BlendState.AlphaBlend);
            },
            new Vector2(Main.rand.NextFloat(-2f, 2f), -5f),
            part =>
            {
                part.ai[1] = Main.rand.NextFloat(0.05f, 0.1f);
                part.position = position;
                part.ai[0] = 10;
            });


            sys2.CreateParticle((part) =>
            {
                if (part.ai[0] < 0)
                {
                    part.dead = true;
                }
                part.ai[0]--;
                part.scale = 0.3f * (part.ai[0] / 10);
                part.alpha = (part.ai[0] / 10);
            }, new[]
            {
                    ModContent.Request<Texture2D>("Regressus/Extras/Extras2/fire_01").Value
            }, (part, spriteBatch, position) =>
            {
                Color c = Main.hslToRgb((float)Math.Sin(Main.GlobalTimeWrappedHourly + part.ai[0] / 60 * Math.PI) / 2 + 0.5f, 1f, 0.5f);
                spriteBatch.Reload(BlendState.Additive);
                spriteBatch.Draw(part.textures[0], position, null, Color.White * part.alpha, part.rotation, part.textures[0].Size() / 2, part.scale * 0.85f, SpriteEffects.None, 0f);
                spriteBatch.Draw(part.textures[0], position, null, Color.OrangeRed * part.alpha, part.rotation, part.textures[0].Size() / 2, part.scale, SpriteEffects.None, 0f);
                spriteBatch.Reload(BlendState.AlphaBlend);
            },
            new Vector2(Main.rand.NextFloat(-2f, 2f), -5f),
            part =>
            {
                part.ai[1] = Main.rand.NextFloat(0.05f, 0.1f);
                part.position = position;
                part.ai[0] = 10;
            });
            sys.UpdateParticles();
            sys.DrawParticles();
            sys2.UpdateParticles();
            sys2.DrawParticles();
            return false;
        }
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            Dust.NewDust(Item.Center, 0, 0, ModContent.DustType<FireDust>(), Main.windSpeedTarget * 4, -10f);
        }
    }
}
