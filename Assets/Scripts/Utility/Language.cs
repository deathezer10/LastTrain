
namespace Trident
{
    // languageEnum
    public enum Language
    {
        En,
        Ja
    }

    // Ex
    static class LanguageEx
    {
        //高速でstringに変換する拡張メソッド
        public static string ToStringQuickly(this Language language)
        {
            //各enumごとにstringを返す
            switch (language)
            {
                case Language.En: return "english";
                case Language.Ja: return "japan";
                default: return language.ToString();
            }
        }
    }
}
