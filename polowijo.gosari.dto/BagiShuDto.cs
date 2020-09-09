using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polowijo.gosari.dto
{
    public class BagiShuDto
    {
        public long Id { set; get; }
        public string IdAnggota { set; get; }
        public string NamaAnggota { set; get; }
        public decimal TotalSimpanan { set; get; }
        public decimal TotalBungaPinjaman { set; get; }
        public decimal TotalBelanja { set; get; }
        public decimal Jma { set; get; }
        public decimal Jua { set; get; }
        public decimal Jpa { set; get; }
        public decimal TotalShu { set; get; }
        public int Periode { set; get; }
        public bool Status { set; get; }
        public DateTime CreatedDate { set; get; }
        public string CreatedBy { set; get; }
        public DateTime? ModifiedDate { set; get; }
        public string ModifiedBy { set; get; }

    }
}
