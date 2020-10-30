using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Pong2.Content
{
    class Ball
    {
        public Vector2 ballPosition { get; set; }
        public float ballSpeed{ get; set; }
        public Texture2D ballTexture;
        public Texture2D chargedUP;
        private float initialSpeed;
        public bool isCharged { get; set; }
        public int hitCount { get; set; }

        public Ball(Vector2 startLoc, float speed, Texture2D tex, Texture2D charged)
        {
            ballPosition = startLoc;
            initialSpeed = speed;
            ballSpeed = speed;
            ballTexture = tex;
            chargedUP = charged;
            isCharged = false;
            hitCount = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if (isCharged)
            {
                ballSpeed = 2.2f * initialSpeed;
                spriteBatch.Draw(chargedUP, ballPosition, Color.White);
            }
            else
            {
                ballSpeed = (4 * hitCount) + initialSpeed;
                spriteBatch.Draw(ballTexture, ballPosition, Color.White);
            }
            
            spriteBatch.End();
        }
    }
}
