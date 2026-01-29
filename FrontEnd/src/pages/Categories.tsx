import { useEffect, useState } from "react";
import api from "../api/api";
import { Category } from "../types";
import {
  Container, Typography, TextField, Button,
  Select, MenuItem, Table, TableHead, TableRow, TableCell, TableBody, Paper
} from "@mui/material";

export default function CategoriesPage() {
  const [categories, setCategories] = useState<Category[]>([]);
  const [description, setDescription] = useState("");
  const [purpose, setPurpose] = useState("despesa");

  const loadCategories = async () => {
    const res = await api.get("/category");
    setCategories(res.data);
  };

  const addCategory = async () => {
    if (!description) return alert("Informe uma descrição.");
    await api.post("/category", { description, purpose });
    setDescription("");
    setPurpose("despesa");
    loadCategories();
  };

  useEffect(() => { loadCategories(); }, []);

  return (
    <Container>
      <Typography variant="h4" gutterBottom>Cadastro de Categorias</Typography>
      <TextField label="Descrição" value={description} onChange={(e) => setDescription(e.target.value)} sx={{ mr: 2 }} />
      <Select value={purpose} onChange={(e) => setPurpose(e.target.value)} sx={{ mr: 2 }}>
        <MenuItem value="despesa">Despesa</MenuItem>
        <MenuItem value="receita">Receita</MenuItem>
        <MenuItem value="ambas">Ambas</MenuItem>
      </Select>
      <Button variant="contained" onClick={addCategory}>Adicionar</Button>

      <Paper sx={{ mt: 4 }}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Descrição</TableCell>
              <TableCell>Finalidade</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {categories.map((x) => (
              <TableRow key={x.id}>
                <TableCell>{x.description}</TableCell>
                <TableCell>{x.purpose}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </Paper>
    </Container>
  );
}
