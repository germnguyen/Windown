namespace QuanlyPhongKham1
{
    partial class ucThemThuoc
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_them = new System.Windows.Forms.Button();
            this.TB_TenThuoc = new System.Windows.Forms.TextBox();
            this.btn_xacNhan = new System.Windows.Forms.Button();
            this.TB_MaThuoc = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.TB_Soluong = new System.Windows.Forms.TextBox();
            this.TB_LieuDung = new System.Windows.Forms.TextBox();
            this.DGV = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TenThuoc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SoLuong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LieuDung = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CMS_Xoa = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.xóaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
            this.CMS_Xoa.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label2.Location = new System.Drawing.Point(42, 115);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Mã Thuốc: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label3.Location = new System.Drawing.Point(42, 172);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 20);
            this.label3.TabIndex = 3;
            this.label3.Text = "Tên Thuốc: ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Elephant", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(18, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(311, 30);
            this.label4.TabIndex = 4;
            this.label4.Text = "Thêm Thuốc Cho Hồ Sơ";
            // 
            // btn_them
            // 
            this.btn_them.BackColor = System.Drawing.Color.LightGreen;
            this.btn_them.Location = new System.Drawing.Point(389, 246);
            this.btn_them.Name = "btn_them";
            this.btn_them.Size = new System.Drawing.Size(148, 40);
            this.btn_them.TabIndex = 5;
            this.btn_them.Text = "Thêm vào bảng";
            this.btn_them.UseVisualStyleBackColor = false;
            this.btn_them.Click += new System.EventHandler(this.btn_them_Click);
            // 
            // TB_TenThuoc
            // 
            this.TB_TenThuoc.Location = new System.Drawing.Point(147, 170);
            this.TB_TenThuoc.Name = "TB_TenThuoc";
            this.TB_TenThuoc.ReadOnly = true;
            this.TB_TenThuoc.Size = new System.Drawing.Size(221, 22);
            this.TB_TenThuoc.TabIndex = 7;
            // 
            // btn_xacNhan
            // 
            this.btn_xacNhan.BackColor = System.Drawing.Color.LightGreen;
            this.btn_xacNhan.Location = new System.Drawing.Point(580, 246);
            this.btn_xacNhan.Name = "btn_xacNhan";
            this.btn_xacNhan.Size = new System.Drawing.Size(94, 40);
            this.btn_xacNhan.TabIndex = 8;
            this.btn_xacNhan.Text = "Xác nhận";
            this.btn_xacNhan.UseVisualStyleBackColor = false;
            this.btn_xacNhan.Click += new System.EventHandler(this.btn_xacNhan_Click);
            // 
            // TB_MaThuoc
            // 
            this.TB_MaThuoc.Location = new System.Drawing.Point(147, 113);
            this.TB_MaThuoc.Name = "TB_MaThuoc";
            this.TB_MaThuoc.Size = new System.Drawing.Size(221, 22);
            this.TB_MaThuoc.TabIndex = 9;
            this.TB_MaThuoc.Leave += new System.EventHandler(this.DienTenThuoc);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label1.Location = new System.Drawing.Point(42, 229);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 20);
            this.label1.TabIndex = 10;
            this.label1.Text = "Số Lượng:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label5.Location = new System.Drawing.Point(42, 286);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 20);
            this.label5.TabIndex = 11;
            this.label5.Text = "Liều Dùng:";
            // 
            // TB_Soluong
            // 
            this.TB_Soluong.Location = new System.Drawing.Point(147, 229);
            this.TB_Soluong.Name = "TB_Soluong";
            this.TB_Soluong.Size = new System.Drawing.Size(221, 22);
            this.TB_Soluong.TabIndex = 12;
            // 
            // TB_LieuDung
            // 
            this.TB_LieuDung.Location = new System.Drawing.Point(147, 286);
            this.TB_LieuDung.Name = "TB_LieuDung";
            this.TB_LieuDung.Size = new System.Drawing.Size(221, 22);
            this.TB_LieuDung.TabIndex = 13;
            // 
            // DGV
            // 
            this.DGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.TenThuoc,
            this.SoLuong,
            this.LieuDung});
            this.DGV.ContextMenuStrip = this.CMS_Xoa;
            this.DGV.GridColor = System.Drawing.SystemColors.ButtonHighlight;
            this.DGV.Location = new System.Drawing.Point(3, 314);
            this.DGV.Name = "DGV";
            this.DGV.ReadOnly = true;
            this.DGV.RowHeadersWidth = 51;
            this.DGV.RowTemplate.Height = 24;
            this.DGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV.Size = new System.Drawing.Size(690, 305);
            this.DGV.TabIndex = 14;
            this.DGV.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_CellContentClick);
            // 
            // ID
            // 
            this.ID.HeaderText = "Mã Thuốc";
            this.ID.MinimumWidth = 6;
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            // 
            // TenThuoc
            // 
            this.TenThuoc.HeaderText = "Tên Thuốc";
            this.TenThuoc.MinimumWidth = 6;
            this.TenThuoc.Name = "TenThuoc";
            this.TenThuoc.ReadOnly = true;
            // 
            // SoLuong
            // 
            this.SoLuong.HeaderText = "Số Lượng";
            this.SoLuong.MinimumWidth = 6;
            this.SoLuong.Name = "SoLuong";
            this.SoLuong.ReadOnly = true;
            // 
            // LieuDung
            // 
            this.LieuDung.HeaderText = "Liều Dùng";
            this.LieuDung.MinimumWidth = 6;
            this.LieuDung.Name = "LieuDung";
            this.LieuDung.ReadOnly = true;
            // 
            // CMS_Xoa
            // 
            this.CMS_Xoa.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.CMS_Xoa.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.xóaToolStripMenuItem});
            this.CMS_Xoa.Name = "CMS_Xoa";
            this.CMS_Xoa.Size = new System.Drawing.Size(105, 28);
            // 
            // xóaToolStripMenuItem
            // 
            this.xóaToolStripMenuItem.Name = "xóaToolStripMenuItem";
            this.xóaToolStripMenuItem.Size = new System.Drawing.Size(104, 24);
            this.xóaToolStripMenuItem.Text = "Xóa";
            this.xóaToolStripMenuItem.Click += new System.EventHandler(this.xóaToolStripMenuItem_Click);
            // 
            // ucThemThuoc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DGV);
            this.Controls.Add(this.TB_LieuDung);
            this.Controls.Add(this.TB_Soluong);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TB_MaThuoc);
            this.Controls.Add(this.btn_xacNhan);
            this.Controls.Add(this.TB_TenThuoc);
            this.Controls.Add(this.btn_them);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Name = "ucThemThuoc";
            this.Size = new System.Drawing.Size(696, 622);
            this.Load += new System.EventHandler(this.ucThemThuoc_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
            this.CMS_Xoa.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_them;
        private System.Windows.Forms.TextBox TB_TenThuoc;
        private System.Windows.Forms.Button btn_xacNhan;
        private System.Windows.Forms.TextBox TB_MaThuoc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TB_Soluong;
        private System.Windows.Forms.TextBox TB_LieuDung;
        private System.Windows.Forms.DataGridView DGV;
        private System.Windows.Forms.ContextMenuStrip CMS_Xoa;
        private System.Windows.Forms.ToolStripMenuItem xóaToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn TenThuoc;
        private System.Windows.Forms.DataGridViewTextBoxColumn SoLuong;
        private System.Windows.Forms.DataGridViewTextBoxColumn LieuDung;
    }
}
