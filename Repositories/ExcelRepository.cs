using AlumniManagement.Frontend.AlumniService;
using AlumniManagement.Frontend.FacultyService;
using AlumniManagement.Frontend.Interfaces;
using AlumniManagement.Frontend.MajorService;
using AlumniManagement.Frontend.Models;
using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace AlumniManagement.Frontend.Repositories
{
    public class ExcelRepository : IExcelRepository
    {
        private AlumniServiceClient _alumniServiceClient;
        private MajorServiceClient _majorServiceClient;
        private FacultyServiceClient _facultyServiceClient;

        public ExcelRepository()
        {
            _alumniServiceClient = new AlumniServiceClient();
            _majorServiceClient = new MajorServiceClient();
            _facultyServiceClient = new FacultyServiceClient();
        }

        public Workbook AlumniExportExcel()
        {
            var data = _alumniServiceClient.GetAll().Select(a => new
            {
                AlumniId = a.AlumniID,
                FirstName = a.FirstName,
                MiddleName = a.MiddleName,
                LastName = a.LastName,
                Email = a.Email,
                MobileNumber = a.MobileNumber,
                Address = a.Address,
                StateDistrict = a.StateName + " - " + a.DistrictName,
                DateOfBirth = a.DateOfBirth,
                GraduationYear = a.GraduationYear,
                Degree = a.Degree,
                FacultyMajor = a.FacultyName + " - " + a.MajorName,
                LinkedInProfile = a.LinkedInProfile,
                Gender = a.Gender
            });

            //dropdown
            var stateDistricts = _alumniServiceClient.GetStatesDistrictName();
            var majorFaculties = _alumniServiceClient.GetMajorFacultiesName();
            var degreeList = new List<string>()
            {
                "D3",
                "S1",
                "S2",
                "S3"
            };

            var genderList = new List<string>()
            {
                "Male",
                "Female"
            };


            Workbook workBook = new Workbook();
            Worksheet workSheet = workBook.Worksheets[0];

            int[] maxColumnWidths = new int[14];

            workSheet.Name = "Alumni Data";


            workSheet.Cells[0, 0].PutValue("AlumniId");
            workSheet.Cells[0, 1].PutValue("First Name");
            workSheet.Cells[0, 2].PutValue("Middle Name");
            workSheet.Cells[0, 3].PutValue("Last Name");
            workSheet.Cells[0, 4].PutValue("Gender");
            workSheet.Cells[0, 5].PutValue("Email");
            workSheet.Cells[0, 6].PutValue("Mobile Number");
            workSheet.Cells[0, 7].PutValue("Address");
            workSheet.Cells[0, 8].PutValue("State - District");
            workSheet.Cells[0, 9].PutValue("Date of Birth");
            workSheet.Cells[0, 10].PutValue("Graduation Year");
            workSheet.Cells[0, 11].PutValue("Degree");
            workSheet.Cells[0, 12].PutValue("Faculty Major");
            workSheet.Cells[0, 13].PutValue("LinkedIn Profile");

            maxColumnWidths[1] = "First Name".Length;
            maxColumnWidths[2] = "Middle Name".Length;
            maxColumnWidths[3] = "Last Name".Length;
            maxColumnWidths[4] = "Gender".Length;
            maxColumnWidths[5] = "Email".Length;
            maxColumnWidths[6] = "Mobile Number".Length;
            maxColumnWidths[7] = "Address".Length;
            maxColumnWidths[8] = "State - District".Length;
            maxColumnWidths[9] = "Date of Birth".Length;
            maxColumnWidths[10] = "Graduation Year".Length;
            maxColumnWidths[11] = "Degree".Length;
            maxColumnWidths[12] = "Faculty Major".Length;
            maxColumnWidths[13] = "LinkedIn Profile".Length;

            workSheet.Cells.HideColumn(0);

            Style unlockedStyle = workSheet.Workbook.CreateStyle();
            unlockedStyle.HorizontalAlignment = TextAlignmentType.Center;
            unlockedStyle.IsLocked = false;
            workSheet.Cells.ApplyStyle(unlockedStyle, new StyleFlag() { Locked = true });

            Style style = workSheet.Workbook.CreateStyle();
            StyleFlag styleFlag = new StyleFlag();
            style.IsLocked = true;
            styleFlag.Locked = true;
            workSheet.Cells.Columns[0].ApplyStyle(style, styleFlag); // StateProvinceID

            Style dateStyle = workSheet.Workbook.CreateStyle();
            dateStyle.Custom = dateStyle.Custom = "dd-mmm-yyyy";
            StyleFlag flag = new StyleFlag();
            flag.NumberFormat = true;
            workSheet.Cells.Columns[9].ApplyStyle(dateStyle, flag);

            //style nomor hp
            Style textStyle = workSheet.Workbook.CreateStyle();
            textStyle.Number = 49; // Format Text
            textStyle.HorizontalAlignment = TextAlignmentType.Left;
            StyleFlag styleNumberFlag = new StyleFlag();
            styleNumberFlag.NumberFormat = true;
            workSheet.Cells.Columns[5].ApplyStyle(textStyle, styleNumberFlag);



            

            workSheet.Protect(ProtectionType.All);

            for (int i = 0; i < data.Count(); i++)
            {
                workSheet.Cells[i + 1, 0].PutValue(data.ToList()[i].AlumniId);
                workSheet.Cells[i + 1, 1].PutValue(data.ToList()[i].FirstName);
                workSheet.Cells[i + 1, 2].PutValue(data.ToList()[i].MiddleName);
                workSheet.Cells[i + 1, 3].PutValue(data.ToList()[i].LastName);
                workSheet.Cells[i + 1, 4].PutValue(data.ToList()[i].Gender);
                workSheet.Cells[i + 1, 5].PutValue(data.ToList()[i].Email);
                workSheet.Cells[i + 1, 6].PutValue(data.ToList()[i].MobileNumber);
                workSheet.Cells[i + 1, 7].PutValue(data.ToList()[i].Address);
                workSheet.Cells[i + 1, 8].PutValue(data.ToList()[i].StateDistrict);
                workSheet.Cells[i + 1, 9].PutValue(data.ToList()[i].DateOfBirth);
                workSheet.Cells[i + 1, 10].PutValue(data.ToList()[i].GraduationYear);
                workSheet.Cells[i + 1, 11].PutValue(data.ToList()[i].Degree);
                workSheet.Cells[i + 1, 12].PutValue(data.ToList()[i].FacultyMajor);
                workSheet.Cells[i + 1, 13].PutValue(data.ToList()[i].LinkedInProfile);
                    

                maxColumnWidths[1] = Math.Max(maxColumnWidths[1], data.ToList()[i].FirstName.Length);
                if (data.ToList()[i].MiddleName != null)
                {
                    maxColumnWidths[2] = Math.Max(maxColumnWidths[2], data.ToList()[i].MiddleName.Length);
                }
           
                maxColumnWidths[3] = Math.Max(maxColumnWidths[3], data.ToList()[i].LastName.Length);

                if(data.ToList()[i].Gender != null)
                {
                    maxColumnWidths[4] = Math.Max(maxColumnWidths[4], data.ToList()[i].Gender.Length);
                }
                
                maxColumnWidths[5] = Math.Max(maxColumnWidths[5], data.ToList()[i].Email.ToString().Length);
                maxColumnWidths[6] = Math.Max(maxColumnWidths[6], data.ToList()[i].MobileNumber.ToString().Length);
                maxColumnWidths[7] = Math.Max(maxColumnWidths[7], data.ToList()[i].Address.ToString().Length);
                maxColumnWidths[8] = Math.Max(maxColumnWidths[8], data.ToList()[i].StateDistrict.ToString().Length);
                maxColumnWidths[9] = Math.Max(maxColumnWidths[9], data.ToList()[i].DateOfBirth.ToString().Length);
                maxColumnWidths[10] = Math.Max(maxColumnWidths[10], data.ToList()[i].GraduationYear.ToString().Length);
                maxColumnWidths[11] = Math.Max(maxColumnWidths[11], data.ToList()[i].Degree.ToString().Length);
                maxColumnWidths[12] = Math.Max(maxColumnWidths[12], data.ToList()[i].FacultyMajor.ToString().Length);

                if (data.ToList()[i].LinkedInProfile != null)
                {
                    maxColumnWidths[13] = Math.Max(maxColumnWidths[13], data.ToList()[i].LinkedInProfile.ToString().Length);
                }


            }

            for (int col = 1; col < maxColumnWidths.Length; col++)
            {
                // Tambahkan padding agar ada ruang di sekitar teks
                workSheet.Cells.SetColumnWidth(col, maxColumnWidths[col] + 2);
            }

            Worksheet masterSheet = workBook.Worksheets.Add("Master");

            for (int i = 0; i < stateDistricts.Count(); i++)
            {
                masterSheet.Cells[i, 0].PutValue(stateDistricts[i]);
            }

            for (int i = 0; i < majorFaculties.Count(); i++)
            {
                masterSheet.Cells[i, 1].PutValue(majorFaculties[i]);
            }

            for (int i = 0; i < degreeList.Count(); i++)
            {
                masterSheet.Cells[i, 2].PutValue(degreeList[i]);
            }

            for (int i = 0; i < genderList.Count(); i++)
            {
                masterSheet.Cells[i, 3].PutValue(genderList[i]);
            }

            masterSheet.VisibilityType = VisibilityType.VeryHidden;

            //dropdown
            int stateDisIndex = 8;
            CellArea stateDisArea = CellArea.CreateCellArea(1, stateDisIndex, 1000, stateDisIndex);
            Validation stateDisVal = workSheet.Validations[workSheet.Validations.Add(stateDisArea)];
            stateDisVal.Type = ValidationType.List;
            stateDisVal.Formula1 = "=Master!$A$1:$A$" + stateDistricts.Count();

            int majFacIndex = 12;
            CellArea majfacArea = CellArea.CreateCellArea(1, majFacIndex, 1000, majFacIndex);
            Validation majFacVal = workSheet.Validations[workSheet.Validations.Add(majfacArea)];
            majFacVal.Type = ValidationType.List;
            majFacVal.Formula1 = "=Master!$B$1:$B$" + majorFaculties.Count();

            int degreeIndex = 11;
            CellArea degreeArea = CellArea.CreateCellArea(1, degreeIndex, 1000, degreeIndex);
            Validation degreeVal = workSheet.Validations[workSheet.Validations.Add(degreeArea)];
            degreeVal.Type = ValidationType.List;
            degreeVal.Formula1 = "=Master!$C$1:$C$" + degreeList.Count();

            int genderIndex = 4;
            CellArea genderArea = CellArea.CreateCellArea(1, genderIndex, 1000, genderIndex);
            Validation genderVal = workSheet.Validations[workSheet.Validations.Add(genderArea)];
            genderVal.Type = ValidationType.List;
            genderVal.Formula1 = "=Master!$D$1:$D$" + genderList.Count();

            //Firsname text lenght
            int firstNameI = 1; // Index kolom state Province Code
            CellArea firstNameArea = CellArea.CreateCellArea(1, firstNameI, 1000, firstNameI);
            Validation firstNameVal = workSheet.Validations[workSheet.Validations.Add(firstNameArea)];
            firstNameVal.Type = ValidationType.TextLength;
            firstNameVal.Operator = OperatorType.Between;
            firstNameVal.Formula1 = "1"; // min length
            firstNameVal.Formula2 = "50"; // max length
            firstNameVal.ShowError = true;
            firstNameVal.AlertStyle = ValidationAlertType.Stop;
            firstNameVal.ErrorTitle = "Invalid input";
            firstNameVal.ErrorMessage = "The text length max is 3 characters.";

            //mid
            int midNameI = 2; // Index kolom state Province Code
            CellArea midNameAre = CellArea.CreateCellArea(1, midNameI, 1000, midNameI);
            Validation midNameVal = workSheet.Validations[workSheet.Validations.Add(midNameAre)];
            midNameVal.Type = ValidationType.TextLength;
            midNameVal.Operator = OperatorType.Between;
            midNameVal.Formula1 = "1"; // min length
            midNameVal.Formula2 = "50"; // max length
            midNameVal.ShowError = true;
            midNameVal.AlertStyle = ValidationAlertType.Stop;
            midNameVal.ErrorTitle = "Invalid input";
            midNameVal.ErrorMessage = "The text length max is 3 characters.";

            int lastNameI = 3; // Index kolom state Province Code
            CellArea lastNameA = CellArea.CreateCellArea(1, lastNameI, 1000, lastNameI);
            Validation lastNameVal = workSheet.Validations[workSheet.Validations.Add(lastNameA)];
            lastNameVal.Type = ValidationType.TextLength;
            lastNameVal.Operator = OperatorType.Between;
            lastNameVal.Formula1 = "1"; // min length
            lastNameVal.Formula2 = "50"; // max length
            lastNameVal.ShowError = true;
            lastNameVal.AlertStyle = ValidationAlertType.Stop;
            lastNameVal.ErrorTitle = "Invalid input";
            lastNameVal.ErrorMessage = "The text length max is 3 characters.";

            int emailI = 5; // Index kolom untuk email
            CellArea email = CellArea.CreateCellArea(1, emailI, 1000, emailI);
            Validation emailVal = workSheet.Validations[workSheet.Validations.Add(email)];


            emailVal.ShowInput = true;
            emailVal.InputTitle = "Enter Email";
            emailVal.InputMessage = "Enter a valid Email. Example: user@example.com";



            int mobileNumI = 6; // Index kolom state Province Code
            CellArea mobileArea = CellArea.CreateCellArea(1, mobileNumI, 1000, mobileNumI);
            Validation mobileVal = workSheet.Validations[workSheet.Validations.Add(mobileArea)];
            mobileVal.Type = ValidationType.TextLength;
            mobileVal.Operator = OperatorType.Between;
            mobileVal.Formula1 = "10"; // min length
            mobileVal.Formula2 = "15"; // max length
            mobileVal.ShowError = true;
            mobileVal.AlertStyle = ValidationAlertType.Stop;
            mobileVal.ErrorTitle = "Invalid input";
            mobileVal.ErrorMessage = "The text length max is 15 characters.";

            Validation customMobileVal = workSheet.Validations[workSheet.Validations.Add(mobileArea)];
            customMobileVal.Type = ValidationType.Custom;
            customMobileVal.Formula1 = "=AND(ISNUMBER(O2),LEN(O2)>=10,LEN(O2)<=15)";
            customMobileVal.ShowError = true;
            customMobileVal.AlertStyle = ValidationAlertType.Stop;
            customMobileVal.ErrorTitle = "Invalid phone number";
            customMobileVal.ErrorMessage = "The phone number must start with '08'.";

            mobileVal.ShowInput = true;
            mobileVal.InputTitle = "Enter Phone Number";
            mobileVal.InputMessage = "Enter a valid Number using Indonesian Number Code. Example: 08xxxxxxx";

            int dOBI = 9;
            CellArea dateArea = CellArea.CreateCellArea(1, dOBI, 1000, dOBI);
            Validation dateVal = workSheet.Validations[workSheet.Validations.Add(dateArea)];
            dateVal.Type = ValidationType.Date;
            dateVal.Operator = OperatorType.Between;
            dateVal.Formula1 = "1900-01-01";
            dateVal.Formula2 = "2099-12-31";
            dateVal.ShowError = true;
            dateVal.AlertStyle = ValidationAlertType.Stop;
            dateVal.ErrorTitle = "Invalid input";
            dateVal.ErrorMessage = "Please enter a valid date in the format YYYY-MM-DD (e.g., 1995-06-15).";

            dateVal.ShowInput = true;
            dateVal.InputTitle = "Enter Date of Birth";
            dateVal.InputMessage = "Enter a valid date (YYYY-MM-DD). Example: 1995-06-15";


            // Validation for Graduation Year
            int gradeYearI = 10;
            CellArea yearArea = CellArea.CreateCellArea(1, gradeYearI, 1000, gradeYearI);
            Validation yearVal = workSheet.Validations[workSheet.Validations.Add(yearArea)];
            yearVal.Type = ValidationType.Date;
            yearVal.Operator = OperatorType.Between;
            yearVal.Formula1 = "1980";
            yearVal.Formula2 = "2024";
            yearVal.ShowError = true;
            yearVal.AlertStyle= ValidationAlertType.Stop;
            yearVal.ErrorTitle = "Invalid Graduation Year Input";
            yearVal.ErrorMessage = "Please enter year between 1980 - 2024";

            yearVal.ShowInput = true;
            yearVal.InputTitle = "Enter Graduation year";
            yearVal.InputMessage = "Enter year between 1980 - 2024";



            return workBook;
        }


        public List<AlumniModel> ImportAlumniFromExcel(HttpPostedFileBase file)
        {
            var workbook = new Workbook(file.InputStream);
            var worksheet = workbook.Worksheets[0];

            var listAlumnis = new List<AlumniModel>();
            var listStringError = new List<string>(); // tujuannya untuk menampung error yang terjadi pada saat import

            for (int i = 1; i <= worksheet.Cells.MaxDataRow; i++)
            {
                int AlumniID = ConvertJob(worksheet,i);
                string FirstName = worksheet.Cells[i, 1].StringValue;
                string MiddleName = (worksheet.Cells[i, 2].StringValue ?? "") ;
                string LastName = worksheet.Cells[i, 3].StringValue;
                string Gender = GenderValue(worksheet,i, listStringError);
                string Email = ValidateEmail(worksheet,i, listStringError);
                string MobileNumber = ConvertMobileNumber(worksheet,i,listStringError);
                string Address = worksheet.Cells[i, 7].StringValue;
                string StateDistrict = worksheet.Cells[i, 8].StringValue;
                DateTime DateOfBirth = ConvertDOB(worksheet,i);

                int GraduationYear = worksheet.Cells[i, 10].IntValue;
                string Degree = worksheet.Cells[i, 11].StringValue;
                string FacultyMajor = worksheet.Cells[i, 12].StringValue;
                string LinkedInProfile = worksheet.Cells[i, 13].StringValue;

                var parts = StateDistrict.Split(new[] { " - " }, StringSplitOptions.None);
                string stateName = parts[0];
                string districtName = parts[1];

                var parts2 = FacultyMajor.Split(new[] { " - " }, StringSplitOptions.None);
                string facultName = parts2[0];
                string majorName = parts2[1];




                var alumniData = new AlumniModel
                {
                    AlumniID = AlumniID,
                    FirstName = FirstName,
                    MiddleName = MiddleName,
                    LastName = LastName,
                    Email = Email,
                    MobileNumber = MobileNumber,
                    Address = Address,
                    DistrictID = _alumniServiceClient.GetDistrictIdByName(districtName),
                    DateOfBirth = DateOfBirth,
                    GraduationYear = GraduationYear,
                    Degree = Degree,
                    MajorID = _majorServiceClient.GetMajorIdByName(majorName).MajorID,
                    LinkedInProfile = LinkedInProfile,
                    ModifiedDate= DateTime.Now,
                    Gender = Gender,
                    ErrorDetails = String.Join(" ,", listStringError),//untuk menampung error yang terjadi pada saat import
                    FullName = FirstName + " " + (MiddleName ?? "") + " " + LastName,
                    StateName = stateName,
                    DistrictName = districtName,
                    MajorName = majorName,
                    FacultyName = facultName

                };

                

                listAlumnis.Add(alumniData);
                listStringError = new List<string>();
                //_alumniServiceClient.ImportFromExcel(result);

            }


            //_alumniServiceClient.UpsertMultipleAlumni(listAlumnis.ToArray());
            //manggil upsert with list

            return listAlumnis;
        }

        private int ConvertJob(Worksheet worksheet, int i)
        {
            if (worksheet.Cells[i, 0].Value != null)
            {
                return worksheet.Cells[i, 0].IntValue;
            }
            else
            {
                return 0;
            }
        }

        private DateTime ConvertDOB(Worksheet worksheet, int i)
        {
            if (worksheet.Cells[i, 9].Value != null)
            {
                return worksheet.Cells[i, 9].DateTimeValue;
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        private string GenderValue(Worksheet worksheet, int i, List<string> errorList)
        {
            if (worksheet.Cells[i, 4].Value != null)
            {
                return worksheet.Cells[i, 4].StringValue;
            }
            else
            {
                errorList.Add("Gender is null");
                return "";
            }
        }

        private string DateConverter(DateTime? dateTime)
        {
            if (dateTime == DateTime.MinValue || dateTime == null) { return "N/A"; }

            return dateTime.Value.ToString("yyyy-MM-dd");
        }

        private string ConvertMobileNumber(Worksheet worksheet, int i, List<string> errorList)
        {
            string pattern = @"^08\d{8,13}$"; // Nomor HP harus dimulai dengan 08 dan hanya angka (10-15 digit)

            if (worksheet.Cells[i, 6].Value != null)
            {
                if (!Regex.IsMatch(worksheet.Cells[i, 6].StringValue, pattern))
                {
                    //throw new Exception("Invalid phone number! using only Indonesian Number (08xxxxxxx) and contain only numbers (10-15 digits).");

                    errorList.Add("Nomor Hp Error");
                }

                return worksheet.Cells[i, 6].StringValue;
            }
            else
            {
                return "";
            }
        }

        private string ValidateEmail(Worksheet worksheet, int i, List<string> errorList)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"; // Pola email yang valid

            if (worksheet.Cells[i, 5].Value != null) // Asumsikan kolom ke-6 menyimpan email
            {
                string email = worksheet.Cells[i, 5].StringValue.Trim();

                if (!Regex.IsMatch(email, pattern))
                {
                    errorList.Add("Email Error: Format email tidak valid");
                }

                return email;
            }
            else
            {
                return "";
            }
        }
    }
}