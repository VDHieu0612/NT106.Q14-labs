using HomNayAnGi.Models;
using HomNayAnGi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Lab04_Bai07
{
    public partial class MainForm : Form
    {
        private FoodRepository _foodRepository;
        private List<Food> _allLocalFoods = new List<Food>();
        private int _currentPage = 1;
        private int _pageSize = 5;
        private int _totalPages = 0;
        private bool _isBindingData = false;

        public MainForm()
        {
            InitializeComponent();
            _foodRepository = new FoodRepository();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            cboPageSize.SelectedItem = "5";
            LoadLocalData();
        }

        // Tải toàn bộ dữ liệu từ SQLite
        private async void LoadLocalData()
        {
            ToggleLoading(true);
            try
            {
                _allLocalFoods = await _foodRepository.GetAllContributedFoodsAsync();
                _currentPage = 1;
                UpdatePaginationUI();
                RenderCurrentPage();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu SQLite: " + ex.Message);
            }
            finally
            {
                ToggleLoading(false);
            }
        }

        // Cập nhật giao diện phân trang
        private void UpdatePaginationUI()
        {
            _isBindingData = true;

            int totalItems = _allLocalFoods.Count;
            _totalPages = (int)Math.Ceiling((double)totalItems / _pageSize);
            if (_totalPages == 0) _totalPages = 1;

            cboPage.Items.Clear();
            for (int i = 1; i <= _totalPages; i++)
            {
                cboPage.Items.Add(i.ToString());
            }

            if (_currentPage > _totalPages) _currentPage = _totalPages;
            if (cboPage.Items.Count > 0)
            {
                cboPage.SelectedIndex = _currentPage - 1;
            }

            _isBindingData = false;
        }

        // Vẽ trang hiện tại lên giao diện
        private void RenderCurrentPage()
        {
            flpFoodList.Controls.Clear();

            var pagedFoods = _allLocalFoods
                                .Skip((_currentPage - 1) * _pageSize)
                                .Take(_pageSize)
                                .ToList();

            RenderList(flpFoodList, pagedFoods);
        }

        // Hàm vẽ danh sách món ăn
        private void RenderList(FlowLayoutPanel panel, List<Food> data)
        {
            panel.Controls.Clear();
            foreach (var item in data)
            {
                FoodItem foodControl = new FoodItem();
                foodControl.SetData(
                    item.TenMonAn,
                    item.Gia > 0 ? item.Gia.ToString("N0") + " đ" : "Miễn phí",
                    string.IsNullOrEmpty(item.DiaChi) ? "Đóng góp qua Email" : item.DiaChi,
                    item.NguoiDongGop,
                    item.HinhAnh
                );
                foodControl.Width = panel.Width - 25;
                foodControl.ShowDeleteButton(false);
                panel.Controls.Add(foodControl);
            }
        }

        // --- CÁC SỰ KIỆN NÚT BẤM ---
        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddFoodForm frm = new AddFoodForm();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Đã gửi email! Hãy bấm nút 'Cập nhật đóng góp' để tải món ăn về máy.");
            }
        }

        private async void btnGetContributions_Click(object sender, EventArgs e)
        {
            string imapServer = "imap.gmail.com";
            int port = 993;
            string email = "23521269@gm.uit.edu.vn";  
            string password = "oqdk olqi geak dvtu"; 

            if (email.Contains("your_email"))
            {
                MessageBox.Show("Bạn chưa cấu hình Email trong code MainForm.cs!");
                return;
            }

            ToggleLoading(true);
            btnGetContributions.Text = "Đang tải...";
            try
            {
                var emailService = new EmailContributionService(imapServer, port, email, password);
                int count = await emailService.ProcessNewContributionsAsync();
                if (count > 0)
                {
                    MessageBox.Show($"Đã tải thành công {count} món ăn mới!", "Thành công");
                    LoadLocalData();
                }
                else
                {
                    MessageBox.Show("Không có email đóng góp mới nào.", "Thông báo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xử lý Email: " + ex.Message);
            }
            finally
            {
                ToggleLoading(false);
                btnGetContributions.Text = "Cập nhật đóng góp";
            }
        }

        private async void btnRandom_Click(object sender, EventArgs e)
        {
            try
            {
                Food randomFood = await _foodRepository.GetRandomContributedFoodAsync();
                if (randomFood != null)
                {
                    RandomResultForm resultForm = new RandomResultForm();
                    resultForm.SetFood(randomFood);
                    resultForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Kho dữ liệu trống. Hãy cập nhật từ Email trước!", "Thông báo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        // --- CÁC HÀM PHỤ TRỢ ---
        private void ToggleLoading(bool isLoading)
        {
            pgbLoading.Visible = isLoading;
            btnAdd.Enabled = !isLoading;
            btnGetContributions.Enabled = !isLoading;
            btnRandom.Enabled = !isLoading;
            cboPage.Enabled = !isLoading;
            cboPageSize.Enabled = !isLoading;
        }

        private void lkLogOut_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Application.Restart();
        }

        // --- SỰ KIỆN PHÂN TRANG ---
        private void cboPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isBindingData) return;
            if (cboPage.SelectedItem == null) return;
            _currentPage = cboPage.SelectedIndex + 1;
            RenderCurrentPage();
        }

        private void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isBindingData) return;
            if (cboPageSize.SelectedItem == null) return;
            _pageSize = int.Parse(cboPageSize.SelectedItem.ToString());
            _currentPage = 1;
            UpdatePaginationUI();
            RenderCurrentPage();
        }
    }
}