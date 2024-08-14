using Microsoft.AspNetCore.Mvc;
using SIMS_MVC_SE06204.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace SIMS_MVC_SE06204.Controllers
{
    public class StudentController : Controller
    {
        private static List<Student> students = new List<Student>
        {
            new Student { Id = 1, FullName = "Nguyen Van A", BirthYear = new DateTime(2000, 1, 1), Gender = "Male", PhoneNumber = "0123456789", Email = "a@example.com", Major = "Computer Science" },
            new Student { Id = 2, FullName = "Le Thi B", BirthYear = new DateTime(2001, 2, 2), Gender = "Female", PhoneNumber = "0987654321", Email = "b@example.com", Major = "Information Technology" },
            new Student { Id = 3, FullName = "Pham Van C", BirthYear = new DateTime(2004, 2, 2), Gender = "Male", PhoneNumber = "0366144145", Email = "c@example.com", Major = "Marketing" }
        };

        // Kiểm tra trạng thái đăng nhập
        private bool IsUserLoggedIn()
        {
            return HttpContext.Session.GetString("UserName") != null;
        }

        public IActionResult Index(string searchString)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Authentication");
            }

            var filteredStudents = string.IsNullOrEmpty(searchString)
                ? students
                : students.Where(s => s.FullName.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                                      s.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                                      s.PhoneNumber.Contains(searchString)).ToList();

            ViewData["CurrentFilter"] = searchString; // Lưu từ khóa tìm kiếm vào ViewData
            return View(filteredStudents);
        }

        public IActionResult Create()
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Authentication");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Create(Student student)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Authentication");
            }

            if (ModelState.IsValid)
            {
                student.Id = students.Count + 1;
                students.Add(student);
                return RedirectToAction("Index");
            }
            return View(student);
        }

        public IActionResult Edit(int id)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Authentication");
            }

            var student = students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpPost]
        public IActionResult Edit(Student student)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Authentication");
            }

            if (ModelState.IsValid)
            {
                var existingStudent = students.FirstOrDefault(s => s.Id == student.Id);
                if (existingStudent != null)
                {
                    existingStudent.FullName = student.FullName;
                    existingStudent.BirthYear = student.BirthYear;
                    existingStudent.Gender = student.Gender;
                    existingStudent.PhoneNumber = student.PhoneNumber;
                    existingStudent.Email = student.Email;
                    existingStudent.Major = student.Major;
                }
                return RedirectToAction("Index");
            }
            return View(student);
        }

        public IActionResult Delete(int id)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Authentication");
            }

            var student = students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Authentication");
            }

            var student = students.FirstOrDefault(s => s.Id == id);
            if (student != null)
            {
                students.Remove(student);
            }
            return RedirectToAction("Index");
        }
    }
}
