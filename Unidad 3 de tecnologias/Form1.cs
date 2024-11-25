using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace Unidad_3_de_tecnologias
{
    public partial class Form1 : Form
    {
        private SerialPort serialPort; // Instancia de SerialPort

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Configurar el puerto serial
            serialPort = new SerialPort();
            serialPort.BaudRate = 9600; // Configuración de 9600 baudios
            serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler); // Evento para manejar datos recibidos

            // Aquí puedes agregar los puertos disponibles al ComboBox
            foreach (string port in SerialPort.GetPortNames())
            {
                comboBoxPorts.Items.Add(port);
            }
        }

        private void btnConnect_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxPorts.SelectedItem != null)
                {
                    serialPort.PortName = comboBoxPorts.SelectedItem.ToString(); // Seleccionar el puerto COM
                    serialPort.Open(); // Abrir conexión
                    MessageBox.Show("Conectado a " + serialPort.PortName);
                }
                else
                {
                    MessageBox.Show("Por favor, seleccione un puerto COM.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar: " + ex.Message);
            }
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            // Leer datos recibidos
            string inData = serialPort.ReadLine(); // Leer la línea de datos del puerto

            // Invocar método para actualizar la interfaz de usuario
            this.Invoke(new MethodInvoker(delegate
            {
                richTextBoxReceivedData.AppendText(inData + Environment.NewLine + "\n"); // Agregar datos al RichTextBox

                // Aquí se asume que inData es una representación en formato binario
                // Por ejemplo, '01000001' para 'A'
                // Convertir de binario a carácter
                if (!string.IsNullOrEmpty(inData))
                {
                    try
                    {
                        // Asumiendo que inData es una cadena de bits (ej. "01000001")
                        string trimmedData = inData.Trim(); // Limpiar espacios en blanco
                        if (trimmedData.Length == 8) // Debe tener 8 bits
                        {
                            int charCode = Convert.ToInt32(trimmedData, 2); // Convertir de binario a entero
                            char letra = (char)charCode; // Convertir a carácter
                            richTextBoxFrase.AppendText(letra.ToString()); // Mostrar la letra en el RichTextBox
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al convertir los datos: " + ex.Message);
                    }
                }
            }));
        }

        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            // Cerrar el puerto al salir de la aplicación
            MessageBox.Show("Usted ha salido del programa. \n" + "Puerto " + serialPort.PortName + " desconectado.");
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }

        private void btnDesconnected_Click(object sender, EventArgs e)
        {
            // Cerrar el puerto al salir de la aplicación
            if (serialPort.IsOpen)
            {
                MessageBox.Show("Puerto " + serialPort.PortName + " desconectado.");
                serialPort.Close();
                richTextBoxReceivedData.Text = "";
                richTextBoxFrase.Text = ""; // Limpiar richTextBoxFrase también
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Usted ha salido del programa. \n" + "Puerto " + serialPort.PortName + " desconectado.");

            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
            this.Close();
        }
    }
}
