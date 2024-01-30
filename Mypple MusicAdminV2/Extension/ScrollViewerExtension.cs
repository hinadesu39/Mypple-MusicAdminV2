using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Documents;

namespace Mypple_MusicAdminV2.Extension
{
    public static class ScrollViewerExtension
    {
        #region VerticalOffset

        // 定义一个附加依赖属性 VerticalOffsetProperty
        public static readonly DependencyProperty VerticalOffsetProperty =
            DependencyProperty.RegisterAttached(
                "VerticalOffset",
                typeof(double),
                typeof(ScrollViewerExtension),
                new UIPropertyMetadata(0.0, OnVerticalOffsetChanged)
            );

        // 获取 VerticalOffsetProperty 的值
        public static double GetVerticalOffset(DependencyObject obj)
        {
            return (double)obj.GetValue(VerticalOffsetProperty);
        }

        // 设置 VerticalOffsetProperty 的值
        public static void SetVerticalOffset(DependencyObject obj, double value)
        {
            obj.SetValue(VerticalOffsetProperty, value);
        }

        // 当 VerticalOffsetProperty 的值发生变化时，更新 ScrollViewer 的垂直偏移量
        private static void OnVerticalOffsetChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e
        )
        {
            var viewer = d as ScrollViewer;
            if (viewer != null)
            {
                viewer.ScrollToVerticalOffset((double)e.NewValue);
            }
        }

        // 定义一个扩展方法 ScrollToVerticalOffsetWithAnimation
        public static void ScrollToVerticalOffsetWithAnimation(
            this ScrollViewer viewer,
            double offset,
            TimeSpan duration,
            IEasingFunction easingFunction
        )
        {
            //viewer.BeginAnimation(VerticalOffsetProperty, null);
            // 创建一个 DoubleAnimation 对象
            var animation = new DoubleAnimation();
            animation.To = offset; // 设置目标的垂直偏移量
            animation.Duration = duration; // 设置持续时间
            animation.EasingFunction = easingFunction; // 设置缓动函数
            // 将动画应用到 VerticalOffsetProperty 上
            viewer.BeginAnimation(VerticalOffsetProperty, animation);
        }

        #endregion

        #region ScrollUp

        public static bool GetScrollUp(DependencyObject obj)
        {
            return (bool)obj.GetValue(ScrollUpProperty);
        }

        public static void SetScrollUp(DependencyObject obj, bool value)
        {
            obj.SetValue(ScrollUpProperty, value);
        }

        // Using a DependencyProperty as the backing store for ScrollUp.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScrollUpProperty =
            DependencyProperty.RegisterAttached(
                "ScrollUp",
                typeof(bool),
                typeof(ScrollViewerExtension),
                new PropertyMetadata(false, OnScrollUpChanged)
            );

        private static void OnScrollUpChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e
        )
        {
            if (d != null && d is Button)
            {
                var button = d as Button;
                button.Click += ScrollUpClick;
            }
        }

        private static void ScrollUpClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            UserControl userControl = GetParentObject<UserControl>(button!);
            if (userControl != null)
            {
                ScrollViewer scrollViewer = FindVisualChild<ScrollViewer>(userControl);
                if (scrollViewer != null)
                {
                    scrollViewer.ScrollToVerticalOffsetWithAnimation(
                        0,
                        TimeSpan.FromSeconds(1),
                        new CircleEase()
                    );
                }
            }
        }

        #endregion

        #region ScrollDown

        public static bool GetScrollDown(DependencyObject obj)
        {
            return (bool)obj.GetValue(ScrollDownProperty);
        }

        public static void SetScrollDown(DependencyObject obj, bool value)
        {
            obj.SetValue(ScrollDownProperty, value);
        }

        // Using a DependencyProperty as the backing store for ScrollDown.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScrollDownProperty =
            DependencyProperty.RegisterAttached(
                "ScrollDown",
                typeof(bool),
                typeof(ScrollViewerExtension),
                new PropertyMetadata(false, OnScrollDownChanged)
            );

        private static void OnScrollDownChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e
        )
        {
            if (d != null && d is Button)
            {
                var button = d as Button;
                button.Click += ScrollDownClick;
            }
        }

        private static void ScrollDownClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            UserControl userControl = GetParentObject<UserControl>(button!);
            if (userControl != null)
            {
                ScrollViewer scrollViewer = FindVisualChild<ScrollViewer>(userControl);
                if (scrollViewer != null)
                {
                    scrollViewer.ScrollToVerticalOffsetWithAnimation(
                        scrollViewer.ScrollableHeight,
                        TimeSpan.FromSeconds(1),
                        new CircleEase()
                    );
                }
            }
        }

        #endregion

        #region FindChild&Parent
        /// <summary>
        /// 通过遍历可视化树获取到Listbox中的ScrollViewer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static T FindVisualChild<T>(DependencyObject parent)
            where T : DependencyObject
        {
            if (parent != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                    if (child != null && child is T)
                    {
                        return (T)child;
                    }
                    T childItem = FindVisualChild<T>(child);
                    if (childItem != null)
                    {
                        return childItem;
                    }
                }
            }
            return null;
        }

        public static T GetParentObject<T>(DependencyObject child)
            where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);
            while (parent != null)
            {
                if (parent is T && parent != null)
                {
                    return (T)parent;
                }
                // 在上一级父控件中没有找到指定的控件，就再往上一级找
                parent = VisualTreeHelper.GetParent(parent);
            }
            return null;
        }

        #endregion
    }
}
