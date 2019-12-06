using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Http;
using System.Web;
using System.Web.UI;

namespace Core_Elms
{
    public partial class _Default : Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
        private string COREELMSFile = "";
        private string ApplicantFile = "";
        private string CollegesFile = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //set the last load dates
            if (IsPostBack)
            {
            }
            else
            {
                SetLoadDates();
            }
        }

        protected void SetLoadDates()
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                cnn.Open();

                string SQLcmd = "";
                // start here
                //SqlCommand command = new SqlCommand("", cnn);
                //command.Parameters.AddWithValue("@cas_id", "09877");

                SQLcmd = "dbo.[GetLastLoadDates] ";

                using (SqlCommand cmd = new SqlCommand(SQLcmd, cnn))
                {
                    //cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            this.lblLastCOREELMsDownload.Text = reader.GetString(0);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    reader.Close();
                }
            }
        }

        protected void btnUploadCoreELMSFile_Click(object sender, EventArgs e)
        {
            if (this.fuCOREElmsFile.HasFile)
            {
                try
                {
                    string filename = Path.GetFileName(fuCOREElmsFile.FileName);
                    COREELMSFile = Server.MapPath("~/") + filename;
                    lblCOREELMSFileName.Text = Server.MapPath("~/") + filename;
                    fuCOREElmsFile.SaveAs(Server.MapPath("~/") + filename);

                    COREELMSImportLoad("");

                    //lblCOREELMSResults.Text = "Upload status: File uploaded!";
                }
                catch (Exception ex)
                {
                    lblCOREELMSResults.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
                }
            }
        }

        protected void btnLoadCOREELMSFileToSQL_Click(object sender, EventArgs e)
        {
            COREELMSImportLoad("");
            lblCOREELMSResults.Text = "Done";
        }

        private void COREELMSImportLoad(string filePath)
        {
            //string connectionString = null;
            string SQLcmd = null;
            //string Tester = null;

            string workLine;
            int RowCount = 0;
            string[] rowArray = new string[40];
            try
            {
                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    string CourseNumber = "";
                    string strSession = "";
                    Int32 PHRBegin = 0;
                    Int32 PHREnd = 0;
                    cnn.Open();
                    SQLcmd = "exec [dbo].[TruncateCoreELMSImportTable] ";
                    using (SqlCommand cmd = new SqlCommand(SQLcmd, cnn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    foreach (string line in File.ReadLines(lblCOREELMSFileName.Text))
                    //foreach (string line in lines)
                    {
                        RowCount = RowCount + 1;
                        if (RowCount > 1)
                        {
                            workLine = CleanAString(line);
                            //workLine = line.Replace("\"", "");
                            var values = workLine.Split(',');

                            if (values.Length != 25)
                            {
                                // File is not the right size
                                // report an error and exit
                            }
                            //string CASID = values[0];

                            // start here
                            //SqlCommand command = new SqlCommand("", cnn);
                            //command.Parameters.AddWithValue("@cas_id", "09877");
                            PHRBegin = 0;
                            PHREnd = 0;
                            PHRBegin = values[14].IndexOf("(PHR")+1;
                            if (PHRBegin > 0)
                            {
                                PHREnd = values[14].IndexOf(")");
                                CourseNumber = values[14].Substring(PHRBegin, PHREnd-PHRBegin);
                            }
                            else
                            {
                                CourseNumber = "";
                            }
                            PHRBegin = 0;
                            PHREnd = 0;
                            PHRBegin = values[17].IndexOf("Block") + 6;
                            if (PHRBegin > 0)
                            {
                                //PHREnd = values[14].IndexOf(")");
                                strSession = values[17].Substring(PHRBegin,1);
                            }
                            else
                            {
                                strSession = "";
                            }

                            SQLcmd = "dbo.[Core_ElmsImport_Insert] ";
                            //SQLcmd += " @cas_id='" + CASID + "'";

                            SQLcmd += " @firstName='" + values[0].Replace("'", "''") + "'";
                            SQLcmd += ", @MiddleName='" + values[1].Replace("'", "''") + "'";
                            SQLcmd += ", @lastname='" + values[2].Replace("'", "''") + "'";
                            SQLcmd += ", @ID='" + values[3].Replace("'", "''") + "'";
                            SQLcmd += ", @Location='" + values[4].Replace("'", "''") + "'";
                            SQLcmd += ", @Program= '" + values[5].Replace("'", "''") + "'";
                            SQLcmd += ", @GradYear='" + values[6] + "'";
                            SQLcmd += ", @Email='" + values[7] + "'";
                            SQLcmd += ", @Phone='" + values[8] + "'";
                            SQLcmd += ", @PhoneCell='" + values[9] + "'";

                            SQLcmd += ", @PreceptorFirst='" + values[10] + "'";
                            SQLcmd += ", @PreceptorLast='" + values[11] + "'";
                            SQLcmd += ", @Site='" + values[12].Replace("'", "''") + "'";
                            SQLcmd += ", @SiteNumber='" + values[13] + "'";
                            SQLcmd += ", @RotationType='" + values[14].Replace("'", "''") + "'";
                            SQLcmd += ", @Elective='" + values[15].Replace("'", "''") + "'";
                            SQLcmd += ", @Rotation='" + values[16].Replace("'", "''") + "'";
                            SQLcmd += ", @RotationSpecialty='" + values[17].Replace("'", "''") + "'";
                            SQLcmd += ", @StartDate='" + values[18] + "'";
                            SQLcmd += ", @EndDate='" + values[19] + "'";

                            SQLcmd += ", @SiteRequirement='" + values[20].Replace("'", "''") + "'";
                            SQLcmd += ", @Status='" + values[21] + "'";
                            SQLcmd += ", @Expiration='" + values[22] + "'";                            
                            SQLcmd += ", @CompletedOn='" + values[23] + "'";
                            SQLcmd += ", @CourseNumber='" + CourseNumber + "'";
                            SQLcmd += ", @JenzabarID='" + values[24] + "'";
                            SQLcmd += ", @Session='" + strSession + "'";
                            using (SqlCommand cmd = new SqlCommand(SQLcmd, cnn))
                            {
                                if (RowCount > 1)
                                {
                                    cmd.ExecuteNonQuery();
                                }
                                //MessageBox.Show("Row inserted !! ");
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                var test21 = "";
                test21 = e.Message.ToString();
            }
            CreateOutputData();
            CreateCOREELMSStudentsFile();
        }

        protected void CreateOutputData()
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                cnn.Open();

                string SQLcmd = "";
                // start here
                //SqlCommand command = new SqlCommand("", cnn);
                //command.Parameters.AddWithValue("@cas_id", "09877");

                SQLcmd = "dbo.CreateOutputData ";

                using (SqlCommand cmd = new SqlCommand(SQLcmd, cnn))
                {
                    //cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            //btnCreateOutputFile.Text = reader.GetString(0);
                            //btnCreateOutputFile.Enabled = false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    reader.Close();
                }
            }
        }
        protected void btnCreateOutputFiles_Click(object sender, EventArgs e)
        {

            CreateOutputData();
            //CreateAllCOREELMSOutputFiles();
        }

        //*************************************** create the text files **********************************************

        private void CreateCOREELMSStudentsFile()
        {
            string constr = connectionString;
            string strFileContents = "";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    //cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Select * from vwSONISOutput ";
                    cmd.Connection = con;
                    con.Open();
                    strFileContents += "Action,Campus,Department,Division,Course,Session,Section,SchoolYear,Semester,UserName,SONISID,LastName,FirstName" + System.Environment.NewLine; ;

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                strFileContents += sdr[0].ToString() + ",";  //  Action
                                strFileContents += sdr[1].ToString() + ",";  // Campus
                                strFileContents += sdr[2].ToString() + ",";  // Department
                                strFileContents += sdr[3].ToString() + ",";  // Division
                                strFileContents += sdr[4].ToString() + ",";  // Course
                                strFileContents += sdr[5].ToString() + ",";  // Session
                                strFileContents += sdr[6].ToString() + ",";  // Section
                                strFileContents += sdr[7].ToString() + ",";  // SchoolYear
                                strFileContents += sdr[8].ToString() + ",";  // Semester
                                strFileContents += sdr[9].ToString() + ",";  // UserName
                                strFileContents += sdr[10].ToString() + ",";  // SONISID
                                strFileContents += sdr[11].ToString() + ",";  // LastName
                                strFileContents += sdr[12].ToString() + ",";  // FirstName
                                strFileContents += System.Environment.NewLine;
                            }
                        }
                    }

                    con.Close();
                }

                DownloadFile("COREELMSstudents.csv", strFileContents);
            }
        }

        private void DownloadFile(string fileName, string strContents)
        {
            string fName = Server.MapPath("~/") + fileName;
            System.IO.File.WriteAllText(fName, strContents);
            HttpResponseMessage response = new HttpResponseMessage();
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/excel";
            System.IO.FileInfo file = new System.IO.FileInfo(fName);
            if (file.Exists)
            {
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name); Response.AddHeader("Content-Length", file.Length.ToString());
                Response.WriteFile(fName);
                Response.Flush();
                Response.End();
            }
        }
        public string CleanAString(string strInput)
        {
            string lineOut = "";
            string workLine = "";
            string beginquoteFlag = "";
            string endquoteFlag = "";
            string Quote1 = ((char)34).ToString();
            string C1 = ((char)46).ToString();
            C1 = ",";
            string commaSubstitute = " ";
            workLine = strInput;
            lineOut = "";
            for (int i = 0; i < workLine.Length; i++)
            {
                if (workLine.Substring(i, 1) == Quote1)
                {
                    if (endquoteFlag == "Y")
                    {
                        endquoteFlag = "";
                        beginquoteFlag = "";
                    }
                    if (beginquoteFlag == "")
                    {
                        beginquoteFlag = "Y";
                    }
                    else if (beginquoteFlag == "Y")
                    {
                        endquoteFlag = "Y";
                        //beginquoteFlag = "";
                    }
                }
                if (beginquoteFlag == "Y" && endquoteFlag != "Y")
                {
                    if (workLine.Substring(i, 1) == Quote1)
                    {
                        //Don't put quotes into the output string
                        //lineOut = lineOut + line.Substring(i, 1);
                    }
                    else if (workLine.Substring(i, 1) == C1)
                    {
                        //If it's a comma, put in a substitute character
                        lineOut = lineOut + commaSubstitute;
                    }
                    else
                    {
                        if (workLine.Substring(i, 1) == "\\" || workLine.Substring(i, 1) == "\"")
                        {
                        }
                        else
                        {
                            lineOut = lineOut + workLine.Substring(i, 1);
                        }
                    }
                }
                else
                {
                    if (workLine.Substring(i, 1) == Quote1)
                    {
                        //Don't put quotes into the output string
                        //lineOut = lineOut + line.Substring(i, 1);
                    }
                    else
                    {
                        lineOut = lineOut + workLine.Substring(i, 1);
                    }
                }
            }
            return lineOut;
        }
    }
}