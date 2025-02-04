using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using EgCenterMgmt.Shared.DTO;
using Newtonsoft.Json;
using System.Net.Http.Json;

class Program
{
    static async Task Main(string[] args)
    {
        // تعيين سياق الرخصة
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        Console.ReadKey();
        string studentApiUrl = "https://localhost:7129/api/Booking/GetStudents";
        string bookingggetApiUrl = "https://localhost:7129/api/Booking/GetBookings";
        string bookingApiUrl = "https://localhost:7129/api/Booking/PostBooking";

        string tenantId = "+Zzq2MYxNi5CrHnvPNOqXhKUI4/AsX8LkgXhTvflh54="; // استبدل هذه بالقيمة الفعلية
        string userId = "sR3LCWv0tarhvx1p5cTIuIsUX57qGHKvO/JJLDPE+eWPyx0+2SuoG5KSHp1RYwzwYO22Z2B76czmtrT1wCJfEg=="; // استبدل هذه بالقيمة الفعلية

        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // إضافة الرؤوس المحددة فقط
            client.DefaultRequestHeaders.Add("tenantid", tenantId);
            client.DefaultRequestHeaders.Add("userid", userId);

            // إرسال بيانات الطلاب
            var studentResponse = await client.GetAsync(studentApiUrl);
            var bookingResponse = await client.GetAsync(bookingggetApiUrl);

            if (studentResponse.IsSuccessStatusCode)
            {
                // قراءة الاستجابة وتحويلها إلى قائمة من الطلاب
                var studentResponseList = await studentResponse.Content.ReadFromJsonAsync<List<StudentDto>>();

                // إذا كانت الاستجابة الخاصة بالحجوزات ناجحة
                if (bookingResponse.IsSuccessStatusCode)
                {
                    // قراءة الحجوزات وتحويلها إلى قائمة من الحجوزات
                    var bookings = await bookingResponse.Content.ReadFromJsonAsync<List<BookingDto>>();

                    // حذف الحجوزات الحالية
                    foreach (var booking in bookings)
                    {
                        // إرسال طلب حذف الحجز
                        var deleteBookingUrl = $"https://localhost:7129/api/Booking/DeleteBooking?id={booking.BookingId}";
                        var deleteBookingResponse = await client.DeleteAsync(deleteBookingUrl);

                        if (deleteBookingResponse.IsSuccessStatusCode)
                        {
                            Console.WriteLine($"تم حذف الحجز بنجاح للطالب {booking.StudentId}");
                        }
                        else
                        {
                            Console.WriteLine($"فشل حذف الحجز للطالب {booking.StudentId}: {deleteBookingResponse.StatusCode}");
                        }
                    }

                    // إضافة الحجوزات من جديد
                    foreach (var student in studentResponseList)
                    {
                        var bookingDto = new RegesterBookingDto
                        {
                            StudentId = student.StudentId,
                            GroupId = 2,
                            BookingDate = DateOnly.FromDateTime(DateTime.UtcNow),
                            PaymentType = "شهري"
                        };

                        var bookingJson = JsonConvert.SerializeObject(bookingDto);
                        var bookingContent = new StringContent(bookingJson, Encoding.UTF8, "application/json");

                        var bookingPostResponse = await client.PostAsync(bookingApiUrl, bookingContent);

                        if (bookingPostResponse.IsSuccessStatusCode)
                        {
                            Console.WriteLine($"تم إضافة الطالب والحجز بنجاح للطالب {student.StudentName}!");
                        }
                        else
                        {
                            Console.WriteLine($"تم إضافة الطالب ولكن حدث خطأ في عملية الحجز للطالب {student.StudentName}: {bookingPostResponse.StatusCode}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"خطأ في استرجاع الحجوزات: {bookingResponse.StatusCode}");
                }
            }
            else
            {
                Console.WriteLine($"خطأ في استرجاع الطلاب: {studentResponse.StatusCode}");
            }
        }
    }
static List<StudentBookingDto> ReadExcelFile(string filePath)
        {
            var studentBookings = new List<StudentBookingDto>();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    var studentBooking = new StudentBookingDto
                    {
                        StudentName = worksheet.Cells[row, 1].Text,
                        GradeId = 3,  // تعيين معرف الصف الدراسي بشكل ثابت
                        GroupId = 2,  // تعيين معرف المجموعة بشكل ثابت
                        StudentPhone = worksheet.Cells[row, 4].Text,
                        StudentWhatsApp = worksheet.Cells[row, 5].Text,
                        FatherWhatsApp = worksheet.Cells[row, 6].Text,
                        Country = worksheet.Cells[row, 7].Text,
                        IsMaile = worksheet.Cells[row, 8].Text == "ذكر" ? true : false,
                        StudentEmail = worksheet.Cells[row, 9].Text,
                        IStatus = false,
                        PaymentType = "شهري",
                        CreatedDate = DateTime.UtcNow,
                        BookingDate = DateOnly.FromDateTime(DateTime.UtcNow)
                    };

                    studentBookings.Add(studentBooking);
                }
            }

            return studentBookings;
        }
}