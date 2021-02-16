
namespace TicTacToe.GUI
{
	partial class MainForm
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.menuBar = new System.Windows.Forms.MenuStrip();
			this.playToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.playWithHumanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.playWithComputerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.panelBoard = new System.Windows.Forms.Panel();
			this.menuBar.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuBar
			// 
			this.menuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playToolStripMenuItem,
            this.aboutToolStripMenuItem});
			this.menuBar.Location = new System.Drawing.Point(0, 0);
			this.menuBar.Name = "menuBar";
			this.menuBar.Size = new System.Drawing.Size(899, 24);
			this.menuBar.TabIndex = 0;
			this.menuBar.Text = "menuStrip1";
			// 
			// playToolStripMenuItem
			// 
			this.playToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playWithHumanToolStripMenuItem,
            this.playWithComputerToolStripMenuItem});
			this.playToolStripMenuItem.Name = "playToolStripMenuItem";
			this.playToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.playToolStripMenuItem.Text = "Play!";
			// 
			// playWithHumanToolStripMenuItem
			// 
			this.playWithHumanToolStripMenuItem.Name = "playWithHumanToolStripMenuItem";
			this.playWithHumanToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.H)));
			this.playWithHumanToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
			this.playWithHumanToolStripMenuItem.Text = "Play with Human";
			// 
			// playWithComputerToolStripMenuItem
			// 
			this.playWithComputerToolStripMenuItem.Name = "playWithComputerToolStripMenuItem";
			this.playWithComputerToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.C)));
			this.playWithComputerToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
			this.playWithComputerToolStripMenuItem.Text = "Play with Computer";
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
			this.aboutToolStripMenuItem.Text = "About";
			// 
			// panelBoard
			// 
			this.panelBoard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.panelBoard.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelBoard.Location = new System.Drawing.Point(0, 24);
			this.panelBoard.Name = "panelBoard";
			this.panelBoard.Size = new System.Drawing.Size(899, 577);
			this.panelBoard.TabIndex = 1;
			this.panelBoard.Paint += new System.Windows.Forms.PaintEventHandler(this.panelBoard_Paint);
			this.panelBoard.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelBoard_MouseClick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(899, 601);
			this.Controls.Add(this.panelBoard);
			this.Controls.Add(this.menuBar);
			this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuBar;
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Tic Tac Toe";
			this.menuBar.ResumeLayout(false);
			this.menuBar.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuBar;
		private System.Windows.Forms.ToolStripMenuItem playToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem playWithHumanToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem playWithComputerToolStripMenuItem;
		private System.Windows.Forms.Panel panelBoard;
	}
}

