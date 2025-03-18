using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace QuanlyPhongKham1
{
    public partial class ucThemThuoc: UserControl
    {
        public string maHS;
        SqlConnection conn = null;
        string strCon = @"Data Source=LAPTOP-N4EKNJ67;Initial Catalog=QuanLyPhongKham;Integrated Security=True;";
        public ucThemThuoc()
        {
            InitializeComponent();
            this.maHS = maHS;
        }

        
        private void ucThemThuoc_Load(object sender, EventArgs e)
        {

        }

        private void btn_them_Click(object sender, EventArgs e)
        {
            if (TB_MaThuoc.Text == "" || TB_TenThuoc.Text == "" || TB_Soluong.Text == "" || TB_LieuDung.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
                return;
            }
            string ID = TB_MaThuoc.Text;
            string TenThuoc = TB_TenThuoc.Text;
            string SoLuong = TB_Soluong.Text;
            string LieuDung = TB_LieuDung.Text;
            DGV.Rows.Add(ID,  TenThuoc, SoLuong, LieuDung);
        }

        private void DienTenThuoc(object sender, EventArgs e)
        {
            TB_TenThuoc.Text = LayTenThuoc(TB_MaThuoc.Text);
        }
        private string LayTenThuoc(string maThuoc)
        {
            string tenThuoc = "";
            using (SqlConnection conn = new SqlConnection(strCon))
            {
                conn.Open();
                string query = "SELECT TenThuoc FROM Thuoc WHERE ID = @MaThuoc";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaThuoc", maThuoc);

                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    tenThuoc = result.ToString();
                }
            }
            return tenThuoc;
        }
        int i;
        private void DGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            i = e.RowIndex;
        }

        private void xóaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DGV.Rows.RemoveAt(i);
        }

        private void btn_xacNhan_Click(object sender, EventArgs e)
        {
            using (conn = new SqlConnection(strCon))
            {   
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    for (int i = 0; i < DGV.Rows.Count; i++)
                    {
                        string mathuoc = DGV.Rows[i].Cells[0].Value.ToString();
                        string idHS = maHS;
                        int soluong = int.Parse(DGV.Rows[i].Cells[1].Value.ToString().TrimEnd());
                        string lieuDung = DGV.Rows[i].Cells[2].Value.ToString();

                        string query = @"
                                        INSERT INTO DonThuoc (HoSoBenhAnID, ThuocID, SoLuong, LieuDung) 
                                        VALUES (@HoSoBenhAnID, @ThuocID, @SoLuong, @LieuDung)";
                        using (SqlCommand cmd = new SqlCommand(query, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@HoSoBenhAnID", maHS);
                            cmd.Parameters.AddWithValue("@ThuocID", mathuoc);
                            cmd.Parameters.AddWithValue("@SoLuong", soluong);
                            cmd.Parameters.AddWithValue("@LieuDung", lieuDung);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tran.Commit();
                    MessageBox.Show("Thêm thuốc thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {   
                    tran.Rollback();
                    MessageBox.Show("Lỗi khi thêm thuốc: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
