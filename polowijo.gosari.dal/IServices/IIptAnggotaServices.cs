using polowijo.gosari.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polowijo.gosari.dal
{
    public interface IIptAnggotaServices
    {
        List<IptAnggotaDto> GetAllByTahun(int Tahun);
        IptAnggotaDto InserOrUpdate(IptAnggotaDto Dto);
        IptAnggotaDto GetById(long Id);
        IptAnggotaDto GetByIdAnggota(string IdAnggota);
        List<IptAnggotaDto> GetAllByIdAnggotaDanTahun(string IdAnggota, int Tahun);
        void Delete(long Id);
    }
}
