using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace KulaGame.Engine.Inferfaces
{
    public interface IModel
    {
        void Update(GameTime gameTime);
        Matrix GetWorldMatrix();
        void Draw(GameTime gameTime, ICamera camera);
    }
}
