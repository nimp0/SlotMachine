using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {


        static void Main(string[] args)
        {
            String path = @"C:\TestTream.txt";


            using (StreamWriter sr = File.AppendText(path))
            {
                sr.WriteLine("I love slot machines");
                sr.Close();


                Console.WriteLine(File.ReadAllText(path));
            }


            //Console.ReadLine();
            Console.ReadKey();

            FileStream stream1 = File.Open(@"C:\TestTream.txt", FileMode.Open);
            Print(stream1);

            MemoryStream stream2 = new MemoryStream(new byte[1234]);
            Print(stream2);
        }



        static void Print(Stream stream)
        {
            Console.WriteLine(stream.Length);
            Console.WriteLine(stream.Position);
        }
    }
}
