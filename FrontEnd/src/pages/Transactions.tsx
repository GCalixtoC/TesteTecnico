import { useEffect, useState } from "react";
import api from "../api/api";
import { Transaction, Person, Category,} from "../types";
import {
  Container, Typography, TextField, Button,
  Select, MenuItem, Table, TableHead, TableRow, TableCell, TableBody, Paper,
  FormControl,
  InputLabel
} from "@mui/material";

export default function Transactions() {
  const [transactions, setTransactions] = useState<Transaction[]>([]);
  const [persons, setPersons] = useState<Person[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);
  const [description, setDescription] = useState("");
  const [value, setValue] = useState<number | "">("");
  const [type, setType] = useState("");
  const [personId, setPersonId] = useState("");
  const [categoryId, setCategoryId] = useState("");

  const loadAll = async () => {
    const [t, p, c] = await Promise.all([
      api.get("/transaction"),
      api.get("/person"),
      api.get("/category"),
    ]);
    setTransactions(t.data);
    setPersons(p.data);
    setCategories(c.data.map((x:any) => ({...x, disable: false})));
  };

  const addTransaction = async () => {
    if (!personId || !categoryId || !value || !type) {
      alert("Preencha todos os campos corretamente!");
      return;
    }

    const res = await api.post("/transaction", {
      description,
      value,
      type,
      personId,
      categoryId,
    });

    setTransactions((prev) => [...prev, res.data]);

    setDescription("");
    setValue("");
    setPersonId("");
    setCategoryId("");
};

  useEffect(() => { loadAll(); }, []);

  return (
    <Container>
      <Typography variant="h4" gutterBottom>Transações</Typography>
      <TextField label="Descrição" value={description} onChange={(e) => setDescription(e.target.value)} sx={{ mr: 2 }} />
      <TextField label="Valor" type="number" value={value} onChange={(e) =>{ 
        const val = e.target.value;
        setValue(val === "" ? "" : Number(val))}} 
        sx={{ mr: 2 }} />
      <FormControl sx={{minWidth: 120 }}>
      <InputLabel id="label-id">Tipo</InputLabel>
      <Select autoWidth id="select-id" labelId="label-id" label="Tipo" value={type} onChange={(e) => setType(e.target.value)} sx={{ mr: 2 }}>
        <MenuItem value="despesa">Despesa</MenuItem>
        <MenuItem value="receita">Receita</MenuItem>
      </Select>
    </FormControl>

    <FormControl sx={{minWidth: 180 }}>
      <InputLabel id="label-id">Pessoa</InputLabel>
      <Select autoWidth id="select-id" labelId="label-id" label="Pessoa" value={personId} onChange={(e) => setPersonId(e.target.value)}sx={{ mr: 2}}>
        {persons.map((p) => (
          <MenuItem key={p.id} value={p.id}>
            {p.name}
          </MenuItem>
        ))}
      </Select>
    </FormControl>

    <FormControl sx={{minWidth: 150 }}>
      <InputLabel id="label-id">Categoria</InputLabel>
      <Select autoWidth id="select-id" labelId="label-id" label="Categoria" value={categoryId} onChange={(e) => setCategoryId(e.target.value)}
        sx={{ mr: 2}}>
        {categories.map((c) => {
          if(type ==="despesa" && c.purpose !== "receita"){
            return(
            <MenuItem key={c.id} value={c.id} disabled={c.disable}>
              {c.description}
            </MenuItem>
        )}
            else if(type === "receita")
              return(
                <MenuItem key={c.id} value={c.id} disabled={c.disable}>
                  {c.description}
                </MenuItem>
        )})}
      </Select>
    </FormControl> 


      <Button variant="contained" onClick={addTransaction}>Adicionar</Button>

      <Paper sx={{ mt: 4 }}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Descrição</TableCell>
              <TableCell>Valor</TableCell>
              <TableCell>Tipo</TableCell>
              <TableCell>Pessoa</TableCell>
              <TableCell>Categoria</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {transactions.map(x => (
              <TableRow key={x.id}>
                <TableCell>{x.description}</TableCell>
                <TableCell>{x.value}</TableCell>
                <TableCell>{x.type}</TableCell>
                <TableCell>{x.personName}</TableCell>
                <TableCell>{x.categoryPurpose}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </Paper>
    </Container>
  );
}
