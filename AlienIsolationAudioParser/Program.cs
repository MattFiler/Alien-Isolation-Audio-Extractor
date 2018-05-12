using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace AlienIsolationAudioExtractor
{
    class Program
    {
        private static string[] directories = { "DATA\\SOUND", "DATA\\SOUND_ORGANISED", "DATA\\SOUND_CONVERTED" };
        private enum CounterType { EXPORTED, NOT_FOUND, SKIPPED };

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Alien: Isolation Audio Extractor" + Environment.NewLine +
                              "Created by Matt Filer" + Environment.NewLine +
                              "---" + Environment.NewLine +
                              "Please be aware this program will take around 30 mins to 1 hour to process.");

            //Validate we're running in the correct directory.
            if (!File.Exists("AI.exe"))
            {
                Console.WriteLine("---" + Environment.NewLine + "Could not locate sounds." + Environment.NewLine + "Please place this tool in your Alien: Isolation directory.");
                Console.Read();
                Environment.Exit(0);
            }

            //Copy resources and make directories
            Directory.CreateDirectory(directories[1]);
            Directory.CreateDirectory(directories[2]);
            File.WriteAllBytes(directories[2] + "\\ww2ogg.exe", Properties.Resources.ww2ogg);
            File.WriteAllBytes(directories[2] + "\\revorb.exe", Properties.Resources.revorb);
            File.WriteAllBytes(directories[2] + "\\bnkextr.exe", Properties.Resources.bnkextr);
            File.WriteAllBytes(directories[2] + "\\base_library.zip", Properties.Resources.base_library);
            File.WriteAllBytes(directories[2] + "\\python36.dll", Properties.Resources.python36);
            File.WriteAllBytes(directories[2] + "\\packed_codebooks_aoTuV_603.bin", Properties.Resources.packed_codebooks_aoTuV_603);

            //Extract and convert all files
            Console.WriteLine("---" + Environment.NewLine + "Extracting soundbanks.");
            extractSoundbanks();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Environment.NewLine + "---" + Environment.NewLine + "Converting soundbanks.");
            convertSoundbankFiles();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Environment.NewLine + "---" + Environment.NewLine + "Converting remaining sound files.");
            convertBaseSoundFiles();

            //Clear up conversion resources
            foreach (var file in Directory.GetFiles(directories[2], "*.exe", SearchOption.AllDirectories))
            {
                File.Delete(file);
            }
            File.Delete(directories[2] + "\\base_library.zip");
            File.Delete(directories[2] + "\\python36.dll");
            File.Delete(directories[2] + "\\packed_codebooks_aoTuV_603.bin");

            //Delete any empty directories from conversion
            foreach (var directory in Directory.GetDirectories(directories[2]))
            {
                if (Directory.GetFiles(directory).Length == 0 &&
                    Directory.GetDirectories(directory).Length == 0)
                {
                    Directory.Delete(directory, false);
                }
            }

            //Process file names
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Environment.NewLine + "---" + Environment.NewLine + "Starting file rename process.");
            nameFilesIfPossible();

            //Finished
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("---" + Environment.NewLine +
                          "Finished processing all files." + Environment.NewLine +
                          "Press any key to close.");
            Console.Read();
        }


        /*
         * Rename files if possible and place in directories[1] NEW
         */
        private static void nameFilesIfPossible()
        {
            //Load soundbank & files listed
            XDocument soundbankXML = XDocument.Parse(Properties.Resources.SOUNDBANKSINFO);
            var fileArray = soundbankXML.Descendants("File");
            int[] counter = { 0,0,0 }; 

            //Run through each file...
            foreach (var file in fileArray)
            {
                try
                {
                    //Get attributes of current file
                    string originalFileNameID = file.Attribute("Id").Value.ToString();
                    string originalFileNameLanguage = file.Attribute("Language").Value.ToString();
                    string originalFileNameShortName = "";
                    foreach (var shortName in file.Descendants("ShortName"))
                    {
                        originalFileNameShortName = shortName.Value.ToString().Substring(0, shortName.Value.Length - 3) + "ogg";
                    }

                    //Skip out any non sfx files - currently unsupported
                    if (originalFileNameLanguage != "SFX")
                    {
                        counter[(int)CounterType.SKIPPED]++;
                        continue;
                    }

                    //Try and find OGG file to rename
                    bool hasFoundFile = false;
                    foreach (string oggFile in Directory.GetFiles(directories[2], originalFileNameID + ".ogg", SearchOption.AllDirectories))
                    {
                        Directory.CreateDirectory(directories[1] + "\\SFX\\" + Path.GetDirectoryName(originalFileNameShortName));
                        if (!File.Exists(oggFile))
                        {
                            File.Copy(oggFile, directories[1] + "\\SFX\\" + originalFileNameShortName);
                            counter[(int)CounterType.EXPORTED]++;
                        }
                        hasFoundFile = true;
                    }

                    //Didn't find an OGG file, try find a WEM file (last resort)
                    if (!hasFoundFile)
                    {
                        foreach (string wemFile in Directory.GetFiles(directories[2], originalFileNameID + ".wem", SearchOption.AllDirectories))
                        {
                            Directory.CreateDirectory(directories[1] + "\\SFX\\" + Path.GetDirectoryName(originalFileNameShortName));
                            if (!File.Exists(directories[1] + "\\SFX\\" + originalFileNameShortName.Substring(0, originalFileNameShortName.Length - 3) + "wem"))
                            {
                                File.Copy(wemFile, directories[1] + "\\SFX\\" + originalFileNameShortName.Substring(0, originalFileNameShortName.Length - 3) + "wem");
                                counter[(int)CounterType.EXPORTED]++;
                            }
                            hasFoundFile = true;
                        }
                    }

                    //Didn't find a file at all!
                    if (!hasFoundFile)
                    {
                        counter[(int)CounterType.NOT_FOUND]++;
                    }
                }
                catch
                {
                    //Most likely no shortname, this is normal.
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\r{0}   ", counter[(int)CounterType.EXPORTED] + " named, " + counter[(int)CounterType.NOT_FOUND] + " referenced but not found, " + counter[(int)CounterType.SKIPPED] + " skipped out.");
            }
        }


        /*
         * Try and convert all WEM files in directories[0]
         */
        private static void convertBaseSoundFiles()
        {
            int counter = 0;
            var searchQuery = Directory.GetFiles(directories[0], "*.WEM", SearchOption.AllDirectories);
            foreach (var file in searchQuery)
            {
                string inputFile = "..\\SOUND\\" + file.Split(new[] { "SOUND\\" }, StringSplitOptions.None)[1];
                string outputFile = file.Split(new[] { "SOUND\\" }, StringSplitOptions.None)[1];
                outputFile = outputFile.Substring(0, outputFile.Length - 3) + "ogg";

                if (!File.Exists(outputFile)) //Check file hasn't already been converted.
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(directories[2] + outputFile));

                    RunProgramAndWait("ww2ogg.exe", "\"" + inputFile + "\" --pcb packed_codebooks_aoTuV_603.bin -o \"" + outputFile + "\"", directories[2]);
                    RunProgramAndWait("revorb.exe", outputFile, directories[2]);

                    if (!File.Exists(directories[2] + outputFile))
                    {
                        File.Copy(directories[0] + "\\" + file.Split(new[] { "SOUND\\" }, StringSplitOptions.None)[1], directories[2] + "\\" + outputFile.Substring(0, outputFile.Length - 3) + "wem"); //Couldn't convert, copy original.
                    }
                }
                
                counter++;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\r{0}   ", counter + " of " + searchQuery.Count() + " converted.");
            }
        }


        /*
         * Try and convert all files exported from BNKs
         */
        private static void convertSoundbankFiles()
        {
            int counter = 0;
            var searchQuery = Directory.GetFiles(directories[2], "*.wem", SearchOption.AllDirectories);
            foreach (var file in searchQuery)
            {
                string fileName = file.Split(new[] { "SOUND_CONVERTED\\" }, StringSplitOptions.None)[1];
                if (File.Exists(file.Substring(0, file.Length - 3) + "ogg"))
                {
                    File.Delete(file); //File already converted somehow, ignore.
                }
                else
                {
                    RunProgramAndWait("ww2ogg.exe", "\"" + fileName + "\" --pcb packed_codebooks_aoTuV_603.bin", directories[2]);
                    RunProgramAndWait("revorb.exe", fileName.Substring(0, fileName.Length - 3) + "ogg", directories[2]);

                    if (File.Exists(file.Substring(0, file.Length - 3) + "ogg"))
                    {
                        File.Delete(file); //Only delete original if conversion successful.
                    }

                    counter++;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("\r{0}   ", counter + " of " + searchQuery.Count() + " converted.");
                }
            }
        }


        /*
         * Try and extract all BNK files in directories[0]
         */
        private static void extractSoundbanks()
        {
            int counter = 0;
            var searchQuery = Directory.GetFiles(directories[0], "*.BNK", SearchOption.AllDirectories);
            foreach (var file in searchQuery)
            {
                string workingFile = file.Split(new[] { "SOUND\\" }, StringSplitOptions.None)[1];

                File.Copy(file, directories[2] + "\\" + Path.GetFileName(workingFile));
                RunProgramAndWait("bnkextr.exe", Path.GetFileName(workingFile), directories[2]);
                File.Delete(directories[2] + "\\" + Path.GetFileName(workingFile)); //Remove BNK after extract.

                counter++;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\r{0}   ", counter + " of " + searchQuery.Count() + " processed.");
            }
        }


        /*
         * Generic function for running a program and waiting for it to finish
         */
        private static void RunProgramAndWait(string exe, string args, string directory)
        {
            Process program = new Process();
            program.StartInfo.FileName = Directory.GetCurrentDirectory() + "\\" + directory + "\\" + exe;
            program.StartInfo.Arguments = args;
            program.StartInfo.WorkingDirectory = directory;
            program.StartInfo.UseShellExecute = false;
            program.StartInfo.CreateNoWindow = true;
            program.Start();
            program.WaitForExit();
        }
    }
}
