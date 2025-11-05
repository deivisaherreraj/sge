export interface Employee {
  id: number;
  fullName: string;
  hireDate: string;
  role: string;
  salary: number;
  department: string;
}

export interface EmployeeCreate {
  fullName: string;
  hireDate: string;
  role: string;
  salary: number;
  departmentId: number;
}

export interface EmployeeUpdate {
  fullName: string;
  hireDate: string;
  role: string;
  salary: number;
  departmentId: number;
}

export interface Department {
  id: number;
  name: string;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
}