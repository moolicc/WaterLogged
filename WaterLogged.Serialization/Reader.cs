using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace WaterLogged.Serialization
{
    public abstract class Reader
    {
        public static KeyValuePair<string, Type>[] FileLookup {
            get { return _fileLookup.ToArray(); }
        }

        private static Dictionary<string, Type> _fileLookup;

        static Reader()
        {
            _fileLookup = new Dictionary<string, Type>();
        }

        public static void RegisterExtension<T>(string extension, T readerType) where T : Reader
        {
            _fileLookup.Add(extension, readerType.GetType());
        }

        public static void RegisterExtension(string extension, Type readerType)
        {
            _fileLookup.Add(extension, readerType);
        }

        public static void DropExtension(string extension)
        {
            _fileLookup.Remove(extension);
        }

        public static Type GetRegisteredType(string extension)
        {
            return _fileLookup[extension];
        }

        public static bool IsRegistered(string extension)
        {
            return _fileLookup.ContainsKey(extension);
        }

        public static Reader CreateReader(string extension)
        {
            return (Reader)Activator.CreateInstance(_fileLookup[extension]);
        }

        public static T CreateReader<T>(string extension) where T : Reader
        {
            return (T)Activator.CreateInstance(_fileLookup[extension]);
        }

        
        public static Configuration ReadFile(string filepath)
        {
            string contents = System.IO.File.ReadAllText(filepath);
            string extension = System.IO.Path.GetExtension(filepath);
            var config = CreateReader(extension).Read(contents);
            config.FilePath = filepath;
            return config;
        }

        public abstract Configuration Read(string data);
    }
}
