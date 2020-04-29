using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            AddConvertersToDirectory(directories[2]);

            //Get all WEM files into our working directory
            Console.WriteLine("Extracting BNK soundbanks...");
            ExtractBnkToWorkingDirectory();
            Console.WriteLine("Extracting PCK soundbanks...");
            ExtractPckToWorkingDirectory();
            Console.WriteLine("Copying WEMs...");
            CopyWemToWorkingDirectory();

            //Convert and name the WEMs
            Console.WriteLine("Converting to proper names...");
            RenameWemFiles();
            Console.WriteLine("Converting untitled sounds...");
            ConvertRemainingWemFiles();

            //Clear up conversion resources
            Console.WriteLine("Clearing up...");
            RemoveConvertersFromDirectory(directories[2]);

            //Finished
            Process.Start(directories[1]);
            Environment.Exit(0);
        }

        /* Copy existing WEMs to our working directory */
        private static void CopyWemToWorkingDirectory()
        {
            string[] searchQuery = Directory.GetFiles(directories[0], "*.WEM", SearchOption.AllDirectories);
            foreach (string file in searchQuery)
            {
                File.Copy(file, directories[2] + "\\" + Path.GetFileName(file), true);
            }
        }

        /* Try and extract all soundbank files to working directory */
        private static void ExtractBnkToWorkingDirectory()
        {
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
        private static void ExtractPckToWorkingDirectory()
        {
            var searchQueryPCK = Directory.GetFiles(directories[0], "*.PCK", SearchOption.AllDirectories);
            foreach (var file in searchQueryPCK)
            {
                BinaryReader reader = new BinaryReader(File.OpenRead(file));
                reader.BaseStream.Position += 12;
                reader.BaseStream.Position += reader.ReadInt32() + 20;

                List<AudioFile> audioFiles = new List<AudioFile>(reader.ReadInt32());
                for (int i = 0; i < audioFiles.Capacity; i++)
                {
                    AudioFile thisFile = new AudioFile();
                    thisFile.fileID = reader.ReadInt32();
                    thisFile.unk1 = reader.ReadInt32();
                    thisFile.fileLength = reader.ReadInt32();
                    thisFile.offsetInPCK = reader.ReadInt32();
                    thisFile.unk2 = reader.ReadInt32();
                    audioFiles.Add(thisFile);
                }
                foreach (AudioFile thisFile in audioFiles)
                {
                    reader.BaseStream.Position = thisFile.offsetInPCK;
                    BinaryWriter writer = new BinaryWriter(File.OpenWrite(directories[2] + "/" + thisFile.fileID + ".WEM"));
                    writer.BaseStream.SetLength(0);
                    writer.Write(reader.ReadBytes(thisFile.fileLength));
                    writer.Close();
                }

                reader.Close();
            }
        }

        /* Match WEMs in the working directory with an entry in the soundbank, then convert to OGG and rename */
        private static void RenameWemFiles()
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
                        string outPath = Path.GetDirectoryName(directories[1] + "/" + thisFile["new_name"].Value<string>());
                        string outName = Path.GetFileName(thisFile["new_name"].Value<string>());

                        Directory.CreateDirectory(outPath);
                        outPath += "/" + outName;
                        if (ConvertWEM(file)) outPath = outPath.Substring(0, outPath.Length - 3) + "ogg";
                        else outPath = outPath.Substring(0, outPath.Length - 3) + "wem";

                        if (File.Exists(outPath)) File.Delete(outPath);
                        File.Move(file, outPath);
                        File.Delete(file);
                        break;
                    }
                }
            }
        }

        /* Convert remaining untitled WEMs in the working directory to OGG */
        private static void ConvertRemainingWemFiles()
        {
            var searchQuery = Directory.GetFiles(directories[2], "*.WEM", SearchOption.TopDirectoryOnly);
            foreach (var file in searchQuery)
            {
                if (ConvertWEM(file)) File.Move(file, file.Substring(0, file.Length - 3) + "ogg");
            }
        }

        /* Convert a WEM to OGG if possible */
        private static bool ConvertWEM(string inName)
        {
            RunProgramAndWait("ww2ogg.exe", "\"" + Path.GetFileName(inName) + "\" --pcb packed_codebooks_aoTuV_603.bin -o \"" + Path.GetFileName(inName) + "_conv\"", inName.Substring(0, inName.Length - Path.GetFileName(inName).Length));
            RunProgramAndWait("revorb.exe", Path.GetFileName(inName) + "_conv", inName.Substring(0, inName.Length - Path.GetFileName(inName).Length));
            if (!File.Exists(inName + "_conv")) return false;
            File.Delete(inName);
            File.Move(inName + "_conv", inName);
            return true;
        }

        /* Place converter resources in a given directory */
        private static void AddConvertersToDirectory(string directory)
        {
            File.WriteAllBytes(directory + "\\ww2ogg.exe", Properties.Resources.ww2ogg);
            File.WriteAllBytes(directory + "\\revorb.exe", Properties.Resources.revorb);
            File.WriteAllBytes(directory + "\\bnkextr.exe", Properties.Resources.bnkextr);
            File.WriteAllBytes(directory + "\\base_library.zip", Properties.Resources.base_library);
            File.WriteAllBytes(directory + "\\python36.dll", Properties.Resources.python36);
            File.WriteAllBytes(directory + "\\packed_codebooks_aoTuV_603.bin", Properties.Resources.packed_codebooks_aoTuV_603);
        }

        /* Remove converter resources from a given directory */
        private static void RemoveConvertersFromDirectory(string directory)
        {
            File.Delete(directory + "\\ww2ogg.exe");
            File.Delete(directory + "\\revorb.exe");
            File.Delete(directory + "\\bnkextr.exe");
            File.Delete(directory + "\\base_library.zip");
            File.Delete(directory + "\\python36.dll");
            File.Delete(directory + "\\packed_codebooks_aoTuV_603.bin");
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
