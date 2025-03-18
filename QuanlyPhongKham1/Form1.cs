using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; 
using System.Globalization;
//using System.Drawing.Design;
using System.Data.SqlTypes;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace QuanlyPhongKham1
{
    
    public partial class Form1 : Form
    { private QuanLyHoSoBenhAn qlhs = new QuanLyHoSoBenhAn();
        string strCon = @"Data Source=LAPTOP-N4EKNJ67;Initial Catalog=QuanLyPhongKham;Integrated Security=True;";
        SqlConnection sqlCon = null;
        public Form1()
        {   
            
            InitializeComponent();
            LoadLichKham(Liv_LK);
            LoadLichKham(Liv_LK3);
            LoadLichKham(Liv_LK2);
            LoadHSBA();
        }
        #region
        //Hàm gọi danh sách

        private void Form1_Load(object sender, EventArgs e)
        {
            ResizeColumns(Liv_LK);
            ResizeColumns(Liv_LK2);
            ResizeColumns(Liv_LK3);
        }
        private void resize(object sender, EventArgs e)
        {
            ListView lv = (ListView)sender;
            ResizeColumns(lv);
        }
        void ResizeColumns(ListView listView)
        {
            if (listView.Columns.Count == 0) return;

            int totalWidth = listView.ClientSize.Width; // Lấy chiều rộng khả dụng của ListView
            int columnCount = listView.Columns.Count;
            int remainingWidth = totalWidth;

            for (int i = 0; i < columnCount - 1; i++)
            {
                listView.Columns[i].Width = totalWidth / columnCount;
                remainingWidth -= listView.Columns[i].Width;
            }

            // Cột cuối cùng lấy phần còn lại để không có khoảng trắng dư
            listView.Columns[columnCount - 1].Width = remainingWidth;
        }
        private void LoadLichKham(ListView listView)
        {
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }

            // Truy vấn để lấy thông tin lịch khám cùng tên bệnh nhân và bác sĩ
            string query = @"
                            SELECT lk.ID, bn.ID, bn.HoTen AS TenBenhNhan, 
                                   bs.ID, bs.HoTen AS TenBacSi, 
                                   lk.NgayKham, lk.GioKham, lk.TrangThai
                            FROM LichKham lk
                            JOIN BenhNhan bn ON lk.BenhNhanID = bn.ID
                            JOIN BacSi bs ON lk.BacSiID = bs.ID";

            SqlCommand Sqlcmd = new SqlCommand(query, sqlCon);
            SqlDataReader reader = Sqlcmd.ExecuteReader();
            listView.Items.Clear(); // Xóa danh sách cũ để tránh trùng lặp

            while (reader.Read())
            {
                int MaLK = reader.GetInt32(0);
                int MaBN = reader.GetInt32(1);
                string TenBenhNhan = reader.GetString(2);
                int MaBS = reader.GetInt32(3);
                string TenBacSi = reader.GetString(4);
                string NgayKham = reader.GetDateTime(5).ToString("dd/MM/yyyy");
                string GioKham = reader.GetTimeSpan(6).ToString(@"hh\:mm");
                string TrangThai = reader.GetString(7);

                ListViewItem liv = new ListViewItem(MaLK.ToString());
                liv.SubItems.Add(MaBN.ToString());
                liv.SubItems.Add(TenBenhNhan);  
                liv.SubItems.Add(MaBS.ToString());
                liv.SubItems.Add(TenBacSi);
                liv.SubItems.Add(NgayKham);
                liv.SubItems.Add(GioKham);
                if(TrangThai == "Đã khám")
                {
                    liv.BackColor = Color.LightGreen;
                }
                else if (TrangThai == "Đang chờ")
                {
                    liv.BackColor = Color.Yellow;
                }
                else if (TrangThai == "Hủy")
                {
                    liv.BackColor = Color.PaleVioletRed;
                }
                    liv.SubItems.Add(TrangThai);
                listView.Items.Add(liv);
            }
            reader.Close();
        }


        private void LoadDGV_LK(DataGridView dgv)
        {
            using (sqlCon = new SqlConnection(strCon))
            {
                sqlCon.Open();
                string query = @"
                    SELECT lk.ID AS MaLichKham, bn.ID AS MaBenhNhan, bn.HoTen AS TenBenhNhan, 
                           bs.ID AS MaBacSi, bs.HoTen AS TenBacSi, 
                           lk.NgayKham, lk.GioKham, lk.TrangThai
                    FROM LichKham lk
                    JOIN BenhNhan bn ON lk.BenhNhanID = bn.ID
                    JOIN BacSi bs ON lk.BacSiID = bs.ID";

                using (SqlCommand cmd = new SqlCommand(query, sqlCon))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgv.Rows.Clear(); // Xóa dữ liệu cũ trước khi tải dữ liệu mới

                    foreach (DataRow row in dt.Rows)
                    {
                        dgv.Rows.Add(
                            row["MaLichKham"].ToString(),
                            row["MaBenhNhan"].ToString(),
                            row["TenBenhNhan"].ToString(),
                            row["MaBacSi"].ToString(),
                            row["TenBacSi"].ToString(),
                            Convert.ToDateTime(row["NgayKham"]).ToString("dd/MM/yyyy"), // Định dạng ngày
                            row["GioKham"].ToString(),
                            row["TrangThai"].ToString()
                        );
                    }
                }
            }
        }




        private void btn_accept_Click(object sender, EventArgs e)
        {
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }
            try
            {   
                if( TB_IDBN.Text == "" || TB_IDBS.Text == "" || TB_GioKham.Text == "" || CB_TT.Text == "")
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
                    return;
                }
                SqlCommand Sqlcmd = new SqlCommand();
                Sqlcmd.CommandType = CommandType.Text;
                Sqlcmd.CommandText = "INSERT INTO LichKham (BenhNhanID, BacSiID, NgayKham, GioKham, TrangThai) VALUES (@MaBN, @MaBS, @NgayKham, @GioKham, @TrangThai)";

                Sqlcmd.Parameters.Add("@MaBN", SqlDbType.VarChar).Value = TB_IDBN.Text;
                Sqlcmd.Parameters.Add("@MaBS", SqlDbType.VarChar).Value = TB_IDBS.Text;

                DateTime ngaykham;
                if (DateTime.TryParse(DT_NgayKham.Text, out ngaykham))
                {
                    Sqlcmd.Parameters.Add("@NgayKham", SqlDbType.Date).Value = ngaykham;
                }
                else
                {
                    MessageBox.Show("Ngày không hợp lệ");
                    return;
                }
                TimeSpan time;
                if (TimeSpan.TryParse(TB_GioKham.Text, out time))
                {
                    Sqlcmd.Parameters.Add("@GioKham", SqlDbType.Time).Value = time;
                }
                else
                {
                    MessageBox.Show("Giờ không hợp lệ");
                    return;
                }
                Sqlcmd.Parameters.Add("@TrangThai", SqlDbType.NVarChar, 20).Value = CB_TT.Text;
                Sqlcmd.Connection = sqlCon;

                int result = Sqlcmd.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("Thêm thành công");
                    LoadLichKham(Liv_LK);
                    LoadLichKham(Liv_LK3);
                    LoadLichKham(Liv_LK2);
                }
                else
                {
                    MessageBox.Show("Thêm thất bại");
                }
            }
            catch(Exception)
            {
                MessageBox.Show($"Đã xảy ra lỗi vui lòng nhập lại");
            }
        }

        private void Liv_LK2_SelectedItems(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (Liv_LK2.SelectedItems.Count == 1)
            {
                ListViewItem item = Liv_LK2.SelectedItems[0];
                TB_FixIDLK.Text = item.SubItems[0].Text;
                TB_FixIDBN.Text = item.SubItems[1].Text;
                TB_FixIDBS.Text = item.SubItems[3].Text;
                //MessageBox.Show($"Giá trị ngày khám: {item.SubItems[5].Text}");
                DT_FixNgayKham.Value = DateTime.ParseExact(item.SubItems[5].Text,"dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                TB_FixGioKham.Text = item.SubItems[6].Text;
                CB_FixTT.Text = item.SubItems[7].Text;
            }
        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
            
            using (SqlCommand Sqlcmd = new SqlCommand())
            {
                Sqlcmd.CommandType = CommandType.Text;
                Sqlcmd.CommandText = "Update dbo.LichKham set BenhNhanID = @MaBN, BacSiID = @MaBS, NgayKham = @NgayKham, GioKham = @GioKham, TrangThai = @TrangThai where ID = @MaLK";

                Sqlcmd.Parameters.Add("@MaLK", SqlDbType.Int).Value = int.Parse(TB_FixIDLK.Text);
                Sqlcmd.Parameters.Add("@MaBN", SqlDbType.Int).Value = int.Parse(TB_FixIDBN.Text);
                Sqlcmd.Parameters.Add("@MaBS", SqlDbType.Int).Value = int.Parse(TB_FixIDBS.Text);

                if (DateTime.TryParse(DT_FixNgayKham.Text, out DateTime ngaykham))
                {
                    Sqlcmd.Parameters.Add("@NgayKham", SqlDbType.Date).Value = ngaykham;
                }
                else
                {
                    MessageBox.Show("Ngày không hợp lệ");
                    return;
                }

                if (TimeSpan.TryParse(TB_FixGioKham.Text, out TimeSpan time))
                {
                    Sqlcmd.Parameters.Add("@GioKham", SqlDbType.Time).Value = time;
                }
                else
                {
                    MessageBox.Show("Giờ không hợp lệ");
                    return;
                }

                // Kiểm tra trạng thái hợp lệ
                string[] trangThaiHopLe = { "Đang chờ", "Đã khám", "Hủy" };
                if (!trangThaiHopLe.Contains(CB_FixTT.Text))
                {
                    MessageBox.Show("Lỗi: Trạng thái không hợp lệ!");
                    return;
                }
                Sqlcmd.Parameters.Add("@TrangThai", SqlDbType.NVarChar).Value = CB_FixTT.Text;

                Sqlcmd.Connection = sqlCon;
                int result = Sqlcmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Sửa thành công");
                    LoadLichKham(Liv_LK);
                    LoadLichKham(Liv_LK3);
                    LoadLichKham(Liv_LK2);
                }
                else
                {
                    MessageBox.Show("Sửa thất bại");
                }
            }

        }


        // su kien Tiem kiem
        private void button1_Click(object sender, EventArgs e)
        {

            Search_LK();
        }
        private void Search_LK()
        {
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }

            string query = "SELECT * FROM LichKham WHERE 1=1";
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlCon;

            // Nếu người dùng nhập Mã bệnh nhân
            if (!string.IsNullOrEmpty(TB_SearchIDBN.Text))
            {
                query += " AND BenhNhanID = @MaBN";
                cmd.Parameters.Add("@MaBN", SqlDbType.VarChar).Value = TB_SearchIDBN.Text;
            }

            // Nếu người dùng nhập Mã bác sĩ
            if (!string.IsNullOrEmpty(TB_SearchIDBS.Text))
            {
                query += " AND BacSiID = @MaBS";
                cmd.Parameters.Add("@MaBS", SqlDbType.VarChar).Value = TB_SearchIDBS.Text;
            }

            // Nếu chọn ngày từ - đến
            if (DT_SearchDate_from.Value <= DT_SearchDate_to.Value && DT_SearchDate_from.Value.ToString() != " " && DT_SearchDate_to.Value.ToString() != " ")
            {
                query += " AND NgayKham BETWEEN @NgayTu AND @NgayDen";
                cmd.Parameters.Add("@NgayTu", SqlDbType.Date).Value = DT_SearchDate_from.Value;
                cmd.Parameters.Add("@NgayDen", SqlDbType.Date).Value = DT_SearchDate_to.Value;
            }

            // Nếu người dùng nhập Giờ khám
            if (!string.IsNullOrEmpty(TB_GioKham.Text))
            {
                try
                {
                    TimeSpan time = TimeSpan.Parse(TB_GioKham.Text);
                    query += " AND CONVERT(VARCHAR(5), GioKham, 108) = @GioKham"; // Chỉ lấy hh:mm
                    cmd.Parameters.Add("@GioKham", SqlDbType.Time).Value = time.ToString(@"hh\:mm");
                }
                catch (Exception)
                {
                    MessageBox.Show("Giờ không hợp lệ");
                    return;
                }
            }

            // Nếu chọn trạng thái
            if (CB_SearchTT.SelectedItem != null && CB_SearchTT.SelectedItem.ToString() != "")
            {
                query += " AND TrangThai = @TrangThai";
                cmd.Parameters.Add("@TrangThai", SqlDbType.NVarChar).Value = CB_SearchTT.SelectedItem.ToString();
            }

            cmd.CommandText = query;

            // Thực thi truy vấn và hiển thị kết quả
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            Liv_LK3.Items.Clear();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int MaLK = reader.GetInt32(0);
                int MaBN = reader.GetInt32(1);
                int MaBS = reader.GetInt32(2);
                string NgayKham = reader.GetDateTime(3).ToString("dd/MM/yyyy");
                string GioKham = reader.GetTimeSpan(4).ToString(@"hh\:mm");
                string TrangThai = reader.GetString(5);
                ListViewItem liv = new ListViewItem(MaLK.ToString());
                liv.SubItems.Add(MaBN.ToString());
                liv.SubItems.Add(MaBS.ToString());
                liv.SubItems.Add(NgayKham);
                liv.SubItems.Add(GioKham);
                if (TrangThai == "Đã khám")
                {
                    liv.BackColor = Color.LightGreen;
                }
                else if (TrangThai == "Đang chờ")
                {
                    liv.BackColor = Color.Yellow;
                }
                else if (TrangThai == "Hủy")
                {
                    liv.BackColor = Color.PaleVioletRed;
                }
                liv.SubItems.Add(TrangThai);
                Liv_LK3.Items.Add(liv);
            }
            reader.Close();
        }
        // Xử lý sự kiện khi thay đổi giá trị của DateTimePicker
        private void From_Change(object sender, EventArgs e)
        {
            DT_SearchDate_from.Format = DateTimePickerFormat.Custom; // Hiển thị dạng ngày
            DT_SearchDate_from.CustomFormat = "yyyy/MM/dd"; // Định dạng ngày
        }

        private void To_Change(object sender, EventArgs e)
        {
            DT_SearchDate_to.Format = DateTimePickerFormat.Custom;
            DT_SearchDate_to.CustomFormat = "yyyy/MM/dd";
        }

        private void click_reload(object sender, EventArgs e)
        {
            LoadLichKham(Liv_LK3);
        }

        private void XoaNgay(object sender, EventArgs e)
        {
            DT_SearchDate_from.Format = DateTimePickerFormat.Custom;
            DT_SearchDate_from.CustomFormat = " ";
            DT_SearchDate_to.Format = DateTimePickerFormat.Custom;
            DT_SearchDate_to.CustomFormat = " ";
        }

        private void Click_toolXoa(object sender, EventArgs e)
        {
            if (Liv_LK3.SelectedItems.Count == 1)
            {
                ListViewItem item = Liv_LK3.SelectedItems[0]; // lấy item đầu tiên được chọn
                string maLK = item.SubItems[0].Text; // Gán maLk = mã lịch khám của item đó
                if (MessageBox.Show($"Bạn có chắc chắn muốn xóa lịch khám: {maLK}không?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (sqlCon == null)
                    {
                        sqlCon = new SqlConnection(strCon);
                    }
                    if (sqlCon.State == ConnectionState.Closed)
                    {
                        sqlCon.Open();
                    }
                    SqlCommand cmd = new SqlCommand("DELETE FROM LichKham WHERE ID = @MaLK", sqlCon);
                    cmd.Parameters.Add("@MaLK", SqlDbType.Int).Value = int.Parse(maLK);
                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Xóa thành công");
                        LoadLichKham(Liv_LK3);
                        LoadLichKham(Liv_LK2);
                        LoadLichKham(Liv_LK);
                    }
                    else
                    {
                        MessageBox.Show("Xóa thất bại");
                    }
                }
            }
        }



        ////
        ////
        ///
        // Quản lý hồ sơ bệnh án

        private void LoadHSBA()
        {
            DataTable dt = qlhs.LayDanhSachHoSo();
            DGV_HSBA.Rows.Clear(); // Xóa các dòng cũ trước khi thêm mới

            foreach (DataRow row in dt.Rows)
            {
                // Thêm dữ liệu vào từng dòng của DataGridView
                DGV_HSBA.Rows.Add(
                    row["ID"].ToString(),              // Cột ID
                    row["BenhNhanID"].ToString(),      // Cột Mã Bệnh Nhân
                    row["HoTen"].ToString(),           // Cột Tên Bệnh Nhân
                    row["BacSiID"].ToString(),         // Cột Mã Bác Sĩ
                    row["HoTen1"].ToString(),          // Cột Tên Bác Sĩ (SQL trả về 2 HoTen, cần alias cho bác sĩ)
                    Convert.ToDateTime(row["NgayKham"]).ToString("dd/MM/yyyy"), // Định dạng ngày
                    row["ChuanDoan"].ToString()        // Cột Chẩn Đoán
                );
            }
        }


        // Xoá hồ sơ khỏi DB
        private void xoáToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (DGV_HSBA.Rows.Count > 0)
            {
                string id = DGV_HSBA.Rows[0].Cells["MaHS"].Value.ToString();
                DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xoá hồ sơ {id} không ?", "Xác nhận xoá", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    bool success = qlhs.XoaHoSo(id);
                    if (success)
                    {
                        MessageBox.Show("Xoá thành công");
                        LoadHSBA();
                    }
                    else
                    {
                        MessageBox.Show("Xoá thất bại.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn 1 hồ sơ để xoá");
            }
        }



        // Thêm hồ sơ vào DB

        // tự động lấy tên bệnh nhân
        private void AutoGetBN(object sender, EventArgs e)
        {
            TB_TenBNHS.Text = GetBn(TB_MaBNHS.Text.Trim());
        }

        private void AutoGetBNSHS (object sender, EventArgs e)
        {
            TB_TenBN_SHS.Text = GetBn(TB_MaBN_SHS.Text.Trim());
        }
        private string GetBn(string maBN)
        {
            if (!int.TryParse(maBN, out int maBNInt))
            {
                MessageBox.Show("Mã bệnh nhân không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
            using (SqlConnection sqlCon = new SqlConnection(strCon))
            {
                string query = "SELECT HoTen FROM BenhNhan WHERE ID = @MaBS";
                using (SqlCommand cmd = new SqlCommand(query, sqlCon))
                {
                    cmd.Parameters.Add("@MaBS", SqlDbType.Int).Value = maBNInt;
                    sqlCon.Open();
                    object result = cmd.ExecuteScalar();
                    return result?.ToString() ?? "";
                }
            }
            
        }

        // tự động lấy tên bác sĩ them
        private void AutoGetBS(object sender, EventArgs e)
        {
            TB_TenBSHS.Text = GetBs(TB_MaBSHS.Text.Trim());
        }
        // lay bs o sua
        private void AutoGetBSSHS(object sender, EventArgs e)
        {
            TB_TenBS_SHS.Text = GetBs(TB_MaBS_SHS.Text.Trim());
        }
        private string GetBs(string maBS)
        {
            if (!int.TryParse(maBS, out int maBSInt))
            {
                MessageBox.Show("Mã bác sĩ không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }

            using (SqlConnection sqlCon = new SqlConnection(strCon))
            {
                string query = "SELECT HoTen FROM BacSi WHERE ID = @MaBS";
                using (SqlCommand cmd = new SqlCommand(query, sqlCon))
                {
                    cmd.Parameters.Add("@MaBS", SqlDbType.Int).Value = maBSInt;
                    sqlCon.Open();
                    object result = cmd.ExecuteScalar();
                    return result?.ToString() ?? "";
                }
            }
        }


        // Sự kiện thêm Hồ sơ bệnh án 
        private void btn_themHS_Click(object sender, EventArgs e)
        {
            using (SqlConnection sqlCon = new SqlConnection(strCon))
            {
                string MaBN = TB_MaBNHS.Text.Trim();
                string MaBS = TB_MaBSHS.Text.Trim();
                DateTime HsNgayKham = DTP_ThemHS.Value;
                string HsChuanDoan = RTB_ChuanDoan.Text.Trim();
                qlhs.ThemHoSo(MaBN, MaBS, HsNgayKham, HsChuanDoan);
                LoadHSBA();
            }
        }

        private void AutoDien(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= DGV_HSBA.Rows.Count - 1) return;

            // Lấy giá trị từ DataGridView và kiểm tra null
            LB_MaHS.Text = DGV_HSBA.Rows[e.RowIndex].Cells[0].Value?.ToString() ?? "";
            TB_MaBN_SHS.Text = DGV_HSBA.Rows[e.RowIndex].Cells[1].Value?.ToString() ?? "";

            // Kiểm tra null trước khi gọi Trim()
            TB_TenBN_SHS.Text = !string.IsNullOrEmpty(TB_MaBN_SHS.Text) ? GetBn(TB_MaBN_SHS.Text.Trim()) : "";

            TB_MaBS_SHS.Text = DGV_HSBA.Rows[e.RowIndex].Cells[3].Value?.ToString() ?? "";
            TB_TenBS_SHS.Text = !string.IsNullOrEmpty(TB_MaBS_SHS.Text) ? GetBs(TB_MaBS_SHS.Text.Trim()) : "";

            // Chuyển đổi ngày khám an toàn hơn
            if (DateTime.TryParse(DGV_HSBA.Rows[e.RowIndex].Cells[5].Value?.ToString(), out DateTime NgayKham))
            {
                DTP_SuaHS.Value = NgayKham;
            }
            else
            {
                DTP_SuaHS.Value = DateTime.Now; // Giá trị mặc định nếu không hợp lệ
            }

            // Chẩn đoán
            RTB_ChuanDoan_SHS.Text = DGV_HSBA.Rows[e.RowIndex].Cells[6].Value?.ToString() ?? "";
        }



        private void btn_SuaHS_Click(object sender, EventArgs e)
        {
            using (SqlConnection sqlCon = new SqlConnection(strCon))
            {
                string MaHS = LB_MaHS.Text.Trim();
                string MaBN = TB_MaBN_SHS.Text.Trim();
                string MaBS = TB_MaBS_SHS.Text.Trim();
                DateTime HsNgayKham = DTP_SuaHS.Value;
                string HsChuanDoan = RTB_ChuanDoan_SHS.Text.Trim();
                if(MessageBox.Show($"Bạn có chắc muốn sửa hồ sơ mã {MaHS} không?", "Xác nhận sửa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {   
                    bool result = qlhs.SuaHoSo(MaHS, MaBN, MaBS, HsNgayKham, HsChuanDoan);
                    if (result)
                    {
                        MessageBox.Show("Sửa thành công");
                        LoadHSBA();
                    }
                    else
                    {
                        MessageBox.Show("Sửa thất bại");
                    }
                }
                
                   
            }
        }

        // Xuat file PDF hồ sơ bệnh án + đơn thuốc


        

        private void MaHS_Leave(object sender, EventArgs e)
        {
            string maHoSo = TB_MaHSPDF.Text.Trim();
            if (!string.IsNullOrEmpty(maHoSo))
            {
                DienThongTinHoSo(maHoSo);
                DienDanhSachThuoc(maHoSo);
            }
        }
        private void DienThongTinHoSo(string maHoSo)
        {
            
            using (SqlConnection conn = new SqlConnection(strCon))
            {
                conn.Open();
                string query = @"
                                SELECT hs.NgayKham, bn.HoTen AS TenBenhNhan, bn.ID AS MaBenhNhan, 
                                       bs.HoTen AS TenBacSi, bs.ID AS MaBacSi, hs.ChuanDoan
                                FROM HoSoBenhAn hs
                                INNER JOIN BenhNhan bn ON hs.BenhNhanID = bn.ID
                                INNER JOIN BacSi bs ON hs.BacSiID = bs.ID
                                WHERE hs.ID = @MaHoSo";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaHoSo", maHoSo);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    TB_NgayPDF.Text = Convert.ToDateTime(reader["NgayKham"]).ToString("dd/MM/yyyy");
                    TB_TenBNPDF.Text = reader["TenBenhNhan"].ToString();
                    TB_MaBNPDF.Text = reader["MaBenhNhan"].ToString();
                    TB_TenBSPDF.Text = reader["TenBacSi"].ToString();
                    TB_MaBSPDF.Text = reader["MaBacSi"].ToString();
                    RTB_CDPDF.Text = reader["ChuanDoan"].ToString();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy hồ sơ bệnh án!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                reader.Close();
            }

        }

        private void DienDanhSachThuoc(string maHoSo)
        {

            using (SqlConnection conn = new SqlConnection(strCon))
            {
                conn.Open();
                string query = @"
                                SELECT ROW_NUMBER() OVER (ORDER BY dt.ID) AS STT, 
                                       t.TenThuoc, dt.SoLuong, dt.LieuDung
                                FROM DonThuoc dt
                                INNER JOIN Thuoc t ON dt.ThuocID = t.ID
                                WHERE dt.HoSoBenhAnID = @MaHoSo";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                adapter.SelectCommand.Parameters.AddWithValue("@MaHoSo", maHoSo);

                DataTable dt = new DataTable();
                adapter.Fill(dt);
                DGV_Thuoc.Rows.Clear();
                if (dt.Rows.Count == 0)
                {
                    if(MessageBox.Show("Chua co don thuoc trong ho so cho benh nhan! \n Ban co muon them thuoc cho benh nhan khong?", "Canh Bao", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        string MaHS = TB_MaHSPDF.Text;
                        ucThemThuoc ucThemThuoc = new ucThemThuoc();
                        ucThemThuoc.maHS = MaHS;


                        tabPage8.Controls.Add(ucThemThuoc);

                        // Thiết lập kích thước panel
                        ucThemThuoc.Size = new Size(800, 600); // Điều chỉnh kích thước phù hợp

                        // Căn giữa panel trong tabPage8
                        int centerX = (tabPage8.Width - ucThemThuoc.Width) / 2;
                        int centerY = (tabPage8.Height - ucThemThuoc.Height) / 2;
                        ucThemThuoc.Location = new Point(centerX, centerY);

                        ucThemThuoc.BringToFront();
                    }
                    else
                    {
                        return;
                    }
                }
                foreach (DataRow dr in dt.Rows)
                {
                    DGV_Thuoc.Rows.Add(dr["STT"], dr["TenThuoc"], dr["SoLuong"], dr["LieuDung"]);
                }
            }
        }

        // Xử lý sự kiện khi click vào nút Xuất PDF
        

        private void Btn_Xuat_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TB_MaHSPDF.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập mã hồ sơ bệnh án!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maHoSo = TB_MaHSPDF.Text.Trim();
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf",
                FileName = $"DonThuoc_{maHoSo}.pdf"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                XuatDonThuocPDF(maHoSo, filePath);
            }
        }

        private void XuatDonThuocPDF(string maHoSo, string filePath)
        {
            try
            {
                Document doc = new Document(PageSize.A4);
                PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
                doc.Open();

                // Khai báo font hỗ trợ tiếng Việt
                string fontPath = @"C:\Windows\Fonts\times.ttf"; // Đảm bảo đường dẫn đúng
                BaseFont bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                iTextSharp.text.Font titleFont = new iTextSharp.text.Font(bf, 18, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font normalFont = new iTextSharp.text.Font(bf, 12, iTextSharp.text.Font.NORMAL);
                iTextSharp.text.Font headerFont = new iTextSharp.text.Font(bf, 13 , iTextSharp.text.Font.BOLD);

                // Thêm tiêu đề
                Paragraph title = new Paragraph("ĐƠN THUỐC\n\n", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                doc.Add(title);

                // Lấy thông tin hồ sơ bệnh án
                using (SqlConnection conn = new SqlConnection(strCon))
                {
                    conn.Open();
                    string query = @"
                                    SELECT hs.NgayKham, bn.HoTen AS TenBenhNhan, bn.ID AS MaBenhNhan, 
                                           bs.HoTen AS TenBacSi, bs.ID AS MaBacSi, hs.ChuanDoan
                                    FROM HoSoBenhAn hs
                                    INNER JOIN BenhNhan bn ON hs.BenhNhanID = bn.ID
                                    INNER JOIN BacSi bs ON hs.BacSiID = bs.ID
                                    WHERE hs.ID = @MaHoSo";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaHoSo", maHoSo);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        doc.Add(new Paragraph($"Ngày khám: {Convert.ToDateTime(reader["NgayKham"]).ToString("dd/MM/yyyy")}", normalFont));
                        doc.Add(new Paragraph($"Tên bệnh nhân: {reader["TenBenhNhan"]}", normalFont));
                        doc.Add(new Paragraph($"Mã bệnh nhân: {reader["MaBenhNhan"]}", normalFont));
                        doc.Add(new Paragraph($"Tên bác sĩ: {reader["TenBacSi"]}", normalFont));
                        doc.Add(new Paragraph($"Mã bác sĩ: {reader["MaBacSi"]}", normalFont));
                        doc.Add(new Paragraph($"Chẩn đoán: {reader["ChuanDoan"]}\n\n", normalFont));
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy hồ sơ bệnh án!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    reader.Close();
                }

                // Lấy danh sách thuốc
                PdfPTable table = new PdfPTable(4);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 10, 40, 20, 30 });

                // Thêm header bảng
                table.AddCell(new PdfPCell(new Phrase("STT", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Tên Thuốc", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Số Lượng", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Liều Dùng", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER });

                using (SqlConnection conn = new SqlConnection(strCon))
                {
                    conn.Open();
                    string query = @"
                                    SELECT ROW_NUMBER() OVER (ORDER BY dt.ID) AS STT, 
                                           t.TenThuoc, dt.SoLuong, dt.LieuDung
                                    FROM DonThuoc dt
                                    INNER JOIN Thuoc t ON dt.ThuocID = t.ID
                                    WHERE dt.HoSoBenhAnID = @MaHoSo";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaHoSo", maHoSo);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        table.AddCell(new PdfPCell(new Phrase(reader["STT"].ToString(), normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table.AddCell(new PdfPCell(new Phrase(reader["TenThuoc"].ToString(), normalFont)));
                        table.AddCell(new PdfPCell(new Phrase(reader["SoLuong"].ToString(), normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table.AddCell(new PdfPCell(new Phrase(reader["LieuDung"].ToString(), normalFont)));
                    }
                    reader.Close();
                }

                doc.Add(table);
                doc.Close();

                MessageBox.Show("Xuất đơn thuốc thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                System.Diagnostics.Process.Start(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất file PDF: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }







        #endregion

        private void ucThemThuoc1_Load(object sender, EventArgs e)
        {

        }

        private void ucThemThuoc1_Load_1(object sender, EventArgs e)
        {

        }
    }
}
