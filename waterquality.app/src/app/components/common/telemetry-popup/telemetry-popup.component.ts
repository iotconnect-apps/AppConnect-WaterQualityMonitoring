import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-telemetry-popup',
  templateUrl: './telemetry-popup.component.html',
  styleUrls: ['./telemetry-popup.component.css']
})
export class TelemetryPopupComponent implements OnInit {

  lineChart = {
    chartType: 'LineChart',
    dataTable: [
      ['Day', 'Prediction'],
      ['Mon', 700],
      ['Tue', 300],
      ['Web', 400],
      ['Thu', 500],
      ['Fri', 600],
      ['Sat', 800],
      ['Sun', 100]
    ],
    options: {
      title: '',
      height: 250,
      // width:'100%',
      hAxis: {
        title: 'Day',
        gridlines: {
          count: 5
        },
      },
      vAxis: {
        title: 'Gallons',
        gridlines: {
          count: 1
        },
      }
    }
  };

  constructor() { }

  ngOnInit() {
  }

}
