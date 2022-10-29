﻿using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System;

namespace Regressus.Effects.Prims
{
    public static class PrimitiveHelper
    {
        private static int width;
        private static int height;
        private static Vector2 zoom;
        private static bool CheckGraphicsChanged()
        {
            var device = Main.graphics.GraphicsDevice;
            bool changed = device.Viewport.Width != width
                || device.Viewport.Height != height
                || Main.GameViewMatrix.Zoom != zoom;

            if (!changed) return false;

            width = device.Viewport.Width;
            height = device.Viewport.Height;
            zoom = Main.GameViewMatrix.Zoom;

            return true;
        }

        private static Matrix view;
        private static Matrix projection;
        public static Matrix GetMatrix()
        {
            if (CheckGraphicsChanged())
            {
                var device = Main.graphics.GraphicsDevice;
                int width = device.Viewport.Width;
                int height = device.Viewport.Height;
                Vector2 zoom = Main.GameViewMatrix.Zoom;
                view =
                    Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up)
                    * Matrix.CreateTranslation(width / 2, height / -2, 0)
                    * Matrix.CreateRotationZ(MathHelper.Pi)
                    * Matrix.CreateScale(zoom.X, zoom.Y, 1f);
                projection = Matrix.CreateOrthographic(width, height, 0, 1000);
            }

            return view * projection;
        }
        public static VertexPositionColorTexture AsVertex(Vector2 position, Color color, Vector2 textureCoordinate = default)
        {
            return new VertexPositionColorTexture(new Vector3(position, 0), color, textureCoordinate);
        }
        public static VertexPositionColorTexture AsVertex(Vector3 position, Color color, Vector2 textureCoordinate = default)
        {
            return new VertexPositionColorTexture(position, color, textureCoordinate);
        }
        private static int GetPrimitiveCount(int vertexCount, PrimitiveType type)
        {
            switch (type)
            {
                case PrimitiveType.LineList:
                    return vertexCount / 2;
                case PrimitiveType.LineStrip:
                    return vertexCount - 1;
                case PrimitiveType.TriangleList:
                    return vertexCount / 3;
                case PrimitiveType.TriangleStrip:
                    return vertexCount - 2;
                default: return 0;
            }
        }
        public static void DrawPrimitives(VertexPositionColorTexture[] vertices, PrimitiveType type, bool drawBacksides = true)
        {
            if (vertices.Length < 6) return;
            GraphicsDevice device = Main.graphics.GraphicsDevice;
            Effect effect = Regressus.TrailShader;
            effect.Parameters["WorldViewProjection"].SetValue(GetMatrix());
            effect.CurrentTechnique.Passes[0].Apply();
            if (drawBacksides)
            {
                short[] indices = new short[vertices.Length * 2];
                for (int i = 0; i < vertices.Length; i += 3)
                {
                    indices[i * 2] = (short)i;
                    indices[i * 2 + 1] = (short)(i + 1);
                    indices[i * 2 + 2] = (short)(i + 2);

                    indices[i * 2 + 5] = (short)i;
                    indices[i * 2 + 4] = (short)(i + 1);
                    indices[i * 2 + 3] = (short)(i + 2);
                }
                device.DrawUserIndexedPrimitives(type, vertices, 0, vertices.Length, indices, 0,
                    GetPrimitiveCount(vertices.Length, type) * 2);
            }
            else
            {
                device.DrawUserPrimitives(type, vertices, 0, GetPrimitiveCount(vertices.Length, type));
            }
        }
    }
    // in case you already have all the vertices set up and simply want to draw them
    public class PrimitivePacket
    {
        readonly IEnumerable<VertexPositionColorTexture> vertices;
        readonly PrimitiveType type;

        readonly int vertexCount;
        Effect effect = Regressus.TrailShader;
        string pass = "Default";
        Texture2D usedTexture;
        public PrimitivePacket(IEnumerable<VertexPositionColorTexture> vertices, PrimitiveType type, int vertexCount, Texture2D texture = default)
        {
            this.vertices = vertices;
            this.type = type;
            this.vertexCount = vertexCount;
            if (texture != default)
            {
                pass = "Texture";
                usedTexture = texture;
            }
        }
        int Count
        {
            get
            {
                switch (type)
                {
                    case PrimitiveType.LineList:
                        return vertexCount / 2;
                    case PrimitiveType.LineStrip:
                        return vertexCount - 1;
                    case PrimitiveType.TriangleList:
                        return vertexCount / 3;
                    case PrimitiveType.TriangleStrip:
                        return vertexCount - 2;
                    default: return 0;
                }
            }
        }
        public void Send()
        {
            GraphicsDevice device = Main.graphics.GraphicsDevice;

            var verticesAsArray = vertices as VertexPositionColorTexture[] ?? vertices.ToArray();
            if (Count > 0)
            {
                effect.Parameters["WorldViewProjection"].SetValue(PrimitiveHelper.GetMatrix());
                device.Textures[0] = usedTexture;
                device.Textures[1] = usedTexture;
                effect.CurrentTechnique.Passes[pass].Apply();

                device.DrawUserPrimitives(type, verticesAsArray, 0, Count);
            }
        }
    }
}
