using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks.Sources;

namespace Pong2
{
    class Paddle
    {
        public Vector2 paddlePosition { get; set; }
        public float paddleSpeed { get; set; }
        public Texture2D tex;
        public int score;


        public Paddle(Vector2 position, float speed, Texture2D texture)
        {
            paddlePosition = position;
            paddleSpeed = speed;
            tex = texture;
            score = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(tex, paddlePosition, Color.White);
            spriteBatch.End();
        }


    }
}
