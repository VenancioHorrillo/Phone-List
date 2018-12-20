using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections;

/// <summary>
/// Espacio Lista Telefónica
/// </summary>
namespace Phone_List
{
    /// <summary>
    ///  Clase principal
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// Variables estáticas encargadas de controlar el tamaño del texto anteriormente introducido en los textBox,
        /// una por cada textBox, son utilizadas para optimizar las busquedas.
        /// </summary>
        public static int lengthName;
        public static int lengthSurname;
        public static int lengthCity;

        ///<summary>
        /// Objeto Fichero, almacena el nombre y contactos del archivo.
        ///</summary>
        public static class File
        {
            // Nombre del archivo
            public static string fileName;
            // Array con todos los contactos 
            public static ArrayList arrContacts = new ArrayList();
        }

        /// <summary>
        ///  Objeto contacto, almacena nombre, apellido, ciudad y teléfono de un contacto,
        ///  utilizado en los arrays.
        /// </summary>
        public class Contact {
            public string name;
            public string surname;
            public string city;
            public string phone;
        }

        /// <summary>
        /// Constructor, inicializa los componentes, lee el archivo y carga los contactos en el array.
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            // Ruta del archivo, modificar con la nueva ruta del fichero seleccionado.
            File.fileName = "C:\\Users\\rt00866\\Downloads\\list.csv";

            StreamReader objReader = new StreamReader(File.fileName);
            string sLine = "";

            // bucle encargado de recorrer el archivo y cargar los contactos en el array.
            while (sLine != null)
            {
                sLine = objReader.ReadLine();
                Contact contact = new Contact();
                if (sLine != null)
                {
                    string[] parts = sLine.Split('|');
                    contact.name = parts[0];
                    contact.city = parts[1];
                    contact.phone = parts[2];
                    parts = contact.name.Split(' ');
                    int i = 0;
                    contact.name = "";
                    while (i < parts.Count() - 1)
                    {
                        if (i != 0) contact.name = contact.name + " ";
                        contact.name = contact.name + parts[i];
                        i++;
                    }
                    contact.surname = parts[i];
                    File.arrContacts.Add(contact);
                }
            }
            objReader.Close();

            // Funcion que inicializa el primer listView
            ToList();

            // Limpia las variables en caso de contener algo para iniciar el programa.
            clearBox(1);
            clearBox(2);
        }

        //No utilizada
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //No utilizada
        private void label1_Click(object sender, EventArgs e)
        {

        }

        //No utilizada
        private void label2_Click(object sender, EventArgs e)
        {

        }

        //No utilizada
        private void label3_Click(object sender, EventArgs e)
        {

        }

        //No utilizada
        private void label4_Click(object sender, EventArgs e)
        {

        }

        //No utilizada
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //No utilizada
        private void label5_Click(object sender, EventArgs e)
        {

        }

        //No utilizada
        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Funcion que inicializa los dos listView, el listView1 mantiene la lista con todos los contactos para
        /// optimizar cargas, sobre el listView dos es sobre el que se trabaja y se modifica.
        /// </summary>
        private void ToList()
        {
            listView1.View = View.Details;
            listView2.View = View.Details;
            
            listView1.AllowColumnReorder = true;
            listView2.AllowColumnReorder = true;

            listView1.FullRowSelect = true;
            listView2.FullRowSelect = true;
            
            listView1.GridLines = true;
            listView2.GridLines = true;
            
            listView1.Sorting = SortOrder.Ascending;
            listView2.Sorting = SortOrder.Ascending;

            // Bucle que carga los contactos en el listView1
            int i = 0;
            while (i < File.arrContacts.Count)
            {
                Contact contact = (Contact)File.arrContacts[i];
                ListViewItem item1 = new ListViewItem(contact.name, 0);
                item1.Checked = true;
                item1.SubItems.Add(contact.surname);
                item1.SubItems.Add(contact.city);
                item1.SubItems.Add(contact.phone);
                i++;

                listView1.Items.AddRange(new ListViewItem[] { item1 });
            }

            // Nombre de las columnas de abos listView
            listView1.Columns.Add("Nombre", -2, HorizontalAlignment.Left);
            listView1.Columns.Add("Apellido", -2, HorizontalAlignment.Left);
            listView1.Columns.Add("Ciudad", -2, HorizontalAlignment.Center);
            listView1.Columns.Add("Telefono", -2, HorizontalAlignment.Left);
            listView2.Columns.Add("Nombre", -2, HorizontalAlignment.Left);
            listView2.Columns.Add("Apellido", -2, HorizontalAlignment.Left);
            listView2.Columns.Add("Ciudad", -2, HorizontalAlignment.Center);
            listView2.Columns.Add("Telefono", -2, HorizontalAlignment.Left);

            // Añadimos listView1 al control y ocultamos listView2
            this.Controls.Add(listView1);
            listView2.Visible = false;
        }

        ///<summary>
        /// Clase que compara dos objetos, utilizada para ordenar las columnas antes de las busquedas dicotómicas o binarias.
        ///</summary>
        class ListViewItemComparer : IComparer
        {
            /// <summary>
            /// Variables para indicar la coumna y el orden, en nuestro caso ascendente.
            /// </summary>
            public SortOrder Order = SortOrder.Ascending;
            public int Column;

            /// <summary>
            /// Constructor que inicializa la clase a la primera columna.
            /// </summary>
            public ListViewItemComparer()
            {
                Column = 0;
            }
            /// <summary>
            /// Constructor para inicializar por otras columnas
            /// </summary>
            /// <param name="column">
            /// Parámetro de entrada donde indicamos la columna a ordenar
            /// </param>
            public ListViewItemComparer(int column)
            {
                Column = column;
            }
            /// <summary>
            /// Función que compara los dos objetos
            /// </summary>
            /// <param name="x">
            /// primer objeto
            /// </param>
            /// <param name="y">
            /// segundo objeto
            /// </param>
            /// <returns>
            /// Devuelve 1 si el primer objeto va despues,
            /// -1 si el primer objeto va antes y 0 si los objetos son iguales
            /// </returns>
            public int Compare(object x, object y)
            {
                int returnVal = String.Compare(((ListViewItem)x).SubItems[Column].Text,
                ((ListViewItem)y).SubItems[Column].Text);

                if (Order == SortOrder.Descending)
                    return -returnVal;
                else
                    return returnVal;
            }
        }

        //No utilizada
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        //No utilizada
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        //No utilizada
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        //No utilizada
        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        ///<summary>
        /// Función utilizada para limpiar los textBox no implicados en la busqueda.
        ///</summary>
        /// <param name="clear">
        /// El parametro clear indica que tipo de cajas limpiar,
        /// 2 para las superiores, 1 para la inferior.
        /// </param>
        private void clearBox(int clear)
        {
            switch (clear)
            {
                case 1:
                    textBox4.Clear();
                    listView2.Items.Clear();
                    break;

                case 2:
                    textBox1.Clear();
                    lengthName = 0;
                    textBox2.Clear();
                    lengthSurname = 0;
                    textBox3.Clear();
                    lengthCity = 0;
                    listView2.Items.Clear();
                    break;
            }

        }

        ///<summary>
        /// Función que inicializa la busqueda de nombre, apellido y/o ciudad al pulsar el botón superior
        ///</summary>
        /// <param name="sender">
        /// No utilizado
        /// </param>
        /// <param name="e">
        /// No utilizado
        /// </param>
        private void button1_Click(object sender, EventArgs e)
        {
            // Variables que recogen los textos escritos en los textBox superiores
            string textName = textBox3.Text;
            string textSurname = textBox2.Text;
            string textCity = textBox1.Text;

            // Si el textBox inferior tiene texto lo limpia
            if(textBox4.Text.Count() != 0) clearBox(1);

            // Ruta para mostrar la lista completa cuando ningun texBox tiene contenido
            if (textName.Count() == 0 && textSurname.Count() == 0 && textCity.Count() == 0)
            {
                listView1.Visible = true;
                listView2.Visible = false;
                listView2.Items.Clear();
            }
            // Ruta para inicializar, modificar y mostrar el segundo listView
            else
            {
                //Variables que controlan si se borran caracteres para ampliar la lista
                lengthName = textName.Count();
                lengthSurname = textSurname.Count();
                lengthCity = textCity.Count();

                // Cambia el listView mostrado
                listView2.Visible = true;
                listView1.Visible = false;
                listView2.Items.Clear();

                // Rutas para controlar los texBox que modifican la seleccion y realizar las busquedas
                if (lengthName > 0) rangeSearch(textName, listView1, 0);
                if (lengthSurname > 0 && lengthName <= 0) rangeSearch(textSurname, listView1, 1);
                else if (lengthSurname > 0 && lengthName > 0) rangeSearch(textSurname, listView2, 1);
                if (lengthCity > 0 && lengthName <= 0 && lengthSurname <= 0) rangeSearch(textCity, listView1, 2);
                else if (lengthCity > 0 && (lengthName > 0 || lengthSurname > 0)) rangeSearch(textCity, listView2, 2);

                listView2.ListViewItemSorter = new ListViewItemComparer(0);
            }
        }

        /// <summary>
        /// Función que realiza la busqueda dicotómica o binaria
        /// </summary>
        /// <param name="element">
        /// Contine el texto a buscar
        /// </param>
        /// <param name="listViewSearch">
        /// Contiene la lista donde se realizará la busqueda
        /// </param>
        /// <param name="columns">
        /// Contiene la columna por la que buscar
        /// </param>
        /// <returns>
        /// Devuelve la posición donde se encuentra el elemento que cumple con los requisitos
        /// </returns>
        public int binarySearch(String element, ListView listViewSearch, int columns)
        {
            listViewSearch.ListViewItemSorter = new ListViewItemComparer(columns);
            int size = listViewSearch.Items.Count;
            int middle, botton, top;
            botton = 0;
            top = size - 1;

            // Bucle que realiza la busqueda dicotómica o binaria dividiendo en dos el segmento.
            while (botton <= top)
            {
                middle = ((top + botton) / 2);
                if (element.Count()<= listViewSearch.Items[middle].SubItems[columns].Text.Count() && listViewSearch.Items[middle].SubItems[columns].Text.Substring(0, element.Count()).ToLowerInvariant().Contains(element.ToLowerInvariant()))
                {
                    return middle;
                }
                else if (listViewSearch.Items[middle].SubItems[columns].Text.ToLowerInvariant().CompareTo(element.ToLowerInvariant()) > 0)
                {
                    top = middle - 1;
                }
                else
                {
                    botton = middle + 1;
                }
            }
            return -1;
        }

        /// <summary>
        /// Función que una vez dada la posición de un elemento que cumple los requisitos busca el resto de elementos
        /// que los cumplen por orden.
        /// </summary>
        /// <param name="textSearch">
        /// Texto a buscar
        /// </param>
        /// <param name="listViewSearch">
        /// Lista donde realizar la busqueda
        /// </param>
        /// <param name="column">
        /// Columna por la que buscar
        /// </param>
        public void rangeSearch(string textSearch, ListView listViewSearch, int column)
        {
            listViewSearch.ListViewItemSorter = new ListViewItemComparer(column);
            // Llamada a la función que busca el elemento que cumple los requisitos
            int pos = binarySearch(textSearch, listViewSearch, column);

            if (pos > -1 && listView2.Items.Count == 0)
            {
                ListViewItem item = listViewSearch.Items[pos];
                listView2.Items.Add((ListViewItem)item.Clone());
                int i = pos - 1;

                // Bucle que busca los elementos que cumplen con los requisitos anteriores al elemento encontrado
                while (i > -1 && listViewSearch.Items[i].SubItems[column].Text.Substring(0,textSearch.Count()).ToLowerInvariant().Contains(textSearch.ToLowerInvariant()))
                {
                    item = listViewSearch.Items[i];
                    listView2.Items.Add((ListViewItem)item.Clone());
                    i--;
                }
                i = pos + 1;

                // Bucle que busca los elementos que cumplen con los requisitos posteriores al elemento encontrado
                while (i < listViewSearch.Items.Count && listView1.Items[i].SubItems[column].Text.Substring(0, textSearch.Count()).ToLowerInvariant().Contains(textSearch.ToLowerInvariant()))
                {
                    item = listViewSearch.Items[i];
                    listView2.Items.Add((ListViewItem)item.Clone());
                    i++;
                }
                i = 0;
            }

            // Ruta para eliminar elementos que no cumplen con todos los requisitos
            else if (listView2.Items.Count > 0)
            {
                ListViewItem item;
                int i = listView2.Items.Count-1;
                while(i>=0)
                {
                   item = listView2.Items[i];
                   if (item.SubItems[column].Text.Count() <= textSearch.Count() || !item.SubItems[column].Text.Substring(0, textSearch.Count()).ToLowerInvariant().Contains(textSearch.ToLowerInvariant()))
                        listView2.Items[i].Remove();
                   i--;
                }
            }
        }

        ///<summary>
        /// Función que inicializa la busqueda de un texto en todas las columnas excepto la telefónica al pulsar
        /// el botón inferior, más lenta que la anterior pero busca segmentos de palabra sin necesidad de que esten
        /// al principio.
        ///</summary>
        /// <param name="sender">
        /// No utilizado
        /// </param>
        /// <param name="e">
        /// No utilizado
        /// </param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox4.Text.Count() < 2) textBox4.Text = "Escribe un mínimo de dos caracteres";
            else
            {
                // Variable que recogen el textos escrito en el textBox inferior
                string text = textBox4.Text;
                // Si los textBox superiores tienen textos los limpia
                if (textBox1.Text.Count() != 0 || textBox2.Text.Count() != 0 || textBox3.Text.Count() != 0) clearBox(2);

                // Ruta para mostrar la lista completa cuando el texBox inferior no tiene contenido
                if (text.Count() == 0)
                {
                    listView1.Visible = true;
                    listView2.Visible = false;
                    listView2.Items.Clear();
                }
                // Ruta para inicializar, modificar y mostrar el segundo listView
                else
                {
                    // Cambia el listView mostrado
                    listView2.Visible = true;
                    listView1.Visible = false;
                    listView2.Items.Clear();

                    // Bucle que controla que elementos contienen el texto en alguna de sus columnas
                    int i = 0;
                    while (i < File.arrContacts.Count)
                    {
                        Contact contact = (Contact)File.arrContacts[i];
                        // Variables para indicar que un elemento ha sido encontrado
                        bool nameOK = contact.name.ToLowerInvariant().Contains(text.ToLowerInvariant());
                        bool surnameOK = contact.surname.ToLowerInvariant().Contains(text.ToLowerInvariant());
                        bool cityOK = contact.city.ToLowerInvariant().Contains(text.ToLowerInvariant());
                        i++;

                        // Si el elemento cumple los requisitos se añade a la listView2
                        if (nameOK || surnameOK || cityOK)
                        {
                            ListViewItem item1 = new ListViewItem(contact.name, 0);

                            item1.Checked = true;
                            item1.SubItems.Add(contact.surname);
                            item1.SubItems.Add(contact.city);
                            item1.SubItems.Add(contact.phone);

                            listView2.Items.AddRange(new ListViewItem[] { item1 });
                        }
                    }
                }
            }
        }
    }
}
