namespace Polumna007.Logic;

public class FileAnalyst
{
    public int FileSize(string filePath)
    {
        int size = 0;
        using (StreamReader stroka = new StreamReader(filePath))
        {
            while (stroka.Read() != 0)
            {
                size ++;
            }
            stroka.Close();
        }
        return size;
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

            str.Close();
        }
        return length;
    }
}

