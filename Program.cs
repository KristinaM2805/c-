
using System;
using System.Collections.Generic;
using System.IO;

namespace Lab1
{
    public struct GeneticData
    {
        public static List<char> letters = new List<char> { 'A', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'V', 'W', 'Y' };
        public string name;
        public string organism;
        public string formula;

        public static string Decode(string formula)
        {
            string decoded = "";
            for (int i = 0; i < formula.Length; i++)
            {
                if (char.IsDigit(formula[i]))
                {
                    int count = formula[i] - '0';
                    char letter = formula[i + 1];
                    decoded += new string(letter, count);
                    i++;
                }
                else
                {
                    decoded += formula[i];
                }
            }
            return decoded;
        }

        public bool IsValid()
        {
            foreach (char ch in Decode(formula))
            {
                if (!letters.Contains(ch)) return false;
            }
            return true;
        }
    }

    class Program
    {
        static List<GeneticData> data = FileParser.ReadGeneticData();

        static string GetFormula(string proteinName)
        {
            foreach (var item in data)
            {
                if (item.name == proteinName)
                    return item.formula;
            }
            return null;
        }

        static List<int> Search(string aminoAcid)
        {
            string decoded = GeneticData.Decode(aminoAcid);
            List<int> results = new List<int>();

            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].formula.Contains(decoded))
                    results.Add(i);
            }

            return results;
        }

        static int Diff(string name1, string name2)
        {
            string formula1 = GetFormula(name1), formula2 = GetFormula(name2);

            if (formula1 == null) return -1;
            if (formula2 == null) return -2;

            int diff = Math.Abs(formula1.Length - formula2.Length);
            int minLength = Math.Min(formula1.Length, formula2.Length);

            for (int i = 0; i < minLength; i++)
            {
                if (formula1[i] != formula2[i])
                    diff++;
            }

            return diff;
        }

        static int Mode(string name)
        {
            string formula = GetFormula(name);
            if (formula == null) return -1;

            int[] counts = new int[GeneticData.letters.Count];
            foreach (char ch in formula)
            {
                int index = GeneticData.letters.IndexOf(ch);
                if (index != -1) counts[index]++;
            }

            int maxCount = 0, maxIndex = 0;
            for (int i = 0; i < counts.Length; i++)
            {
                if (counts[i] > maxCount)
                {
                    maxCount = counts[i];
                    maxIndex = i;
                }
            }

            return maxIndex;
        }

        public static void HandleCommands()
        {
            using (StreamReader reader = new StreamReader(FileParser.CommandsDataFile))
            using (StreamWriter writer = new StreamWriter(FileParser.OutputDataFile))
            {
                Console.WriteLine("Genetic Searching");

                int lineNum = 0;
                while (!reader.EndOfStream)
                {
                    string[] command = reader.ReadLine().Split('\t');
                    lineNum++;

                    switch (command[0])
                    {
                        case "search":
                            Console.WriteLine($"{lineNum}. Searching: {GeneticData.Decode(command[1])}");
                            var indices = Search(command[1]);
                            if (indices.Count > 0)
                            {
                                foreach (int idx in indices)
                                {
                                    Console.WriteLine($"Organism: {data[idx].organism}, Protein: {data[idx].name}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("NOT FOUND");
                            }
                            break;

                        case "diff":
                            int diffResult = Diff(command[1], command[2]);
                            if (diffResult >= 0)
                            {
                                Console.WriteLine($"Difference: {diffResult}");
                            }
                            else
                            {
                                Console.WriteLine($"MISSING: {(diffResult == -1 ? command[1] : command[2])}");
                            }
                            break;

                        case "mode":
                            int modeResult = Mode(command[1]);
                            if (modeResult >= 0)
                            {
                                Console.WriteLine($"Most frequent amino acid: {GeneticData.letters[modeResult]}");
                            }
                            else
                            {
                                Console.WriteLine($"MISSING: {command[1]}");
                            }
                            break;
                    }
                }
            }
        }

        static void Main()
        {
            foreach (var entry in data)
            {
                if (!entry.IsValid())
                {
                    Console.WriteLine($"Invalid protein formula for {entry.name}");
                    return;
                }
            }

            HandleCommands();
        }
    }
}
