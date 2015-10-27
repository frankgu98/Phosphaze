using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Phosphaze.Core.Collision;

namespace Phosphaze.Core.SceneElements
{

    /// <summary>
    /// I considered making this inherit from SceneButton, but that class is so ugly and messed up
    /// I'm scared it would break something by its shear presence.
    /// </summary>
    public class FadingSceneButton
    {

        /// <summary>
        /// The currently loaded texture.
        /// </summary>
        Texture2D mainTexture;

        /// <summary>
        /// The texture when the state is VisibleHovering.
        /// </summary>
        Texture2D hoverTexture;

        /// <summary>
        /// The texture when the state is VisibleClicked.
        /// </summary>
        Texture2D clickedTexture;

        /// <summary>
        /// The position of the button.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// The value by which to multiply the y-value increment during fade ins and fade outs.
        /// </summary>
        public double YMultiplier { get; set; }

        /// <summary>
        /// This button's collision mask.
        /// </summary>
        RectCollider collider;

        /// <summary>
        /// The rectangle representing this button (which also acts as its collision mask).
        /// </summary>
        public Rectangle Rect {
            get { return collider.GetBoundingBox(); } 
            set { collider = new RectCollider(value); } 
        }

        public double LocalTime { get; private set; }

        /// <summary>
        /// The possible states this button can be in. Each state determines a 
        /// different way to render and update the button.
        /// </summary>
        enum States { FadingIn, Visible, VisibleHovering, VisibleClicked, FadingOut, Invisible }

        /// <summary>
        /// The current state this button is in.
        /// </summary>
        States currentState = States.Invisible;

        /// <summary>
        /// The duration of a fade.
        /// </summary>
        public double FadeDuration { get; set; }

        /// <summary>
        /// The time of beginning a fade (in or out).
        /// </summary>
        private double fadeStart = 0;

        /// <summary>
        /// The current alpha value (i.e. the transparency)
        /// </summary>
        private float alpha = -0.1f;

        private float alphaIncrement
        {
            get
            {
                // The division by 60000 turns converts from milliseconds into seconds, and seconds into frames.
                // For example, if FadeDuration = 1000 (1 second), then it would take 60 frames for alpha to become
                // 255 starting at 0, thus alphaIncrement = 1000/60000.
                return (float) (1d / (((FadeDuration) / 1000d) * 60d));
            }
        }

        public FadingSceneButton(
            string textureName, Vector2 position, Rectangle collisionRect, 
            double fadeDuration, double yOffset, bool centred = true)
            : this(textureName, textureName, textureName, position, collisionRect, fadeDuration, yOffset, centred) { }

        public FadingSceneButton(
            string textureName, string textureHover, string textureClicked, 
            Vector2 position, Rectangle collisionRect, double fadeDuration, 
            double yOffset, bool centred=true)
        {
            mainTexture = Globals.content.Load<Texture2D>(textureName);
            hoverTexture = Globals.content.Load<Texture2D>(textureHover);
            clickedTexture = Globals.content.Load<Texture2D>(textureClicked);

            YMultiplier = 10 * yOffset;
            Rect = collisionRect;
            
            if (centred)
            {
                var bb = collisionRect;
                var delta = new Vector2();

                delta.X = position.X - bb.Width / 2;
                delta.Y = position.Y - bb.Height / 2;
                Console.WriteLine(delta);
                collider.Translate(delta);
            }
            else
                collider.Translate(position);

            FadeDuration = fadeDuration;
        }

        private bool CollideMouse()
        {
            return CollisionChecker.Collision(collider, new ParticleCollider(Globals.currentMouseState.Position));
        }

        /// <summary>
        /// Update the button.
        /// </summary>
        public void Update()
        {
            LocalTime += Globals.deltaTime;
            
            if (currentState == States.FadingIn)
            {
                alpha += alphaIncrement;
                collider.Translate(new Vector2(0, (float)(-alphaIncrement * YMultiplier)));
                if (alpha >= 1)
                {
                    alpha = 1f;
                    currentState = States.Visible;
                }
            }
            else if (currentState == States.FadingOut)
            {
                alpha -= alphaIncrement;
                collider.Translate(new Vector2(0, (float)(alphaIncrement * YMultiplier)));
                if (alpha <= 0)
                {
                    alpha = -0.1f;
                    currentState = States.Invisible;
                }
            }
            else if (currentState != States.Invisible)
            {
                var mouseHover = CollideMouse();
                var clicked = Globals.currentMouseState.LeftButton == ButtonState.Pressed;
                currentState = (clicked && mouseHover) ? States.VisibleClicked : (mouseHover ? States.VisibleHovering : States.Visible);
            }
        }

        /// <summary>
        /// Check if the button is clicked or not.
        /// </summary>
        /// <returns></returns>
        public bool IsClicked()
        {
            return CollideMouse() && Globals.currentMouseState.LeftButton == ButtonState.Released &&
                  Globals.previousMouseState != null && Globals.previousMouseState.LeftButton == ButtonState.Pressed &&
                  currentState == States.VisibleHovering;
        }

        public void Draw()
        {
            if (currentState == States.Invisible)
                return;
            Texture2D texture = null;
            // Get the appropriate texture depending on the current state.
            switch (currentState)
            {
                case States.Visible:
                case States.FadingIn:
                case States.FadingOut:
                    texture = mainTexture;
                    break;
                case States.VisibleHovering:
                    texture = hoverTexture;
                    break;
                case States.VisibleClicked:
                    texture = clickedTexture;
                    break;
                default:
                    break;
            }
            Globals.spriteBatch.Draw(texture, Rect, Color.White * (alpha >= 0 ? alpha : 0));
        }

        public void FadeIn()
        {
            Fade(States.FadingIn);
        }

        public void FadeOut()
        {
            Fade(States.FadingOut);
        }

        private void Fade(States fadeType)
        {
            currentState = fadeType;
            fadeStart = LocalTime;
        }

    }
}
