using EmployeesAndSalaryWpfApp.Core;
using EmployeesAndSalaryWpfApp.MVVM.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EmployeesAndSalaryWpfApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for PayrollControl.xaml
    /// </summary>
    public partial class PayrollControl : UserControl
    {
        // Данное окно отвечает за расчет зарплаты сотрудника
        //
        // Создаем список
        List<Staffer> staffersname;
        // Создаем объект класса ApplicationContext для взаимодействия с базой данных
        ApplicationContext db;
        public PayrollControl()
        {
            InitializeComponent();

            db = new ApplicationContext();

            // В ComboBox x:Name="payrollstaffernamesList" передаем имена сотрудников из таблица Staffers
            payrollstaffernamesList.ItemsSource = staffersname = db.Staffers.ToList();
        }

        // Метод запускает расчет зарплаты сотрудника/сотрудников при нажатии кнопки x:Name="buttonOk" 
        private void PayrollOutput_Click(object sender, RoutedEventArgs e)
        {
            // Если в ComboBox x:Name="payrollstaffernamesList" не выбрано имя сотрудника
            // или в CheckBox x:Name="checkboxallstaffers" не установлена флажок и не выбрана дата окончания расчетного периода,
            // то пропускаем нажатие кнопки
            if (payrollstaffernamesList.SelectedItem == null && checkboxallstaffers.IsChecked == false
                || calendarEndingDate.SelectedDate == null) return;

            // При нажатии на кнопку "Ok" данные в DataGrid очищаются
            payrolldatagridList.Items.Clear();

            // Если в CheckBox x:Name="checkboxallstaffers" установлен флажок
            if (checkboxallstaffers.IsChecked == true)
            {
                // Убираем выбранное имя в ComboBox x:Name="payrollstaffernamesList"
                payrollstaffernamesList.SelectedItem = null;

                // Переменная для вывода суммарной зарплаты всех сотрудников
                decimal finalAmountsAllStaffers = 0;

                // Из базы данных выгружаем всех сотрудников
                var allstaffersinbase = from allstaff in db.Staffers.Include(s => s.Post)
                                        select allstaff;

                // Получаем из массива сотрудника
                foreach (var v in allstaffersinbase)
                {
                    // Преобразуем выбранный объект из массива к классу Staffer
                    Staffer staffers = v as Staffer;

                    // Проверяем в базе данных, есть ли у сотрудника подчиненные
                    var allsubordinateEmployee = from head in db.Staffers
                                                 where head.Headofdepartment == v.NameStaff
                                                 select head;

                    // Булевская переменная получает значение true, если у сотрудника есть подчиненные
                    bool headsAndEmploees = false;

                    // В данную коллекцию будут добавлены подчиненные выбранного сотрудника
                    ArrayList allSubordinateEmployeeId = new ArrayList();

                    // Если подчиненные есть, тогда добавляем их в коллекцию и переменной headsAndEmploees
                    // передаем значение true
                    foreach (var n in allsubordinateEmployee)
                    {
                        allSubordinateEmployeeId.Add(n.Id);
                        headsAndEmploees = true;
                    }

                    // Если переменная headsAndEmploees хранит значение false
                    // тогда вызываем метод OneEmployee и в переменную finalAmountsAllStaffers передаем зарплату сотрудника
                    // иначе вызываем метод TwoAndMoreEmployees и в переменную finalAmountsAllStaffers передаем зарплату сотрудников
                    if (headsAndEmploees == false)
                    {
                        // Создаем коллекцию, которая получен данные из метода OneEmployee, 
                        // а в метод передаем сотрудника из массива allstaffersinbase
                        ArrayList AllOneEmployeeList = OneEmployee(staffers);

                        finalAmountsAllStaffers += Convert.ToDecimal(AllOneEmployeeList[2]);
                    }
                    else if (headsAndEmploees == true)
                    {
                        // Создаем коллекцию, которая получен данные из метода TwoAndMoreEmployees, 
                        // а в метод передаем сотрудника из массива allstaffersinbase
                        ArrayList AllTwoAndMoreEmployeesList = TwoAndMoreEmployees(staffers);

                        finalAmountsAllStaffers += Convert.ToDecimal(AllTwoAndMoreEmployeesList[2]);
                    }
                }

                // Создаем объект класса Payroll, передаем в него суммарную зарплату сотрудников и 
                // выводим результат в DataGrid x:Name="payrolldatagridList"
                Payroll allPayroll = new Payroll() { FinalAmount = finalAmountsAllStaffers };
                payrolldatagridList.Items.Add(allPayroll);
            }
            else
            {
                // Если в CheckBox x:Name="checkboxallstaffers" не установлен флажок
                // и в ComboBox x:Name="payrollstaffernamesList" выбрано имя сотрудника, тогда
                // выбранный объект в ComboBox x:Name="payrollstaffernamesList" преобразуем к классу Staffer
                Staffer selectedstaffer = payrollstaffernamesList.SelectedItem as Staffer;

                // Булевская переменная получает значение true, если у сотрудника есть подчиненные
                bool headAndEmploees = false;

                // Проверяем в базе данных, есть ли у сотрудника подчиненные
                var subordinateEmployee = from head in db.Staffers
                                          where head.Headofdepartment == selectedstaffer.NameStaff
                                          select head;

                // В данную коллекцию будут добавлены подчиненные выбранного сотрудника
                ArrayList subordinateEmployeeId = new ArrayList();

                // Если подчиненные есть, тогда добавляем их в коллекцию и переменной headAndEmploees
                // передаем значение true
                foreach (var n in subordinateEmployee)
                {
                    subordinateEmployeeId.Add(n.Id);
                    headAndEmploees = true;
                }

                // Если переменная headAndEmploees хранит значение false
                // тогда вызываем метод OneEmployee, иначе вызываем метод TwoAndMoreEmployees
                if (headAndEmploees == false)
                {
                    // Создаем коллекцию, которая получен данные из метода OneEmployee, 
                    // а в метод передаем сотрудника выбранного в ComboBox x:Name="payrollstaffernamesList
                    ArrayList OneEmployeeList = OneEmployee(selectedstaffer);

                    // Создаем объект класса Payroll, передаем в него значения из коллекции OneEmployeeList
                    // и selectedstaffer
                    Payroll payrollOneEmployee = new Payroll()
                    {
                        NameStaff = selectedstaffer.NameStaff,
                        Dateofemployment = selectedstaffer.Dateofemployment,
                        Post = selectedstaffer.Post,
                        Headofdepartment = selectedstaffer.Headofdepartment,
                        Paycheck = selectedstaffer.Paycheck,
                        YearsWorked = Convert.ToInt32(OneEmployeeList[0]),
                        SeniorityPay = Convert.ToDecimal(OneEmployeeList[1]),
                        FinalAmount = Convert.ToDecimal(OneEmployeeList[2])
                    };

                    // Выводим результат в DataGrid x:Name="payrolldatagridList"
                    payrolldatagridList.Items.Add(payrollOneEmployee);
                }
                else if (headAndEmploees == true)
                {
                    // Создаем коллекцию, которая получен данные из метода TwoAndMoreEmployees, 
                    // а в метод передаем сотрудника выбранного в ComboBox x:Name="payrollstaffernamesList
                    ArrayList TwoAndMoreEmployeesList = TwoAndMoreEmployees(selectedstaffer);

                    // Создаем объект класса Payroll, передаем в него значения из коллекции OneEmployeeList
                    // и selectedstaffer
                    Payroll payrollTwoAndMoreEmployees = new Payroll()
                    {
                        NameStaff = selectedstaffer.NameStaff,
                        Dateofemployment = selectedstaffer.Dateofemployment,
                        Post = selectedstaffer.Post,
                        Headofdepartment = selectedstaffer.Headofdepartment,
                        Paycheck = selectedstaffer.Paycheck,
                        YearsWorked = Convert.ToInt32(TwoAndMoreEmployeesList[0]),
                        SeniorityPay = Convert.ToDecimal(TwoAndMoreEmployeesList[1]),
                        FinalAmount = Convert.ToDecimal(TwoAndMoreEmployeesList[2])
                    };

                    // Из базы данных получаем сотрудников которые в подчинении у руководителя 
                    var subordinateEmployees = db.Staffers.Include(x => x.Post).Where(h => h.Headofdepartment == selectedstaffer.NameStaff).ToList();

                    // Выводим результат в DataGrid x:Name="payrolldatagridList"
                    payrolldatagridList.Items.Add(payrollTwoAndMoreEmployees);

                    // Выводим сотрудников в DataGrid x:Name="payrolldatagridList"
                    for (var i = 0; i < subordinateEmployees.Count; i++)
                        payrolldatagridList.Items.Add(subordinateEmployees[i]);
                }
            }
        }

        // Метод рассчитывает зарплату сотрудника, у которого есть подчиненные
        public ArrayList TwoAndMoreEmployees(Staffer staf)
        {
            // Получаем из базы данных значение дополнительной ставки получаемой с зарплаты подчиненных
            var headstaffer = from staffer in db.Staffers.Include(s => s.Post)
                              where staffer.Id == staf.Id
                              select staffer;

            // Переменная будет хранить значение дополнительной ставки
            decimal headStafferRatePlus = 0;
            // Переменная будет хранить название должности
            string namePostHead = null;

            // Передаем в переменную headstafferRatePlus значение дополнительной ставки и название должности 
            foreach (var headstaff in headstaffer)
            {
                headStafferRatePlus = headstaff.Post.RatePlus;
                namePostHead = headstaff.Post.NamePost;
            }

            // В метод OneEmployee передаем выбранного сотрудника(руководителя) и получаем значения его зарплаты
            // без начисления дополнительной ставки
            ArrayList headstafferList = OneEmployee(staf);

            // В данную коллекцию будут добавлены подчиненные выбранного сотрудника
            List<Staffer> subordinateEmployeeId = new List<Staffer>();

            // Из базы данных получаем сотрудников которые в подчинении у руководителя
            if (namePostHead == "Manager")
            {
                // Если у руководителя должность Manager, дополнительный процент он может получать только
                // с сотрудников с должностью Employee
                var subordinateEmployee = db.Staffers.Where(h => h.Headofdepartment == staf.NameStaff)
                                            .Intersect(db.Staffers.Where(h => h.Post.NamePost == "Employee"));

                // Добавляем в коллекцию подчиненных сотрудников
                foreach (var v in subordinateEmployee)
                {
                    subordinateEmployeeId.Add(v);
                }
            }
            else if (namePostHead == "Salesman")
            {
                // Если у руководителя должность Salesman, дополнительный процент он может получать со всех сотрудников
                var subordinateAll = db.Staffers.Where(h => h.Headofdepartment == staf.NameStaff);

                // Добавляем в коллекцию подчиненных сотрудников
                foreach (var v in subordinateAll)
                {
                    subordinateEmployeeId.Add(v);
                }
            }

            // В данную коллекцию будут добавлены зарплаты подчиненных сотрудников
            ArrayList payrollOneEmployeeList = new ArrayList();

            // В коллекцию payrollOneEmployeeList передаем зарплату подчиненных сотрудников
            // полученную из метода OneEmployee
            foreach (Staffer staff in subordinateEmployeeId)
            {
                payrollOneEmployeeList.AddRange(OneEmployee(staff));
            }

            // Переменная хранит суммарную зарплату подчиненных
            decimal finalAmountsubordinateEmployeeId = 0;

            // Из коллекции payrollOneEmployeeList получаем сумму которая больше 1000, так как выслуга лет и базовая ставка 
            // в размере 1000 является исключением(в данных условиях)
            foreach (object obj in payrollOneEmployeeList)
            {
                if (Convert.ToDecimal(obj) > 1000)
                    finalAmountsubordinateEmployeeId += Convert.ToDecimal(obj);
            }

            // Переменная хранит зарплату руководителя полученную из коллекции headstafferList
            decimal finalAmountsHeadStaffer = Convert.ToDecimal(headstafferList[2]);

            // Рассчитываем суммарную зарплату руководителя
            finalAmountsHeadStaffer += finalAmountsubordinateEmployeeId * headStafferRatePlus / 100;

            // Создаем коллекцию в которую передаем значения полученные из коллекции headstafferList
            // и суммарную зарплату из finalAmountsHeadStaffer
            ArrayList payrollTwoAndMoreEmployees = new ArrayList()
            {
                headstafferList[0],
                headstafferList[1],
                finalAmountsHeadStaffer
            };

            // Возвращаем коллекцию из метода
            return payrollTwoAndMoreEmployees;
        }

        // Метод рассчитывает зарплату одного выбранного сотрудника, без подчиненных
        public ArrayList OneEmployee(Staffer staf)
        {
            // Выгружаем из базы данных значения по сотруднику
            var staffers = from staffer in db.Staffers.Include(s => s.Post)
                           where staffer.Id == staf.Id
                           select staffer;

            // Переменные по порядку:
            // дата трудоустройства,
            // зарплата,
            // ставка,
            // максимальная ставка
            string datestart = null;
            decimal paycheck = 0;
            decimal rate = 0;
            decimal maxRate = 0;

            // Передаем в переменные значения по сотруднику
            foreach (var s in staffers)
            {
                datestart = s.Dateofemployment;
                paycheck = s.Paycheck;
                rate = s.Post.Rate;
                maxRate = s.Post.MaxRate;
            }

            // Создаем объект DatePicker и передаем в него значения переменной datestart
            DatePicker calendarDateOfEmployment = new DatePicker();
            calendarDateOfEmployment.Text = datestart;

            // Получаем значения из calendarEndingDate
            calendarEndingDate.SelectedDate.Value.Date.ToShortDateString();

            // Создаем коллекцию arrayListCountMonthsAndYears в которую передаем значения из метода CountMonthsAndYears
            ArrayList arrayListCountMonthsAndYears = CountMonthsAndYears(calendarDateOfEmployment, calendarEndingDate, rate, maxRate);

            // В переменную передаем количество лет из коллекции arrayListCountMonthsAndYears
            int countyearsCountMonthsAndYears = Convert.ToInt32(arrayListCountMonthsAndYears[0]);
            // В переменную передаем количество месяцев из коллекции arrayListCountMonthsAndYears
            int countmonthsCountMonthsAndYears = Convert.ToInt32(arrayListCountMonthsAndYears[1]);
            // В переменную передаем ставку за выслугу лет из коллекции arrayListCountMonthsAndYears
            decimal seniorityPayCountMonthsAndYears = Convert.ToDecimal(arrayListCountMonthsAndYears[2]);

            // В переменную передаем значения полученные из метода EmployeePayroll
            decimal finalAmountEmployeePayroll = EmployeePayroll(paycheck, seniorityPayCountMonthsAndYears, countmonthsCountMonthsAndYears);

            // Создаем коллекцию в которую передаем количество лет, ставку за выслугу лет и зарплату
            ArrayList payrollOneEmployee = new ArrayList()
            {
                countyearsCountMonthsAndYears,
                seniorityPayCountMonthsAndYears,
                finalAmountEmployeePayroll
            };

            // Возвращаем коллекцию из метода
            return payrollOneEmployee;
        }

        // Метод считает зарплату, без дополнительного коэффициента
        public decimal EmployeePayroll(decimal payCheckIn, decimal seniorityPayIn, int countmonths)
        {
            // В переменную передаем сумму зарплаты, ставку за выслугу лет и количество месяцев
            decimal finalAmount = (payCheckIn + (payCheckIn * seniorityPayIn / 100)) * countmonths;

            // Возвращаем значения переменной из метода
            return finalAmount;
        }

        // Метод считает количество лет, месяцев и максимальную ставку
        public ArrayList CountMonthsAndYears(DatePicker startDateOfEmployment, DatePicker endDate, decimal rateCountMonthsAndYears, decimal maxRateIn)
        {
            // Создаем объекты класса DateTime и передаем в них значение DatePicker
            DateTime dateTimeStart = new DateTime();
            // Дата трудоустройства сотрудника
            DateTime dateTimeStartOfEmployment = startDateOfEmployment.SelectedDate.Value;
            // Дата окончания расчетного периода
            DateTime dateTimeEnd = endDate.SelectedDate.Value;

            // Переменная хранит количество отработанных месяцев
            int countMonths = (dateTimeEnd.Month - dateTimeStartOfEmployment.Month) + 12 * (dateTimeEnd.Year - dateTimeStartOfEmployment.Year);

            // Если в DatePicker Name="calendarStartDate" не выбрана дата начала расчетного периода,
            // то в него передается дата трудоустройства сотрудника
            if (calendarStartDate.SelectedDate == null)
            {
                dateTimeStart = startDateOfEmployment.SelectedDate.Value;
            }
            else
            {
                // Если дата выбрана, тогда по ней рассчитывается количество отработанных месяцев сотрудника
                dateTimeStart = calendarStartDate.SelectedDate.Value;
                if (dateTimeStart.Date < dateTimeStartOfEmployment.Date) dateTimeStart = startDateOfEmployment.SelectedDate.Value;

                countMonths = (dateTimeEnd.Month - dateTimeStart.Month) + 12 * (dateTimeEnd.Year - dateTimeStart.Year);
            }

            // Если месяц отрицателен, присваиваем ему значение ноль
            if (countMonths < 0) countMonths = 0;

            // Рассчитываем стаж сотрудника
            int countYears = dateTimeEnd.Year - dateTimeStartOfEmployment.Year;

            // Корректируем годы
            if (dateTimeStartOfEmployment.Month == dateTimeEnd.Month &&
                dateTimeEnd.Day < dateTimeStartOfEmployment.Day ||
                dateTimeEnd.Month < dateTimeStartOfEmployment.Month)
            {
                countYears--;
            }

            // Переменная хранит ставку за выслугу лет
            decimal seniorityPay = 0;


            // Если стаж больше года, прибавляем в переменную процент за выслугу лет,
            // если процент за выслугу лет больше максимального процента за выслугу лет, то
            // в переменную передаем значение максимального процента за выслугу лет
            if (countYears >= 1)
            {
                seniorityPay = (rateCountMonthsAndYears * countYears);
                if (seniorityPay > maxRateIn)
                {
                    seniorityPay = maxRateIn;
                }
            }

            // Создаем коллекцию, передаем в нее количеством лет, месяцев, процентом за выслугу лет
            ArrayList AmountMonthsAndYears = new ArrayList() { countYears, countMonths, seniorityPay };

            // Возвращаем коллекцию из метода
            return AmountMonthsAndYears;
        }
    }
}
