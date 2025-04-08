using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Assets.YG.Scripts
{
    public class Languages : YandexBase
    {
        protected override string sourceMessage => "Languages";
        private static Dictionary<string, string> langs;
      

        public Languages(YandexGame yandex, bool isMessage = true) : base(yandex, isMessage){ }

        [DllImport("__Internal")] private static extern string GetLanguage();

        public string GetLangs()=> GetLanguage();

        public static string GetContent(string keys)
        {
            if (langs != null)
            {
                if (langs.TryGetValue(keys, out var result))
                    return result;
            }

            return keys;
        }
         public void CreateLangs()
         {
            langs = new()
            {  
               { "Daily Rewards","Дни наград" },
               { "Claim reward","Получить вознаграждение" },
               { "Close","Закрыть" },
               { "Claim","Взять" },
               { "Reward","Награда" },
               { "Ok","Хорошо" },
               { "Coin","Монета" },
               { "Daily ","День" },

               { "Settings","Настройки" },
               { "Music","Музыка" },
               { "Effects","Эффекты" },
               { "Tutorials","Руководство" },
               { "Menu","Меню" },
               { "Back","Назад" },
               { "Rewards","Награды" },


               { "Shop","Лавка" },
               { "watch the video x100 coins","просмотр видео х100 монет" },
               { "Buy","Куп" },
               { "SmallBomb","Бомба" },
               { "HorizontalBonus","Горизонт ракета" },
               { "VerticalBonus","Вертикал ракета" },
               { "ColorBonus","Цветной бонус" },

               { "No moves","Нет ходов" },
               { "Watch","Просмотр" },
               { "Continue by buying 5 moves or watching a video","Продолжайте, купив 5 ходов или посмотрев видео" },

               { "Level ","Уровень " },
               { "Next","Дальше" },
               { "Victory!!!","Победа!!!" },
               { "Fail","Неудача" },

               { "Goals","Цели" },
               { "Moves","Ходы" },

               { "Location ","Локация " },

               { "Levels ","Уровени " },

               { "Level Goals","Цели уровня" },

               { "3 moves","3 хода" },
               { "Score: ","Счет: " },

            };

         }
    }
}