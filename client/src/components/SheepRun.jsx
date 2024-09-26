import { Flex } from "antd";

export default function SheepRun({ loadRun, progress, currentSheep }) {
  return (
    <div>
      <Flex justify="space-between">
     <div>
     <p>Cửa trang trại</p>
     <img style={{width:90}} src="https://sundoor.vn/wp-content/uploads/2021/06/cua-phong-ngu-co-o-thoang-3.jpg" alt="" />
     </div>
    <div>
    <p>Cân</p>
    <img style={{width:90}}  src="https://nhonhoa.net/wp-content/uploads/2015/08/300kg.1.jpg" alt="" />
    </div>
     <div>
     <p>Xe chở</p>
     <img style={{width:120}} src="https://vmmotors.vn/data/images/image-3.jpg" alt="" />
     </div>
      </Flex>
      <div className="progress-container">
        <div
          className={`progress-bar ${loadRun ? "active" : ""}`}
          style={{ width: `${progress}%` }}
        >
          <div
            style={{
              backgroundColor: `${
                currentSheep?.color === "Xám"
                  ? "gray"
                  : currentSheep?.color === "Đen"
                  ? "black"
                  : currentSheep?.color === "Trắng"
                  ? "#fff"
                  : "#fff"
              } `,
            }}
            className="sheep"
          >
            <div className="head">
              <div className="eye left">
                <div className="pupil"></div>
              </div>
              <div className="eye right">
                <div className="pupil"></div>
              </div>
              <div className="ear left"></div>
              <div className="ear right"></div>
            </div>
            <div className="leg left"></div>
            <div className="leg right"></div>
          </div>
        </div>
      </div>

      {currentSheep && (
        <div>
          <h2>Thông tin cừu</h2>
          <p>
            {" "}
            <strong>Màu:</strong> <span>{currentSheep.color}</span>
          </p>
          <p>
            {" "}
            <strong>Trọng lượng: </strong>{" "}
            <span>{currentSheep.meatWeight} kg</span>
          </p>

          <p>
            {" "}
            <strong>Khối lượng lông: </strong>{" "}
            <span>{currentSheep.woolWeight} kg</span>
          </p>

          <p>
            {" "}
            <strong>Thơi gian: </strong> <span>{currentSheep.time} s</span>
          </p>
        </div>
      )}
    </div>
  );
}
