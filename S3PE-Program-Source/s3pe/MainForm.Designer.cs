﻿namespace S3PIDemoFE
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.browserWidget1 = new S3PIDemoFE.BrowserWidget();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lbProgress = new System.Windows.Forms.Label();
            this.pnAuto = new System.Windows.Forms.Panel();
            this.packageInfoWidget1 = new S3PIDemoFE.PackageInfo.PackageInfoWidget();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.controlPanel1 = new S3PIDemoFE.ControlPanel();
            this.resourceFilterWidget1 = new S3PIDemoFE.Filter.ResourceFilterWidget();
            this.saveAsFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.exportFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.exportBatchTarget = new System.Windows.Forms.FolderBrowserDialog();
            this.importResourcesDialog = new System.Windows.Forms.OpenFileDialog();
            this.importPackagesDialog = new System.Windows.Forms.OpenFileDialog();
            this.exportToPackageDialog = new System.Windows.Forms.OpenFileDialog();
            this.menuBarWidget1 = new S3PIDemoFE.MenuBarWidget();
            this.packageInfoFields1 = new S3PIDemoFE.PackageInfo.PackageInfoFields();
            this.replaceResourceDialog = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 23);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.packageInfoWidget1);
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Panel2MinSize = 131;
            this.splitContainer1.Size = new System.Drawing.Size(923, 654);
            this.splitContainer1.SplitterDistance = 514;
            this.splitContainer1.TabIndex = 1;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.browserWidget1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.pnAuto);
            this.splitContainer2.Size = new System.Drawing.Size(923, 514);
            this.splitContainer2.SplitterDistance = 629;
            this.splitContainer2.TabIndex = 0;
            // 
            // browserWidget1
            // 
            this.browserWidget1.AllowDrop = true;
            this.browserWidget1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.browserWidget1.Fields = null;
            this.browserWidget1.Filter = null;
            this.browserWidget1.Location = new System.Drawing.Point(0, 0);
            this.browserWidget1.Name = "browserWidget1";
            this.browserWidget1.Package = null;
            this.browserWidget1.ProgressBar = this.progressBar1;
            this.browserWidget1.ProgressLabel = this.lbProgress;
            this.browserWidget1.ResourceKey = null;
            this.browserWidget1.SelectedResource = null;
            this.browserWidget1.Size = new System.Drawing.Size(625, 510);
            this.browserWidget1.Sortable = false;
            this.browserWidget1.TabIndex = 0;
            this.browserWidget1.ItemActivate += new System.EventHandler(this.browserWidget1_ItemActivate);
            this.browserWidget1.SelectedResourceChanging += new System.EventHandler<S3PIDemoFE.BrowserWidget.ResourceChangingEventArgs>(this.browserWidget1_SelectedResourceChanging);
            this.browserWidget1.SelectedResourceChanged += new System.EventHandler<S3PIDemoFE.BrowserWidget.ResourceChangedEventArgs>(this.browserWidget1_SelectedResourceChanged);
            this.browserWidget1.DeletePressed += new System.EventHandler(this.browserWidget1_DeletePressed);
            this.browserWidget1.DragDrop += new System.Windows.Forms.DragEventHandler(this.browserWidget1_DragDrop);
            this.browserWidget1.DragOver += new System.Windows.Forms.DragEventHandler(this.browserWidget1_DragOver);
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar1.Location = new System.Drawing.Point(68, 0);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(0);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(851, 27);
            this.progressBar1.TabIndex = 2;
            // 
            // lbProgress
            // 
            this.lbProgress.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbProgress.AutoSize = true;
            this.lbProgress.Location = new System.Drawing.Point(0, 7);
            this.lbProgress.Margin = new System.Windows.Forms.Padding(0);
            this.lbProgress.Name = "lbProgress";
            this.lbProgress.Size = new System.Drawing.Size(68, 13);
            this.lbProgress.TabIndex = 3;
            this.lbProgress.Text = "Progress text";
            // 
            // pnAuto
            // 
            this.pnAuto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnAuto.Location = new System.Drawing.Point(0, 0);
            this.pnAuto.Margin = new System.Windows.Forms.Padding(0);
            this.pnAuto.Name = "pnAuto";
            this.pnAuto.Size = new System.Drawing.Size(286, 510);
            this.pnAuto.TabIndex = 4;
            // 
            // packageInfoWidget1
            // 
            this.packageInfoWidget1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.packageInfoWidget1.Fields = null;
            this.packageInfoWidget1.Location = new System.Drawing.Point(0, 103);
            this.packageInfoWidget1.Name = "packageInfoWidget1";
            this.packageInfoWidget1.Package = null;
            this.packageInfoWidget1.Size = new System.Drawing.Size(919, 2);
            this.packageInfoWidget1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.progressBar1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbProgress, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 105);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(919, 27);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.controlPanel1);
            this.panel1.Controls.Add(this.resourceFilterWidget1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(919, 103);
            this.panel1.TabIndex = 0;
            // 
            // controlPanel1
            // 
            this.controlPanel1.AutoOff = false;
            this.controlPanel1.AutoPreview = true;
            this.controlPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.controlPanel1.Location = new System.Drawing.Point(0, 74);
            this.controlPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.controlPanel1.Name = "controlPanel1";
            this.controlPanel1.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.controlPanel1.Size = new System.Drawing.Size(919, 29);
            this.controlPanel1.Sort = true;
            this.controlPanel1.TabIndex = 1;
            this.controlPanel1.UseNames = true;
            this.controlPanel1.UseTags = true;
            this.controlPanel1.SortChanged += new System.EventHandler(this.controlPanel1_SortChanged);
            this.controlPanel1.HexClick += new System.EventHandler(this.controlPanel1_HexClick);
            this.controlPanel1.AutoChanged += new System.EventHandler(this.controlPanel1_AutoChanged);
            this.controlPanel1.HexOnlyChanged += new System.EventHandler(this.controlPanel1_HexOnlyChanged);
            this.controlPanel1.ValueClick += new System.EventHandler(this.controlPanel1_PreviewClick);
            this.controlPanel1.GridClick += new System.EventHandler(this.controlPanel1_GridClick);
            this.controlPanel1.UseNamesChanged += new System.EventHandler(this.controlPanel1_UseNamesChanged);
            this.controlPanel1.UseTagsChanged += new System.EventHandler(this.controlPanel1_UseTagsChanged);
            this.controlPanel1.Helper1Click += new System.EventHandler(this.controlPanel1_Helper1Click);
            this.controlPanel1.Helper2Click += new System.EventHandler(this.controlPanel1_Helper2Click);
            this.controlPanel1.HexEditClick += new System.EventHandler(this.controlPanel1_HexEditClick);
            this.controlPanel1.CommitClick += new System.EventHandler(this.controlPanel1_CommitClick);
            // 
            // resourceFilterWidget1
            // 
            this.resourceFilterWidget1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resourceFilterWidget1.Location = new System.Drawing.Point(0, 0);
            this.resourceFilterWidget1.Name = "resourceFilterWidget1";
            this.resourceFilterWidget1.PasteButtonEnabled = true;
            this.resourceFilterWidget1.Size = new System.Drawing.Size(919, 103);
            this.resourceFilterWidget1.TabIndex = 0;
            this.resourceFilterWidget1.FilterChanged += new System.EventHandler(this.resourceFilterWidget1_FilterChanged);
            this.resourceFilterWidget1.PasteClicked += new System.EventHandler(this.resourceFilterWidget1_PasteClicked);
            // 
            // saveAsFileDialog
            // 
            this.saveAsFileDialog.FileName = "*.package";
            this.saveAsFileDialog.Filter = global::S3PIDemoFE.Properties.Settings.Default.DBPFFilesAndAll;
            this.saveAsFileDialog.Title = "Save As";
            // 
            // exportFileDialog
            // 
            this.exportFileDialog.AddExtension = false;
            this.exportFileDialog.Filter = "Exported files (S3_*.*)|S3_*.*|All files (*.*)|*.*";
            this.exportFileDialog.Title = "Export File";
            // 
            // exportBatchTarget
            // 
            this.exportBatchTarget.Description = "Choose the folder to receive the exported resources";
            // 
            // importResourcesDialog
            // 
            this.importResourcesDialog.Filter = "Exported files (S3_*.*)|S3_*.*|All files (*.*)|*.*";
            this.importResourcesDialog.Multiselect = true;
            this.importResourcesDialog.Title = "Import Resources";
            // 
            // importPackagesDialog
            // 
            this.importPackagesDialog.Filter = global::S3PIDemoFE.Properties.Settings.Default.DBPFFilesAndAll;
            this.importPackagesDialog.Multiselect = true;
            this.importPackagesDialog.Title = "Import Packages";
            // 
            // exportToPackageDialog
            // 
            this.exportToPackageDialog.CheckFileExists = false;
            this.exportToPackageDialog.DefaultExt = "package";
            this.exportToPackageDialog.Filter = global::S3PIDemoFE.Properties.Settings.Default.DBPFFilesAndAll;
            this.exportToPackageDialog.Title = "Export to package";
            // 
            // menuBarWidget1
            // 
            this.menuBarWidget1.Dock = System.Windows.Forms.DockStyle.Top;
            this.menuBarWidget1.Location = new System.Drawing.Point(0, 0);
            this.menuBarWidget1.Margin = new System.Windows.Forms.Padding(0);
            this.menuBarWidget1.Name = "menuBarWidget1";
            this.menuBarWidget1.Size = new System.Drawing.Size(923, 23);
            this.menuBarWidget1.TabIndex = 0;
            this.menuBarWidget1.MBDropDownOpening += new S3PIDemoFE.MenuBarWidget.MBDropDownOpeningEventHandler(this.menuBarWidget1_MBDropDownOpening);
            this.menuBarWidget1.MBFile_Click += new S3PIDemoFE.MenuBarWidget.MBClickEventHandler(this.menuBarWidget1_MBFile_Click);
            this.menuBarWidget1.MBEdit_Click += new S3PIDemoFE.MenuBarWidget.MBClickEventHandler(this.menuBarWidget1_MBEdit_Click);
            this.menuBarWidget1.MBResource_Click += new S3PIDemoFE.MenuBarWidget.MBClickEventHandler(this.menuBarWidget1_MBResource_Click);
            this.menuBarWidget1.MBTools_Click += new S3PIDemoFE.MenuBarWidget.MBClickEventHandler(this.menuBarWidget1_MBTools_Click);
            this.menuBarWidget1.MBSettings_Click += new S3PIDemoFE.MenuBarWidget.MBClickEventHandler(this.menuBarWidget1_MBSettings_Click);
            this.menuBarWidget1.MBHelp_Click += new S3PIDemoFE.MenuBarWidget.MBClickEventHandler(this.menuBarWidget1_MBHelp_Click);
            this.menuBarWidget1.HelperClick += new S3PIDemoFE.MenuBarWidget.HelperClickEventHandler(this.menuBarWidget1_HelperClick);
            this.menuBarWidget1.MRUClick += new S3PIDemoFE.MenuBarWidget.MRUClickEventHandler(this.menuBarWidget1_MRUClick);
            this.menuBarWidget1.BookmarkClick += new S3PIDemoFE.MenuBarWidget.BookmarkClickEventHandler(this.menuBarWidget1_BookmarkClick);
            // 
            // replaceResourceDialog
            // 
            this.replaceResourceDialog.AddExtension = false;
            this.replaceResourceDialog.Title = "Replace Resource";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "*.package";
            this.openFileDialog1.Filter = global::S3PIDemoFE.Properties.Settings.Default.DBPFFilesAndAll;
            this.openFileDialog1.ShowReadOnly = true;
            this.openFileDialog1.SupportMultiDottedExtensions = true;
            this.openFileDialog1.Title = "Open package";
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(923, 677);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuBarWidget1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "s3pe";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveAsFileDialog;
        private System.Windows.Forms.SaveFileDialog exportFileDialog;
        private MenuBarWidget menuBarWidget1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private BrowserWidget browserWidget1;
        private PackageInfo.PackageInfoWidget packageInfoWidget1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private PackageInfo.PackageInfoFields packageInfoFields1;
        private System.Windows.Forms.Panel panel1;
        private Filter.ResourceFilterWidget resourceFilterWidget1;
        private ControlPanel controlPanel1;
        private System.Windows.Forms.FolderBrowserDialog exportBatchTarget;
        private System.Windows.Forms.OpenFileDialog importResourcesDialog;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lbProgress;
        private System.Windows.Forms.OpenFileDialog importPackagesDialog;
        private System.Windows.Forms.OpenFileDialog exportToPackageDialog;
        private System.Windows.Forms.Panel pnAuto;
        private System.Windows.Forms.OpenFileDialog replaceResourceDialog;
    }
}