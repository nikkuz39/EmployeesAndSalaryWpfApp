using EmployeesAndSalaryWpfApp.MVVM.Model;
using System;
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
using System.Windows.Shapes;

namespace EmployeesAndSalaryWpfApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for EditorStaffersWindow.xaml
    /// </summary>
    public partial class EditorStaffersWindow : Window
    {
        // Окно EditorStaffersWindow редактирует данные объекта класса Staffer.
        // Для передачи данных между окнами, используем DataContext и элемент Binding 
        public Staffer Staffer { get; private set; }
        public EditorStaffersWindow(Staffer s)
        {
            InitializeComponent();

            Staffer = s;
            this.DataContext = Staffer;
        }

        // Если диалоговое окно закрывается кнопкой "Ok" - данные передаются обратно объекту класса Staffer,
        // если диалоговое окно закрывается кнопкой "Cancel" - данные в диалоговом окне стираются и изменение не происходит.
        private void EditorStafferAccept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
