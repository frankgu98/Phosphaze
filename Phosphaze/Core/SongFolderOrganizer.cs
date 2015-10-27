using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Phosphaze.Core
{
    public class SongFolderOrganizer
    {
        //makes a folder for each .wav file in the song folder without one
        public static void Organize()
        {
            

            //for testing purposes!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            foreach (string filename in Directory.GetFiles(Options.SongFolder, "*.dml", SearchOption.AllDirectories))
            {
                //File.Delete(filename);
            }

            string newDirectory;
            string noExt;
            //searches through the song folder for any .wav files that aren't in a folder, then puts them into their own folder
            foreach (string songname in Directory.GetFiles(Options.SongFolder, "*.wav", SearchOption.TopDirectoryOnly))
            {
                noExt = Path.GetFileNameWithoutExtension(songname);
                newDirectory = Options.SongFolder + "\\" + noExt;
                if (!Directory.Exists(newDirectory))
                {
                    Directory.CreateDirectory(newDirectory);
                    File.Move(songname, newDirectory + "\\" + noExt + ".wav");
                }
            }
        

            StreamWriter writer;
            MapMaker mapMaker;
            string[] wavfiles;
            //searches though all the folders in the song folder to see if there are any .wav files without a .dml file
            //if there are, it writes a .dml file for the .wav and puts it into the same folder
            foreach (string folder in Directory.GetDirectories(Options.SongFolder, "*", SearchOption.TopDirectoryOnly))
            {
                wavfiles = Directory.GetFiles(folder, "*.wav", SearchOption.TopDirectoryOnly);
                if (wavfiles.Length == 1 &&
                    Directory.GetFiles(folder, "*.dml", SearchOption.TopDirectoryOnly).Length == 0)
                {
                    mapMaker = new MapMaker(new Wave(wavfiles[0]));
                    writer = new StreamWriter(wavfiles[0].Substring(0, wavfiles[0].Length - 4) + ".dml");
                    writer.Write(mapMaker.MakeMap());
                    writer.Close();
                }
            }
        }

    }
}
