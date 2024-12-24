using System;
using Microsoft.Xna.Framework;

namespace MonoZenith.Screen
{
    public abstract class Screen
    {
        public bool IsUnloading { get; set; } = false;
        
        /// <summary>
        /// Removes all side effects of the screen when switching to another screen.
        /// </summary>
        public abstract void Unload(Action unOnloadComplete = null);

        /// <summary>
        /// Load state.
        /// </summary>
        public abstract void Load();
        
        /// <summary>
        /// Update state.
        /// </summary>
        /// <param name="deltaTime">GameTime object.</param>
        public abstract void Update(GameTime deltaTime);

        /// <summary>
        /// Draw state.
        /// </summary>
        public abstract void Draw();
    }
}
