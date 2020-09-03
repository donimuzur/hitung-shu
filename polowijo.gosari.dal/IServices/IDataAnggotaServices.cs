using polowijo.gosari.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polowijo.gosari.dal
{
    public interface IDataAnggotaServices
    {
        List<DataAnggotaDto> GetAllActive();
        DataAnggotaDto GetById(long Id);
        DataAnggotaDto GetByIdAnggota(string IdAnggota);
        DataAnggotaDto InserOrUpdate(DataAnggotaDto Dto);
        DataAnggotaDto Insert(DataAnggotaDto Dto);
        DataAnggotaDto Update(DataAnggotaDto Dto);
        void Delete(long Id);
        List<DataAnggotaDto> GetAll();
    }
}
