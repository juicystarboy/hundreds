using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Julian_Hundreds
{
    class Ball
    {
        public int x;
        public int y;
        public int ix;
        public int iy;
        public int identifier;
        public int colsize = 50;
        public int size = 50;
        public bool lost;
        public Rectangle Hitbox;
        public Rectangle Y;
        public Rectangle X;
        List<Ball> balls;
        Texture2D circle;
        GraphicsDevice graphicsdevice;
        float scale = 1f;
        public bool isGrowing = false;

        public Ball(int x, int y, int ix, int iy, Texture2D circle, GraphicsDevice graphicsdevice, int identifier)
        {
            this.identifier = identifier;
            this.x = x;
            this.y = y;
            this.ix = ix;
            this.iy = iy;
            this.circle = circle;
            this.graphicsdevice = graphicsdevice;
            circle = new Texture2D(graphicsdevice, colsize, colsize);
            Hitbox = new Rectangle(x, y, colsize, colsize);
        }

        public void Move(List<Ball> balls)
        {
            isGrowing = false;
            this.balls = balls;
            int cursorxdis = Math.Abs(Mouse.GetState().Position.X - Y.X);
            int cursorydis = Math.Abs(Mouse.GetState().Position.Y - X.Y);
            int cursordis = (int)Math.Sqrt((cursorxdis * cursorxdis) + (cursorydis * cursorydis));

            Y = new Rectangle(x + ((int)(size * scale) / 2), y, 1, (int)(size * scale));
            X = new Rectangle(x, y + ((int)(size * scale) / 2), (int)(size * scale), 1);
            if (x + colsize >= graphicsdevice.Viewport.Width && ix > 0)
            {
                ix = -Math.Abs(ix);
            }
            else if (x <= 0 && ix < 0)
            {
                ix = Math.Abs(ix);
            }
            if (y + colsize >= graphicsdevice.Viewport.Height && iy > 0)
            {
                iy = -Math.Abs(iy);
            }
            if (y <= 0 && iy < 0)
            {
                iy = Math.Abs(iy);
            }
            if (cursordis < colsize / 2)
            {
                scale += 0.05f;
                colsize = (int)(size * scale);
                x -= (int)(.025 * size);
                y -= (int)(.025 * size);
                isGrowing = true;
            }
            foreach (Ball b in balls)
            {
                bool isColliding = false;
                if (b != this)
                {
                    int xdis = (b.Y.X - Y.X);
                    int ydis = (b.X.Y - X.Y);
                    int distance = (int)Math.Sqrt((xdis * xdis) + (ydis * ydis));
                    int minDis = colsize / 2 + b.colsize / 2;
                    if (b.identifier != identifier)
                    {
                        if (b.Hitbox.Intersects(Hitbox))
                        {
                            if (xdis > 0 && ydis > 0 && distance <= (colsize / 2 + b.colsize / 2))
                            {
                                ix = -Math.Abs(ix);
                                iy = -Math.Abs(iy);
                                b.ix = Math.Abs(b.ix);
                                b.iy = Math.Abs(b.iy);
                                isColliding = true;
                            }
                            if (xdis < 0 && ydis < 0 && distance <= (colsize / 2 + b.colsize / 2))
                            {
                                ix = Math.Abs(ix);
                                iy = Math.Abs(iy);
                                b.ix = -Math.Abs(b.ix);
                                b.iy = -Math.Abs(b.iy);
                                isColliding = true;
                            }
                            if (xdis < 0 && ydis > 0 && distance <= (colsize / 2 + b.colsize / 2))
                            {
                                ix = Math.Abs(ix);
                                iy = -Math.Abs(iy);
                                b.ix = -Math.Abs(b.ix);
                                b.iy = Math.Abs(b.iy);
                                isColliding = true;
                            }
                            if (xdis > 0 && ydis < 0 && distance <= (colsize / 2 + b.colsize / 2))
                            {
                                ix = -Math.Abs(ix);
                                iy = Math.Abs(iy);
                                b.ix = Math.Abs(b.ix);
                                b.iy = -Math.Abs(b.iy);
                                isColliding = true;
                            }
                        }
                    }
                    cursorxdis = Math.Abs(Mouse.GetState().Position.X - Y.X);
                    cursorydis = Math.Abs(Mouse.GetState().Position.Y - X.Y);
                    cursordis = (int)Math.Sqrt((cursorxdis * cursorxdis) + (cursorydis * cursorydis));
                    //if (cursordis < colsize / 2)
                    //{
                    //xdis = (b.Y.X - Y.X);
                    //ydis = (b.X.Y - X.Y);
                    //distance = (int)Math.Sqrt((xdis * xdis) + (ydis * ydis));
                    //minDis = colsize / 2 + b.colsize / 2;
                    if (isGrowing && isColliding || b.isGrowing && isColliding /*distance < minDis*/)
                    {
                        lost = true;
                        b.lost = true;
                    }
                    //}
                }

            }
            x += ix;
            y += iy;
            Hitbox = new Rectangle(x, y, (int)(size * scale), (int)(size * scale));
        }

        public void Draw(SpriteBatch spritebatch, SpriteFont arial)
        {
            //spritebatch.Draw(circle, new Vector2(Y.Y, X.X), null, Color.White, 0f, new Vector2(Y.X, X.Y), scale, SpriteEffects.None, 0f);
            spritebatch.Draw(circle, Hitbox, Color.White);
            //spritebatch.DrawString(arial, ix.ToString(), new Vector2((Y.X-arial.MeasureString(ix.ToString()).X/2), X.Y-14), Color.White);
            //spritebatch.DrawString(arial, iy.ToString(), new Vector2((Y.X - arial.MeasureString(ix.ToString()).X / 2), X.Y + 2), Color.White);
        }
    }
}
