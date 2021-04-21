using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polowijo.gosari.dto
{
    public class IptAnggotaDto
    {
        public long Id { set; get; }
        public string IdAnggota { set; get; }
        public string NamaAnggota { set; get; }
        public DateTime Tanggal { set; get; }
        public double Pokok { set; get; }
        public double Wajib { set; get; }
        public double Sukarela { set; get; }
        public double Belanja { set; get; }
        public double BungaPinjaman { set; get; }
        public DateTime CreatedDate { set; get; }
        public string CreatedBy { set; get; }
        public DateTime ModifiedDate { set; get; }
        public string ModifiedBy { set; get; }
        public string Message { set; get; }
    }
}
