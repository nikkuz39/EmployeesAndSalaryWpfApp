using EmployeesAndSalaryWpfApp.Core;
using EmployeesAndSalaryWpfApp.MVVM.Model;
using Microsoft.EntityFrameworkCore;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EmployeesAndSalaryWpfApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for StafferControl.xaml
    /// </summary>
    public partial class StafferControl : UserControl
    {
        // Данное окно взаимодействует с базой данных и объектами класса Staffer.
        //
        // Создаем списки
        List<Post> posts;
        List<Staffer> staffersheadsList;
        public StafferControl()
        {
            InitializeComponent();

            try
            {
                // Создаем объект класса ApplicationContext для взаимодействия с базой данных
                using (ApplicationContext db = new ApplicationContext())
                {
                    // В ComboBox x:Name="stafferpostsList" передаем название должностей из таблицы Posts
                    stafferpostsList.ItemsSource = posts = db.Posts.ToList();

                    // Загружаем данные из таблица Staffers
                    db.Staffers.Load();

                    // Передаем данные из таблицы в DataGrid
                    this.DataContext = db.Staffers.Local.ToBindingList();

                    // В ComboBox x:Name="headsList" передаем имена сотрудников из таблица Staffers
                    headsList.ItemsSource = staffersheadsList = db.Staffers.Intersect(db.Staffers.Where(h => h.Post.NamePost != "Employee")).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Добавляем данные в таблицу Staffers
        private void StafferAdd_Click(object sender, RoutedEventArgs e)
        {
            // Выбранный объект в ComboBox x:Name="stafferpostsList" преобразуем к классу Post
            Post post = stafferpostsList.SelectedItem as Post;

            // Если в ComboBox x:Name="stafferpostsList" не выбран объект, пропускаем нажатие кнопки
            if (post == null) return;

            // Создаем объект класса Staffer, передаем в него значения из элементов StafferWindow
            Staffer staffer = new Staffer
            {
                NameStaff = namestaffer.Text,
                Dateofemployment = dateofemployment.SelectedDate.Value.Date.ToShortDateString(),
                Headofdepartment = headsList.Text,
                Paycheck = Int32.Parse(paycheck.Text),
                Post = post
            };

            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    // Для сохранения объекта класса Post, в текущем контексте используется метода Attach 
                    db.Posts.Attach(post);
                    // Добавляем данные в таблицу Staffers
                    db.Staffers.Add(staffer);
                    // Сохраняем изменения в таблице
                    if (db.SaveChanges() > 0)
                    {
                        // Загружаем данные из таблица Staffers по ключу PostId в DataGrid
                        staffersList.ItemsSource = db.Staffers.Include(x => x.Post).ToList();
                    }

                    // В ComboBox x:Name="headsList" обновляем имена сотрудников из таблица Staffers 
                    headsList.ItemsSource = staffersheadsList = db.Staffers.Intersect(db.Staffers.Where(h => h.Post.NamePost != "Employee")).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            // Убираем данные из StafferWindow
            namestaffer.Clear();
            dateofemployment.SelectedDate = null;
            headsList.SelectedItem = null;
            paycheck.Clear();
            stafferpostsList.SelectedItem = null;            
        }

        // Редактируем данные в таблице Staffers
        private void StafferEdit_Click(object sender, RoutedEventArgs e)
        {
            // Если в DataGrid не выбран объект, пропускаем нажатие кнопки
            if (staffersList.SelectedItem == null) return;

            Staffer staffer = staffersList.SelectedItem as Staffer;            

            // Передаем копию объекта в окно EditorStaffersWindow
            EditorStaffersWindow editorStaffersWindow = new EditorStaffersWindow(new Staffer
            {
                Id = staffer.Id,
                NameStaff = staffer.NameStaff,
                Dateofemployment = staffer.Dateofemployment,
                Headofdepartment = staffer.Headofdepartment,
                Paycheck = staffer.Paycheck
            });

            // В ComboBox x:Name="stafferpostsEditList" передаем название должностей из таблицы Posts
            editorStaffersWindow.stafferpostsEditList.ItemsSource = posts;
            // В ComboBox x:Name="stafferheadsEditList" передаем имена сотрудников из таблица Staffers
            editorStaffersWindow.stafferheadsEditList.ItemsSource = staffersheadsList;

            // Передаем в ComboBox x:Name="stafferpostsEditList" и x:Name="stafferheadsEditList" текущие значения объекта
            editorStaffersWindow.stafferheadsEditList.SelectedValue = staffer.Headofdepartment;
            editorStaffersWindow.stafferpostsEditList.SelectedValue = staffer.PostId;

            if (editorStaffersWindow.ShowDialog() == true)
            {
                try
                {
                    using (ApplicationContext db = new ApplicationContext())
                    {
                        // В базе данных ищем выбранный объект
                        staffer = db.Staffers.Find(editorStaffersWindow.Staffer.Id);
                        if (staffer != null)
                        {
                            // Если объект находится в базе данных, вносим изменения
                            staffer.NameStaff = editorStaffersWindow.Staffer.NameStaff;
                            staffer.Dateofemployment = editorStaffersWindow.Staffer.Dateofemployment;
                            staffer.Headofdepartment = editorStaffersWindow.stafferheadsEditList.Text;
                            staffer.Paycheck = editorStaffersWindow.Staffer.Paycheck;
                            staffer.Post = (Post)editorStaffersWindow.stafferpostsEditList.SelectedItem;

                            // Указываем, что объект нужно изменить в базе данных
                            db.Entry(staffer).State = EntityState.Modified;
                            // Сохраняем изменения в таблице
                            if (db.SaveChanges() > 0)
                            {
                                // Загружаем данные из таблица Staffers по ключу PostId в DataGrid
                                staffersList.ItemsSource = db.Staffers.Include(x => x.Post).ToList();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }                
            }
        }

        // Удаляем данные в таблице Staffers и DataGrid
        private void StafferDelete_Click(object sender, RoutedEventArgs e)
        {
            // Если в DataGrid не выбран объект, пропускаем нажатие кнопки
            if (staffersList.SelectedItem == null) return;

            Staffer staffer = staffersList.SelectedItem as Staffer;

            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    // Указываем, что объект нужно удалить в базе данных
                    db.Staffers.Remove(staffer);
                    // Сохраняем изменения
                    if (db.SaveChanges() > 0)
                    {
                        // Загружаем данные из таблица Staffers по ключу PostId в DataGrid
                        staffersList.ItemsSource = db.Staffers.Include(x => x.Post).ToList();
                    }

                    // В ComboBox x:Name="headsList" обновляем имена сотрудников из таблица Staffers 
                    headsList.ItemsSource = staffersheadsList = db.Staffers.Intersect(db.Staffers.Where(h => h.Post.NamePost != "Employee")).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }
    }
}
