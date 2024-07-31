using Application_.Services.Abstract;
using Core.Entities;
using Core.Messages;
using Data.UnitsOfWork.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_.Services.Concrete
{
    public class StudentService : IStudentService
    {
        private readonly Unitofwork _unitofwork;
        public StudentService()
        {
            _unitofwork = new Unitofwork();
        }

        public void AddStudent()
        {
            var CountGroups = _unitofwork.Groups.GetAllGroupsCount();
            if (CountGroups <= 0)
            {
                Messages.CountZeroMessage("Group");
                return;
            }
        GroupInput: _unitofwork.Groups.GetAllGroups();
            Messages.InputMessage("Group id");
            string groupIdInput = Console.ReadLine();
            int groupId;
            bool isSucceded = int.TryParse(groupIdInput, out groupId);
            if (!isSucceded || string.IsNullOrWhiteSpace(groupIdInput))
            {
                Messages.InvalidInputMessage(groupIdInput);
                goto GroupInput;
            }
            var existGroup = _unitofwork.Groups.GetStudentOfGroupById(groupId);
            if (existGroup == null)
            {
                Messages.NotFoundMessage(groupIdInput);
                goto GroupInput;
            }
            var CountStudentsinGroup = existGroup.Students.Count();
            if (CountStudentsinGroup == existGroup.Limit)
            {
                Messages.StudentLimitMessage();
                if (CountGroups == 1)
                    return;
                else
                    goto GroupInput;
            }
        NameInput: Messages.InputMessage("Student name");
            string studentName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(studentName))
            {
                Messages.InvalidInputMessage(studentName);
                goto NameInput;
            }
        SurnameInput: Messages.InputMessage("Student surname");
            string studentSurname = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(studentSurname))
            {
                Messages.InvalidInputMessage(studentSurname);
                goto SurnameInput;
            }
            Student student = new Student();
            student.GroupId = groupId;
            student.Surname = studentSurname;
            student.Name = studentName;
            _unitofwork.Students.Add(student);
            _unitofwork.Commit();
            Messages.SuccessMessage(studentName, "added");
        }

        public void UpdateStudent()
        {
            var studentCount = _unitofwork.Students.GetAllStudentsCount();
            if (studentCount <= 0)
            {
                Messages.CountZeroMessage("Student");
                return;
            }
        UpdateInput: GetAllStudents();
            Messages.InputMessage("Student id");
            string inputId = Console.ReadLine();
            int studentId;
            bool isSucceded = int.TryParse(inputId, out studentId);
            if (!isSucceded || string.IsNullOrWhiteSpace(inputId))
            {
                Messages.InvalidInputMessage(inputId);
                goto UpdateInput;
            }
            var existStudent = _unitofwork.Students.GetByIdWithGroup(studentId);
            if (existStudent == null)
            {
                Messages.NotFoundMessage(inputId);
                goto UpdateInput;
            }
            string studentName = existStudent.Name;
            string studentSurname = existStudent.Surname;
            int studentGroupId = existStudent.GroupId;
        NameInput: Messages.WantToChangeMessage("Student name");
            string input = Console.ReadLine();
            char result;
            isSucceded = char.TryParse(input, out result);
            if (!isSucceded || string.IsNullOrWhiteSpace(input) || result != 'y' && result != 'n')
            {
                Messages.InvalidInputMessage(input);
                goto NameInput;
            }
            if (result == 'y')
            {
            StudentNameInput: Messages.InputMessage("New name");
                studentName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(studentName))
                {
                    Messages.InvalidInputMessage(studentName);
                    goto StudentNameInput;
                }
            }
        SurnameInput: Messages.WantToChangeMessage("Student surname");
            input = Console.ReadLine();
            isSucceded = char.TryParse(input, out result);
            if (!isSucceded || string.IsNullOrWhiteSpace(input) || result != 'y' && result != 'n')
            {
                Messages.InvalidInputMessage(input);
                goto SurnameInput;
            }
            if (result == 'y')
            {
            StudentSurnameInput: Messages.InputMessage("new surname");
                studentSurname = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(studentSurname))
                {
                    Messages.InvalidInputMessage(studentSurname);
                    goto StudentSurnameInput;
                }
            }
        GroupInput: Messages.WantToChangeMessage("Group");
            input = Console.ReadLine();
            isSucceded = char.TryParse(input, out result);
            if (!isSucceded || string.IsNullOrWhiteSpace(input) || result != 'y' && result != 'n')
            {
                Messages.InvalidInputMessage(input);
                goto GroupInput;
            }
            if (result == 'y')
            {
                var CountGroup = _unitofwork.Groups.GetAllGroupsCount();
                if (CountGroup <= 1)
                {
                    Messages.CountZeroMessage("Group");
                    return;
                }
                _unitofwork.Groups.GetAllGroups();
                Messages.InputMessage("Group id");
                input = Console.ReadLine();
                isSucceded = int.TryParse(input, out studentGroupId);
                if (!isSucceded || string.IsNullOrWhiteSpace(input))
                {
                    Messages.InvalidInputMessage(input);
                    goto GroupInput;
                }
                var existGroup = _unitofwork.Groups.GetStudentOfGroupById(studentGroupId);
                if (existGroup == null)
                {
                    Messages.NotFoundMessage(input);
                    goto GroupInput;
                }
                existStudent.Name = studentName;
                existStudent.Surname = studentSurname;
                existStudent.GroupId = studentGroupId;
                _unitofwork.Students.Update(existStudent);
                _unitofwork.Commit();
                Messages.SuccessMessage(studentName, "updated");
            }
        }

        public void DeleteStudent()
        {
        StudentInput: GetAllStudents();
            Messages.InputMessage("Student id");
            string InputId = Console.ReadLine();
            int studentId;
            bool isSucceded = int.TryParse(InputId, out studentId);
            if (!isSucceded || string.IsNullOrWhiteSpace(InputId))
            {
                Messages.InvalidInputMessage(InputId);
                goto StudentInput;
            }
            var existStudent = _unitofwork.Students.GetByIdWithGroup(studentId);
            if (existStudent == null)
            {
                Messages.NotFoundMessage(InputId);
                goto StudentInput;
            }
            _unitofwork.Students.Delete(existStudent);
            _unitofwork.Commit();
            Messages.SuccessMessage(existStudent.Name, "deleted");
        }

        public void GetAllStudents()
        {

            _unitofwork.Students.GetAllStudents();
        }

        public void GetStudentDetail()
        {
        StudentInput: 
            GetAllStudents();
            Messages.InputMessage("Student id");
            string InputId = Console.ReadLine();
            int studentId;
            bool isSucceded = int.TryParse(InputId, out studentId);
            if (!isSucceded || string.IsNullOrWhiteSpace(InputId))
            {
                Messages.InvalidInputMessage(InputId);
                goto StudentInput;
            }
            var existStudent = _unitofwork.Students.GetByIdWithGroup(studentId);
            if (existStudent == null)
            {
                Messages.NotFoundMessage(InputId);
                goto StudentInput;
            }
            Console.WriteLine($"Id: {existStudent.Id} | Surname: {existStudent.Surname} Name: {existStudent.Name} | Group: {existStudent.Group.Name}");
        }
    }
}
