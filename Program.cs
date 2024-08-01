using Application_.Services;
using Application_.Services.Concrete;
using Core.Constats;
using Core.Entities;
using Core.Messages;

namespace Presentation
{
    public static class Program
    {
        private static readonly GroupService _groupService = new GroupService();
        private static readonly StudentService _studentService = new StudentService();
        

        static void Main(string[] args)
        {
            bool isSucceded = true;
            bool Exit = true;
            while (Exit)
            {
                ShowMenu();
                Messages.InputMessage("choice");
                string choiceInput = Console.ReadLine();
                isSucceded = int.TryParse(choiceInput, out int choice);
                if (isSucceded)
                {
                    switch ((Operations)choice)
                    {
                        case Operations.ViewGroups:
                            _groupService.GetAllGroups();
                            break;
                        case Operations.GetDetailsofGroup:
                            _groupService.GetDetailsOfGroup();
                            break;
                        case Operations.AddGroup:
                            _groupService.AddGroup();
                            break;
                        case Operations.UpdateGroup:
                            _groupService.UpdateGroup();
                            break;
                        case Operations.DeleteGroup:
                            _groupService.RemoveGroup();
                            break;
                        case Operations.ViewStudents:
                            _studentService.GetAllStudents();
                            break;
                        case Operations.GetDetailsofStudent:
                            _studentService.GetStudentDetail();
                            break;
                        case Operations.AddStudent:
                            _studentService.AddStudent();
                            break;
                        case Operations.UpdateStudent:
                            _studentService.UpdateStudent();
                            break;
                        case Operations.DeleteStudent:
                            _studentService.DeleteStudent();
                            break;
                        case Operations.Exit:
                            Exit = false;
                            break;
                        default:
                            Messages.InvalidInputMessage(choiceInput);
                            break;
                    }
                }
                else
                {
                    Messages.InvalidInputMessage(choiceInput);
                }
            }
        }
        static void ShowMenu()
        {
            Console.WriteLine("1 All Groups");
            Console.WriteLine("2 Add Group");
            Console.WriteLine("3 Update Group");
            Console.WriteLine("4 Delete Group");
            Console.WriteLine("5 Get Details of Group");
            Console.WriteLine("6 All Students");
            Console.WriteLine("7 Add Student");
            Console.WriteLine("8 Update Student");
            Console.WriteLine("9 Delete Student");
            Console.WriteLine("10 Get Details of Student");
            Console.WriteLine("0 Exit");
        }
    }
}
