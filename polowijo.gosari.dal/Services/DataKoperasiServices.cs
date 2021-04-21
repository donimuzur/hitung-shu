using polowijo.gosari.dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polowijo.gosari.dal
{
    public class DataKoperasiServices : IDataKoperasiServices
    {
        private OleDbConnection conn;
        public DataKoperasiServices()
        {
            conn = DBConnection.connect();
        }
        public DataKoperasiDto InserOrUpdate(DataKoperasiDto Dto)
        {
            if(Dto.Id == 0)
            {
                Dto = Insert(Dto);     
            }
            else
            {
                Dto = Update(Dto);
            }
            return Dto;
        }
        public DataKoperasiDto Insert(DataKoperasiDto Dto)
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn = DBConnection.connect();
                using (OleDbCommand _command = new OleDbCommand())
                {
                    _command.CommandType = CommandType.Text;
                    _command.Connection = conn;
                    _command.CommandText = "INSERT INTO DATA_KOPERASI " +
                        "(tahun, total_shu, total_simpanan, total_penjualan, total_pinjaman, created_date, created_by, modified_date, modified_by) " +
                        "VALUES (@Tahun, @TotalShu, @TotalSimpanan, @TotalPenjualan, @TotalPinjaman, @created_date, @created_by, @modified_date, @modified_by);";
                    _command.Parameters.AddWithValue("@Tahun", Dto.Tahun);
                    _command.Parameters.AddWithValue("@TotalShu", Dto.TotalShu);
                    _command.Parameters.AddWithValue("@TotalSimpanan", Dto.TotalSimpanan);
                    _command.Parameters.AddWithValue("@TotalPenjualan", Dto.TotalPenjualan);
                    _command.Parameters.AddWithValue("@TotalPinjaman", Dto.TotalPinjaman);
                    _command.Parameters.AddWithValue("@created_date", DateTime.Today);
                    _command.Parameters.AddWithValue("@created_by", "Admin");
                    _command.Parameters.AddWithValue("@modified_date", DateTime.Today);
                    _command.Parameters.AddWithValue("@modified_by", "Admin");
                   
                    _command.ExecuteNonQuery();

                    string query2 = "Select @@Identity";
                    _command.CommandText = query2;
                    int ID = (int)_command.ExecuteScalar();

                    Dto.CreatedBy = "Admin";
                    Dto.CreatedDate = DateTime.Today;
                    Dto.ModifiedBy = "Admin";
                    Dto.ModifiedDate = DateTime.Today;
                    Dto.Id = ID;
                    return Dto;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DataKoperasiDto Update(DataKoperasiDto Dto)
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn = DBConnection.connect();
                var GetData = GetById(Dto.Id);

                using (OleDbCommand _command = new OleDbCommand())
                {
                    _command.CommandType = CommandType.Text;
                    _command.Connection = conn;
                    _command.CommandText = "Update DATA_KOPERASI " +
                        "SET tahun =@Tahun, total_shu =@TotalShu, total_simpanan =@TotalSimpanan, total_penjualan=@TotalPenjualan, total_pinjaman=@TotalPinjaman, created_date=@created_date, created_by=@created_by, modified_date=@modified_date, modified_by=@modified_by WHERE id = @Id;";
                    
                    _command.Parameters.AddWithValue("@Tahun", Dto.Tahun);
                    _command.Parameters.AddWithValue("@TotalShu", Dto.TotalShu);
                    _command.Parameters.AddWithValue("@TotalSimpanan", Dto.TotalSimpanan);
                    _command.Parameters.AddWithValue("@TotalPenjualan", Dto.TotalPenjualan);
                    _command.Parameters.AddWithValue("@TotalPinjaman", Dto.TotalPinjaman);
                    _command.Parameters.AddWithValue("@created_date", GetData.CreatedDate);
                    _command.Parameters.AddWithValue("@created_by", GetData.CreatedBy);
                    _command.Parameters.AddWithValue("@modified_date", DateTime.Today);
                    _command.Parameters.AddWithValue("@modified_by", "Admin");
                    _command.Parameters.AddWithValue("@Id", Dto.Id);

                    _command.ExecuteNonQuery();

                    Dto.ModifiedDate= DateTime.Today;
                    Dto.ModifiedBy = "Admin";

                    return Dto;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DataKoperasiDto GetByTahun (int Tahun)
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn = DBConnection.connect();
            DataKoperasiDto Dto = null;
            using (OleDbCommand _command = new OleDbCommand())
            {
                _command.CommandType = CommandType.Text;
                _command.Connection = conn;
                _command.CommandText = "Select * from DATA_KOPERASI WHERE TAHUN =  @TAHUN";
                _command.Parameters.AddWithValue("@TAHUN", Tahun);

                using (OleDbDataReader _dr = _command.ExecuteReader())
                {
                    DataTable _dt = new DataTable();
                    _dt.Load(_dr);

                    foreach (DataRow row in _dt.AsEnumerable())
                    {
                        Dto = new DataKoperasiDto();
                        Dto.Id = Convert.ToInt64(row["id"]);
                        Dto.Tahun = Convert.ToInt32(row["tahun"]);
                        Dto.TotalShu = Convert.ToDouble(row["total_shu"]);
                        Dto.TotalSimpanan = Convert.ToDouble(row["total_simpanan"]);
                        Dto.TotalPenjualan = Convert.ToDouble(row["total_penjualan"]);
                        Dto.TotalPinjaman = Convert.ToDouble(row["total_pinjaman"]);
                        Dto.CreatedBy = Convert.ToString(row["created_by"]);
                        Dto.CreatedDate = Convert.ToDateTime(row["created_date"]);
                        Dto.ModifiedDate= Convert.ToDateTime(row["modified_date"]);
                        Dto.ModifiedBy = Convert.ToString(row["modified_by"]);
                    }
                }
            }
            return Dto;
        }

        public DataKoperasiDto GetById(long Id)
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn = DBConnection.connect();
            DataKoperasiDto Dto = null;
            using (OleDbCommand _command = new OleDbCommand())
            {
                _command.CommandType = CommandType.Text;
                _command.Connection = conn;
                _command.CommandText = "Select * from DATA_KOPERASI WHERE ID =  @Id";
                _command.Parameters.AddWithValue("@Id", Id);

                using (OleDbDataReader _dr = _command.ExecuteReader())
                {
                    DataTable _dt = new DataTable();
                    _dt.Load(_dr);

                    foreach (DataRow row in _dt.AsEnumerable())
                    {
                        Dto = new DataKoperasiDto();
                        Dto.Id = Convert.ToInt64(row["id"]);
                        Dto.Tahun = Convert.ToInt32(row["tahun"]);
                        Dto.TotalShu = Convert.ToDouble(row["total_shu"]);
                        Dto.TotalSimpanan = Convert.ToDouble(row["total_simpanan"]);
                        Dto.TotalPenjualan = Convert.ToDouble(row["total_penjualan"]);
                        Dto.TotalPinjaman = Convert.ToDouble(row["total_pinjaman"]);
                        Dto.CreatedBy = Convert.ToString(row["created_by"]);
                        Dto.CreatedDate = Convert.ToDateTime(row["created_date"]);
                        Dto.ModifiedDate = Convert.ToDateTime(row["modified_date"]);
                        Dto.ModifiedBy = Convert.ToString(row["modified_by"]);
                    }
                }
            }
            return Dto;
        }
    }
}
