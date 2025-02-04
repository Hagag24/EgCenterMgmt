using System;
using System.Collections.Generic;
using System.Linq;

namespace EgCenterMgmt.Shared.DTO
{
    public class GenerateStudentCode
    {
        private static HashSet<string> existingCodes = new HashSet<string>();

        private static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                                        .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string StudentCode(int studentId)
        {
            string studentCode;
            do
            {
                // توليد سلسلة عشوائية من 4 أحرف وأرقام
                string randomString = GenerateRandomString(4);
                studentCode = $"{randomString}-{studentId:D5}";
            } while (existingCodes.Contains(studentCode)); // تأكد من عدم التكرار

            existingCodes.Add(studentCode); // أضف الكود إلى القائمة
            return studentCode;
        }
    }
}
