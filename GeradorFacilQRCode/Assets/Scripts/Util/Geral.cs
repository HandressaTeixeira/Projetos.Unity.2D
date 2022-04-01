using System.Text;
using UnityEngine;
using System.Globalization;
using System.Linq;

namespace Assets.Scripts.Util
{
    public static class Geral
    {
        #region Paths
        public static string PathUploadQRCode()
        {
            return Application.persistentDataPath + "/GerarQRCodeFacil/";
        }
        #endregion

        public static string SemAcento(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in from c in normalizedString let unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c) where unicodeCategory != UnicodeCategory.NonSpacingMark select c)
            {
                stringBuilder.Append(c);
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
