import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  private baseUrl = 'https://localhost:7001/api/vehicles';

  constructor(private http: HttpClient) {}

  getVehiclesByCustomerId(customerId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/by-customer/${customerId}`).pipe(
      catchError((error) => {
        console.error('Error fetching vehicles by customer:', error);
        return throwError(error);
      })
    );
  }

  getAllVehicles(): Observable<any> {
    return this.http.get(this.baseUrl).pipe(
      catchError((error) => {
        console.error('Error fetching all vehicles:', error);
        return throwError(error);
      })
    );
  }
}
