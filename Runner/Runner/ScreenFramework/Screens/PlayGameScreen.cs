using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Runner.ScreenFramework.Framework;
using Runner.EntityFramework.Components;
using Runner.EntityFramework.Entities;
using Runner.EntityFramework.Args;
using Runner.EntityFramework.Actions.AttackAI;
using Runner.Managers;

namespace Runner.ScreenFramework.Screens
{
    class PlayGameScreen : GameScreen
    {
        private LevelManager levelManager;
        private GraphicsDevice Graphics;
        private SpriteBatch Batch;
        private SpriteFont Font;

        Vector2 cam1_position = Vector2.Zero, cam2_position = Vector2.Zero;
        Vector2 cam_velocity = new Vector2(-1, 0);

        public PlayGameScreen()
        {
            levelManager = new LevelManager();
        }

        public override void LoadContent()
        {
            Graphics = ScreenManager.GraphicsDevice;
            Batch = ScreenManager.SpriteBatch;
            Font = ScreenManager.Font;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            levelManager.Update(gameTime);
            levelManager.AnimateObjects(gameTime);
            levelManager.CheckCollisions(ScreenManager, this);
            levelManager.CheckMobAI();
        }

        public override void HandleInput(InputState input)
        {
            // firing arrows
            if (input.IsNewKeyPress(Keys.Z, GameUtil.arrowDelay) || input.IsNewLeftClick(GameUtil.arrowDelay)) // fire weapon
            {
                levelManager.Fire(input.GetMousePosition());
            }

            levelManager.HandlePlayerJump(input);
        }

        public override void Draw(GameTime gameTime)
        {
            cam1_position += cam_velocity;
            cam2_position += cam_velocity;
            if (cam1_position.X <= -40)
                cam1_position = Vector2.Zero;
            if (cam2_position.X <= -200)
                cam2_position = Vector2.Zero;

            Graphics.Clear(Color.CornflowerBlue);

            // draw ground
            Batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.CreateTranslation(new Vector3(cam1_position * 4f, 0)));
            Batch.Draw(GameUtil.spriteDictionary["ground"], Vector2.Zero, GameUtil.spriteDictionary["ground"].Bounds, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0);
            Batch.End();

            // draw background
            Batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.CreateTranslation(new Vector3(cam2_position * .8f, 0)));
            Batch.Draw(GameUtil.spriteDictionary["background"], Vector2.Zero, GameUtil.spriteDictionary["background"].Bounds, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0);
            Batch.End();


            // draw everything else
            Batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null);

            Batch.DrawString(Font, "Runner Prototype " + GameUtil.VERSION, new Vector2(20, 20), Color.White, 0, Vector2.Zero, GameUtil.fontScale, SpriteEffects.None, 0);
            Batch.DrawString(Font, "MOBS KILLED: " + levelManager.Score, new Vector2(20, 40), Color.White, 0, Vector2.Zero, GameUtil.fontScale, SpriteEffects.None, 0);

            // draw player HP
            for (int i = 0; i < (levelManager.player.GetComponent("Hitpoints") as Hitpoints).HP; i++)
            {
                Batch.Draw(GameUtil.spriteDictionary["heart"], new Vector2(20 + (35 * i),60), GameUtil.spriteDictionary["heart"].Bounds, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }

            levelManager.Draw(Batch);

            Batch.End();

            base.Draw(gameTime);
        }

        
    }
}
