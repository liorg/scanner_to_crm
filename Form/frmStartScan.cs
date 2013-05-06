using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace testdotnettwain
{
	/// <summary>
	/// Summary description for Form2.
	/// </summary>
	public class frmStartScan : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button BtnScan;
		private System.Windows.Forms.TextBox txtNote;
		private System.Windows.Forms.TextBox txtHeader;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		
		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        public frmStartScan()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.BtnScan = new System.Windows.Forms.Button();
            this.txtNote = new System.Windows.Forms.TextBox();
            this.txtHeader = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BtnScan
            // 
            this.BtnScan.Location = new System.Drawing.Point(56, 136);
            this.BtnScan.Name = "BtnScan";
            this.BtnScan.Size = new System.Drawing.Size(64, 24);
            this.BtnScan.TabIndex = 9;
            this.BtnScan.Text = "סרוק";
            this.BtnScan.Click += new System.EventHandler(this.BtnScan_Click);
            // 
            // txtNote
            // 
            this.txtNote.Location = new System.Drawing.Point(56, 40);
            this.txtNote.MaxLength = 500;
            this.txtNote.Multiline = true;
            this.txtNote.Name = "txtNote";
           
            this.txtNote.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtNote.Size = new System.Drawing.Size(312, 88);
            this.txtNote.TabIndex = 8;
            // 
            // txtHeader
            // 
            this.txtHeader.Location = new System.Drawing.Point(56, 8);
            this.txtHeader.MaxLength = 50;
            this.txtHeader.Name = "txtHeader";
          
            this.txtHeader.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtHeader.Size = new System.Drawing.Size(312, 20);
            this.txtHeader.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(376, 40);
            this.label2.Name = "label2";
            this.label2.Text = "מסמך";
            this.label2.Size = new System.Drawing.Size(64, 24);
            this.label2.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(376, 8);
            this.label1.Name = "label1";
            this.label1.Text = "כותרת";
          
            this.label1.Size = new System.Drawing.Size(64, 24);
            this.label1.TabIndex = 5;
            // 
            // frmStartScan
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(440, 173);
            this.Controls.Add(this.BtnScan);
            this.Controls.Add(this.txtNote);
            this.Controls.Add(this.txtHeader);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmStartScan";
            this.Text = "Guardian Scanner ver 2.0";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		

		private void BtnScan_Click(object sender, System.EventArgs e)
		{


            frmScanner frmmain = new frmScanner(this.txtHeader.Text, this.txtNote.Text);
			
			//frmmain.Show();
			frmmain.Acquire();
          //  frmmain.Dispose();
		}
	}
}
