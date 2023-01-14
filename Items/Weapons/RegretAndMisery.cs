using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Projectiles.Omniclass;
using Terraria.DataStructures;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Regressus.Items.Weapons
{
    public class RegretAndMisery : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("RegreSUS");
            Tooltip.SetDefault("Worth it\nSTOP POSTING ABOUT AMONG US! I'M TIRED OF SEEING IT! MY FRIENDS ON TIKTOK SEND ME MEMES, ON DISCORD IT'S MEMES!");
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.crit = 50;
            Item.damage = 500;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.reuseDelay = 100;
            Item.noMelee = true;
            Item.autoReuse = false;
            Item.channel = true;
            Item.noUseGraphic = false;
            Item.DamageType = DamageClass.Generic;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.rare = ItemRarityID.Purple;
            Item.shootSpeed = 15f;
            Item.shoot = ModContent.ProjectileType<RegretAndMiseryP2>();
        }

        public static readonly SoundStyle sussySound = new("Regressus/Sounds/Custom/Sussy", SoundType.Sound)
        {
            Volume = 0.65f,
            Pitch = 0,
            PitchVariance = 0.2f
        };
        public static readonly SoundStyle sussySound2 = new("Regressus/Sounds/Custom/Sussy2", SoundType.Sound)
        {
            Volume = 0.75f,
            Pitch = 0,
            PitchVariance = 0.2f
        };
        ParticleSystem2 sys = new();
        ParticleSystem2 sys2 = new();
        /*public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
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
                spriteBatch.Draw(part.textures[0], position + Item.Size / 2, null, Color.White * part.alpha, part.rotation, part.textures[0].Size() / 2, part.scale * 0.85f, SpriteEffects.None, 0f);
                spriteBatch.Draw(part.textures[0], position + Item.Size / 2, null, Color.Green * part.alpha, part.rotation, part.textures[0].Size() / 2, part.scale, SpriteEffects.None, 0f);
                spriteBatch.Reload(BlendState.AlphaBlend);
            },
            new Vector2(Main.rand.NextFloat(-2f, 2f), -.5f),
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
                spriteBatch.Draw(part.textures[0], position + Item.Size / 2, null, Color.White * part.alpha, part.rotation, part.textures[0].Size() / 2, part.scale * 0.85f, SpriteEffects.None, 0f);
                spriteBatch.Draw(part.textures[0], position + Item.Size / 2, null, Color.Green * part.alpha, part.rotation, part.textures[0].Size() / 2, part.scale, SpriteEffects.None, 0f);
                spriteBatch.Reload(BlendState.AlphaBlend);
            },
            new Vector2(Main.rand.NextFloat(-2f, 2f), -.5f),
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
            return true;
        }*/

        int attacks;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //if (++attacks >= 15)
            //    attacks = 0;

            // What the fuck is this cringe
            SoundEngine.PlaySound(sussySound, player.position);
            Projectile proj = Main.projectile[Projectile.NewProjectile(source, new Vector2(Main.MouseWorld.X, Main.screenPosition.Y), new Vector2(Main.rand.NextFloat(-2.5f, 2.5f), Item.shootSpeed), type, damage, knockback, player.whoAmI)];
            /*if (attacks == 14)
            {
                Terraria.Audio.SoundEngine.PlaySound(sussySound2);
                proj.scale = 5f;
                proj.damage = damage * 2;
            }*/
            return false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine tooltipLine = tooltips.Find((TooltipLine x) => x.Name == "ItemName");
            tooltipLine.OverrideColor = new Color(255, 0, 0, Main.DiscoR);
            TooltipLine tooltipLine2 = tooltips.Find((TooltipLine x) => x.Text == "STOP POSTING ABOUT AMONG US! I'M TIRED OF SEEING IT! MY FRIENDS ON TIKTOK SEND ME MEMES, ON DISCORD IT'S MEMES!");
            tooltipLine2.OverrideColor = new Color(255, 0, 0, Main.DiscoR);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Items.Weapons.Magic.Vision>(1).
                AddIngredient(ItemID.LunarBar, 35).
                AddIngredient(ItemID.SuspiciousLookingEye, 1).
                AddIngredient(ItemID.SuspiciousLookingTentacle, 1).
                AddIngredient(ItemID.FragmentVortex, 25).
                AddIngredient(ItemID.FragmentStardust, 25).
                AddIngredient(ItemID.FragmentSolar, 25).
                AddIngredient(ItemID.FragmentNebula, 25).
                AddTile(TileID.LunarCraftingStation).Register();
        }
    }
}
