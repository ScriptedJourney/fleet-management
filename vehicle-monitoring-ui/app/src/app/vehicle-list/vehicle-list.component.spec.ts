import { ComponentFixture, TestBed } from '@angular/core/testing';
import { VehicleListComponent } from './vehicle-list.component';
import { ApiService } from '../services/api.service';
import { of } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('VehicleListComponent', () => {
  let component: VehicleListComponent;
  let fixture: ComponentFixture<VehicleListComponent>;
  let mockApiService: jasmine.SpyObj<ApiService>;

  const mockVehicles = [
    {
      identificationNumber: 'TEST123',
      regNumber: 'ABC123',
      lastPing: new Date().toISOString(),
      isConnected: true,
      customer: {
        id: 1,
        name: 'Test Customer',
        address: 'Test Address'
      }
    }
  ];

  beforeEach(async () => {
    mockApiService = jasmine.createSpyObj('ApiService', ['getAllVehicles']);
    mockApiService.getAllVehicles.and.returnValue(of(mockVehicles));

    await TestBed.configureTestingModule({
      imports: [
        FormsModule,
        HttpClientTestingModule,
        VehicleListComponent
      ],
      providers: [
        { provide: ApiService, useValue: mockApiService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(VehicleListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load vehicles on init', () => {
    expect(mockApiService.getAllVehicles).toHaveBeenCalled();
    expect(component.vehicles).toEqual(mockVehicles);
  });

  it('should filter vehicles based on search text', () => {
    component.filterText = 'TEST123';
    component.onFilterChange();
    expect(component.filteredVehicles.length).toBe(1);
    
    component.filterText = 'NONEXISTENT';
    component.onFilterChange();
    expect(component.filteredVehicles.length).toBe(0);
  });

  it('should start countdown timer', (done) => {
    const initialValue = component.nextRefreshIn;
    setTimeout(() => {
      expect(component.nextRefreshIn).toBeLessThan(initialValue);
      done();
    }, 1100);
  });
});
