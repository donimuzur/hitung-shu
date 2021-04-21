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
    public class IptAnggotaServices : IIptAnggotaServices
    {
        private OleDbConnection conn;
        public IptAnggotaServices()
        {
            conn = DBConnection.connect();
        }
        public IptAnggotaDto InserOrUpdate(IptAnggotaDto Dto)
        {
            if (Dto.Id == 0)
            {
                Dto = Insert(Dto);
            }
            else
            {
                Dto = Update(Dto);
            }
            return Dto;
        }
        public List<IptAnggotaDto> GetAllByTahun(int Tahun)
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn = DBConnection.connect();
            var ListDto = new List<IptAnggotaDto>();

            using (OleDbCommand _command = new OleDbCommand())
            {
                _command.CommandType = CommandType.Text;
                _command.Connection = conn;
                _command.CommandText = "Select B.NAMA_ANGGOTA AS nama_anggota,  A.* from IPT_ANGGOTA A INNER JOIN DATA_ANGGOTA B ON A.ID_ANGGOTA = B.ID_ANGGOTA WHERE YEAR(A.tanggal) =  @TAHUN";
                _command.Parameters.AddWithValue("@TAHUN", Tahun);

                using (OleDbDataReader _dr = _command.ExecuteReader())
                {
                    DataTable _dt = new DataTable();
                    _dt.Load(_dr);

                    foreach (DataRow row in _dt.AsEnumerable())
                    {
                        var Dto = new IptAnggotaDto();

                        Dto.Id = Convert.ToInt64(row["id"]);
                        Dto.IdAnggota= Convert.ToString(row["id_anggota"]);
                        Dto.NamaAnggota = Convert.ToString(row["nama_anggota"]);
                        Dto.Tanggal= Convert.ToDateTime(row["tanggal"]);
                        Dto.Pokok = Convert.ToDouble(row["pokok"]);
                        Dto.Wajib = Convert.ToDouble(row["wajib"]);
                        Dto.Sukarela = Convert.ToDouble(row["sukarela"]);
                        Dto.Belanja = Convert.ToDouble(row["belanja"]);
                        Dto.BungaPinjaman = Convert.ToDouble(row["bunga_pinjaman"]);
                        Dto.CreatedBy = Convert.ToString(row["created_by"]);
                        Dto.CreatedDate = Convert.ToDateTime(row["created_date"]);
                        Dto.ModifiedDate = Convert.ToDateTime(row["modified_date"]);
                        Dto.ModifiedBy = Convert.ToString(row["modified_by"]);

                        ListDto.Add(Dto);
                    }
                }
            }
            return ListDto;
        }
        public List<IptAnggotaDto> GetAllByIdAnggotaDanTahun(string IdAnggota,int Tahun)
        {
            if(conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn = DBConnection.connect();
            var ListDto = new List<IptAnggotaDto>();

            using (OleDbCommand _command = new OleDbCommand())
            {
                _command.CommandType = CommandType.Text;
                _command.Connection = conn;
                _command.CommandText = "Select B.NAMA_ANGGOTA AS nama_anggota,  A.* from IPT_ANGGOTA A INNER JOIN DATA_ANGGOTA B ON A.ID_ANGGOTA = B.ID_ANGGOTA WHERE YEAR(A.tanggal) =  @TAHUN and A.id_anggota = @IdAnggota";
                _command.Parameters.AddWithValue("@TAHUN", Tahun);
                _command.Parameters.AddWithValue("@IdAnggota", IdAnggota);

                using (OleDbDataReader _dr = _command.ExecuteReader())
                {
                    DataTable _dt = new DataTable();
                    _dt.Load(_dr);

                    foreach (DataRow row in _dt.AsEnumerable())
                    {
                        var Dto = new IptAnggotaDto();

                        Dto.Id = Convert.ToInt64(row["id"]);
                        Dto.IdAnggota = Convert.ToString(row["id_anggota"]);
                        Dto.NamaAnggota = Convert.ToString(row["nama_anggota"]);
                        Dto.Tanggal = Convert.ToDateTime(row["tanggal"]);
                        Dto.Pokok = Convert.ToDouble(row["pokok"]);
                        Dto.Wajib = Convert.ToDouble(row["wajib"]);
                        Dto.Sukarela = Convert.ToDouble(row["sukarela"]);
                        Dto.Belanja = Convert.ToDouble(row["belanja"]);
                        Dto.BungaPinjaman = Convert.ToDouble(row["bunga_pinjaman"]);
                        Dto.CreatedBy = Convert.ToString(row["created_by"]);
                        Dto.CreatedDate = Convert.ToDateTime(row["created_date"]);
                        Dto.ModifiedDate = Convert.ToDateTime(row["modified_date"]);
                        Dto.ModifiedBy = Convert.ToString(row["modified_by"]);

                        ListDto.Add(Dto);
                    }
                }
            }
            return ListDto;
        }
        public IptAnggotaDto GetById(long Id)
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn = DBConnection.connect();
            IptAnggotaDto Dto = null;
            using (OleDbCommand _command = new OleDbCommand())
            {
                _command.CommandType = CommandType.Text;
                _command.Connection = conn;
                _command.CommandText = "Select B.NAMA_ANGGOTA AS nama_anggota,  A.* from IPT_ANGGOTA A INNER JOIN DATA_ANGGOTA B ON A.ID_ANGGOTA = B.ID_ANGGOTA  WHERE A.ID = @Id";
                _command.Parameters.AddWithValue("@Id", Id);

                using (OleDbDataReader _dr = _command.ExecuteReader())
                {
                    DataTable _dt = new DataTable();
                    _dt.Load(_dr);

                    foreach (DataRow row in _dt.AsEnumerable())
                    {
                        Dto = new IptAnggotaDto();
                        Dto.Id = Convert.ToInt64(row["id"]);
                        Dto.IdAnggota = Convert.ToString(row["id_anggota"]);
                        Dto.NamaAnggota = Convert.ToString(row["nama_anggota"]);
                        Dto.Tanggal = Convert.ToDateTime(row["tanggal"]);
                        Dto.Pokok = Convert.ToDouble(row["pokok"]);
                        Dto.Wajib = Convert.ToDouble(row["wajib"]);
                        Dto.Sukarela = Convert.ToDouble(row["sukarela"]);
                        Dto.Belanja = Convert.ToDouble(row["belanja"]);
                        Dto.BungaPinjaman = Convert.ToDouble(row["bunga_pinjaman"]);
                        Dto.CreatedBy = Convert.ToString(row["created_by"]);
                        Dto.CreatedDate = Convert.ToDateTime(row["created_date"]);
                        Dto.ModifiedDate = Convert.ToDateTime(row["modified_date"]);
                        Dto.ModifiedBy = Convert.ToString(row["modified_by"]);
                    }
                }
            }
            return Dto;
        }

        public IptAnggotaDto GetByIdAnggota(string IdAnggota)
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn = DBConnection.connect();
            IptAnggotaDto Dto = null;

            using (OleDbCommand _command = new OleDbCommand())
            {
                _command.CommandType = CommandType.Text;
                _command.Connection = conn;
                _command.CommandText = "Select B.NAMA_ANGGOTA AS nama_anggota,  A.* from IPT_ANGGOTA A INNER JOIN DATA_ANGGOTA B ON A.ID_ANGGOTA = B.ID_ANGGOTA  WHERE A.id_anggota =  @IdAnggota";
                _command.Parameters.AddWithValue("@IdAnggota", IdAnggota);

                using (OleDbDataReader _dr = _command.ExecuteReader())
                {
                    DataTable _dt = new DataTable();
                    _dt.Load(_dr);

                    foreach (DataRow row in _dt.AsEnumerable())
                    {
                        Dto = new IptAnggotaDto();

                        Dto.Id = Convert.ToInt64(row["id"]);
                        Dto.IdAnggota = Convert.ToString(row["id_anggota"]);
                        Dto.NamaAnggota = Convert.ToString(row["nama_anggota"]);
                        Dto.Tanggal = Convert.ToDateTime(row["tanggal"]);
                        Dto.Pokok = Convert.ToDouble(row["pokok"]);
                        Dto.Wajib = Convert.ToDouble(row["wajib"]);
                        Dto.Sukarela = Convert.ToDouble(row["sukarela"]);
                        Dto.Belanja = Convert.ToDouble(row["belanja"]);
                        Dto.BungaPinjaman = Convert.ToDouble(row["bunga_pinjaman"]);
                        Dto.CreatedBy = Convert.ToString(row["created_by"]);
                        Dto.CreatedDate = Convert.ToDateTime(row["created_date"]);
                        Dto.ModifiedDate = Convert.ToDateTime(row["modified_date"]);
                        Dto.ModifiedBy = Convert.ToString(row["modified_by"]);
                    }
                }
            }
            return Dto;
        }

        public IptAnggotaDto Insert(IptAnggotaDto Dto)
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn = DBConnection.connect();
            try
            {
                using (OleDbCommand _command = new OleDbCommand())
                {
                    _command.CommandType = CommandType.Text;
                    _command.Connection = conn;
                    _command.CommandText = "INSERT INTO IPT_ANGGOTA " +
                        "(id_anggota, tanggal, pokok, wajib, sukarela, belanja, bunga_pinjaman, created_date, created_by, modified_date, modified_by) " +
                        "VALUES (@IdAnggota, @Tanggal, @Pokok, @Wajib, @Sukarela, @Belanja, @BungaPinjaman, @created_date, @created_by, @modified_date, @modified_by);";
                    _command.Parameters.AddWithValue("@IdAnggota", Dto.IdAnggota);
                    _command.Parameters.AddWithValue("@Tanggal", Dto.Tanggal);
                    _command.Parameters.AddWithValue("@Pokok", Dto.Pokok);
                    _command.Parameters.AddWithValue("@Wajib", Dto.Wajib);
                    _command.Parameters.AddWithValue("@Sukarela", Dto.Sukarela);
                    _command.Parameters.AddWithValue("@Belanja", Dto.Belanja);
                    _command.Parameters.AddWithValue("@BungaPinjaman", Dto.BungaPinjaman);
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
        public bool InsertBulk (List<IptAnggotaDto> ListDto)
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn = DBConnection.connect();
            try
            {
                using (OleDbCommand _command = new OleDbCommand())
                {

                    using (var trans = conn.BeginTransaction())
                    {
                        try
                        {
                            _command.Transaction = trans;
                            _command.CommandType = CommandType.Text;
                            _command.Connection = conn;

                            foreach (var Dto in ListDto)
                            {
                                string Message = "('" + Dto.IdAnggota + "','" + Dto.Tanggal + "'," + Dto.Pokok + "," + Dto.Wajib + "," + Dto.Sukarela + "," + Dto.Belanja + "," + Dto.BungaPinjaman + ",@created_date, @created_by, @modified_date, @modified_by)";
                                _command.CommandText = "INSERT INTO IPT_ANGGOTA " +
                              "(id_anggota, tanggal, pokok, wajib, sukarela, belanja, bunga_pinjaman, created_date, created_by, modified_date, modified_by) " +
                              "VALUES " + Message;

                                _command.Parameters.AddWithValue("@created_date", DateTime.Today);
                                _command.Parameters.AddWithValue("@created_by", "Admin");
                                _command.Parameters.AddWithValue("@modified_date", DateTime.Today);
                                _command.Parameters.AddWithValue("@modified_by", "Admin");
                                _command.ExecuteNonQuery();
                                
                            }
                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            throw;
                            /* log exception and the fact that rollback succeeded */
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IptAnggotaDto Update(IptAnggotaDto Dto)
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn = DBConnection.connect();
            try
            {
                var GetData = GetById(Dto.Id);

                using (OleDbCommand _command = new OleDbCommand())
                {
                    _command.CommandType = CommandType.Text;
                    _command.Connection = conn;
                    _command.CommandText = "UPDATE IPT_ANGGOTA " +
                        "SET id_anggota = @IdAnggota, tanggal=@Tanggal, pokok = @Pokok, wajib=@Wajib, sukarela=@Sukarela, belanja=@Belanja, bunga_pinjaman=@BungaPinjaman, created_date=@created_date, created_by=@created_by, modified_date=@modified_date, modified_by=@modified_by where id=@Id;";

                    _command.Parameters.AddWithValue("@IdAnggota", Dto.IdAnggota);
                    _command.Parameters.AddWithValue("@Tanggal", Dto.Tanggal);
                    _command.Parameters.AddWithValue("@Pokok", Dto.Pokok);
                    _command.Parameters.AddWithValue("@Wajib", Dto.Wajib);
                    _command.Parameters.AddWithValue("@Sukarela", Dto.Sukarela);
                    _command.Parameters.AddWithValue("@Belanja", Dto.Belanja);
                    _command.Parameters.AddWithValue("@BungaPinjaman", Dto.BungaPinjaman);
                    _command.Parameters.AddWithValue("@created_date", GetData.CreatedDate);
                    _command.Parameters.AddWithValue("@created_by", GetData.CreatedBy);
                    _command.Parameters.AddWithValue("@modified_date", DateTime.Today);
                    _command.Parameters.AddWithValue("@modified_by", "Admin");
                    _command.Parameters.AddWithValue("@Id", Dto.Id);

                    _command.ExecuteNonQuery();

                    Dto.ModifiedDate = DateTime.Today;
                    Dto.ModifiedBy = "Admin";

                    
                }
                return Dto;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Delete(long Id)
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
                    _command.CommandText = "DELETE FROM IPT_ANGGOTA WHERE id = @Id ;";
                    _command.Parameters.AddWithValue("@Id", Id);
                    _command.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
