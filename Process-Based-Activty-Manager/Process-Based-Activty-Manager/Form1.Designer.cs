﻿
namespace ActivityTracker
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.userMenu = new System.Windows.Forms.MenuStrip();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.optionsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.cleanDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.button2 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.listBoxActiveProcesses = new System.Windows.Forms.ListBox();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.textBoxAllProcesses = new System.Windows.Forms.TextBox();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.listBoxAllProcesses = new System.Windows.Forms.ListBox();
			this.sampleTimer = new System.Windows.Forms.Timer(this.components);
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.userMenu.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// userMenu
			// 
			this.userMenu.ImageScalingSize = new System.Drawing.Size(19, 19);
			this.userMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.optionsToolStripMenuItem1});
			this.userMenu.Location = new System.Drawing.Point(0, 0);
			this.userMenu.Name = "userMenu";
			this.userMenu.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
			this.userMenu.Size = new System.Drawing.Size(403, 28);
			this.userMenu.TabIndex = 0;
			this.userMenu.Text = "menuStrip1";
			// 
			// optionsToolStripMenuItem
			// 
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Size = new System.Drawing.Size(55, 24);
			this.optionsToolStripMenuItem.Text = "Help";
			this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
			// 
			// optionsToolStripMenuItem1
			// 
			this.optionsToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cleanDatabaseToolStripMenuItem,
            this.aboutToolStripMenuItem});
			this.optionsToolStripMenuItem1.Name = "optionsToolStripMenuItem1";
			this.optionsToolStripMenuItem1.Size = new System.Drawing.Size(75, 24);
			this.optionsToolStripMenuItem1.Text = "Options";
			// 
			// cleanDatabaseToolStripMenuItem
			// 
			this.cleanDatabaseToolStripMenuItem.Name = "cleanDatabaseToolStripMenuItem";
			this.cleanDatabaseToolStripMenuItem.Size = new System.Drawing.Size(195, 26);
			this.cleanDatabaseToolStripMenuItem.Text = "Clean Database";
			this.cleanDatabaseToolStripMenuItem.Click += new System.EventHandler(this.cleanDatabaseToolStripMenuItem_Click);
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(195, 26);
			this.aboutToolStripMenuItem.Text = "About";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new System.Drawing.Point(0, 33);
			this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(403, 422);
			this.tabControl1.TabIndex = 2;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.groupBox1);
			this.tabPage1.Controls.Add(this.button2);
			this.tabPage1.Controls.Add(this.button1);
			this.tabPage1.Controls.Add(this.listBoxActiveProcesses);
			this.tabPage1.Location = new System.Drawing.Point(4, 25);
			this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
			this.tabPage1.Size = new System.Drawing.Size(395, 393);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Active Processes";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.textBox1);
			this.groupBox1.Location = new System.Drawing.Point(9, 7);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.groupBox1.Size = new System.Drawing.Size(237, 53);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Search";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(5, 21);
			this.textBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(225, 22);
			this.textBox1.TabIndex = 1;
			this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(253, 337);
			this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(133, 23);
			this.button2.TabIndex = 3;
			this.button2.Text = "Sort Descending";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(253, 308);
			this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(133, 23);
			this.button1.TabIndex = 2;
			this.button1.Text = "Sort Ascending";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// listBoxActiveProcesses
			// 
			this.listBoxActiveProcesses.ForeColor = System.Drawing.SystemColors.WindowText;
			this.listBoxActiveProcesses.FormattingEnabled = true;
			this.listBoxActiveProcesses.ItemHeight = 16;
			this.listBoxActiveProcesses.Location = new System.Drawing.Point(8, 68);
			this.listBoxActiveProcesses.Margin = new System.Windows.Forms.Padding(4);
			this.listBoxActiveProcesses.Name = "listBoxActiveProcesses";
			this.listBoxActiveProcesses.Size = new System.Drawing.Size(239, 292);
			this.listBoxActiveProcesses.TabIndex = 0;
			this.listBoxActiveProcesses.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.groupBox2);
			this.tabPage2.Controls.Add(this.button3);
			this.tabPage2.Controls.Add(this.button4);
			this.tabPage2.Controls.Add(this.listBoxAllProcesses);
			this.tabPage2.Location = new System.Drawing.Point(4, 25);
			this.tabPage2.Margin = new System.Windows.Forms.Padding(4);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(4);
			this.tabPage2.Size = new System.Drawing.Size(395, 393);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "All Processes";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.textBoxAllProcesses);
			this.groupBox2.Location = new System.Drawing.Point(8, 7);
			this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.groupBox2.Size = new System.Drawing.Size(237, 53);
			this.groupBox2.TabIndex = 8;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Search";
			// 
			// textBoxAllProcesses
			// 
			this.textBoxAllProcesses.Location = new System.Drawing.Point(5, 21);
			this.textBoxAllProcesses.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.textBoxAllProcesses.Name = "textBoxAllProcesses";
			this.textBoxAllProcesses.Size = new System.Drawing.Size(225, 22);
			this.textBoxAllProcesses.TabIndex = 1;
			this.textBoxAllProcesses.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(253, 336);
			this.button3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(133, 23);
			this.button3.TabIndex = 7;
			this.button3.Text = "Sort Descending";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(253, 306);
			this.button4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(133, 23);
			this.button4.TabIndex = 6;
			this.button4.Text = "Sort Ascending";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// listBoxAllProcesses
			// 
			this.listBoxAllProcesses.FormattingEnabled = true;
			this.listBoxAllProcesses.ItemHeight = 16;
			this.listBoxAllProcesses.Location = new System.Drawing.Point(7, 66);
			this.listBoxAllProcesses.Margin = new System.Windows.Forms.Padding(4);
			this.listBoxAllProcesses.Name = "listBoxAllProcesses";
			this.listBoxAllProcesses.Size = new System.Drawing.Size(239, 292);
			this.listBoxAllProcesses.TabIndex = 5;
			this.listBoxAllProcesses.SelectedIndexChanged += new System.EventHandler(this.listBoxAllProcesses_SelectedIndexChanged);
			// 
			// sampleTimer
			// 
			this.sampleTimer.Enabled = true;
			this.sampleTimer.Interval = 500;
			this.sampleTimer.Tick += new System.EventHandler(this.sampleTimer_Tick);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(19, 19);
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
			// 
			// trayIcon
			// 
			this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
			this.trayIcon.Text = "Best Activity Traker";
			this.trayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.trayIcon_MouseDoubleClick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(403, 450);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.userMenu);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.userMenu;
			this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.Name = "MainForm";
			this.Text = "Activity Tracker";
			this.Resize += new System.EventHandler(this.MainForm_Resize);
			this.userMenu.ResumeLayout(false);
			this.userMenu.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

        #endregion

        private System.Windows.Forms.MenuStrip userMenu;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ListBox listBoxActiveProcesses;
        private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.Timer sampleTimer;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox textBoxAllProcesses;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.ListBox listBoxAllProcesses;
		private System.Windows.Forms.NotifyIcon trayIcon;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem cleanDatabaseToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
	}
}

