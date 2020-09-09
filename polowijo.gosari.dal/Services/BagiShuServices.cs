using polowijo.gosari.dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace polowijo.gosari.dal
{
    public class BagiShuServices : IBagiShuServices
    {
        private OleDbConnection conn;
        private IDataKoperasiServices _dataKoperasiServices;
        private IDataAnggotaServices _dataAnggotaServices;
        private IptAnggotaServices _iptAnggotaServices;

        public BagiShuServices()
        {
            conn = DBConnection.connect();
            _dataKoperasiServices = new DataKoperasiServices();
            _dataAnggotaServices = new DataAnggotaServices();
            _iptAnggotaServices = new IptAnggotaServices();
        }
        public void Insert(BagiShuDto Dto)
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
                _command.CommandText = "DELETE FROM BAGI_SHU WHERE periode=@Periode; ";                    
                _command.Parameters.AddWithValue("@periode", Dto.Periode);
                _command.ExecuteNonQuery();

                _command.Connection = conn;
                _command.CommandText = "INSERT INTO BAGI_SHU " +
                    "(id_anggota, JMA, JUA, total_SHU, total_simpanan, total_belanja, total_bungapinjaman, periode, created_date, created_by, modified_date, modified_by,status) " +
                    "VALUES (@id_anggota, @JMA, @JUA, @JPA, @total_SHU, @total_simpanan, @total_belanja, @total_bungapinjaman, @periode, @created_date, @created_by, @modified_date, @modified_by, @status);";
                _command.Parameters.AddWithValue("@id_anggota", Dto.IdAnggota);
                _command.Parameters.AddWithValue("@JMA", Dto.Jma);
                _command.Parameters.AddWithValue("@JUA", Dto.Jua);
                _command.Parameters.AddWithValue("@JPA", Dto.Jpa);
                _command.Parameters.AddWithValue("@total_SHU", Dto.TotalShu);
                _command.Parameters.AddWithValue("@total_simpanan", Dto.TotalSimpanan);
                _command.Parameters.AddWithValue("@total_belanja", Dto.TotalBelanja);
                _command.Parameters.AddWithValue("@total_bungapinjaman", Dto.TotalBungaPinjaman);
                _command.Parameters.AddWithValue("@periode", Dto.Periode);
                _command.Parameters.AddWithValue("@created_date", DateTime.Now);
                _command.Parameters.AddWithValue("@created_by", "Admin");
                _command.Parameters.AddWithValue("@modified_date", DateTime.Now);
                _command.Parameters.AddWithValue("@modified_by", "Admin");
                _command.Parameters.AddWithValue("@status", true);
                _command.ExecuteNonQuery();
            }
        }
        public List<BagiShuDto> GetAllByTahun(int Tahun)
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn = DBConnection.connect();
            List<BagiShuDto> ListDto = new List<BagiShuDto>();
            using (OleDbCommand _command = new OleDbCommand())
            {
                _command.CommandType = CommandType.Text;
                _command.Connection = conn;
                _command.CommandText = "Select * from BAGI_SHU where periode=@Periode";

                using (OleDbDataReader _dr = _command.ExecuteReader())
                {
                    DataTable _dt = new DataTable();
                    _dt.Load(_dr);

                    foreach (DataRow row in _dt.AsEnumerable())
                    {
                        var Dto = new BagiShuDto();
                        Dto.Id = Convert.ToInt64(row["id"]);
                        Dto.IdAnggota = Convert.ToString(row["id_anggota"]);
                        Dto.NamaAnggota = Convert.ToString(row["nama_anggota"]);
                        Dto.Jma = Convert.ToDecimal(row["JMA"]);
                        Dto.Jua = Convert.ToDecimal(row["JUA"]);
                        Dto.Jpa = Convert.ToDecimal(row["JPA"]);
                        Dto.ModifiedBy = Convert.ToString(row["modified_by"]);
                        Dto.ModifiedDate = Convert.ToDateTime(row["modified_date"]);
                        Dto.CreatedBy = Convert.ToString(row["created_by"]);
                        Dto.CreatedDate = Convert.ToDateTime(row["created_date"]);
                        Dto.Periode = Convert.ToInt32(row["periode"]);
                        Dto.Status = Convert.ToBoolean(row["status"]);
                        Dto.TotalShu = Convert.ToInt64(row["total_SHU"]);

                        ListDto.Add(Dto);
                    }
                }
            }
            return ListDto;
        }

        public List<BagiShuDto> GetAll()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn = DBConnection.connect();
            List<BagiShuDto> ListDto = new List<BagiShuDto>();
            using (OleDbCommand _command = new OleDbCommand())
            {
                _command.CommandType = CommandType.Text;
                _command.Connection = conn;
                _command.CommandText = "Select * from BAGI_SHU ";

                using (OleDbDataReader _dr = _command.ExecuteReader())
                {
                    DataTable _dt = new DataTable();
                    _dt.Load(_dr);

                    foreach (DataRow row in _dt.AsEnumerable())
                    {
                        var Dto = new BagiShuDto();
                        Dto.Id = Convert.ToInt64(row["id"]);
                        Dto.IdAnggota = Convert.ToString(row["id_anggota"]);
                        Dto.NamaAnggota = Convert.ToString(row["nama_anggota"]);
                        Dto.Jma = Convert.ToDecimal(row["JMA"]); 
                        Dto.Jua = Convert.ToDecimal(row["JUA"]);
                        Dto.Jpa = Convert.ToDecimal(row["JPA"]);
                        Dto.ModifiedBy = Convert.ToString(row["modified_by"]); 
                        Dto.ModifiedDate = Convert.ToDateTime(row["modified_date"] );
                        Dto.CreatedBy = Convert.ToString(row["created_by"]);  
                        Dto.CreatedDate = Convert.ToDateTime(row["created_date"]); 
                        Dto.Periode = Convert.ToInt32(row["periode"]); 
                        Dto.Status = Convert.ToBoolean(row["status"]); 
                        Dto.TotalShu = Convert.ToInt64(row["total_SHU"]);

                        ListDto.Add(Dto);
                    }
                }
            }
            return ListDto;
        }
        public BagiShuDto HitungShuAnggota(int Tahun, string IdAnggota)
        {
            try
            {
                var ListBagiShuDto = new List<BagiShuDto>();
                var GetDataShu = _dataKoperasiServices.GetByTahun(Tahun);
                var GetDataAnggota = _dataAnggotaServices.GetByIdAnggota(IdAnggota);

                var Dto = new BagiShuDto();

                Dto.IdAnggota = GetDataAnggota.IdAnggota;
                Dto.NamaAnggota = GetDataAnggota.NamaAnggota;
                Dto.Periode = Tahun;
                var GetAllIpt = _iptAnggotaServices.GetAllByIdAnggotaDanTahun(Dto.IdAnggota, Tahun);
                if (GetAllIpt.Count > 0)
                {
                    #region hitung JMA
                    try
                    {
                        var Pokok = GetAllIpt.FirstOrDefault().Pokok;
                        var Wajib = GetAllIpt.Sum(x => x.Wajib);
                        var Sukarela = GetAllIpt.Sum(x => x.Sukarela);

                        Dto.TotalSimpanan = Pokok + Wajib + Sukarela;
                        Dto.Jma = (Dto.TotalSimpanan / GetDataShu.TotalSimpanan) * (decimal)0.2 * GetDataShu.TotalShu;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    #endregion

                    #region hitung JUA
                    try
                    {
                        Dto.TotalBelanja = GetAllIpt.Sum(x => x.Belanja);
                        Dto.Jua = (Dto.TotalBelanja / GetDataShu.TotalPenjualan) * (decimal)0.1 * GetDataShu.TotalShu;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    #endregion

                    #region  hitung JPA
                    try
                    {
                        Dto.TotalBungaPinjaman = GetAllIpt.Sum(x => x.BungaPinjaman);
                        Dto.Jpa = (Dto.TotalBungaPinjaman / GetDataShu.TotalPinjaman) * (decimal)0.1 * GetDataShu.TotalShu;
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    Dto.TotalShu = Dto.Jma + Dto.Jpa + Dto.Jua;
                    #endregion
                }
                return Dto;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<BagiShuDto> HitungAllShu(int Tahun)
        {
            try
            {
                var ListBagiShuDto = new List<BagiShuDto>();
                var GetDataShu = _dataKoperasiServices.GetByTahun(Tahun);
                var GetAllDataAnggota = _dataAnggotaServices.GetAllActive();

                foreach (var item in GetAllDataAnggota)
                {
                    var Dto = new BagiShuDto();

                    Dto.IdAnggota = item.IdAnggota;
                    Dto.NamaAnggota = item.NamaAnggota;
                    Dto.Periode = Tahun;
                    var GetAllIpt = _iptAnggotaServices.GetAllByIdAnggotaDanTahun(Dto.IdAnggota, Tahun);
                    if(GetAllIpt.Count > 0)
                    {
                        #region hitung JMA
                        try
                        {
                            var Pokok = GetAllIpt.FirstOrDefault().Pokok;
                            var Wajib = GetAllIpt.Sum(x => x.Wajib);
                            var Sukarela = GetAllIpt.Sum(x => x.Sukarela);

                            Dto.TotalSimpanan = Pokok + Wajib + Sukarela;
                            Dto.Jma = (Dto.TotalSimpanan / GetDataShu.TotalSimpanan) * (decimal)0.2 * GetDataShu.TotalShu;
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        #endregion

                        #region hitung JUA
                        try
                        {
                            Dto.TotalBelanja = GetAllIpt.Sum(x => x.Belanja);
                            Dto.Jua = (Dto.TotalBelanja / GetDataShu.TotalPenjualan) * (decimal)0.1 * GetDataShu.TotalShu;
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        #endregion

                        #region  hitung JPA
                        try
                        {
                            Dto.TotalBungaPinjaman = GetAllIpt.Sum(x => x.BungaPinjaman);
                            Dto.Jpa = (Dto.TotalBungaPinjaman / GetDataShu.TotalPinjaman) * (decimal)0.1 * GetDataShu.TotalShu;
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        Dto.TotalShu = Dto.Jma + Dto.Jpa + Dto.Jua;
                        ListBagiShuDto.Add(Dto);
                        #endregion
                    }

                }
                return ListBagiShuDto;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
