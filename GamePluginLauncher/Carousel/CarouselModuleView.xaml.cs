using GamePluginLauncher.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GamePluginLauncher.Carousel
{
    /// <summary>
    /// CarouselModuleView.xaml 的交互逻辑
    /// </summary>
    public partial class CarouselModuleView : UserControl
    {
        public GameLauncher gameLauncher => (GameLauncher)DataContext;

        public ObservableCollection<GamePlugin> plugins
        {
            get => gameLauncher.GamePlugins;
        }
        public CarouselModuleView()
        {
            InitializeComponent();
            //this.LoadImgList();
            this.Loaded += CarouselModuleView_Loaded;
            this.Unloaded += CarouselModuleView_Unloaded;
        }

        private void CarouselModuleView_Unloaded(object sender, RoutedEventArgs e)
        {
            CompositionTarget.Rendering -= new EventHandler(CompositionTarget_Rendering);
        }

        private void CarouselModuleView_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= CarouselModuleView_Loaded;

            this.CreateElements();

            this.GdRoot.MouseLeftButtonDown += GdRoot_MouseLeftButtonDown;
            //this.MouseMove += Carousel2DView_MouseMove;
            this.MouseUp += Carousel2DView_MouseUp;
            RotateToSelectItem();
        }

        #region Create Elements

        private double Radius = 325d;
        private double VisualCount = 6d;//10d 推测是可见数量的二倍
        private List<AnimImage> ElementList;
        private double CenterDegree = 180d;
        private double TotalDegree = 0;
        private double ElementWidth = 444;//260
        private double ElementHeight = 250;//180

        private double GetScaledSize(double degrees)
        {
            return GetCoefficient(degrees);
        }

        private double GetCoefficient(double degrees)
        {
            return 1.0 - Math.Cos(ConvertToRads(degrees)) / 2.0 - 0.5;
        }

        private double ConvertToRads(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

        private int GetZValue(double degrees)
        {
            return (int)((360 * GetCoefficient(degrees)) * 1000);
        }

        private void CreateElements()
        {
            //设计模式时可见，如果没有会报错，但可以运行
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = new GameLauncher()
                {
                    SelectPluginId = 0,
                    Name = "启动器",
                    Id = 0,
                    GamePlugins = new ObservableCollection<GamePlugin>()
                    {
                        new GamePlugin(){BackgroundPath=Directory.GetCurrentDirectory()+"\\GamePluginLauncher\\Image\\background1.jpg",Name="1号",Path=""},
                        new GamePlugin(){BackgroundPath=Directory.GetCurrentDirectory()+"\\GamePluginLauncher\\Image\\background2.jpg",Name="2号",Path=""},
                        new GamePlugin(){BackgroundPath=Directory.GetCurrentDirectory()+"\\GamePluginLauncher\\Image\\background3.jpg",Name="3号",Path=""}
                    }
                };
            }
            double dAverageDegree = 360d / VisualCount;

            this.TotalDegree = plugins.Count * dAverageDegree;
            this.ElementList = new List<AnimImage>();
            for (int i = 0; i < plugins.Count; i++)
            {
                string BackgroundPath = plugins[i].BackgroundPath;
                AnimImage oItem = new AnimImage(BackgroundPath);
                oItem.LauncherId = gameLauncher.Id;
                oItem.PluginId = plugins[i].Id;
                oItem.GamePlugin = plugins[i];
                oItem.MouseLeftButtonDown += OItem_MouseLeftButtonDown;
                oItem.MouseLeftButtonUp += OItem_MouseLeftButtonUp;
                oItem.Width = this.ElementWidth;
                oItem.Height = this.ElementHeight;
                oItem.Y = 0d;
                oItem.Degree = i * dAverageDegree;
                oItem.Is180 = false;
                this.ElementList.Add(oItem);
            }

            this.UpdateLocation();
        }

        private AnimImage CurNavItem;

        private void OItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this.IsMouseDown && CurNavItem == sender && this.TotalMoveDegree < 50)
            {
                this.InertiaDegree = CenterDegree - this.CurNavItem.Degree;
                this.CurNavItem = null;
                this.IsMouseDown = false;
                if (this.InertiaDegree != 0)
                    CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
                e.Handled = true;
                gameLauncher.SelectPluginId = ((AnimImage)sender).PluginId;
            }
        }

        private void RotateToSelectItem()
        {
            int index = 0;
            var count = plugins.Where(it => it.Id == gameLauncher.SelectPluginId).Count();

            if(count > 0) 
                index = plugins.IndexOf(plugins.Where(it => it.Id == gameLauncher.SelectPluginId).First());
            else 
                index = 0; 

            this.InertiaDegree = CenterDegree - ElementList[index].Degree;
            if (this.InertiaDegree != 0)
                CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
        }

        private void OItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CurNavItem = sender as AnimImage;
        }

        private void UpdateLocation()
        {
            for (int i = 0; i < this.ElementList.Count; i++)
            {
                AnimImage oItem = this.ElementList[i];

                if (this.ElementList.Count == 1)
                    oItem.Degree = 180;
                else if (oItem.Degree - this.CenterDegree >= this.TotalDegree / 2d)
                    oItem.Degree -= this.TotalDegree;
                else if (this.CenterDegree - oItem.Degree > this.TotalDegree / 2d)
                    oItem.Degree += this.TotalDegree;

                if (oItem.Degree >= 90d && oItem.Degree < 270d) // Degree 在90-270之间的显示
                    this.SetElementVisiable(oItem);
                else
                    this.SetElementInvisiable(oItem);
            }
        }

        private void SetElementVisiable(AnimImage oItem)
        {
            if (oItem == null)
                return;

            if (!oItem.IsVisible)
            {
                if (!this.CvMain.Children.Contains(oItem))
                {
                    oItem.IsVisible = true;
                    this.CvMain.Children.Add(oItem);
                }
            }

            this.DoUpdateElementLocation(oItem);
        }

        private void SetElementInvisiable(AnimImage oItem)
        {
            if (oItem.IsVisible)
            {
                if (this.CvMain.Children.Contains(oItem))
                {
                    this.CvMain.Children.Remove(oItem);
                    oItem.IsVisible = false;
                }
            }
        }

        public void DoUpdateElementLocation(AnimImage oItem)
        {
            double CenterX = this.GdRoot.ActualWidth / 2.0;
            double dX = -Radius * Math.Sin(ConvertToRads(oItem.Degree));
            oItem.X = (dX + CenterX - this.ElementWidth / 2d);
            double dScale = GetScaledSize(oItem.Degree);
            oItem.ScaleX = dScale;
            oItem.ScaleY = dScale;

            if (Math.Abs(oItem.Degree - 180d) < 5)
            {
                oItem.Is180 = true;
            }
            else
            {
                oItem.Is180 = false;
            }
            int nZIndex = GetZValue(oItem.Degree);
            Canvas.SetZIndex(oItem, nZIndex);
        }

        #endregion

        #region Drag And Move

        private bool IsMouseDown = false;
        private double PreviousX = 0;
        private double CurrentX = 0;
        private double IntervalDegree = 0;
        private double InertiaDegree = 0;
        private double TotalMoveDegree = 0;

        private void GdRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.IsMouseDown = true;
            this.IntervalDegree = 0;
            this.PreviousX = e.GetPosition(this).X;
            this.TotalMoveDegree = 0;
            CompositionTarget.Rendering -= new EventHandler(CompositionTarget_Rendering);
        }

        //private void Carousel2DView_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (this.IsMouseDown)
        //    {
        //        this.CurrentX = e.GetPosition(this).X;
        //        this.IntervalDegree = this.CurrentX - this.PreviousX;
        //        this.TotalMoveDegree += Math.Abs(this.IntervalDegree * 0.5d);
        //        this.InertiaDegree = this.IntervalDegree * 5d;

        //        for (int i = 0; i < this.ElementList.Count; i++)
        //        {
        //            AnimImage oItem = this.ElementList[i];
        //            oItem.Degree += this.IntervalDegree;
        //        }
        //        this.UpdateLocation();
        //        this.PreviousX = this.CurrentX;
        //    }
        //}

        private void Carousel2DView_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.IsMouseDown)
            {
                this.IsMouseDown = false;
                this.CurNavItem = null;
                if (this.InertiaDegree != 0)
                {
                    CompositionTarget.Rendering -= new EventHandler(CompositionTarget_Rendering);
                    CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
                }
            }
        }

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            double dIntervalDegree = this.InertiaDegree * 0.1;
            for (int i = 0; i < this.ElementList.Count; i++)
            {
                AnimImage oItem = this.ElementList[i];
                oItem.Degree += dIntervalDegree;
            }
            this.UpdateLocation();

            this.InertiaDegree -= dIntervalDegree;
            if (Math.Abs(this.InertiaDegree) < 0.1)
                CompositionTarget.Rendering -= new EventHandler(CompositionTarget_Rendering);
        }

        #endregion
    }
}
