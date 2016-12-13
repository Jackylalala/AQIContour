using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Surfer;
using System.Xml;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class frmAQIContour : Form
    {
        #region | Field |
            // 定義網頁座標資料結構，用以表示資料所在的表格、列、欄
            struct WebPosition
            {
                private int table;
                private int row;
                private int column;
                public WebPosition(int table, int row, int col)
                {
                    this.table = table;
                    this.row = row;
                    column = col;
                }

                public int Table {get {return table;}}
                public int Row {get {return row;}}
                public int Column {get {return column;}}
            }
            private string path = AppDomain.CurrentDomain.BaseDirectory;
        #endregion

        public frmAQIContour()
        {
            InitializeComponent();
        }

        #region | Methods |
            /// <summary>
            /// 搜尋HTML字串，取得第tableNO個表格，第rowNO列，第columnNO行之資訊，並轉為數值
            /// 運用Regular Expression搜尋HTML字串
            /// 因為這個方法會忽略包含其他table的table(或者包含其他tr的tr)，亦即只搜尋最深層的標籤，
            /// 所以搜尋表格時要特別注意有幾個表格，通常表格順序如同網頁瀏覽器上所看到的順序，不會有沒看到的表格
            /// </summary>
            /// <param name="html">HTML碼</param>
            /// <param name="pos">資料所在位置</param>
            /// <param name="isnumeric">是否為數字</param>
            /// <returns>資料</returns>
            private string ExtractHTMLValue(string html, WebPosition pos, bool isnumeric=false)
            {
                html = html.ToLower();
                MatchCollection blocks;
                // 因為找到的片段之索引(index)是從0開始，所以所有的順序都要減去1(因為一般由1開始算)
                // 找出 <table...</table> 片段
                blocks = Regex.Matches(html, "<table\\b[^>]*>(?:(?>[^<]+)|<(?!table\\b[^>]*>))*?</table>");
                html = blocks[(pos.Table - 1)].Value;
                // 找出 <tr>...</tr> 片段
                blocks = Regex.Matches(html, "<tr\\b[^>]*>(?:(?>[^<]+)|<(?!tr\\b[^>]*>))*?</tr>");
                html = blocks[(pos.Row - 1)].Value;
                // 找出 <td>...</td> 片段
                blocks = Regex.Matches(html, "<td\\b[^>]*>(?:(?>[^<]+)|<(?!td\\b[^>]*>))*?</td>");
                html = blocks[(pos.Column - 1)].Value;
                // 利用Regular Expression去除HTML標籤
                // Regular Expression: ? 接在*或+等運算子後面，代表使該運算子的次數越少越好(亦即可以只重複一次就不要重複兩次)
                //                     . 代表除了/n之外的所有單字元
                html = Regex.Replace(html, @"<[^>]*>", String.Empty);
                html = html.Replace("\r\n", string.Empty);
                //去除括號之後字串(大額交易人資訊會有括號代表百分比，此為不需要之資訊)
                if ((html.IndexOf("(") + 1) > 0)
                    //有找到括號再去除，避免將字串吃乾抹淨
                    html = html.Substring(0, ((html.IndexOf("(") + 1) - 1));
                // 回傳數值
                html = html.Replace(",", String.Empty); // 去除千分位符號
                html = html.Replace(" ", String.Empty); // 去除空白
                if (isnumeric)
                    html = Regex.Replace(html, "[^0-9.]", "");
                return html;
            }
            
            /// <summary>
            /// 將網頁的表格資料存入dataset
            /// </summary>
            /// <param name="html">HTML code</param>
            /// <returns></returns>
            private DataSet ExtractHTMLValue(string html)
            {
                html = html.ToLower();
                DataSet dt = new DataSet();
                MatchCollection blocks_table, blocks_row, blocks_col;
                // 因為找到的片段之索引(index)是從0開始，所以所有的順序都要減去1(因為一般由1開始算)
                // 找出 <table...</table> 片段
                blocks_table = Regex.Matches(html, "<table\\b[^>]*>(?:(?>[^<]+)|<(?!table\\b[^>]*>))*?</table>");
                for (int i=0;i<blocks_table.Count;i++)
                {
                    dt.Tables.Add();
                    blocks_row = Regex.Matches(blocks_table[i].Value, "<tr\\b[^>]*>(?:(?>[^<]+)|<(?!tr\\b[^>]*>))*?</tr>");
                    int header = 0; //header correction to row count
                    for (int j = 0; j < blocks_row.Count; j++)
                    {
                        //try to read header
                        if (j == 0) //第一列的時候定義行數
                        {
                            blocks_col = Regex.Matches(blocks_row[0].Value, "<th\\b[^>]*>(?:(?>[^<]+)|<(?!td\\b[^>]*>))*?</th>");
                            if (blocks_col.Count != 0)
                            {
                                header = 1;
                                for (int k = 0; k < blocks_col.Count; k++)
                                {
                                    string content = Regex.Replace(blocks_col[k].Value, @"<[^>]*>", String.Empty);
                                    content = content.Replace("\r\n", string.Empty);
                                    content = content.Replace(" ", string.Empty);
                                    dt.Tables[i].Columns.Add(content);
                                }
                            }
                            else //no header
                            {
                                blocks_col = Regex.Matches(blocks_row[0].Value, "<td\\b[^>]*>(?:(?>[^<]+)|<(?!td\\b[^>]*>))*?</td>");
                                for (int n = 0; n < blocks_col.Count; n++)
                                    dt.Tables[i].Columns.Add();
                                dt.Tables[i].Rows.Add();
                                for (int k = 0; k < blocks_col.Count; k++)
                                {
                                    string content = Regex.Replace(blocks_col[k].Value, @"<[^>]*>", String.Empty);
                                    content = content.Replace("\r\n", string.Empty);
                                    content = content.Replace(" ", string.Empty);
                                    dt.Tables[i].Rows[0][k] = content;
                                }
                            }
                        }
                        else
                        {
                            blocks_col = Regex.Matches(blocks_row[j].Value, "<td\\b[^>]*>(?:(?>[^<]+)|<(?!td\\b[^>]*>))*?</td>");
                            dt.Tables[i].Rows.Add();
                            for (int k = 0; k < blocks_col.Count; k++)
                            {
                                string content = Regex.Replace(blocks_col[k].Value, @"<[^>]*>", String.Empty);
                                content = content.Replace("\r\n", string.Empty);
                                content = content.Replace(" ", string.Empty);
                                dt.Tables[i].Rows[j - header][k] = content;
                            }
                        }
                    }
                }
                return dt;
            }

            private string getSourceCode(string url, Encoding encode)
            {
                //利用WebClient開啟網址，將原始碼讀入資料串流(Stream)中，再利用StreamReader設定編碼並讀出字串
                WebClient cc = new WebClient();
                Stream dataS = cc.OpenRead(url);
                StreamReader dataR = new StreamReader(dataS, encode);
                return dataR.ReadToEnd();
            }

            private void DatToGrid(string algorithmText)
            {
                //determine algorithm
                Surfer.SrfGridAlgorithm algorithm = new Surfer.SrfGridAlgorithm();
                switch (algorithmText)
                {
                    case "InverseDistance":
                        algorithm = Surfer.SrfGridAlgorithm.srfInverseDistance;
                        break;
                    case "Kriging":
                        algorithm = Surfer.SrfGridAlgorithm.srfKriging;
                        break;
                    case "LocalPolynomial":
                        algorithm = Surfer.SrfGridAlgorithm.srfLocalPolynomial;
                        break;
                    case "MinCurvature":
                        algorithm = Surfer.SrfGridAlgorithm.srfMinCurvature;
                        break;
                    case "MovingAverage":
                        algorithm = Surfer.SrfGridAlgorithm.srfMovingAverage;
                        break;
                    case "NaturalNeighbor":
                        algorithm = Surfer.SrfGridAlgorithm.srfNaturalNeighbor;
                        break;
                    case "NearestNeighbor":
                        algorithm = Surfer.SrfGridAlgorithm.srfNearestNeighbor;
                        break;
                    case "RadialBasis":
                        algorithm = Surfer.SrfGridAlgorithm.srfRadialBasis;
                        break;
                    case "Regression":
                        algorithm = Surfer.SrfGridAlgorithm.srfRegression;
                        break;
                    case "Shepards":
                        algorithm = Surfer.SrfGridAlgorithm.srfShepards;
                        break;
                    case "Triangulation":
                        algorithm = Surfer.SrfGridAlgorithm.srfTriangulation;
                        break;
                }
                Surfer.Application app = new Surfer.Application();
                app.GridData2(DataFile: path + "temp.dat",          //数据文件地址
                    xCol: 1,                                        //x为第一列数据
                    yCol: 2,                                        //y为第二列数据
                    zCol: 3,                                        //z为第三列数据
                    DupMethod: Surfer.SrfDupMethod.srfDupNone,
                    xMin: 140000,                               //x最小值
                    xMax: 370000,                               //x最大者
                    yMin: 2400000,                                //y最小值
                    yMax: 2800000,                                //y最大值
                    Algorithm: algorithm,  //插值算法
                    NumCols: 39,      //x方向插值数据量
                    NumRows: 100,        //y方向插值数据量
                    OutGrid: path + "temp.grd",                     //返回文件为gridfile
                    OutFmt: Surfer.SrfGridFormat.srfGridFmtAscii);  //返回文件编码为Ascii
                app.Quit();
                System.GC.Collect(System.GC.GetGeneration(app));
            }

            private void GenerateContourMap()
            {
                Surfer.Application app = new Surfer.Application();
                IDocuments docs = app.Documents;
                IPlotDocument Doc = (IPlotDocument)docs.Add(SrfDocTypes.srfDocPlot);	//創建一個空白繪圖文檔
                IShapes Shapes = Doc.Shapes;

                #region 添加等值面
                IMapFrame contourMapFrame = Shapes.AddContourMap(path + "temp.grd");	//加載網格文件
                for (int i = 1; i <= contourMapFrame.Axes.Count; i++)
                {
                    contourMapFrame.Axes.Item(i).Visible = false;
                    contourMapFrame.Axes.Item(i).MajorTickType = SrfTickType.srfTickNone;
                    contourMapFrame.Axes.Item(i).ShowLabels = false;
                }
                contourMapFrame.SetLimits(xMin: 140000,	//x最小值
                                xMax: 370000,			//x最大者
                                yMin: 2400000,			//y最小值
                                yMax: 2800000				//y最大值
                );
                contourMapFrame.xMapPerPU = 23000;			//設置比例
                contourMapFrame.yMapPerPU = 20000;			//設置比例


                IContourMap contourMap = (IContourMap)contourMapFrame.Overlays.Item(1);

                /*
                contourMap.ShowColorScale = true;										// 顯示對應色柱
                contourMap.ColorScale.Top = 10;										//色柱y方向位置
                contourMap.ColorScale.Left = contourMap.Left + contourMap.Width + 0.8;//色柱x方向位置
                contourMap.ColorScale.Width = 1;									//色柱寬度
                contourMap.ColorScale.Height = 10;										//色柱高度
                contourMap.ColorScale.LabelFont.Size = 10;
                contourMap.ColorScale.LabelFont.Face = "Time New Roman";
                contourMap.ColorScale.LabelFrequency = 1;
                contourMap.ColorScale.Name = "PM2.5 (μg/m3)";
                */

                contourMap.FillContours = true;//添加顏色填充
                //通過文件加載顏色
                ILevels levels = contourMap.Levels;
                levels.LoadFile(path+"PM25.lvl");

                //加載系統顏色
                //contourMap.FillForegroundColorMap.LoadFile(path + "Rainbow.clr");
                //contourMap.ApplyFillToLevels(1, 1, 0);

                //使用灰色
                //contourMap.Levels.AutoGenerate(contourMap.Grid.zMin,contourMap.Grid.zMax,10);

                for (int i = 0; i < contourMap.Levels.Count; i++)
                {
                    contourMap.Levels.Item(i + 1).ShowLabel = false;					//顯示等值線上的數值
                    contourMap.Levels.Item(i + 1).ShowHach = false;					//
                    contourMap.Levels.Item(i + 1).Line.Style = "Invisible";			//不顯示線
                }

                contourMap.SmoothContours = SrfConSmoothType.srfConSmoothNone;   //平滑等值線邊界當前設置不平滑
                #endregion


                #region 添加邊界
                //後添加的會覆蓋在先前添加的圖片之上
                IMapFrame boundryMapFrame = Shapes.AddBaseMap(path + "city.shp", "Defaults=1");
                for (int i = 1; i <= boundryMapFrame.Axes.Count; i++)
                {
                    boundryMapFrame.Axes.Item(i).Visible = false;							//隱藏軸線
                    boundryMapFrame.Axes.Item(i).MajorTickType = SrfTickType.srfTickNone;	//隱藏邊線
                    boundryMapFrame.Axes.Item(i).ShowLabels = false;						//隱藏軸線上的坐標
                }
                boundryMapFrame.SetLimits(xMin: 140000,	//x最小值
                                xMax: 370000,			//x最大者
                                yMin: 2400000,			//y最小值
                                yMax: 2800000				//y最大值
                );
                boundryMapFrame.xMapPerPU = 23000;
                boundryMapFrame.yMapPerPU = 20000;

                IBaseMap boundryBaseMap = (IBaseMap)boundryMapFrame.Overlays.Item(1);
                boundryBaseMap.Line.Width = 0.001;			//設置邊線寬度
                #endregion

                string strWH = string.Format("width = {0:f0}, height = {1:f0}, KeepAspect = 1, ColorDepth = 32", 680, 1280);//設置輸出圖片的高度和寬度
                Doc.Export2(path + "Image.png", SelectionOnly: false, Options: strWH, FilterId: "png");//設置輸出圖片格式名
                Doc.Close(SrfSaveTypes.srfSaveChangesNo);	//不生成srf文件
                app.Quit();
                System.GC.Collect(System.GC.GetGeneration(app));
            }
        #endregion

        private void btnNow_Click(object sender, EventArgs e)
        {
            bgdWorker.RunWorkerAsync(new string[1] {cboAlgorithm.SelectedItem.ToString()});
            toolStripProgressBar1.Style = ProgressBarStyle.Marquee;
            txtInformation.Text = "Interpolation method: " + cboAlgorithm.SelectedItem.ToString() + "\r\nPollutant: PM2.5\r\nUnit: μg/m3";
        }

        private void bgdWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                DataSet dt = new DataSet();
                if (File.Exists(path + "temp.dat"))
                    File.Delete(path + "temp.dat");
                if (File.Exists(path + "temp.grd"))
                    File.Delete(path + "temp.grd");
                dt.Clear();
                dt.Tables.Add("sites");
                dt.Tables["sites"].Columns.Add("name", typeof(string));
                dt.Tables["sites"].Columns.Add("utm-x", typeof(string));
                dt.Tables["sites"].Columns.Add("utm-y", typeof(string));
                StreamReader sr = new StreamReader(path + "sites.dat", Encoding.UTF8);
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] props = line.Split(' ');
                    dt.Tables[0].Rows.Add(new object[] { props[0], props[1], props[2] });
                }
                string html = getSourceCode("http://opendata.epa.gov.tw/Data/Contents/AQX/", Encoding.UTF8);
                //DataSet dtTemp = ExtractHTMLValue(html);
                dt.Tables.Add(ExtractHTMLValue(html).Tables[3].Copy());
                dt.Tables[1].TableName = "data";
                dt.Tables["sites"].Columns.Add("data");
                foreach (DataRow dr in dt.Tables["sites"].Rows)
                {
                    foreach (DataRow dr2 in dt.Tables["data"].Rows)
                    {
                        if (dr["name"].ToString().Equals(dr2["測站名稱"].ToString()))
                            dr["data"] = dr2["細懸浮微粒濃度"];
                    }
                }
                //write data to temp.dat
                StreamWriter sw = new StreamWriter(path + "temp.dat");
                foreach (DataRow dr in dt.Tables["sites"].Rows)
                    sw.WriteLine(dr["utm-x"] + " " + dr["utm-y"] + " " + dr["data"]);
                sw.Close();
                //start draw contour
                DatToGrid((e.Argument as string[])[0]);
                GenerateContourMap();
                picDisplay.ImageLocation = path + "image.png";
                //get data time
                DateTime datatime = new DateTime();
                //add space to datatime string
                string tmp = dt.Tables["data"].Rows[0]["發布時間"].ToString().Substring(0, 10) + " " + dt.Tables["data"].Rows[0]["發布時間"].ToString().Substring(10);
                DateTime.TryParse(tmp, out datatime);
                bgdWorker.ReportProgress(100, datatime);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bgdWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            toolStripProgressBar1.Style = ProgressBarStyle.Blocks;
            toolStripProgressBar1.Value = 0;
        }

        private void bgdWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.lblDataTime.Text = "Data Time: " + ((DateTime)e.UserState).ToString("yyyy/MM/dd HH:mm:ss");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.lblTime.Text = "Current Time: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }

        private void frmAQIContour_Load(object sender, EventArgs e)
        {
            cboAlgorithm.SelectedIndex = 0;
        }
    }
}
