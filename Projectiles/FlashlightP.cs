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
using static System.Net.Mime.MediaTypeNames;
using static Humanizer.In;
using Regressus.NPCs.Bosses.Vagrant;
using Terraria.GameContent.UI;
using Terraria.UI.Chat;
using Terraria.GameContent.ItemDropRules;

namespace Regressus.Projectiles
{
    public class FlashlightP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flashlight");
            Main.projFrames[Type] = 2;
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
        int frame;
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            SpriteEffects effects = player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, new Rectangle(0, frame, 34, 14), lightColor, Projectile.rotation, Projectile.Size / 2, Projectile.scale, effects, 0);
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            return false;
        }
        public override void PostDraw(Color lightColor)
        {
            DelegateMethods.v3_1 = new Color(20, 63, 128).ToVector3();
            Player player = Main.player[Projectile.owner];
            Texture2D tex2 = RegreUtils.GetExtraTexture("cone3");
            SpriteEffects effects = player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;
            Terraria.Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.rotation.ToRotationVector2() * 175, Projectile.width * 3, new Terraria.Utils.TileActionAttempt(DelegateMethods.CastLight));
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            Main.spriteBatch.Draw(tex2, Projectile.Center + Projectile.rotation.ToRotationVector2() * 90f - Main.screenPosition, null, Color.White * 0.5f, Projectile.rotation, tex2.Size() / 2, Projectile.scale, effects, 0);
            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
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
            if (++Projectile.frameCounter % 5 == 0)
            {
                if (frame == 0)
                    frame = Projectile.height;
                else
                    frame = 0;
            }
            player.heldProj = Projectile.whoAmI;
            Projectile.Center = player.RotatedRelativePoint(player.MountedCenter) + Projectile.rotation.ToRotationVector2() * 25f;
            Projectile.rotation = RegreUtils.FromAToB(player.Center, Main.MouseWorld).ToRotation();
            player.itemRotation = Projectile.rotation * player.direction;
            player.ChangeDir(Main.MouseWorld.X < player.Center.X ? -1 : 1);
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, RegreUtils.FromAToB(player.Center, Main.MouseWorld).ToRotation() - MathHelper.PiOver2);
            //  Lighting.AddLight(Projectile.Center, TorchID.White);
        }
    }
    public class FlashlightP2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Overclocked Flashlight");
            Main.projFrames[Type] = 2;
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
        int frame;
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            SpriteEffects effects = player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, new Rectangle(0, frame, 34, 14), Color.White, Projectile.rotation, Projectile.Size / 2, Projectile.scale, effects, 0);
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            return false;
        }
        public override void PostDraw(Color lightColor)
        {
            DelegateMethods.v3_1 = new Color(20, 63, 128).ToVector3();
            Player player = Main.player[Projectile.owner];
            Texture2D tex2 = RegreUtils.GetExtraTexture("cone5_transparent");
            SpriteEffects effects = player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;
            Terraria.Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.rotation.ToRotationVector2() * 700, 500, new Terraria.Utils.TileActionAttempt(DelegateMethods.CastLightOpen));
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            //for (int i = 0; i < 2; i++)
            Main.spriteBatch.Draw(tex2, Projectile.Center - Projectile.rotation.ToRotationVector2() * 40 - Main.screenPosition, null, Color.White, Projectile.rotation, new(0, tex2.Height / 2), Projectile.scale, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(tex2, Projectile.Center - Projectile.rotation.ToRotationVector2() * 40 - Main.screenPosition, null, Color.LightBlue * 0.75f, Projectile.rotation, new(0, tex2.Height / 2), Projectile.scale * 1.1f, SpriteEffects.None, 0);
            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
        }
        int vagrantTimer;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!Main.dayTime && !NPC.AnyNPCs(ModContent.NPCType<VoltageVagrant>()) && player.ZonePurity && player.ZoneOverworldHeight)
            {
                vagrantTimer++;
                if (vagrantTimer >= 200)
                {
                    if (player.GetModPlayer<RegrePlayer>().hasEncounteredMoth)
                    {
                        //player.chatOverhead.NewMessage("Here it comes!", 300);
                        EmoteBubble.MakeLocalPlayerEmote(EmoteID.DebuffSilence);
                    }
                    else
                    {

                        //player.chatOverhead.NewMessage("There's something nearby...", 300);
                        EmoteBubble.MakeLocalPlayerEmote(EmoteID.EmoteConfused);
                        player.GetModPlayer<RegrePlayer>().hasEncounteredMoth = true;
                    }
                    player.channel = false;
                    NPC.NewNPCDirect(Projectile.InheritSource(Projectile), player.Center + Projectile.rotation.ToRotationVector2() * Main.screenWidth, ModContent.NPCType<VoltageVagrant>());
                }
            }
            if (player.channel)
            {
                Projectile.timeLeft = 2;
                player.itemTime = 2;
                player.itemAnimation = 2;
            }
            if (++Projectile.frameCounter % 5 == 0)
            {
                if (frame == 0)
                    frame = Projectile.height;
                else
                    frame = 0;
            }
            player.heldProj = Projectile.whoAmI;
            Projectile.Center = player.RotatedRelativePoint(player.MountedCenter) + Projectile.rotation.ToRotationVector2() * 25f;
            Projectile.rotation = RegreUtils.FromAToB(player.Center, Main.MouseWorld).ToRotation();
            player.itemRotation = Projectile.rotation * player.direction;
            player.ChangeDir(Main.MouseWorld.X < player.Center.X ? -1 : 1);
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, RegreUtils.FromAToB(player.Center, Main.MouseWorld).ToRotation() - MathHelper.PiOver2);
            //  Lighting.AddLight(Projectile.Center, TorchID.White);
        }
    }
}
