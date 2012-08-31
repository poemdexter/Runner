using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Runner.ScreenFramework.Framework;

namespace Runner.Managers
{
   public class ScreenManager : DrawableGameComponent
   {
      private List<GameScreen> Screens = new List<GameScreen>();
      private List<GameScreen> ScreensToUpdate = new List<GameScreen>();
      private InputState Input = new InputState();
      private bool IsInitialized;

      // shared amongst screen components
      public GraphicsDeviceManager Graphics { get; set; }
      public SpriteBatch SpriteBatch { get; set; }
      public SpriteFont Font { get; set; }

      // constructor
      public ScreenManager(Game game, GraphicsDeviceManager graphics)
         : base(game)
      {
         Graphics = graphics;
      }

      public override void Initialize()
      {
         base.Initialize();
         IsInitialized = true;
      }

      protected override void LoadContent()
      {
         // Load content belonging to the screen manager.
         ContentManager content = Game.Content;

         SpriteBatch = new SpriteBatch(GraphicsDevice);
         Font = content.Load<SpriteFont>("font/lofi_font");

         // Tell each of the screens to load their content.
         foreach (GameScreen screen in Screens)
         {
            screen.LoadContent();
         }
      }

      protected override void UnloadContent()
      {
         // Tell each of the screens to unload their content.
         foreach (GameScreen screen in Screens)
         {
            screen.UnloadContent();
         }
      }

      public override void Update(GameTime gameTime)
      {
         Input.Update(gameTime);

         // Make a copy of the master screen list, to avoid confusion if
         // the process of updating one screen adds or removes others.
         ScreensToUpdate.Clear();

         foreach (GameScreen screen in Screens)
            ScreensToUpdate.Add(screen);

         bool otherScreenHasFocus = !Game.IsActive;

         while (ScreensToUpdate.Count > 0)
         {
            // Pop the topmost screen off the waiting list.
            GameScreen TopScreen = ScreensToUpdate[ScreensToUpdate.Count - 1];
            ScreensToUpdate.Remove(ScreensToUpdate[ScreensToUpdate.Count - 1]);
            // Update the screen.
            TopScreen.Update(gameTime);

            if (TopScreen.CurrentScreenState == ScreenState.Active)
            {
               // If this is the first active screen we came across,
               // give it a chance to handle input.
               if (!otherScreenHasFocus)
               {
                  TopScreen.HandleInput(Input);

                  otherScreenHasFocus = true;
               }
            }
         }
      }

      public override void Draw(GameTime gameTime)
      {
         foreach (GameScreen Screen in Screens)
         {
            if (Screen.CurrentScreenState == ScreenState.Hidden)
               continue;

            Screen.Draw(gameTime);
         }
      }

      public void AddScreen(GameScreen screen)
      {
         screen.ScreenManager = this;

         if (IsInitialized)
         {
            screen.LoadContent();
         }

         Screens.Add(screen);
      }

      /// <summary>
      /// Removes a screen from the screen manager. You should normally
      /// use GameScreen.ExitScreen instead of calling this directly, so
      /// the screen can gradually transition off rather than just being
      /// instantly removed.
      /// </summary>
      public void RemoveScreen(GameScreen screen)
      {
         // If we have a graphics device, tell the screen to unload content.
         if (IsInitialized)
         {
            screen.UnloadContent();
         }

         Screens.Remove(screen);
         ScreensToUpdate.Remove(screen);
      }

      public GameScreen[] GetScreens()
      {
         return Screens.ToArray();
      }
   }
}
