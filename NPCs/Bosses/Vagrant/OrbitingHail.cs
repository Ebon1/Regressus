using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Regressus.Projectiles.Minibosses.Vagrant;
using Terraria.DataStructures;
using Regressus.NPCs.Bosses.Vagrant;
using ReLogic.Content;
using Terraria.GameContent.Bestiary;
namespace Regressus.NPCs.Bosses.Vagrant
{
    public class OrbitingHail : ModNPC
    {
        public override string Texture => "Regressus/Projectiles/Minibosses/Vagrant/Hail1";
        public override void SetStaticDefaults()
        {

            NPCID.Sets.TrailCacheLength[NPC.type] = 10;
            NPCID.Sets.TrailingMode[NPC.type] = 0;
        }
        public override void SetDefaults()
        {
            NPC.Size = Vector2.One * 30;
            NPC.aiStyle = -1;
            NPC.lifeMax = 50;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.knockBackResist = 0f;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPosition, Color drawColor)
        {
            spriteBatch.Reload(BlendState.Additive);
            Texture2D tex = ModContent.Request<Texture2D>("Regressus/Projectiles/Minibosses/Vagrant/Hail1").Value;
            Texture2D glow = ModContent.Request<Texture2D>("Regressus/Projectiles/Minibosses/Vagrant/Hail1_Glow").Value;
            var fadeMult = 1f / NPCID.Sets.TrailCacheLength[NPC.type];
            for (int i = 0; i < NPCID.Sets.TrailCacheLength[NPC.type]; i++)
            {
                spriteBatch.Draw(glow, NPC.oldPos[i] - screenPosition + new Vector2(NPC.width / 2f, NPC.height / 2f), new Rectangle(0, 0, glow.Width, glow.Height), Color.LightBlue * (1f - fadeMult * i), NPC.oldRot[i], glow.Size() / 2, NPC.scale * (NPCID.Sets.TrailCacheLength[NPC.type] - i) / NPCID.Sets.TrailCacheLength[NPC.type], NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            }

            spriteBatch.Reload(BlendState.AlphaBlend);
            spriteBatch.Draw(tex, NPC.Center - screenPosition, new Rectangle(0, 0, tex.Width, tex.Height), Color.White, NPC.rotation, tex.Size() / 2, NPC.scale, NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            return false;
        }
        public override Color? GetAlpha(Color drawColor) => Color.White;
        public override void OnKill()
        {
            Projectile.NewProjectile(NPC.GetSource_Death(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<HailExplosion>(), 15, 1);
            Projectile.NewProjectile(NPC.GetSource_Death(), NPC.Center, RegreUtils.FromAToB(NPC.Center, Main.LocalPlayer.Center) * 5f, ModContent.ProjectileType<Hail1>(), 15, 1);
        }
        public override void AI()
        {
            NPC center = Main.npc[(int)NPC.ai[0]];
            if (!center.active || center.type != ModContent.NPCType<VoltageVargant>())
            {
                NPC.active = false;
            }
            NPC.ai[1] += 2f * (float)Math.PI / 600f * 3f;
            NPC.ai[1] %= 2f * (float)Math.PI;
            NPC.Center = center.Center + 100 * new Vector2((float)Math.Cos(NPC.ai[1]), (float)Math.Sin(NPC.ai[1]));
        }
    }
}
