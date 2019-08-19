using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.IO;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args) {


            string ModelMainBoard = "";
            string ModelCPU = "";
            string Path = @"\\fileserv.omsu.vmr\Inventory$\InstalledPC\PConAMD.csv";
            string SearchCPU = "intel";




            string GetHardWareInfo(string _wmiClass, string _nameProperty) {
                string Model = "";
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + _wmiClass);
                foreach (ManagementObject share in searcher.Get()) {
                    foreach (PropertyData PC in share.Properties) {
                        if (PC.Name == _nameProperty) {
                            Model = PC.Value.ToString();
                            return Model;
                        }

                    }
                }
                return Model;
            }

            ModelMainBoard = GetHardWareInfo("Win32_baseboard", "Product");
            ModelCPU = GetHardWareInfo("Win32_processor", "Name");

            Console.WriteLine(ModelCPU.ToUpper());
            Console.ReadKey();


            if (ModelCPU.ToUpper().Contains(SearchCPU.ToUpper())) {
                if (!File.Exists(Path))
                    using (StreamWriter writer = new StreamWriter(Path)) {
                        writer.WriteLine("NamePC::MainBoard::CPU");
                    }

                Random random = new Random();
                bool flag = true;

                while (flag) {
                    try {
                        StreamWriter writer = new StreamWriter(Path, true);
                        writer.WriteLine("{0}::{1}::{2}", Environment.MachineName, ModelMainBoard, ModelCPU);
                        writer.Close();
                        flag = false;

                    }
                    catch (System.IO.IOException) {
                        System.Threading.Thread.Sleep(random.Next(1000, 5000));
                    }

                }

                Environment.Exit(0);
            }
        }
    }
}