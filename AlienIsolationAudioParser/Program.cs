using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace AlienIsolationAudioExtractor
{
    class Program
    {
        private static string[] directories = { "DATA\\SOUND", "DATA\\SOUND_ORGANISED", "DATA\\SOUND_UNORGANISED" };

        static void Main(string[] args)
        {
            Console.WriteLine("Alien: Isolation Audio Extractor");
            Console.WriteLine("Created by Matt Filer");
            Console.WriteLine("----");
            Console.WriteLine("This window will close when extraction is complete.");
            Console.WriteLine("----");

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
            Console.WriteLine("Setting up...");
            if (Directory.Exists(directories[1])) Directory.Delete(directories[1], true);
            if (Directory.Exists(directories[2])) Directory.Delete(directories[2], true);

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

            //Get all WEM files into our working directory
            Console.WriteLine("Copying WEMs...");
            copyWEMs();
            Console.WriteLine("Extracting BNK soundbanks...");
            extractBNK();
            Console.WriteLine("Extracting PCK soundbanks...");
            extractPCK();

            //Convert and name the WEMs
            Console.WriteLine("Converting to proper names...");
            renameWEMs();

            //Clear up conversion resources
            Console.WriteLine("Clearing up...");
            foreach (var file in Directory.GetFiles(directories[2], "*.exe", SearchOption.AllDirectories))
            {
                File.Delete(file);
            }
            File.Delete(directories[2] + "\\base_library.zip");
            File.Delete(directories[2] + "\\python36.dll");
            File.Delete(directories[2] + "\\packed_codebooks_aoTuV_603.bin");
            File.Delete(directories[2] + "\\wavescan.bms");

            //Finished
            Process.Start(directories[1]);
            Environment.Exit(0);
        }

        /* Copy existing WEMs from the game's SOUND folder to our working directory */
        private static void copyWEMs()
        {
            string[] searchQuery = Directory.GetFiles(directories[0], "*.WEM", SearchOption.AllDirectories);
            foreach (string file in searchQuery)
            {
                File.Copy(file, directories[2] + "\\" + Path.GetFileName(file), true);
            }
        }

        /* Try and extract all soundbank files in directories[0] */
        private static void extractBNK()
        {
            /* BNK Soundbanks */
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
            }
        }
        private static void extractPCK()
        {
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
            }
        }

        /* Match WEMs in the working directory with an entry in the soundbank, then export */
        private static void renameWEMs()
        {
            JArray soundbankData = JObject.Parse(Properties.Resources.soundbank)["soundbank_names"].ToObject<JArray>();
            var searchQuery = Directory.GetFiles(directories[2], "*.WEM", SearchOption.TopDirectoryOnly);
            foreach (var file in searchQuery)
            {
                int thisFileID = Convert.ToInt32(Path.GetFileNameWithoutExtension(file));
                foreach (JObject thisFile in soundbankData)
                {
                    if (thisFile["original_id"].Value<int>() == thisFileID)
                    {
                        string inName = Path.GetFileName(file);
                        string outPath = Path.GetDirectoryName(directories[1] + "/" + thisFile["new_name"].Value<string>());
                        string outName = Path.GetFileName(thisFile["new_name"].Value<string>());

                        Directory.CreateDirectory(outPath);
                        outPath += "/" + outName;

                        RunProgramAndWait("ww2ogg.exe", "\"" + inName + "\" --pcb packed_codebooks_aoTuV_603.bin -o \"" + inName + "_conv\"", directories[2]);
                        RunProgramAndWait("revorb.exe", inName + "_conv", directories[2]);
                        if (File.Exists(file + "_conv"))
                        {
                            outPath = outPath.Substring(0, outPath.Length - 3) + "ogg";
                            if (File.Exists(outPath)) File.Delete(outPath);
                            File.Move(file + "_conv", outPath);
                        }
                        else
                        {
                            File.Move(file, outPath.Substring(0, outPath.Length - 3) + "wem");
                        }
                        File.Delete(file);
                        break;
                    }
                }
            }
        }

        /* Generic function for running a program and waiting for it to finish */
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
