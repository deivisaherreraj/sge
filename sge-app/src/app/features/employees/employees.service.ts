import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { Employee, EmployeeCreate, EmployeeUpdate, Department, PagedResult } from '../../core/models/employee.model';

@Injectable({
  providedIn: 'root'
})
export class EmployeesService {
  private http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiBaseUrl}/employees`;
  private readonly departmentsUrl = `${environment.apiBaseUrl}/departments`;

  constructor() {}

  getEmployees(
    query?: string,
    page: number = 1,
    pageSize: number = 10,
    orderBy: string = 'FullName',
    desc: boolean = false
  ): Observable<PagedResult<Employee>> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString())
      .set('orderBy', orderBy)
      .set('desc', desc.toString());

    if (query) {
      params = params.set('query', query);
    }

    return this.http.get<PagedResult<Employee>>(this.apiUrl, { params });
  }

  getEmployee(id: number): Observable<Employee> {
    return this.http.get<Employee>(`${this.apiUrl}/${id}`);
  }

  createEmployee(employee: EmployeeCreate): Observable<Employee> {
    return this.http.post<Employee>(this.apiUrl, employee);
  }

  updateEmployee(id: number, employee: EmployeeUpdate): Observable<Employee> {
    return this.http.put<Employee>(`${this.apiUrl}/${id}`, employee);
  }

  deleteEmployee(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getDepartments(): Observable<Department[]> {
    return this.http.get<Department[]>(this.departmentsUrl);
  }
}