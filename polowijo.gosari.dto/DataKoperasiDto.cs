using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polowijo.gosari.dto
{
    public class DataKoperasiDto
    {
        public long Id { set; get; }
        public int Tahun { set; get; }
        public decimal TotalShu { set; get; }
        public decimal TotalSimpanan { set; get; }
        public decimal TotalPenjualan { set; get; }
        public decimal TotalPinjaman { set; get; }
        public DateTime CreatedDate { set; get; }
        public string CreatedBy { set; get; }
        public DateTime ModifiedDate { set; get; }
        public string ModifiedBy { set; get; }
    }
}
