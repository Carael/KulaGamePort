//-----------------------------------------------------------------------------
// TextControl.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KulaGame.ScreenManagement.Controls
{
    /// <summary>
    /// TextControl is a control that displays a single string of text. By default, the
    /// size is computed from the given text and spritefont.
    /// </summary>
    public class TextControl : Control
    {
        private SpriteFont font;
        private string text;

        public Color Color;

        public bool Special = false;
        private double timer = 0;
        private Vector2 offSet = Vector2.Zero;
        private Vector2 sizeOf = new Vector2(0.7f, 0.7f);

        // Actual text to draw
        public string Text
        {
            get { return text; }
            set
            {
                if (text != value)
                {
                    text = value;
                    InvalidateAutoSize();
                }
            }
        }

        // Font to use
        public SpriteFont Font
        {
            get { return Font; }
            set
            {
                if (font != value)
                {
                    font = value;
                    InvalidateAutoSize();
                }
            }
        }

        public Vector2 SizeOf
        {
            get { return sizeOf; }
            set { sizeOf = value; }
        }

        public Vector2 OffSet
        {
            get { return offSet; }
            set { offSet = value; }
        }

        public TextControl()
            : this(string.Empty, null, Color.White, Vector2.Zero)
        {
        }

        public TextControl(string text, SpriteFont font)
            : this(text, font, Color.White, Vector2.Zero)
        {
        }

        public TextControl(string text, SpriteFont font, Color color)
            : this(text, font, color, Vector2.Zero)
        {
        }

        public TextControl(string text, SpriteFont font, Color color, Vector2 position)
        {
            this.text = text;
            this.font = font;
            this.Position = position;
            this.Color = color;
        }

        public override void Update(GameTime gametime)
        {
            base.Update(gametime);

            timer = gametime.TotalGameTime.TotalSeconds;
        }

        public override void Draw(DrawContext context)
        {
            base.Draw(context);
            if(!Special)
            {
                context.SpriteBatch.DrawString(font, Text, context.DrawOffset, Color);
            }
            else
            {
                Vector2 fontOrigin = new Vector2(font.MeasureString(Text).X / 2, font.MeasureString(Text).Y / 2);
                float pulsate = (float)Math.Sin(timer * 2f) + 1;
                Vector2 scale = new Vector2(sizeOf.X + pulsate * 0.03f, sizeOf.Y + pulsate * 0.03f);
                context.SpriteBatch.DrawString(font, Text, new Vector2(context.DrawOffset.X + offSet.X, context.DrawOffset.Y + offSet.Y), Color, 0, fontOrigin, scale, SpriteEffects.None, 0);
            }
            
        }

        override public Vector2 ComputeSize()
        {
            return font.MeasureString(Text);
        }
    }
}
