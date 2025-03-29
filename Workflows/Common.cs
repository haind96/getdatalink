using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using RPA_GetDataLink.ObjectRepository;
using UiPath.Core;
using UiPath.Core.Activities.Storage;
using UiPath.Excel;
using UiPath.Excel.Activities;
using UiPath.Excel.Activities.API;
using UiPath.Excel.Activities.API.Models;
using UiPath.GSuite.Activities.Api;
using UiPath.Orchestrator.Client.Models;
using UiPath.UIAutomationNext.API.Contracts;
using UiPath.UIAutomationNext.API.Models;
using UiPath.UIAutomationNext.Enums;

namespace RPA_GetDataLink
{
    public class Common
    {
        public static string GetPositionsFromHtml(string htmlContent, string pattern)
        {
            Regex regex = new Regex(pattern);

            // Kiểm tra khớp với chuỗi
            Match match = regex.Match(htmlContent);

            // Nếu có kết quả, trả về số lượng vị trí tuyển dụng
            if (match.Success)
            {
                return match.Groups[1].Value; // Lấy số từ nhóm đầu tiên
            }

            return null;
        }

        public static int GetTotalPages(int totalPositions, int positionsPerPage)
        {
            // Tính số trang
            int totalPages = totalPositions / positionsPerPage;

            // Kiểm tra nếu có phần dư và cần thêm một trang nữa
            if (totalPositions % positionsPerPage != 0)
            {
                totalPages++;
            }

            return totalPages;
        }

        public static DataTable GetJobTitlesAndLinks(string htmlContent, string pattern)
        {
            DataTable jobTable = new DataTable();
            jobTable.Columns.Add("Subject", typeof(string));
            jobTable.Columns.Add("Link", typeof(string));

            // Tìm tất cả các kết quả khớp với biểu thức
            MatchCollection matches = Regex.Matches(htmlContent, pattern, RegexOptions.Singleline);

            // Duyệt qua các kết quả và thêm vào bảng
            foreach (Match match in matches)
            {
                string jobTitle = match.Groups[1].Value;  // Lấy tên công việc từ nhóm 1
                string jobLink = match.Groups[2].Value;   // Lấy link công việc từ nhóm 2

                jobTable.Rows.Add(jobTitle, jobLink);
            }

            return jobTable;
        }

        public static List<List<int>> GroupValues(int totalValue, int groupSize)
        {
            List<List<int>> groups = new List<List<int>>();

            // Chia thành các nhóm có kích thước groupSize
            for (int i = 0; i < totalValue; i += groupSize)
            {
                // Tạo một nhóm con chứa các giá trị từ i đến i + groupSize - 1
                List<int> group = new List<int>();
                for (int j = i + 1; j <= Math.Min(i + groupSize, totalValue); j++)
                {
                    group.Add(j);
                }
                groups.Add(group);
            }

            return groups;
        }
    }
}