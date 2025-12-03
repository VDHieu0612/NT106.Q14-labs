using HomNayAnGi.Models;
using HomNayAnGi.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab04_Bai07
{
    public partial class MainForm : Form
    {
        // Biến lưu trạng thái trang hiện tại
        private int _currentPage = 1;
        private int _pageSize = 5;

        private int _totalItems = 0;   // Tổng số món ăn từ Server trả về
        private int _totalPages = 0;   // Tổng số trang tính được

        // Cờ hiệu để tránh lỗi lặp vô tận khi gán dữ liệu vào ComboBox
        private bool _isBindingData = false;


        // 1. Danh sách cho Tab "All"
        private List<Food> _listAllFoods = new List<Food>();

        // 2. Danh sách cho Tab "Tôi đóng góp"
        private List<Food> _listMyFoods = new List<Food>();
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // 1. Cài đặt hiển thị tên user
            if (Program.CurrentUser != null)
            {
                
                lblWelcome.Text = $"Welcome, {Program.CurrentUser.first_name} |";
            }

            // 2. Cài đặt mặc định cho ComboBox Phân trang (Nếu có)
             cboPageSize.SelectedIndex = 0; 

            // 3. Tải dữ liệu lần đầu
            LoadFoodList();

        }

        private async void LoadFoodList()
        {
            // Xóa danh sách cũ đi
            flpFoodList.Controls.Clear();
            ToggleLoading(true);
            // Gọi Service
            FoodService service = new FoodService();
            var response = await service.GetAllFoodAsync(_currentPage, _pageSize);

            if (response != null && response.Data != null)
            {
                
                _listAllFoods = response.Data;
                if (response.Pagination != null)
                {
                    // Gọi hàm cập nhật giao diện phân trang với số Total lấy từ API
                    UpdatePaginationUI(response.Pagination);
                }

                // Vẽ lên giao diện (flpFoodList)
                RenderList(flpFoodList, _listAllFoods);
            }
            else
            {
                // Nếu không có dữ liệu
                Label lblEmpty = new Label();
                lblEmpty.Text = "Chưa có món ăn nào!";
                lblEmpty.AutoSize = true;
                flpFoodList.Controls.Add(lblEmpty);
            }
            ToggleLoading(false);
        }

        private async void LoadMyFoodList()
        {
            ToggleLoading(true);
            FoodService service = new FoodService();
            // Gọi API mới viết
            var response = await service.GetMyFoodAsync(_currentPage, _pageSize); // Tạm lấy 100 món đầu tiên

            if (response != null)
            {
                if (response.Pagination != null)
                {
                    // Gọi hàm cập nhật giao diện phân trang với số Total lấy từ API
                    UpdatePaginationUI(response.Pagination);
                }

                _listMyFoods = response.Data;
                // Vẽ lên cái FlowLayoutPanel thứ 2 (flpMyFood)
                RenderList(flpMyFood, response.Data);
            }
            ToggleLoading(false);
        }

        // Hàm dùng chung để vẽ danh sách món ăn lên FlowLayoutPanel bất kỳ
        private void RenderList(FlowLayoutPanel panel, List<Food> data)
        {
            panel.Controls.Clear(); // Xóa cũ
            if (data == null || data.Count == 0) return;

            foreach (var item in data)
            {
                FoodItem foodControl = new FoodItem();
                foodControl.SetData(
                    item.TenMonAn,
                    item.Gia.ToString("N0") + " đ",
                    item.DiaChi,
                    item.NguoiDongGop,
                    item.HinhAnh
                );

                // Chỉnh chiều rộng trừ thanh cuộn
                foodControl.Width = panel.Width - 25;

                panel.Controls.Add(foodControl);

                // 1. Chỉ hiện nút Xóa nếu đang ở Tab "Tôi đóng góp"
                if (tabControl1.SelectedTab == tabPage2)
                {
                    foodControl.ShowDeleteButton(true);
                }
                else
                {
                    foodControl.ShowDeleteButton(false);
                }
                foodControl.OnDeleteClicked += async (s, e) =>
                {
                    // Hỏi cho chắc ăn
                    DialogResult dialog = MessageBox.Show(
                        $"Bạn có chắc muốn xóa món '{item.TenMonAn}' không?",
                        "Xác nhận xóa",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (dialog == DialogResult.Yes)
                    {
                        // Gọi Service xóa
                        FoodService sv = new FoodService();
                        bool ketQua = await sv.DeleteFoodAsync(item.Id); // item.Id lấy từ vòng lặp

                        if (ketQua)
                        {
                            MessageBox.Show("Đã xóa thành công!");

                            // Tải lại danh sách để mất cái món vừa xóa đi
                            LoadMyFoodList();
                        }
                    }
                };
            }
        }

        private void flpMyFood_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage1) // Tab Tất cả
            {
                // Hàm này bạn đã viết ở bước trước (nhớ sửa lại để nó dùng RenderList cho gọn nhé)
                LoadFoodList();
            }
            else if (tabControl1.SelectedTab == tabPage2) // Tab Tôi đóng góp
            {
                LoadMyFoodList();
            }
        }

        private void btnRandom_Click(object sender, EventArgs e)
        {
            // 1. Xác định danh sách dựa trên Tab đang chọn
            List<Food> targetList = null;

            if (tabControl1.SelectedTab == tabPage1) // Tab All
            {
                targetList = _listAllFoods;
            }
            else if (tabControl1.SelectedTab == tabPage2) // Tab Tôi đóng góp
            {
                targetList = _listMyFoods;
            }

            // 2. Kiểm tra danh sách rỗng
            if (targetList == null || targetList.Count == 0)
            {
                MessageBox.Show("Danh sách đang trống! Hãy tải dữ liệu trước.", "Thông báo");
                return;
            }

            // 3. Random chọn món
            Random rand = new Random();
            int index = rand.Next(0, targetList.Count);
            Food monAnMayMan = targetList[index];

            // 4. HIỂN THỊ KẾT QUẢ BẰNG FORM 

            // Khởi tạo form kết quả
            RandomResultForm resultForm = new RandomResultForm();

            // Truyền dữ liệu món ăn vào
            resultForm.SetFood(monAnMayMan);

            // Hiện lên (ShowDialog để bắt buộc người dùng tắt đi mới thao tác tiếp được)
            resultForm.ShowDialog();
        }

        private void UpdatePaginationUI(HomNayAnGi.Models.Pagination apiPagination)
        {
            if (apiPagination == null) return;

            _isBindingData = true; // KHÓA SỰ KIỆN (Quan trọng để không bị loop)

            // 1. Cập nhật biến cục bộ theo dữ liệu thật từ Server
            _totalItems = apiPagination.Total;
            _pageSize = apiPagination.PageSize;   // Lấy pageSize server trả về
            _currentPage = apiPagination.Current; // Lấy trang hiện tại server trả về

            // 2. Cập nhật ComboBox PageSize (Kích thước trang)
            // Nếu trong danh sách chưa có items nào thì thêm mặc định
            if (cboPageSize.Items.Count == 0)
            {
                cboPageSize.Items.Add("5");
                cboPageSize.Items.Add("10");
                cboPageSize.Items.Add("20");
            }

            // Chọn đúng giá trị server trả về (Ví dụ server trả về 5 thì combo chọn số 5)
            // Dùng ToString() vì items trong combobox thường là text
            string sizeString = _pageSize.ToString();
            if (cboPageSize.Items.Contains(sizeString))
            {
                cboPageSize.SelectedItem = sizeString;
            }
            else
            {
                // Trường hợp server trả về số lạ (VD: 15) chưa có trong list thì thêm vào
                cboPageSize.Items.Add(sizeString);
                cboPageSize.SelectedItem = sizeString;
            }

            // 3. Tính lại tổng số trang
            _totalPages = (int)Math.Ceiling((double)_totalItems / _pageSize);
            if (_totalPages < 1) _totalPages = 1;

            // 4. Cập nhật ComboBox Số trang (cboPage)
            cboPage.Items.Clear();
            for (int i = 1; i <= _totalPages; i++)
            {
                cboPage.Items.Add(i.ToString());
            }

            // Chọn đúng trang hiện tại
            // (Kiểm tra an toàn để không bị lỗi index)
            if (_currentPage > 0 && _currentPage <= cboPage.Items.Count)
            {
                cboPage.SelectedIndex = _currentPage - 1;
            }
            _isBindingData = false; // MỞ KHÓA SỰ KIỆN
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddFoodForm frm = new AddFoodForm();

            // Mở Form dưới dạng Dialog (Chặn form chính lại)
            // Và kiểm tra xem người dùng tắt form hay đã thêm thành công (DialogResult.OK)
            if (frm.ShowDialog() == DialogResult.OK)
            {
                // Nếu thêm thành công -> Tự động chuyển sang tab "Tôi đóng góp" và tải lại
                tabControl1.SelectedTab = tabPage2; // Chuyển Tab
                LoadMyFoodList(); LoadFoodList(); // Tải lại dữ liệu
            }
        }

        private void cboPage_SelectedIndexChanged(object sender, EventArgs e)
        {

            // Cờ này đang bật nghĩa là code đang tự chỉnh sửa, người dùng không thao tác -> Bỏ qua
            if (_isBindingData) return;

            // QUAN TRỌNG: Kiểm tra null trước khi dùng
            if (cboPage.SelectedItem == null) return;

            if (int.TryParse(cboPage.SelectedItem.ToString(), out int newPage))
            {
                _currentPage = newPage;
                ReloadCurrentTab(); // Tải lại dữ liệu
            }

        }
        // Hàm phụ trợ để biết đang ở Tab nào mà gọi hàm Load tương ứng
        private void ReloadCurrentTab()
        {
            if (tabControl1.SelectedTab == tabPage1)
                LoadFoodList();
            else
                LoadMyFoodList();
        }

        private void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isBindingData) return;
            if (cboPageSize.SelectedItem == null) return;

            if (int.TryParse(cboPageSize.SelectedItem.ToString(), out int newSize))
            {
                _pageSize = newSize;

                // QUAN TRỌNG: Khi đổi kích thước trang (ví dụ 5 lên 10), 
                // số lượng trang sẽ thay đổi, nên tốt nhất là RESET về trang 1.
                _currentPage = 1;

                ReloadCurrentTab();
            }
        }

        private void lkLogOut_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            // 1. Hỏi người dùng cho chắc ăn
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất không?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // 2. Xóa dữ liệu lưu trong biến tĩnh (Program.cs)
                // Bước này quan trọng để nếu ai đó mở lại form sẽ không còn token cũ
                Program.Token = null;
                Program.CurrentUser = null;

                // KHỞI ĐỘNG LẠI ỨNG DỤNG
                // Lệnh này sẽ tắt toàn bộ Form hiện tại và chạy lại từ đầu (vào lại LoginForm)
                Application.Restart();

            }
        }


        private void ToggleLoading(bool isLoading)
        {
            // Nếu đang loading thì hiện thanh bar, ngược lại thì ẩn
            pgbLoading.Visible = isLoading;

            // Nếu muốn an toàn hơn, tắt các nút bấm để user không bấm lung tung khi đang load
            btnRandom.Enabled = !isLoading;
            btnAdd.Enabled = !isLoading;
            cboPage.Enabled = !isLoading;
            cboPageSize.Enabled = !isLoading;
           
        }
    }
}
