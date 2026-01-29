import { useEffect, useState } from "react";
import api from "../api/api";
import { Person } from "../types";
import {
  Container, Typography, TextField, Button,
  Table, TableBody, TableCell, TableHead, TableRow, Paper
} from "@mui/material";

export default function Persons() {
  const [persons, setPersons] = useState<Person[]>([]);
  const [name, setName] = useState("");
  const [age, setAge] = useState<number | "">("");
  const [editingId, setEditingId] = useState<string | null>(null); // controla edição

  // Busca todas as pessoas
  const loadPersons = async () => {
    const res = await api.get("/person");
    setPersons(res.data);
  };

  // Cria ou edita pessoa
  const savePerson = async () => {
    if (!name.trim()) return alert("Preencha nome e idade válidos");

    if (editingId) {
      // Editando existente
      await api.put(`/person`, { id: editingId, name, age });
      setEditingId(null);
    } else {
      // Criando nova
      await api.post("/person", { name, age });
    }

    setName("");
    setAge(0);
    loadPersons();
  };

  // Preenche campos para editar
  const handleEdit = (person: Person) => {
    setEditingId(person.id);
    setName(person.name);
    setAge(person.age);
  };

  // Exclui pessoa
  const handleDelete = async (id: string) => {
    if (window.confirm("Deseja realmente excluir esta pessoa?")) {
      await api.delete(`/person/${id}`);
      loadPersons();
    }
  };

  useEffect(() => {
    loadPersons();
  }, []);

  return (
    <Container>
      <Typography variant="h4" gutterBottom>
        Cadastro de Pessoas
      </Typography>

      <TextField
        label="Nome"
        value={name}
        onChange={(e) => setName(e.target.value)}
        sx={{ mr: 2 }}
      />
      <TextField
        label="Idade"
        type="number"
        value={age}
        onChange={(e) => {
          const val = e.target.value;
          setAge(val === "" ? "" : Number(val));
        }}
        sx={{
          mr: 2,
          '& input[type=number]': { MozAppearance: 'textfield' },
          '& input[type=number]::-webkit-outer-spin-button': {
            WebkitAppearance: 'none',
            margin: 0,
          },
          '& input[type=number]::-webkit-inner-spin-button': {
            WebkitAppearance: 'none',
            margin: 0,
          },
        }}
      />
      <Button
        variant="contained"
        color={editingId ? "warning" : "primary"}
        onClick={savePerson}
      >
        {editingId ? "Salvar Alterações" : "Adicionar"}
      </Button>

      {editingId && (
        <Button
          variant="outlined"
          color="inherit"
          sx={{ ml: 2 }}
          onClick={() => {
            setEditingId(null);
            setName("");
            setAge(0);
          }}
        >
          Cancelar
        </Button>
      )}

      {/* 🔹 Tabela */}
      <Paper sx={{ mt: 4 }}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Nome</TableCell>
              <TableCell>Idade</TableCell>
              <TableCell>Ações</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {persons.map((p) => (
              <TableRow key={p.id}>
                <TableCell>{p.name}</TableCell>
                <TableCell>{p.age}</TableCell>
                <TableCell>
                  <Button
                    color="primary"
                    onClick={() => handleEdit(p)}
                    sx={{ mr: 1 }}
                  >
                    Editar
                  </Button>
                  <Button
                    color="error"
                    onClick={() => handleDelete(p.id)}
                  >
                    Excluir
                  </Button>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </Paper>
    </Container>
  );
}
