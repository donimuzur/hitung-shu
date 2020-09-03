using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polowijo.gosari.dto
{
    public class DataAnggotaDto
    {
        public long Id { set; get; }
        public string IdAnggota { set; get; }
        public string NamaAnggota { set; get; }
        public DateTime TanggalGabung { set; get; }
        public bool Status { set; get; }
        public DateTime CreatedDate { set; get; }
        public string CreatedBy { set; get; }
        public DateTime? ModifiedDate { set; get; }
        public string ModifiedBy { set; get; }
        public string StatusDesc
        {
            get { return !Status ? "Tidak Aktif" : "Aktif"; }
        }
    }
}
