using HabilitadorGraduaciones.Data.Utils.Enums;

namespace HabilitadorGraduaciones.Data.Utils.Extensions
{
    public static class CarrerasEnumExtension
    {
        public static string GetString(this Carreras carrera)
        {
            switch (carrera)
            {
                case Carreras.Medicina:
                    return "MC";
                case Carreras.Odontologia:
                    return "MO";
                case Carreras.LicNutricion:
                    return "LNB";
                case Carreras.LicPsicologiaClinica:
                    return "LPS";
                default:
                    return "default";
            }
        }
    }
}
