import {
  unstable_HistoryRouter as HistoryRouter,
  Route,
  Routes,
} from "react-router-dom";
import { createBrowserHistory } from "history";
import Body from "../components/Body";
import SheepList from "../components/SheepList";
import ReportTime from "../components/ReportTime";
import CheckLogin from "../components/CheckLogin";
import Login from "../components/Login";

const history = createBrowserHistory();
export default function Router() {
  return (
    <HistoryRouter history={history}>
      <Routes>
        <Route path="" element={<CheckLogin/>} >
          <Route path="" element={<Body />}>
            <Route index element={<ReportTime />} />
            <Route path="/order" element={<SheepList />} />
          </Route>
        </Route>
        <Route >
          <Route path="/login" element={<Login />} />
        </Route>
      </Routes>
    </HistoryRouter>
  );
}
