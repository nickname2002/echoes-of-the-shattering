using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MonoZenith.Classes.Screen
{
    internal abstract class Screen
    {
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
