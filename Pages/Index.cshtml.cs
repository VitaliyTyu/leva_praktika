using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BD9.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BD9.Pages.Orders
{
    [Authorize]
    public class IndexModel : PageModel
    {
        ApplicationContext context;
        public List<Order> Orders { get; private set; } = new();
        public IndexModel(ApplicationContext db)
        {
            context = db;
        }
        public void OnGet()
        {
            Orders = context.Orders
                .Include(x => x.Service)
                .Include(x => x.Emp).ThenInclude(x=>x.ContactInform)
                .Include(x => x.Complaints)
                .Include(x => x.Client)
                .Include(x => x.Complaints)
                .AsNoTracking().ToList();
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var user = await context.Orders.Include(x => x.Service).Include(x => x.Emp).FirstOrDefaultAsync(x => x.id == id);

            if (user != null)
            {
                context.Orders.Remove(user);
                await context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using BD9.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.EntityFrameworkCore;
//using OfficeOpenXml;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.ComponentModel;

//namespace BD9.Pages.Orders
//{
//    [Authorize]
//    public class IndexModel : PageModel
//    {
//        private readonly ApplicationContext context;
//        public List<Order> Orders { get; private set; } = new();

//        public IndexModel(ApplicationContext db)
//        {
//            context = db;
//        }

//        public void OnGet()
//        {
//            Orders = context.Orders.AsNoTracking().ToList();
//        }

//        public async Task<IActionResult> OnPostDeleteAsync(int id)
//        {
//            var order = await context.Orders.FindAsync(id);

//            if (order != null)
//            {
//                context.Orders.Remove(order);
//                await context.SaveChangesAsync();
//            }

//            return RedirectToPage();
//        }

//        public IActionResult OnPostExportToExcel()
//        {
//            // �������� ������ �� ���� ������
//            List<Order> data = context.Orders.AsNoTracking().ToList();

//            // ������� ����� ����� Excel
//            ExcelPackage package = new ExcelPackage();

//            // ������� ����� ���� Excel
//            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Orders");
//            // ��������� ��������� ��������
//            worksheet.Cells[1, 1].Value = "������������ ������";
//            worksheet.Cells[1, 2].Value = "������";
//            worksheet.Cells[1, 3].Value = "������";
//            worksheet.Cells[1, 4].Value = "���� �������� ������";
//            worksheet.Cells[1, 5].Value = "��������";
//            worksheet.Cells[1, 6].Value = "�������� ��������� ��������";
//            worksheet.Cells[1, 7].Value = "������";

//            // ��������� ������
//            for (int i = 0; i < data.Count; i++)
//            {
//                worksheet.Cells[i + 2, 1].Value = data[i].ServiceId;
//                worksheet.Cells[i + 2, 2].Value = data[i].ClientId;
//                worksheet.Cells[i + 2, 3].Value = data[i].EmploeeId;
//                worksheet.Cells[i + 2, 4].Value = data[i].DateIssue;
//                worksheet.Cells[i + 2, 5].Value = data[i].Warraty;
//                worksheet.Cells[i + 2, 6].Value = data[i].Description;
//                worksheet.Cells[i + 2, 7].Value = data[i].ComplaintId;
//            }

//            // ��������� ���� Excel
//            byte[] fileContents = Encoding.UTF8.GetBytes(package.GetAsByteArray());
//            string fileName = "Orders.xlsx";

//            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
//        }
//    }
//}


