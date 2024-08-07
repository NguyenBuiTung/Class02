import { Button } from "antd";

export default function HeaderApp() {
  const sigout = ()=>{
    localStorage.removeItem("token");
    window.location.href ="/login"
  }
  return (
    <div>
      <Button type="primary" danger onClick={sigout}>Đăng xuất </Button>
    </div>
  );
}
