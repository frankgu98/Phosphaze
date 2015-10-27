using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Phosphaze.Core
{
    public class UserInfoLoader
    {

        public const string FILENAME = "user.info";

        public bool hasInfo { get; private set; }

        public UserInfoLoader()
        {
            hasInfo = File.Exists(FILENAME);
        }

        public void Write()
        {
            using (var writer = new StreamWriter(FILENAME))
                writer.Write(Options.SongFolder);
            
        }

        public void Load()
        {
            if (!hasInfo)
                return;

            using (var reader = new StreamReader(FILENAME))
                Options.SongFolder = reader.ReadLine();
        }

    }
}
