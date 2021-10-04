using EmployeesAndSalaryWpfApp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesAndSalaryWpfApp.MVVM.Model
{
    // Класс Staffer, содержит имя сотрудника, дату трудоустройства, руководителя сотрудника,
    // и оклад.
    // Так же класс реализует интерфейс INotifyPropertyChanged, чтобы при изменении свойств класса,
    // данные отображались в DataGrid.
    public class Staffer : ObservableObj
    {
        // По порядку:
        // имя сотрудника;
        // дата трудоустройства;
        // руководитель сотрудника;
        // оклад
        private string namestaff;
        private string dateofemployment;
        private string headofdepartment;
        private decimal paycheck;

        // Свойство Id назначается к каждому сотруднику, и является основным ключом.
        public int Id { get; set; }

        // Создаем свойства, так же в свойствах указываем метод OnPropertyChanged,
        // который вызывает событие PropertyChanged.
        public string NameStaff
        {
            get { return namestaff; }
            set
            {
                namestaff = value;
                OnPropertyChanged("NameStaff");
            }
        }
        public string Dateofemployment
        {
            get { return dateofemployment; }
            set
            {
                dateofemployment = value;
                OnPropertyChanged("Dateofemployment");
            }
        }
        public string Headofdepartment
        {
            get { return headofdepartment; }
            set
            {
                headofdepartment = value;
                OnPropertyChanged("Headofdepartment");
            }
        }
        public decimal Paycheck
        {
            get { return paycheck; }
            set
            {
                paycheck = value;
                OnPropertyChanged("Paycheck");
            }
        }

        // Класс Staffer является зависимой сущностью от класса Post,
        // хранит внешний ключ PostId и ссылку Post Post.
        // Реализован принцип один-ко-многим.
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
