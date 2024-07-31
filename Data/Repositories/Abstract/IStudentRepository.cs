﻿using Core.Entities;
using Data.Repositories.Base_Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Abstract
{
    public interface IStudentRepository : IBaseRepository<Student>
    {
        Student GetByIdGroup(int id);
        public int AllStudentsCount();
        public void GetAllStudents();
    }
}
