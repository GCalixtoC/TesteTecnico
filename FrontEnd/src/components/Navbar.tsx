import { AppBar, Toolbar, Button } from "@mui/material";
import { Link } from "react-router-dom";

export default function Navbar() {
  return (
    <AppBar position="static">
      <Toolbar>
        <Button color="inherit" component={Link} to="/">Pessoas</Button>
        <Button color="inherit" component={Link} to="/categorias">Categorias</Button>
        <Button color="inherit" component={Link} to="/transacoes">Transações</Button>
      </Toolbar>
    </AppBar>
  );
}
