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
using EmployeesAndSalaryWpfApp.Core;
using EmployeesAndSalaryWpfApp.MVVM.Model;
using Microsoft.EntityFrameworkCore;

namespace EmployeesAndSalaryWpfApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for PostControl.xaml
    /// </summary>
    public partial class PostControl : UserControl
    {
        public PostControl()
        {
            InitializeComponent();

            try
            {
                // Создаем объект класса ApplicationContext для взаимодействия с базой данных
                using (ApplicationContext db = new ApplicationContext())
                {
                    // Загружаем данные из таблица Posts
                    db.Posts.Load();

                    // Передаем данные из таблицы в DataGrid
                    this.DataContext = db.Posts.Local.ToBindingList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Добавляем данные в таблицу Posts
        private void PostAdd_Click(object sender, RoutedEventArgs e)
        {
            // Вызываем окно EditorPostWindow и передаем в него объект класса Post
            EditorPostWindow editorPostWindow = new EditorPostWindow(new Post());
            if (editorPostWindow.ShowDialog() == true)
            {
                try
                {
                    using (ApplicationContext db = new ApplicationContext())
                    {
                        Post post = editorPostWindow.Post;
                        // Добавляем данные в таблицу Posts
                        db.Posts.Add(post);
                        // Сохраняем изменения в таблице
                        db.SaveChanges();
                        // Загружаем данные из таблица Posts
                        postsList.ItemsSource = db.Posts.ToList();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // Редактируем данные в таблице Posts
        private void PostEdit_Click(object sender, RoutedEventArgs e)
        {
            // Если в DataGrid не выбран объект, пропускаем нажатие кнопки
            if (postsList.SelectedItem == null) return;

            // Выбранный объект в DataGrid преобразуем к классу Post
            Post post = postsList.SelectedItem as Post;

            // Передаем копию объекта в окно EditorPostWindow
            EditorPostWindow editorPostWindow = new EditorPostWindow(new Post
            {
                Id = post.Id,
                NamePost = post.NamePost,
                Rate = post.Rate,
                MaxRate = post.MaxRate,
                RatePlus = post.RatePlus
            });

            if (editorPostWindow.ShowDialog() == true)
            {
                try
                {
                    using (ApplicationContext db = new ApplicationContext())
                    {
                        // В базе данных ищем выбранный объект
                        post = db.Posts.Find(editorPostWindow.Post.Id);
                        // Если объект находится в базе данных, вносим изменения
                        if (post != null)
                        {
                            post.NamePost = editorPostWindow.Post.NamePost;
                            post.Rate = editorPostWindow.Post.Rate;
                            post.MaxRate = editorPostWindow.Post.MaxRate;
                            post.RatePlus = editorPostWindow.Post.RatePlus;

                            // Указываем, что объект нужно изменить в базе данных
                            db.Entry(post).State = EntityState.Modified;
                            // Сохраняем изменения
                            db.SaveChanges();
                            // Загружаем данные из таблица Posts
                            postsList.ItemsSource = db.Posts.ToList();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }                
            }
        }

        // Удаляем данные в таблице Posts и DataGrid
        private void PostDelete_Click(object sender, RoutedEventArgs e)
        {
            // Если в DataGrid не выбран объект, пропускаем нажатие кнопки
            if (postsList.SelectedItem == null) return;

            // Выбранный объект в DataGrid преобразуем к классу Post
            Post post = postsList.SelectedItem as Post;

            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    // Указываем, что объект нужно удалить в базе данных
                    db.Posts.Remove(post);
                    // Сохраняем изменения
                    db.SaveChanges();
                    // Загружаем данные из таблица Posts
                    postsList.ItemsSource = db.Posts.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
