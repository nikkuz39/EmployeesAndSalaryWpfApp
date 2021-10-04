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
    /// Interaction logic for EditorPostWindow.xaml
    /// </summary>
    public partial class EditorPostWindow : Window
    {
        // Окно EditorPostWindow создает/редактирует данные объекта класса Post.
        // Для передачи данных между окнами, используем DataContext и элемент Binding 
        public Post Post { get; private set; }

        public EditorPostWindow(Post p)
        {
            InitializeComponent();

            Post = p;
            this.DataContext = Post;
        }

        // Если диалоговое окно закрывается кнопкой "Ok" - данные передаются обратно объекту класса Post,
        // если диалоговое окно закрывается кнопкой "Cancel" - данные в диалоговом окне стираются и изменение не происходит.
        private void EditorPostAccept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
