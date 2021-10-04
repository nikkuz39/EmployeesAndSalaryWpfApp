using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeesAndSalaryWpfApp.Core;

namespace EmployeesAndSalaryWpfApp.MVVM.ViewModel
{
    class MaInViewModel : ObservableObj
    {
        public RelayCommand PayrollViewCommand { get; set; }
        public RelayCommand PostViewCommand { get; set; }
        public RelayCommand StafferViewCommand { get; set; }

        public PayrollViewModel PayrollVM { get; set; }
        public PostViewModel PostVM { get; set; }
        public StafferViewModel StafferVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MaInViewModel()
        {
            PayrollVM = new PayrollViewModel();
            PostVM = new PostViewModel();
            StafferVM = new StafferViewModel();

            PayrollViewCommand = new RelayCommand(o =>
            {
                CurrentView = PayrollVM;
            });

            PostViewCommand = new RelayCommand(o =>
            {
                CurrentView = PostVM;
            });

            StafferViewCommand = new RelayCommand(o =>
            {
                CurrentView = StafferVM;
            });
        }
    }
}