using CapaDatos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Cockteles
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataSet dataSet = new DataSet();
        DataTable dataTable = new DataTable();
        DataTable dt = new DataTable();
        DataRow _ravi = null;

        int clave = 0;

        public MainWindow()
        {
            InitializeComponent();

            List<string> Bebidas = new List<string>();
            Bebidas.Add("Select...");
            Bebidas.Add("Categories");
            Bebidas.Add("Glasses");
            Bebidas.Add("Ingredients");


            cbBebidas.ItemsSource = Bebidas;
            cbBebidas.SelectedIndex = 0;

        }


        private void ComboBox_SelectionChanged_Bebidas(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                switch (cbBebidas.SelectedIndex)
                {
                    case 1:
                        /**Caegotrias**/
                        var json = new WebClient().DownloadString(Conexion.getConexionCategorias());
                        dataSet = JsonConvert.DeserializeObject<DataSet>(json);
                       
                        if (dataSet != null)
                        {
                            dataTable = dataSet.Tables["drinks"];

                            dt = new DataTable();
                            dt.Clear();
                            dt.Columns.Add("ID");
                            dt.Columns.Add("Drink");
                            //dt.Columns.Add("Nombre");

                            _ravi = dt.NewRow();
                            foreach (DataRow row in dataTable.Rows)
                            {
                                _ravi = dt.NewRow();
                                _ravi["ID"] = row["idDrink"];
                                _ravi["Drink"] = row["strDrink"];
                                //  _ravi["Drink"] = row["strDrinkThumb"];
                                dt.Rows.Add(_ravi);
                            }

                         //   dt.Columns[0].ColumnMapping = MappingType.Hidden;

                            dG_Bebidas.ItemsSource = dt.DefaultView;

                        }

                        break;
                    case 2:
                        /**Vaso**/
                        json = new WebClient().DownloadString(Conexion.getConexionVasos());

                        dataSet = JsonConvert.DeserializeObject<DataSet>(json);
                        


                        if (dataSet != null)
                        {
                            dataTable = dataSet.Tables["drinks"];
                            dt.Clear();
                           

                            _ravi = dt.NewRow();
                            foreach (DataRow row in dataTable.Rows)
                            {
                                _ravi = dt.NewRow();
                                _ravi["ID"] = row["idDrink"];
                                _ravi["Drink"] = row["strDrink"];
                                // _ravi["Drink"] = row["strDrinkThumb"];
                                dt.Rows.Add(_ravi);
                            }

                            dG_Bebidas.ItemsSource = dt.DefaultView;
                        }

                        break;
                    case 3:
                        /**Ingredientes**/
                        json = new WebClient().DownloadString(Conexion.getConexionIngredientes());

                        dataSet = JsonConvert.DeserializeObject<DataSet>(json);
                       

                        if (dataSet != null)
                        {
                            dataTable = dataSet.Tables["drinks"];
                            dt.Clear();
                           

                            _ravi = dt.NewRow();
                            foreach (DataRow row in dataTable.Rows)
                            {
                                _ravi = dt.NewRow();
                                _ravi["ID"] = row["idDrink"];
                                _ravi["Drink"] = row["strDrink"];
                                // _ravi["Drink"] = row["strDrinkThumb"];
                                dt.Rows.Add(_ravi);
                            }

                            
                            dG_Bebidas.ItemsSource = dt.DefaultView;
                        }

                       
                        break;
                }

            }
            catch (Exception ex)
            {
                mError(ex.ToString());

            }

        }






        private void dG_Cocktel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            try
            {
                DataRowView DRV = dG_Bebidas.CurrentCell.Item as DataRowView;
                if (DRV != null)
                {
                    clave = Convert.ToInt32(DRV.Row[0].ToString());



                    //www.thecocktaildb.com/api/json/v1/1/lookup.php?i=
                    var json = new WebClient().DownloadString(Conexion.getConexionBuscaBebida(clave));

                    dataSet = JsonConvert.DeserializeObject<DataSet>(json);

                    dataTable = dataSet.Tables["drinks"];

                    DataRow dr = dataTable.Rows[0];

                    //mOK("ID: " + clave + ", Table : " + dataTable.TableName + ", Rows: " + dr["strDrink"] );



                    string bebida_info = "Drink     :      " + dr["strDrink"] + "\n"
                                    + "Category     :      " + dr["strCategory"] + "\n"
                                    + "Alcoholic    :      " + dr["strAlcoholic"] + "\n"
                                    + "Type of Glass:      " + dr["strGlass"] + "\n\n"

                                    + "Ingredients  :  \n";


                    for (int i = 1; i <= 15; i++)
                    {
                        if (dr["strIngredient" + i + ""].ToString() != "")
                        {
                            bebida_info += (dr["strIngredient" + i + ""] != null ? dr["strIngredient" + i + ""] : "") + " : " + (dr["strMeasure" + i + ""] != null ? dr["strMeasure" + i + ""] : "") + "\n";
                        }

                    }

                    bebida_info += "\n\n\npreparation   :     " + Wrap(dr["strInstructions"].ToString(),60)+ "\n";

                    lb_IndicadorBebidas.Content = bebida_info;

                    var image = new BitmapImage();
                    int BytesToRead = 100;

                    WebRequest request = WebRequest.Create(new Uri(dr["strDrinkThumb"].ToString(), UriKind.Absolute));
                    request.Timeout = -1;
                    WebResponse response = request.GetResponse();
                    Stream responseStream = response.GetResponseStream();
                    BinaryReader reader = new BinaryReader(responseStream);
                    MemoryStream memoryStream = new MemoryStream();

                    byte[] bytebuffer = new byte[BytesToRead];
                    int bytesRead = reader.Read(bytebuffer, 0, BytesToRead);

                    while (bytesRead > 0)
                    {
                        memoryStream.Write(bytebuffer, 0, bytesRead);
                        bytesRead = reader.Read(bytebuffer, 0, BytesToRead);
                    }

                    image.BeginInit();
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    image.StreamSource = memoryStream;
                    image.EndInit();

                    imgDynamic.Source = image;



                }
            }
            catch (Exception ex)
            {
                mError(ex.ToString());
            }


        }
        private void tB_BuscarC_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                DataTable aux = new DataTable();


                /**Caegotrias**/
                switch (cbBebidas.SelectedIndex)
                {
                    case 1:
                        var json = new WebClient().DownloadString(Conexion.getConexionCategorias(tB_BuscarC.Text));
                        /**devolver dt con filtro incluido*/
                        dataSet = JsonConvert.DeserializeObject<DataSet>(json);

                        if (dataSet != null)
                        { dataTable = dataSet.Tables["drinks"]; }

                           

                        break;
                    case 2:
                        json = new WebClient().DownloadString(Conexion.getConexionVasos(tB_BuscarC.Text));
                        /**devolver dt con filtro incluido*/
                        dataSet = JsonConvert.DeserializeObject<DataSet>(json);
                        if (dataSet != null)
                        { dataTable = dataSet.Tables["drinks"]; }

                        
                        break;
                    case 3:
                        json = new WebClient().DownloadString(Conexion.getConexionIngredientes(tB_BuscarC.Text));
                        /**devolver dt con filtro incluido*/
                        dataSet = JsonConvert.DeserializeObject<DataSet>(json);
                        if (dataSet != null)
                        { dataTable = dataSet.Tables["drinks"]; }

                        
                        break;

                    default:
                        json = new WebClient().DownloadString(Conexion.getConexionBuscaBebidas(tB_BuscarC.Text));
                        /**devolver dt con filtro incluido*/
                        dataSet = JsonConvert.DeserializeObject<DataSet>(json);
                        if (dataSet != null)
                        { dataTable = dataSet.Tables["drinks"]; }
                        break;

                }
               

                dt = new DataTable();
                dt.Clear();
                dt.Columns.Add("ID");
                dt.Columns.Add("Drink");
                //dt.Columns.Add("Nombre");

                _ravi = dt.NewRow();
                foreach (DataRow row in dataTable.Rows)
                {
                    _ravi = dt.NewRow();
                    _ravi["ID"] = row["idDrink"];
                    _ravi["Drink"] = row["strDrink"];
                    //  _ravi["Drink"] = row["strDrinkThumb"];
                    dt.Rows.Add(_ravi);
                }

                //   dt.Columns[0].ColumnMapping = MappingType.Hidden;

                dG_Bebidas.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                mError(ex.ToString());

            }
        }



        public void mError(string rpta)
        {
            MessageBox.Show(rpta, "Error Fatal", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void mOK(string rpta)
        {
            MessageBox.Show(rpta, "O.K.", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var json = new WebClient().DownloadString(Conexion.getConexionCategorias());

            dataSet = JsonConvert.DeserializeObject<DataSet>(json);

            if (dataSet != null)
            {
                dataTable = dataSet.Tables["drinks"];

                dt = new DataTable();
                dt.Clear();
                dt.Columns.Add("ID");
                dt.Columns.Add("Drink");
                //dt.Columns.Add("Nombre");

                _ravi = dt.NewRow();
                foreach (DataRow row in dataTable.Rows)
                {
                    _ravi = dt.NewRow();
                    _ravi["ID"] = row["idDrink"];
                    _ravi["Drink"] = row["strDrink"];
                    //  _ravi["Drink"] = row["strDrinkThumb"];
                    dt.Rows.Add(_ravi);
                }

                //   dt.Columns[0].ColumnMapping = MappingType.Hidden;

                dG_Bebidas.ItemsSource = dt.DefaultView;

            }
        }

        public static string Wrap(string singleLineString, int columns)
        {
            if (singleLineString == null)
                throw new ArgumentNullException("singleLineString");
            if (columns < 1)
                throw new ArgumentException("'columns' must be greater than 0.");

            var rows = Math.Ceiling((double)singleLineString.Length / columns);
            if (rows < 2) return singleLineString;

            var sb = new StringBuilder();

            for (int i = 0; i < rows; i++)
            {
                if (i > 0) sb.Append(Environment.NewLine);
                var index = i * columns;
                var length = Math.Min(columns, singleLineString.Length - index);
                var line = singleLineString.Substring(index, length);
                sb.Append(line);
            }

            return sb.ToString();
        }

    }
}
