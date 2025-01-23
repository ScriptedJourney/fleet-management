import { Component, OnInit, OnDestroy } from '@angular/core';
import { ApiService } from '../services/api.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { interval, Subscription, timer } from 'rxjs';
import { startWith, switchMap, map } from 'rxjs/operators';
import { Vehicle } from './vehicle.model';

@Component({
  selector: 'app-vehicle-list',
  templateUrl: './vehicle-list.component.html',
  styleUrls: ['./vehicle-list.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule]
})
export class VehicleListComponent implements OnInit, OnDestroy {
  vehicles: Vehicle[] = [];
  filteredVehicles: Vehicle[] = [];
  filterText: string = '';
  private updateSubscription?: Subscription;
  private countdownSubscription?: Subscription;
  nextRefreshIn: number = 60;

  constructor(private apiService: ApiService) {}

  ngOnInit(): void {
    this.startUpdateCycle();
    this.startCountdown();
  }

  ngOnDestroy(): void {
    if (this.updateSubscription) {
      this.updateSubscription.unsubscribe();
    }
    if (this.countdownSubscription) {
      this.countdownSubscription.unsubscribe();
    }
  }

  private startUpdateCycle(): void {
    this.updateSubscription = interval(60000).pipe(
      startWith(0),
      switchMap(() => this.apiService.getAllVehicles())
    ).subscribe({
      next: (data: Vehicle[]) => {
        this.vehicles = data;
        this.applyFilter();
        this.nextRefreshIn = 60;
      },
      error: (error) => {
        console.error('Error fetching vehicles:', error);
      }
    });
  }

  private startCountdown(): void {
    this.countdownSubscription = timer(0, 1000).pipe(
      map(() => {
        this.nextRefreshIn = Math.max(0, this.nextRefreshIn - 1);
      })
    ).subscribe();
  }

  onFilterChange(): void {
    this.applyFilter();
  }

  private applyFilter(): void {
    if (!this.filterText) {
      this.filteredVehicles = [...this.vehicles];
      return;
    }
    
    this.filteredVehicles = this.vehicles.filter((vehicle) =>
      this.matchesFilter(vehicle)
    );
  }

  private matchesFilter(vehicle: Vehicle): boolean {
    const filter = this.filterText.toLowerCase();
    return (
      vehicle.customer.name.toLowerCase().includes(filter) ||
      vehicle.customer.address.toLowerCase().includes(filter) ||
      vehicle.identificationNumber.toLowerCase().includes(filter) ||
      vehicle.regNumber.toLowerCase().includes(filter)
    );
  }
}
