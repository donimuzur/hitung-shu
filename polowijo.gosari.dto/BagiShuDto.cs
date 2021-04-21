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
        public double TotalSimpanan { set; get; }
        public double TotalBungaPinjaman { set; get; }
        public double TotalBelanja { set; get; }
        public double Jma { set; get; }
        public double Jua { set; get; }
        public double Jpa { set; get; }
        public double TotalShu { set; get; }
        public int Periode { set; get; }
        public bool Status { set; get; }
        public DateTime CreatedDate { set; get; }
        public string CreatedBy { set; get; }
        public DateTime? ModifiedDate { set; get; }
        public string ModifiedBy { set; get; }

    }
}
