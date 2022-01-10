using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Kokoava
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
                
        public MainWindow()
        {
            //Kieliasetus lokalisoinnin testausta varten
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fi-FI");
            InitializeComponent();
        }

        private void DrawButton_Click(object sender, RoutedEventArgs e)
        {
            
            var radiobutton = sender as RadioButton;
            string radioBPressed = radiobutton.Name.ToString();
            


            if (radioBPressed == "Piirrä")
            {
                MyCanvas.EditingMode = InkCanvasEditingMode.Ink;
                MyCanvas.UseCustomCursor = true;
                MyCanvas.Cursor = Cursors.Pen;

            }
            else if (radioBPressed == "Pyyhi")
            {
                this.MyCanvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
                MyCanvas.UseCustomCursor = false;

            }
            else if (radioBPressed == "Valitse")
            {
                this.MyCanvas.EditingMode = InkCanvasEditingMode.Select;
                MyCanvas.UseCustomCursor = false;
            }
        }

        private void Puhdista_Ruutu_Button_Click(object sender, RoutedEventArgs e)
        {
            if (MyTextBox.IsVisible)
            {
                MyTextBox.Text = "";
            }
            else
            {

                if (MyCanvas.Strokes.Count != 0)
                {
                    while (MyCanvas.Strokes.Count > 0)
                    {
                        MyCanvas.Strokes.RemoveAt(MyCanvas.Strokes.Count - 1);
                    }
                }
                else
                {

                }
            }
        }

        private void MenuItem_Save_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "isf files (*.isf)|*.isf";

            if (saveFileDialog1.ShowDialog() == true)
            {
                FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.Create);
                MyCanvas.Strokes.Save(fs);
                fs.Close();
            }
        }

        private void MenuItem_Open_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "isf files (*.isf)|*.isf";

                if (openFileDialog1.ShowDialog() == true)
                {
                    FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open);
                    MyCanvas.Strokes = new StrokeCollection(fs);
                    fs.Close();
                }
        }

        private void MyCanvas_MouseEnter(object sender, MouseEventArgs e)
        {
            strokeAttr.Color = (Color)Colorpicker.SelectedColor;
            strokeAttr.Width = Convert.ToDouble(siveltimen_koko.SelectionBoxItem);
            strokeAttr.Height = Convert.ToDouble(siveltimen_koko.SelectionBoxItem);
            
        }

        private void MenuItem_ExitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        
        private void MenuItem_Print_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            PrintDialog prnt = new PrintDialog();

            if (prnt.ShowDialog() == true)
            {
                prnt.PrintVisual(MyCanvas, "Printing InkCanvas");
            }

        }

        private void MenuItem_Extrat_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            var newForm = new Netistalukeminen.MainWindow();

            newForm.Show();
        }

        private void Text_Draw_Button_Click(object sender, RoutedEventArgs e)
        {
            if (MyTextBox.IsVisible)
            {
                var teksti = Properties.Resources.ResourceManager.GetString("Muistivihko");
                MyCanvas.Visibility = Visibility.Visible;
                MyTextBox.Visibility = Visibility.Collapsed;
                teksti_piirrustus_nappi.Content = teksti;
                Piirrä.Visibility = Visibility.Visible;
                Pyyhi.Visibility = Visibility.Visible;
                Valitse.Visibility = Visibility.Visible;
                Colorpicker.Visibility = Visibility.Visible;
                siveltimen_koko.Visibility = Visibility.Visible;
                Menubar.Visibility = Visibility.Visible;
            }
            else
            {
                var teksti = Properties.Resources.ResourceManager.GetString("Piirrä");
                MyCanvas.Visibility = Visibility.Collapsed;
                MyTextBox.Visibility = Visibility.Visible;
                teksti_piirrustus_nappi.Content = teksti;
                Piirrä.Visibility = Visibility.Hidden;
                Pyyhi.Visibility = Visibility.Hidden;
                Valitse.Visibility = Visibility.Hidden;
                Colorpicker.Visibility = Visibility.Hidden;
                siveltimen_koko.Visibility = Visibility.Hidden;
                Menubar.Visibility = Visibility.Hidden;
                
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            File.WriteAllText("SavedData.json", JsonSerializer.Serialize(MyTextBox.Text));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MyTextBox.Text = JsonSerializer.Deserialize<String>(File.ReadAllText("SavedData.json"));
        }
    }
}
