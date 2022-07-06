using GamePluginLauncher.Model;
using GamePluginLauncher.Utils;
using GamePluginLauncher.View.Dialogs;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using SimpleMvvm;
using SimpleMvvm.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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

namespace GamePluginLauncher.Carousel
{
    /// <summary>
    /// AnimImage.xaml 的交互逻辑
    /// </summary>
    public partial class AnimImage : UserControl
    {
        public int LauncherId { get; set; }
        public int PluginId { get; set; }

        private GamePlugin _gamePlugin;
        public GamePlugin GamePlugin
        {
            get => _gamePlugin;
            set
            {
                _gamePlugin = value;
            }
        }
        //public string PluginName { 
        //    get => PluginName;
        //    set
        //    {
        //        PluginName = value;
        //        ConnectStrChanged?.Invoke(null, EventArgs.Empty);
        //    }
        //}
        //public string PluginPath { get; set; }
        //public string PluginBackgroundPath { get; set; }
        //public GamePlugin gamePlugin
        //{
        //    get => StaticData.GameLaunchers[LauncherId].GamePlugins[PluginId];
        //    set => StaticData.GameLaunchers[LauncherId].GamePlugins[PluginId] = value;
        //}

        public bool Is180
        {
            get { return (bool)GetValue(Is180Property); }
            set { SetValue(Is180Property, value); }
        }
        public static readonly DependencyProperty Is180Property =
            DependencyProperty.Register("Is180", typeof(bool), typeof(AnimImage),
                new PropertyMetadata(false));
        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register("X", typeof(double), typeof(AnimImage), new UIPropertyMetadata(0.0));

        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }
        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register("Y", typeof(double), typeof(AnimImage), new UIPropertyMetadata(0.0));

        public double ScaleX
        {
            get { return (double)GetValue(ScaleXProperty); }
            set { SetValue(ScaleXProperty, value); }
        }
        public static readonly DependencyProperty ScaleXProperty =
            DependencyProperty.Register("ScaleX", typeof(double), typeof(AnimImage), new UIPropertyMetadata(1.0));

        public double ScaleY
        {
            get { return (double)GetValue(ScaleYProperty); }
            set { SetValue(ScaleYProperty, value); }
        }
        public static readonly DependencyProperty ScaleYProperty =
            DependencyProperty.Register("ScaleY", typeof(double), typeof(AnimImage), new UIPropertyMetadata(1.0));

        public double Degree { set; get; }
        
        private string FileSrc = "";

        private bool _IsVisible = false;
        public new bool IsVisible
        {
            get { return _IsVisible; }
            set
            {
                _IsVisible = value;
                if (value)
                    this.LoadUiImmediate();
            }
        }

        private bool IsUiLoaded = false;

        public void LoadUiImmediate()
        {
            if (!IsUiLoaded)
            {
                IsUiLoaded = true;

                try
                {
                    if (File.Exists(FileSrc))
                        this.ImgMain.Source = new BitmapImage(new Uri(FileSrc));
                }
                catch { }
            }
        }

        public AnimImage(string BackgroundPath)
        {
            InitializeComponent();
            this.FileSrc = BackgroundPath;
            this.Loaded += ImageItem_Loaded;
            this.DataContext = this;

            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);
            CheckTextChangeCommand = new DelegateCommand(CheckTextChange);
            EditBackgroundPathCommand = new DelegateCommand(EditBackgroundPath);
        }

        private void ImageItem_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= ImageItem_Loaded;
            AsynchUtils.AsynchSleepExecuteFunc(this.Dispatcher, LoadUiImmediate, 0.5);
        }

        private void Save()
        {
            string name = txtPluginName.Text;
            string path = txtPluginPath.Text;
            string backgroundpath = txtBackgroundPath.Text;

            GamePlugin.Name = name;
            if (File.Exists(path))
            {
                GamePlugin.Path = path;
            }
            else
            {
                txtPluginPath.Text = GamePlugin.Path;
                if (SnackbarTips.MessageQueue is { } messageQueue)
                {
                    string message = "插件路径不正确或不存在";
                    Task.Factory.StartNew(() => messageQueue.Enqueue(message));
                }
            }
            if (File.Exists(backgroundpath))
            {
                if (GamePlugin.BackgroundPath != backgroundpath)
                {
                    ImgMain.Source = new BitmapImage(new Uri(backgroundpath));
                }
                GamePlugin.BackgroundPath = backgroundpath;
            }
            else
            {
                txtBackgroundPath.Text = GamePlugin.BackgroundPath;
                if (SnackbarTips.MessageQueue is { } messageQueue)
                {
                    string message = "图片路径不正确或不存在";
                    Task.Factory.StartNew(() => messageQueue.Enqueue(message));
                }
                 
            }
            if (File.Exists(backgroundpath) && File.Exists(path))
            {
                if (SnackbarTips.MessageQueue is { } messageQueue)
                {
                    string message = "保存成功";
                    Task.Factory.StartNew(() => messageQueue.Enqueue(message));
                }
            }
            btnSave.IsEnabled = false;
            btnCancel.IsEnabled = false;
        }
        private void Cancel()
        {
            txtPluginName.Text = GamePlugin.Name;
            txtPluginPath.Text = GamePlugin.Path;
            txtBackgroundPath.Text = GamePlugin.BackgroundPath;

        }
        private void CheckTextChange()
        {
            bool nameIsChange = txtPluginName.Text == GamePlugin.Name;
            bool pathIsChange = txtPluginPath.Text == GamePlugin.Path;
            bool backgroundIsChange = txtBackgroundPath.Text == GamePlugin.BackgroundPath;
            if(nameIsChange && pathIsChange && backgroundIsChange)
            {
                btnCancel.IsEnabled = false;
                btnSave.IsEnabled = false;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(txtPluginName.Text) || string.IsNullOrWhiteSpace(txtPluginPath.Text) ||
                    string.IsNullOrWhiteSpace(txtBackgroundPath.Text)) 
                {
                    btnSave.IsEnabled = false;
                }
                else
                {
                    btnSave.IsEnabled = true;
                }
                btnCancel.IsEnabled = true;
                
            }
        }

        private void EditBackgroundPath(object obj)
        {
            try
            {
                var ofd = new OpenFileDialog
                {
                    Filter = "图片文件 (*.jpg;*.JPG;*.jpeg;*.JPEG;*.png;*.PNG;*.gif;*.GIF)|*.jpg;*.JPG;*.jpeg;*.JPEG;*.png;*.PNG;*.gif;*.GIF"
                };
                if (ofd.ShowDialog() == true)
                {
                    ((TextBox)obj).Text = ofd.FileName;
                }
            }
            catch (Exception ex)
            {
                MsgBoxHelper.ShowError(ex.Message);
            }
        }
        

        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CheckTextChangeCommand { get; set; }
        public DelegateCommand EditBackgroundPathCommand { get; set; }
        public DelegateCommand RemoveGamePluginCommand { get; set; }
    }
}
