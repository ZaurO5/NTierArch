using Application_.Services.Abstract;
using Core.Entities;
using Core.Messages;
using Data.Context;
using Data.UnitsOfWork.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_.Services
{
    public class GroupService : IGroupService
    {
        private readonly Unitofwork _unitofwork;
        public GroupService()
        {
            _unitofwork = new Unitofwork();
        }

        public void AddGroup()
        {
            string groupName;
            do
            {
                Messages.InputMessage("Group name");
                groupName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(groupName))
                {
                    Messages.InvalidInputMessage(groupName);
                }
                else if (_unitofwork.Groups.GetGroupByName(groupName) != null)
                {
                    Messages.ExiststingMessage(groupName);
                    groupName = null;
                }
            } while (groupName == null);

            int groupLimit;
            do
            {
                Messages.InputMessage("Group limit");
                string groupLimitInput = Console.ReadLine();
                if (!int.TryParse(groupLimitInput, out groupLimit) || groupLimit <= 0 || groupLimit > 15)
                {
                    Messages.InvalidInputMessage(groupLimitInput);
                }
            } while (groupLimit <= 0 || groupLimit > 15);

            string beginDateInput;
            DateTime beginDate;
            do
            {
                Messages.InputMessage("Begin date");
                beginDateInput = Console.ReadLine();
                if (!DateTime.TryParse(beginDateInput, out beginDate))
                {
                    Messages.InvalidInputMessage(beginDateInput);
                }
            } while (!DateTime.TryParse(beginDateInput, out beginDate));

            string endDateInput;
            DateTime endDate;
            do
            {
                Messages.InputMessage("End date");
                endDateInput = Console.ReadLine();
                if (!DateTime.TryParse(endDateInput, out endDate) || beginDate.AddMonths(6) > endDate)
                {
                    Messages.InvalidInputMessage(endDateInput);
                }
            } while (!DateTime.TryParse(endDateInput, out endDate) || beginDate.AddMonths(6) > endDate);

            var group = new Group
            {
                Name = groupName,
                BeginDate = beginDate,
                EndDate = endDate,
                Limit = groupLimit
            };
            _unitofwork.Groups.Add(group);
            _unitofwork.Commit();
            Messages.SuccessMessage(groupName, "added");
        }

        public void UpdateGroup()
        {
            var groupCount = _unitofwork.Groups.GetAllGroupsCount();
            if (groupCount <= 0)
            {
                Messages.CountZeroMessage("group");
                return;
            }

        UpdateInput:
            GetAllGroups();
            Messages.InputMessage("Group id");
            if (!int.TryParse(Console.ReadLine(), out int groupId))
            {
                Messages.InvalidInputMessage("Group id");
                goto UpdateInput;
            }

            var existGroup = _unitofwork.Groups.GetStudentOfGroupById(groupId);
            if (existGroup == null)
            {
                Messages.NotFoundMessage(groupId.ToString());
                goto UpdateInput;
            }

            string groupName = existGroup.Name;
            int groupLimit = existGroup.Limit;
            DateTime groupBeginDate = existGroup.BeginDate;
            DateTime groupEndDate = existGroup.EndDate;

            if (PromptUser("Change group name y or n") == 'y')
            {
                do
                {
                    Messages.InputMessage("New name");
                    groupName = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(groupName))
                    {
                        Messages.InvalidInputMessage(groupName);
                    }
                    else if (_unitofwork.Groups.GetGroupByName(groupName) != null)
                    {
                        Messages.ExiststingMessage("Group name");
                        groupName = null;
                    }
                } while (groupName == null);

                existGroup.Name = groupName;
            }

            if (PromptUser("Change group limit y or n") == 'y')
            {
                do
                {
                    Messages.InputMessage("New limit");
                    string limitInput = Console.ReadLine();
                    if (!int.TryParse(limitInput, out groupLimit) || groupLimit < existGroup.Students.Count())
                    {
                        Messages.InvalidInputMessage(limitInput);
                    }
                    else
                    {
                        existGroup.Limit = groupLimit;
                    }
                } while (groupLimit < existGroup.Students.Count());
            }

            string newBeginDateInput;
            if (PromptUser("Change group begin date y n?") == 'y')
            {
                do
                {
                    Messages.InputMessage("New begin date)");
                    newBeginDateInput = Console.ReadLine();
                    if (!DateTime.TryParse(newBeginDateInput, out groupBeginDate))
                    {
                        Messages.InvalidInputMessage(newBeginDateInput);
                    }
                    else
                    {
                        existGroup.BeginDate = groupBeginDate;
                    }
                } while (!DateTime.TryParse(newBeginDateInput, out groupBeginDate));
            }

            string newEndDateInput;
            if (PromptUser("Change group end date y or n?") == 'y')
            {
                do
                {
                    Messages.InputMessage("New end date)");
                    newEndDateInput = Console.ReadLine();
                    if (!DateTime.TryParse(newEndDateInput, out groupEndDate) || groupBeginDate.AddMonths(6) > groupEndDate)
                    {
                        Messages.InvalidInputMessage(newEndDateInput);
                    }
                    else
                    {
                        existGroup.EndDate = groupEndDate;
                    }
                } while (!DateTime.TryParse(newEndDateInput, out groupEndDate) || groupBeginDate.AddMonths(6) > groupEndDate);
            }

            _unitofwork.Groups.Update(existGroup);
            _unitofwork.Commit();
            Messages.SuccessMessage(groupName, "updated");
        }

        public void RemoveGroup()
        {
            var groupCount = _unitofwork.Groups.GetAllGroupsCount();
            if (groupCount <= 0)
            {
                Messages.CountZeroMessage("Group");
                return;
            }

        GroupIdInput:
            GetAllGroups();
            Messages.InputMessage("Group id");
            if (!int.TryParse(Console.ReadLine(), out int groupId))
            {
                Messages.InvalidInputMessage("Group id");
                goto GroupIdInput;
            }

            var existGroup = _unitofwork.Groups.GetStudentOfGroupById(groupId);
            if (existGroup == null)
            {
                Messages.NotFoundMessage(groupId.ToString());
                goto GroupIdInput;
            }

            foreach (var student in existGroup.Students)
            {
                _unitofwork.Students.Delete(student);
            }
            _unitofwork.Groups.Delete(existGroup);
            _unitofwork.Commit();
            Messages.SuccessMessage(existGroup.Name, "deleted");
        }

        public void GetAllGroups()
        {
            _unitofwork.Groups.GetAllGroups();
        }

        public void GetDetailsOfGroup()
        {
        GroupInput:
            GetAllGroups();
            Messages.InputMessage("Group id");
            if (!int.TryParse(Console.ReadLine(), out int groupId))
            {
                Messages.InvalidInputMessage("Group id");
                goto GroupInput;
            }

            var existGroup = _unitofwork.Groups.GetStudentOfGroupById(groupId);
            if (existGroup == null)
            {
                Messages.NotFoundMessage(groupId.ToString());
                goto GroupInput;
            }

            Console.WriteLine($"Id: {groupId} | Name: {existGroup.Name} | Limit: {existGroup.Limit} | BeginDate: {existGroup.BeginDate} | EndDate: {existGroup.EndDate}");
            Console.WriteLine("Students:");
            foreach (var student in existGroup.Students)
            {
                Console.WriteLine($"{student.Surname} {student.Name}");
            }
        }

        private char PromptUser(string message)
        {
            Messages.WantToChangeMessage(message);
            string input = Console.ReadLine();
            char result;
            return (char.TryParse(input, out result) && (result == 'y' || result == 'n')) ? result : 'n';
        }
    }
}
