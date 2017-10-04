using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rijindael
{
    class Program
    {
        static void Main(string[] args)
        {
            var text =  Guid.NewGuid().ToString();
            var password = "4801CB755CB29BEABB7BFF0C1161A52D";
            var ivKey = "0C790D11A2C619DB07C19F526CC505F5";
            var obj = new CryptoRijndael();
            var encrypt = obj.Encrypt(text, password, ivKey);
            var decrypt = obj.Decrypt(encrypt, password, ivKey);
            Console.WriteLine("\nPlain Text is: " + decrypt);
            Console.ReadLine();

            
            //Guid g;
            //g = Guid.NewGuid();
            //var id = g.ToString();
            //var id2 = ModHex.Encode(g.ToByteArray());
            //Console.Write(id2);
        }
    }
}
