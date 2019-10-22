namespace AudioImportExportGUI
{
    partial class Explorer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.FileTree = new System.Windows.Forms.TreeView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.importFile = new System.Windows.Forms.Button();
            this.exportFile = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.fileTypeInfo = new System.Windows.Forms.Label();
            this.fileSizeInfo = new System.Windows.Forms.Label();
            this.fileNameInfo = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.importFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exportAllFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expandAllDirectoriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shrinkAllDirectoriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugtestbtn = new System.Windows.Forms.Button();
            this.debugtest = new System.Windows.Forms.TextBox();
            this.debugtestout = new System.Windows.Forms.TextBox();
            this.debugtestout2 = new System.Windows.Forms.TextBox();
            this.debugtest2 = new System.Windows.Forms.TextBox();
            this.debugtestbtn2 = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // FileTree
            // 
            this.FileTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.FileTree.Location = new System.Drawing.Point(1, 23);
            this.FileTree.Name = "FileTree";
            this.FileTree.Size = new System.Drawing.Size(500, 679);
            this.FileTree.TabIndex = 6;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.importFile);
            this.groupBox3.Controls.Add(this.exportFile);
            this.groupBox3.Location = new System.Drawing.Point(507, 138);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(277, 78);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "File Utilities";
            // 
            // importFile
            // 
            this.importFile.Enabled = false;
            this.importFile.Location = new System.Drawing.Point(6, 48);
            this.importFile.Name = "importFile";
            this.importFile.Size = new System.Drawing.Size(265, 24);
            this.importFile.TabIndex = 1;
            this.importFile.Text = "Replace Selected";
            this.importFile.UseVisualStyleBackColor = true;
            // 
            // exportFile
            // 
            this.exportFile.Enabled = false;
            this.exportFile.Location = new System.Drawing.Point(6, 21);
            this.exportFile.Name = "exportFile";
            this.exportFile.Size = new System.Drawing.Size(265, 24);
            this.exportFile.TabIndex = 0;
            this.exportFile.Text = "Export Selected";
            this.exportFile.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.fileTypeInfo);
            this.groupBox2.Controls.Add(this.fileSizeInfo);
            this.groupBox2.Controls.Add(this.fileNameInfo);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(507, 36);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(277, 96);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "File Info";
            // 
            // fileTypeInfo
            // 
            this.fileTypeInfo.AutoSize = true;
            this.fileTypeInfo.Location = new System.Drawing.Point(55, 61);
            this.fileTypeInfo.Name = "fileTypeInfo";
            this.fileTypeInfo.Size = new System.Drawing.Size(0, 13);
            this.fileTypeInfo.TabIndex = 15;
            // 
            // fileSizeInfo
            // 
            this.fileSizeInfo.AutoSize = true;
            this.fileSizeInfo.Location = new System.Drawing.Point(55, 43);
            this.fileSizeInfo.Name = "fileSizeInfo";
            this.fileSizeInfo.Size = new System.Drawing.Size(0, 13);
            this.fileSizeInfo.TabIndex = 14;
            // 
            // fileNameInfo
            // 
            this.fileNameInfo.AutoSize = true;
            this.fileNameInfo.Location = new System.Drawing.Point(55, 26);
            this.fileNameInfo.Name = "fileNameInfo";
            this.fileNameInfo.Size = new System.Drawing.Size(0, 13);
            this.fileNameInfo.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(15, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Type:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(19, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Size:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(11, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Name:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(789, 24);
            this.menuStrip1.TabIndex = 14;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripSeparator2,
            this.importFileToolStripMenuItem,
            this.exportFileToolStripMenuItem,
            this.toolStripSeparator1,
            this.exportAllFilesToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem.Text = "Open PCK";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(175, 6);
            // 
            // importFileToolStripMenuItem
            // 
            this.importFileToolStripMenuItem.Name = "importFileToolStripMenuItem";
            this.importFileToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.importFileToolStripMenuItem.Text = "Import Selected File";
            // 
            // exportFileToolStripMenuItem
            // 
            this.exportFileToolStripMenuItem.Name = "exportFileToolStripMenuItem";
            this.exportFileToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.exportFileToolStripMenuItem.Text = "Export Selected File";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(175, 6);
            // 
            // exportAllFilesToolStripMenuItem
            // 
            this.exportAllFilesToolStripMenuItem.Name = "exportAllFilesToolStripMenuItem";
            this.exportAllFilesToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.exportAllFilesToolStripMenuItem.Text = "Export All Files";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.expandAllDirectoriesToolStripMenuItem,
            this.shrinkAllDirectoriesToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // expandAllDirectoriesToolStripMenuItem
            // 
            this.expandAllDirectoriesToolStripMenuItem.Name = "expandAllDirectoriesToolStripMenuItem";
            this.expandAllDirectoriesToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.expandAllDirectoriesToolStripMenuItem.Text = "Expand All Directories";
            // 
            // shrinkAllDirectoriesToolStripMenuItem
            // 
            this.shrinkAllDirectoriesToolStripMenuItem.Name = "shrinkAllDirectoriesToolStripMenuItem";
            this.shrinkAllDirectoriesToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.shrinkAllDirectoriesToolStripMenuItem.Text = "Shrink All Directories";
            // 
            // debugtestbtn
            // 
            this.debugtestbtn.Location = new System.Drawing.Point(613, 429);
            this.debugtestbtn.Name = "debugtestbtn";
            this.debugtestbtn.Size = new System.Drawing.Size(75, 23);
            this.debugtestbtn.TabIndex = 15;
            this.debugtestbtn.Text = "button1";
            this.debugtestbtn.UseVisualStyleBackColor = true;
            this.debugtestbtn.Click += new System.EventHandler(this.debugtestbtn_Click);
            // 
            // debugtest
            // 
            this.debugtest.Location = new System.Drawing.Point(507, 431);
            this.debugtest.Name = "debugtest";
            this.debugtest.Size = new System.Drawing.Size(100, 20);
            this.debugtest.TabIndex = 16;
            // 
            // debugtestout
            // 
            this.debugtestout.Location = new System.Drawing.Point(507, 457);
            this.debugtestout.Name = "debugtestout";
            this.debugtestout.Size = new System.Drawing.Size(277, 20);
            this.debugtestout.TabIndex = 17;
            // 
            // debugtestout2
            // 
            this.debugtestout2.Location = new System.Drawing.Point(507, 538);
            this.debugtestout2.Name = "debugtestout2";
            this.debugtestout2.Size = new System.Drawing.Size(277, 20);
            this.debugtestout2.TabIndex = 20;
            // 
            // debugtest2
            // 
            this.debugtest2.Location = new System.Drawing.Point(507, 512);
            this.debugtest2.Name = "debugtest2";
            this.debugtest2.Size = new System.Drawing.Size(100, 20);
            this.debugtest2.TabIndex = 19;
            // 
            // debugtestbtn2
            // 
            this.debugtestbtn2.Location = new System.Drawing.Point(613, 510);
            this.debugtestbtn2.Name = "debugtestbtn2";
            this.debugtestbtn2.Size = new System.Drawing.Size(75, 23);
            this.debugtestbtn2.TabIndex = 18;
            this.debugtestbtn2.Text = "button1";
            this.debugtestbtn2.UseVisualStyleBackColor = true;
            this.debugtestbtn2.Click += new System.EventHandler(this.debugtestbtn2_Click);
            // 
            // Explorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 703);
            this.Controls.Add(this.debugtestout2);
            this.Controls.Add(this.debugtest2);
            this.Controls.Add(this.debugtestbtn2);
            this.Controls.Add(this.debugtestout);
            this.Controls.Add(this.debugtest);
            this.Controls.Add(this.debugtestbtn);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.FileTree);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Explorer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Alien: Isolation Audio Editor";
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView FileTree;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button importFile;
        private System.Windows.Forms.Button exportFile;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label fileTypeInfo;
        private System.Windows.Forms.Label fileSizeInfo;
        private System.Windows.Forms.Label fileNameInfo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem importFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exportAllFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandAllDirectoriesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shrinkAllDirectoriesToolStripMenuItem;
        private System.Windows.Forms.Button debugtestbtn;
        private System.Windows.Forms.TextBox debugtest;
        private System.Windows.Forms.TextBox debugtestout;
        private System.Windows.Forms.TextBox debugtestout2;
        private System.Windows.Forms.TextBox debugtest2;
        private System.Windows.Forms.Button debugtestbtn2;
    }
}

