using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace KulaGame.Engine.Utils
{
    public class SoundManager
    {
        #region Declarations

        private bool isSoundOn;

        // MUSIC
        private int CampainID;
        private Song song;

        // SOUNDS
        private SoundEffect coin;
        private SoundEffect fanfary;
        private SoundEffect intro;
        private SoundEffect kulaOdbicie;
        private SoundEffect kulaPrzebicie;
        private SoundEffect kulaUpadek;
        private SoundEffect odbicieKostka;
        private SoundEffect przegrana;
        private SoundEffect teleport;
        private SoundEffect trampolina;
        private SoundEffect wygrana;
        private SoundEffect klucz;
        private SoundEffect zebraniePrzedmiotu;
        private SoundEffect pozaPlansza;
        private SoundEffect tykniecieZegara;

        #endregion

        #region Initialization

        public SoundManager(ContentManager content, int CampaignID)
        {
            this.CampainID = CampaignID;
            ReadMusicStatus(content);
            isSoundOn = ReadSoundStatus();

            try
            {
                // MUSIC
                // jak trzeba :P

                // SOUNDS
                coin = content.Load<SoundEffect>(@"Sounds\Coin");
                fanfary = content.Load<SoundEffect>(@"Sounds\fanfary");
                intro = content.Load<SoundEffect>(@"Sounds\intro");
                kulaOdbicie = content.Load<SoundEffect>(@"Sounds\kula-odbicie");
                kulaPrzebicie = content.Load<SoundEffect>(@"Sounds\kula-przebita");
                kulaUpadek = content.Load<SoundEffect>(@"Sounds\kula-upadek");
                odbicieKostka = content.Load<SoundEffect>(@"Sounds\odbicie_kostka");
                przegrana = content.Load<SoundEffect>(@"Sounds\przegrana");
                teleport = content.Load<SoundEffect>(@"Sounds\teleport");
                trampolina = content.Load<SoundEffect>(@"Sounds\trampolina");
                wygrana = content.Load<SoundEffect>(@"Sounds\wygrana");
                klucz = content.Load<SoundEffect>(@"Sounds\klucz");
                zebraniePrzedmiotu = content.Load<SoundEffect>(@"Sounds\zebranie-przedmiotu");
                pozaPlansza = content.Load<SoundEffect>(@"Sounds\poza-plansza");
                tykniecieZegara = content.Load<SoundEffect>(@"Sounds\tick");
            }
            catch
            {
                Debug.WriteLine("SoundManager Initialization Failed");
            }
        }

        #endregion

        #region Checking Music and Sounds  Methods

        private void ReadMusicStatus(ContentManager content)
        {
            if ((Common.ReadMusicSettings() == MusicSettings.StopZune || Microsoft.Xna.Framework.Media.MediaPlayer.GameHasControl))
            {
                try
                {
                    MediaPlayer.Stop();

                    if (Common.ReadBackgroundMusicSettings() == SettingsEnum.On)
                    {
                        song = content.Load<Song>(string.Format("Music/{0}", CampainID));
                        MediaPlayer.IsRepeating = true;
                        MediaPlayer.Play(song);
                    }
                }
                catch
                {
                    Debug.WriteLine("Music Failed");
                }
            }
        }

        private bool ReadSoundStatus()
        {
            IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();
            StreamReader reader = new StreamReader(new IsolatedStorageFileStream("KulaGame\\sound.txt", FileMode.OpenOrCreate, myStore));
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

        #endregion

        #region Sounds

        public void Coin()
        {
            if (isSoundOn)
            {
                coin.Play();
            }
        }

        public void Fanfary()
        {
            if (isSoundOn)
            {
                fanfary.Play();
            }

        }

        public void Intro()
        {
            if (isSoundOn)
            {
                intro.Play();
            }
        }

        public void KulaOdbicie()
        {
            if (isSoundOn)
            {
                kulaOdbicie.Play();
            }
        }

        public void KulaPrzebicie()
        {
            if (isSoundOn)
            {
                kulaPrzebicie.Play();
            }
        }

        public void KulaUpadek()
        {
            if (isSoundOn)
            {
                kulaUpadek.Play();
            }
        }

        public void OdbicieKostka()
        {
            if (isSoundOn)
            {
                odbicieKostka.Play();
            }
        }

        public  void Przegrana()
        {
            if (isSoundOn)
            {
                przegrana.Play();
            }
        }

        public void Teleport()
        {
            if (isSoundOn)
            {
                teleport.Play();
            }
        }

        public void Trampolina()
        {
            if (isSoundOn)
            {
                trampolina.Play();
            }
        }

        public void Wygrana()
        {
            if (isSoundOn)
            {
                wygrana.Play();
            }
        }

        public void Klucz()
        {
            if (isSoundOn)
            {
                klucz.Play();
            }
        }

        public void ZebraniePrzedmiotu()
        {
            if (isSoundOn)
            {
                zebraniePrzedmiotu.Play();
            }
        }

        public void PozaPlansza()
        {
            if (isSoundOn)
            {
                pozaPlansza.Play();
            }
        }

        public void ZykniecieZegara()
        {
            if (isSoundOn)
            {
                tykniecieZegara.Play();
            }
        }

        #endregion
    }
}
