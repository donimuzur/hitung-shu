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
    public class DataAnggotaServices : IDataAnggotaServices
    {
        private OleDbConnection conn;

        public DataAnggotaServices()
        {
            conn = DBConnection.connect();
        }
        public DataAnggotaDto InserOrUpdate(DataAnggotaDto Dto)
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
        public DataAnggotaDto Insert(DataAnggotaDto Dto)
        {
            try
            {
                conn = DBConnection.connect();
                using (OleDbCommand _command = new OleDbCommand())
                {
                    _command.CommandType = CommandType.Text;
                    _command.Connection = conn;
                    _command.CommandText = "INSERT INTO DATA_ANGGOTA " +
                        "(id_anggota, nama_anggota, tanggal_gabung, status, created_date, created_by, modified_date, modified_by) " +
                        "VALUES (@IdAnggota, @NamaAnggota, @TanggalGabung, @Status, @created_date, @created_by, @modified_date, @modified_by);";
                    _command.Parameters.AddWithValue("@IdAnggota", Dto.IdAnggota);
                    _command.Parameters.AddWithValue("@NamaAnggota", Dto.NamaAnggota);
                    _command.Parameters.AddWithValue("@TanggalGabung", Dto.TanggalGabung);
                    _command.Parameters.AddWithValue("@Status", Dto.Status);
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
        public DataAnggotaDto Update(DataAnggotaDto Dto)
        {
            try
            {
                conn = DBConnection.connect();
                var GetData = GetById(Dto.Id);

                using (OleDbCommand _command = new OleDbCommand())
                {
                    _command.CommandType = CommandType.Text;
                    _command.Connection = conn;
                    _command.CommandText = "Update DATA_ANGGOTA " +
                        "SET id_anggota = @IdAnggota, nama_anggota =@NamaAnggota, tanggal_gabung=@TanggalGabung, status = @status, modified_date=@modified_date, modified_by=@modified_by WHERE id = @Id;";
                    _command.Parameters.AddWithValue("@Id", Dto.Id);
                    _command.Parameters.AddWithValue("@IdAnggota", Dto.IdAnggota);
                    _command.Parameters.AddWithValue("@NamaAnggota", Dto.NamaAnggota);
                    _command.Parameters.AddWithValue("@TanggalGabung", Dto.TanggalGabung);
                    _command.Parameters.AddWithValue("@status", Dto.Status);
                    _command.Parameters.AddWithValue("@modified_date", DateTime.Today);
                    _command.Parameters.AddWithValue("@modified_by", "Admin");
                    _command.ExecuteNonQuery();

                    Dto.ModifiedDate = DateTime.Today;
                    Dto.ModifiedBy = "Admin";

                    return Dto;
                }
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
                conn = DBConnection.connect();
                using (OleDbCommand _command = new OleDbCommand())
                {
                    _command.CommandType = CommandType.Text;
                    _command.Connection = conn;
                    _command.CommandText = "DELETE FROM DATA_ANGGOTA WHERE id = @Id ;";
                    _command.Parameters.AddWithValue("@Id",Id);
                    _command.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<DataAnggotaDto> GetAll()
        {
            try
            {
                conn = DBConnection.connect();
                List<DataAnggotaDto> ListDto = new List<DataAnggotaDto>();
                using (OleDbCommand _command = new OleDbCommand())
                {
                    _command.CommandType = CommandType.Text;
                    _command.Connection = conn;
                    _command.CommandText = "Select * from DATA_ANGGOTA";

                    using (OleDbDataReader _dr = _command.ExecuteReader())
                    {
                        DataTable _dt = new DataTable();
                        _dt.Load(_dr);

                        foreach (DataRow row in _dt.AsEnumerable())
                        {

                            var Dto = new DataAnggotaDto();
                            Dto.Id = Convert.ToInt64(row["id"]);
                            Dto.IdAnggota = Convert.ToString(row["id_anggota"]);
                            Dto.NamaAnggota = Convert.ToString(row["nama_anggota"]);
                            Dto.TanggalGabung = Convert.ToDateTime(row["tanggal_gabung"]);
                            Dto.Status = Convert.ToBoolean(row["status"]);
                            Dto.ModifiedBy = Convert.ToString(row["modified_by"]);
                            Dto.ModifiedDate = Convert.ToDateTime(row["modified_date"]);
                            Dto.CreatedBy = Convert.ToString(row["created_by"]);
                            Dto.CreatedDate = Convert.ToDateTime(row["created_date"]);

                            ListDto.Add(Dto);
                        }
                    }
                }
                return ListDto;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<DataAnggotaDto> GetAllActive()
        {
            try
            {
                conn = DBConnection.connect();
                List<DataAnggotaDto> ListDto = new List<DataAnggotaDto>();
                using (OleDbCommand _command = new OleDbCommand())
                {
                    _command.CommandType = CommandType.Text;
                    _command.Connection = conn;
                    _command.CommandText = "Select * from DATA_ANGGOTA WHERE STATUS = 1 ";

                    using (OleDbDataReader _dr = _command.ExecuteReader())
                    {
                        DataTable _dt = new DataTable();
                        _dt.Load(_dr);

                        foreach (DataRow row in _dt.AsEnumerable())
                        {

                            var Dto = new DataAnggotaDto();
                            Dto.Id = Convert.ToInt64(row["id"]);
                            Dto.IdAnggota = Convert.ToString(row["id"]);
                            Dto.NamaAnggota = Convert.ToString(row["id_anggota"]);
                            Dto.TanggalGabung = Convert.ToDateTime(row["tanggal_gabung"]);
                            Dto.Status = Convert.ToBoolean(row["status"]);
                            Dto.ModifiedBy = Convert.ToString(row["modified_by"]);
                            Dto.ModifiedDate = Convert.ToDateTime(row["modified_date"]);
                            Dto.CreatedBy = Convert.ToString(row["created_by"]);
                            Dto.CreatedDate = Convert.ToDateTime(row["created_date"]);

                            ListDto.Add(Dto);
                        }
                    }
                }
                return ListDto;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataAnggotaDto GetById(long Id)
        {
            try
            {
                conn = DBConnection.connect();
                DataAnggotaDto Dto = null;
                using (OleDbCommand _command = new OleDbCommand())
                {
                    _command.CommandType = CommandType.Text;
                    _command.Connection = conn;
                    _command.CommandText = "Select * from DATA_ANGGOTA WHERE ID =  @Id";
                    _command.Parameters.AddWithValue("@Id", Id);

                    using (OleDbDataReader _dr = _command.ExecuteReader())
                    {
                        DataTable _dt = new DataTable();
                        _dt.Load(_dr);

                        foreach (DataRow row in _dt.AsEnumerable())
                        {
                            Dto = new DataAnggotaDto();
                            Dto.Id = Convert.ToInt64(row["id"]);
                            Dto.IdAnggota = Convert.ToString(row["id_anggota"]);
                            Dto.NamaAnggota = Convert.ToString(row["nama_anggota"]);
                            Dto.TanggalGabung = Convert.ToDateTime(row["tanggal_gabung"]);
                            Dto.Status = Convert.ToBoolean(row["status"]);
                            Dto.CreatedBy = Convert.ToString(row["created_by"]);
                            Dto.CreatedDate = Convert.ToDateTime(row["created_date"]);
                            Dto.ModifiedDate = Convert.ToDateTime(row["modified_date"]);
                            Dto.ModifiedBy = Convert.ToString(row["modified_by"]);
                        }
                    }
                }
                return Dto;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataAnggotaDto GetByIdAnggota(string IdAnggota)
        {
            try
            {
                conn = DBConnection.connect();
                DataAnggotaDto Dto = null;
                using (OleDbCommand _command = new OleDbCommand())
                {
                    _command.CommandType = CommandType.Text;
                    _command.Connection = conn;
                    _command.CommandText = "Select * from DATA_ANGGOTA WHERE id_anggota =  @IdAnggota";
                    _command.Parameters.AddWithValue("@IdAnggota", IdAnggota);

                    using (OleDbDataReader _dr = _command.ExecuteReader())
                    {
                        DataTable _dt = new DataTable();
                        _dt.Load(_dr);

                        foreach (DataRow row in _dt.AsEnumerable())
                        {
                            Dto = new DataAnggotaDto();
                            Dto.Id = Convert.ToInt64(row["id"]);
                            Dto.IdAnggota = Convert.ToString(row["id_anggota"]);
                            Dto.NamaAnggota = Convert.ToString(row["nama_anggota"]);
                            Dto.TanggalGabung = Convert.ToDateTime(row["tanggal_gabung"]);
                            Dto.Status = Convert.ToBoolean(row["status"]);
                            Dto.CreatedBy = Convert.ToString(row["created_by"]);
                            Dto.CreatedDate = Convert.ToDateTime(row["created_date"]);
                            Dto.ModifiedDate = Convert.ToDateTime(row["modified_date"]);
                            Dto.ModifiedBy = Convert.ToString(row["modified_by"]);
                        }
                    }
                }
                return Dto;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
