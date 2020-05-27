using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using KulaGame.Engine.GameObjects.Camera;
using KulaGame.Engine.GameObjects.Level;
using KulaGame.Engine.GameObjects.Models;
using KulaGame.Engine.Utils;
using KulaGame.ScreenManagement.ScreenManager;
using LevelDefinition;
//using Microsoft.Devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
//using Microsoft.Devices.Sensors;

namespace KulaGame.ScreenManagement.Screens
{
    class GameplayScreen : GameScreen
    {
        private bool takingPictures = false;

        private GameLevel _gameLevel;
        private BehindCamera _camera;

        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private SpriteFont _statusFont;

        private string _drawText = string.Empty;
        private int _coinsCollected = 0;
        private int _fruitsCollected = 0;
        private bool _keyFound = false;

        private Campaign _campaign;
        private Level _level;
        private int _timer = 60;
        private int _timeElapsed = 0;
        private GameState _gameState = GameState.BeforeStart;
        private ContentManager _content;
        Song _song;

        private Texture2D _star;

        private int _campaignId;
        private int _levelId;
        private int _points;

        private bool _vibrate;

        private bool _wasSaved;

        private int _maxPointsPossible;

        private int _bonusMultiplier = 1;
        private int _bonusMultiplierTime = 0;
        private bool _isBonusActive = false;

        private readonly float _oneStar = .20f;
        private readonly float _twoStar = .40f;
        private readonly float _threeStars = .60f;
        private readonly float _fourStars = .80f;
        private readonly float _fiveStars = 1;

        private int _howManyStars;


        private bool _teleport = true;

        //Timer
        private Clock clock;
        private Texture2D _clockBody;
        private Texture2D _clockShield;
        private Texture2D _clockPointer;

        private Texture2D coinTexture;

        private Texture2D menuTexture;
        //Crystal timeBar
        private TimeBar crystalTimeBar;

        //VibrateController _vibrationController = VibrateController.Default;

        private Pad pad;
        private AccelInput accelerometer;
        private SoundManager soundManager;
        private SpecialEffects effects;
        private Texture2D keyTexture;

        private Texture2D backTexture;
        private Texture2D nextTexture;
        private Texture2D againTexture;
        private Texture2D again2Texture;

        public GameplayScreen(int campaignId, int levelId)
            : base()
        {
            EnabledGestures = GestureType.Tap | GestureType.Pinch | GestureType.DoubleTap;
            _campaignId = campaignId;
            _levelId = levelId;
            _wasSaved = false;
            _howManyStars = 0;

        }

        public override void LoadContent()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");
            //initialize content
            _spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);
            _font = _content.Load<SpriteFont>("Fonts/StandardFont");
            _statusFont = _content.Load<SpriteFont>("Fonts/Andy");
            _star = _content.Load<Texture2D>("Images/Other/star");
            
            coinTexture = _content.Load<Texture2D>(@"ScreenManager\coin");
            menuTexture = _content.Load<Texture2D>(@"ScreenManager\menuBoard");

            _vibrate = ReadVibrationStatus();

            //load campaign and level
            Campaign campaign = _content.Load<Campaign[]>("GameDefinition").SingleOrDefault(p => p.Id == _campaignId);
            Level level = campaign.Levels.SingleOrDefault(p => p.Id == _levelId);
            //store the campaign and level in local variables
            _level = level;
            _campaign = campaign;
            //load the aprproiate game level
            _gameLevel = new GameLevel(_content, _campaign, _level);
            //initialize the level timer
            _timer = level.TimeLimit;
            
            
            accelerometer = new AccelInput();
            pad = new Pad(_content, new Vector2(120, 367), new Vector2(690, 375), accelerometer);
            _gameLevel.Kula.accelerometer = accelerometer;
            _gameLevel.Kula.pad = pad;

            Vector3 kulaPosition = _gameLevel.Kula.GetPosition();
            //initialize camera
            _camera = new BehindCamera(ScreenManager.GraphicsDevice.Viewport.AspectRatio, kulaPosition, _gameLevel.Kula.InitialRotation);

            _maxPointsPossible = _level.Coins.Count + (_level.Fruits.Count * 5);
           
            //load and start playing the song
            soundManager = new SoundManager(_content, _campaignId);
            _gameLevel.Kula.soundManager = soundManager;
            //Timer
            _clockBody = _content.Load<Texture2D>(@"Textures\zolty");
            _clockShield = _content.Load<Texture2D>(@"Textures\plansza");
            _clockPointer = _content.Load<Texture2D>(@"Textures\wskazowka");
            clock = new Clock(_clockBody, _clockShield, _clockPointer);
            clock.soundManager = soundManager;
            //Clock
            clock.StartingTime = (float)level.TimeLimit;
            // Timebar
            crystalTimeBar = new TimeBar(250, Color.Orange, new Vector2(400, 45), _content);

            effects = new SpecialEffects(_content);
            _gameLevel.Kula.specialEffect = effects;

            keyTexture = _content.Load<Texture2D>(@"Textures\key");
            backTexture = _content.Load<Texture2D>(@"Textures\back");;
            nextTexture = _content.Load<Texture2D>(@"Textures\next"); ;
            againTexture = _content.Load<Texture2D>(@"Textures\again"); ;
            again2Texture = _content.Load<Texture2D>(@"Textures\again2"); ;

            base.LoadContent();
        }


        public bool ReadVibrationStatus()
        {
            IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();
            StreamReader reader = new StreamReader(new IsolatedStorageFileStream("KulaGame\\vibration.txt", FileMode.OpenOrCreate, myStore));
            string valueS = reader.ReadLine();
            reader.Close();
            int value = 1;
            try
            {
                value = int.Parse(valueS);

            }
            catch (Exception)
            {
                value = 1;
            }
            return value == 1 ? true : false;
        }





        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            effects.Update(gameTime);

            if (_gameState == GameState.BeforeStart)
            {
                _camera.ZoomInTheCamera();
                _camera.UpdateCamera(_gameLevel.Kula.GetPosition(), _gameLevel.Kula.RotationMatrix, _gameLevel.Kula.GetUpVector());
            }
            else if (_gameState == GameState.Active)
            {
                _timeElapsed += gameTime.ElapsedGameTime.Milliseconds;

                 // Dpad
                pad.Update(gameTime);
                //accelerometer
                accelerometer.Update(gameTime);
                //Clock
                clock.Update(gameTime);
                // Crystal timer
                crystalTimeBar.Update(gameTime);


                if (_timeElapsed > 1000)
                {
                    _timer -= 1;
                    _timeElapsed = 0;
                }

                if (_timer == 0)
                {
                    //gameEnd
                    soundManager.Przegrana();
                    _gameState = GameState.Loss;
                    
                    
                }


                if (_isBonusActive)
                {
                    _bonusMultiplierTime -= gameTime.ElapsedGameTime.Milliseconds;

                    if (_bonusMultiplierTime < 0)
                    {
                        _isBonusActive = false;
                        _bonusMultiplier = 1;
                        _bonusMultiplierTime = 0;
                    }
                }



                _gameLevel.Kula.Update(gameTime);
                _gameLevel.Flag.Update(gameTime);
                _gameLevel.Key.Update(gameTime);

                if (_gameLevel.Flag.Unlocked)
                {
                    if (_gameLevel.Flag.BoundingBox.Intersects(_gameLevel.Kula.BoundingBox))
                    {
                        effects.StartEffect(effects.effect = SpecialEffects.Effect.Dada, new Vector2(400, 240), 0.5f, 0.5f);
                        //game wonn
                        _gameState = GameState.Won;
                        soundManager.Wygrana();
                    }
                }

                if (_gameLevel.Key.BoundingBox.Intersects(_gameLevel.Kula.BoundingBox) && _gameLevel.Key.Active)
                {
                    effects.StartEffect(effects.effect = SpecialEffects.Effect.Click, new Vector2(400, 350), 0.3f, 0.3f);
                    _gameLevel.Key.Active = false;
                    _gameLevel.Flag.Unlocked = true;
                    _keyFound = true;

                    soundManager.Klucz();

                    if (_vibrate)
                    {
                        //_vibrationController.Start(TimeSpan.FromMilliseconds(100));
                    }
                }

                foreach (var brick in _gameLevel.Bricks)
                {
                    brick.Update(gameTime);
                }


                foreach (var skyboard in _gameLevel.Skyboards)
                {
                    skyboard.Update(gameTime);
                }

                foreach (var crystal in _gameLevel.Crystals.GetModels().Where(p => p.Active))
                {
                    crystal.Update(gameTime);

                    if (crystal.BoundingBox.Intersects(_gameLevel.Kula.BoundingBox))
                    {
                        _bonusMultiplier *= 5;
                        crystal.Active = false;
                        _isBonusActive = true;
                        _bonusMultiplierTime += 10000;
                        crystalTimeBar.Start(10);
                        soundManager.ZebraniePrzedmiotu();

                    }
                }

                foreach (var rock in _gameLevel.Rocks)
                {
                    rock.Update(gameTime);
                    //sprawdz kolizje
                    if (rock.BoundingBox.Intersects(_gameLevel.Kula.BoundingBox))
                    {
                        effects.StartEffect(effects.effect = SpecialEffects.Effect.Puff, new Vector2(400, 240), 0.5f, 0.5f);
                        _gameState = GameState.Loss;
                        soundManager.KulaPrzebicie();
                    }
                }

                foreach (var spool in _gameLevel.Spools)
                {
                    spool.Update(gameTime);

                    if (spool.BoundingBox.Intersects(_gameLevel.Kula.BoundingBox))
                    {
                        effects.StartEffect(effects.effect = SpecialEffects.Effect.Puff, new Vector2(400, 240), 0.5f, 0.5f);
                        _gameState = GameState.Loss;
                        soundManager.KulaPrzebicie();
                    }

                }

                if (_gameLevel.Kula.IsMoving == false)
                {

                    foreach (var teleport in _gameLevel.Teleports.GetModels())
                    {
                        teleport.Update(gameTime);
                        //sprawdzamy kolizje
                        if (_teleport)
                        {
                            if (teleport.BoundingBox.Intersects(_gameLevel.Kula.BoundingBox))
                            {
                                effects.StartEffect(effects.effect = SpecialEffects.Effect.Zzzzz, new Vector2(400, 350), 0.3f, 0.4f);
                                _gameLevel.Kula.Teleport(teleport.DestinationPosition, teleport.DestinationRotation);
                                _teleport = false;
                                soundManager.Teleport();
                            }
                        }
                    }


                }
                if (_gameLevel.Kula.IsMoving)
                {
                    _teleport = true;
                }




                //blok z kolizjami i wydarzeniami które powoduj¹ przegran¹
                foreach (var obstacle in _gameLevel.Obstacles)
                {
                    obstacle.Update(gameTime);

                    if (obstacle.BoundingBox.Intersects(_gameLevel.Kula.BoundingBox))
                    {
                        effects.StartEffect(effects.effect = SpecialEffects.Effect.Puff, new Vector2(400, 240), 0.5f, 0.5f);
                        _gameState = GameState.Loss;
                        soundManager.KulaPrzebicie();
                    }
                }

                if (_gameLevel.Kula.FallFromTooHeight)
                {
                    _gameState = GameState.Loss;
                    soundManager.PozaPlansza();
                }

                if (_gameLevel.Kula.CheckIsOutsideTheLevel)
                {
                    if (_gameLevel.Kula.IsMoving == false)
                    {
                        if (_gameLevel.Kula.CheckCollision() == false)
                        {
                            _gameState = GameState.Loss;
                            soundManager.PozaPlansza();
                        }
                    }
                }



                foreach (var coin in _gameLevel.Coins.GetModels())
                {

                    if (coin.BoundingBox.Intersects(_gameLevel.Kula.BoundingBox) && ((CoinModel)coin).Active)
                    {
                        ((CoinModel)coin).Active = false;
                        _coinsCollected += 1;
                        _points += _bonusMultiplier * 10;

                        soundManager.Coin();

                        if (_vibrate)
                        {
                            //_vibrationController.Start(TimeSpan.FromMilliseconds(100));
                        }
                    }

                    coin.Update(gameTime);
                }

                foreach (var fruit in _gameLevel.Fruits.GetModels())
                {
                    if (fruit.BoundingBox.Intersects(_gameLevel.Kula.BoundingBox) && ((FruitModel)fruit).Active)
                    {
                        ((FruitModel)fruit).Active = false;
                        _fruitsCollected += 1;
                        _points += _bonusMultiplier * 50;

                        soundManager.ZebraniePrzedmiotu();

                        if (_vibrate)
                        {
                            //_vibrationController.Start(TimeSpan.FromMilliseconds(100));
                        }
                    }

                    fruit.Update(gameTime);
                }

                _camera.UpdateCamera(_gameLevel.Kula.GetPosition(), _gameLevel.Kula.RotationMatrix, _gameLevel.Kula.GetUpVector());


                //calculate the stars

                float result = ((float)_coinsCollected + _fruitsCollected) / (_level.Fruits.Count + _level.Coins.Count);

                if (result >= _oneStar)
                {
                    _howManyStars = 1;
                }
                if (result >= _twoStar)
                {
                    _howManyStars = 2;
                }
                if (result >= _threeStars)
                {
                    _howManyStars = 3;
                }
                if (result >= _fourStars)
                {
                    _howManyStars = 4;
                }
                if (result >= _fiveStars)
                {
                    _howManyStars = 5;
                }

            }
            else if (_gameState == GameState.Won)
            {
                crystalTimeBar.IsActive = false;

                _camera.ZoomOutTheCamera();
                _camera.UpdateCamera(_gameLevel.Kula.GetPosition(), _gameLevel.Kula.RotationMatrix, _gameLevel.Kula.GetUpVector());

                if (!_wasSaved)
                {
                    IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();
                    try
                    {
                        StreamReader streamReader = new StreamReader(new IsolatedStorageFileStream("KulaGame\\save.txt", FileMode.OpenOrCreate, myStore));
                        int oldCampaignId = int.Parse(streamReader.ReadLine());
                        int oldLevelId = int.Parse(streamReader.ReadLine());
                        streamReader.Close();

                        if (_campaignId >= oldCampaignId)
                        {
                            if (_levelId >= oldLevelId)
                            {
                                //write the new score
                                StreamWriter save = new StreamWriter(new IsolatedStorageFileStream("KulaGame\\save.txt", FileMode.OpenOrCreate, myStore));

                                Campaign[] campaigns = _content.Load<Campaign[]>("GameDefinition");
                                Campaign campaign = campaigns.SingleOrDefault(p => p.Id == _campaignId);
                                if (campaign != null)
                                {
                                    var level = campaign.Levels.SingleOrDefault(p => p.Id == _levelId + 1);

                                    if (level != null)
                                    {
                                        //same campaign but next level
                                        save.WriteLine(_campaignId);
                                        save.WriteLine(_levelId + 1);
                                    }
                                    else
                                    {
                                        //try to get new campaign, if exist then go to the campaign select screen with campaign selected
                                        var newCampaign = campaigns.SingleOrDefault(p => p.Id == _campaignId + 1);
                                        if (newCampaign != null)
                                        {
                                            if (newCampaign.Id > _campaignId)
                                            {
                                                save.WriteLine(newCampaign.Id);
                                                save.WriteLine(1);
                                            }
                                        }
                                    }
                                }

                                save.Close();
                            }
                        }

                    }
                    catch (Exception)
                    {
                    }
                    //check the higscore/stars - if beaten write else not
                    try
                    {
                        if (myStore.FileExists(string.Format("KulaGame\\{0}_{1}.txt", _campaignId, _levelId)))
                        {
                            StreamReader streamReader = new StreamReader(new IsolatedStorageFileStream(string.Format("KulaGame\\{0}_{1}.txt", _campaignId, _levelId),
                                      FileMode.OpenOrCreate, myStore));

                            string resultString = streamReader.ReadLine();
                            int oldStars = int.Parse(resultString.Split('$')[0]);
                            int oldPoints = int.Parse(resultString.Split('$')[1]);
                            streamReader.Close();

                            if (oldStars > _howManyStars)
                            {
                                _howManyStars = oldStars;
                            }


                            if (_points + (_timer * 5) > oldPoints)
                            {
                                //save the new result
                                WriteResult();

                            }
                        }
                        else
                        {
                            //file don't exist - write new
                            WriteResult();
                        }
                    }
                    catch { }
                    SendHighscores();
                    _wasSaved = true;
                }


            }
            else if (_gameState == GameState.Loss)
            {
                crystalTimeBar.IsActive = false;

                _camera.ZoomOutTheCamera();
                _camera.UpdateCamera(_gameLevel.Kula.GetPosition(), _gameLevel.Kula.RotationMatrix, _gameLevel.Kula.GetUpVector());
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        private void WriteResult()
        {
            try
            {
                IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();
                StreamWriter streamWriter = new StreamWriter(
                    new IsolatedStorageFileStream(string.Format("KulaGame\\{0}_{1}.txt", _campaignId, _levelId),
                                                  FileMode.OpenOrCreate, myStore));
                string newResult = string.Format("{0}${1}", _howManyStars, _points+(_timer * 5));
                streamWriter.WriteLine(newResult);
                streamWriter.Close();
            }
            catch (Exception)
            {
            }
        }

        private void SendHighscores()
        {
            var onlineSettings = Common.ReadOnlineCredentials();
            if (onlineSettings.IsOnline)
            {
                try
                {
                    //Send highscore
                    HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(string.Format("http://kulagame.pl.hostingasp.pl/Api/SendScore?username={0}&password={1}&campaignId={2}&levelId={3}&score={4}", onlineSettings.UserName, onlineSettings.Password, _campaignId, _levelId, _points+(5*_timer)));
                    httpWebRequest.Method = "POST";
                    httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                    httpWebRequest.BeginGetResponse(Response_Completed, httpWebRequest);

                    HttpWebRequest httpWebRequestCoins = (HttpWebRequest)HttpWebRequest.Create(string.Format("http://kulagame.pl.hostingasp.pl/Api/SendCoin?username={0}&password={1}&campaignId={2}&levelId={3}&count={4}", onlineSettings.UserName, onlineSettings.Password, _campaignId, _levelId, _coinsCollected));
                    httpWebRequestCoins.Method = "POST";
                    httpWebRequestCoins.ContentType = "application/x-www-form-urlencoded";

                    httpWebRequestCoins.BeginGetResponse(Response_Completed, httpWebRequest);

                    HttpWebRequest httpWebRequestBonuses = (HttpWebRequest)HttpWebRequest.Create(string.Format("http://kulagame.pl.hostingasp.pl/Api/SendBonus?username={0}&password={1}&campaignId={2}&levelId={3}&count={4}", onlineSettings.UserName, onlineSettings.Password, _campaignId, _levelId, _fruitsCollected));
                    httpWebRequestBonuses.Method = "POST";
                    httpWebRequestBonuses.ContentType = "application/x-www-form-urlencoded";

                    httpWebRequestBonuses.BeginGetResponse(Response_Completed, httpWebRequest);
                }
                catch
                {
                }
            }

        }

        void Response_Completed(IAsyncResult result)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)result.AsyncState;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);


                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    string responseText = streamReader.ReadToEnd();
                }
            }
            catch (Exception)
            {
            }
        }


        public override void HandleInput(InputState input)
        {
            if (_gameState == GameState.BeforeStart)
            {
                foreach (var gesture in input.Gestures)
                {
                    if (gesture.GestureType == GestureType.Tap)
                    {
                        _gameState = GameState.Active;
                        _camera.ResetZoom();
                    }
                }
            }
            else if (_gameState == GameState.Active)
            {
                bool handled = false;

                if (input.TouchState.Count > 0)
                {
                    if (input.TouchState[0].Position.X >= pad.JumpRect.X && input.TouchState[0].Position.X <= pad.JumpRect.Width &&
                        input.TouchState[0].Position.Y >= pad.JumpRect.Y &&
                        input.TouchState[0].Position.Y <= pad.JumpRect.Height)
                    {
                        pad.JumpColor = Color.Green;
                        handled = true;
                    }
                    else if (input.TouchState[0].Position.X >= pad.DpadRect.X && input.TouchState[0].Position.X <= pad.DpadRect.Width &&
                             input.TouchState[0].Position.Y >= pad.DpadRect.Y &&
                             input.TouchState[0].Position.Y <= pad.DpadRect.Height)
                    {
                        pad.DpadColor = Color.Gray;
                        handled = true;
                    }
                    else if (input.TouchState[0].Position.X >= 280 && input.TouchState[0].Position.X <= 520 &&
                             input.TouchState[0].Position.Y >= 120 && input.TouchState[0].Position.Y <= 360)
                    {
                        foreach (var gesture in input.Gestures)
                        {
                            if (gesture.GestureType == GestureType.DoubleTap)
                            {
                                accelerometer.stateChanged = true;
                                accelerometer.IsTurnOn = !accelerometer.IsTurnOn;
                                pad.IsVisible = !pad.IsVisible;
                                handled = true;
                            }
                        }
                    }
                }
                if (handled == false)
                {
                    foreach (var gesture in input.Gestures)
                    {
                        if (gesture.GestureType == GestureType.Tap)
                        {
                            //X=600,Y350
                            if (gesture.Position.X >= pad.JumpRect.X && gesture.Position.X <= pad.JumpRect.Width && gesture.Position.Y >= pad.JumpRect.Y &&
                                gesture.Position.Y <= pad.JumpRect.Height)
                            {
                                pad.JumpColor = Color.Green;
                            }

                            if (gesture.Position.X >= pad.DpadRect.X && gesture.Position.X <= pad.DpadRect.Width && gesture.Position.Y >= pad.DpadRect.Y &&
                                gesture.Position.Y <= pad.DpadRect.Height)
                            {
                                pad.DpadColor = Color.Gray;
                            }
                        }

                        if (gesture.GestureType == GestureType.Pinch)
                        {
                            if (gesture.Delta.X > 5)
                            {
                                 _camera.ZoomOutTheCamera_Pinch();
                            }
                            else if (gesture.Delta.X < -5)
                            {
                                _camera.ZoomInTheCamera_Pinch();
                            }
                        }
                    }
                }
                _gameLevel.Kula.HandleInput(input);
            }
            else if (_gameState == GameState.Won)
            {
                foreach (var gesture in input.Gestures)
                {
                    if (gesture.GestureType == GestureType.Tap)
                    {
                        if (gesture.Position.X > 170 && gesture.Position.X < 350 && gesture.Position.Y > 392 &&
                            gesture.Position.Y < 468)
                        {
                            Campaign[] campaigns = _content.Load<Campaign[]>("GameDefinition");
                            Campaign campaign = campaigns.SingleOrDefault(p => p.Id == _campaignId);
                            if (campaign != null)
                            {
                                var level = campaign.Levels.SingleOrDefault(p => p.Id == _levelId + 1);

                                if (level != null)
                                {
                                    if (Common.ReadMusicSettings() == MusicSettings.StopZune || Microsoft.Xna.Framework.Media.MediaPlayer.GameHasControl)
                                    {
                                        MediaPlayer.Stop();
                                    }
                                    LoadingScreen.Load(
                                        ScreenManager,
                                        true,
                                        0,
                                        new BackgroundScreen(), new MainMenuScreen(), new CampaignSelectScreen(_campaignId),
                                        new LevelSelectScreen(_campaignId, _levelId + 1));
                                }
                                else
                                {
                                    //try to get new campaign, if exist then go to the campaign select screen with campaign selected
                                    var newCampaign = campaigns.SingleOrDefault(p => p.Id == _campaignId + 1);
                                    if (newCampaign != null)
                                    {
                                        if (Common.ReadMusicSettings() == MusicSettings.StopZune || Microsoft.Xna.Framework.Media.MediaPlayer.GameHasControl)
                                        {
                                            MediaPlayer.Stop();
                                        }

                                        LoadingScreen.Load(
                                            ScreenManager,
                                            true,
                                            0,
                                            new BackgroundScreen(), new MainMenuScreen(),
                                            new CampaignSelectScreen(_campaignId + 1));
                                    }
                                    else
                                    {
                                        if (Common.ReadMusicSettings() == MusicSettings.StopZune || Microsoft.Xna.Framework.Media.MediaPlayer.GameHasControl)
                                        {
                                            MediaPlayer.Stop();
                                        }
                                        //if no new level available show apropriate screen
                                        LoadingScreen.Load(
                                            ScreenManager,
                                            true,
                                            0,
                                            new BackgroundScreen(), new MainMenuScreen(), new EndGameScreen());
                                    }

                                }
                            }

                        }
                        else if (gesture.Position.X > 460 && gesture.Position.X < 640 && gesture.Position.Y > 392 &&
                                 gesture.Position.Y < 468)
                        {
                            ResetGame();
                            _camera = new BehindCamera(ScreenManager.GraphicsDevice.Viewport.AspectRatio, _gameLevel.Kula.GetPosition(), _gameLevel.Kula.InitialRotation);
                            _gameState = GameState.BeforeStart;
                        }
                    }
                }
            }
            else if (_gameState == GameState.Loss)
            {
                foreach (var gesture in input.Gestures)
                {

                    if (gesture.GestureType == GestureType.Tap)
                    {

                        if (gesture.Position.X > 170 && gesture.Position.X < 350 && gesture.Position.Y > 392 &&
                            gesture.Position.Y < 468)
                        {
                            ResetGame();
                            _camera = new BehindCamera(ScreenManager.GraphicsDevice.Viewport.AspectRatio, _gameLevel.Kula.GetPosition(), _gameLevel.Kula.InitialRotation);
                            _gameState = GameState.BeforeStart;

                        }
                        else if (gesture.Position.X > 460 && gesture.Position.X < 640 && gesture.Position.Y > 392 &&
                                 gesture.Position.Y < 468)
                        {
                            if (Common.ReadMusicSettings() == MusicSettings.StopZune || Microsoft.Xna.Framework.Media.MediaPlayer.GameHasControl)
                            {
                                MediaPlayer.Stop();
                            }
                            ScreenManager.RemoveScreen(this);
                        }
                    }
                }
            }
            PlayerIndex player;
            if (input.IsNewButtonPress(Buttons.Back, ControllingPlayer, out player))
            {
                OnCancel(player);
            }
            base.HandleInput(input);
        }

        /// <summary>
        /// Handler for when the user has cancelled the menu.
        /// </summary>
        protected virtual void OnCancel(PlayerIndex playerIndex)
        {
            //Stop the media player if playing
            if (Common.ReadMusicSettings() == MusicSettings.StopZune || Microsoft.Xna.Framework.Media.MediaPlayer.GameHasControl)
            {
                MediaPlayer.Stop();
            }

            LoadingScreen.Load(
    ScreenManager,
    true,
   playerIndex,
    new BackgroundScreen(), new MainMenuScreen(), new CampaignSelectScreen(_campaign.Id), new LevelSelectScreen(_campaign.Id, _levelId));
        }



        public void ResetGame()
        {
            //in the future load the game level definition from the xml file
            _gameLevel = new GameLevel(_content, _campaign, _level);
            _gameLevel.Kula.pad = pad;
            _gameLevel.Kula.accelerometer = accelerometer;
            _gameLevel.Kula.soundManager = soundManager;
            _gameLevel.Kula.specialEffect = effects;
            clock.soundManager = soundManager;
            clock.isLossSoundOn = false;
            _drawText = string.Empty;
            _coinsCollected = 0;
            _fruitsCollected = 0;
            _keyFound = false;
            clock.StartingTime = (float)_level.TimeLimit;
            _timer = _level.TimeLimit;
            _timeElapsed = 0;
            _gameState = GameState.BeforeStart;
            _points = 0;
            _wasSaved = false;
            _howManyStars = 0;
            _bonusMultiplierTime = 0;
            _bonusMultiplier = 1;
            _isBonusActive = false;
        }



        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.Black);
            ScreenManager.GraphicsDevice.BlendState = BlendState.Opaque;
            ScreenManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle screenSize = new Rectangle(0, 0, viewport.Width, viewport.Height);

            Vector2 menuOrigin = new Vector2(menuTexture.Width / 2, menuTexture.Height / 2);

            foreach (var obstacle in _gameLevel.Obstacles)
            {
                obstacle.Draw(gameTime, _camera);
            }

            foreach (var point in _gameLevel.Coins)
            {
                point.Draw(gameTime, _camera);
            }

            foreach (var skyboard in _gameLevel.Skyboards)
            {
                skyboard.Draw(gameTime, _camera);
            }

            foreach (var crystal in _gameLevel.Crystals.GetModels().Where(p => p.Active))
            {
                crystal.Draw(gameTime, _camera);
            }

            foreach (var teleport in _gameLevel.Teleports)
            {
                teleport.Draw(gameTime, _camera);
            }
            foreach (var spool in _gameLevel.Spools)
            {
                spool.Draw(gameTime, _camera);
            }
            foreach (var rock in _gameLevel.Rocks)
            {
                rock.Draw(gameTime, _camera);
            }


            _gameLevel.Sky.Draw(gameTime, _camera);


            _gameLevel.Kula.Draw(gameTime, _camera);
            _gameLevel.Key.Draw(gameTime, _camera);


            _gameLevel.Flag.Draw(gameTime, _camera);

            foreach (var brick in _gameLevel.Bricks)
            {
                    brick.Draw(gameTime, _camera);

            } 
            
            if (_gameLevel.IsBonusTransparent)
            {
                ScreenManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;

                foreach (var fruit in _gameLevel.Fruits)
                {
                    fruit.DrawTransparent(gameTime, _camera);
                }
                ScreenManager.GraphicsDevice.BlendState = BlendState.Opaque;

            }
            else
            {
                foreach (var fruit in _gameLevel.Fruits)
                {
                    fruit.Draw(gameTime, _camera);
                }
            }

            effects.Draw(_spriteBatch);

            if (_gameState == GameState.BeforeStart)
            {
                double time = gameTime.TotalGameTime.TotalSeconds;
                float pulsate = (float)Math.Sin(time * 5) + 1;
                float scale = 1 + pulsate * 0.09f;
                string txt = "Tap to start";
                _spriteBatch.Begin();
                _spriteBatch.DrawString(_statusFont, txt, new Vector2(screenSize.Width / 2, 200), Color.White, 0, new Vector2(_statusFont.MeasureString(txt).X / 2, 0), scale, SpriteEffects.None, 0);
                _spriteBatch.End();
            }

            else if (_gameState == GameState.Active)
            {
                _spriteBatch.Begin();

                if (!takingPictures)
                {
                    _spriteBatch.DrawString(_font, string.Format("Points: {0}", _points), new Vector2(10, 4),
                                            Color.Black, 0, new Vector2(0, 0), 1.1f, SpriteEffects.None, 0);
                    _spriteBatch.Draw(coinTexture, new Vector2(10, 45), null, Color.White, 0, Vector2.Zero, 0.9f, SpriteEffects.None, 0);
                    _spriteBatch.DrawString(_font, string.Format("{0}/{1}", _coinsCollected, _gameLevel.Coins.Count()),
                                            new Vector2(50, 45), Color.Black, 0, Vector2.Zero, 0.9f, SpriteEffects.None, 0);
                    _spriteBatch.DrawString(_font,string.Format("Bonus: {0}/{1}", _fruitsCollected, _gameLevel.Fruits.Count()),
                                            new Vector2(10, 75), Color.Black, 0,Vector2.Zero, 0.9f, SpriteEffects.None, 0);
                    _spriteBatch.DrawString(_font, "Key:", new Vector2(10, 105), Color.Black, 0, Vector2.Zero, 0.9f, SpriteEffects.None, 0);
                    if (_keyFound)
                    {
                        double etime = gameTime.TotalGameTime.TotalSeconds;
                        float pulsate = (float)Math.Sin(etime * 4) + 1;
                        float scaling = 0.6f + pulsate * 0.05f;
                        _spriteBatch.Draw(keyTexture, new Vector2(72, 125), null, Color.White, 0, new Vector2(keyTexture.Width / 2, keyTexture.Height / 2), scaling, SpriteEffects.None, 0);
                    
                    }
                    
                    //Clock
                    clock.Draw(_spriteBatch, new Vector2(750, 50), 0.25f);
                    // TimeBar
                    crystalTimeBar.Draw(_spriteBatch);
                    // TIMER  
                    //_spriteBatch.DrawString(_font, string.Format("Time left: {0}s", _timer), new Vector2(600, 100), Color.White);
                    // Dpad and Jump
                    pad.Draw(_spriteBatch);

                    

                    if (_bonusMultiplier > 1)
                    {
                        _spriteBatch.DrawString(_font, string.Format("x {0}", _bonusMultiplier), new Vector2(220, 10),
                                                Color.YellowGreen, 0, new Vector2(0, 0), 1.2f, SpriteEffects.None, 0);

                    }

                    //draw stars

                    for (int i = 0; i < 5; i++)
                    {

                        Color starColor = Color.YellowGreen;

                        if (i > _howManyStars - 1 || _howManyStars == 0)
                        {
                            starColor = Color.Transparent;
                        }


                        _spriteBatch.Draw(_star, new Vector2(310 + (i * 35), 50), null, starColor, 0, new Vector2(0, 0),
                                          0.15f, SpriteEffects.None, 0);

                    }
                }

                _spriteBatch.End();

            }
            else if (_gameState == GameState.Won)
            {
                string points = "Points: " + _points + " + " + _timer * 5;

                _spriteBatch.Begin();
                _spriteBatch.Draw(menuTexture, new Vector2(screenSize.Width / 2, 105), null, Color.White, 0, menuOrigin, new Vector2(0.8f, 0.30f), SpriteEffects.None, 0);
                _spriteBatch.DrawString(_statusFont, points, new Vector2(screenSize.Width / 2, 110), Color.Black, 0, new Vector2(_statusFont.MeasureString(points).X / 2, 0), 0.7f, SpriteEffects.None, 0);


                for (int i = 0; i < 5; i++)
                {

                    Color starColor = Color.YellowGreen;

                    if (i > _howManyStars - 1 || _howManyStars == 0)
                    {
                        starColor = new Color(0, 0, 0, 25);
                    }

                    _spriteBatch.Draw(_star, new Vector2(310 + (i * 35), 70), null, starColor, 0, new Vector2(0, 0), 0.15f, SpriteEffects.None, 0);

                }
                //nowe 
                double time = gameTime.TotalGameTime.TotalSeconds;
                float pulsate = (float)Math.Sin(time * 5) + 1;
                float scale = 1.2f + pulsate * 0.09f;

                _spriteBatch.Draw(menuTexture, new Vector2(screenSize.Width / 2, 400), null, Color.White, 0, menuOrigin, new Vector2(1.4f, 0.50f), SpriteEffects.None, 0);
                string txt = "Congratulation!.";
                _spriteBatch.DrawString(_statusFont, txt, new Vector2(screenSize.Width / 2, 335), Color.Black, 0, new Vector2(_statusFont.MeasureString(txt).X / 2, 0), 1.0f, SpriteEffects.None, 0);
                _spriteBatch.Draw(nextTexture, new Vector2(260, 430), null, Color.White, 0,
                                  new Vector2(nextTexture.Width / 2, nextTexture.Height / 2), scale, SpriteEffects.None, 0);
                _spriteBatch.Draw(againTexture, new Vector2(540, 430), null, Color.White, 0,
                                 new Vector2(againTexture.Width / 2, againTexture.Height / 2), scale, SpriteEffects.None, 0);
                _spriteBatch.End();

            }
            else if (_gameState == GameState.Loss)
            {


                double time = gameTime.TotalGameTime.TotalSeconds;
                float pulsate = (float)Math.Sin(time * 5) + 1;
                float scale = 1.2f + pulsate * 0.09f;

                _spriteBatch.Begin();
                _spriteBatch.Draw(menuTexture, new Vector2(screenSize.Width / 2, 400), null, Color.White, 0, menuOrigin, new Vector2(1.4f, 0.50f), SpriteEffects.None, 0);
                string txt = "Sorry, try again.";
                _spriteBatch.DrawString(_statusFont, txt, new Vector2(screenSize.Width / 2, 335), Color.Black, 0, new Vector2(_statusFont.MeasureString(txt).X / 2, 0), 1.0f, SpriteEffects.None, 0);
                _spriteBatch.Draw(again2Texture, new Vector2(260, 430), null, Color.White, 0,
                                  new Vector2(again2Texture.Width / 2, again2Texture.Height / 2), scale, SpriteEffects.None, 0);
                _spriteBatch.Draw(backTexture, new Vector2(540, 430), null, Color.White, 0,
                                 new Vector2(backTexture.Width / 2, backTexture.Height / 2), scale, SpriteEffects.None, 0);

                _spriteBatch.End();
            }




            base.Draw(gameTime);
        }


        public void DrawBox(BaseModel model)
        {
            VertexPositionColor[] points = new VertexPositionColor[8];

            Vector3[] corners = model.BoundingBox.GetCorners();

            Color lineColor = Color.Black;

            points[0] = new VertexPositionColor(corners[1], lineColor); // Front Top Right
            points[1] = new VertexPositionColor(corners[0], lineColor); // Front Top Left
            points[2] = new VertexPositionColor(corners[2], lineColor); // Front Bottom Right
            points[3] = new VertexPositionColor(corners[3], lineColor); // Front Bottom Left
            points[4] = new VertexPositionColor(corners[5], lineColor); // Back Top Right
            points[5] = new VertexPositionColor(corners[4], lineColor); // Back Top Left
            points[6] = new VertexPositionColor(corners[6], lineColor); // Back Bottom Right
            points[7] = new VertexPositionColor(corners[7], lineColor); // Bakc Bottom Left

            short[] index = new short[] {
	            0, 1, 0, 2, 1, 3, 2, 3,
	            4, 5, 4, 6, 5, 7, 6, 7,
	            0, 4, 1, 5, 2, 6, 3, 7
                };


            BasicEffect basicEffect = new BasicEffect(ScreenManager.GraphicsDevice);

            basicEffect.World = Matrix.Identity;
            basicEffect.View = _camera.View;
            basicEffect.Projection = _camera.Projection;
            basicEffect.VertexColorEnabled = true;

            basicEffect.CurrentTechnique.Passes[0].Apply();
            ScreenManager.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineList, points, 0, points.Length, index, 0, 12);
        }


    }
}
