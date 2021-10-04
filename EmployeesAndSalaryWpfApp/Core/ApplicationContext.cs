using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EmployeesAndSalaryWpfApp.MVVM.Model;

namespace EmployeesAndSalaryWpfApp.Core
{
    // Для взаимодействия с базой данных, используем класс DbContext, который находится в Microsoft.EntityFrameworkCore
    public class ApplicationContext : DbContext
    {
        // DbSet<> представляет набор объектов, которые хранятся в базе данных и взаимодействуют с классами
        public DbSet<Post> Posts { get; set; }
        public DbSet<Staffer> Staffers { get; set; }

        // Устанавливаем параметры подключения к базе данных
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(@"Data Source=C:\Data\companydata.db");
    }
}
