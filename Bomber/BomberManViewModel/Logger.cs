using System;
using System.IO;

namespace BomberManViewModel
{
    /// <summary>
    /// Klasa odpowiedzialna za logowanie błędów związanych z integracją z bazą danych po stronie modelu.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Zapisz błedy do pliku tekstowego zanjdującego się w głownym projektcie o nazwie log.txt.
        /// </summary>
        /// <param name="className">azwa klasy, w które wywołano metodę powodującą błąd</param>
        /// <param name="methodeName">nazwa metody, w którey pojawił się wyjątek</param>
        /// <param name="errorMessage">stack trace wyjątku</param>
        public static void LogMessage(String className, String methodeName,
            String errorMessage)
        {
            using (StreamWriter w = File.AppendText(@"../../../log.txt"))
            {
                w.WriteLine("CLASS : " + className + " METHODE : " + methodeName + " ERROR : " + errorMessage);
                w.Close();
            }
        }
    }
}
