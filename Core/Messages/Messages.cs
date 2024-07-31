using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Core.Messages
{
    public static class Messages
    {
        public static void InputMessage(string name) => Console.WriteLine($"Input {name}");
        public static void RenameTeacher() => Console.WriteLine("");
        public static void SuccessMessage(string name, string type) => Console.WriteLine($"{name} succesfully {type}");
        public static void BeginDateMessage(string name) => Console.WriteLine($"Input {name}");
        public static void EndDateMessage(string name, int period) => Console.WriteLine($"Input {name}");
        public static void WantToChangeMessage(string name) => Console.WriteLine($"Do you want to change {name}");
        public static void ErrorMessage() => Console.WriteLine("Error happen");
        public static void InvalidInputMessage(string name) => Console.WriteLine($"{name} is invalid.");
        public static void NotFoundMessage(string name) => Console.WriteLine($"{name} not found");
        public static void LimitInputMessage(string name) => Console.WriteLine($"{name} is small {name} should be between 0 and 15");
        public static void StudentLimitMessage() => Console.WriteLine("You can't add student");
        public static void CountZeroMessage(string name) => Console.WriteLine($"Add {name}");
        public static void ExiststingMessage(string name) => Console.WriteLine($"This {name} is already exists");
    }
}
