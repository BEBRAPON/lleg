using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;

namespace lleg
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        DispatcherTimer _timer;
        TimeSpan _time;

        public List <string> path = new List<string> { };
        public int selct1;
        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Tick += timer_tick;

            
        }
        private void timer_tick(object sender, EventArgs e)
        {
            string str = Time();
            time1.Text = str;
            time.Text = media.Position.ToString(@"hh\:mm\:ss");
            if (time.Text == str && nado != true) {
                selct1 += 1;
                if (selct1 > path.Count - 1)
                    selct1 = 0;
                media.Source = new Uri(path[selct1]);
                media.Play();
            }
            if (time.Text == str && nado == true)
            {
                media.Source = new Uri(path[selct1]);
                media.Play();
            }
            slider.Value = media.Position.TotalSeconds;
        }
        private CommonOpenFileDialog fileChoice = new CommonOpenFileDialog { IsFolderPicker = true };
        private void choiceBut_Click(object sender, RoutedEventArgs e)
        {
            var result = fileChoice.ShowDialog();

            string fr;
            string genious;
            string genious1;


            if (result == CommonFileDialogResult.Ok)
            {
                string[] files = Directory.GetFiles(fileChoice.FileName);
                foreach ( string file in files )
                {
                    genious = file;
                    genious1 = genious.Substring(genious.Length - 3);
                    if (genious1 == "mp3" || genious1 == "mp4" || genious1 == "mpg" || genious1 == "wav" || genious1 == "wmv" || genious1 == "avi")
                    {
                        path.Add(file);
                        fr = System.IO.Path.GetFileNameWithoutExtension(file);
                        listb.Items.Add(fr);
                    }
                }
            }
        }
        private void listb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { 
            int selct = listb.SelectedIndex;
            selct1 = selct;
            check();

            
            media.Source = new Uri(path[selct1]);
            media.Play();

            
            /*_time = TimeSpan.FromSeconds(500);

            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                time1.Text = _time.ToString("c");
                if (_time == TimeSpan.Zero) _timer.Stop();
                _time = _time.Add(TimeSpan.FromSeconds(-1));
            }, Application.Current.Dispatcher);

            _timer.Start();*/
            timer.Start();

            media.Volume = 0.1;
        }
        private void check()
        {
            if (selct1 == path.Count - 1)
            {
                next.IsEnabled = false;
                back.IsEnabled = true;
            }

            if (selct1 == 0)
            {
                back.IsEnabled = false;
                next.IsEnabled = true;
            }

            if (selct1 != 0 && selct1 != path.Count - 1)
            {
                next.IsEnabled = true;
                back.IsEnabled = true;
            }
        }
        private string Time()
        {
            string times = media.NaturalDuration.ToString();
            for (int i = times.Length; i > 7;  i--)
            {
                times = times.Remove(i);
            }
            return times;
        }
        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            media.Pause();
            media.Position = TimeSpan.FromSeconds(slider.Value);
            media.Play();
        }
        private void med(object sender, RoutedEventArgs e)
        {
            slider.Maximum = media.NaturalDuration.TimeSpan.TotalSeconds;
        }
        private void stop_Click(object sender, RoutedEventArgs e)
        {
            media.Pause();
            timer.Stop();
        }
        private void play_Click(object sender, RoutedEventArgs e)
        {
            media.Play();
            timer.Start();
        }
        private void next_Click(object sender, RoutedEventArgs e)
        {
            selct1 += 1;
            back.IsEnabled = true;
            media.Source = new Uri(path[selct1]);
            if (selct1 == path.Count - 1)
            {
                next.IsEnabled = false;
            }
        }
        private void back_Click(object sender, RoutedEventArgs e)
        {
            selct1 -= 1;
            next.IsEnabled = true;
            media.Source = new Uri(path[selct1]);
             if (selct1 == 0)
             {
                back.IsEnabled = false;
             }
        }
        private void rand_Click(object sender, RoutedEventArgs e)
        {
                Random random = new Random();
            for (int i = path.Count - 1; i >= 1; i--)
            {
                int j = random.Next(i + 1);
                var temp = path[j];
                path[j] = path[i];
                path[i] = temp;
            }
        }

        private bool nado = false;
        private bool nado2 = true;
        private void res_Click(object sender, RoutedEventArgs e)
        {
            (nado, nado2) = (nado2, nado);
        }
    }
}
