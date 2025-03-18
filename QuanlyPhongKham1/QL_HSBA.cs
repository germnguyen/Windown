using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
namespace QuanlyPhongKham1
{
    class QuanLyHoSoBenhAn
    {
        private string strCon = @"Data Source=LAPTOP-N4EKNJ67;Initial Catalog=QuanLyPhongKham;Integrated Security=True;";
        SqlConnection conn;
        public QuanLyHoSoBenhAn()
        {
            conn = new SqlConnection(strCon);
        }
        public DataTable LayDanhSachHoSo()
        {
            if(conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            string query = @"
                            SELECT hs.ID, hs.BenhNhanID, bn.HoTen, hs.BacSiID, bs.HoTen, hs.NgayKham, hs.ChuanDoan
                            FROM HoSoBenhAn hs
                            INNER JOIN BacSi bs ON hs.BacSiID = bs.ID
                            INNER JOIN BenhNhan bn ON hs.BenhNhanID = bn.ID";
            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }
        public void ThemHoSo( string MaBN, string MaBS, DateTime NgayKham , string ChuanDoan)
        {
            using (SqlConnection conn = new SqlConnection(strCon))
            {
                string query = "INSERT INTO HoSoBenhAn (BenhNhanID, BacSiID, NgayKham, ChuanDoan) VALUES (@MaBN, @MaBS, @NgayKham, @ChuanDoan)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaBN", MaBN);
                cmd.Parameters.AddWithValue("@MaBS", MaBS);
                cmd.Parameters.AddWithValue("@NgayKham", NgayKham);
                cmd.Parameters.Add("@ChuanDoan", SqlDbType.NVarChar, 20).Value = ChuanDoan;
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("Thêm Thành Công");
                    LayDanhSachHoSo();
                }
                else
                {
                    MessageBox.Show("Thêm Thất bại");
                }
            }
        }
        public bool SuaHoSo(string id ,string MaBN, string MaBS, DateTime NgayKham, string ChuanDoan)
        {
            using (SqlConnection conn = new SqlConnection(strCon))
            {
                string query = "UPDATE HoSoBenhAn SET BenhNhanID = @MaBN, BacSiID = @MaBS, NgayKham = @NgayKham, ChuanDoan = @ChuanDoan WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand (query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@MaBN", MaBN);
                cmd.Parameters.AddWithValue("@MaBS", MaBS);
                cmd.Parameters.AddWithValue("@NgayKham", NgayKham);
                cmd.Parameters.Add("@ChuanDoan", SqlDbType.NVarChar, 1000).Value = ChuanDoan;
                conn.Open();
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }
        public bool XoaHoSo(string id)
        {
            using (SqlConnection conn = new SqlConnection(strCon))
            {
                string query = "DELETE FROM HoSoBenhAn WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                conn.Open();
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }
    }
}
