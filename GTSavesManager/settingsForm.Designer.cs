namespace GTSavesManager
{
    partial class SettingsForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveTextBox = new System.Windows.Forms.TextBox();
            this.launcherTextBox = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.browseSaveButton = new System.Windows.Forms.Button();
            this.browseLauncherButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Save folder location";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Launcher location";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // saveTextBox
            // 
            this.saveTextBox.Enabled = false;
            this.saveTextBox.Location = new System.Drawing.Point(12, 25);
            this.saveTextBox.Name = "saveTextBox";
            this.saveTextBox.Size = new System.Drawing.Size(857, 20);
            this.saveTextBox.TabIndex = 3;
            // 
            // launcherTextBox
            // 
            this.launcherTextBox.Enabled = false;
            this.launcherTextBox.Location = new System.Drawing.Point(12, 74);
            this.launcherTextBox.Name = "launcherTextBox";
            this.launcherTextBox.Size = new System.Drawing.Size(857, 20);
            this.launcherTextBox.TabIndex = 4;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(12, 114);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 5;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // browseSaveButton
            // 
            this.browseSaveButton.Location = new System.Drawing.Point(875, 25);
            this.browseSaveButton.Name = "browseSaveButton";
            this.browseSaveButton.Size = new System.Drawing.Size(34, 20);
            this.browseSaveButton.TabIndex = 6;
            this.browseSaveButton.Text = "...";
            this.browseSaveButton.UseVisualStyleBackColor = true;
            // 
            // browseLauncherButton
            // 
            this.browseLauncherButton.Location = new System.Drawing.Point(875, 74);
            this.browseLauncherButton.Name = "browseLauncherButton";
            this.browseLauncherButton.Size = new System.Drawing.Size(34, 20);
            this.browseLauncherButton.TabIndex = 7;
            this.browseLauncherButton.Text = "...";
            this.browseLauncherButton.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(921, 152);
            this.Controls.Add(this.browseLauncherButton);
            this.Controls.Add(this.browseSaveButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.launcherTextBox);
            this.Controls.Add(this.saveTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TextBox saveTextBox;
        private System.Windows.Forms.TextBox launcherTextBox;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button browseSaveButton;
        private System.Windows.Forms.Button browseLauncherButton;
    }
}