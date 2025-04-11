using PKHeX.Core;
using System;
using System.IO;
using System.Collections.Generic;

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
        IList<IList<PKM>> boxes = sav.BoxData;
        if (boxes == null)
        {
            Console.WriteLine("Save file does not contain box data.");
            return;
        }

        for (int box = 0; box < boxes.Count; box++)
        {
            var boxData = boxes[box];
            for (int i =
