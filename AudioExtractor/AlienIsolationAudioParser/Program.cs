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
        private static string[] directories = { "DATA\\SOUND", "DATA\\SOUND_ORGANISED", "DATA\\SOUND_UNORGANISED" };

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Alien: Isolation Audio Extractor" + Environment.NewLine +
                                "Created by Matt Filer" + Environment.NewLine +
                                "---" + Environment.NewLine +
                                "This process will take around an hour and a half to complete." + Environment.NewLine +
                                "---");

            //Validate we're running in the correct directory
            if (!File.Exists("AI.exe"))
            {
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("Could not locate sounds." + Environment.NewLine + "Please place this tool in your Alien: Isolation directory.");
                Console.Read();
                Environment.Exit(0);
            }

            //Check there are no lingering files which could interfere with export
            if (Directory.Exists(directories[1]) || Directory.Exists(directories[2]))
            {
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("Files remain from a previous conversion." + Environment.NewLine + "Please delete/rename folders \"" + directories[1] + "\" and \"" + directories[2] + "\" before running again.");
                Console.Read();
                Environment.Exit(0);
            }

            //Wait for user input to start
            Console.WriteLine(Environment.NewLine + "Press enter to begin.");
            Console.Read();

            //Copy resources and make directories
            Directory.CreateDirectory(directories[1]);
            Directory.CreateDirectory(directories[2]);
            File.WriteAllBytes(directories[2] + "\\ww2ogg.exe", Properties.Resources.ww2ogg);
            File.WriteAllBytes(directories[2] + "\\revorb.exe", Properties.Resources.revorb);
            File.WriteAllBytes(directories[2] + "\\bnkextr.exe", Properties.Resources.bnkextr);
            File.WriteAllBytes(directories[2] + "\\base_library.zip", Properties.Resources.base_library);
            File.WriteAllBytes(directories[2] + "\\python36.dll", Properties.Resources.python36);
            File.WriteAllBytes(directories[2] + "\\packed_codebooks_aoTuV_603.bin", Properties.Resources.packed_codebooks_aoTuV_603);
            File.WriteAllBytes(directories[2] + "\\quickbms.exe", Properties.Resources.quickbms);
            File.WriteAllBytes(directories[2] + "\\wavescan.bms", Properties.Resources.wavescan);

            //Extract and convert all files
            Console.WriteLine("---" + Environment.NewLine + "Extracting soundbanks:");
            extractSoundbanks();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Environment.NewLine + "---" + Environment.NewLine + "Converting all soundbank outputs:");
            convertSoundbankFiles();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Environment.NewLine + "---" + Environment.NewLine + "Converting remaining sound files:");
            convertBaseSoundFiles();

            //Clear up conversion resources
            foreach (var file in Directory.GetFiles(directories[2], "*.exe", SearchOption.AllDirectories))
            {
                File.Delete(file);
            }
            File.Delete(directories[2] + "\\base_library.zip");
            File.Delete(directories[2] + "\\python36.dll");
            File.Delete(directories[2] + "\\packed_codebooks_aoTuV_603.bin");
            File.Delete(directories[2] + "\\wavescan.bms");

            //Delete any empty directories from conversion
            foreach (var directory in Directory.GetDirectories(directories[2]))
            {
                if (Directory.GetFiles(directory).Length == 0 &&
                    Directory.GetDirectories(directory).Length == 0)
                {
                    Directory.Delete(directory, false);
                }
            }

            //Delete any duplicate wem files
            foreach (var file in Directory.GetFiles(directories[2], "*.wem", SearchOption.AllDirectories))
            {
                if (File.Exists(file.Substring(0, file.Length - 3) + "ogg"))
                {
                    File.Delete(file); //Delete all duplicate WEMs in unorganised directory
                }
            }

            //Process file names
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Environment.NewLine + "---" + Environment.NewLine + "Renaming files where possible:");
            nameFilesIfPossible();

            //Finished
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(Environment.NewLine + "---" + Environment.NewLine +
                            "Finished processing all files." + Environment.NewLine + Environment.NewLine +
                            "Press enter to close.");
            Console.ReadLine();
            Console.ReadLine();
            Environment.Exit(0);
        }


        /*
         * Rename files if possible and place in directories[1] NEW
         */
        private static void nameFilesIfPossible()
        {
            //Load soundbank & files listed
            XDocument soundbankXML = XDocument.Parse(Properties.Resources.SOUNDBANKSINFO);
            var fileArray = soundbankXML.Descendants("File");

            //Run through each file...
            int counter = 0;
            foreach (var file in fileArray)
            {
                counter++;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\r{0}   ", counter + " of " + fileArray.Count() + " processed.");
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
                    
                    if (originalFileNameLanguage == "SFX")
                    {
                        //Create directory no matter if we find the file.
                        Directory.CreateDirectory(directories[1] + "\\SFX\\" + Path.GetDirectoryName(originalFileNameShortName));

                        //Try and find OGG file to rename
                        foreach (string oggFile in Directory.GetFiles(directories[2], originalFileNameID + ".ogg", SearchOption.AllDirectories))
                        {
                            if (!File.Exists(directories[1] + "\\SFX\\" + originalFileNameShortName))
                            {
                                File.Copy(oggFile, directories[1] + "\\SFX\\" + originalFileNameShortName);
                                File.Delete(oggFile);
                                continue;
                            }
                        }

                        //Didn't find an OGG file, try find a WEM file (last resort)
                        foreach (string wemFile in Directory.GetFiles(directories[2], originalFileNameID + ".wem", SearchOption.AllDirectories))
                        {
                            if (!File.Exists(directories[1] + "\\SFX\\" + originalFileNameShortName.Substring(0, originalFileNameShortName.Length - 3) + "wem"))
                            {
                                File.Copy(wemFile, directories[1] + "\\SFX\\" + originalFileNameShortName.Substring(0, originalFileNameShortName.Length - 3) + "wem");
                                File.Delete(wemFile);
                                continue;
                            }
                        }
                    }
                    else
                    {
                        //Create language directory if it doesn't already exist
                        Directory.CreateDirectory(directories[1] + "\\Voices\\" + originalFileNameLanguage);

                        //Try find our file as ogg
                        foreach (string voiceFile in Directory.GetFiles(directories[2] + "\\" + originalFileNameLanguage.ToUpper(), Path.GetFileNameWithoutExtension(originalFileNameShortName) + ".ogg", SearchOption.AllDirectories))
                        {
                            if (!File.Exists(directories[1] + "\\Voices\\" + originalFileNameLanguage + "\\" + Path.GetFileNameWithoutExtension(originalFileNameShortName) + ".ogg"))
                            {
                                File.Copy(voiceFile, directories[1] + "\\Voices\\" + originalFileNameLanguage + "\\" + Path.GetFileNameWithoutExtension(originalFileNameShortName) + ".ogg");
                                File.Delete(voiceFile);
                                continue;
                            }
                        }

                        //Try find our file as wem
                        foreach (string voiceFile in Directory.GetFiles(directories[2] + "\\" + originalFileNameLanguage.ToUpper(), Path.GetFileNameWithoutExtension(originalFileNameShortName) + ".wem", SearchOption.AllDirectories))
                        {
                            if (!File.Exists(directories[1] + "\\Voices\\" + originalFileNameLanguage + "\\" + Path.GetFileNameWithoutExtension(originalFileNameShortName) + ".wem"))
                            {
                                File.Copy(voiceFile, directories[1] + "\\Voices\\" + originalFileNameLanguage + "\\" + Path.GetFileNameWithoutExtension(originalFileNameShortName) + ".wem");
                                File.Delete(voiceFile);
                                continue;
                            }
                        }
                    }
                }
                catch
                {
                    //Most likely no shortname, this is normal.
                }
            }
            foreach (var file in Directory.GetFiles(directories[1], "*.wem", SearchOption.AllDirectories))
            {
                if (File.Exists(file.Substring(0, file.Length - 3) + "ogg"))
                {
                    File.Delete(file); //Delete any duplicate WEM files that have come across
                }
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
                outputFile = outputFile.Substring(0, outputFile.Length - 3);

                if (!File.Exists(directories[2] + outputFile + "ogg")) //Check file hasn't already been converted.
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(directories[2] + outputFile));

                    RunProgramAndWait("ww2ogg.exe", "\"" + inputFile + "\" --pcb packed_codebooks_aoTuV_603.bin -o \"" + outputFile + "ogg\"", directories[2]);
                    RunProgramAndWait("revorb.exe", outputFile + "ogg", directories[2]);
                }
                
                counter++;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\r{0}   ", counter + " of " + searchQuery.Count() + " converted.");
            }
            foreach (string file in Directory.GetFiles(directories[0], "*.WEM", SearchOption.AllDirectories))
            {
                string fileThatShouldExist = directories[2] + "\\" + Path.GetFileNameWithoutExtension(file) + "."; 
                if (!File.Exists(fileThatShouldExist + "ogg") && !File.Exists(fileThatShouldExist + "wem"))
                {
                    File.Copy(file, fileThatShouldExist + "wem"); //Copy over any failed conversions to OGG in their raw WEM format to fill the gaps.
                }
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
                string fileName = file.Split(new[] { "SOUND_UNORGANISED\\" }, StringSplitOptions.None)[1];

                RunProgramAndWait("ww2ogg.exe", "\"" + fileName + "\" --pcb packed_codebooks_aoTuV_603.bin", directories[2]);
                RunProgramAndWait("revorb.exe", fileName.Substring(0, fileName.Length - 3) + "ogg", directories[2]);

                counter++;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\r{0}   ", counter + " of " + searchQuery.Count() + " converted.");
            }
        }


        /*
         * Try and extract all soundbank files in directories[0]
         */
        private static void extractSoundbanks()
        {
            /* BNK Soundbanks */
            int counter = 0;
            var searchQuery = Directory.GetFiles(directories[0], "*.BNK", SearchOption.AllDirectories);
            foreach (var file in searchQuery)
            {
                string workingFileLocal = Path.GetFileName(file.Split(new[] { "SOUND\\" }, StringSplitOptions.None)[1]);
                string workingFile = directories[2] + "\\" + workingFileLocal;

                if (File.Exists(workingFile))
                    File.Delete(workingFile);

                File.Copy(file, workingFile);
                RunProgramAndWait("bnkextr.exe", workingFileLocal, directories[2]);
                File.Delete(workingFile); //Remove BNK after extract.

                counter++;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\r{0}   ", counter + " of " + searchQuery.Count() + " processed.");
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Environment.NewLine + "---" + Environment.NewLine + "Converting language soundbanks:");

            /* PCK Soundbanks (languages) */
            counter = 0;
            var searchQueryPCK = Directory.GetFiles(directories[0], "*.PCK", SearchOption.AllDirectories);
            foreach (var file in searchQueryPCK)
            {
                string folderName = Path.GetFileNameWithoutExtension(file).Substring(0, Path.GetFileNameWithoutExtension(file).Length - 9);
                string workingFile = directories[2] + "\\" + folderName + "\\" + Path.GetFileName(file);
                string workingFileLocal = folderName + "\\" + Path.GetFileName(file);

                Directory.CreateDirectory(directories[2] + "\\" + folderName);

                if (File.Exists(workingFile))
                    File.Delete(workingFile);

                File.Copy(file, workingFile);
                File.Copy(directories[2] + "\\quickbms.exe", directories[2] + "\\" + folderName + "\\quickbms.exe");
                RunProgramAndWait("quickbms.exe", "\"../wavescan.bms\" \"" + Path.GetFileName(file) + "\" \"\"", directories[2] + "\\" + folderName);
                File.Delete(workingFile); 

                foreach (var exportedFile in Directory.GetFiles(directories[2] + "\\" + folderName, "*.wem", SearchOption.AllDirectories))
                {
                    string newFileName = Path.GetFileName(exportedFile).Substring(Path.GetFileNameWithoutExtension(file).Length + 1);
                    if (newFileName.Contains("~"))
                    {
                        newFileName = newFileName.Split('~')[1];
                    }
                    if (!File.Exists(Path.GetDirectoryName(exportedFile) + "\\" + newFileName))
                    {
                        File.Move(exportedFile, Path.GetDirectoryName(exportedFile) + "\\" + newFileName);
                    }
                    else
                    {
                        File.Delete(exportedFile); //Duplicate file - this is an odd error.
                    }
                }

                counter++;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\r{0}   ", counter + " of " + searchQueryPCK.Count() + " processed.");
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
