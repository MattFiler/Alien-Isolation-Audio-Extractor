using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioImportExportGUI
{
    public partial class Explorer : Form
    {
        public Explorer()
        {
            InitializeComponent();
        }

        SoundbankLookup soundbank = new SoundbankLookup();
        private void debugtestbtn_Click(object sender, EventArgs e)
        {
            debugtestout.Text = soundbank.GetFileName(debugtest.Text);
        }

        private void debugtestbtn2_Click(object sender, EventArgs e)
        {
            debugtestout2.Text = soundbank.GetEventName(debugtest2.Text);
        }

        /* Let the user select a PCK file */
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ArchivePicker = new OpenFileDialog();
            ArchivePicker.Filter = "Wwise PCK|*.PCK";
            if (ArchivePicker.ShowDialog() == DialogResult.OK)
            {
                OpenFileAndPopulateGUI(ArchivePicker.FileName);
            }
        }

        /* Open a PCK and populate the GUI */
        private void OpenFileAndPopulateGUI(string filename)
        {
            Cursor.Current = Cursors.WaitCursor;

            BinaryReader thisPCK = new BinaryReader(File.OpenRead(filename));
            thisPCK.BaseStream.Position += 4;
            int startOfFiles = thisPCK.ReadInt32();
            thisPCK.BaseStream.Position += 4;
            int numberOfFolders = thisPCK.ReadInt32();
            //thisPCK.BaseStream.Position = 132; - incorrect
            int fileCount = thisPCK.ReadInt32();
            List<string> fileNames = new List<string>();
            for (int i = 0; i < fileCount; i++)
            {
                int thisFileID = thisPCK.ReadInt32();
                thisPCK.BaseStream.Position += 16;
                string thisFileName = soundbank.GetFileName(thisFileID.ToString());
                if (thisFileName == null) thisFileName = thisFileID.ToString() + ".wav";
                fileNames.Add(thisFileName);
            }
            thisPCK.Close();

            UpdateFileTree(fileNames);
            
            Cursor.Current = Cursors.Default;
        }

        /* Update the file tree GUI */
        private void UpdateFileTree(List<string> FilesToList)
        {
            FileTree.Nodes.Clear();
            foreach (string FileName in FilesToList)
            {
                string[] FileNameParts = FileName.Split('/');
                if (FileNameParts.Length == 1) { FileNameParts = FileName.Split('\\'); }
                AddFileToTree(FileNameParts, 0, FileTree.Nodes);
            }
            //UpdateSelectedFilePreview();
            FileTree.Sort();
        }
        /* Add a file to the GUI tree structure */
        private void AddFileToTree(string[] FileNameParts, int index, TreeNodeCollection LoopedNodeCollection)
        {
            if (FileNameParts.Length <= index)
            {
                return;
            }

            bool should = true;
            foreach (TreeNode ThisFileNode in LoopedNodeCollection)
            {
                if (ThisFileNode.Text == FileNameParts[index])
                {
                    should = false;
                    AddFileToTree(FileNameParts, index + 1, ThisFileNode.Nodes);
                    break;
                }
            }
            if (should)
            {
                TreeNode FileNode = new TreeNode(FileNameParts[index]);
                /*
                TreeItem ThisTag = new TreeItem();
                if (FileNameParts.Length - 1 == index)
                {
                    //Node is a file
                    for (int i = 0; i < FileNameParts.Length; i++)
                    {
                        ThisTag.String_Value += FileNameParts[i] + "/";
                    }
                    ThisTag.String_Value = ThisTag.String_Value.ToString().Substring(0, ThisTag.String_Value.ToString().Length - 1);

                    ThisTag.Item_Type = TreeItemType.EXPORTABLE_FILE;
                    FileNode.ImageIndex = (int)TreeItemIcon.FILE;
                    FileNode.SelectedImageIndex = (int)TreeItemIcon.FILE;
                    FileNode.ContextMenuStrip = fileContextMenu;
                }
                else
                {
                    //Node is a directory
                    ThisTag.Item_Type = TreeItemType.DIRECTORY;
                    FileNode.ImageIndex = (int)TreeItemIcon.FOLDER;
                    FileNode.SelectedImageIndex = (int)TreeItemIcon.FOLDER;
                    AddFileToTree(FileNameParts, index + 1, FileNode.Nodes);
                }

                FileNode.Tag = ThisTag;
                */
                LoopedNodeCollection.Add(FileNode);
            }
        }
    }
}
