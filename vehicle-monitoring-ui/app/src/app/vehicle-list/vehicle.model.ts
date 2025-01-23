export interface Customer {
  id: number;
  name: string;
  address: string;
}

export interface Vehicle {
  identificationNumber: string;
  regNumber: string;
  lastPing: string | null;
  isConnected: boolean;
  customer: Customer;
}

