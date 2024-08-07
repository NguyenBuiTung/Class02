import { LogoutOutlined } from "@ant-design/icons";
import { Button } from "antd";

export default function HeaderApp() {
  const sigout = ()=>{
    localStorage.removeItem("token");
    window.location.href ="/login"
  }
  return (
    <div style={{textAlign:"end",width:"100%"}}>
      <Button type="primary" danger onClick={sigout} icon={<LogoutOutlined/>}>Đăng xuất </Button>
    </div>
  );
}
