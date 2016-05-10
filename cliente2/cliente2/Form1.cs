/* form.cs
 * Descripción:
 * Cliente con protocolo TCP.
 * Aplicación de chat.
 * El cliente manda un nombre como identificador de cliente al servidor.
 * Cada mensaje enviado por el cliente es recibido por el servidor y reenviado a todos los clientes.
 * Fecha: 21 de enero de 2015
 * Versión 1.0
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// Necesario para sockets.
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace cliente2
{
    public partial class frmCliente : Form
    {
        TcpClient Cliente;
        NetworkStream StreamCliente;
        string mensaje;
        public frmCliente()
        {
            InitializeComponent();
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            try
            {
                Cliente = new TcpClient("127.0.0.1", 4444);
                StreamCliente = Cliente.GetStream();
                byte[] data = Encoding.ASCII.GetBytes(txtNombre.Text);
                StreamCliente.Write(data, 0, data.Length);
                StreamCliente.Flush();
                mensaje = "Conectando con el servidor...";
                Mensaje_Recibido();
                Thread Hilo = new Thread(recibir_mensaje);
                Hilo.Start();
            }
            catch
            {
                MessageBox.Show("Error al conectar con el servidor");
            }
        }

        public void Mensaje_Recibido()
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(Mensaje_Recibido));
            else
                txtChat.Text += "\r\n =>>" + mensaje;
        }
        private void btnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                StreamCliente = Cliente.GetStream();
                byte[] data = Encoding.ASCII.GetBytes(txtEnviar.Text);
                StreamCliente.Write(data, 0, data.Length);
                StreamCliente.Flush();
            }
            catch
            {
                MessageBox.Show("No se pudo enviar el mensaje");
            }
        }
        private void recibir_mensaje()
        {
            while (true)
            {
                StreamCliente = Cliente.GetStream();
                byte[] bit = new byte[1];
                StreamCliente.Read(bit, 0, bit.Length);
                mensaje = Encoding.ASCII.GetString(bit);
                Mensaje_Recibido();
            }
        }

        private void frmCliente_Load(object sender, EventArgs e)
        {

        }
    }
}