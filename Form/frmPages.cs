using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using testdotnettwain.Mechanism;
using testdotnettwain.Mechanism.WCFClient;
using testdotnettwain.UploadLargeImages;

/* Multipage tiff viewer
 * Created by Matjaž Grahek - 25.11.2008
 * 
 * Hi. This is my first article for CodeProject.
 * What you're seeing is an application for opening multipage tiffs and viewing the selected page.
 * 
 * This example is just a part of what might come out from one of you, but i think its good for a start.
 * 
 * Some comments are added next to the code. If you have some more questions, email me at matjaz.grahek@gmail.com
 * or post something on the article page.
 * 
 * PS: This code and the article was written by me and i dont know that much code. Intelisense helps a lot :)
 * I'm trying to say, that the code and application can be written MUCH better but its ment just as something to start from.
 */

namespace testdotnettwain
{
    public partial class frmPages : Form
    {
        Action<string> LogText;
        string _path;
        public frmPages(string path, Action<string> log)
        {
            InitializeComponent();
            LogText = log;
            intCurrPage = 0; // reseting the counter
            lblFile.Text = path; // showing the file name in the lblFile
            _path = path;
            RefreshImage(); // refreshing and showing the new file
            opened = true; // the files was opened.
           

        }
        
        private int intCurrPage = 0; // defining the current page (its some sort of a counter)
        bool opened = false; // if an image was opened

        public void RefreshImage()
        {
            Image myImg; // setting the selected tiff
            Image myBmp; // a new occurance of Image for viewing
        
            myImg = System.Drawing.Image.FromFile(@lblFile.Text); // setting the image from a file
            
            int intPages = myImg.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page); // getting the number of pages of this tiff
            intPages--; // the first page is 0 so we must correct the number of pages to -1
            lblNumPages.Text = Convert.ToString(intPages); // showing the number of pages
            lblCurrPage.Text = Convert.ToString(intCurrPage); // showing the number of page on which we're on
   
            myImg.SelectActiveFrame(System.Drawing.Imaging.FrameDimension.Page,intCurrPage); // going to the selected page

            myBmp = new Bitmap(myImg,pictureBox1.Width,pictureBox1.Height); // setting the new page as an image
            // Description on Bitmap(SOURCE, X,Y)

            pictureBox1.Image = myBmp; // showing the page in the pictureBox1

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (opened) // the button works if the file is opened. you could go with button.enabled
            {
                if (intCurrPage == 0) // it stops here if you reached the bottom, the first page of the tiff
                { intCurrPage = 0; }
                else
                {
                    intCurrPage--; // if its not the first page, then go to the previous page
                    RefreshImage(); // refresh the image on the selected page
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (opened) // the button works if the file is opened. you could go with button.enabled
            {
                if (intCurrPage == Convert.ToInt32(lblNumPages.Text)) // if you have reached the last page it ends here
                    // the "-1" should be there for normalizing the number of pages
                { intCurrPage = Convert.ToInt32(lblNumPages.Text); }
                else
                {
                    intCurrPage++;
                    RefreshImage();
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Upload(_path);
            Close();
        }

        private void Upload(string textFile)
        {
            WCFClientAdaptor clientUpload = new WCFClientAdaptor(LogText, Cursor, progressBar1);
            clientUpload.Upload(textFile);
        }
    }
}
