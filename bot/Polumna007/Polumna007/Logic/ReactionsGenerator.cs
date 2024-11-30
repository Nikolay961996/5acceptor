using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polumna007.Logic
{
    internal class ReactionsGenerator
    {
        private string[] _coolEmoji = ["😎", "🌻", "😉", "😊", "⚡", "🤘", "🍀", "💪", "⎛⎝ ≽  >  ⩊   < ≼ ⎠⎞", "☺️"];
        private string[] _soSoEmoji = ["🤔", "🙃", "🙄", "😦", "👀", "🙉", "😑", "😩", "🤨", "😬"];
        private string[] _veryBadEmoji = ["🙈", "😭", "😠", "😢", "😕", "😖", "😔", "❌", "(｡•́︿•̀｡)(╥﹏╥)", "😒"];

        public string GetAsciiEmoji(float rating)
        {
            switch(rating)
            {
                case <= 0.5f:
                    return _veryBadEmoji[Random.Shared.Next(0, _veryBadEmoji.Length)];
                case >= 0.9f:
                    return _coolEmoji[Random.Shared.Next(0, _coolEmoji.Length)];
                default:
                    return _soSoEmoji[Random.Shared.Next(0, _soSoEmoji.Length)];
            }


            if (rating <= 0.5)
            {
                return _veryBadEmoji[Random.Shared.Next(0, _veryBadEmoji.Length)];
            }
            if (rating >= 0.9)
            {
                return _coolEmoji[Random.Shared.Next(0, _coolEmoji.Length)];
            }
           
            return _soSoEmoji[Random.Shared.Next(0, _soSoEmoji.Length)];
        }

        
    }
}
