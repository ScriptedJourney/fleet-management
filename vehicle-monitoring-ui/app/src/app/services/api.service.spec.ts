import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ApiService } from './api.service';

describe('ApiService', () => {
  let service: ApiService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ApiService]
    });
    service = TestBed.inject(ApiService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch vehicles', () => {
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

    service.getAllVehicles().subscribe(vehicles => {
      expect(vehicles).toEqual(mockVehicles);
    });

    const req = httpMock.expectOne('https://localhost:7001/api/vehicles');
    expect(req.request.method).toBe('GET');
    req.flush(mockVehicles);
  });
});
