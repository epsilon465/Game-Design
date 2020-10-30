using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pong2.Content;
using System;

namespace Pong2
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D redPaddleImg;
        private Texture2D bluePaddleImg;
        private Texture2D backgroundImg;
        private Texture2D ballImg;
        private Texture2D chargedImg;
        System.Random random = new System.Random();
        private double radPerDeg = Math.PI / 180;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            base.Initialize();

        }


        private int gameState = 0;
        private Paddle paddleB;
        private Paddle paddleR;
        private Ball ball;
        private SpriteFont font;
        double ballAngle = 0;

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load background and paddle images 
            bluePaddleImg = Content.Load<Texture2D>("BluePaddle");
            redPaddleImg = Content.Load<Texture2D>("RedPaddle");
            backgroundImg = Content.Load<Texture2D>("Background");
            ballImg = Content.Load<Texture2D>("Ball");
            chargedImg = Content.Load<Texture2D>("Charged");
            font = Content.Load<SpriteFont>("Font");

            // Add paddles first
            paddleB = new Paddle(new Vector2(30, 240-bluePaddleImg.Height/2), 350f, bluePaddleImg);
            paddleR = new Paddle(new Vector2(770-redPaddleImg.Width, 240-redPaddleImg.Height / 2), 350f, redPaddleImg);
            ball = new Ball(new Vector2(60, 240 - ballImg.Height / 2), 350f, ballImg, chargedImg);
            
        }



        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var kstate = Keyboard.GetState();

            Vector2 bluePos = paddleB.paddlePosition;
            Vector2 redPos = paddleR.paddlePosition;
            Vector2 ballPos = ball.ballPosition;


            if (kstate.IsKeyDown(Keys.W))
                bluePos.Y -= paddleB.paddleSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;   
            if (kstate.IsKeyDown(Keys.S))
                bluePos.Y += paddleB.paddleSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (bluePos.Y > _graphics.PreferredBackBufferHeight - bluePaddleImg.Height)
                bluePos.Y = _graphics.PreferredBackBufferHeight - bluePaddleImg.Height;
            else if (bluePos.Y < 0)
                bluePos.Y = 0;


            if (kstate.IsKeyDown(Keys.Up))
                redPos.Y -= paddleR.paddleSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (kstate.IsKeyDown(Keys.Down))
                redPos.Y += paddleR.paddleSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (redPos.Y > _graphics.PreferredBackBufferHeight - redPaddleImg.Height)
                redPos.Y = _graphics.PreferredBackBufferHeight - redPaddleImg.Height;
            else if (redPos.Y < 0)
                redPos.Y = 0;


            if (gameState == 0)
            {
                ballPos.X = bluePos.X + bluePaddleImg.Width;
                ballPos.Y = bluePos.Y + bluePaddleImg.Height / 2 - ballImg.Height / 2;

            }
            if (kstate.IsKeyDown(Keys.Space) && gameState == 0)
            {
                gameState = 1;
                ballAngle = (double)(10 * random.Next(4, 15));
            }

            if (gameState == 2)
            {
                ballPos.X = redPos.X - ballImg.Width;
                ballPos.Y = redPos.Y + redPaddleImg.Height / 2 - ballImg.Height / 2;

            }
            if (kstate.IsKeyDown(Keys.Space) && gameState == 2)
            {
                gameState = 1;
                ballAngle = (double)(10 * random.Next(21, 33));
            }

            if (gameState == 1)
            {

                ballPos.X += ball.ballSpeed * (float)Math.Sin(ballAngle * radPerDeg) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                ballPos.Y -= ball.ballSpeed * (float)Math.Cos(ballAngle * radPerDeg) * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (ballPos.Y > _graphics.PreferredBackBufferHeight - ballImg.Height)
                {
                    ballPos.Y = _graphics.PreferredBackBufferHeight - ballImg.Height - 1;
                    ballAngle = 180 - ballAngle;
                }
                else if (ballPos.Y < 0)
                {
                    ballPos.Y = 0;
                    ballAngle = 180 - ballAngle;
                }

                //  && (ballPos.Y < redPos.Y + redPaddleImg.Height) && (ballPos.Y > redPos.Y)

                if ((ballPos.X > 770 - redPaddleImg.Width - ballImg.Width) && ballPos.X < 750 && (ballPos.Y < redPos.Y + redPaddleImg.Height) && (ballPos.Y + ballImg.Height > redPos.Y))
                {
                    ball.isCharged = false;
                    ballAngle = 360 - ballAngle + random.Next(-60, 60);
                    if (ballAngle > 330 || ballAngle < 210)
                    {
                        ballAngle = 270;;
                        ball.isCharged = true;
                        ball.hitCount += 1;
                    }
                }

                if ((ballPos.X < 30 + bluePaddleImg.Width) && ballPos.X > 10 && (ballPos.Y < bluePos.Y + bluePaddleImg.Height) && (ballPos.Y + ballImg.Height > bluePos.Y))
                {
                    ball.isCharged = false;
                    ballAngle = 360 - ballAngle + random.Next(-60, 60);
                    if (ballAngle > 150 || ballAngle < 30)
                    {
                        ballAngle = 90;
                        ball.isCharged = true;
                        ball.hitCount += 1;
                    }
                    
                }

                if (ballPos.X > 799)
                {
                    paddleB.score++;
                    gameState = 0;
                }
                if (ballPos.X < 1)
                {
                    paddleR.score++;
                    gameState = 2;
                }

            }



            paddleB.paddlePosition = bluePos;
            paddleR.paddlePosition = redPos;
            ball.ballPosition = ballPos;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw background
            _spriteBatch.Begin();
            _spriteBatch.Draw(backgroundImg, new Rectangle(0, 0, 800, 480), Color.White);
            _spriteBatch.End();

            // Draw ball
            ball.Draw(_spriteBatch);

            // Draw paddle
            paddleB.Draw(_spriteBatch);
            paddleR.Draw(_spriteBatch);

            _spriteBatch.Begin();
            if (gameState == 0 || gameState == 2)
            {
                _spriteBatch.DrawString(font, "Press space to launch ball", new Vector2(100, 100), Color.LawnGreen);
            }
            _spriteBatch.DrawString(font, paddleB.score+ "", new Vector2(360, 22), Color.LawnGreen);
            _spriteBatch.DrawString(font, "|", new Vector2(395, 22), Color.LawnGreen);
            _spriteBatch.DrawString(font,""+paddleR.score, new Vector2(410, 22), Color.LawnGreen);
            _spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
