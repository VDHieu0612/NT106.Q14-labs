using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;

namespace Lab03_Bai04
{
    public partial class ClientForm : Form
    {
        private TcpClient client;
        private NetworkStream ns;
        private BinaryFormatter formatter;
        private readonly object networkLock = new object();
        private Dictionary<string, List<string>> movieRoomsData;
        private List<Ticket> tickets;
        private Dictionary<string, Button> seatButtons;
        private List<string> selectedSeats;
        private string currentMovie = "";
        private string currentRoom = "";
        private bool isConnected = false;

        public ClientForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            formatter = new BinaryFormatter();
            movieRoomsData = new Dictionary<string, List<string>>();
            tickets = new List<Ticket>();
            seatButtons = new Dictionary<string, Button>();
            selectedSeats = new List<string>();
            InitializeSeatButtons();
            DisableControls();
        }

        private void InitializeSeatButtons()
        {
            seatButtons.Add("A1", btnA1); seatButtons.Add("A2", btnA2); seatButtons.Add("A3", btnA3); seatButtons.Add("A4", btnA4); seatButtons.Add("A5", btnA5);
            seatButtons.Add("B1", btnB1); seatButtons.Add("B2", btnB2); seatButtons.Add("B3", btnB3); seatButtons.Add("B4", btnB4); seatButtons.Add("B5", btnB5);
            seatButtons.Add("C1", btnC1); seatButtons.Add("C2", btnC2); seatButtons.Add("C3", btnC3); seatButtons.Add("C4", btnC4); seatButtons.Add("C5", btnC5);
            foreach (var btn in seatButtons.Values)
            {
                btn.Click += SeatButton_Click;
            }
        }

        #region Connection Management
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtServerIP.Text))
            {
                MessageBox.Show("Vui lòng nhập IP Server!", "Lỗi"); return;
            }
            btnConnect.Enabled = false;
            lblStatus.Text = "● Đang kết nối...";
            lblStatus.ForeColor = Color.Orange;
            new Thread(() => {
                try
                {
                    client = new TcpClient();
                    client.Connect(txtServerIP.Text.Trim(), 8080);
                    ns = client.GetStream();
                    isConnected = true;
                    TicketMessage request = new TicketMessage { Command = "GET_INITIAL_DATA" };
                    TicketMessage response;
                    lock (networkLock)
                    {
                        formatter.Serialize(ns, request);
                        response = (TicketMessage)formatter.Deserialize(ns);
                    }
                    if (response.Success)
                    {
                        movieRoomsData = response.MovieRoomsData;
                        this.Invoke(new Action(() => {
                            lblStatus.Text = "● Đã kết nối";
                            lblStatus.ForeColor = Color.Green;
                            btnDisconnect.Enabled = true;
                            txtServerIP.Enabled = false;
                            cbChonPhim.Items.Clear();
                            response.Movies.ForEach(movie => cbChonPhim.Items.Add(movie));
                            EnableControls();
                        }));
                    }
                    else { throw new Exception("Không nhận được dữ liệu từ server."); }
                }
                catch (Exception ex)
                {
                    this.Invoke(new Action(() => {
                        MessageBox.Show($"Kết nối thất bại: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Disconnect();
                    }));
                }
            }).Start();
        }

        private void btnDisconnect_Click(object sender, EventArgs e) => Disconnect();

        private void Disconnect()
        {
            isConnected = false;
            client?.Close();
            ns?.Close();
            btnConnect.Enabled = true;
            btnDisconnect.Enabled = false;
            txtServerIP.Enabled = true;
            DisableControls();
            ResetAll();
        }
        #endregion

        #region Data Loading & UI Events
        private void LoadTickets(string movieName, string roomName)
        {
            if (!isConnected) return;
            try
            {
                TicketMessage request = new TicketMessage { Command = "GET_TICKETS", MovieName = movieName, RoomName = roomName };
                TicketMessage response;
                lock (networkLock)
                {
                    formatter.Serialize(ns, request);
                    response = (TicketMessage)formatter.Deserialize(ns);
                }
                if (response.Success && response.Tickets != null)
                {
                    tickets = response.Tickets;
                    this.Invoke(new Action(UpdateSeatDisplay));
                }
            }
            catch (Exception)
            {
                this.Invoke(new Action(() => {
                    if (isConnected)
                    {
                        MessageBox.Show("Mất kết nối đến server.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Disconnect();
                    }
                }));
            }
        }

        private void cbChonPhim_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbChonPhim.SelectedIndex == -1) return;
            currentMovie = cbChonPhim.SelectedItem.ToString();
            cbPhongChieu.Items.Clear();
            cbPhongChieu.Text = "";
            currentRoom = "";
            ResetSeats();
            txtThongTin.Clear();
            if (movieRoomsData.ContainsKey(currentMovie))
            {
                movieRoomsData[currentMovie].ForEach(room => cbPhongChieu.Items.Add(room));
            }
        }

        private void cbPhongChieu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPhongChieu.SelectedIndex == -1) return;
            currentRoom = cbPhongChieu.SelectedItem.ToString();
            ResetSeats();
            txtThongTin.Clear();
            new Thread(() => LoadTickets(currentMovie, currentRoom)).Start();
        }

        private void SeatButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentRoom)) return;
            Button btn = (Button)sender;
            var ticket = tickets.FirstOrDefault(t => t.SeatNumber == btn.Text);
            if (ticket == null || ticket.IsBooked) return;
            if (selectedSeats.Contains(btn.Text))
            {
                selectedSeats.Remove(btn.Text);
                btn.BackColor = Color.White;
            }
            else
            {
                selectedSeats.Add(btn.Text);
                btn.BackColor = Color.Yellow;
            }
            UpdateTicketInfo();
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (selectedSeats.Count == 0 || string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập tên và chọn ghế!", "Cảnh báo"); return;
            }
            List<string> seatsToBook = new List<string>(selectedSeats);
            string customerName = txtHoTen.Text.Trim();
            new Thread(() => {
                try
                {
                    this.Invoke(new Action(() => {
                        gbThanhToan.Enabled = false; gbChonGhe.Enabled = false;
                        progressBar1.Visible = true; progressBar1.Value = 0; progressBar1.Maximum = seatsToBook.Count;
                    }));
                    double totalPrice = 0;
                    List<string> successSeats = new List<string>(), failedSeats = new List<string>();
                    foreach (var seatNumber in seatsToBook)
                    {
                        var request = new TicketMessage { Command = "BOOK", MovieName = currentMovie, RoomName = currentRoom, SeatNumber = seatNumber, CustomerName = customerName };
                        TicketMessage response;
                        lock (networkLock)
                        {
                            formatter.Serialize(ns, request);
                            response = (TicketMessage)formatter.Deserialize(ns);
                        }
                        if (response.Success)
                        {
                            successSeats.Add(seatNumber);
                            var ticket = tickets.FirstOrDefault(t => t.SeatNumber == seatNumber);
                            if (ticket != null) { totalPrice += ticket.Price; }
                        }
                        else { failedSeats.Add(seatNumber); }
                        this.Invoke(new Action(() => progressBar1.Value++));
                    }
                    this.Invoke(new Action(() => {
                        string msg = "";
                        if (successSeats.Count > 0) msg += $"✓ Đặt thành công {successSeats.Count} vé:\n   Ghế: {string.Join(", ", successSeats)}\n   Tổng tiền: {totalPrice:N0} VNĐ\n\n";
                        if (failedSeats.Count > 0) msg += $"✗ Đặt thất bại {failedSeats.Count} vé:\n   Ghế: {string.Join(", ", failedSeats)}";
                        MessageBox.Show(msg, "Kết quả thanh toán");
                        progressBar1.Visible = false; gbThanhToan.Enabled = true; gbChonGhe.Enabled = true;
                        txtHoTen.Clear(); txtThongTin.Clear(); selectedSeats.Clear();
                        new Thread(() => LoadTickets(currentMovie, currentRoom)).Start();
                    }));
                }
                catch (Exception ex)
                {
                    this.Invoke(new Action(() => {
                        if (isConnected)
                        {
                            MessageBox.Show($"Mất kết nối: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Disconnect();
                        }
                    }));
                }
            }).Start();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            selectedSeats.Clear();
            txtThongTin.Clear();
            UpdateSeatDisplay();
        }
        #endregion

        #region UI & State Helpers
        private void UpdateSeatDisplay()
        {
            if (tickets == null) return;
            foreach (var ticket in tickets)
            {
                if (seatButtons.TryGetValue(ticket.SeatNumber, out Button btn))
                {
                    btn.Enabled = true;
                    btn.BackColor = ticket.IsBooked ? Color.Red : Color.White;
                    btn.ForeColor = ticket.IsBooked ? Color.White : Color.Black;
                }
            }
            foreach (var seat in selectedSeats)
            {
                if (seatButtons.TryGetValue(seat, out Button btn))
                {
                    btn.BackColor = Color.Yellow;
                }
            }
        }

        private void UpdateTicketInfo()
        {
            if (selectedSeats.Count == 0)
            {
                txtThongTin.Clear(); return;
            }
            double totalPrice = selectedSeats.Sum(seat => tickets.FirstOrDefault(t => t.SeatNumber == seat)?.Price ?? 0);
            txtThongTin.Text = $"Phim: {currentMovie}\r\nPhòng: {currentRoom}\r\nGhế: {string.Join(", ", selectedSeats)}\r\n-------------------\r\n💰 Tổng tiền: {totalPrice:N0} VNĐ\r\n";
        }

        private void DisableControls()
        {
            gbThongTin.Enabled = false; gbThanhToan.Enabled = false;
            foreach (var btn in seatButtons.Values) { btn.Enabled = false; }
        }

        private void EnableControls()
        {
            gbThongTin.Enabled = true; gbThanhToan.Enabled = true;
        }

        private void ResetSeats()
        {
            selectedSeats.Clear();
            tickets?.Clear();
            foreach (var btn in seatButtons.Values)
            {
                btn.BackColor = Color.White;
                btn.ForeColor = Color.Black;
                btn.Enabled = false;
            }
        }

        private void ResetAll()
        {
            cbChonPhim.Items.Clear(); cbPhongChieu.Items.Clear();
            txtHoTen.Clear(); txtThongTin.Clear();
            movieRoomsData.Clear(); selectedSeats.Clear();
            currentMovie = ""; currentRoom = "";
            lblStatus.Text = "● Chưa kết nối";
            lblStatus.ForeColor = Color.Red;
            ResetSeats();
        }

        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e) => Disconnect();
        #endregion
    }
}