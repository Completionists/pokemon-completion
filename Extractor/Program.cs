using System.IO;
using PKHeX.Core;

class Program
{
    static void Main(string[] args)
    {
        string savFile = args[0];
        string outputDir = args[1];
        Directory.CreateDirectory(outputDir);

        var data = File.ReadAllBytes(savFile);
        var sav = SaveUtil.GetVariantSAV(data);
        if (sav == null)
        {
            Console.WriteLine("Invalid save file.");
            return;
        }

        int count = 0;
        for (int box = 0; box < sav.BoxCount; box++)
        {
            var boxData = sav.GetBox(box);
            foreach (var slot in boxData)
            {
                if (slot != null)
                {
                    string fname = Path.Combine(outputDir, $"box{box}_slot{count}.pk{slot.Format}");
                    File.WriteAllBytes(fname, slot.DecryptedBoxData);
                    count++;
                }
            }
        }

        Console.WriteLine($"Extracted {count} Pokémon.");
    }
}