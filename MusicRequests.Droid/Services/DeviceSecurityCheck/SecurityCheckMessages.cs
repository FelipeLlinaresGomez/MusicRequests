using System;
namespace MusicRequests.Droid.Services.DeviceSecurityCheck
{
    internal static class SecurityCheckMessages
    {
        // Debug Measures
        public const string AppIsDebuggable = "Flag de depuración activo";
        public const string DebuggerConnected = "Depurador conectado";

        // Emulator Measures
        public const string HasQEmuProps = "Ejecución en entorno de emulación";
        public const string HasQEmuDriver = "Ejecución de un driver de emulación: goldish";
        public const string HasEmulatorBuild = "Ejecución sobre una build de emulación";
        public const string HasQEmuFiles = "Encontrado ficheros de un entorno de emulación";

        // Root measures
        public const string DetectRootManagementApps = "Detectados paquetes de gestión de Root";
        public const string DetectDangerousApps = "Detectadas aplicaciones consideradas no seguras";
        public const string CheckForSuBinary = "Detectados binarios con permisos de super usuario: su";
        public const string CheckDangerousProps = "Detectadas configuraciones no permitidas";
        public const string CheckRWPaths = "Detectadas carpetas de sistema con permisos de escritura";
        public const string DetectTestKeys = "Detectada build distribución con etiqueta no permitida: test-keys";
        public const string CheckSuEnabled = "Detectado proceso 'su' activado";
        public const string DetectRootCloakingApps = "Detectadas aplicaciones para ocultación de configuraciones root";

        // Tamper measures
        public const string CheckValidAppSignature = "Firma digital de la aplicación no coincidente";
    }
}

