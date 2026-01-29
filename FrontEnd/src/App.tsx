import { BrowserRouter, Routes, Route } from "react-router-dom";
import Person from "./pages/Person";
import CategoriesPage from "./pages/Categories";
import Transactions from "./pages/Transactions";
import Navbar from "./components/Navbar";
import { Container } from "@mui/material";

export default function App() {
  return (
    <BrowserRouter>
      <Navbar />
      <Container sx={{ mt: 4 }}>
        <Routes>
          <Route path="/" element={<Person />} />
          <Route path="/categorias" element={<CategoriesPage />} />
          <Route path="/transacoes" element={<Transactions />} />
        </Routes>
      </Container>
    </BrowserRouter>
  );
}