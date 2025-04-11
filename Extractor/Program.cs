using PKHeX.Core;
using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: Extractor <input.sav> <output-folder>");
            return;
        }

        string inputFile = args[0];
        string outputDir = args[1];

        if (!File.Exists(inputFile))
        {
            Console.WriteLine($"File not found: {inputFile}");
            return;
        }

        Directory.CreateDirectory(outputDir);
        var data = File.ReadAllBytes(inputFile);
        var sav = SaveUtil.GetVariantSAV(data);
        if (sav == null)
        {
            Console.WriteLine("Failed to load save file.");
            return;
        }

        int count = 0;
        var boxes = sav.BoxData;
        if (boxes == null)
        {
            Console.WriteLine("Save file does not contain box data.");
            return;
        }

        for (int box = 0; box < boxes.Length; box++)
        {
            var boxData = boxes[box];
            for (int i = 0; i < boxData.Length; i++)
            {
                var slot = boxData[i];
                if (slot != null)
                {
                    string ext = slot.Format.ToString(); // e.g., "8"
                    string outPath = Path.Combine(outputDir, $"box{box + 1}_slot{i + 1}.pk{ext}");
                    File.WriteAllBytes(outPath, slot.DecryptedBoxData);
                    count++;
                }
            }
        }

        Console.WriteLine($"Extracted {count} Pokémon to {outputDir}");
    }
}
