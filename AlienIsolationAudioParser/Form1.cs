using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Xml;
using System.Xml.Linq;

namespace AlienIsolationAudioParser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Load XML
            XDocument soundbankXML = XDocument.Load("SOUNDBANKSINFO.XML");

            //Grab file array from XML
            var fileArray = soundbankXML.Descendants("File");

            //Define our error log path, and then delete and re-make the file to clear it
            string errorLogPath = "errors.txt";
            if (File.Exists(errorLogPath)) {
                File.Delete(errorLogPath);
            }
            using (File.Create(errorLogPath));

            //Pre-define our path 
            string fileDirectory = @"D:\Documents\Isolation Mod Tools\ALL SOUNDS"; //Might need to change sometimes

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
                        var originalFile = Directory.GetFiles(fileDirectory, originalFileNameID + ".ogg", SearchOption.AllDirectories);

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
                                var originalFileAttemptTwo = Directory.GetFiles(fileDirectory + @"\" + originalFileNameLanguage.ToUpper(), shortNameWithoutExtension + ".ogg", SearchOption.AllDirectories);

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
                                    var originalFileAttemptThree = Directory.GetFiles(fileDirectory + @"\" + originalFileNameLanguage.ToUpper() + "_DIALOGUE-PCK", shortNameWithoutExtension + ".ogg", SearchOption.AllDirectories);

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

                        //Get WEM files location
                        string pathToWEM = @"D:\Documents\Isolation Mod Tools\Alien Isolation PC Orig\DATA\SOUND"; //might need to change sometimes

                        //Only do if we haven't found anything yet - a last resort
                        if (finalOriginalFilePath == "")
                        {
                            //Try and grab original file path using the FileID
                            var originalFileWEM = Directory.GetFiles(pathToWEM, originalFileNameID + ".WEM", SearchOption.AllDirectories); //Caps

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
                            string fileShortNameFULL = originalFileNameLanguage + @"\" + originalFileNameShortName.Substring(0, originalFileNameShortName.Length - 3) + fileExtension;

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
                        }
                    }
                    else
                    {
                        using (StreamWriter sw = File.AppendText(errorLogPath))
                        {
                            sw.WriteLine("REFERENCED SOUND FILE DID NOT HAVE SHORTNAME");
                            sw.WriteLine("Time Occured: " + DateTime.Now.ToString("h:mm:ss tt"));
                            sw.WriteLine("Audio ID: " + originalFileNameID);
                            sw.WriteLine("----------------------------------------");
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

                        sw.WriteLine("ENCOUNTERED AN EXCEPTION WHILE PROCESSING");
                        sw.WriteLine("Time Occured: " + DateTime.Now.ToString("h:mm:ss tt"));
                        sw.WriteLine("Audio ID: " + originalFileNameID);
                        sw.WriteLine("Error Info: " + ex.Message);
                        sw.WriteLine("----------------------------------------");
                    }
                }
            }

            //Done! Open error log and close program.
            Process.Start(errorLogPath);
        }
    }
}
