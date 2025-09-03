using System.Text.RegularExpressions;

namespace FileSearch
{
    internal class Program
    {
        public static List<string> FileSearch(string catalog)
        {
            string[] files;
            List<string> fileNames = new List<string>();
            try
            {
                files = Directory.GetFiles(catalog);
                if (files.Length > 0)
                {
                    foreach (string fileName in files)
                    {
                        fileNames.Add(fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }           
            return fileNames;
        }
        public static List<int> AllIndexesOf(string content, string word)
        {            
            List<int> indexes = new List<int>();
            word = word.ToLower();
            for (int index = 0; ; index += word.Length)
            {
                index = content.IndexOf(word, index);
                if (index == -1)
                {
                    return indexes;
                }                    
                indexes.Add(index);
            }
        }
        public static Dictionary<string,List<int>> SreahWord(string fileName,string word,Dictionary<string,List<int>> occurrencesWord)
        {
            var fileСontent = File.ReadAllText(fileName);
            fileСontent=fileСontent.ToLower();
            var List = AllIndexesOf(fileСontent, word);
            occurrencesWord.Add(fileName, List);
            return occurrencesWord;
        }
        
        static void Main(string[] args)
        {
            bool work = true;
            
            while (work == true)
            {
                string word = string.Empty;
                while (word == string.Empty)
                {
                    string catalog = "C:\\Users\\user\\source\\repos\\fileSearch\\FileSearchWord\\bin\\Debug\\net8.0";
                    Console.WriteLine("Введите название каталога");
                    catalog = Console.ReadLine();
                    Console.WriteLine("Введите слово для поиска, для выхода из программы нажмите f");
                    word = Console.ReadLine();
                    if (word != string.Empty)
                    {
                        if (word == "f")
                        {
                            work = false;
                        }
                        else
                        {
                            List<string> files = FileSearch(catalog);
                            if (files.Count() > 0) 
                            {
                                Dictionary<string, List<int>> occurrencesWord = new Dictionary<string, List<int>>();
                                List<Task<Dictionary<string, List<int>>>> tasks = new List<Task<Dictionary<string, List<int>>>>();
                                foreach (string fileName in files)
                                {
                                    tasks.Add(Task.Run(() => SreahWord(fileName, word, occurrencesWord)));
                                    Console.WriteLine();
                                }
                                Task.WaitAll(tasks.ToArray());
                                foreach (var sreahWord in occurrencesWord)
                                {
                                    Console.WriteLine($" В файле {sreahWord.Key} найдено совпадений {sreahWord.Value.Count()}");
                                }
                            }
                            else 
                            { 
                                Console.WriteLine("Каталог пуст"); 
                            }
                            
                        }
                        
                    }
                }
                
            }
        }
    }
}
