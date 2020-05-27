using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace KulaGame.Engine.Utils
{
    public static class Common
    {

        public static Vector3 RoundVector(Vector3 input)
        {
            Vector3 output = new Vector3();
            output.X = (int)Math.Round(input.X);
            output.Y = (int)Math.Round(input.Y);
            output.Z = (int)Math.Round(input.Z);

            return output;
        }

        public static Vector3 RoundVector(Vector3 input, int stepCount)
        {
            Vector3 output = new Vector3();
            output.X = (int)Math.Round(input.X);
            output.Y = (int)Math.Round(input.Y);
            output.Z = (int)Math.Round(input.Z);

            if (output.X == stepCount || output.X == -stepCount)
            {
                output.Z = 0;
                output.Y = 0;

            }
            else if (output.Y == stepCount || output.Y == -stepCount)
            {
                output.X = 0;
                output.Z = 0;

            }
            else if (output.Z == stepCount || output.Z == -stepCount)
            {
                output.X = 0;
                output.Y = 0;

            }

            return output;
        }

        public static CameraRotationAxis CameraRotationAxis(Vector3 up)
        {
            up = Common.RoundVector(up);
            CameraRotationAxis axis = Utils.CameraRotationAxis.Y;

            if (up.Y == 1 || up.Y == -1)
            {
                axis = Utils.CameraRotationAxis.Y;
            }
            else if (up.X == 1 || up.X == -1)
            {
                axis = Utils.CameraRotationAxis.X;
            }
            else if (up.Z == 1 || up.Z == -1)
            {
                axis = Utils.CameraRotationAxis.Z;
            }

            return axis;
        }


        public static CameraRotationAxisExact CameraRotationAxisExact(Vector3 up)
        {
            up = Common.RoundVector(up);
            CameraRotationAxisExact axis = Utils.CameraRotationAxisExact.Y_plus;

            if (up.Y == 1)
            {
                axis = Utils.CameraRotationAxisExact.Y_plus;
            }
            else if (up.Y == -1)
            {
                axis = Utils.CameraRotationAxisExact.Y_minus;
            }
            else if (up.X == 1)
            {
                axis = Utils.CameraRotationAxisExact.X_plus;
            }
            else if (up.X == -1)
            {
                axis = Utils.CameraRotationAxisExact.X_minus;
            }
            else if (up.Z == 1)
            {
                axis = Utils.CameraRotationAxisExact.Z_plus;
            }
            else if (up.Z == -1)
            {
                axis = Utils.CameraRotationAxisExact.Z_minus;
            }

            return axis;
        }


        public static void WriteMusicSettings(MusicSettings musicSetting)
        {
            IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();
            StreamWriter save = new StreamWriter(new IsolatedStorageFileStream("KulaGame\\musicSetting.txt", FileMode.OpenOrCreate, myStore));
            save.WriteLine((int)musicSetting);
            save.Close();
        }


        public static MusicSettings ReadMusicSettings()
        {
            IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();
            StreamReader streamReader = new StreamReader(new IsolatedStorageFileStream("KulaGame\\musicSetting.txt", FileMode.OpenOrCreate, myStore));
            string valueString = streamReader.ReadLine();
            streamReader.Close();
            int value = 0;
            if (valueString != null)
            {
                int.TryParse(valueString, out value);
            }
            return (MusicSettings) value;
        }


        public static void WriteBackgroundMusicSettings(SettingsEnum state)
        {
            IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();
            StreamWriter save = new StreamWriter(new IsolatedStorageFileStream("KulaGame\\backgroundMusicSetting.txt", FileMode.OpenOrCreate, myStore));
            save.WriteLine((int)state);
            save.Close();
        }


        public static SettingsEnum ReadBackgroundMusicSettings()
        {
            IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();
            StreamReader streamReader = new StreamReader(new IsolatedStorageFileStream("KulaGame\\backgroundMusicSetting.txt", FileMode.OpenOrCreate, myStore));
            string valueString = streamReader.ReadLine();
            streamReader.Close();
            int value = 0;
            if (valueString != null)
            {
                int.TryParse(valueString, out value);
            }
            return (SettingsEnum)value;
        }


        public static void WriteCredentials(string username, string password,bool? isOnline)
        {
            IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();
            StreamWriter writer = new StreamWriter(new IsolatedStorageFileStream("KulaGame\\onlinesettings.txt", FileMode.OpenOrCreate, myStore));
            writer.WriteLine(username);
            writer.WriteLine(password);
            writer.WriteLine(isOnline);
            writer.Close();
        }


        public static OnlineCredentials ReadOnlineCredentials()
        {
            IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();
            StreamReader reader = new StreamReader(new IsolatedStorageFileStream("KulaGame\\onlinesettings.txt", FileMode.OpenOrCreate, myStore));
            OnlineCredentials onlineCredentials = new OnlineCredentials();
            try
            {
                onlineCredentials.UserName=reader.ReadLine();
                onlineCredentials.Password = reader.ReadLine();
                onlineCredentials.IsOnline = bool.Parse(reader.ReadLine());
            }
            catch (Exception)
            {
                onlineCredentials.UserName = "notset";
                onlineCredentials.Password = "notset";
                onlineCredentials.IsOnline = false;
            }
            reader.Close();

            return onlineCredentials;
        }



        public static bool CheckUpVector(Vector3 first, Vector3 second)
        {
            first = RoundVector(first);
            second = RoundVector(second);

            if (first == second)
            {
                return true;
            }


            return false;
        }

        public static void RemoveOnlineCredentials()
        {
            IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();
            myStore.DeleteFile("KulaGame\\onlinesettings.txt");
        }

        public static bool OnlineCredentialsExist()
        {
            IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();
            return myStore.FileExists("KulaGame\\onlinesettings.txt"); 
        }

        public static bool IntroPlayed()
        {
            bool returnValue = false;
            IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();
            StreamReader reader = new StreamReader(new IsolatedStorageFileStream("KulaGame\\intro.txt", FileMode.OpenOrCreate, myStore));
            try
            {
                string value = reader.ReadLine();
                int valueParsed = int.Parse(value);
                if (valueParsed == 1)
                {
                    returnValue= true;
                }
                else
                {
                    returnValue= false;
                }
            }
            catch (Exception)
            {
                returnValue= false;
            }
            reader.Close();
            return returnValue;
        }
        public static void SaveIntroPlayed()
        {
            IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();
            StreamWriter writer = new StreamWriter(new IsolatedStorageFileStream("KulaGame\\intro.txt", FileMode.OpenOrCreate, myStore));
            writer.Write(1);
            writer.Close();
        }
        
    }
}
