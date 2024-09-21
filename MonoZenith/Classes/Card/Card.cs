using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MonoZenith.Classes.Card
{
    internal abstract class Card
    {
        private Vector2 _position;
        private int _width;
        private int _height;
        private Texture2D _texture;
        private string _name;

        public abstract void ValidNextCard(Card previousCard);
    }
}
