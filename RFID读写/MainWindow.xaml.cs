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
using System.IO.Ports;
using System.Collections;
using System.Runtime.InteropServices;
using ISO15693DLL;/*引用命名空间！*/
namespace RFID读写
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
           ISO15693DLL.ISO15693Reader reader = new ISO15693DLL.ISO15693Reader();/*创建实例*/

        int a = 0;/*定义一个变量用于后方判断读取成功提示信息框的数量。*/
        public MainWindow()
        {
            InitializeComponent();
            MessageBox.Show("欢迎使用本读卡器 V0.0.1 Beta\n当前仅支持ISO15693卡片读取\nMade by Cjw", "Cjw提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        


        private void RB_ck_Click(object sender, RoutedEventArgs e)
        {
            if(RB_ck.IsChecked==true)/*判断串口单选框是否被选定，如果被选定则将按钮内容修改*/
            {
                Button_Open.Content = ("打开串口");
                Button_Close.Content = ("关闭串口");
            }
        }

        private void RB_USB_Checked(object sender, RoutedEventArgs e)/*判断USB单选框是否被选择，如果被选择，则按钮内容修改。*/
        {
            if (RB_USB.IsChecked == true)
            {
                  Button_Open.Content = ("打开USB口");
                  Button_Close.Content = ("关闭USB口");
            }
        }
   
        private void Button_Open_Click(object sender, RoutedEventArgs e)
        {
            /*判断，如果当前选定为串口，因为我没有写串口，所以，报错！*/
            if(Button_Open.Content.ToString()=="打开串口")
            {
                MessageBox.Show("当前暂时不支持串口功能！","Cjw提示",MessageBoxButton.OK,MessageBoxImage.Error);
            }
            else
                /*打开的是USB口，如果返回0x00则为成功，否则失败*/
                if(reader.OpenHIDDevice()==0x00)
                {
                    MessageBox.Show("打开USB口成功","Cjw提示",MessageBoxButton.OK,MessageBoxImage.Information);
                Button_Open.Content = "USB口已打开";
                Button_Open.IsEnabled = false;
                Button_Close.IsEnabled = true;
                Button_Read.IsEnabled = true;
                }
                else
                    MessageBox.Show("打开USB口失败\n请检查数据线是否连接好", "Cjw提示", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Button_Read_Click(object sender, RoutedEventArgs e)
        {   /*定义一个变量用于存放返回卡片的数量*/
            Int32 TagCount=0;
                /*定义一个字符串*/
            String[] TagNumber = new String[1];
            if (reader.Inventory(ModulateMethod.ASK, InventoryModel.Single, ref TagCount, ref TagNumber)==0x00)
            {
                Card_Numberbox.Text=TagNumber[0];
                if (a == 3)
                    Mess.Visibility = Visibility.Visible;
               
                
                 if (Mess.IsChecked==false)
                    MessageBox.Show("读取成功！", "Cjw提示", MessageBoxButton.OK, MessageBoxImage.Information);
                CardId.Items.Add(DateTime.Now.ToLongTimeString().ToString()+"  "+TagNumber[0]);
                a++;
            }
            else
            {
                MessageBox.Show("读取失败！\n请检查卡片是否放稳或卡片协议是否为ISO15693", "Cjw提示", MessageBoxButton.OK, MessageBoxImage.Error);
                Card_Numberbox.Text = ("读取失败！请检查卡片是否放稳!");
            }
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            if(Button_Close.Content.ToString()=="关闭USB口")
             if (reader.CloseHIDDevice()==0x00) /*同理，调用关闭USB口方法，*/
                {
                    MessageBox.Show("关闭USB口成功", "Cjw提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    Button_Open.Content = "打开USB口";
                    Button_Open.IsEnabled = true;
                    Button_Close.IsEnabled = false;
                    Button_Read.IsEnabled = false;
                }
                else
                    MessageBox.Show("关闭USB口失败", "Cjw提示", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetDataObject(Card_Numberbox.Text);
            MessageBox.Show("已成功复制到剪贴板！", "Cjw提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }

}
