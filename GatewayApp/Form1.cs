using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace GatewayApp
{
    public partial class GatewayForm : Form
    {
        // Main Panels
        private Panel pnlHeader;
        private Panel pnlNetworkA;
        private Panel pnlNetworkB;

        // TextBoxes
        private TextBox txtA_IP;
        private TextBox txtA_Port;
        private TextBox txtB_IP;
        private TextBox txtB_Port;

        // Buttons
        private Button btnStart;
        private Button btnStop;

        // Communication Log
        private RichTextBox txtLog;

        // Status
        private Label lblStatus;
        // UDP Communication
        private UdpClient udpA;
        private UdpClient udpB;

        // Client Endpoints
        private IPEndPoint clientAEndPoint;
        private IPEndPoint clientBEndPoint;

        // Receive Threads
        private Thread threadA;
        private Thread threadB;

        // Gateway Status
        private bool isRunning = false;
        public GatewayForm()
        {
            InitializeComponent();
            BuildGatewayDesign();
            btnStart.Click += btnStart_Click;
            btnStop.Click += btnStop_Click;
        }
        private void BuildGatewayDesign()
        {
            // Main Form
            this.ClientSize = new Size(800, 650);
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 9F);

            // Header
            pnlHeader = new Panel();
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Size = new Size(800, 85);
            pnlHeader.BackColor = Color.FromArgb(25, 39, 67);

            this.Controls.Add(pnlHeader);

            // Header Title
            Label lblTitle = new Label();
            lblTitle.Text = "UDP Network Gateway";
            lblTitle.Location = new Point(25, 15);
            lblTitle.Size = new Size(400, 30);
            lblTitle.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;

            pnlHeader.Controls.Add(lblTitle);

            // Header Subtitle
            Label lblSubtitle = new Label();
            lblSubtitle.Text = "Communication between Network A and Network B";
            lblSubtitle.Location = new Point(27, 50);
            lblSubtitle.Size = new Size(450, 20);
            lblSubtitle.Font = new Font("Segoe UI", 9F);
            lblSubtitle.ForeColor = Color.LightGray;

            pnlHeader.Controls.Add(lblSubtitle);
            // Network A Panel
            pnlNetworkA = new Panel();
            pnlNetworkA.Location = new Point(30, 110);
            pnlNetworkA.Size = new Size(350, 200);
            pnlNetworkA.BackColor = Color.FromArgb(225, 238, 252);
            pnlNetworkA.BorderStyle = BorderStyle.FixedSingle;

            this.Controls.Add(pnlNetworkA);

            // Network A Heading
            Label lblNetworkA = new Label();
            lblNetworkA.Text = "Network A";
            lblNetworkA.Location = new Point(15, 15);
            lblNetworkA.Size = new Size(300, 28);
            lblNetworkA.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblNetworkA.ForeColor = Color.FromArgb(40, 105, 175);

            pnlNetworkA.Controls.Add(lblNetworkA);
            // Network A IP Address
            Label lblA_IP = new Label();
            lblA_IP.Text = "Local IP Address";
            lblA_IP.Location = new Point(15, 60);
            lblA_IP.Size = new Size(300, 22);
            lblA_IP.ForeColor = Color.FromArgb(55, 65, 80);

            pnlNetworkA.Controls.Add(lblA_IP);

            txtA_IP = new TextBox();
            txtA_IP.Location = new Point(15, 85);
            txtA_IP.Size = new Size(315, 28);
            txtA_IP.BackColor = Color.White;
            txtA_IP.BorderStyle = BorderStyle.FixedSingle;

            pnlNetworkA.Controls.Add(txtA_IP);
            // Network A Listening Port
            Label lblA_Port = new Label();
            lblA_Port.Text = "Listening Port";
            lblA_Port.Location = new Point(15, 125);
            lblA_Port.Size = new Size(300, 22);
            lblA_Port.ForeColor = Color.FromArgb(55, 65, 80);

            pnlNetworkA.Controls.Add(lblA_Port);

            txtA_Port = new TextBox();
            txtA_Port.Location = new Point(15, 150);
            txtA_Port.Size = new Size(315, 28);
            txtA_Port.BackColor = Color.White;
            txtA_Port.BorderStyle = BorderStyle.FixedSingle;

            pnlNetworkA.Controls.Add(txtA_Port);
            // Network B Panel
            pnlNetworkB = new Panel();
            pnlNetworkB.Location = new Point(420, 110);
            pnlNetworkB.Size = new Size(350, 200);
            pnlNetworkB.BackColor = Color.FromArgb(226, 242, 232);
            pnlNetworkB.BorderStyle = BorderStyle.FixedSingle;

            this.Controls.Add(pnlNetworkB);

            // Network B Heading
            Label lblNetworkB = new Label();
            lblNetworkB.Text = "Network B";
            lblNetworkB.Location = new Point(15, 15);
            lblNetworkB.Size = new Size(300, 28);
            lblNetworkB.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblNetworkB.ForeColor = Color.FromArgb(35, 130, 85);

            pnlNetworkB.Controls.Add(lblNetworkB);
            // Network B IP Address
            Label lblB_IP = new Label();
            lblB_IP.Text = "Local IP Address";
            lblB_IP.Location = new Point(15, 60);
            lblB_IP.Size = new Size(300, 22);
            lblB_IP.ForeColor = Color.FromArgb(55, 65, 80);

            pnlNetworkB.Controls.Add(lblB_IP);

            txtB_IP = new TextBox();
            txtB_IP.Location = new Point(15, 85);
            txtB_IP.Size = new Size(315, 28);
            txtB_IP.BackColor = Color.White;
            txtB_IP.BorderStyle = BorderStyle.FixedSingle;

            pnlNetworkB.Controls.Add(txtB_IP);
            // Network B Listening Port
            Label lblB_Port = new Label();
            lblB_Port.Text = "Listening Port";
            lblB_Port.Location = new Point(15, 125);
            lblB_Port.Size = new Size(300, 22);
            lblB_Port.ForeColor = Color.FromArgb(55, 65, 80);

            pnlNetworkB.Controls.Add(lblB_Port);

            txtB_Port = new TextBox();
            txtB_Port.Location = new Point(15, 150);
            txtB_Port.Size = new Size(315, 28);
            txtB_Port.BackColor = Color.White;
            txtB_Port.BorderStyle = BorderStyle.FixedSingle;

            pnlNetworkB.Controls.Add(txtB_Port);
            // Bind & Start Button
            btnStart = new Button();
            btnStart.Text = "Bind & Start";
            btnStart.Location = new Point(285, 330);
            btnStart.Size = new Size(110, 35);
            btnStart.BackColor = Color.FromArgb(40, 122, 98);
            btnStart.ForeColor = Color.White;
            btnStart.FlatStyle = FlatStyle.Flat;
            btnStart.FlatAppearance.BorderSize = 0;
            btnStart.Cursor = Cursors.Hand;

            this.Controls.Add(btnStart);
            // Stop Button
            btnStop = new Button();
            btnStop.Text = "Stop";
            btnStop.Location = new Point(405, 330);
            btnStop.Size = new Size(75, 35);
            btnStop.BackColor = Color.FromArgb(166, 77, 101);
            btnStop.ForeColor = Color.White;
            btnStop.FlatStyle = FlatStyle.Flat;
            btnStop.FlatAppearance.BorderSize = 0;
            btnStop.Cursor = Cursors.Hand;
            btnStop.Enabled = true;

            this.Controls.Add(btnStop);
            // Communication Log Heading
            Label lblLog = new Label();
            lblLog.Text = "Communication Log";
            lblLog.Location = new Point(30, 390);
            lblLog.Size = new Size(300, 28);
            lblLog.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblLog.ForeColor = Color.FromArgb(25, 39, 67);

            this.Controls.Add(lblLog);
            // Communication Log Box
            txtLog = new RichTextBox();
            txtLog.Location = new Point(30, 425);
            txtLog.Size = new Size(740, 165);
            txtLog.BackColor = Color.FromArgb(25, 39, 67);
            txtLog.ForeColor = Color.White;
            txtLog.ReadOnly = true;
            txtLog.BorderStyle = BorderStyle.None;
            txtLog.Font = new Font("Consolas", 10F);

            this.Controls.Add(txtLog);
            // Status Label
            lblStatus = new Label();
            lblStatus.Text = "Status: Not Started";
            lblStatus.Location = new Point(30, 610);
            lblStatus.Size = new Size(300, 25);
            lblStatus.Font = new Font("Segoe UI", 9F);
            lblStatus.ForeColor = Color.FromArgb(70, 80, 95);

            this.Controls.Add(lblStatus);
            // UDP Protocol Label
            Label lblProtocol = new Label();
            lblProtocol.Text = "UDP Protocol";
            lblProtocol.Location = new Point(680, 610);
            lblProtocol.Size = new Size(100, 25);
            lblProtocol.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblProtocol.ForeColor = Color.FromArgb(40, 105, 175);

            this.Controls.Add(lblProtocol);
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            IPAddress ipA, ipB;
            int portA, portB;

            // Validate Network A IP
            if (!IPAddress.TryParse(txtA_IP.Text, out ipA))
            {
                MessageBox.Show("Please enter a valid Network A IP Address.");
                return;
            }

            // Validate Network B IP
            if (!IPAddress.TryParse(txtB_IP.Text, out ipB))
            {
                MessageBox.Show("Please enter a valid Network B IP Address.");
                return;
            }
            // Validate Network A Port
            if (!int.TryParse(txtA_Port.Text, out portA) ||
                portA < 1 || portA > 65535)
            {
                MessageBox.Show("Please enter a valid Network A Port.");
                return;
            }

            // Validate Network B Port
            if (!int.TryParse(txtB_Port.Text, out portB) ||
                portB < 1 || portB > 65535)
            {
                MessageBox.Show("Please enter a valid Network B Port.");
                return;
            }
            try
            {
                udpA = new UdpClient(new IPEndPoint(ipA, portA));
                udpB = new UdpClient(new IPEndPoint(ipB, portB));

                isRunning = true;

                AddLog("Gateway started successfully.");
                AddLog("Network A is ready.");
                AddLog("Network B is ready.");

                threadA = new Thread(ReceiveFromA);
                threadA.IsBackground = true;
                threadA.Start();

                threadB = new Thread(ReceiveFromB);
                threadB.IsBackground = true;
                threadB.Start();
                lblStatus.Text = "Status: Running";

                btnStart.Enabled = false;
                btnStop.Enabled = true;
            }
            catch (Exception ex)
            {
                udpA?.Close();
                udpB?.Close();

                udpA = null;
                udpB = null;

                isRunning = false;

                MessageBox.Show("Gateway Error: " + ex.Message);
            }
        }

            private void btnStop_Click(object sender, EventArgs e)
        {
            if (!isRunning)
                return;
            isRunning = false;

            udpA?.Close();
            udpB?.Close();

            udpA = null;
            udpB = null;

            clientAEndPoint = null;
            clientBEndPoint = null;

            btnStart.Enabled = true;
            btnStop.Enabled = false;

            lblStatus.Text = "Status: Stopped";

            AddLog("Gateway stopped.");
      
        }
        private void AddLog(string message)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action(() => AddLog(message)));
                return;
            }

            txtLog.AppendText(
                DateTime.Now.ToString("HH:mm:ss") + "  " +
                message + Environment.NewLine
            );
        }
        private void ReceiveFromA()
        {
            IPEndPoint remoteEndPoint =
                new IPEndPoint(IPAddress.Any, 0);

            while (isRunning)
            {
                try
                {
                    byte[] data = udpA.Receive(ref remoteEndPoint);

                    string message = Encoding.UTF8.GetString(data);

                    clientAEndPoint = new IPEndPoint(
                        remoteEndPoint.Address,
                        remoteEndPoint.Port
                    );

                    AddLog("Network A: " + message);

                    if (clientBEndPoint != null)
                    {
                        udpB.Send(data, data.Length, clientBEndPoint);
                        AddLog("Message forwarded to Network B.");
                    }
                    else
                    {
                        AddLog("Waiting for Client 2...");
                    }
                }
                catch
                {
                    break;
                }
            }
        }
        private void ReceiveFromB()
        {
            IPEndPoint remoteEndPoint =
                new IPEndPoint(IPAddress.Any, 0);

            while (isRunning)
            {
                try
                {
                    byte[] data = udpB.Receive(ref remoteEndPoint);

                    string message = Encoding.UTF8.GetString(data);

                    clientBEndPoint = new IPEndPoint(
                        remoteEndPoint.Address,
                        remoteEndPoint.Port
                    );

                    AddLog("Network B: " + message);

                    if (clientAEndPoint != null)
                    {
                        udpA.Send(data, data.Length, clientAEndPoint);
                        AddLog("Message forwarded to Network A.");
                    }
                    else
                    {
                        AddLog("Waiting for Client 1...");
                    }
                }
                catch
                {
                    break;
                }
            }
        }
        private void GatewayForm_Load(object sender, EventArgs e)
        {

        }
    }
}
