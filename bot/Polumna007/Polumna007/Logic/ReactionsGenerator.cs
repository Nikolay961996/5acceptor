using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polumna007.Logic
{
    internal class ReactionsGenerator
    {
        public string GetAsciiEmoji(float rating)
        {
            string[] coolEmoji = ["😎", "🌻", "😉", "😊", "⚡", "🤘", "🍀", "💪", "⎛⎝ ≽  >  ⩊   < ≼ ⎠⎞", "☺️"];
            string[] soSoEmoji = ["🤔", "🙃", "🙄", "😦", "👀", "🙉", "😑", "😩", "🤨", "😬"];
            string[] veryBadEmoji = ["🙈", "😭", "😠", "😢", "😕", "😖", "😔", "❌", "(｡•́︿•̀｡)(╥﹏╥)", "😒"];

            if (rating <= 0.5)
            {
                return veryBadEmoji[new Random().Next(0, veryBadEmoji.Length)];
            }
            if (rating >= 0.9)
            {
                return coolEmoji[new Random().Next(0, coolEmoji.Length)];
            }
            else
            { 
                return soSoEmoji[new Random().Next(0, soSoEmoji.Length)];
            }
            
        }

        
    }
}
