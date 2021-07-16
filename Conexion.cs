using System;
//using System.Data;
//using System.Data.SqlClient;

namespace CapaDatos
{
    internal class Conexion
    {
        public static string con = string.Empty;
        public static string getConexionVasos()
        {
            try
            {
                con = "https://www.thecocktaildb.com/api/json/v1/1/filter.php?g=Cocktail_glass";

            }
            catch (Exception m)
            {
                con = m.ToString();
            }
            return con;
        }
        public static string getConexionVasos(string vaso)
        {
            try
            {
                con = "https://www.thecocktaildb.com/api/json/v1/1/filter.php?g="+vaso+"";

            }
            catch (Exception m)
            {
                con = m.ToString();
            }
            return con;
        }
        public static string getConexionCategorias()
        {
            try
            {
                con = "https://www.thecocktaildb.com/api/json/v1/1/filter.php?c=Ordinary_Drink";

            }
            catch (Exception m)
            {
                con = m.ToString();
            }
            return con;
        }

        public static string getConexionCategorias(string categoria)
        {
            try
            {
                con = "https://www.thecocktaildb.com/api/json/v1/1/filter.php?c="+categoria+"";

            }
            catch (Exception m)
            {
                con = m.ToString();
            }
            return con;
        }


        public static string getConexionIngredientes()
        {
            try
            {
                con = "https://www.thecocktaildb.com/api/json/v1/1/filter.php?i=Gin";

            }
            catch (Exception m)
            {
                con = m.ToString();
            }
            return con;
        }

        public static string getConexionIngredientes(string ingredientes)
        {
            try
            {
                con = "https://www.thecocktaildb.com/api/json/v1/1/filter.php?i="+ingredientes+"";

            }
            catch (Exception m)
            {
                con = m.ToString();
            }
            return con;
        }

        public static string getConexionBuscaBebida(int claveBebida)
        {
            try
            {
                con = "https://www.thecocktaildb.com/api/json/v1/1/lookup.php?i=" + claveBebida;

            }
            catch (Exception m)
            {
                con = m.ToString();
            }
            return con;
        }

        public static string getConexionBuscaBebidas(string claveBebida)
        {
            try
            {
                con = "https://www.thecocktaildb.com/api/json/v1/1/search.php?s=" + claveBebida;

            }
            catch (Exception m)
            {
                con = m.ToString();
            }
            return con;
        }

        


    }
}
