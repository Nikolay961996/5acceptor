using System.Net.Mail;

namespace Polumna007.Logic;

public class EmailValidator
{
    public static bool IsValid(string email)
    {
        try
        {
            _ = new MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
