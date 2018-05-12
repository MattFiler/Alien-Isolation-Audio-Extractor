using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace AlienIsolationAudioParser
{
    class Program
    {
        static void Main(string[] args)
        {
            //Load XML and get file array
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Alien: Isolation Audio Parser" + Environment.NewLine + "Created by Matt Filer");
            XDocument soundbankXML = XDocument.Parse(Properties.Resources.SOUNDBANKSINFO);
            var fileArray = soundbankXML.Descendants("File");

            //Set directory based on exe location
            string[] directories = { "", "", "" };
            if (File.Exists("AI.exe"))
            {
                directories[0] = "DATA\\SOUND";
                directories[1] = "DATA\\SOUND_ORGANISED";
                directories[2] = "DATA\\SOUND_CONVERTED";
            }
            else if (File.Exists("..\\AI.exe"))
            {
                directories[0] = "..\\DATA\\SOUND";
                directories[1] = "..\\DATA\\SOUND_ORGANISED";
                directories[2] = "..\\DATA\\SOUND_CONVERTED";
            }
            else
            {
                Console.WriteLine("---" + Environment.NewLine + "Could not locate sounds." + Environment.NewLine + "Please place AlienIsolationAudioParser in your Alien: Isolation directory.");
                Console.Read();
                Environment.Exit(0);
            }

            Directory.CreateDirectory(directories[1]);
            Directory.CreateDirectory(directories[2]);
            File.WriteAllBytes(directories[2] + "\\ww2ogg.exe", Properties.Resources.ww2ogg);
            File.WriteAllBytes(directories[2] + "\\revorb.exe", Properties.Resources.revorb);
            File.WriteAllBytes(directories[2] + "\\bnkextr.exe", Properties.Resources.bnkextr);
            File.WriteAllBytes(directories[2] + "\\base_library.zip", Properties.Resources.base_library);
            File.WriteAllBytes(directories[2] + "\\python36.dll", Properties.Resources.python36);
            File.WriteAllBytes(directories[2] + "\\packed_codebooks_aoTuV_603.bin", Properties.Resources.packed_codebooks_aoTuV_603);
            
            Console.WriteLine("---" + Environment.NewLine + "Starting conversion of sound files to OGG.");
            int counter = 0;

            foreach (var file in Directory.GetFiles(directories[0], "*.WEM", SearchOption.AllDirectories))
            {
                string inputFile = "..\\SOUND\\" + file.Split(new[] { "SOUND\\" }, StringSplitOptions.None)[1];
                string outputFile = file.Split(new[] { "SOUND\\" }, StringSplitOptions.None)[1];
                Directory.CreateDirectory(Path.GetDirectoryName(directories[2] + outputFile));

                Process ww2ogg = new Process();
                ww2ogg.StartInfo.FileName = "ww2ogg.exe";
                ww2ogg.StartInfo.Arguments = "\"" + inputFile + "\" --pcb packed_codebooks_aoTuV_603.bin -o \"" + outputFile.Substring(0,outputFile.Length-3) + "ogg\"";
                ww2ogg.StartInfo.WorkingDirectory = directories[2];
                ww2ogg.Start();
                ww2ogg.WaitForExit();

                Process revorb = new Process();
                revorb.StartInfo.FileName = "revorb.exe";
                revorb.StartInfo.Arguments = outputFile.Substring(0, outputFile.Length - 3) + "ogg";
                revorb.StartInfo.WorkingDirectory = directories[2];
                revorb.Start();
                revorb.WaitForExit();

                counter++;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\r{0}   ", counter+ " of 2289 WEM files converted.");
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Environment.NewLine + "---" + Environment.NewLine + "Starting conversion of soundbanks to WEM.");
            counter = 0;

            foreach (var file in Directory.GetFiles(directories[0], "*.BNK", SearchOption.AllDirectories))
            {
                string workingFile = file.Split(new[] { "SOUND\\" }, StringSplitOptions.None)[1];
                
                File.Copy(file, directories[2] + "\\" + Path.GetFileName(workingFile));

                Process bnkextr = new Process();
                bnkextr.StartInfo.FileName = "bnkextr.exe";
                bnkextr.StartInfo.Arguments = Path.GetFileName(workingFile);
                bnkextr.StartInfo.WorkingDirectory = directories[2];
                bnkextr.Start();
                bnkextr.WaitForExit();

                File.Delete(directories[2] + "\\" + Path.GetFileName(workingFile));

                counter++;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\r{0}   ", counter + " of 483 BNK files converted.");
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Environment.NewLine + "---" + Environment.NewLine + "Starting conversion of remaining sound files to OGG.");
            counter = 0;

            foreach (var file in Directory.GetFiles(directories[2], "*.wem", SearchOption.AllDirectories))
            {
                if (File.Exists(file.Substring(0,file.Length-3)+"ogg"))
                {
                    File.Delete(file);
                }
                else
                {
                    Process ww2ogg = new Process();
                    ww2ogg.StartInfo.FileName = "ww2ogg.exe";
                    ww2ogg.StartInfo.Arguments = "\"" + file.Split(new[] { "SOUND_CONVERTED\\" }, StringSplitOptions.None)[1] + "\" --pcb packed_codebooks_aoTuV_603.bin";
                    ww2ogg.StartInfo.WorkingDirectory = directories[2];
                    ww2ogg.Start();
                    ww2ogg.WaitForExit();

                    Process revorb = new Process();
                    revorb.StartInfo.FileName = "revorb.exe";
                    revorb.StartInfo.Arguments = file.Split(new[] { "SOUND_CONVERTED\\" }, StringSplitOptions.None)[1].Substring(0, file.Split(new[] { "SOUND_CONVERTED\\" }, StringSplitOptions.None)[1].Length - 3) + "ogg";
                    revorb.StartInfo.WorkingDirectory = directories[2];
                    revorb.Start();
                    revorb.WaitForExit();

                    File.Delete(file);

                    counter++;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("\r{0}   ", counter + " of 6561 BNK WEM files converted.");
                }
            }
            
            foreach (var file in Directory.GetFiles(directories[2], "*.exe", SearchOption.AllDirectories))
            {
                File.Delete(file);
            }
            File.Delete(directories[2] + "\\base_library.zip");
            File.Delete(directories[2] + "\\python36.dll");
            File.Delete(directories[2] + "\\packed_codebooks_aoTuV_603.bin");

            foreach (var directory in Directory.GetDirectories(directories[2]))
            {
                if (Directory.GetFiles(directory).Length == 0 &&
                    Directory.GetDirectories(directory).Length == 0)
                {
                    Directory.Delete(directory, false);
                }
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Environment.NewLine + "---" + Environment.NewLine+"Starting file rename process.");

            //Define our error log path, and then delete and re-make the file to clear it
            string errorLogPath = "AlienIsolationAudioParser_Errors.txt";
            if (File.Exists(errorLogPath))
            {
                File.Delete(errorLogPath);
            }

            //Start off error log
            using (StreamWriter sw = File.AppendText(errorLogPath))
            {
                sw.WriteLine("AUDIO EXPORT ERROR LOG");
                sw.WriteLine("----------------------------------------");
            }

            //Run through each file...
            foreach (var file in fileArray)
            {
                //New thing - check if we're copying as WEM or ogg.
                bool isWEM = false; //Only set true if is WEM. Last resort!

                try
                {
                    //Pre-define our final FILE PATH string to use for copying...
                    string finalOriginalFilePath = "";

                    //Grab needed attributes from current file
                    string originalFileNameID = file.Attribute("Id").Value.ToString();
                    string originalFileNameLanguage = file.Attribute("Language").Value.ToString();

                    //Get the ShortName value of current file
                    var originalFileNameShortNameUnparsed = file.Descendants("ShortName"); //Hmm... got to trim 4 chars from the end to use this to find a file.

                    //Parse ShortName of current file
                    string originalFileNameShortName = "";
                    foreach (var shortName in originalFileNameShortNameUnparsed)
                    {
                        originalFileNameShortName = shortName.Value.ToString();
                    }

                    //Check we've got a ShortName - if we don't it's not something we can export. Add to log with different notation.
                    if (originalFileNameShortName != "")
                    {
                        //Try and grab original file path using the FileID
                        var originalFile = Directory.GetFiles(directories[2], originalFileNameID + ".ogg", SearchOption.AllDirectories);

                        //Parse our file path from the ID search
                        string originalFilePathFromID = "";
                        foreach (string filePath in originalFile)
                        {
                            originalFilePathFromID = filePath;
                            finalOriginalFilePath = filePath;
                        }

                        //If we didn't find any files from the ID search, try again using the ShortName (with the .wav extension cut off).
                        //This is most likely going to be for language-specific audio files, so we're gonna search in the specific language folders. Might need to change this later.
                        if (originalFilePathFromID == "")
                        {
                            //ShortName without extension pls
                            string shortNameWithoutExtension = originalFileNameShortName.Substring(0, originalFileNameShortName.Count() - 4);

                            //Define this here for later
                            string originalFilePathFromShortName = "";

                            //If we're trying to find SFX here something's gone wrong. Only search (also with dialogue variant) if we're not using SFX as language.
                            if (originalFileNameLanguage != "SFX")
                            {
                                //Try and grab original file path using the ShortName (without .wav)
                                var originalFileAttemptTwo = Directory.GetFiles(directories[2] + @"\" + originalFileNameLanguage.ToUpper(), shortNameWithoutExtension + ".ogg", SearchOption.AllDirectories);

                                //Parse our file path from the ShortName search
                                foreach (string filePath in originalFileAttemptTwo)
                                {
                                    originalFilePathFromShortName = filePath;
                                    finalOriginalFilePath = filePath;
                                }

                                //Still not got anything. Gonna try the dialogue variant
                                if (finalOriginalFilePath == "")
                                {
                                    //Try and grab original file path using the ShortName (without .wav)
                                    var originalFileAttemptThree = Directory.GetFiles(directories[2] + @"\" + originalFileNameLanguage.ToUpper() + "_DIALOGUE-PCK", shortNameWithoutExtension + ".ogg", SearchOption.AllDirectories);

                                    //Parse our file path from the ShortName search 2
                                    foreach (string filePath in originalFileAttemptThree)
                                    {
                                        originalFilePathFromShortName = filePath;
                                        finalOriginalFilePath = filePath;
                                    }
                                }
                            }
                        }

                        //Quick extra bit... there are some files that don't want to convert to ogg, so I'm gonna go ahead and add in something special.
                        //Will still do the copy/paste stuff, but will keep it as a WEM format (not playable). Not great, but fine for now.

                        //Only do if we haven't found anything yet - a last resort
                        if (finalOriginalFilePath == "")
                        {
                            //Try and grab original file path using the FileID
                            var originalFileWEM = Directory.GetFiles(directories[0], originalFileNameID + ".WEM", SearchOption.AllDirectories); //Caps

                            //Parse our file path from the WEM search
                            foreach (string filePath in originalFileWEM)
                            {
                                finalOriginalFilePath = filePath;
                                isWEM = true;
                                //If we haven't found it here, I don't think there's any hope. It must be taken out.
                            }
                        }

                        //If we've found the original file, copy it to the right place.
                        if (finalOriginalFilePath != "")
                        {
                            //Apply a fix for the voice folder issue
                            if (originalFileNameLanguage == "English(US)" |
                                originalFileNameLanguage == "French(France)" |
                                originalFileNameLanguage == "German" |
                                originalFileNameLanguage == "Italian" |
                                originalFileNameLanguage == "Portuguese(Brazil)" |
                                originalFileNameLanguage == "Russian" |
                                originalFileNameLanguage == "Spanish(Spain)")
                            {
                                originalFileNameLanguage = @"Voices\" + originalFileNameLanguage; //Just add to originalFileNameLanguage.
                            }

                            //Generate our FULL shortname with the correct extension!
                            string fileExtension = "ogg";
                            if (isWEM == true)
                            {
                                fileExtension = "WEM"; //caps
                            }
                            string fileShortNameFULL = directories[1] + "\\" + originalFileNameLanguage + "\\" + originalFileNameShortName.Substring(0, originalFileNameShortName.Length - 3) + fileExtension;

                            //Generate our FileInfo vars for copying and modifying in general
                            FileInfo originalFileInfo = new FileInfo(finalOriginalFilePath);
                            FileInfo newFileInfo = new FileInfo(fileShortNameFULL);

                            //Make sure the converted file doesn't already exist, if it does we want to delete it before continuing.
                            if (File.Exists(fileShortNameFULL))
                            {
                                //CHANGED: Don't delete anymore, just skip over it. Will save on export time.
                                //newFileInfo.Delete();
                            }
                            else
                            {
                                //Create directory if it doesn't exist ready to copy to
                                Directory.CreateDirectory(Path.GetDirectoryName(fileShortNameFULL));

                                //Copy our file to the correct directory layout and name
                                originalFileInfo.CopyTo(fileShortNameFULL);
                            }

                            Console.ForegroundColor = ConsoleColor.Green;
                            if (isWEM)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                            }
                            Console.WriteLine("EXPORTED: " + fileShortNameFULL);

                            //NEW: Delete original file. We can then gather up any straglers that weren't in the XML.
                            //File.Delete(finalOriginalFilePath);
                        }
                        else
                        {
                            //Didn't find the original file. Write to error log.
                            using (StreamWriter sw = File.AppendText(errorLogPath))
                            {
                                sw.WriteLine("REFERENCED SOUND FILE COULD NOT BE FOUND");
                                sw.WriteLine("Time Occured: " + DateTime.Now.ToString("h:mm:ss tt"));
                                sw.WriteLine("Audio ID: " + originalFileNameID);
                                sw.WriteLine("Audio ShortName: " + originalFileNameShortName);
                                sw.WriteLine("----------------------------------------");
                            }

                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("WARNING: " + directories[1] + "\\" + originalFileNameLanguage + "\\" + originalFileNameShortName.Substring(0, originalFileNameShortName.Length - 3));
                        }
                    }
                    else
                    {
                        using (StreamWriter sw = File.AppendText(errorLogPath))
                        {
                            /*
                            sw.WriteLine("REFERENCED SOUND FILE DID NOT HAVE SHORTNAME");
                            sw.WriteLine("Time Occured: " + DateTime.Now.ToString("h:mm:ss tt"));
                            sw.WriteLine("Audio ID: " + originalFileNameID);
                            sw.WriteLine("----------------------------------------");
                            */
                        }
                    }
                }
                catch (Exception ex)
                {
                    //Unknown exception
                    using (StreamWriter sw = File.AppendText(errorLogPath))
                    {
                        //Grab ID
                        string originalFileNameID = file.Attribute("Id").Value.ToString();
                        /*
                        sw.WriteLine("ENCOUNTERED AN EXCEPTION WHILE PROCESSING");
                        sw.WriteLine("Time Occured: " + DateTime.Now.ToString("h:mm:ss tt"));
                        sw.WriteLine("Audio ID: " + originalFileNameID);
                        sw.WriteLine("Error Info: " + ex.Message);
                        sw.WriteLine("----------------------------------------");
                        */
                    }
                }
            }

            //Count up remaining files.
            /*
            string[] files = Directory.GetFiles(directories[1], "*.ogg", SearchOption.AllDirectories);
            using (StreamWriter sw = File.AppendText(errorLogPath))
            {
                sw.WriteLine("TOTAL OGG SOUND FILES NOT FOUND IN XML = " + files.Length);
                sw.WriteLine("THESE FILES CAN BE FOUND IN THE RAW DIRECTORY.");
                sw.WriteLine("----------------------------------------");
            }
            */

            //Done! Open error log and close program.
            //Process.Start(errorLogPath);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("FINISHED");
            Console.Read();
        }
    }
}
