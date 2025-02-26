using AlumniManagement.Frontend.AlumniService;
using AlumniManagement.Frontend.Models;
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

        List<AlumniModel> ImportAlumniFromExcel(HttpPostedFileBase file);
    }
}
