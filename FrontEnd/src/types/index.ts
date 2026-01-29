export interface Person {
  id: string;
  name: string;
  age: number;
}

export interface Category {
  id: string;
  description: string;
  purpose: "despesa" | "receita" | "ambas";
  disable?: boolean;
}

export interface Transaction {
  id: string;
  description: string;
  value: number;
  type: "despesa" | "receita";
  personId?: string;
  categoryId?: string; 
  personName?: string;
  categoryPurpose?: string;
}
