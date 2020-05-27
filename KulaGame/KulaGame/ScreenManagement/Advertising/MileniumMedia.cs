//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Media;
//using WP7XNASDK;


//namespace KulaGame.ScreenManagement.Advertising
//{
//    /// <summary>
//    /// This is a game component that implements IUpdateable.
//    /// </summary>
//    public class MileniumMedia : DrawableGameComponent
//    {
//        private const int MINIMAL_REFRESH_TIME = 30;
//        private const string APP_ID = "49100";

//        MMAdView adView;

//        public MileniumMedia(Game game)
//            : base(game)
//        {
//        }

//        /// <summary>
//        /// Allows the game component to perform any initialization it needs to before starting
//        /// to run.  This is where it can query for any required services and load content.
//        /// </summary>
//        public override void Initialize()
//        {

//            base.Initialize();
//            adView = new MMAdView(GraphicsDevice, AdPlacement.BannerAdTop);
//            adView.Apid = APP_ID;
//            adView.RefreshTimer = MINIMAL_REFRESH_TIME;
//        }

//        /// <summary>
//        /// Allows the game component to update itself.
//        /// </summary>
//        /// <param name="gameTime">Provides a snapshot of timing values.</param>
//        public override void Update(GameTime gameTime)
//        {
//            // TODO: Add your update code here
//            adView.Update(gameTime);

//            base.Update(gameTime);
//        }
//        public override void Draw(GameTime gameTime)
//        {
//            adView.Draw(gameTime);
//            base.Draw(gameTime);
//        }
//    }
//}
