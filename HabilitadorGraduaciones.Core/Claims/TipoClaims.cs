
namespace HabilitadorGraduaciones.Core.Claims
{
    public static class TipoClaims
    {

        public static string Matricula()
        {
            const string claim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/NAM_cn";
            return claim;
        }

        public static string NombreCompleto()
        {
            const string claim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/fullName";
            return claim;
        }

        public static string Correo()
        {
            const string claim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/NAM_upn";
            return claim;
        }

        public static string Pidm()
        {
            const string claim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/ITESMProfPIDM";
            return claim;
        }

        public static string Nomina()
        {
            const string claim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/NAM_SAMAccountName";
            return claim;
        }

    }
}
