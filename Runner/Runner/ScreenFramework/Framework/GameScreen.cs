using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Runner.Managers;

namespace Runner.ScreenFramework.Framework
{
   public enum ScreenState
   {
      Active,
      Hidden
   }

   public abstract class GameScreen
   {
      // get the screen manager this screen belongs to
      public ScreenManager ScreenManager { get; set; }

      public ScreenState CurrentScreenState = ScreenState.Active;

      public virtual void LoadContent() { }
      public virtual void UnloadContent() { }
      public virtual void HandleInput(InputState input) { }
      public virtual void Draw(GameTime gameTime) { }
      public virtual void Update(GameTime gameTime) { }

      public void ExitScreen()
      {
         ScreenManager.RemoveScreen(this);
      }
   }
}
