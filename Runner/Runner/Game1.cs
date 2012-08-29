using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Runner
{
   public class Game1 : Microsoft.Xna.Framework.Game
   {
      private const String VERSION = "0.0.1";

      GraphicsDeviceManager graphics;
      SpriteBatch spriteBatch;
      SpriteFont lofiFont;  // fontHeight = 7px
      Texture2D playerSprite, arrowSprite;

      List<Arrow> arrowList;

      KeyboardState currentKeyboardState, previousKeyboardState;
      MouseState currentMouseState, previousMouseState;
      int inputElapsedTime = 0;

      

      public Game1()
      {
         graphics = new GraphicsDeviceManager(this);
         Content.RootDirectory = "Content";
         graphics.PreferredBackBufferHeight = GameUtil.windowHeight;
         graphics.PreferredBackBufferWidth = GameUtil.windowWidth;
         this.IsMouseVisible = true;
      }

      protected override void Initialize()
      {
         base.Initialize();
         arrowList = new List<Arrow>();
      }

      protected override void LoadContent()
      {
         spriteBatch = new SpriteBatch(GraphicsDevice);
         lofiFont = Content.Load<SpriteFont>("font/lofi_font");
         playerSprite = Content.Load<Texture2D>("player/char_bandit");
         arrowSprite = Content.Load<Texture2D>("entities/arrow");
      }

      protected override void UnloadContent()
      {
      }

      protected override void Update(GameTime gameTime)
      {
         // this.Exit();
         previousKeyboardState = currentKeyboardState;
         previousMouseState = currentMouseState;
         currentKeyboardState = Keyboard.GetState();
         currentMouseState = Mouse.GetState();
         HandleInput(currentKeyboardState, currentMouseState, gameTime);

         if (arrowList.Count > 0)
         {
            foreach (Arrow arrow in arrowList)
            {
               if (arrow.X > GameUtil.windowWidth)
                  arrow.IsDead = true;
               else
                  arrow.Tick();
            }
         }

         CleanArrowList();

         base.Update(gameTime);
      }

      private void CleanArrowList()
      {
         for (int i = arrowList.Count - 1; i >= 0; --i)
         {
            if (arrowList[i].IsDead)
               arrowList.RemoveAt(i);
         }
      }

      private void HandleInput(KeyboardState keyboard, MouseState mouse, GameTime gameTime)
      {
         inputElapsedTime -= gameTime.ElapsedGameTime.Milliseconds;

         if (inputElapsedTime <= 0)
         {
            if (keyboard.IsKeyDown(Keys.Space) || mouse.LeftButton == ButtonState.Pressed)
            {
               // fire ze arrow!
               arrowList.Add(new Arrow(150, 500));
               inputElapsedTime = 200;
            }
         }
      }

      protected override void Draw(GameTime gameTime)
      {
         GraphicsDevice.Clear(Color.CornflowerBlue);

         spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null);

         spriteBatch.DrawString(lofiFont, "Runner Prototype " + VERSION, new Vector2(20, 20), Color.White, 0, Vector2.Zero, GameUtil.fontScale, SpriteEffects.None, 0);

         spriteBatch.Draw(playerSprite, new Vector2(100, 500), playerSprite.Bounds, Color.White, 0f, Vector2.Zero, GameUtil.spriteScale, SpriteEffects.None, 0);

         if (arrowList.Count > 0)
         {
            foreach (Arrow arrow in arrowList)
            {
               spriteBatch.Draw(arrowSprite, new Vector2(arrow.X, arrow.Y), arrowSprite.Bounds, Color.White, 0f, Vector2.Zero, GameUtil.spriteScale, SpriteEffects.None, 0);
            }
         }

         spriteBatch.End();

         base.Draw(gameTime);
      }
   }
}
