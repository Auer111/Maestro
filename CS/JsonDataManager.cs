using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Maestro.Models;
using System.IO;
using Newtonsoft.Json;

namespace Maestro.CS
{
    public class JsonDataManager
    {

        //Validate file
        public static bool ValidateNewFileName(string path, string fileName) { return !string.IsNullOrEmpty(fileName) && fileName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0 && !File.Exists(Path.Combine(path, fileName)); }
        public static bool ValidateRenamedFile(string fileName) { return !string.IsNullOrEmpty(fileName) && fileName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0; }
        public static void OpenDir()
        {
            Process.Start(Directory.GetCurrentDirectory());
        }

        public static void OpenDir(string path)
        {
            Process.Start(path);
        }

        //--------------------------------PERSIST JSON USER CONTENT-------------------------
        public static List<string> GetJsonDataFiles(string path) { return Directory.EnumerateFiles(path).Where(file => file.EndsWith("json")).ToList(); }

        
        public static string GetJsonRaw(string file)
        {
            using (StreamReader r = new StreamReader(file))
            {
                string json = r.ReadToEnd();
                return json;
            }
        }
        public static List<T> GetJsonDeserialized<T>(string file)
        {
            using (StreamReader r = new StreamReader(file))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<List<T>>(json);
            }
        }
        public static List<T> GetJsonDeserialized<T>(string file, out string raw)
        {
            using (StreamReader r = new StreamReader(file))
            {
                string json = r.ReadToEnd();
                raw = json;
                return JsonConvert.DeserializeObject<List<T>>(json);
            }
        }

        public static bool CreateJsonDataFile(string path, string file)
        {
            
            try
            {
                var newFile = File.Create(Path.Combine(path, file));
                newFile.Close();
                return true;
            }
            catch (SystemException e) { Console.Write(e.Message); return false; }
        }
        public static bool OverwriteJsonDataFile<T>(string path, string file, List<T> content)
        {
            try
            {
                File.WriteAllText(Path.Combine(path, file), JsonConvert.SerializeObject(content));
                return true;
            }
            catch (SystemException e) { Console.Write(e.Message); return false; }
        }

        public static bool DeleteJsonDataFile(string path ,string file)
        {
            try
            {
                File.Delete(Path.Combine(path, file));
                return true;
            }
            catch (SystemException e) { Console.Write(e.Message); return false; }
        }


        public static string ToJSON(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }



        public static Array EnumSerializer<T>() { return (T[])Enum.GetValues(typeof(T)); }
    }
}
