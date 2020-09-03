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
        public decimal Pokok { set; get; }
        public decimal Wajib { set; get; }
        public decimal Sukarela { set; get; }
        public decimal Belanja { set; get; }
        public decimal BungaPinjaman { set; get; }
        public DateTime CreatedDate { set; get; }
        public string CreatedBy { set; get; }
        public DateTime ModifiedDate { set; get; }
        public string ModifiedBy { set; get; }
    }
}
