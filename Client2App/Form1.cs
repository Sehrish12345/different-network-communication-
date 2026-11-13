using System;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Client2App
{
    public partial class Client2Form : Form
    {
        // TextBoxes
        private TextBox txtOtherIP;
        private TextBox txtOtherPort;
        private TextBox txtMessage;
        private TextBox txtLocalPort;
        // Received Messages
        private RichTextBox txtReceived;

        // Buttons
        private Button btnStart;
        private Button btnSend;
        private Button btnStop;

        // Status
        private Label lblStatus;
        // UDP Communication
        private UdpClient udpClient;

        private IPEndPoint gatewayEndPoint;
        private Thread receiveThread;
        private bool isRunning = false;
        public Client2Form()
        {
            InitializeComponent();
            BuildClientDesign();

            btnStart.Click += btnStart_Click;
            btnStop.Click += btnStop_Click;
            btnSend.Click += btnSend_Click;
        }
        private void BuildClientDesign()
        {
            // Main Form
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 9F);

            // Header Panel
            Panel pnlHeader = new Panel();
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Size = new Size(600, 85);
            pnlHeader.BackColor = Color.FromArgb(40, 122, 98);

            // Title
            Label lblTitle = new Label();
            lblTitle.Text = "UDP Client 2";
            lblTitle.Location = new Point(25, 15);
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;

            // Subtitle
            Label lblSubtitle = new Label();
            lblSubtitle.Text = "Text Communication Application";
            lblSubtitle.Location = new Point(28, 52);
            lblSubtitle.AutoSize = true;
            lblSubtitle.ForeColor = Color.WhiteSmoke;

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSubtitle);

            this.Controls.Add(pnlHeader);
            // Destination Network
            Panel pnlDestination = new Panel();
            pnlDestination.Location = new Point(25, 105);
            pnlDestination.Size = new Size(550, 150);
            pnlDestination.BackColor = Color.FromArgb(235, 247, 241);

            // Section Heading
            Label lblDestination = new Label();
            lblDestination.Text = "Gateway Connection";
            lblDestination.Location = new Point(15, 10);
            lblDestination.AutoSize = true;
            lblDestination.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblDestination.ForeColor = Color.FromArgb(40, 122, 98);

            // IP Address
            Label lblIP = new Label();
            lblIP.Text = "Network B IP Address";
            lblIP.Location = new Point(15, 40);
            lblIP.Size = new Size(190, 25);

            txtOtherIP = new TextBox();
            txtOtherIP.Location = new Point(210, 38);
            txtOtherIP.Size = new Size(325, 25);
            txtOtherIP.BackColor = Color.White;

            // Port
            Label lblPort = new Label();
            lblPort.Text = "Network B Port";
            lblPort.Location = new Point(15, 80);
            lblPort.Size = new Size(190, 25);

            txtOtherPort = new TextBox();
            txtOtherPort.Location = new Point(210, 78);
            txtOtherPort.Size = new Size(325, 25);
            txtOtherPort.BackColor = Color.White;

            // Local Port
            Label lblLocalPort = new Label();
            lblLocalPort.Text = "Local Port";
            lblLocalPort.Location = new Point(15, 115);
            lblLocalPort.Size = new Size(190, 25);

            txtLocalPort = new TextBox();
            txtLocalPort.Location = new Point(210, 113);
            txtLocalPort.Size = new Size(325, 25);
            txtLocalPort.BackColor = Color.White;
            txtLocalPort.Text = "6001";

            pnlDestination.Controls.Add(lblDestination);
            pnlDestination.Controls.Add(lblIP);
            pnlDestination.Controls.Add(txtOtherIP);
            pnlDestination.Controls.Add(lblPort);
            pnlDestination.Controls.Add(txtOtherPort);
            pnlDestination.Controls.Add(lblLocalPort);
            pnlDestination.Controls.Add(txtLocalPort);

            this.Controls.Add(pnlDestination);
            // Start Button
            btnStart = new Button();
            btnStart.Text = "Start";
            btnStart.Location = new Point(25, 255);
            btnStart.Size = new Size(110, 38);
            btnStart.BackColor = Color.FromArgb(40, 122, 98);
            btnStart.ForeColor = Color.White;
            btnStart.FlatStyle = FlatStyle.Flat;
            btnStart.FlatAppearance.BorderSize = 0;

            // Stop Button
            btnStop = new Button();
            btnStop.Text = "Stop";
            btnStop.Location = new Point(150, 255);
            btnStop.Size = new Size(75, 38);
            btnStop.BackColor = Color.FromArgb(166, 77, 101);
            btnStop.ForeColor = Color.White;
            btnStop.FlatStyle = FlatStyle.Flat;
            btnStop.FlatAppearance.BorderSize = 0;
            btnStop.Enabled = true;

            // Status Label
            lblStatus = new Label();
            lblStatus.Text = "Status: Disconnected";
            lblStatus.Location = new Point(245, 280);
            lblStatus.AutoSize = true;
            lblStatus.ForeColor = Color.FromArgb(100, 100, 100);

            this.Controls.Add(btnStart);
            this.Controls.Add(btnStop);
            this.Controls.Add(lblStatus);
            // Send Message Heading
            Label lblSend = new Label();
            lblSend.Text = "Send Message";
            lblSend.Location = new Point(25, 315);
            lblSend.AutoSize = true;
            lblSend.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblSend.ForeColor = Color.FromArgb(40, 122, 98);

            // Message TextBox
            txtMessage = new TextBox();
            txtMessage.Location = new Point(25, 345);
            txtMessage.Size = new Size(430, 35);
            txtMessage.Font = new Font("Segoe UI", 10F);

            // Send Button
            btnSend = new Button();
            btnSend.Text = "Send";
            btnSend.Location = new Point(465, 343);
            btnSend.Size = new Size(110, 38);
            btnSend.BackColor = Color.FromArgb(40, 122, 98);
            btnSend.ForeColor = Color.White;
            btnSend.FlatStyle = FlatStyle.Flat;
            btnSend.FlatAppearance.BorderSize = 0;

            this.Controls.Add(lblSend);
            this.Controls.Add(txtMessage);
            this.Controls.Add(btnSend);
            // Received Messages Heading
            Label lblReceived = new Label();
            lblReceived.Text = "Received Messages";
            lblReceived.Location = new Point(25, 400);
            lblReceived.AutoSize = true;
            lblReceived.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblReceived.ForeColor = Color.FromArgb(40, 122, 98);

            // Received Messages Box
            txtReceived = new RichTextBox();
            txtReceived.Location = new Point(25, 430);
            txtReceived.Size = new Size(550, 95);
            txtReceived.BackColor = Color.White;
            txtReceived.ReadOnly = true;
            txtReceived.Font = new Font("Segoe UI", 9F);

            this.Controls.Add(lblReceived);
            this.Controls.Add(txtReceived);
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            IPAddress gatewayIP;
            int gatewayPort;

            if (!IPAddress.TryParse(txtOtherIP.Text, out gatewayIP))
            {
                MessageBox.Show("Please enter a valid Gateway IP Address.");
                return;
            }

            if (!int.TryParse(txtOtherPort.Text, out gatewayPort) ||
                gatewayPort < 1 || gatewayPort > 65535)
            {
                MessageBox.Show("Please enter a valid Gateway Port.");
                return;
            }
            try
            {
                int localPort;

                if (!int.TryParse(txtLocalPort.Text, out localPort) ||
                    localPort < 1 || localPort > 65535)
                {
                    MessageBox.Show("Please enter a valid Local Port.");
                    return;
                }

                udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, localPort));

                gatewayEndPoint = new IPEndPoint(gatewayIP, gatewayPort);

                isRunning = true;

                receiveThread = new Thread(ReceiveMessages);
                receiveThread.IsBackground = true;
                receiveThread.Start();
               

                lblStatus.Text = "Status: Ready";
                btnStart.Enabled = false;
                btnStop.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Client Error: " + ex.Message);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (udpClient == null || !isRunning)
            {
                MessageBox.Show("Please start the client first.");
                return;
            }

            string message = txtMessage.Text.Trim();

            if (message == "")
            {
                MessageBox.Show("Please enter a message.");
                return;
            }

            byte[] data = Encoding.UTF8.GetBytes(message);

            udpClient.Send(data, data.Length, gatewayEndPoint);

            
            txtMessage.Clear();
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            if (!isRunning)
                return;
            isRunning = false;

            udpClient?.Close();
            udpClient = null;

            btnStart.Enabled = true;
            btnStop.Enabled = false;

            lblStatus.Text = "Status: Disconnected";
        }
        private void ReceiveMessages()
        {
            IPEndPoint remoteEndPoint =
                new IPEndPoint(IPAddress.Any, 0);

            while (isRunning)
            {
                try
                {
                    byte[] data = udpClient.Receive(ref remoteEndPoint);
                    
                    string message = Encoding.UTF8.GetString(data);

                    txtReceived.Invoke(new Action(() =>
                    {
                        txtReceived.AppendText(
                            "Received form Client 1: " + message + Environment.NewLine
                        );
                    }));
                }
                catch
                {
                    break;
                }
            }
        }
        private void Client2Form_Load(object sender, EventArgs e)
        {

        }
    }
}
