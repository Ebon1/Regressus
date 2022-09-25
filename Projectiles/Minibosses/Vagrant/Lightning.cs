using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Regressus.Projectiles.Minibosses.Vagrant;

public class Lightning : ModProjectile
{
    public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.StardustTowerMark;
    public override void SetDefaults()
    {
        Projectile.tileCollide = false;
        Projectile.timeLeft = 30;
        Projectile.friendly = false;
        Projectile.hostile = true;
        Projectile.penetrate = -1;
        Projectile.aiStyle = -1;
    }
    public override bool ShouldUpdatePosition() => false;

    public override void AI()
    {
        if (Projectile.localAI[0] == 0)
        {
            /*if (Projectile.ai[0] > Threshold * 2)
            {*/
            GenerateLightning(Projectile.Center - Vector2.UnitY * 500, Projectile.Center + Vector2.UnitY * 500);
            SoundStyle style = new SoundStyle("Regressus/Sounds/Custom/thunder");
            style.PitchVariance = 1;
            style.MaxInstances = 0;
            style.Volume = 2f - Projectile.ai[1];
            SoundEngine.PlaySound(style, Projectile.Center);
            /*}
            else
            {
                Main.NewText($"Lighting length too small ({Projectile.ai[0]} is under threshold of {Threshold * 2})");
                Projectile.Kill();
            }*/
            RegreUtils.Log(Projectile);
            Projectile.localAI[0] = 1;
        }
    }

    private const float Randomness = 75f;
    private const float Threshold = 10f;
    private const float Thickness = 10f;
    private readonly List<Vector2> LightningPoints = new List<Vector2>();
    private List<float> Points = new List<float>();
    public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
    {
        return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center - Vector2.UnitY * 500, Projectile.Center + Vector2.UnitY * 500);
    }
    public void GenerateLightning(Vector2 start, Vector2 end)
    {
        Points.Clear();
        LightningPoints.Clear();
        float len = (end - start).Length();
        Points.Add(0f);
        float range = len / Threshold;
        for (int i = 0; i < len / Threshold; i++)
            Points.Add(i / range + Main.rand.NextFloat(-range, range) / 3);
        Points.Add(1f);
        Points.Sort();

        Vector2 dir = (end - start).SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.PiOver2);
        for (int i = 0; i < Points.Count; i++)
        {
            float offset = Main.rand.NextFloat(-Randomness, Randomness);
            Vector2 pos = Vector2.Lerp(start, end, Points[i]) + dir * offset;
            LightningPoints.Add(pos);
        }
    }
    public override bool PreDraw(ref Color lightColor)
    {
        if (LightningPoints.Count > 2 && !Main.gameInactive)
        {
            Texture2D tex = ModContent.Request<Texture2D>("Regressus/Extras/laser2").Value;
            VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[(LightningPoints.Count - 1) * 6];
            for (int i = 0; i < LightningPoints.Count - 1; i++)
            {
                Vector2 pos1 = LightningPoints[i] - Main.screenPosition;
                Vector2 pos2 = LightningPoints[i + 1] - Main.screenPosition;
                Vector2 dir1 = RegreUtils.GetRotation(LightningPoints, i) * Thickness;
                Vector2 dir2 = RegreUtils.GetRotation(LightningPoints, i + 1) * Thickness;
                float prog1 = Points[i];
                float prog2 = Points[i + 1];
                Vector2 v1 = pos1 + dir1; // bottom left
                Vector2 v2 = pos1 - dir1; // top left
                Vector2 v3 = pos2 + dir2; // bottom right
                Vector2 v4 = pos2 - dir2; // top right
                Color color = Color.LightBlue * ((float)Projectile.timeLeft / 30);
                vertices[i * 6] = RegreUtils.AsVertex(v1, color, new Vector2(prog1, 0));
                vertices[i * 6 + 1] = RegreUtils.AsVertex(v3, color, new Vector2(prog2, 0));
                vertices[i * 6 + 2] = RegreUtils.AsVertex(v4, color, new Vector2(prog2, 1));

                vertices[i * 6 + 3] = RegreUtils.AsVertex(v4, color, new Vector2(prog2, 1));
                vertices[i * 6 + 4] = RegreUtils.AsVertex(v2, color, new Vector2(prog1, 1));
                vertices[i * 6 + 5] = RegreUtils.AsVertex(v1, color, new Vector2(prog1, 0));
            }
            RegreUtils.DrawTexturedPrimitives(vertices, PrimitiveType.TriangleList, tex);
        }
        return false;
    }
}