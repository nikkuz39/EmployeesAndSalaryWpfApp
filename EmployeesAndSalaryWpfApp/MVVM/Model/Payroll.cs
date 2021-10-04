using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesAndSalaryWpfApp.MVVM.Model
{
    // Класс Payroll, содержит количество отработанных лет, процент за выслугу лет, суммарную зарплату
    // и является производным от класса Staffer
    // Так же класс реализует интерфейс INotifyPropertyChanged, чтобы при изменении свойств класса,
    // данные отображались в DataGrid.
    public class Payroll : Staffer, INotifyPropertyChanged
    {
        // По порядку:
        // количество отработанных лет,
        // процент за выслугу лет,
        // суммарная зарплата
        private int yearsWorked;
        private decimal seniorityPay;
        private decimal finalAmount;

        // Создаем свойства, так же в свойствах указываем метод PayrollOnPropertyChanged,
        // который вызывает событие PropertyChanged.
        public int YearsWorked
        {
            get { return yearsWorked; }
            set
            {
                yearsWorked = value;
                PayrollOnPropertyChanged("YearsWorked");
            }
        }

        public decimal SeniorityPay
        {
            get { return seniorityPay; }
            set
            {
                seniorityPay = value;
                PayrollOnPropertyChanged("SeniorityPay");
            }
        }
        public decimal FinalAmount
        {
            get { return finalAmount; }
            set
            {
                finalAmount = value;
                PayrollOnPropertyChanged("FinalAmount");
            }
        }

        // Реализуем интерфейс INotifyPropertyChanged
        // Создаем событие PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        // Объявляем событие PropertyChanged в методе PayrollOnPropertyChanged
        public void PayrollOnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
