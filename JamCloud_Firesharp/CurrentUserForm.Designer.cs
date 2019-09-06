namespace JamCloud_Firesharp
{
    partial class CurrentUserForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.CurrentUserTxt = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label1.Location = new System.Drawing.Point(12, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(194, 39);
            this.label1.TabIndex = 0;
            this.label1.Text = "Current User:";
            // 
            // CurrentUserTxt
            // 
            this.CurrentUserTxt.AutoSize = true;
            this.CurrentUserTxt.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.CurrentUserTxt.Location = new System.Drawing.Point(267, 42);
            this.CurrentUserTxt.Name = "CurrentUserTxt";
            this.CurrentUserTxt.Size = new System.Drawing.Size(185, 39);
            this.CurrentUserTxt.TabIndex = 1;
            this.CurrentUserTxt.Text = "Current User";
            // 
            // CurrentUserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 128);
            this.Controls.Add(this.CurrentUserTxt);
            this.Controls.Add(this.label1);
            this.Name = "CurrentUserForm";
            this.Text = "CurrentUser";
            this.Load += new System.EventHandler(this.CurrentUser_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label CurrentUserTxt;
    }
}