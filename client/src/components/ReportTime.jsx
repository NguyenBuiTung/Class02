import { Button, InputNumber, Select, Table } from "antd";
import axios from "axios";
import { useEffect, useState } from "react";
import {
  ComposedChart,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  Legend,
  Bar,
  ResponsiveContainer,
  PieChart,
  Pie,
  Cell,
} from "recharts";

export default function ReportTime() {
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState();
  const [data, setData] = useState([]);
  const [timestart, setTimestart] = useState("");
  const [timeend, setTimeend] = useState("");
  const [color, setColor] = useState("all");
  const token = localStorage.getItem("token");
  const fetchSheeps = async () => {
    setLoading(true); // Set loading to true when making request
    try {
      const response = await axios.get(
        `http://localhost:5218/report-by-time?startTime=${timestart}&endTime=${timeend}&color=${color}`,
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );
      //   console.log(response);
      setOrders(response.data);
      setLoading(false);
    } catch (error) {
      setLoading(false);
      console.error("Error fetching data:", error);
    } finally {
      setLoading(false); // Set loading to false when request completes
    }
  };
  useEffect(() => {
    const dataNew = orders?.sheeps?.map((items, index) => {
      return {
        key: index,
        color: items.color,
        meatWeight: items?.meatWeight,
        woolWeight: items?.woolWeight,
        time: items?.time,
      };
    });
    setData(dataNew);
  }, [orders]);
  const [dataChart, setDataChart] = useState();
  useEffect(() => {
    const newData = data?.map((item) => {
      return {
        "Màu sắc": item.color,
        "Trọng lượng": item.meatWeight,
        "Khối lượng lông": item?.woolWeight,
        "Thời gian": item?.time,
      };
    });
    setDataChart(newData);
  }, [data]);
  const [pieChart, setPiechart] = useState([]);
  const [showWeight, setShowWeight] = useState(false);
  const [showLong, setShowLong] = useState(false);
  const [showTime, setShowTime] = useState(false);
  useEffect(() => {
    const dataNew = orders?.sheepCountByColor?.map((item) => {
      return {
        color: item?.color,
        value: showWeight
          ? item.totalMeatWeight
          : showLong
          ? item.totalWoolWeight
          : showTime
          ? item.totalTime
          : 0, // giá trị mặc định nếu không có gì được chọn
      };
    });

    setPiechart(dataNew);
  }, [orders, showWeight, showLong, showTime]); //
  const RADIAN = Math.PI / 180;
  const colorMapping = {
    trắng: "#e3d0d0d0", // Màu trắng
    đen: "#000000", // Màu đen
    xám: "#808080", // Màu xám
  };

  const renderCustomizedLabel = ({
    cx,
    cy,
    midAngle,
    innerRadius,
    outerRadius,
    percent,
  }) => {
    const radius = innerRadius + (outerRadius - innerRadius) * 0.5;
    const x = cx + (radius + 25) * Math.cos(-midAngle * RADIAN);
    const y = cy + (radius + 25) * Math.sin(-midAngle * RADIAN);

    return (
      <text
        x={x}
        y={y}
        fill="#f5222d"
        textAnchor="middle" // Căn giữa theo trục x
        dominantBaseline="middle" // Căn giữa theo trục y
        fontSize={15}
      >
        {`${(percent * 100).toFixed(0)}%`}
      </text>
    );
  };
  const columns = [
    {
      title: "STT",
      dataIndex: "key",
    },
    {
      title: "Màu sắc",
      dataIndex: "color",
      editable: true,
    },
    {
      title: "Trọng lượng",
      dataIndex: "meatWeight",
      editable: true,
    },
    {
      title: "Khối lượng lông",
      dataIndex: "woolWeight",
      editable: true,
    },
    {
      title: "Thời gian",
      dataIndex: "time",
      editable: true,
    },
  ];
  const handleExcel = async () => {
    // Its important to set the 'Content-Type': 'blob' and responseType:'arraybuffer'.
    const headers = {
      "Content-Type": "blob",
      Authorization: `Bearer ${token}`,
    };
    const config = {
      method: "GET",
      url: "http://localhost:5218/export-sheep",
      responseType: "arraybuffer",
      headers,
    };

    try {
      const response = await axios(config);

      const outputFilename = `Báo cáo.xls`;

      // If you want to download file automatically using link attribute.
      const url = URL.createObjectURL(new Blob([response.data]));
      const link = document.createElement("a");
      link.href = url;
      link.setAttribute("download", outputFilename);
      document.body.appendChild(link);
      link.click();

      // OR you can save/write file locally.
      //  fs.writeFileSync(outputFilename, response.data);
    } catch (error) {
      throw Error(error);
    }
  };

  const handleChart = (value) => {
    switch (value) {
      case "weight":
        setShowWeight(true);
        setShowLong(false);
        setShowTime(false);
        break;
      case "long":
        setShowLong(true);
        setShowWeight(false);
        setShowTime(false);
        break;
      case "speed":
        setShowTime(true);
        setShowLong(false);
        setShowWeight(false);
        break;
    }
  };
  return (
    <>
      <div style={{ margin: "10px 0" }}>
        <InputNumber
          onChange={(value) => setTimestart(value)}
          placeholder="Thời gian từ"
          style={{ marginRight: "10px", width: "200px" }}
          min={1}
          max={5}
        />
        <InputNumber
          onChange={(value) => setTimeend(value)}
          placeholder="Thời gian đến"
          min={1}
          max={5}
          style={{ marginRight: "10px", width: "200px" }}
        />
        <Select
          placeholder="màu sắc"
          style={{ width: "200px", marginRight: "10px" }}
          onChange={(value) => setColor(value)}
        >
          <Select.Option value="all">Tất cả</Select.Option>
          <Select.Option value="Trắng">Trắng</Select.Option>
          <Select.Option value="Đen">Đen</Select.Option>
          <Select.Option value="Xám">Xám</Select.Option>
        </Select>
        <Button
          style={{ marginRight: "10px" }}
          type="primary"
          onClick={fetchSheeps}
          loading={loading}
        >
          Báo cáo
        </Button>
        <Button type="primary" onClick={handleExcel}>
          Xuất Excel
        </Button>
        <Select
          onChange={handleChart}
          style={{ width: 200, marginLeft: 10 }}
          placeholder="Chọn biểu đồ"
        >
          <Select.Option value="weight">Biểu đồ khối lượng</Select.Option>
          <Select.Option value="speed">Biểu đồ tỷ trọng tốc độ</Select.Option>
          <Select.Option value="long">Biểu đồ tỷ trọng lông</Select.Option>
        </Select>
      </div>
      {showWeight && (
        <div style={{ width: "100%", height: 300 }}>
          <ResponsiveContainer>
            <ComposedChart
              data={dataChart}
              margin={{
                top: 20,
                right: 20,
                bottom: 20,
                left: 50,
              }}
            >
              <CartesianGrid stroke="#f5f5f5" />
              <XAxis
                dataKey="Màu sắc"
                scale="auto"
                label={{
                  // value: "Thời gian",
                  position: "insideBottomRight",
                  offset: -20,
                }}
              />
              <YAxis
                tickFormatter={(value) => value.toLocaleString()}
                label={{
                  value: "KG",
                  angle: 0,
                  position: "insideBottom",
                }}
              />

              <Tooltip formatter={(value) => value.toLocaleString()} />
              <Legend />
              <Bar barSize={15} dataKey="Trọng lượng" fill="#413ea0" />
            </ComposedChart>
          </ResponsiveContainer>
        </div>
      )}
      <div style={{ width: "100%", height: 300, margin: "20px 0" }}>
        <ResponsiveContainer>
          <PieChart>
            <Pie
              data={pieChart}
              cx="50%"
              cy="50%"
              labelLine={false}
              label={renderCustomizedLabel}
              outerRadius={130}
              fill="#8884d8"
              dataKey="value"
            >
              {pieChart?.map((entry, index) => {
                return (
                  <Cell
                    key={`cell-${index}`}
                    fill={colorMapping[entry.color.toLowerCase()]} // Sử dụng màu hoặc fallback
                  />
                );
              })}
            </Pie>
            <Tooltip
              formatter={(value) => {
                if (showWeight) {
                  return `${value.toLocaleString()} kg trọng lượng`; // Hiển thị cho trọng lượng
                } else if (showLong) {
                  return `${value.toLocaleString()} kg lông cừu`; // Hiển thị cho chiều dài
                } else if (showTime) {
                  return `${value.toLocaleString()} giây`; // Hiển thị cho thời gian
                } else {
                  return `${value.toLocaleString()}`; // Mặc định nếu không có gì được chọn
                }
              }}
            />
            <Legend
              verticalAlign="middle"
              align="left"
              layout="vertical"
              payload={pieChart?.map((entry) => ({
                value: "cừu " + entry.color,
                type: "square",
                color: colorMapping[entry.color], // Sử dụng màu cố định hoặc fallback về màu trong mảng COLORS
              }))}
            />
          </PieChart>
        </ResponsiveContainer>
      </div>

      <Table
        bordered
        loading={loading}
        columns={columns}
        dataSource={data}
        pagination={{
          position: ["bottomCenter"], // Đặt phân trang ở vị trí giữa dưới cùng
        }}
      />
    </>
  );
}
