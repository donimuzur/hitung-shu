using polowijo.gosari.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polowijo.gosari.dal
{
    public interface IDataKoperasiServices
    {
        DataKoperasiDto GetByTahun(int Tahun);
        DataKoperasiDto Insert(DataKoperasiDto Dto);
        DataKoperasiDto InserOrUpdate(DataKoperasiDto Dto);
        DataKoperasiDto Update(DataKoperasiDto Dto);
    }
}
