import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { ApiService } from './services/api.service';
import { VehicleListComponent } from './vehicle-list/vehicle-list.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    HttpClientModule,
    VehicleListComponent
  ]
})
export class AppComponent implements OnInit {
  data: any;
  title = 'vehicle-monitoring-ui';

  constructor(private apiService: ApiService) {}

  ngOnInit() {
    this.apiService.getAllVehicles().subscribe((response: any) => {
      this.data = response;
      console.log(response);
    });
  }
}
