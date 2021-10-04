using EmployeesAndSalaryWpfApp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesAndSalaryWpfApp.MVVM.Model
{
    // Класс Post, содержит название должностей, процент за стаж, максимальный процент за стаж,
    // и доп. ставку к зарплате.
    // Так же класс реализует интерфейс INotifyPropertyChanged, чтобы при изменении свойств класса,
    // данные отображались в DataGrid.
    public class Post : ObservableObj
    {
        // По порядку:
        // название должности;
        // процент за стаж;
        // максимальный процент за стаж;
        // доп. процент к зарплате.

        private string namePost;
        private decimal rate;
        private decimal maxRate;
        private decimal ratePlus;

        // Свойство Id назначается к каждой должности, и является основным ключом.
        public int Id { get; set; }

        // Создаем свойства, так же в свойствах указываем метод OnPropertyChanged,
        // который вызывает событие PropertyChanged.
        public string NamePost
        {
            get { return namePost; }
            set
            {
                namePost = value;
                OnPropertyChanged("NamePost");
            }
        }
        public decimal Rate
        {
            get { return rate; }
            set
            {
                rate = value;
                OnPropertyChanged("Rate");
            }
        }
        public decimal MaxRate
        {
            get { return maxRate; }
            set
            {
                maxRate = value;
                OnPropertyChanged("MaxRate");
            }
        }
        public decimal RatePlus
        {
            get { return ratePlus; }
            set
            {
                ratePlus = value;
                OnPropertyChanged("RatePlus");
            }
        }

        // Класс Post является основной сущностью, и хранит коллекцию зависимой сущности, класс Staffer.
        // Реализован принцип один-ко-многим.
        public List<Staffer> Staffers { get; set; }
    }
}
