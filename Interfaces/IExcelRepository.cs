using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AlumniManagement.Frontend.Interfaces
{
    public interface IExcelRepository
    {
        Workbook AlumniExportExcel();

        void ImportAlumniFromExcel(HttpPostedFileBase file);
    }
}
