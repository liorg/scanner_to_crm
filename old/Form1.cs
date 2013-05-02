using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using TwainLib;
using GdiPlusLib;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Net;
using System.Configuration;
using System.Web.Services.Protocols;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Reflection;



namespace testdotnettwain
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	/// 



	
	
	public class TrustAllCertificatePolicy : ICertificatePolicy
	{
		public bool CheckValidationResult(ServicePoint sp,
			X509Certificate cert, WebRequest req, int problem)
		{
			return true;
		}
	}
	public class frmMain : System.Windows.Forms.Form,IMessageFilter
	{
		
		
		private Twain	tw;
		
		private System.ComponentModel.Container components = null;
		BITMAPINFOHEADER bmi;
		Rectangle bmprect= new Rectangle( 0, 0, 0, 0 );
		ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
		IntPtr	bmpptr;
		IntPtr	pixptr;
		static string ObjectId = string.Empty;
		static string ObjectType=string.Empty ;
		static string ScanSource=string.Empty ;
		static string ObjectInfo=string.Empty ;
		string txtHeader=string.Empty;
		string txtNote=string.Empty;
		[DllImport("gdi32.dll", ExactSpelling=true)]
		internal static extern int SetDIBitsToDevice( IntPtr hdc, int xdst, int ydst,
			int width, int height, int xsrc, int ysrc, int start, int lines,
			IntPtr bitsptr, IntPtr bmiptr, int color );

		[DllImport("kernel32.dll", ExactSpelling=true)]
		internal static extern IntPtr GlobalLock( IntPtr handle );
		[DllImport("kernel32.dll", ExactSpelling=true)]
		internal static extern IntPtr GlobalFree( IntPtr handle );

		[DllImport("kernel32.dll", CharSet=CharSet.Auto) ]
		public static extern void OutputDebugString( string outstr );
		[DllImport("gdiplus.dll", ExactSpelling=true)]
		internal static extern int GdipCreateBitmapFromGdiDib( IntPtr bminfo, IntPtr pixdat, ref IntPtr image );

		[DllImport("gdiplus.dll", ExactSpelling=true, CharSet=CharSet.Unicode)]
		internal static extern int GdipSaveImageToFile(IntPtr image,string filename,[In] ref Guid clsid,IntPtr encparams);

		[DllImport("gdiplus.dll", ExactSpelling=true)]
		internal static extern int GdipDisposeImage(IntPtr image);

		public frmMain(string txtHeader,string txtNote)
		{
			InitializeComponent();
			this.txtHeader=txtHeader;
			this.txtNote=txtNote;
		}

		public void Acquire()
		{
			tw = new Twain();
			tw.Init( this.Handle );
			//tw.Select();
			this.Enabled=false;
			Application.AddMessageFilter(this);
			tw.Acquire();
		}

		
		bool IMessageFilter.PreFilterMessage(ref Message m)
		{
			TwainCommand cmd = tw.PassMessage(ref m);
			if( cmd == TwainCommand.Not )
				return false;
			switch(cmd)
			{
				case TwainCommand.CloseRequest:
				{
					EndingScan();
					tw.CloseSrc();
					break;
				}
				case TwainCommand.CloseOk:
				{
					EndingScan();
					tw.CloseSrc();
					break;
				}
				case TwainCommand.DeviceEvent:
				{
					
					break;
				}
				
				case TwainCommand.TransferReady:
				{
					try
					{
						string TmpFolder=@System.Configuration.ConfigurationSettings.AppSettings["tmpFolder"];
						ArrayList pics=tw.TransferPictures();
						EndingScan();
						tw.CloseSrc();
						string strFileName;
						strFileName="";
                        //MODI.DocumentClass doc = new MODI.DocumentClass();
                        //MODI.DocumentClass page ;
                        //doc.Create(String.Empty);
						for( int i = 0; i < pics.Count; i++ )
						{
							IntPtr img = (IntPtr) pics[i];
							bmpptr=GlobalLock(img);
							pixptr=GetPixelInfo(bmpptr);
							Guid clsid;
							
							if(!Directory.Exists(TmpFolder))
							{
								try
								{
									Directory.CreateDirectory(TmpFolder);
								}
								catch(Exception ex)
								{
									ShowException(ex.Message);
								}

							}
							
							strFileName=TmpFolder + "\\"+Environment.MachineName+i.ToString()+DateTime.Now.Millisecond+".tiff";
							GetCodecClsid(strFileName,out clsid);
							IntPtr img2 = IntPtr.Zero;
							GdipCreateBitmapFromGdiDib(bmpptr,pixptr,ref img2);
							GdipSaveImageToFile(img2,strFileName,ref clsid,IntPtr.Zero);


                         Bitmap bp=   BitmapFromDIB(bmpptr, pixptr);


                         //// create a new control
                         //PictureBox pb = new PictureBox();

                         //// assign the image
                         //pb.Image = new Bitmap(bp);

                         //// stretch the image
                         //pb.SizeMode = PictureBoxSizeMode.StretchImage;

                         //// set the size of the picture box
                         //pb.Height = pb.Image.Height / 10;
                         //pb.Width = pb.Image.Width / 10;
                         //this.Controls.Add(pb);

							GdipDisposeImage(img2);
                            //page = new MODI.DocumentClass();
                            //page.Create(strFileName);
                            //doc.Images.Add(page.Images[0],null);
							
                            //page.Close(false);
                            //System.Runtime.InteropServices.Marshal.ReleaseComObject(page);
							
							GC.Collect();
							GC.WaitForPendingFinalizers();
							GC.Collect();
							GC.WaitForPendingFinalizers();
							//page = null;
							try
							{
								
								//		File.Delete(strFileName);
							}
							catch{}
						}
						string OldFileName=strFileName;
						strFileName += ".new.tiff";
						string strNewFileName = TmpFolder+"\\"+ Environment.MachineName + DateTime.Now.Millisecond+".tiff";
                        //doc.SaveAs(strFileName,MODI.MiFILE_FORMAT.miFILE_FORMAT_TIFF,MODI.MiCOMP_LEVEL.miCOMP_LEVEL_LOW);
                        //doc.SaveAs(strNewFileName,MODI.MiFILE_FORMAT.miFILE_FORMAT_TIFF,MODI.MiCOMP_LEVEL.miCOMP_LEVEL_LOW);
						
                        //doc.Close(false);
						//System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);
						
						GC.Collect();
						GC.WaitForPendingFinalizers();
                        //if (doc!=null)
                        //{
                        //    System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);
                        //}
						
						GC.Collect();
						GC.WaitForPendingFinalizers();
						//doc = null;
						
                        //if (ScanSource=="0")
                        //    CrmUpdater(strFileName);

                        //else
                        //    ScanRegular(strFileName);

						
						//						fs.Close();
						GC.WaitForPendingFinalizers();
						try
						{
							

							string tmpfile=@System.Configuration.ConfigurationSettings.AppSettings["FileServer"];
							if (tmpfile!=null && tmpfile!=string.Empty )
							{
								File.Copy (strNewFileName,tmpfile+ "\\" + ObjectInfo+".tiff",true);
							}
							
								
							
                            //File.Delete(strNewFileName);
                            //File.Delete(strFileName);
                            //File.Delete(OldFileName);
						}
						catch(Exception ex)
						{
							MessageBox.Show(this,"Error : \r\n\r\n"+ex.Message.ToString()+"\r\n\r\n" + ex.StackTrace,"Guardian information system");
							}
						
						MessageBox.Show(this,"Done","Guardian Information Systems");
					}
					catch(Exception ex)
					{
						MessageBox.Show(this,"Error : \r\n\r\n"+ex.Message.ToString()+"\r\n\r\n" + ex.StackTrace,"Guardian information system");
					}
					finally
					{
						//Environment.Exit(0);
						//Application.Exit();
					}
					break;
				}
			}
			return true;
		}
     
        public static Bitmap BitmapFromDIB(IntPtr pDIB, IntPtr pPix)
        {


            MethodInfo mi = typeof(Bitmap).GetMethod("FromGDIplus",
                            BindingFlags.Static | BindingFlags.NonPublic);

            if (mi == null)
                return null; // (permission problem) 

            IntPtr pBmp = IntPtr.Zero;
            int status = GdipCreateBitmapFromGdiDib(pDIB, pPix, ref pBmp);

            if ((status == 0) && (pBmp != IntPtr.Zero)) // success 
                return (Bitmap)mi.Invoke(null, new object[] { pBmp });

            else
                return null; // failure 
        }


		private void ScanRegular(string strFileName)
		{
			//ScanSource=args[0];
			//ObjectId = args[1];
			//ObjectInfo=args[2];
			//	ObjectType=args[3];
			if (Directory.Exists(ObjectId)) 
			{
				if (File.Exists(@ObjectId+"\\"+ObjectInfo+".tiff"))
				{
					try
					{
						File.Delete(@ObjectId+"\\"+ObjectInfo+".tiff");
					}
					catch(Exception ex)
					{
						ShowException(ex.Message);
					}
					
				}
				else
				{
					try
					{
						File.Move(strFileName,@ObjectId+"\\"+ObjectInfo+".tiff");
					}
					catch(Exception ex)
					{
						ShowException(ex.Message);
					}
					
				}
			}
			else
			{
				try
				{
					Directory.CreateDirectory(@ObjectId);
					File.Move(strFileName,@ObjectId+"\\"+ObjectInfo+".tiff");
				}
				catch(Exception ex)
				{
					ShowException(ex.Message);
				}
			}
		}
		private void CrmUpdater(string strFileName)
		{
			try    //  create Node
			{
				
				if(ObjectId != string.Empty && ObjectType!=string.Empty && ObjectInfo!=string.Empty )
				{
					Guid ObjectGuid =new Guid(ObjectId);
					
						
					string strNewFileName = Environment.MachineName + DateTime.Now.Millisecond+".tiff";
					
					CrmSdk.CrmService srv = CrmService();
					
					
					
				

					Random RandomClass = new Random();
					int num = RandomClass.Next(100000);
					CrmSdk.annotation note = new CrmSdk.annotation();
					
					note.subject = this.txtHeader;
					note.notetext = this.txtNote;
					note.objectid = new CrmSdk.Lookup();
					note.objectid.type =  ObjectType;//CrmSdk.EntityName.incident.ToString();  //<-- use the correct type
					note.objectid.Value = ObjectGuid;// inc.incidentid.Value;  //<-- use the contact's guid 
					note.objecttypecode = new CrmSdk.EntityNameReference();
					note.objecttypecode.Value = ObjectType;//CrmSdk.EntityName.incident.ToString(); // type
					note.objectid = new CrmSdk.Lookup();
					
					note.objectid.type =  CrmSdk.EntityName.incident.ToString();  //<-- use the correct type
					note.objectid.Value = ObjectGuid;// inc.incidentid.Value;  //<-- use the contact's guid 
					note.objecttypecode = new CrmSdk.EntityNameReference();
					note.objecttypecode.Value = ObjectType; // CrmSdk.EntityName.incident.ToString(); // type
					string save_as_link=System.Configuration.ConfigurationSettings.AppSettings["aslink"];

					if ( save_as_link!=null && save_as_link.ToUpper()=="Y")
					{
						note.mimetype = "text/html";
						string TmpFolder=@System.Configuration.ConfigurationSettings.AppSettings["tmpFolder"];
						string htmlFile=TmpFolder +"\\" + ObjectInfo+".html";
						//FileStream fsw=new FileStream(,FileMode.Create ,FileAccess.Write);
						//TextWriter bRw=new TextWriter(fsw);
						TextWriter bRw = File.CreateText(htmlFile) ;
						//bRw.BaseStream.Position=0;
						bRw.Write("<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title></title></head>"+
									"<body>"+
										"<script type=\"text/javascript\">"+
										" function runApp(fileName) " +
										"{ "+
											"var shell = new ActiveXObject(\"WScript.shell\"); "+
											"shell.run('file://' + fileName  , 1); "+
										"}\n"+
										"runApp('" + System.Configuration.ConfigurationSettings.AppSettings["FileServer"].Replace("\\","/")+"/" + ObjectInfo + ".tiff');"+
									    "window.opener=this;"+
										"window.close();"+
										"</script>"+
									"</body>"+
									"</html>");
						bRw.Flush();
						bRw.Close();
						
						//fsw.Close();
						bRw=null;
						//fsw=null;
						FileStream fs=new FileStream(htmlFile,FileMode.Open,FileAccess.Read);
						BinaryReader bR=new BinaryReader(fs);
						note.filename = ObjectInfo + ".html";
						note.filesize = new CrmSdk.CrmNumber();
						note.filesize.Value = (int)fs.Length;
						
						note.isdocument = new CrmSdk.CrmBoolean();
						note.isdocument.Value = true;
						

						note.documentbody = Convert.ToBase64String(bR.ReadBytes((int)fs.Length));//,Base64FormattingOptions.None);
						fs.Flush ();
						fs.Close();
						
						bR.Close();
						bR=null;
						fs=null;


						//note.documentbody=
							//note.documentbody="<a href='file:///'"+ System.Configuration.ConfigurationSettings.AppSettings["FileServer"]+ "\\" + ObjectInfo + ".tiff>file</a>";
							
					}
					else
					{
						FileStream fs=new FileStream(strFileName,FileMode.Open,FileAccess.Read);
						BinaryReader bR=new BinaryReader(fs);

						note.filename = ObjectInfo + ".tiff";
						note.filesize = new CrmSdk.CrmNumber();
						note.filesize.Value = (int)fs.Length;
						note.mimetype = "image/tiff";
						note.isdocument = new CrmSdk.CrmBoolean();
						note.isdocument.Value = true;
						

						note.documentbody = Convert.ToBase64String(bR.ReadBytes((int)fs.Length));//,Base64FormattingOptions.None);
						fs.Flush ();
						fs.Close();
						
						bR.Close();
						bR=null;
						fs=null;

					}
					
						srv.Create(note);

					
				}
			}
			catch(Exception ex)
			{
				ShowException(ex.Message);
				ShowException(ex.StackTrace);
				
			}
		}
		private void EndingScan()
		{
			Application.RemoveMessageFilter(this);
			this.Enabled=true;
			this.Activate();
		}
		protected IntPtr GetPixelInfo(IntPtr bmpptr)
		{
			bmi = new BITMAPINFOHEADER();
			Marshal.PtrToStructure(bmpptr,bmi);

			bmprect.X=bmprect.Y=0;
			bmprect.Width=bmi.biWidth;
			bmprect.Height=bmi.biHeight;

			if(bmi.biSizeImage==0)
				bmi.biSizeImage=((((bmi.biWidth * bmi.biBitCount) + 31) & ~31) >> 3)*bmi.biHeight;

			int p=bmi.biClrUsed;
			if((p==0)&&(bmi.biBitCount<=8))
				p=1<<bmi.biBitCount;
			p = (p * 4) + bmi.biSize + (int) bmpptr;
			return (IntPtr)p;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
					System.Runtime.InteropServices.Marshal.ReleaseComObject(components);
							
					GC.Collect();
					GC.WaitForPendingFinalizers();
				}
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
			// 
			// frmMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(8, 8);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmMain";
			this.Opacity = 0;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.TopMost = true;

		}
		#endregion

		
		
		[StructLayout(LayoutKind.Sequential,Pack=2)]
			internal class BITMAPINFOHEADER
		{
			public int   biSize;
			public int   biWidth;
			public int   biHeight;
			public short biPlanes;
			public short biBitCount;
			public int   biCompression;
			public int   biSizeImage;
			public int   biXPelsPerMeter;
			public int   biYPelsPerMeter;
			public int   biClrUsed;
			public int   biClrImportant;
		}
	
        private bool GetCodecClsid( string filename, out Guid clsid )
		{
			clsid=Guid.Empty;
			string ext=Path.GetExtension(filename);
			if(ext==null)
				return false;
			ext="*"+ext.ToUpper();
			foreach(ImageCodecInfo codec in codecs)
			{
				if(codec.FilenameExtension.IndexOf(ext)>= 0)
				{
					clsid=codec.Clsid;
					return true;
				}
			}
			return false;
		}
		private CrmSdk.CrmService CrmService()
		{

			
			//System.Text.RegularExpressions.Regex urlRegEx = new System.Text.RegularExpressions.Regex(@"https://"+host +"/.*");
			//System.Net.WebPermission p = new System.Net.WebPermission(NetworkAccess.Connect, urlRegEx);
			//p.Assert();
		    
			string port=System.Configuration.ConfigurationSettings.AppSettings["port"];
			string host=System.Configuration.ConfigurationSettings.AppSettings["Host"];
			string userName=System.Configuration.ConfigurationSettings.AppSettings["userName"];
			string password=System.Configuration.ConfigurationSettings.AppSettings["password"];
			string domain=System.Configuration.ConfigurationSettings.AppSettings["domain"];
			if (port.Trim().ToLower()!="ifd")
			{
				CrmSdk.CrmService srv = new CrmSdk.CrmService();
				srv.Url = System.Configuration.ConfigurationSettings.AppSettings["port"] +"://"+  host + System.Configuration.ConfigurationSettings.AppSettings["crmservice"];;
				;
					
				if (port!=null && port!=string.Empty && port.ToLower()=="https")
					System.Net.ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy();
					
				string OrganizationName=System.Configuration.ConfigurationSettings.AppSettings["OrganizationName"];
				if (OrganizationName!=null && OrganizationName!=string.Empty)
				{
					CrmSdk.CrmAuthenticationToken Token = new CrmSdk.CrmAuthenticationToken();
					Token.AuthenticationType = 0;

					Token.OrganizationName = System.Configuration.ConfigurationSettings.AppSettings["OrganizationName"];
					srv.CrmAuthenticationTokenValue = Token;
				}
				
					
				srv.Credentials = new System.Net.NetworkCredential(userName,password,domain);
				return srv;
			}
			else
			{
				System.Net.ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy();
				Crmdiscovery.CrmDiscoveryService disco = new Crmdiscovery.CrmDiscoveryService();
				disco.Url = host + System.Configuration.ConfigurationSettings.AppSettings["crmservice"];
				Crmdiscovery.RetrieveOrganizationsRequest orgRequest = new Crmdiscovery.RetrieveOrganizationsRequest();
				orgRequest.UserId = System.Configuration.ConfigurationSettings.AppSettings["userName"];
				orgRequest.Password = System.Configuration.ConfigurationSettings.AppSettings["password"];
				Crmdiscovery.RetrieveOrganizationsResponse orgResponse = (Crmdiscovery.RetrieveOrganizationsResponse)disco.Execute(orgRequest);
				Crmdiscovery.OrganizationDetail orgInfo = null;
				string OrgName=System.Configuration.ConfigurationSettings.AppSettings["OrganizationName"];
				foreach (Crmdiscovery.OrganizationDetail orgdetail in orgResponse.OrganizationDetails)
				{
					if (orgdetail.OrganizationName.Equals(OrgName))
					{
						orgInfo = orgdetail;
						break;
					}
				}
				if (orgInfo == null)
				{
					throw new Exception("The specified organization was not found.");
				}
				Crmdiscovery.RetrieveCrmTicketRequest ticketRequest = new Crmdiscovery.RetrieveCrmTicketRequest();
				
				ticketRequest.OrganizationName = orgInfo.OrganizationName;
				ticketRequest.UserId = domain +"\\"+ userName ;
				ticketRequest.Password = password;
				Crmdiscovery.RetrieveCrmTicketResponse ticketResponse = (Crmdiscovery.RetrieveCrmTicketResponse)disco.Execute(ticketRequest);

				CrmSdk.CrmAuthenticationToken sdktoken = new CrmSdk.CrmAuthenticationToken();
				sdktoken.AuthenticationType = 2;
				sdktoken.OrganizationName = orgInfo.OrganizationName;
				sdktoken.CrmTicket = ticketResponse.CrmTicket;
				CrmSdk.CrmService service = new CrmSdk.CrmService();
				service.CrmAuthenticationTokenValue = sdktoken;
				service.Url = orgInfo.CrmServiceUrl;

				return service;
			}
		}
		private string GetBase64StringFromFile(string filename) 
		{

			//Small routine to read the binary content of the file.
			FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            
			BinaryReader reader = new BinaryReader(fs);
			byte[] fileContent = reader.ReadBytes((int) fs.Length);
			reader.Close();
			fs.Close();
			return Convert.ToBase64String(fileContent);
		}
		static private void ShowException(string message)
		{
			MessageBox.Show(null,message,System.Configuration.ConfigurationSettings.AppSettings["ErrorMessgageHeader"],MessageBoxButtons.OK ,MessageBoxIcon.Error);
		}
	}
}
