import {
  unstable_HistoryRouter as HistoryRouter,
  Route,
  Routes,
} from "react-router-dom";
import { createBrowserHistory } from "history";
import Body from "../components/Body";
import SheepList from "../components/SheepList";
import ReportTime from "../components/ReportTime";

const history = createBrowserHistory();
export default function Router() {
  return (
    <HistoryRouter history={history}>   
      <Routes>
        <Route path="">
          <Route path="" element={<Body />}>
            <Route index element={<ReportTime/>} />
            <Route path="/order" element={<SheepList />} />
          </Route>
        </Route>
        {/* <Route path="" element={<CheckLogin />}>
        <Route path="/login" element={<Login />} />
        <Route path="/forgot-password" element={<ForgotPassWord />} />
        <Route path="/reset-password" element={<ResetPassWord />} />
      </Route> */}
      </Routes>
    </HistoryRouter>
  );
}
