﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfCours.MVVM.ViewModel;
using WpfCours.Back;

namespace WpfCours
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowVM mainWindowVM { get; set; }   
        public MainWindow()
        {
            InitializeComponent();
            GlobalVars.GetInstance();

            //create mainwindow view model
            mainWindowVM = new MainWindowVM();

            //Assign VM to datacontext
            //=> View can acces to variables to VM;
            DataContext = mainWindowVM;
        }

       
    }
}