import { LockOutlined, LoginOutlined, UserOutlined } from "@ant-design/icons";
import { Button, Form, Input, message, Spin } from "antd";
import axios from "axios";
import { useState } from "react";
import { useNavigate } from "react-router-dom";

export default function Login() {
  const [form] = Form.useForm();
  const navigate = useNavigate();
  const [loading,setLoading]=useState()
  const onFinish = async (values) => {
    setLoading(true)
    try {
      const response = await axios.post("http://localhost:5218/api/Account/login", {
        username: values.username,
        password: values.password,
      });

      const { data } = response;
      // Lưu token vào localStorage hoặc state
      localStorage.setItem("token", data.token);
      setLoading(false)
      message.success("Đăng nhập thành công!");
      navigate("/")
      // Redirect hoặc cập nhật giao diện
    } catch (error) {
      if (error.response && error.response.status === 401) {
        setLoading(false)
        message.error("Tên người dùng hoặc mật khẩu không đúng!");
      } else {
        setLoading(false)
        message.error("Đăng nhập thất bại. Vui lòng thử lại sau!");
      }
    }
  };

  return (
    <>
      <div
        className="login"
        style={{
          backgroundImage: `url(${"https://baa.vn/Images/NPP-baa.png"})`,
        }}
      >
        <div className="box-login">
          <div className="box-login-container">
            <div className="login-box">
              <div className="title">
                <img
                  style={{ width: 150 }}
                  src={
                    "https://haiphong.work/wp-content/uploads/2022/06/image_bao-an_230622-022032.png"
                  }
                  alt="logo"
                />
                <h2>Bảo An Cừu - Đăng Nhập</h2>
              </div>
              <div className="form-login">
                <Form name="normal_login" onFinish={onFinish} form={form}>
                  <Form.Item
                    name="username"
                    rules={[
                      {
                        required: true,
                        message: "Vui lòng nhập tài khoản",
                      },
                    ]}
                  >
                    <Input
                      prefix={<UserOutlined className="site-form-item-icon" />}
                      placeholder="username"
                    />
                  </Form.Item>
                  <Form.Item
                    name="password"
                    rules={[
                      {
                        required: true,
                        message: "Vui lòng nhập mật khẩu!",
                      },
                    ]}
                  >
                    <Input.Password
                      placeholder="Mật khẩu"
                      prefix={<LockOutlined className="site-form-item-icon" />}
                    />
                  </Form.Item>
                  {loading ? (
                  <Spin tip="Xin chờ" size="large">
                    <div className="content" />
                  </Spin>
                ) : null}

                  <Form.Item style={{ textAlign: "center", margin: "15px 0" }}>
                    <Button
                      style={{
                        padding: "9px 36px",
                        height: "auto",
                        fontSize: "16px",
                        fontWeight: "500",
                        width: "100%",
                      }}
                      disabled={loading}
                      icon={<LoginOutlined />}
                      type="primary"
                      htmlType="submit"
                      className="login-form-button"
                    >
                      Đăng nhập
                    </Button>
                  </Form.Item>
                </Form>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}
