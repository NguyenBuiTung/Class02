import { LogoutOutlined } from "@ant-design/icons";
import { Button, Flex, Typography } from "antd";
import axios from "axios";
import { useEffect, useState } from "react";

export default function HeaderApp() {
  const sigout = () => {
    localStorage.removeItem("token");
    window.location.href = "/login";
  };
  const [loading, setLoading] = useState();
  const [user, setUser] = useState();
  const token = localStorage.getItem("token");
  useEffect(() => {
    setLoading(true)
    const fetchData = async () => {
      try {
        // Lấy token từ localStorage

        // Gửi yêu cầu GET với Bearer token trong header
        const response = await axios.get(
          "http://localhost:5218/api/User/getInfoUser",
          {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          }
        );
        setLoading(false); 
        setUser(response.data);
      } catch (error) {
        if (error.response && error.response.status === 401) {
          // Xử lý khi token không hợp lệ (có thể redirect đến trang đăng nhập)
          console.error("Unauthorized: Please log in again.");
        } else {
          console.error("Error fetching data:", error);
        }
      } finally {
        setLoading(false); // Đặt loading thành false khi yêu cầu hoàn tất
      }
    };

    fetchData();
  }, []);
  return (
    <Flex style={{width:"100%"}} justify="space-between" align="center">
      <img style={{width:"90px"}} src="https://haiphong.work/wp-content/uploads/2022/06/image_bao-an_230622-022032.png" alt="" />
    <div>
    {!loading && <Typography.Text style={{ marginRight: 5 }}>
        Xin chào  <strong style={{fontSize:16}}>{user?.userName}</strong>
      </Typography.Text>}
      <Button type="primary" danger onClick={sigout} icon={<LogoutOutlined />}>
        Đăng xuất{" "}
      </Button>
    </div>
    </Flex>
  );
}
