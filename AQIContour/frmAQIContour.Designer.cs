namespace WindowsFormsApplication1
{
    partial class frmAQIContour
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.picDisplay = new System.Windows.Forms.PictureBox();
            this.btnNow = new System.Windows.Forms.Button();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblDataTime = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.bgdWorker = new System.ComponentModel.BackgroundWorker();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.cboAlgorithm = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblMathod = new System.Windows.Forms.Label();
            this.txtInformation = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picDisplay)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // picDisplay
            // 
            this.picDisplay.Location = new System.Drawing.Point(37, 77);
            this.picDisplay.Name = "picDisplay";
            this.picDisplay.Size = new System.Drawing.Size(300, 600);
            this.picDisplay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picDisplay.TabIndex = 7;
            this.picDisplay.TabStop = false;
            // 
            // btnNow
            // 
            this.btnNow.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnNow.Location = new System.Drawing.Point(78, 13);
            this.btnNow.Name = "btnNow";
            this.btnNow.Size = new System.Drawing.Size(223, 27);
            this.btnNow.TabIndex = 9;
            this.btnNow.Text = "Get Contour Map";
            this.btnNow.UseVisualStyleBackColor = true;
            this.btnNow.Click += new System.EventHandler(this.btnNow_Click);
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblTime.Location = new System.Drawing.Point(342, 12);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(90, 17);
            this.lblTime.TabIndex = 12;
            this.lblTime.Text = "Current Time:";
            // 
            // lblDataTime
            // 
            this.lblDataTime.AutoSize = true;
            this.lblDataTime.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblDataTime.Location = new System.Drawing.Point(342, 34);
            this.lblDataTime.Name = "lblDataTime";
            this.lblDataTime.Size = new System.Drawing.Size(72, 17);
            this.lblDataTime.TabIndex = 13;
            this.lblDataTime.Text = "Data Time:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 680);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(619, 22);
            this.statusStrip1.TabIndex = 14;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // bgdWorker
            // 
            this.bgdWorker.WorkerReportsProgress = true;
            this.bgdWorker.WorkerSupportsCancellation = true;
            this.bgdWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgdWorker_DoWork);
            this.bgdWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgdWorker_ProgressChanged);
            this.bgdWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgdWorker_RunWorkerCompleted);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // cboAlgorithm
            // 
            this.cboAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAlgorithm.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cboAlgorithm.FormattingEnabled = true;
            this.cboAlgorithm.Items.AddRange(new object[] {
            "InverseDistance",
            "Kriging",
            "LocalPolynomial",
            "MinCurvature",
            "MovingAverage",
            "NaturalNeighbor",
            "NearestNeighbor",
            "RadialBasis",
            "Regression",
            "Shepards",
            "Triangulation"});
            this.cboAlgorithm.Location = new System.Drawing.Point(363, 86);
            this.cboAlgorithm.Name = "cboAlgorithm";
            this.cboAlgorithm.Size = new System.Drawing.Size(244, 25);
            this.cboAlgorithm.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(360, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 17);
            this.label1.TabIndex = 16;
            this.label1.Text = "Interpolation method :";
            // 
            // lblMathod
            // 
            this.lblMathod.AutoSize = true;
            this.lblMathod.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblMathod.Location = new System.Drawing.Point(360, 133);
            this.lblMathod.Name = "lblMathod";
            this.lblMathod.Size = new System.Drawing.Size(168, 17);
            this.lblMathod.TabIndex = 17;
            this.lblMathod.Text = "Contour Map Information:";
            // 
            // txtInformation
            // 
            this.txtInformation.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtInformation.Location = new System.Drawing.Point(363, 154);
            this.txtInformation.Multiline = true;
            this.txtInformation.Name = "txtInformation";
            this.txtInformation.ReadOnly = true;
            this.txtInformation.Size = new System.Drawing.Size(244, 340);
            this.txtInformation.TabIndex = 18;
            // 
            // frmAQIContour
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(619, 702);
            this.Controls.Add(this.txtInformation);
            this.Controls.Add(this.lblMathod);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboAlgorithm);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.lblDataTime);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.btnNow);
            this.Controls.Add(this.picDisplay);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmAQIContour";
            this.Text = "AQI Contour";
            this.Load += new System.EventHandler(this.frmAQIContour_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picDisplay)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picDisplay;
        private System.Windows.Forms.Button btnNow;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblDataTime;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.ComponentModel.BackgroundWorker bgdWorker;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ComboBox cboAlgorithm;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblMathod;
        private System.Windows.Forms.TextBox txtInformation;
    }
}

