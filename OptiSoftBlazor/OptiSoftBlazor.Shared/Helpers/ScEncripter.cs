using System;
using System.Collections.Generic;
using System.Text;

namespace OptiSoftBlazor.Shared.Helpers
{
    public static class ScEncripter
    {
        private static string? _cadenaEncriptada;

        private static string Encriptar(string cadenaConexion)
        {
            if (string.IsNullOrWhiteSpace(cadenaConexion))
                throw new ArgumentException("La cadena de conexión es inválida.");

            var partes = cadenaConexion.Split(';', StringSplitOptions.RemoveEmptyEntries);

            string server = partes.First(p => p.StartsWith("Server=", StringComparison.OrdinalIgnoreCase))
                                   .Replace("Server=", "", StringComparison.OrdinalIgnoreCase);

            string database = partes.First(p => p.StartsWith("Database=", StringComparison.OrdinalIgnoreCase))
                                     .Replace("Database=", "", StringComparison.OrdinalIgnoreCase);

            string textoPlano = $"{server}_*_{database}";

            byte[] bytes = Encoding.UTF8.GetBytes(textoPlano);
            string base64_1 = Convert.ToBase64String(bytes);

            bytes = Encoding.UTF8.GetBytes(base64_1);
            string base64_2 = Convert.ToBase64String(bytes);

            return base64_2;
        }

        private static string Desencriptar(string encriptado)
        {
            if (string.IsNullOrWhiteSpace(encriptado))
                throw new ArgumentException("El texto encriptado es inválido.");

            byte[] bytes = Convert.FromBase64String(encriptado);
            string texto = Encoding.UTF8.GetString(bytes);

            bytes = Convert.FromBase64String(texto);
            texto = Encoding.UTF8.GetString(bytes);

            string[] arrayParametros = texto.Split("_*_");

            if (arrayParametros.Length < 2)
                throw new InvalidOperationException("Formato de cadena inválido.");

            return
                $"Server={arrayParametros[0]};" +
                $"Database={arrayParametros[1]};" +
                $"User ID=infoelect;" +
                $"Password=vcsnfaM$1;" +
                $"MultipleActiveResultSets=True;" +
                $"TrustServerCertificate=True;" +
                $"Connection Timeout=300000;";
        }

        public static void Initialize(string cadenaConexionReal)
        {
            if (string.IsNullOrWhiteSpace(cadenaConexionReal))
                throw new ArgumentException("Cadena de conexión inválida.");

            _cadenaEncriptada = Encriptar(cadenaConexionReal);
        }

        public static string GetConnectionString()
        {
            if (string.IsNullOrWhiteSpace(_cadenaEncriptada))
                throw new InvalidOperationException("La cadena no ha sido inicializada.");

            return _cadenaEncriptada;
        }

        public static bool IsInitialized =>
            !string.IsNullOrWhiteSpace(_cadenaEncriptada);
    }
}
