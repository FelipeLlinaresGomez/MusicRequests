using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MusicRequests.Core.Helpers
{
    public static class ParticularesHelper
    {

        /// <summary>
        /// Alta y modificación de clientes: (nombre, apellidos y nombre de visualización).
        /// </summary>
        /// <param name="cadena"></param>
        /// <returns></returns>
        public static string NormalizeBizumAltaModificacion(string cadena)
        {
            return NormalizeRegex(cadena, "[A-Za-záéíóúÁÉÍÓÚàèìòùÀÈÌÒÙäÄëËïÏöÖüÜçÇ\\-\'\\.#$ñÑ\\s\\d]");
        }

        /// <summary>
        /// Procesamiento de operaciones: (concepto, nombre y apellidos de beneficiario y ordenante, dirección de comercio). 
        /// </summary>
        /// <param name="cadena"></param>
        /// <returns></returns>
        public static string NormalizeBizumOperaciones(string cadena)
        {
            return NormalizeRegex(cadena, "[a-zA-ZñÑáÁéÉíÍó ÓúÚ0-9-:()._,'+ ]");
        }

        private static string NormalizeRegex(string cadena, string regexStr)
        {
            Regex regex = new Regex(regexStr);
            var matches = regex.Matches(cadena);
            string normalized = "";
            foreach (var m in matches)
            {
                normalized += m;
            }
            return normalized;
        }

    }
}
