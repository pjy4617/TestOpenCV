namespace Camera
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBoxIpl1 = new OpenCvSharp.UserInterface.PictureBoxIpl();
            this.pictureBoxIpl2 = new OpenCvSharp.UserInterface.PictureBoxIpl();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            this.trackBar3 = new System.Windows.Forms.TrackBar();
            this.pictureBoxIpl3 = new OpenCvSharp.UserInterface.PictureBoxIpl();
            this.trackBar4 = new System.Windows.Forms.TrackBar();
            this.pictureBoxIpl4 = new OpenCvSharp.UserInterface.PictureBoxIpl();
            this.pictureBoxIpl5 = new OpenCvSharp.UserInterface.PictureBoxIpl();
            this.pictureBoxIpl6 = new OpenCvSharp.UserInterface.PictureBoxIpl();
            this.pictureBoxIpl7 = new OpenCvSharp.UserInterface.PictureBoxIpl();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIpl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIpl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIpl3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIpl4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIpl5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIpl6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIpl7)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxIpl1
            // 
            this.pictureBoxIpl1.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxIpl1.Name = "pictureBoxIpl1";
            this.pictureBoxIpl1.Size = new System.Drawing.Size(600, 450);
            this.pictureBoxIpl1.TabIndex = 0;
            this.pictureBoxIpl1.TabStop = false;
            // 
            // pictureBoxIpl2
            // 
            this.pictureBoxIpl2.Location = new System.Drawing.Point(609, 3);
            this.pictureBoxIpl2.Name = "pictureBoxIpl2";
            this.pictureBoxIpl2.Size = new System.Drawing.Size(600, 450);
            this.pictureBoxIpl2.TabIndex = 1;
            this.pictureBoxIpl2.TabStop = false;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(2331, 472);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 3;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(2181, 572);
            this.trackBar1.Maximum = 255;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(271, 45);
            this.trackBar1.TabIndex = 4;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // trackBar2
            // 
            this.trackBar2.Location = new System.Drawing.Point(2172, 676);
            this.trackBar2.Maximum = 255;
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Size = new System.Drawing.Size(271, 45);
            this.trackBar2.TabIndex = 5;
            this.trackBar2.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar2.Scroll += new System.EventHandler(this.trackBar2_Scroll);
            // 
            // trackBar3
            // 
            this.trackBar3.Location = new System.Drawing.Point(2172, 760);
            this.trackBar3.Maximum = 255;
            this.trackBar3.Name = "trackBar3";
            this.trackBar3.Size = new System.Drawing.Size(271, 45);
            this.trackBar3.TabIndex = 6;
            this.trackBar3.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar3.Scroll += new System.EventHandler(this.trackBar3_Scroll);
            // 
            // pictureBoxIpl3
            // 
            this.pictureBoxIpl3.Location = new System.Drawing.Point(1215, 3);
            this.pictureBoxIpl3.Name = "pictureBoxIpl3";
            this.pictureBoxIpl3.Size = new System.Drawing.Size(600, 450);
            this.pictureBoxIpl3.TabIndex = 7;
            this.pictureBoxIpl3.TabStop = false;
            // 
            // trackBar4
            // 
            this.trackBar4.Location = new System.Drawing.Point(2181, 832);
            this.trackBar4.Maximum = 255;
            this.trackBar4.Name = "trackBar4";
            this.trackBar4.Size = new System.Drawing.Size(271, 45);
            this.trackBar4.TabIndex = 8;
            this.trackBar4.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar4.Scroll += new System.EventHandler(this.trackBar4_Scroll);
            // 
            // pictureBoxIpl4
            // 
            this.pictureBoxIpl4.Location = new System.Drawing.Point(1821, 3);
            this.pictureBoxIpl4.Name = "pictureBoxIpl4";
            this.pictureBoxIpl4.Size = new System.Drawing.Size(600, 450);
            this.pictureBoxIpl4.TabIndex = 9;
            this.pictureBoxIpl4.TabStop = false;
            // 
            // pictureBoxIpl5
            // 
            this.pictureBoxIpl5.Location = new System.Drawing.Point(3, 459);
            this.pictureBoxIpl5.Name = "pictureBoxIpl5";
            this.pictureBoxIpl5.Size = new System.Drawing.Size(600, 450);
            this.pictureBoxIpl5.TabIndex = 10;
            this.pictureBoxIpl5.TabStop = false;
            // 
            // pictureBoxIpl6
            // 
            this.pictureBoxIpl6.Location = new System.Drawing.Point(609, 459);
            this.pictureBoxIpl6.Name = "pictureBoxIpl6";
            this.pictureBoxIpl6.Size = new System.Drawing.Size(600, 450);
            this.pictureBoxIpl6.TabIndex = 11;
            this.pictureBoxIpl6.TabStop = false;
            // 
            // pictureBoxIpl7
            // 
            this.pictureBoxIpl7.Location = new System.Drawing.Point(1215, 459);
            this.pictureBoxIpl7.Name = "pictureBoxIpl7";
            this.pictureBoxIpl7.Size = new System.Drawing.Size(600, 450);
            this.pictureBoxIpl7.TabIndex = 12;
            this.pictureBoxIpl7.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2477, 911);
            this.Controls.Add(this.pictureBoxIpl7);
            this.Controls.Add(this.pictureBoxIpl6);
            this.Controls.Add(this.pictureBoxIpl5);
            this.Controls.Add(this.pictureBoxIpl4);
            this.Controls.Add(this.trackBar4);
            this.Controls.Add(this.pictureBoxIpl3);
            this.Controls.Add(this.trackBar3);
            this.Controls.Add(this.trackBar2);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.pictureBoxIpl2);
            this.Controls.Add(this.pictureBoxIpl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIpl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIpl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIpl3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIpl4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIpl5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIpl6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIpl7)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenCvSharp.UserInterface.PictureBoxIpl pictureBoxIpl1;
        private OpenCvSharp.UserInterface.PictureBoxIpl pictureBoxIpl2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.TrackBar trackBar2;
        private System.Windows.Forms.TrackBar trackBar3;
        private OpenCvSharp.UserInterface.PictureBoxIpl pictureBoxIpl3;
        private System.Windows.Forms.TrackBar trackBar4;
        private OpenCvSharp.UserInterface.PictureBoxIpl pictureBoxIpl4;
        private OpenCvSharp.UserInterface.PictureBoxIpl pictureBoxIpl5;
        private OpenCvSharp.UserInterface.PictureBoxIpl pictureBoxIpl6;
        private OpenCvSharp.UserInterface.PictureBoxIpl pictureBoxIpl7;
    }
}

