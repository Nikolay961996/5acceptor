using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Polumna007.Logic
{
    internal class FileAnalyst
    {
        public int FileSize(string filePath)
        {
            int size = 0;
            using (StreamReader stroka = new )
    }

        public int FileLength(string filePath)
        {

            int length = 0;
            using (StreamReader str = new StreamReader(filePath))
            {
                while (str.ReadLine() != null)
                {
                    length++;
                }


            }
            return length;
        }
    }

   


}