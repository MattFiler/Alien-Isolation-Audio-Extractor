using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AudioImportExportGUI
{
    class SoundbankLookup
    {
        XDocument soundbankXML = XDocument.Parse(Properties.Resources.SOUNDBANKSINFO);

        public string GetFileName(string id)
        {
            var fileArray = soundbankXML.Descendants("File");
            foreach (var thisFile in fileArray)
            {
                if (thisFile.Attribute("Id").Value == id && thisFile.Descendants("ShortName") != null)
                {
                    string shortName = thisFile.Descendants("ShortName").ElementAt(0).Value;
                    string path = thisFile.Descendants("Path").ElementAt(0).Value;

                    if (shortName.Contains("\\"))
                    {
                        return thisFile.Attribute("Language").Value + "\\" + shortName;
                    }

                    List<string> pathParts = new List<string>(path.Split('\\'));
                    string compiledPath = "";
                    for (int i = 0; i < pathParts.Count; i++)
                    {
                        if (i == pathParts.Count - 1)
                        {
                            return compiledPath + shortName;
                        }
                        if (i < 2 && thisFile.Attribute("Language").Value != "SFX")
                        {
                            if (compiledPath == "") compiledPath = thisFile.Attribute("Language").Value + "\\";
                            continue;
                        }
                        compiledPath += pathParts[i] + "\\";
                    }
                }
            }
            return null;
        }

        public string GetEventName(string id)
        {
            var eventArray = soundbankXML.Descendants("Event");
            foreach (var thisEvent in eventArray)
            {
                if (thisEvent.Attribute("Id").Value == id && thisEvent.Attribute("Name") != null)
                {
                    return thisEvent.Attribute("Name").Value;
                }
            }
            return null;
        }
    }
}
