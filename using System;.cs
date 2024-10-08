using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lab1
{
    class FileParser
    {
        public static string GenDataFile = "sequences.0.txt";
        public static string CommandsDataFile = "commands.0.txt";
        public static string OutputDataFile = "genedata.txt";
        public static List<GeneticData> ReadGeneticData()
        {
            StreamReader reader = new StreamReader(GenDataFile);
            List<GeneticData> data = new List<GeneticData>();
            int counter = 0;
            while (!reader.EndOfStream)
            {
                counter++;
                if (counter > 25)
                {
                    Console.WriteLine("Too much proteins, input is stopped\n");
                    break;
                }
                string line = reader.ReadLine();
                string[] parts = line.Split('\t');
                GeneticData temp;
                temp.name = parts[0];
                temp.organism = parts[1];
                temp.formula = GeneticData.Decode(parts[2]);
                data.Add(temp);
            }
            reader.Close();
            return data;
        }
    }
}
