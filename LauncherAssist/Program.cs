// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

Process p = new Process();
//设置要启动的应用程序
p.StartInfo.FileName = $"{AppDomain.CurrentDomain.BaseDirectory}GamePluginLauncher.exe";//是否使用操作系统shell启动
if(args.Length > 0) 
   p.StartInfo.Arguments = args[0];
p.StartInfo.UseShellExecute = false;
// 接受来自调用程序的输入信息
p.StartInfo.RedirectStandardInput = true;
//输出信息
p.StartInfo.RedirectStandardOutput = true;
// 输出错误
p.StartInfo.RedirectStandardError = true;
//不显示程序窗口
p.StartInfo.CreateNoWindow = true;

//启动程序
p.Start();


